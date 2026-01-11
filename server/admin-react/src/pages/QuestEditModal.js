import React, { useState } from "react";
import { Modal, Box, Typography, Button, TextField } from "@mui/material";

export default function QuestEditModal({ open, quest, onClose, onSave }) {
  const [title, setTitle] = useState(quest?.title || "");
  const [description, setDescription] = useState(quest?.description || "");

  React.useEffect(() => {
    setTitle(quest?.title || "");
    setDescription(quest?.description || "");
  }, [quest]);

  return (
    <Modal open={open} onClose={onClose}>
      <Box sx={{ bgcolor: "background.paper", p: 4, borderRadius: 2, width: 400, mx: "auto", mt: 10 }}>
        <Typography variant="h6" gutterBottom>
          퀘스트 정보 수정
        </Typography>
        <TextField
          fullWidth
          label="제목"
          value={title}
          onChange={e => setTitle(e.target.value)}
          margin="normal"
        />
        <TextField
          fullWidth
          label="설명"
          value={description}
          onChange={e => setDescription(e.target.value)}
          margin="normal"
        />
        <Box mt={2}>
          <Button variant="contained" color="primary" onClick={() => onSave({ ...quest, title, description })} sx={{ mr: 1 }}>
            저장
          </Button>
          <Button variant="text" onClick={onClose} sx={{ ml: 1 }}>
            취소
          </Button>
        </Box>
      </Box>
    </Modal>
  );
}
