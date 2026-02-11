"use client";

import dynamic from "next/dynamic";
import { use } from "react";

const Editor = dynamic(() => import("@monaco-editor/react"), { ssr: false });

interface FileData {
  content: string;
  language: string;
  size: number;
}

interface CodeViewerProps {
  filePath: string | null;
}

const fileCache = new Map<string, Promise<FileData>>();

function fetchFileData(path: string): Promise<FileData> {
  const existing = fileCache.get(path);
  if (existing) return existing;

  const promise = fetch(`/api/files?path=${encodeURIComponent(path)}`)
    .then((res) => {
      if (!res.ok) throw new Error("Failed to load file");
      return res.json() as Promise<FileData>;
    });

  fileCache.set(path, promise);
  return promise;
}

function FileContent({ filePath }: { filePath: string }) {
  const fileData = use(fetchFileData(filePath));

  return (
    <div className="h-full flex flex-col bg-[#1e1e1e]">
      <div className="flex items-center border-b border-[#3c3c3c] bg-[#252526] px-4 py-1.5">
        <span className="text-sm text-[#cccccc] truncate">{filePath}</span>
        <span className="ml-auto text-xs text-[#666]">
          {fileData.size < 1024
            ? `${fileData.size} B`
            : `${(fileData.size / 1024).toFixed(1)} KB`}
        </span>
      </div>
      <div className="flex-1">
        <Editor
          height="100%"
          language={fileData.language}
          value={fileData.content}
          theme="vs-dark"
          options={{
            readOnly: true,
            minimap: { enabled: true },
            fontSize: 14,
            lineNumbers: "on",
            scrollBeyondLastLine: false,
            wordWrap: "on",
            automaticLayout: true,
            renderLineHighlight: "all",
            padding: { top: 8 },
          }}
        />
      </div>
    </div>
  );
}

export default function CodeViewer({ filePath }: CodeViewerProps) {
  if (!filePath) {
    return (
      <div className="flex h-full items-center justify-center bg-[#1e1e1e]">
        <div className="text-center">
          <p className="text-2xl text-[#555] mb-2">MyMeetings Modular Monolith</p>
          <p className="text-sm text-[#444]">Select a file from the explorer to view its contents</p>
        </div>
      </div>
    );
  }

  return <FileContent key={filePath} filePath={filePath} />;
}
