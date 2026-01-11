import React, { useEffect, useRef, useState } from "react";
import cytoscape from "cytoscape";

// Heap 트리 구조 시각화 + step-by-step/자동재생 지원
export default function HeapSortAnimation({ arr, darkMode, theme }) {
  const [step, setStep] = useState(0);
  const [actions, setActions] = useState([]);
  const cyRef = useRef(null);
  const cyInstance = useRef(null);
  // 노드 크기 계산 (트리 높이와 노드 수 모두 고려)
  const getNodeSize = React.useCallback(() => {
    if (!cyRef.current) return 56;
    const width = cyRef.current.offsetWidth || 400;
    // 트리의 최대 레벨(깊이)
    const maxLevel = Math.floor(Math.log2(arr.length)) + 1;
    // 한 레벨의 최대 노드 수 (leaf)
    const maxNodesAtLevel = Math.pow(2, maxLevel - 1);
    // 한 레벨당 노드 최소 간격 확보
    const nodeWidth = Math.max(44, Math.min(110, (width - 40) / (maxNodesAtLevel + 0.5)));
    return nodeWidth;
  }, [arr.length]);

  // Heapify 과정 기록
  const heapifyTrace = React.useCallback(function heapifyTrace(a, n, i, trace) {
    let largest = i, l = 2 * i + 1, r = 2 * i + 2;
    if (l < n && a[l] > a[largest]) largest = l;
    if (r < n && a[r] > a[largest]) largest = r;
    trace.push({ arr: a.slice(), compare: [i, l, r], heapSize: n, heapify: i });
    if (largest !== i) {
      trace.push({ arr: a.slice(), swap: [i, largest], heapSize: n });
      [a[i], a[largest]] = [a[largest], a[i]];
      heapifyTrace(a, n, largest, trace);
    }
  }, []);

  // Heapify 및 전체 정렬 단계 기록
  useEffect(() => {
    let trace = [];
    let arrCopy = arr.slice();
    let n = arrCopy.length;
    // Build heap (rearrange array)
    for (let i = Math.floor(n / 2) - 1; i >= 0; i--) {
      heapifyTrace(arrCopy, n, i, trace);
    }
    // One by one extract an element from heap
    for (let i = n - 1; i > 0; i--) {
      trace.push({ arr: arrCopy.slice(), swap: [0, i], heapSize: i });
      [arrCopy[0], arrCopy[i]] = [arrCopy[i], arrCopy[0]];
      heapifyTrace(arrCopy, i, 0, trace);
    }
    trace.push({ arr: arrCopy.slice(), swap: null, heapSize: 0 });
    setActions(trace);
    setStep(0);
  }, [arr, heapifyTrace]);


  useEffect(() => {
    if (step < actions.length - 1) {
      const timer = setTimeout(() => setStep(s => s + 1), 1100);
      return () => clearTimeout(timer);
    }
  }, [step, actions.length]);



  // 단계별 해설 생성 (HTML 스타일, 강조 포함)
  function getExplanation() {
    if (!actions.length) return null;
    const curr = actions[step];
    // 강조 스타일
    const strong = (txt, color) => `<span style="font-weight:700;color:${color||'#1976d2'}">${txt}</span>`;
    if (curr.swap && curr.heapSize !== undefined) {
      if (curr.heapSize < curr.arr.length) {
        return `최대값 ${strong(curr.arr[curr.swap[0]], darkMode ? '#ffe082' : '#b71c1c')} (루트)을 힙의 끝 ${strong(curr.swap[1], '#ffb300')}과 <b>교환</b>합니다.`;
      } else {
        return `힙 구성: 노드 ${strong(curr.swap[0])}와 ${strong(curr.swap[1])}를 <b>교환</b>합니다.`;
      }
    }
    if (curr.compare && curr.heapify !== undefined) {
      const c1 = curr.compare[1] < curr.arr.length ? strong(curr.compare[1]) : '-';
      const c2 = curr.compare[2] < curr.arr.length ? strong(curr.compare[2]) : '-';
      return `heapify: 노드 ${strong(curr.heapify, '#ffb300')}와 자식 ${c1}, ${c2}를 <b>비교</b>합니다.`;
    }
    if (curr.heapSize === 0) {
      return `<span style="color:${darkMode ? '#ffd54f' : '#1976d2'};font-weight:700">정렬이 완료되었습니다!</span>`;
    }
    return null;
  }

  // cytoscape 렌더링 (원래 구조)
  useEffect(() => {
    if (!cyRef.current || !actions.length) return;
    const { arr: curr, swap, compare, heapSize } = actions[step] || {};
    const nodes = curr.map((value, i) => ({ data: { id: `n${i}`, label: value.toString(), value, index: i } }));
    const edges = [];
    for (let i = 0; i < curr.length; i++) {
      let left = 2 * i + 1, right = 2 * i + 2;
      if (left < curr.length) edges.push({ data: { source: `n${i}`, target: `n${left}` } });
      if (right < curr.length) edges.push({ data: { source: `n${i}`, target: `n${right}` } });
    }
    const size = getNodeSize();
    if (!cyInstance.current) {
      cyInstance.current = cytoscape({
        container: cyRef.current,
        elements: [...nodes, ...edges],
        userPanningEnabled: false,
        userZoomingEnabled: false,
        boxSelectionEnabled: false,
        autoungrabify: true,
        autounselectify: true,
        selectionType: 'none',
        style: [
          { selector: "node", style: {
              "background-color": "#2196f3",
              "label": function(ele) {
                return `${ele.data('label')}\n[${ele.data('index')}]`;
              },
              "color": "#fff",
              "font-size": size * 0.45,
              "width": size,
              "height": size,
              "border-width": 5,
              "border-color": "#333",
              "text-valign": "center",
              "text-halign": "center",
              "text-wrap": "wrap",
              "font-family": "Pretendard, Noto Sans KR, Roboto, Segoe UI, Apple SD Gothic Neo, sans-serif",
              "font-weight": 600,
              "text-outline-width": 1.2,
              "text-outline-color": "#1976d2"
            }
          },
          { selector: "node.root", style: {
              "background-color": "#ffb300",
              "border-color": "#f44336",
              "color": "#222",
              "text-outline-color": "#f44336"
            }
          },
          { selector: "node.swapped", style: {
              "background-color": "#4caf50",
              "border-color": "#388e3c",
              "color": "#fff",
              "text-outline-color": "#388e3c"
            }
          },
          { selector: "node.out", style: {
              "background-color": "#bdbdbd",
              "border-color": "#bdbdbd",
              "color": "#fff",
              "text-outline-color": "#bdbdbd"
            }
          },
          { selector: "edge", style: { "line-color": "#aaa", width: 2 } }
        ],
        layout: {
          name: "breadthfirst",
          directed: true,
          padding: 40,
          spacingFactor: 1.1,
          animate: false
        },
      });
    } else {
      cyInstance.current.json({ elements: [...nodes, ...edges] });
      cyInstance.current.style()
        .selector('node')
        .style({ width: size, height: size, 'font-size': size * 0.45 })
        .update();
      cyInstance.current.layout({ name: "breadthfirst", directed: true, padding: 40, spacingFactor: 1.1, animate: false }).run();
    }
    // 스타일링: root, swapped, out
    cyInstance.current.nodes().removeClass('root swapped out');
    if (typeof compare !== 'undefined') {
      cyInstance.current.$(`#n${compare[0]}`).addClass('root');
      if (compare[1] < curr.length) cyInstance.current.$(`#n${compare[1]}`).addClass('swapped');
      if (compare[2] < curr.length) cyInstance.current.$(`#n${compare[2]}`).addClass('swapped');
    }
    if (swap) {
      cyInstance.current.$(`#n${swap[0]}`).addClass('root');
      cyInstance.current.$(`#n${swap[1]}`).addClass('swapped');
    }
    for (let i = curr.length - 1; i >= (heapSize ?? 0); i--) {
      cyInstance.current.$(`#n${i}`).addClass('out');
    }
    cyInstance.current.resize();
    cyInstance.current.center();
    return () => {
      cyInstance.current && cyInstance.current.destroy();
      cyInstance.current = null;
    };
  }, [actions, step, getNodeSize]);

  // 반응형 노드 크기
  useEffect(() => {
    if (!cyRef.current) return;
    let resizeObserver = null;
    function handleResize() {
      if (!cyInstance.current) return;
      const newSize = getNodeSize();
      cyInstance.current.style()
        .selector('node')
        .style({ width: newSize, height: newSize, 'font-size': newSize * 0.45 })
        .update();
      cyInstance.current.resize();
    }
    if (window.ResizeObserver) {
      resizeObserver = new window.ResizeObserver(handleResize);
      resizeObserver.observe(cyRef.current);
    } else {
      window.addEventListener('resize', handleResize);
    }
    return () => {
      if (resizeObserver) resizeObserver.disconnect();
      else window.removeEventListener('resize', handleResize);
    };
  }, [getNodeSize]);

  // 자동재생 타이머 (처음부터 끝까지 자동 진행)
  useEffect(() => {
    if (actions.length > 0 && step < actions.length - 1) {
      const timer = setTimeout(() => setStep(s => s + 1), 700);
      return () => clearTimeout(timer);
    }
  }, [step, actions.length]);

  // arr가 바뀌면 항상 step을 0으로 초기화
  useEffect(() => {
    setStep(0);
  }, [arr, heapifyTrace]);

  return (
    <div style={{ width: "100%", display: "flex", flexDirection: "column", alignItems: "center" }}>
      <div
        ref={cyRef}
        style={{
          width: "100%",
          height: 220,
          minHeight: 160,
          maxWidth: 900,
          margin: "0 auto 18px auto",
          borderRadius: 16,
          boxShadow: darkMode ? "0 2px 12px #000a" : "0 2px 12px #1976d233",
          border: darkMode ? "1.5px solid #374151" : "1.5px solid #e3f2fd",
          background: theme && theme.palette ? theme.palette.background.default : (darkMode ? '#181c21' : '#fafdff')
        }}
      />
      <div style={{
        marginTop: 22,
        fontSize: 17.5,
        fontWeight: 600,
        color: darkMode ? '#222' : '#374151',
        minHeight: 44,
        maxWidth: 600,
        width: '95%',
        textAlign: 'center',
        background: darkMode
          ? 'linear-gradient(90deg,#fffde7cc 0%,#ffe08222 100%)'
          : 'linear-gradient(90deg,#fffde7 0%,#e3f2fd 100%)',
        borderRadius: 15,
        padding: '18px 32px',
        boxShadow: darkMode
          ? '0 6px 24px #ffe08233, 0 1.5px 8px #ffe08222'
          : '0 6px 24px #bdbdbd33, 0 1.5px 8px #1976d211',
        border: `1.5px solid ${darkMode ? '#ffe082' : '#1976d2'}`,
        letterSpacing: 0.1,
        lineHeight: 1.7,
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center',
        gap: 14,
        transition: 'all 0.3s',
        zIndex: 2,
        position: 'relative',
      }}>
        <div style={{display: 'flex', flexDirection: 'column', alignItems: 'center', width: '100%'}}>
          <div style={{display: 'flex', alignItems: 'center', width: '100%', justifyContent: 'center'}}>
            <span style={{fontSize:22,marginRight:10}} role="img" aria-label="heap">🔎</span>
            <span dangerouslySetInnerHTML={{ __html: getExplanation() }} />
          </div>
          <div style={{ textAlign: 'center', fontWeight: 600, color: darkMode ? '#ffd54f' : '#1976d2', fontSize: 15 }}>
            {actions.length ? step + 1 : 0} / {actions.length}
          </div>
        </div>
      </div>
    </div>
  );
}
