import React, { useEffect, useRef, useState, useMemo, useCallback } from "react";
import cytoscape from "cytoscape";

export default function ShellSortAnimation({ arr, darkMode, theme }) {
  const [step, setStep] = useState(0);
  const [actions, setActions] = useState([]);
  const cyRef = useRef(null);
  const cyInstance = useRef(null);

  // 노드/엣지 생성 (1차원 배열)
  const nodes = useMemo(
    () => arr.map((value, i) => ({ data: { id: `n${i}`, label: value.toString(), value, index: i } })),
    [arr]
  );
  const edges = useMemo(
    () => arr.slice(0, -1).map((_, i) => ({ data: { source: `n${i}`, target: `n${i+1}` } })),
    [arr]
  );

  // 노드 크기 계산
  const getNodeSize = useCallback(() => {
    const width = cyRef.current?.offsetWidth || 400;
    return Math.max(44, Math.min(120, width / (arr.length <= 8 ? arr.length + 1.2 : arr.length + 2.2)));
  }, [arr.length]);

  // 애니메이션 단계 추적
  const [isPlaying, setIsPlaying] = useState(false);

  useEffect(() => {
    const trace = [];
    const a = arr.slice();
    let gap = Math.floor(a.length / 2);
    while (gap > 0) {
      for (let i = gap; i < a.length; i++) {
        let j = i;
        while (j >= gap && a[j - gap] > a[j]) {
          trace.push({ arr: a.slice(), compare: [j - gap, j], swapped: false, gap, done: false });
          [a[j], a[j - gap]] = [a[j - gap], a[j]];
          trace.push({ arr: a.slice(), compare: [j - gap, j], swapped: true, gap, done: false });
          j -= gap;
        }
      }
      gap = Math.floor(gap / 2);
    }
    trace.push({ arr: a.slice(), compare: [], swapped: false, gap: 0, done: true });
    setActions(trace);
    setStep(0);
    setIsPlaying(true);
  }, [arr]);

  // 자동재생 effect
  useEffect(() => {
    let interval = null;
    if (isPlaying && actions.length && step < actions.length - 1) {
      interval = setInterval(() => {
        setStep(s => {
          if (s < actions.length - 1) return s + 1;
          setIsPlaying(false);
          return s;
        });
      }, 700);
    }
    return () => interval && clearInterval(interval);
  }, [isPlaying, step, actions.length]);


  // Cytoscape 인스턴스 준비
  useEffect(() => {
    if (!cyRef.current) return;
    const size = getNodeSize();
    cyInstance.current = cytoscape({
      container: cyRef.current,
      elements: [...nodes, ...edges],
      userPanningEnabled: false,
      userZoomingEnabled: false,
      boxSelectionEnabled: false,
      style: [
        { selector: "node", style: {
            width: size,
            height: size,
            backgroundColor: darkMode ? '#1976d2' : '#90caf9',
            color: darkMode ? '#fff' : '#1565c0',
            label: 'data(label)',
            fontWeight: 700,
            fontSize: size * 0.45,
            textOutlineColor: darkMode ? '#1565c0' : '#fff',
            textOutlineWidth: 2,
            borderWidth: 3,
            borderColor: '#bdbdbd',
          }
        },
        { selector: "edge", style: { lineColor: "#aaa", width: 2 } }
      ],
      layout: {
        name: "grid",
        rows: 1,
        cols: nodes.length,
        fit: false,
        padding: 60
      },
    });
    // 반응형
    const handleResize = () => {
      if (!cyInstance.current) return;
      const newSize = getNodeSize();
      cyInstance.current.style()
        .selector('node')
        .style({ width: newSize, height: newSize, fontSize: newSize * 0.45 })
        .update();
      cyInstance.current.layout({ name: "grid", rows: 1, cols: nodes.length, fit: false, padding: 60 }).run();
      cyInstance.current.resize();
    };
    window.addEventListener('resize', handleResize);
    return () => {
      window.removeEventListener('resize', handleResize);
      cyInstance.current && cyInstance.current.destroy();
    };
  }, [arr, getNodeSize, nodes, edges, darkMode]);

  // step 변경 시 노드/스타일 동기화
  useEffect(() => {
    if (!cyInstance.current || !actions.length) return;
    const curr = actions[step];
    const { arr: currArr, compare, swapped, gap } = curr;
    for (let i = 0; i < currArr.length; i++) {
      const node = cyInstance.current.$(`#n${i}`);
      node.data('label', currArr[i].toString());
      node.style('backgroundColor', compare && compare.includes(i)
        ? (swapped ? '#43a047' : '#ffd54f')
        : (darkMode ? '#1976d2' : '#90caf9'));
      node.style('color', darkMode ? '#fff' : '#1565c0');
      node.style('borderColor', swapped && compare && compare.includes(i) ? '#43a047' : '#bdbdbd');
    }
  }, [actions, step, darkMode]);

  // 단계별 설명 생성
  function getExplanation() {
    if (!actions.length) return null;
    const curr = actions[step];
    if (curr.done) {
      return `<span style="color:${darkMode ? '#ffd54f' : '#1976d2'};font-weight:700">정렬이 완료되었습니다!</span>`;
    }
    const [i, j] = curr.compare;
    const strong = (txt, color) => `<span style="font-weight:700;color:${color||'#1976d2'}">${txt}</span>`;
    if (curr.swapped) {
      return `🔄 gap=${curr.gap}인 구간에서 ${strong(i)}번(${strong(curr.arr[j], '#43a047')})과 ${strong(j)}번(${strong(curr.arr[i], '#43a047')})를 <span style="color:#43a047;font-weight:700">교환</span>합니다.`;
    }
    if (curr.compare.length === 2) {
      return `🔎 gap=${curr.gap}인 구간에서 ${strong(i)}번(${strong(curr.arr[i])})과 ${strong(j)}번(${strong(curr.arr[j])})를 <span style="color:#ffd54f;font-weight:700">비교</span>합니다.`;
    }
    return null;
  }

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
