import React, { useEffect, useRef, useState, useMemo, useCallback } from "react";
import cytoscape from "cytoscape";

export default function QuickSortAnimation({ arr, darkMode, theme }) {
  const [step, setStep] = useState(0);
  const [actions, setActions] = useState([]);
  const cyRef = useRef(null);
  const cyInstance = useRef(null);
  const playInterval = useRef(null);

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

  // 퀵정렬 trace 생성 함수
  function getQuickSortTrace(inputArr) {
    const trace = [];
    const arr = inputArr.slice();
    function quickSort(l, r) {
      if (l >= r) return;
      const pivot = arr[r];
      trace.push({ arr: arr.slice(), action: 'pivot', pivot: r, range: [l, r], compare: [], done: false });
      let i = l;
      for (let j = l; j < r; j++) {
        trace.push({ arr: arr.slice(), action: 'compare', compare: [j, r], pivot: r, range: [l, r], done: false });
        if (arr[j] < pivot) {
          [arr[i], arr[j]] = [arr[j], arr[i]];
          trace.push({ arr: arr.slice(), action: 'swap', compare: [i, j], pivot: r, range: [l, r], done: false });
          i++;
        }
      }
      [arr[i], arr[r]] = [arr[r], arr[i]];
      trace.push({ arr: arr.slice(), action: 'swap', compare: [i, r], pivot: r, range: [l, r], done: false });
      trace.push({ arr: arr.slice(), action: 'partition', pivot: i, range: [l, r], compare: [], done: false });
      quickSort(l, i - 1);
      quickSort(i + 1, r);
    }
    quickSort(0, arr.length - 1);
    trace.push({ arr: arr.slice(), action: 'done', compare: [], done: true });
    return trace;
  }

  // 애니메이션 단계 추적
  useEffect(() => {
    setActions(getQuickSortTrace(arr));
    setStep(0);
  }, [arr]);

  // 자동재생: mount/reset 시 자동 시작, 마지막 단계에서 멈춤
  useEffect(() => {
    if (actions.length > 0 && step < actions.length - 1) {
      playInterval.current = setInterval(() => {
        setStep(prev => {
          if (prev < actions.length - 1) return prev + 1;
          return prev;
        });
      }, 900);
    } else {
      if (playInterval.current) clearInterval(playInterval.current);
    }
    return () => { if (playInterval.current) clearInterval(playInterval.current); };
  }, [actions, step]);

  // 배열이 바뀌면 자동으로 처음부터 재생
  useEffect(() => {
    setStep(0);
  }, [actions]);

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
    const { arr: currArr, compare, swapped, pivot, range, action } = curr;
    let [l, r] = range || [0, currArr.length-1];
    for (let i = 0; i < currArr.length; i++) {
      const node = cyInstance.current.$(`#n${i}`);
      // 분할 구간 시각화: [l~r]만 진하게, 나머지는 흐리게
      const inRange = i >= l && i <= r;
      let opacity = inRange ? 1 : 0.4;
      // 분할 구간 시각화만 적용
      let bgColor = darkMode ? '#1976d2' : '#90caf9';
      let borderColor = '#bdbdbd';
      let borderWidth = 3;
      let label = currArr[i].toString();
      if (compare && compare.includes(i)) {
        bgColor = swapped ? '#43a047' : '#ffd54f';
        borderColor = swapped ? '#43a047' : '#ffd54f';
        borderWidth = swapped ? 6 : 3;
      }
      node.data('label', label);
      node.style({
        backgroundColor: bgColor,
        color: darkMode ? '#fff' : '#1565c0',
        borderColor,
        borderWidth,
        opacity,
        zIndex: 1,
        transition: 'all 0.25s',
      });
    }
  }, [actions, step, darkMode]);

  // 단계별 설명 생성
  function getExplanation() {
    if (!actions.length) return null;
    const curr = actions[step];
    const strong = (txt, color) => `<span style=\"font-weight:700;color:${color||'#1976d2'}\">${txt}</span>`;
    if (curr.done) {
      return `<span style=\"color:${darkMode ? '#ffd54f' : '#1976d2'};font-weight:700\">정렬이 완료되었습니다!</span>`;
    }
    const [i, j] = curr.compare;
    const [l, r] = curr.range || [];
    if (curr.action === 'pivot') {
      return `🎯 <b>[${l}~${r}]</b> 구간: <span style=\"color:#ffb300;font-weight:700\">pivot</span>은 ${strong(curr.pivot)}번(${strong(curr.arr[curr.pivot],'#ffb300')})입니다.`;
    }
    if (curr.action === 'compare') {
      return `🔎 <b>[${l}~${r}]</b> 구간: ${strong(i)}번(${strong(curr.arr[i])})과 pivot ${strong(j)}번(${strong(curr.arr[j],'#ffb300')})을 <span style=\"color:#ffd54f;font-weight:700\">비교</span>합니다.`;
    }
    if (curr.action === 'swap') {
      return `🔄 <b>[${l}~${r}]</b> 구간: ${strong(i)}번(${strong(curr.arr[i],'#43a047')})과 ${strong(j)}번(${strong(curr.arr[j],'#43a047')})를 <span style=\"color:#43a047;font-weight:700\">교환</span>합니다.`;
    }
    if (curr.action === 'partition') {
      return `🟢 <b>[${l}~${r}]</b> 구간 <span style=\"color:#43a047;font-weight:700\">분할 완료</span>! (pivot: ${strong(curr.pivot,'#ffb300')})`;
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
