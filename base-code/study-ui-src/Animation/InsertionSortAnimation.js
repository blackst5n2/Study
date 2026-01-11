import React, { useEffect, useRef, useState, useMemo, useCallback } from "react";
import cytoscape from "cytoscape";

export default function InsertionSortAnimation({ arr, darkMode, theme }) {
  const [step, setStep] = useState(0);
  const [actions, setActions] = useState([]);
  const playInterval = useRef(null);
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

  // 삽입 정렬 trace 생성 함수
  function getInsertionSortTrace(inputArr) {
    const trace = [];
    const arr = inputArr.slice();
    trace.push({ arr: arr.slice(), compare: [], action: 'start', done: false });
    for (let i = 1; i < arr.length; i++) {
      let key = arr[i];
      let j = i - 1;
      while (j >= 0 && arr[j] > key) {
        trace.push({ arr: arr.slice(), compare: [j, j + 1], action: 'compare', done: false });
        arr[j + 1] = arr[j];
        trace.push({ arr: arr.slice(), compare: [j, j + 1], action: 'overwrite', done: false });
        j--;
      }
      arr[j + 1] = key;
      trace.push({ arr: arr.slice(), compare: [j + 1], action: 'insert', done: false });
    }
    trace.push({ arr: arr.slice(), compare: [], action: 'done', done: true });
    return trace;
  }

  // 애니메이션 단계 추적
  useEffect(() => {
    setActions(getInsertionSortTrace(arr));
    setStep(0);
  }, [arr]);

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
    const { arr: currArr, compare, swapped } = curr;
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
    const strong = (txt, color) => `<span style="font-weight:700;color:${color||'#1976d2'}">${txt}</span>`;
    if (curr.done) {
      return `<span style="color:${darkMode ? '#ffd54f' : '#1976d2'};font-weight:700">정렬이 완료되었습니다!</span><br><span style='color:#43a047;font-weight:600'>모든 원소가 올바른 위치에 삽입되어 배열이 오름차순으로 정렬되었습니다.</span>`;
    }
    if (curr.action === 'start') {
      return `<span style="color:${darkMode ? '#ffd54f' : '#1976d2'};font-weight:700">삽입 정렬을 시작합니다.</span><br><span style='color:#1976d2;font-weight:500'>두 번째 원소부터 시작하여, 각 원소를 앞쪽 정렬된 부분 배열에 <span style='color:#43a047;font-weight:700'>올바른 위치</span>에 삽입합니다.</span>`;
    }
    if (curr.action === 'compare') {
      const [i, j] = curr.compare;
      return `🔎 <b>${strong(i)}번(${strong(curr.arr[i])})</b>과 <b>${strong(j)}번(${strong(curr.arr[j])})</b>를 <span style="color:#ffd54f;font-weight:700">비교</span>합니다.<br><span style='color:#888;font-size:15px'>만약 앞의 값이 더 크면 오른쪽으로 한 칸 이동시켜 빈 공간을 만듭니다.</span>`;
    }
    if (curr.action === 'shift' || curr.action === 'overwrite') {
      const [i, j] = curr.compare;
      return `➡️ <b>${strong(i)}번(${strong(curr.arr[i])})</b>의 값을 <b>${strong(j, '#43a047')}번</b>으로 <span style="color:#43a047;font-weight:700">한 칸 이동</span>합니다.<br><span style='color:#888;font-size:15px'>정렬된 부분에 <span style='color:#43a047;font-weight:700'>삽입할 공간</span>을 만듭니다.</span>`;
    }
    if (curr.action === 'insert') {
      const [k] = curr.compare;
      return `🟢 <b>${strong(k, '#43a047')}번</b> 위치에 <span style="color:#43a047;font-weight:700">키 값</span>을 <b>삽입</b>합니다.<br><span style='color:#888;font-size:15px'>이 위치가 해당 값의 <span style='color:#1976d2;font-weight:700'>정렬상 올바른 자리</span>입니다.</span>`;
    }
    return null;
  }

  // 자동재생 useEffect
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

  return (
    <div style={{ marginTop: 22, width: "100%", display: "flex", flexDirection: "column", alignItems: "center" }}>
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
        fontSize: 'clamp(13px, 2.2vw, 25px)',
        fontWeight: 600,
        color: darkMode ? '#222' : '#374151',
        width: 680,
        height: 110,
        maxWidth: '95vw',
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
        overflow: 'hidden',
        textOverflow: 'ellipsis',
        whiteSpace: 'normal',
        wordBreak: 'break-word',
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
