import { NextRequest, NextResponse } from "next/server";
import fs from "fs";
import path from "path";

const CODEBASE_ROOT = path.resolve(process.cwd(), "..");

const MAX_FILE_SIZE = 1024 * 1024;

function getLanguage(filePath: string): string {
  const ext = path.extname(filePath).toLowerCase();
  const map: Record<string, string> = {
    ".cs": "csharp",
    ".csproj": "xml",
    ".sln": "plaintext",
    ".xml": "xml",
    ".json": "json",
    ".sql": "sql",
    ".ts": "typescript",
    ".tsx": "typescript",
    ".js": "javascript",
    ".jsx": "javascript",
    ".css": "css",
    ".html": "html",
    ".md": "markdown",
    ".yml": "yaml",
    ".yaml": "yaml",
    ".sh": "shell",
    ".ps1": "powershell",
    ".cmd": "bat",
    ".bat": "bat",
    ".props": "xml",
    ".targets": "xml",
    ".config": "xml",
    ".editorconfig": "ini",
    ".gitignore": "plaintext",
    ".dockerignore": "plaintext",
    ".dockerfile": "dockerfile",
    ".txt": "plaintext",
  };
  const name = path.basename(filePath).toLowerCase();
  if (name === "dockerfile") return "dockerfile";
  if (name === "docker-compose.yml") return "yaml";
  return map[ext] || "plaintext";
}

export async function GET(request: NextRequest) {
  const filePath = request.nextUrl.searchParams.get("path");

  if (!filePath) {
    return NextResponse.json({ error: "Missing path parameter" }, { status: 400 });
  }

  const resolved = path.resolve(CODEBASE_ROOT, filePath);
  if (!resolved.startsWith(CODEBASE_ROOT)) {
    return NextResponse.json({ error: "Access denied" }, { status: 403 });
  }

  try {
    const stat = fs.statSync(resolved);
    if (stat.size > MAX_FILE_SIZE) {
      return NextResponse.json({
        content: "File too large to display",
        language: "plaintext",
        size: stat.size,
      });
    }

    const content = fs.readFileSync(resolved, "utf-8");
    const language = getLanguage(resolved);

    return NextResponse.json({ content, language, size: stat.size });
  } catch {
    return NextResponse.json({ error: "File not found" }, { status: 404 });
  }
}
