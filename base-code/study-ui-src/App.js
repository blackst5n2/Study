import React, { useState } from "react";
import BubbleSortModal from "./Modal/BubbleSortModal";
import SelectionSortModal from "./Modal/SelectionSortModal";
import InsertionSortModal from "./Modal/InsertionSortModal";
import MergeSortModal from "./Modal/MergeSortModal";
import QuickSortModal from "./Modal/QuickSortModal";
import HeapSortModal from "./Modal/HeapSortModal";
import CountingSortModal from "./Modal/CountingSortModal";
import RadixSortModal from "./Modal/RadixSortModal";
import ShellSortModal from "./Modal/ShellSortModal";
import { Container, TextField, InputAdornment, IconButton } from "@mui/material";
import { ThemeProvider, createTheme } from '@mui/material/styles';
import Brightness4Icon from '@mui/icons-material/Brightness4';
import Brightness7Icon from '@mui/icons-material/Brightness7';

function App() {
  const [modalOpen, setModalOpen] = useState({
    bubble: false,
    selection: false,
    insertion: false,
    merge: false,
    quick: false,
    heap: false,
    counting: false,
    radix: false,
    shell: false,
  });
  const [search, setSearch] = useState("");
  const [darkMode, setDarkMode] = useState(false);
  // 예시용 배열
  const arr = Array.from({ length: 8 }, () => Math.floor(Math.random() * 20));

  // 알고리즘 카드 목록 (확장 가능)
  const algorithms = [
    {
      key: 'bubble',
      title: 'Bubble Sort (버블 정렬)',
      desc: '인접한 두 원소를 반복적으로 비교하여\n큰 값을 오른쪽으로 차례차례 이동시키는\n가장 직관적이고 쉬운 정렬 알고리즘입니다.',
      type: '비교 기반 정렬 알고리즘 (Comparison-based Sorting)',
      onClick: () => setModalOpen(prev => ({ ...prev, bubble: true })),
    },
    {
      key: 'selection',
      title: 'Selection Sort (선택 정렬)',
      desc: '남은 값 중 가장 작은(또는 큰) 값을 선택해\n앞쪽부터 순서대로 채우는\n단순하고 직관적인 정렬 알고리즘입니다.',
      type: '비교 기반 정렬 알고리즘 (Comparison-based Sorting)',
      onClick: () => setModalOpen(prev => ({ ...prev, selection: true })),
    },
    {
      key: 'insertion',
      title: 'Insertion Sort (삽입 정렬)',
      desc: '앞에서부터 차례로 값을 꺼내\n이미 정렬된 부분에 알맞은 위치에 삽입하는\n간단하면서도 효율적인 정렬 알고리즘입니다.',
      type: '비교 기반 정렬 알고리즘 (Comparison-based Sorting)',
      onClick: () => setModalOpen(prev => ({ ...prev, insertion: true })),
    },
    {
      key: 'merge',
      title: 'Merge Sort (병합 정렬)',
      desc: '배열을 반씩 쪼개어 재귀적으로 정렬한 뒤\n두 개의 정렬된 배열을 합치는\n분할 정복(Divide & Conquer) 기반의 정렬 알고리즘입니다.',
      type: '분할 정복 정렬 알고리즘 (Divide & Conquer)',
      onClick: () => setModalOpen(prev => ({ ...prev, merge: true })),
    },
    {
      key: 'quick',
      title: 'Quick Sort (퀵 정렬)',
      desc: '기준값(pivot)을 정해 작은 값, 큰 값으로 분할해\n각 부분을 재귀적으로 정렬하는\n매우 빠른 분할 정복 기반의 정렬 알고리즘입니다.',
      type: '분할 정복 정렬 알고리즘 (Divide & Conquer)',
      onClick: () => setModalOpen(prev => ({ ...prev, quick: true })),
    },
    {
      key: 'heap',
      title: 'Heap Sort (힙 정렬)',
      desc: '힙(Heap) 자료구조를 활용해\n최댓값/최솟값을 빠르게 꺼내며 정렬하는\n효율적인 비교 기반 정렬 알고리즘입니다.',
      type: '비교 기반 정렬 알고리즘 (Comparison-based Sorting)',
      onClick: () => setModalOpen(prev => ({ ...prev, heap: true })),
    },
    {
      key: 'counting',
      title: 'Counting Sort (카운팅 정렬)',
      desc: '각 값이 등장한 횟수를 세어\n순서대로 값을 복원하는\n비교 연산이 없는(Non-comparison) 정렬 알고리즘입니다.',
      type: '비교 비기반 정렬 알고리즘 (Non-comparison Sorting)',
      onClick: () => setModalOpen(prev => ({ ...prev, counting: true })),
    },
    {
      key: 'radix',
      title: 'Radix Sort (기수 정렬)',
      desc: '각 자리수(자릿수)별로 여러 번 정렬을 반복하여\n전체를 정렬하는\n비교 연산이 없는(Non-comparison) 정렬 알고리즘입니다.',
      type: '비교 비기반 정렬 알고리즘 (Non-comparison Sorting)',
      onClick: () => setModalOpen(prev => ({ ...prev, radix: true })),
    },
    {
      key: 'shell',
      title: 'Shell Sort (셸 정렬)',
      desc: '간격(gap)을 두고 부분적으로 삽입 정렬을 반복하여\n점점 간격을 줄여가며 전체를 정렬하는\n삽입 정렬의 개선형 알고리즘입니다.',
      type: '비교 기반 정렬 알고리즘 (Comparison-based Sorting)',
      onClick: () => setModalOpen(prev => ({ ...prev, shell: true })),
    },
  ];
  
  // 다크/라이트 테마 정의
  const theme = createTheme({
    palette: {
      mode: darkMode ? 'dark' : 'light',
      primary: {
        main: darkMode ? '#90caf9' : '#1976d2',
      },
      background: {
        default: darkMode ? '#181c21' : '#f8fbff',
        paper: darkMode ? '#23272f' : '#fff',
      },
      text: {
        primary: darkMode ? '#e3f2fd' : '#222',
        secondary: darkMode ? '#90caf9' : '#1976d2',
      },
    },
  });

  return (
    <ThemeProvider theme={theme}>
      <div style={{ flexGrow: 1, background: theme.palette.background.default, minHeight: '100vh', transition: 'background 0.3s' }}>
        <header>
          <div style={{ width: '100%', position: 'sticky', top: 0, zIndex: 100 }}>
            <div style={{ background: theme.palette.primary.main, color: '#fff', padding: '18px 0', boxShadow: `0 2px 12px ${theme.palette.primary.main}33`, textAlign: 'center', fontWeight: 700, fontSize: 24, letterSpacing: 0.5, borderBottom: `2px solid ${darkMode ? '#90caf9' : '#1565c0'}`, display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
              <span style={{ flex: 1 }}>옹</span>
              <IconButton onClick={() => setDarkMode(v => !v)} style={{ color: '#fff', marginRight: 18 }} size="large" aria-label="다크모드 토글">
                {darkMode ? <Brightness7Icon /> : <Brightness4Icon />}
              </IconButton>
            </div>
          </div>
        </header>
        <Container maxWidth={false} style={{ margin: '0 auto', marginTop: 40 }}>
          <div style={{ display: 'flex', justifyContent: 'center', margin: '32px 0' }}>
            <div style={{ width: 360 }}>
              <TextField
                variant="outlined"
                size="medium"
                color="primary"
                fullWidth
                placeholder="검색"
                value={search}
                onChange={e => setSearch(e.target.value)}
                InputProps={{
                  endAdornment: (
                    <InputAdornment position="end">
                      <svg width="22" height="22" fill="none" xmlns="http://www.w3.org/2000/svg"><circle cx="10" cy="10" r="8" stroke={darkMode ? '#90caf9' : '#1976d2'} strokeWidth="2"/><path d="M16 16l4 4" stroke={darkMode ? '#90caf9' : '#1976d2'} strokeWidth="2" strokeLinecap="round"/></svg>
                    </InputAdornment>
                  ),
                  style: {
                    background: darkMode ? 'linear-gradient(90deg, #23272f 0%, #181c21 100%)' : 'linear-gradient(90deg, #e3f2fd 0%, #f8fbff 100%)',
                    borderRadius: 14,
                    fontWeight: 500,
                    color: darkMode ? '#e3f2fd' : '#1976d2',
                    fontSize: 18,
                    paddingLeft: 10,
                    paddingRight: 10,
                  }
                }}
                sx={{
                  borderRadius: 2,
                  boxShadow: darkMode ? '0 4px 24px #222a' : '0 4px 24px #1976d222',
                  '& .MuiOutlinedInput-root': {
                    borderRadius: 2,
                  },
                  '& .MuiOutlinedInput-notchedOutline': {
                    borderColor: darkMode ? '#90caf9' : '#1976d2',
                  },
                  '& .MuiOutlinedInput-root.Mui-focused .MuiOutlinedInput-notchedOutline': {
                    borderColor: darkMode ? '#e3f2fd' : '#1565c0',
                  },
                  '& .MuiInputAdornment-root': {
                    cursor: 'pointer',
                  },
                  '& input::placeholder': {
                    color: darkMode ? '#90caf9' : '#90caf9',
                    opacity: 1,
                    fontWeight: 400,
                    letterSpacing: '0.1em',
                  },
                }}
              />
            </div>
          </div>

          {/* 알고리즘 카드 리스트 */}
          <div
            style={{
              display: 'flex',
              flexWrap: 'wrap',
              gap: '20px',
              justifyContent: 'center',
              alignItems: 'stretch',
              marginTop: 36,
              background: darkMode ? '#181c21' : '#f8fbff',
            }}
          >
            {algorithms.filter(algo => algo.title.toLowerCase().includes(search.toLowerCase()) || algo.desc.toLowerCase().includes(search.toLowerCase())).map(algo => {
              // 유형별 색상 지정
              let mainColor = darkMode ? '#90caf9' : '#1976d2', pillBg = darkMode ? '#23272f' : '#e3f2fd', pillColor = darkMode ? '#90caf9' : '#1976d2', icon = '📊';
              if (algo.type.includes('분할 정복')) { mainColor = darkMode ? '#66bb6a' : '#43a047'; pillBg = darkMode ? '#1b2b1b' : '#e8f5e9'; pillColor = darkMode ? '#81c784' : '#388e3c'; icon = '🪓'; }
              if (algo.type.includes('비기반')) { mainColor = darkMode ? '#ffb74d' : '#ff9800'; pillBg = darkMode ? '#3d2f1b' : '#fff3e0'; pillColor = darkMode ? '#ffd54f' : '#ef6c00'; icon = '🔢'; }
              return (
                <div
                  key={algo.key}
                  role="button"
                  tabIndex={0}
                  onClick={algo.onClick}
                  onKeyDown={e => { if (e.key === 'Enter' || e.key === ' ') algo.onClick(); }}
                  style={{
                    boxShadow: `0 4px 16px 0 ${mainColor}22`,
                    borderRadius: 18,
                    background: theme.palette.background.paper,
                    padding: '24px',
                    border: `2.5px solid ${mainColor}`,
                    cursor: 'pointer',
                    transition: 'box-shadow 0.2s, border-color 0.2s, transform 0.15s',
                    userSelect: 'none',
                    outline: 'none',
                    /* margin removed, grid gap handles spacing */
                    width: 350,
                    height: 220,
                    minWidth: 0,
                    maxWidth: 'none',
                    display: 'flex',
                    flexDirection: 'column',
                    alignItems: 'flex-start',
                    position: 'relative',
                  }}
                  onMouseOver={e => { e.currentTarget.style.boxShadow = `0 8px 32px 0 ${mainColor}55`; e.currentTarget.style.transform = 'translateY(-3px) scale(1.018)'; e.currentTarget.style.borderColor = mainColor; }}
                  onMouseOut={e => { e.currentTarget.style.boxShadow = `0 4px 24px 0 ${mainColor}22`; e.currentTarget.style.transform = 'none'; e.currentTarget.style.borderColor = mainColor; }}
                >
                  {/* 상단 컬러바 및 아이콘 */}
                  <div style={{
                    width: '100%',
                    display: 'flex',
                    alignItems: 'center',
                    gap: 10,
                    marginBottom: 10,
                  }}>
                    <div style={{
                      width: 40, height: 40, borderRadius: '50%', background: pillBg, color: mainColor,
                      display: 'flex', alignItems: 'center', justifyContent: 'center', fontSize: 24, fontWeight: 700,
                      boxShadow: `0 2px 8px 0 ${mainColor}11`, border: `1.5px solid ${mainColor}`
                    }}>{icon}</div>
                    <span style={{
                      fontWeight: 700, fontSize: 18, color: mainColor, letterSpacing: 0.1, flex: 1,
                      whiteSpace: 'nowrap', overflow: 'hidden', textOverflow: 'ellipsis',
                    }}>{algo.title}</span>
                  </div>
                  {/* 설명 */}
                  <div style={{
                    color: theme.palette.text.primary, fontSize: 15.5, fontWeight: 400, marginBottom: 0, lineHeight: 1.7, minHeight: 60,
                    whiteSpace: 'pre-line',
                  }}>{algo.desc}</div>
                  {/* 유형 pill */}
                  <div style={{
                    marginTop: 'auto', alignSelf: 'flex-end',
                    background: pillBg, color: pillColor,
                    fontWeight: 600, fontSize: 13.5,
                    borderRadius: 12, padding: '5px 16px', letterSpacing: 0.1,
                    marginBottom: 2,
                    boxShadow: `0 2px 8px 0 ${mainColor}11`, border: `1px solid ${pillColor}`
                  }}>{algo.type}</div>
                </div>
              );
            })}
          </div>
          <BubbleSortModal open={modalOpen.bubble} onClose={() => setModalOpen(prev => ({ ...prev, bubble: false }))} arr={arr} darkMode={darkMode} theme={theme} />
          <SelectionSortModal open={modalOpen.selection} onClose={() => setModalOpen(prev => ({ ...prev, selection: false }))} arr={arr} darkMode={darkMode} theme={theme} />
          <InsertionSortModal open={modalOpen.insertion} onClose={() => setModalOpen(prev => ({ ...prev, insertion: false }))} arr={arr} darkMode={darkMode} theme={theme} />
          <MergeSortModal open={modalOpen.merge} onClose={() => setModalOpen(prev => ({ ...prev, merge: false }))} arr={arr} darkMode={darkMode} theme={theme} />
          <QuickSortModal open={modalOpen.quick} onClose={() => setModalOpen(prev => ({ ...prev, quick: false }))} arr={arr} darkMode={darkMode} theme={theme} />
          <HeapSortModal open={modalOpen.heap} onClose={() => setModalOpen(prev => ({ ...prev, heap: false }))} arr={arr} darkMode={darkMode} theme={theme} />
          <CountingSortModal open={modalOpen.counting} onClose={() => setModalOpen(prev => ({ ...prev, counting: false }))} arr={arr} darkMode={darkMode} theme={theme} />
          <RadixSortModal open={modalOpen.radix} onClose={() => setModalOpen(prev => ({ ...prev, radix: false }))} arr={arr} darkMode={darkMode} theme={theme} />
          <ShellSortModal open={modalOpen.shell} onClose={() => setModalOpen(prev => ({ ...prev, shell: false }))} arr={arr} darkMode={darkMode} theme={theme} />
        </Container>
      </div>
    </ThemeProvider>
  );
}

export default App;
