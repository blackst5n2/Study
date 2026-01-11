import React from "react";
import { Light as SyntaxHighlighter } from "react-syntax-highlighter";
import js from "react-syntax-highlighter/dist/esm/languages/hljs/javascript";
import atomOneDark from "react-syntax-highlighter/dist/esm/styles/hljs/atom-one-dark";

SyntaxHighlighter.registerLanguage("javascript", js);

const quickSortCode = `// 퀵 정렬 예시
function quickSort(arr) {
  if (arr.length < 2) return arr;
  const pivot = arr[0];
  const left = arr.slice(1).filter(x => x < pivot);
  const right = arr.slice(1).filter(x => x >= pivot);
  return [...quickSort(left), pivot, ...quickSort(right)];
}`;

export default function QuickSortCode() {
  return (
    <div style={{
      overflowX: "auto",
      borderRadius: 14,
      border: "2.5px solid #23272f",
      boxShadow: "0 4px 24px 0 #0005",
      background: "#23272f",
      margin: "0 auto",
      maxWidth: '100%',
      width: '100%',
      minWidth: 0,
      boxSizing: 'border-box',
    }}>
      <SyntaxHighlighter
        language="javascript"
        style={atomOneDark}
        showLineNumbers
        customStyle={{
          background: "#282c34",
          borderRadius: 14,
          padding: "min(6vw,26px) min(6vw,26px) min(5vw,20px) min(4vw,18px)",
          fontSize: "clamp(14px,2.2vw,20px)",
          fontFamily: "'Fira Mono', 'Consolas', 'Menlo', monospace",
          color: "#fff",
          lineHeight: 1.7,
          width: '100%',
          minWidth: 0,
          maxWidth: '100%',
          boxSizing: 'border-box',
          boxShadow: "none",
          margin: 0,
        }}
        lineNumberStyle={{ color: '#90caf9', fontSize: 16, marginRight: 12 }}
      >
        {quickSortCode}
      </SyntaxHighlighter>
    </div>
  );
}
