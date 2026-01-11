import React, { useEffect, useRef } from "react";
import cytoscape from "cytoscape";


export default function BubbleSortAnimation({ arr, darkMode, theme }) {
  const [step, setStep] = React.useState(0);
  const [actions, setActions] = React.useState([]);

  const nodes = React.useMemo(
    () => arr.map((value, i) => ({ data: { id: `n${i}`, label: value.toString(), value, index: i } })),
    [arr]
  );
  const edges = React.useMemo(
    () => arr.slice(0, -1).map((_, i) => ({ data: { source: `n${i}`, target: `n${i+1}` } })),
    [arr]
  );
  const cyRef = useRef(null);
  const cyInstance = useRef(null);

  // 컨테이너 크기에 따라 노드 크기 계산
  const getNodeSize = React.useCallback(() => {
    const width = cyRef.current?.offsetWidth || 400;
    // 더 큰 노드: 최소 44, 최대 120, 그리고 노드 수가 적으면 더 크게
    return Math.max(44, Math.min(120, width / (arr.length <= 8 ? arr.length + 1.2 : arr.length + 2.2)));
  }, [arr.length]);

  // Cytoscape 인스턴스 준비 및 actions/step 초기화
  React.useEffect(() => {
    if (!cyRef.current) return;
    const size = getNodeSize();
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
        { selector: "node.highlight", style: {
            "background-color": "#ffeb3b",
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
        { selector: "edge", style: { "line-color": "#aaa", width: 2 } }
      ],
      layout: {
        name: "grid",
        rows: 1,
        cols: nodes.length,
        fit: false,
        padding: 60,
        avoidOverlap: true,
        spacingFactor: 0.95,
        animate: false,
        nodeDimensionsIncludeLabels: true
      },
    });
    cyInstance.current.layout({ name: "grid", rows: 1, cols: nodes.length, fit: true, padding: 40 }).run();
    cyInstance.current.resize();
    cyInstance.current.fit();
    cyInstance.current.center();
    const arrState = arr.slice();
    let trace = [];
    for (let i = 0; i < arrState.length - 1; i++) {
      for (let j = 0; j < arrState.length - 1 - i; j++) {
        trace.push({ arr: arrState.slice(), compare: [j, j+1], swapped: false });
        if (arrState[j] > arrState[j + 1]) {
          [arrState[j], arrState[j + 1]] = [arrState[j + 1], arrState[j]];
          trace.push({ arr: arrState.slice(), compare: [j, j+1], swapped: true });
        }
      }
    }
    trace.push({ arr: arrState.slice(), compare: null, swapped: null, done: true });
    setActions(trace);
    setStep(0);
    // ResizeObserver 등록 (생략 가능)
    let resizeObserver = null;
    function handleResize() {
      if (!cyInstance.current) return;
      const newSize = getNodeSize();
      cyInstance.current.style()
        .selector('node')
        .style({
          width: newSize,
          height: newSize,
          'font-size': newSize * 0.45
        })
        .update();
      cyInstance.current.layout({ name: "grid", rows: 1, cols: nodes.length, fit: false, padding: 60 }).run();
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
      cyInstance.current && cyInstance.current.destroy();
    };
  }, [arr, getNodeSize]);

  // step이 바뀔 때 Cytoscape 노드 스타일/데이터 동기화
  React.useEffect(() => {
    if (!cyInstance.current || !actions.length) return;
    const curr = actions[step];
    const { arr: currArr, compare, swapped } = curr;
    for (let i = 0; i < currArr.length; i++) {
      const node = cyInstance.current.$(`#n${i}`);
      node.removeClass('highlight');
      node.removeClass('swapped');
      node.data('label', currArr[i].toString());
    }
    if (compare && compare.length === 2) {
      const [i, j] = compare;
      if (swapped) {
        [i, j].forEach(idx => {
          const node = cyInstance.current.$(`#n${idx}`);
          node.addClass('highlight');
          node.addClass('swapped');
        });
      } else {
        [i, j].forEach(idx => {
          const node = cyInstance.current.$(`#n${idx}`);
          node.addClass('highlight');
        });
      }
    }
    // step이 바뀔 때마다 grid 레이아웃 재적용
    cyInstance.current.layout({ name: "grid", rows: 1, cols: currArr.length, fit: false, padding: 60 }).run();
    cyInstance.current.resize();
  }, [step, actions]);

  // 자동재생: cyInstance와 actions가 모두 준비된 경우에만
  React.useEffect(() => {
    if (!cyInstance.current || actions.length === 0) return;
    if (step < actions.length - 1) {
      const timer = setTimeout(() => setStep(s => s + 1), 1100);
      return () => clearTimeout(timer);
    }
  }, [step, actions]);

  React.useEffect(() => {
    if (!cyRef.current) return;
    const size = getNodeSize();
    // const spacing = getNodeSpacing(); // 미사용 변수 제거
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
        { selector: "node.highlight", style: {
            "background-color": "#ffeb3b",
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
        { selector: "edge", style: { "line-color": "#aaa", width: 2 } }
      ],
      layout: {
        name: "grid",
        rows: 1,
        cols: nodes.length,
        fit: false,
        padding: 60,
        avoidOverlap: true,
        spacingFactor: 0.95,
        animate: false, // 애니메이션 없이 즉시 배치
        nodeDimensionsIncludeLabels: true
      },
    });
    // layout, resize, fit을 순서대로 실행
    cyInstance.current.layout({ name: "grid", rows: 1, cols: nodes.length, fit: true, padding: 40 }).run();
    cyInstance.current.resize();
    cyInstance.current.fit();
    cyInstance.current.center();
    const Y_BASE = cyInstance.current.$(`#n0`).position('y');
    const Y_UP = Y_BASE - 30;
    // 모든 노드 y좌표를 명확히 중앙(Y_BASE)로 맞춤
    for (let i = 0; i < arr.length; i++) {
      const node = cyInstance.current.$(`#n${i}`);
      node.position({ x: node.position('x'), y: Y_BASE });
    }

    // ResizeObserver로 컨테이너 크기 변화 감지
    let resizeObserver = null;
    function handleResize() {
      if (!cyInstance.current) return;
      const newSize = getNodeSize();
      cyInstance.current.style()
        .selector('node')
        .style({
          width: newSize,
          height: newSize,
          'font-size': newSize * 0.45
        })
        .update();
      // 레이아웃/간격도 즉시 반영
      cyInstance.current.layout({ name: "grid", rows: 1, cols: nodes.length, fit: false, padding: 60 }).run();
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
      cyInstance.current && cyInstance.current.destroy();
    };
  }, [arr, getNodeSize]);

  // 단계별 해설 생성
  function getExplanation() {
    if (!actions.length) return null;
    const curr = actions[step];
    if (curr.done) {
      return `<span style="color:${darkMode ? '#ffd54f' : '#1976d2'};font-weight:700">정렬이 완료되었습니다!</span>`;
    }
    const [i, j] = curr.compare;
    const strong = (txt, color) => `<span style="font-weight:700;color:${color||'#1976d2'}">${txt}</span>`;
    if (curr.swapped) {
      return `🔄 ${strong(i)}번(${strong(curr.arr[j], '#43a047')})과 ${strong(j)}번(${strong(curr.arr[i], '#43a047')})를 <span style="color:#43a047;font-weight:700">교환</span>합니다.`;
    }
    return `🔎 ${strong(i)}번(${strong(curr.arr[i])})과 ${strong(j)}번(${strong(curr.arr[j])})를 <span style="color:#1976d2;font-weight:700">비교</span>합니다.`;
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
