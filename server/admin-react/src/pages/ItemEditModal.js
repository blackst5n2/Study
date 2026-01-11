import React, { useState } from "react";
import { Modal, Box, Typography, Button, TextField } from "@mui/material";

export default function ItemEditModal({ open, item, onClose, onSave }) {
  const [name, setName] = useState(item?.name || "");
  const [description, setDescription] = useState(item?.description || "");

  React.useEffect(() => {
    setName(item?.name || "");
    setDescription(item?.description || "");
  }, [item]);

  return (
    <Modal open={open} onClose={onClose}>
      <Box sx={{ bgcolor: "background.paper", p: 4, borderRadius: 2, width: 400, mx: "auto", mt: 10 }}>
        <Typography variant="h6" gutterBottom>
          아이템 정보 수정
        </Typography>
        <TextField
          fullWidth
          label="이름"
          value={name}
          onChange={e => setName(e.target.value)}
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
          <Button variant="contained" color="primary" onClick={() => onSave({ ...item, name, description })} sx={{ mr: 1 }}>
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
