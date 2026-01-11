import React from "react";
import { Modal, Box, Typography, Button } from "@mui/material";

export default function QuestDetailModal({ open, quest, onClose, onDelete, onEdit }) {
  if (!quest) return null;
  return (
    <Modal open={open} onClose={onClose}>
      <Box sx={{ bgcolor: "background.paper", p: 4, borderRadius: 2, width: 400, mx: "auto", mt: 10 }}>
        <Typography variant="h6" gutterBottom>
          퀘스트 상세 정보
        </Typography>
        <Typography>ID: {quest.id}</Typography>
        <Typography>제목: {quest.title}</Typography>
        <Typography>설명: {quest.description}</Typography>
        <Box mt={2}>
          <Button variant="contained" color="primary" onClick={() => onEdit(quest)} sx={{ mr: 1 }}>
            수정
          </Button>
          <Button variant="contained" color="error" onClick={() => onDelete(quest)}>
            삭제
          </Button>
          <Button variant="text" onClick={onClose} sx={{ ml: 1 }}>
            닫기
          </Button>
        </Box>
      </Box>
    </Modal>
  );
}
