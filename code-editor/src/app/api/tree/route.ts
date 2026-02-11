import { NextResponse } from "next/server";
import fs from "fs";
import path from "path";

interface TreeNode {
  name: string;
  path: string;
  type: "file" | "directory";
  children?: TreeNode[];
}

const IGNORED = new Set([
  "node_modules",
  ".git",
  ".vs",
  "bin",
  "obj",
  ".nuke-working-directory",
  ".next",
  "code-editor",
]);

const CODEBASE_ROOT = path.resolve(process.cwd(), "..");

function buildTree(dirPath: string, relativePath: string, depth: number): TreeNode[] {
  if (depth > 6) return [];

  let entries: fs.Dirent[];
  try {
    entries = fs.readdirSync(dirPath, { withFileTypes: true });
  } catch {
    return [];
  }

  const nodes: TreeNode[] = [];

  const sorted = entries
    .filter((e) => !IGNORED.has(e.name) && !e.name.startsWith(".ntvs"))
    .sort((a, b) => {
      if (a.isDirectory() && !b.isDirectory()) return -1;
      if (!a.isDirectory() && b.isDirectory()) return 1;
      return a.name.localeCompare(b.name);
    });

  for (const entry of sorted) {
    const entryRelPath = relativePath ? `${relativePath}/${entry.name}` : entry.name;
    const fullPath = path.join(dirPath, entry.name);

    if (entry.isDirectory()) {
      nodes.push({
        name: entry.name,
        path: entryRelPath,
        type: "directory",
        children: buildTree(fullPath, entryRelPath, depth + 1),
      });
    } else {
      nodes.push({
        name: entry.name,
        path: entryRelPath,
        type: "file",
      });
    }
  }

  return nodes;
}

export async function GET() {
  const tree = buildTree(CODEBASE_ROOT, "", 0);
  return NextResponse.json(tree);
}
