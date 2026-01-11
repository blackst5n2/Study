import React, { useEffect, useRef, useState, useMemo, useCallback } from "react";
import cytoscape from "cytoscape";

export default function RadixSortAnimation({ arr, darkMode, theme }) {
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

  // Radix Sort trace 생성 함수
  function getRadixSortTrace(inputArr) {
    const trace = [];
    const arr = inputArr.slice();
    const max = Math.max(...arr);
    let exp = 1;
    while (Math.floor(max / exp) > 0) {
      const output = Array(arr.length);
      const count = Array(10).fill(0);
      // count 단계
      for (let i = 0; i < arr.length; i++) {
        const digit = Math.floor(arr[i] / exp) % 10;
        count[digit]++;
        trace.push({ action: 'count', arr: arr.slice(), count: count.slice(), output: output.slice(), index: i, exp, digit, done: false });
      }
      // 누적합 단계
      for (let i = 1; i < 10; i++) {
        count[i] += count[i - 1];
        trace.push({ action: 'accumulate', arr: arr.slice(), count: count.slice(), output: output.slice(), index: i, exp, digit: null, done: false });
      }
      // output(정렬) 단계
      for (let i = arr.length - 1; i >= 0; i--) {
        const digit = Math.floor(arr[i] / exp) % 10;
        count[digit]--;
        output[count[digit]] = arr[i];
        trace.push({ action: 'output', arr: arr.slice(), count: count.slice(), output: output.slice(), index: i, exp, digit, outIndex: count[digit], done: false });
      }
      // 원본 배열에 복사 단계
      for (let i = 0; i < arr.length; i++) {
        arr[i] = output[i];
        trace.push({ action: 'copy', arr: arr.slice(), count: count.slice(), output: output.slice(), index: i, exp, digit: null, done: false });
      }
      exp *= 10;
    }
    trace.push({ action: 'done', arr: arr.slice(), count: [], output: [], index: -1, exp, digit: null, done: true });
    return trace;
  }
  // 애니메이션 단계 추적
  useEffect(() => {
    if (!arr.length) return;
    setActions(getRadixSortTrace(arr));
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
    const { arr: currArr, action, index, outIndex } = curr;
    for (let i = 0; i < currArr.length; i++) {
      const node = cyInstance.current.$(`#n${i}`);
      let bgColor = darkMode ? '#1976d2' : '#90caf9';
      let borderColor = '#bdbdbd';
      let borderWidth = 3;
      let scale = 1;
      let opacity = 0.7;
      // count 단계: 현재 index 노드 강조
      if (action === 'count' && i === index) {
        bgColor = '#ffd54f';
        borderColor = '#ffd600';
        borderWidth = 6;
        scale = 1.18;
        opacity = 1;
      }
      // output 단계: 현재 output 대상 노드(outIndex) 강조
      else if (action === 'output' && typeof outIndex === 'number' && i === outIndex) {
        bgColor = '#43a047';
        borderColor = '#43a047';
        borderWidth = 6;
        scale = 1.18;
        opacity = 1;
      }
      // copy 단계: 현재 index 노드 강조 (파랑)
      else if (action === 'copy' && i === index) {
        bgColor = darkMode ? '#1976d2' : '#1976d2';
        borderColor = '#1976d2';
        borderWidth = 6;
        scale = 1.18;
        opacity = 1;
      }
      // done 단계: 모두 정상
      else if (action === 'done') {
        opacity = 1;
      }
      node.data('label', currArr[i] !== undefined ? currArr[i].toString() : '');
      node.style({
        backgroundColor: bgColor,
        color: darkMode ? '#fff' : '#1565c0',
        borderColor,
        borderWidth,
        opacity,
        transition: 'all 0.22s',
        transform: `scale(${scale})`,
        zIndex: scale > 1 ? 999 : 1
      });
    }
  }, [actions, step, darkMode]);

  // 단계별 설명 생성
  function getExplanation() {
    if (!actions.length) return null;
    const curr = actions[step];
    const strong = (txt, color) => `<span style="font-weight:700;color:${color||'#1976d2'}">${txt}</span>`;
    if (curr.done) {
      return `<span style="color:${darkMode ? '#ffd54f' : '#1976d2'};font-weight:700">정렬이 완료되었습니다!<br>모든 자릿수에 대해 안정적으로 정렬이 수행되었습니다.<br>최종 결과: <span style='color:#43a047;font-weight:700'>[${curr.arr.join(', ')}]</span></span>`;
    }
    if (curr.action === 'count') {
      return `자릿수 <b>${curr.exp}</b> 기준으로 <b>arr[${curr.index}]</b>의 값 ${strong(curr.arr[curr.index], '#ffd54f')}에서 <b>${curr.digit}</b>번째(0=일의자리) 숫자를 추출해 <span style="color:#ffd54f;font-weight:700">count[${curr.digit}]</span>를 1 증가시킵니다.<br><span style="color:#aaa">(이 단계는 해당 자릿수별 값의 빈도를 세는 과정입니다)</span><br>→ count 배열: <span style="color:#ffd54f">[${curr.count.join(', ')}]</span>`;
    }
    if (curr.action === 'accumulate') {
      return `count 배열을 누적합으로 변환 중입니다.<br>count[${curr.index}]에 count[${curr.index-1}](${curr.count[curr.index-1]})을 더해 누적합을 만듭니다.<br><span style="color:#aaa">(누적합은 현재 자릿수 값이 정렬 후 어디에 위치해야 하는지 알려줍니다)</span><br>→ 누적합 count: <span style="color:#43a047">[${curr.count.join(', ')}]</span>`;
    }
    if (curr.action === 'output') {
      return `누적합 count를 이용해 <b>arr[${curr.index}]</b>의 값 ${strong(curr.arr[curr.index], '#ffd54f')}을(를) <span style="color:#43a047;font-weight:700">output[${curr.outIndex}]</span>에 배치합니다.<br>count[${curr.digit}]을 1 감소시켜 같은 자릿수 값이 올바른 위치에 들어가도록 합니다.<br><span style="color:#aaa">(이 단계는 안정 정렬을 보장하며, output 배열이 점점 완성됩니다)</span><br>→ output 배열: <span style="color:#43a047">[${curr.output.join(', ')}]</span>`;
    }
    if (curr.action === 'copy') {
      return `정렬된 output 배열을 원본 배열에 복사합니다.<br>arr[${curr.index}]에 <span style="color:#1976d2;font-weight:700">${curr.arr[curr.index]}</span>을(를) 대입합니다.<br><span style="color:#aaa">(이 과정을 통해 다음 자릿수 정렬을 위한 준비가 완료됩니다)</span><br>→ arr: <span style="color:#1976d2">[${curr.arr.join(', ')}]</span>`;
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
          margin: "0 auto 10px auto",
          borderRadius: 16,
          boxShadow: darkMode ? "0 2px 12px #000a" : "0 2px 12px #1976d233",
          border: darkMode ? "1.5px solid #374151" : "1.5px solid #e3f2fd",
          background: theme && theme.palette ? theme.palette.background.default : (darkMode ? '#181c21' : '#fafdff')
        }}
      />
      <div style={{
        color: darkMode ? '#222' : '#374151',
        minHeight: 32,
        maxWidth: 600,
        width: '96%',
        margin: '12px auto 0 auto',
        padding: '10px 6px 8px 6px',
        fontSize: 'clamp(13.5px, 1.5vw, 16px)',
        fontWeight: 600,
        textAlign: 'center',
        border: darkMode ? '1.5px solid #ffd54f88' : '1.5px solid #1976d255',
        borderRadius: 16,
        boxShadow: darkMode ? '0 2px 12px #ffd54f22' : '0 2px 12px #1976d211',
        background: darkMode
          ? 'linear-gradient(90deg,#fffde7cc 0%,#ffe08222 100%)'
          : 'linear-gradient(90deg,#e3f2fdcc 0%,#fafdff 100%)',
        letterSpacing: 0.1,
        lineHeight: 1.7,
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center',
        gap: 14,
        transition: 'all 0.2s',
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