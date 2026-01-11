import React from "react";
import { Modal, Box, Typography, Button } from "@mui/material";

export default function ItemDetailModal({ open, item, onClose, onDelete, onEdit }) {
  if (!item) return null;
  return (
    <Modal open={open} onClose={onClose}>
      <Box sx={{ bgcolor: "background.paper", p: 4, borderRadius: 2, width: 400, mx: "auto", mt: 10 }}>
        <Typography variant="h6" gutterBottom>
          아이템 상세 정보
        </Typography>
        <Typography>ID: {item.id}</Typography>
        <Typography>이름: {item.name}</Typography>
        <Typography>설명: {item.description}</Typography>
        <Box mt={2}>
          <Button variant="contained" color="primary" onClick={() => onEdit(item)} sx={{ mr: 1 }}>
            수정
          </Button>
          <Button variant="contained" color="error" onClick={() => onDelete(item)}>
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
