import React, { useState } from "react"; // 필요한 import만 유지
import { Modal, Box, Typography, Button, TextField } from "@mui/material";

export default function UserEditModal({ open, user, onClose, onSave }) {
  const [nickname, setNickname] = useState(user?.nickname || "");
  const [email, setEmail] = useState(user?.email || "");

  React.useEffect(() => {
    setNickname(user?.nickname || "");
    setEmail(user?.email || "");
  }, [user]);

  return (
    <Modal open={open} onClose={onClose}>
      <Box sx={{ bgcolor: "background.paper", p: 4, borderRadius: 2, width: 400, mx: "auto", mt: 10 }}>
        <Typography variant="h6" gutterBottom>
          유저 정보 수정
        </Typography>
        <TextField
          fullWidth
          label="닉네임"
          value={nickname}
          onChange={e => setNickname(e.target.value)}
          margin="normal"
        />
        <TextField
          fullWidth
          label="이메일"
          value={email}
          onChange={e => setEmail(e.target.value)}
          margin="normal"
        />
        <Box mt={2}>
          <Button variant="contained" color="primary" onClick={() => onSave({ ...user, nickname, email })} sx={{ mr: 1 }}>
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
