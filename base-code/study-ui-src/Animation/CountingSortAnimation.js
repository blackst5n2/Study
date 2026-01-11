import React, { useEffect, useRef, useState } from "react";


export default function CountingSortAnimation({ arr, darkMode, theme }) {
  const [step, setStep] = useState(0);
  const [actions, setActions] = useState([]);
  const timerRef = useRef(null);

  // 애니메이션 단계 추적
  useEffect(() => {
    if (!arr.length) return;
    const trace = [];
    const a = arr.slice();
    const max = Math.max(...a);
    const min = Math.min(...a);
    const count = Array(max - min + 1).fill(0);
    // Count 배열 생성 및 누적
    for (let i = 0; i < a.length; i++) {
      count[a[i] - min]++;
      trace.push({ arr: a.slice(), count: count.slice(), index: i, phase: 'count', done: false });
    }
    for (let i = 1; i < count.length; i++) {
      count[i] += count[i - 1];
      trace.push({ arr: a.slice(), count: count.slice(), index: i, phase: 'accumulate', done: false });
    }
    // 정렬 과정
    const output = Array(a.length);
    for (let i = a.length - 1; i >= 0; i--) {
      output[--count[a[i] - min]] = a[i];
      trace.push({ arr: output.slice(), count: count.slice(), index: i, phase: 'output', done: false });
    }
    trace.push({ arr: output.slice(), count: count.slice(), index: -1, phase: 'done', done: true });
    setActions(trace);
    setStep(0);
  }, [arr]);


  // 자동재생 타이머 (처음부터 끝까지 자동 진행)
  useEffect(() => {
    if (actions.length > 0 && step < actions.length - 1) {
      timerRef.current = setTimeout(() => setStep(s => s + 1), 700);
    } else {
      clearTimeout(timerRef.current);
    }
    return () => clearTimeout(timerRef.current);
  }, [step, actions.length]);

  // arr가 바뀌면 항상 step을 0으로 초기화
  useEffect(() => {
    setStep(0);
  }, [arr]);

  // 단계별 설명 생성
  function getExplanation() {
    if (!actions.length) return null;
    const curr = actions[step];
    if (curr.done) {
      return `<span style=\"color:${darkMode ? '#ffd54f' : '#1976d2'};font-weight:700\">정렬이 완료되었습니다!</span>`;
    }
    if (curr.phase === 'count') {
      return `카운트 배열에 <span style=\"color:#ffd54f;font-weight:700\">${curr.arr[curr.index]}</span>의 개수를 증가시킵니다.`;
    }
    if (curr.phase === 'accumulate') {
      return `카운트 배열 누적합 계산: <span style=\"color:#ffd54f;font-weight:700\">${curr.index}</span>번째 누적`;
    }
    if (curr.phase === 'output') {
      return `출력 배열에 <span style=\"color:#43a047;font-weight:700\">${curr.arr[curr.index]}</span>을(를) 배치합니다.`;
    }
    return null;
  }

  // 배열 시각화 유틸
  const renderArray = (arrData, highlightIdx, highlightColor, label, boxStyle = {}) => (
    <div
      style={{
        display: 'flex',
        flexWrap: 'wrap',
        alignItems: 'center',
        margin: '0 auto ',
        justifyContent: 'center',
        rowGap: 10,
        columnGap: 4,
        width: '100%',
        maxWidth: 800,
      }}
    >
      <span
        style={{
          minWidth: 56,
          textAlign: 'right',
          color: darkMode ? '#ffd54f' : '#1976d2',
          fontWeight: 700,
          fontSize: 16,
          marginRight: 10,
          marginBottom: 6,
        }}
      >
        {label}
      </span>
      {arrData.map((v, i) => (
        <div
          key={i}
          style={{
            width: '10vw',
            maxWidth: 44,
            minWidth: 28,
            height: '10vw',
            maxHeight: 44,
            minHeight: 28,
            margin: '0 2px 4px 2px',
            borderRadius: 8,
            background: i === highlightIdx ? highlightColor : (darkMode ? '#263238' : '#fff'),
            color: i === highlightIdx ? '#fff' : (darkMode ? '#ffd54f' : '#1565c0'),
            border: `2px solid ${i === highlightIdx ? highlightColor : (darkMode ? '#ffd54f' : '#bdbdbd')}`,
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center',
            fontWeight: 700,
            fontSize: 'clamp(13px, 3vw, 18px)',
            boxSizing: 'border-box',
            transition: 'all 0.2s',
            ...boxStyle,
          }}
        >
          {v}
        </div>
      ))}
    </div>
  );

  // 현재 단계 정보
  const curr = actions[step] || {};
  const min = arr.length ? Math.min(...arr) : 0;
  const max = arr.length ? Math.max(...arr) : 0;
  const countArr = curr.count || Array(max - min + 1).fill(0);
  const outputArr = curr.arr || Array(arr.length).fill('');

  // 단계별 강조 인덱스/색상
  let inputHighlight = -1, countHighlight = -1, outputHighlight = -1, countColor = '#ffd54f', outputColor = '#43a047';
  if (curr.phase === 'count') { inputHighlight = curr.index; countHighlight = arr[curr.index] - min; }
  if (curr.phase === 'accumulate') { countHighlight = curr.index; countColor = '#43a047'; }
  if (curr.phase === 'output') { outputHighlight = countArr.findIndex((v, i) => outputArr[i] === arr[curr.index]); countHighlight = arr[curr.index] - min; outputColor = '#43a047'; }

  return (
    <div style={{ width: "100%", display: "flex", flexDirection: "column", alignItems: "center" }}>
      {renderArray(arr, inputHighlight, '#ffd54f', '입력')}
      {renderArray(countArr, countHighlight, countColor, '카운트', { fontSize: 15 })}
      {renderArray(outputArr, outputHighlight, outputColor, '출력')}
      <div style={{
        marginTop: 38, // 충분한 여백 확보
        marginBottom: 18, // 코드 예시 등 하단과도 분리
        fontSize: 18.2,
        fontWeight: 600,
        color: darkMode ? '#222' : '#374151',
        minHeight: 48,
        maxWidth: 600,
        width: '95%',
        textAlign: 'center',
        background: darkMode
          ? 'linear-gradient(90deg,#fffde7cc 0%,#ffe08222 100%)'
          : 'linear-gradient(90deg,#fffde7 0%,#e3f2fd 100%)',
        borderRadius: 15,
        padding: '20px 32px 20px 32px',
        boxShadow: darkMode
          ? '0 6px 24px #ffe08233, 0 1.5px 8px #ffe08222'
          : '0 6px 24px #bdbdbd33, 0 1.5px 8px #1976d211',
        border: `1.5px solid ${darkMode ? '#ffe082' : '#1976d2'}`,
        letterSpacing: 0.1,
        lineHeight: 1.8,
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
