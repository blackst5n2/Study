import React, { useEffect, useRef, useState } from "react";
import { Box, Typography, Paper } from "@mui/material";
import { AdminWsClient } from '../ws/AdminWsClient';

export default function DashboardPage() {
  const [stats, setStats] = useState({
    totalUsers: 0,
    newUsersToday: 0,
    onlineUsers: 0,
    serverStatus: "-"
  });
  const wsClientRef = useRef(null);

  useEffect(() => {
    // WebSocket 연결
    const jwt = localStorage.getItem("admin_token");
    if (!jwt) return;
    const wsUrl = process.env.REACT_APP_WS_URL || "ws://localhost:5000/ws/admin";
    wsClientRef.current = new AdminWsClient(
      wsUrl,
      jwt,
      (msg) => {
        if (msg.type === 'auth_ok') {
          // 인증 성공 처리 (필요시 추가 로직 작성)
        } else if (msg.type === 'auth_fail') {
          // 인증 실패 처리
          setStats({
            totalUsers: 0,
            newUsersToday: 0,
            onlineUsers: 0,
            serverStatus: "인증 실패"
          });
          // 필요시 alert("인증 실패! 다시 로그인하세요");
        } else if (msg.type === 'stats_update') {
          if (msg.stats && typeof msg.stats === 'object') {
            setStats({
              totalUsers: msg.stats.totalUsers ?? 0,
              newUsersToday: msg.stats.newUsersToday ?? 0,
              onlineUsers: msg.stats.onlineUsers ?? 0,
              serverStatus: msg.stats.serverStatus ?? "-"
            });
          }
        }
      },
      () => {
        // 연결 종료 처리: 상태 초기화 등
        setStats({
          totalUsers: 0,
          newUsersToday: 0,
          onlineUsers: 0,
          serverStatus: "연결 종료"
        });
      }
    );
    wsClientRef.current.connect();
    return () => {
      wsClientRef.current?.close && wsClientRef.current.close();
      setStats({
        totalUsers: 0,
        newUsersToday: 0,
        onlineUsers: 0,
        serverStatus: "-"
      });
    };
    // eslint-disable-next-line
  }, []);

  // 아이콘 import
  const statIcons = [
    {
      icon: <span style={{fontSize:36, color:'#6366f1'}} className="material-symbols-rounded">groups</span>,
      label: '총 유저 수',
      value: stats.totalUsers,
      color: 'linear-gradient(120deg,#a1c4fd,#c2e9fb)',
    },
    {
      icon: <span style={{fontSize:36, color:'#22c55e'}} className="material-symbols-rounded">person_add</span>,
      label: '오늘 생성 유저',
      value: stats.newUsersToday,
      color: 'linear-gradient(120deg,#fbc2eb,#a6c1ee)',
    },
    {
      icon: <span style={{fontSize:36, color:'#f59e42'}} className="material-symbols-rounded">visibility</span>,
      label: '접속자 수',
      value: stats.onlineUsers,
      color: 'linear-gradient(120deg,#fbc2eb,#fcb69f)',
    },
    {
      icon: <span style={{fontSize:36, color: stats.serverStatus === '정상' ? '#0fa958' : '#e63946'}} className="material-symbols-rounded">dns</span>,
      label: '서버 상태',
      value: stats.serverStatus,
      color: stats.serverStatus === '정상'
        ? 'linear-gradient(120deg,#b7ffb7,#a1ffce)'
        : 'linear-gradient(120deg,#ffdde1,#ee9ca7)',
    },
  ];

  return (
    <Box
      sx={{
        minHeight: '100vh',
        background: 'linear-gradient(120deg,#f8fafc 0%,#e0e7ff 100%)',
        px: { xs: 1.5, md: 4 },
        py: { xs: 2, md: 5 },
      }}
    >
      <Typography
        variant="h3"
        fontWeight={900}
        color="#4338ca"
        sx={{ letterSpacing: -1.5, mb: 1, textShadow: '0 2px 12px #fff8, 0 1px 0 #fff8' }}
      >
        👋 관리자님, 환영합니다!
      </Typography>
      <Typography
        variant="subtitle1"
        sx={{ mb: 4, color: '#64748b', fontWeight: 500 }}
      >
        실시간 서버/유저 현황을 한눈에 확인하세요.
      </Typography>
      <Box
        sx={{
          display: 'flex',
          flexWrap: 'wrap',
          gap: { xs: 2, sm: 3, md: 4 },
          justifyContent: 'center',
          alignItems: 'stretch',
          mt: 2,
          mb: 2,
        }}
      >
        {statIcons.map((stat, idx) => (
          <Paper
            key={stat.label}
            elevation={0}
            sx={{
              borderRadius: 5,
              p: 3,
              minHeight: 130,
              height: 130,
              width: 200,
              m: 0,
              display: 'flex',
              flexDirection: 'column',
              alignItems: 'center',
              justifyContent: 'center',
              background: stat.color,
              boxShadow: '0 8px 32px rgba(100,150,255,0.11)',
              backdropFilter: 'blur(9px) saturate(1.25)',
              border: '1.5px solid #e0e7ff',
              transition: 'transform .18s',
              position: 'relative',
              overflow: 'hidden',
              '&:hover': {
                transform: 'translateY(-7px) scale(1.04)',
                boxShadow: '0 16px 48px #6366f1aa',
              },
              animation: `fadeInUp 0.7s ${0.1 + idx * 0.07}s both`,
            }}
          >
            <Box sx={{ mb: 1 }}>{stat.icon}</Box>
            <Typography variant="subtitle1" sx={{ color: '#374151', fontWeight: 700, mb: 0.5 }}>{stat.label}</Typography>
            <Typography
              variant="h4"
              sx={{
                fontWeight: 900,
                color: idx === 3 && stat.value !== '정상' ? '#e63946' : '#22223b',
                textShadow: '0 2px 8px #fff4',
                letterSpacing: -2,
                fontSize: { xs: '2rem', md: '2.6rem' },
                mt: 0.5,
              }}
            >
              {stat.value}
            </Typography>
            </Paper>
          ))}
        </Box>
        <Box sx={{ mt: 6, p: 0, textAlign: 'center', color: '#64748b', fontWeight: 500 }}>
          <Typography variant="h6" sx={{ mb: 1, fontWeight: 700, color: '#6366f1' }}>
            📊 대시보드 요약
          </Typography>
          <Typography variant="body1">
            전체 유저 {stats.totalUsers}명, 오늘 가입 {stats.newUsersToday}명, 현재 접속 {stats.onlineUsers}명, 서버 상태: <b style={{ color: stats.serverStatus === '정상' ? '#0fa958' : '#e63946' }}>{stats.serverStatus}</b>
          </Typography>
        </Box>
      </Box>
  );
}

