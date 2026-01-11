import React, { useState } from "react";
import { Modal, Box, Typography, Button } from "@mui/material";
import GmToolModal from "./GmToolModal";

export default function UserDetailModal({ open, user, onClose, onDelete, onEdit }) {
  const [showGm, setShowGm] = useState(false);
  const handleGiveItem = async (user, itemId, itemAmount) => {
    // TODO: 실제 아이템 지급 API 연동
    alert(`${user.nickname}에게 ${itemId} (${itemAmount}) 지급!`);
    setShowGm(false);
  };
  const handleBanUser = async (user, reason) => {
    // TODO: 실제 유저 밴 API 연동
    alert(`${user.nickname} 밴! 사유: ${reason}`);
    setShowGm(false);
  };
  if (!user) return null;
  return (
    <>
      <Modal open={open} onClose={onClose}>
        <Box sx={{ bgcolor: "background.paper", p: 4, borderRadius: 2, width: 400, mx: "auto", mt: 10 }}>
          <Typography variant="h6" gutterBottom>
            유저 상세 정보
          </Typography>
          <Typography>ID: {user.id}</Typography>
          <Typography>닉네임: {user.nickname}</Typography>
          <Typography>이메일: {user.email}</Typography>
          <Box mt={2}>
            <Button variant="contained" color="primary" onClick={() => onEdit(user)} sx={{ mr: 1 }}>
              수정
            </Button>
            <Button variant="contained" color="error" onClick={() => onDelete(user)}>
              삭제
            </Button>
            <Button variant="outlined" color="secondary" onClick={() => setShowGm(true)} sx={{ ml: 1 }}>
              GM툴
            </Button>
            <Button variant="text" onClick={onClose} sx={{ ml: 1 }}>
              닫기
            </Button>
          </Box>
        </Box>
      </Modal>
      <GmToolModal
        open={showGm}
        user={user}
        onClose={() => setShowGm(false)}
        onGiveItem={handleGiveItem}
        onBanUser={handleBanUser}
      />
    </>
  );
}
