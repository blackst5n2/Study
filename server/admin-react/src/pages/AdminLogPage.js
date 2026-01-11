import React, { useEffect, useState } from "react";
import { Box, Typography, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Paper } from "@mui/material";
import axios from "axios";

export default function AdminLogPage() {
  const [logs, setLogs] = useState([]);
  useEffect(() => {
    // 실제 API 연동 시 아래 부분을 교체
    // axios.get(process.env.REACT_APP_API_URL + "/api/admin/logs", ...)
    setLogs([
      { id: 1, admin: "admin", action: "로그인", target: "", detail: "성공", time: "2025-04-21 21:00" },
      { id: 2, admin: "admin", action: "유저 수정", target: "user123", detail: "닉네임 변경", time: "2025-04-21 21:05" },
      { id: 3, admin: "admin", action: "아이템 지급", target: "user234", detail: "item001 x10", time: "2025-04-21 21:10" },
      { id: 4, admin: "admin", action: "유저 밴", target: "user345", detail: "비매너", time: "2025-04-21 21:15" },
    ]);
  }, []);
  return (
    <Box p={3}>
      <Typography variant="h4" gutterBottom>관리자 활동 로그</Typography>
      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>시간</TableCell>
              <TableCell>관리자</TableCell>
              <TableCell>행동</TableCell>
              <TableCell>대상</TableCell>
              <TableCell>상세</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {logs.map((log) => (
              <TableRow key={log.id}>
                <TableCell>{log.time}</TableCell>
                <TableCell>{log.admin}</TableCell>
                <TableCell>{log.action}</TableCell>
                <TableCell>{log.target}</TableCell>
                <TableCell>{log.detail}</TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
    </Box>
  );
}
