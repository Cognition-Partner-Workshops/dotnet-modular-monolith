"use client";

import { useState } from "react";

interface TreeNode {
  name: string;
  path: string;
  type: "file" | "directory";
  children?: TreeNode[];
}

interface FileTreeProps {
  tree: TreeNode[];
  selectedPath: string | null;
  onSelectFile: (path: string) => void;
}

function FileIcon({ type, expanded }: { type: "file" | "directory"; expanded?: boolean }) {
  if (type === "directory") {
    return <span className="mr-1.5 text-sm">{expanded ? "\u{1F4C2}" : "\u{1F4C1}"}</span>;
  }
  return <span className="mr-1.5 text-sm">{"\u{1F4C4}"}</span>;
}

function TreeItem({
  node,
  depth,
  selectedPath,
  onSelectFile,
}: {
  node: TreeNode;
  depth: number;
  selectedPath: string | null;
  onSelectFile: (path: string) => void;
}) {
  const [expanded, setExpanded] = useState(depth < 1);
  const isSelected = selectedPath === node.path;

  const handleClick = () => {
    if (node.type === "directory") {
      setExpanded(!expanded);
    } else {
      onSelectFile(node.path);
    }
  };

  return (
    <div>
      <button
        onClick={handleClick}
        className={`flex w-full items-center py-0.5 px-2 text-left text-sm hover:bg-[#2a2d2e] transition-colors ${
          isSelected ? "bg-[#37373d] text-white" : "text-[#cccccc]"
        }`}
        style={{ paddingLeft: `${depth * 16 + 8}px` }}
      >
        {node.type === "directory" && (
          <span className="mr-1 text-xs text-[#888]">{expanded ? "\u25BC" : "\u25B6"}</span>
        )}
        <FileIcon type={node.type} expanded={expanded} />
        <span className="truncate">{node.name}</span>
      </button>
      {node.type === "directory" && expanded && node.children && (
        <div>
          {node.children.map((child) => (
            <TreeItem
              key={child.path}
              node={child}
              depth={depth + 1}
              selectedPath={selectedPath}
              onSelectFile={onSelectFile}
            />
          ))}
        </div>
      )}
    </div>
  );
}

export default function FileTree({ tree, selectedPath, onSelectFile }: FileTreeProps) {
  return (
    <div className="h-full overflow-y-auto bg-[#252526] select-none">
      <div className="sticky top-0 z-10 bg-[#252526] border-b border-[#3c3c3c] px-4 py-2">
        <h2 className="text-xs font-semibold uppercase tracking-wider text-[#bbbbbb]">
          Explorer
        </h2>
      </div>
      <div className="py-1">
        {tree.map((node) => (
          <TreeItem
            key={node.path}
            node={node}
            depth={0}
            selectedPath={selectedPath}
            onSelectFile={onSelectFile}
          />
        ))}
      </div>
    </div>
  );
}
