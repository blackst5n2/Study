import React, { useEffect, useRef, useState, useMemo, useCallback } from "react";
import cytoscape from "cytoscape";

export default function MergeSortAnimation({ arr, darkMode, theme }) {
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

  // 병합 정렬 trace 생성 함수
  function getMergeSortTrace(inputArr) {
    const trace = [];
    const arr = inputArr.slice();
    function mergeSort(l, r) {
      if (l >= r) return;
      trace.push({ arr: arr.slice(), compare: [], action: 'divide', range: [l, r], done: false });
      const m = Math.floor((l + r) / 2);
      mergeSort(l, m);
      mergeSort(m + 1, r);
      let temp = [], tempOrigin = [], i = l, j = m + 1;
      while (i <= m && j <= r) {
        trace.push({ arr: arr.slice(), compare: [i, j], action: 'compare', range: [l, r], temp: temp.slice(), tempOrigin: tempOrigin.slice(), done: false });
        if (arr[i] <= arr[j]) {
          temp.push(arr[i]);
          tempOrigin.push(i);
          i++;
        } else {
          temp.push(arr[j]);
          tempOrigin.push(j);
          j++;
        }
      }
      while (i <= m) { temp.push(arr[i]); tempOrigin.push(i); i++; }
      while (j <= r) { temp.push(arr[j]); tempOrigin.push(j); j++; }
      for (let k = l; k <= r; k++) {
        arr[k] = temp[k - l];
        trace.push({ arr: arr.slice(), compare: [k], action: 'copy', range: [l, r], temp: temp.slice(), tempOrigin: tempOrigin.slice(), done: false });
      }
      trace.push({ arr: arr.slice(), compare: [l, r], action: 'merge', range: [l, r], temp: temp.slice(), tempOrigin: tempOrigin.slice(), done: false });
    }
    mergeSort(0, arr.length - 1);
    trace.push({ arr: arr.slice(), compare: [], action: 'done', range: [0, arr.length - 1], done: true });
    return trace;
  }

  // 애니메이션 단계 추적
  useEffect(() => {
    setActions(getMergeSortTrace(arr));
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
    const { arr: currArr, compare, merged, tempOrigin } = curr;
    // 노드와 temp 연동: temp에 방금 추가된 원본 인덱스 강조
    let highlightIdx = null;
    if (tempOrigin && tempOrigin.length > 0) {
      highlightIdx = tempOrigin[tempOrigin.length - 1];
    }
    for (let i = 0; i < currArr.length; i++) {
      const node = cyInstance.current.$(`#n${i}`);
      node.data('label', currArr[i].toString());
      let highlight = false;
      if (highlightIdx !== null && i === highlightIdx) highlight = true;
      node.style('backgroundColor', compare && compare.includes(i)
        ? (merged ? '#43a047' : '#ffd54f')
        : (darkMode ? '#1976d2' : '#90caf9'));
      node.style('color', darkMode ? '#fff' : '#1565c0');
      node.style('borderColor', highlight ? '#ff7043' : (merged && compare && compare.includes(i) ? '#43a047' : '#bdbdbd'));
      node.style('borderWidth', highlight ? 7 : 3);
      node.style('zIndex', highlight ? 999 : 1);
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
    if (curr.action === 'divide') {
      return `🪓 <b>[${l}~${r}]</b> 구간을 <span style=\"color:#1976d2;font-weight:700\">분할</span>합니다.`;
    }
    if (curr.action === 'compare') {
      return `🔎 <b>[${l}~${r}]</b> 구간: ${strong(i)}번(${strong(curr.arr[i])})과 ${strong(j)}번(${strong(curr.arr[j])})를 <span style=\"color:#ffd54f;font-weight:700\">비교</span>합니다.`;
    }
    if (curr.action === 'copy') {
      return `📥 <b>[${l}~${r}]</b> 구간: 병합 결과를 ${strong(i, '#43a047')}번 위치에 <span style=\"color:#43a047;font-weight:700\">복사</span>합니다.`;
    }
    if (curr.action === 'merge') {
      return `🟢 <b>[${l}~${r}]</b> 구간 <span style=\"color:#43a047;font-weight:700\">병합 완료</span>!`;
    }
    return null;
  }

  return (
    <div style={{ width: "100%", display: "flex", flexDirection: "column", alignItems: "center" }}>
      <div
        style={{
          width: "100%",
          height: 220,
          minHeight: 160,
          maxWidth: 900,
          margin: "0 auto 18px auto",
          borderRadius: 16,
          boxShadow: darkMode ? "0 2px 12px #000a" : "0 2px 12px #1976d233",
          border: darkMode ? "1.5px solid #374151" : "1.5px solid #e3f2fd",
          background: theme && theme.palette ? theme.palette.background.default : (darkMode ? '#181c21' : '#fafdff'),
          position: 'relative',
          display: 'flex',
          flexDirection: 'column',
          justifyContent: 'flex-end',
        }}
      >
        <div
          ref={cyRef}
          style={{
            width: '100%',
            height: 180,
            minHeight: 120,
            maxWidth: 900,
            borderRadius: 16,
            background: 'transparent',
            position: 'relative',
            zIndex: 1
          }}
        />
        {/* 임시배열(temp) 세련된 시각화: 그래프 박스 내부 하단 */}
      {actions.length > 0 && actions[step] && actions[step].temp && actions[step].tempOrigin && (
        <div style={{
          width: '92%',
          minHeight: 44,
          background: darkMode ? 'linear-gradient(90deg,#232b36ee 0%,#181c21ee 100%)' : 'linear-gradient(90deg,#fafdffee 0%,#e3f2fdcc 100%)',
          borderRadius: 16,
          boxShadow: darkMode ? '0 2px 12px #ffd54f22' : '0 2px 12px #1976d222',
          border: darkMode ? '1.5px solid #ffd54f99' : '1.5px solid #1976d299',
          position: 'absolute',
          left: '4%',
          bottom: 10,
          zIndex: 3,
          display: 'flex',
          alignItems: 'center',
          gap: 15,
          padding: '7px 18px',
          pointerEvents: 'auto',
          transition: 'all 0.25s',
        }}>
          <span style={{ fontWeight: 800, color: darkMode ? '#ffd54f' : '#1976d2', fontSize: 17, letterSpacing: 1, display:'flex', alignItems:'center', gap:7 }}>
            <span style={{fontSize:19,verticalAlign:'middle'}}>🗂️</span> temp
          </span>
          {actions[step].temp.length === 0 ? (
            <span style={{ color: '#bdbdbd', fontWeight: 600, fontSize: 15 }}>비어있음</span>
          ) : (
            actions[step].temp.map((v, idx) => {
              const origin = actions[step].tempOrigin[idx];
              // 방금 추가된 값이면 강조
              const isLatest = idx === actions[step].temp.length - 1;
              return (
                <span key={idx} style={{
                  background: isLatest ? (darkMode ? '#ff7043' : '#43a047') : (darkMode ? '#ffd54f' : '#1976d2'),
                  color: isLatest ? '#fff' : (darkMode ? '#181c21' : '#fff'),
                  borderRadius: 999,
                  padding: '5px 18px',
                  margin: '0 2px',
                  fontWeight: 800,
                  fontSize: 17,
                  border: isLatest
                    ? (darkMode ? '2.5px solid #ff7043' : '2.5px solid #43a047')
                    : (darkMode ? '2px solid #fffde7' : '2px solid #e3f2fd'),
                  boxShadow: isLatest
                    ? (darkMode ? '0 2px 12px #ff704355' : '0 2px 12px #43a04755')
                    : (darkMode ? '0 2px 8px #ffd54f33' : '0 2px 8px #1976d233'),
                  letterSpacing: 1,
                  transition: 'all 0.22s',
                  userSelect: 'none',
                  display: 'inline-block',
                  textAlign: 'center',
                  lineHeight: 1.6,
                  outline: 'none',
                  filter: isLatest ? 'brightness(1.08)' : 'brightness(0.97)',
                }}>{v} <span style={{fontWeight:600, fontSize:13, opacity:0.8}}>({origin})</span></span>
              );
            })
          )}
        </div>
      )}
      </div>
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

