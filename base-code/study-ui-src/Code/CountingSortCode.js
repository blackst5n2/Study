import React from "react";
import { Light as SyntaxHighlighter } from "react-syntax-highlighter";
import js from "react-syntax-highlighter/dist/esm/languages/hljs/javascript";
import atomOneDark from "react-syntax-highlighter/dist/esm/styles/hljs/atom-one-dark";

SyntaxHighlighter.registerLanguage("javascript", js);

const countingSortCode = `// 카운팅 정렬 예시
function countingSort(arr, maxValue) {
  let count = new Array(maxValue + 1).fill(0);
  let output = new Array(arr.length);
  for (let i = 0; i < arr.length; i++) count[arr[i]]++;
  for (let i = 1; i < count.length; i++) count[i] += count[i - 1];
  for (let i = arr.length - 1; i >= 0; i--) {
    output[count[arr[i]] - 1] = arr[i];
    count[arr[i]]--;
  }
  return output;
}`;

export default function CountingSortCode() {
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
        {countingSortCode}
      </SyntaxHighlighter>
    </div>
  );
}
