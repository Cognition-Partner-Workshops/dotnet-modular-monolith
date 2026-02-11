"use client";

import { useState, useEffect } from "react";
import FileTree from "@/components/FileTree";
import CodeViewer from "@/components/CodeViewer";

interface TreeNode {
  name: string;
  path: string;
  type: "file" | "directory";
  children?: TreeNode[];
}

export default function Home() {
  const [tree, setTree] = useState<TreeNode[]>([]);
  const [selectedFile, setSelectedFile] = useState<string | null>(null);
  const [sidebarWidth, setSidebarWidth] = useState(280);
  const [isResizing, setIsResizing] = useState(false);
  const [treeLoading, setTreeLoading] = useState(true);

  useEffect(() => {
    fetch("/api/tree")
      .then((res) => res.json())
      .then((data: TreeNode[]) => {
        setTree(data);
        setTreeLoading(false);
      })
      .catch(() => setTreeLoading(false));
  }, []);

  useEffect(() => {
    const handleMouseMove = (e: MouseEvent) => {
      if (!isResizing) return;
      const newWidth = Math.max(200, Math.min(600, e.clientX));
      setSidebarWidth(newWidth);
    };

    const handleMouseUp = () => {
      setIsResizing(false);
    };

    if (isResizing) {
      document.addEventListener("mousemove", handleMouseMove);
      document.addEventListener("mouseup", handleMouseUp);
    }

    return () => {
      document.removeEventListener("mousemove", handleMouseMove);
      document.removeEventListener("mouseup", handleMouseUp);
    };
  }, [isResizing]);

  return (
    <div className="flex h-screen overflow-hidden bg-[#1e1e1e]">
      <div
        className="flex-shrink-0 h-full overflow-hidden"
        style={{ width: `${sidebarWidth}px` }}
      >
        {treeLoading ? (
          <div className="flex h-full items-center justify-center bg-[#252526]">
            <div className="flex items-center gap-2 text-[#888] text-sm">
              <svg className="animate-spin h-4 w-4" viewBox="0 0 24 24">
                <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4" fill="none" />
                <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4z" />
              </svg>
              Loading explorer...
            </div>
          </div>
        ) : (
          <FileTree
            tree={tree}
            selectedPath={selectedFile}
            onSelectFile={setSelectedFile}
          />
        )}
      </div>

      <div
        className="w-1 flex-shrink-0 cursor-col-resize bg-[#3c3c3c] hover:bg-[#007acc] transition-colors"
        onMouseDown={() => setIsResizing(true)}
      />

      <div className="flex-1 h-full overflow-hidden">
        <CodeViewer filePath={selectedFile} />
      </div>
    </div>
  );
}
