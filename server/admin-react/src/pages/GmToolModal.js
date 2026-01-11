import React, { useState } from "react";
import { Modal, Box, Typography, Button, TextField, MenuItem } from "@mui/material";

export default function GmToolModal({ open, user, onClose, onGiveItem, onBanUser }) {
  const [itemId, setItemId] = useState("");
  const [itemAmount, setItemAmount] = useState(1);
  const [banReason, setBanReason] = useState("");

  return (
    <Modal open={open} onClose={onClose}>
      <Box sx={{ bgcolor: "background.paper", p: 4, borderRadius: 2, width: 400, mx: "auto", mt: 10 }}>
        <Typography variant="h6" gutterBottom>
          GM툴 (아이템 지급 / 유저 밴)
        </Typography>
        <Typography>대상 유저: {user?.nickname} ({user?.id})</Typography>
        <Box mt={2}>
          <Typography fontWeight="bold">아이템 지급</Typography>
          <TextField
            label="아이템ID"
            value={itemId}
            onChange={e => setItemId(e.target.value)}
            fullWidth
            margin="dense"
          />
          <TextField
            label="수량"
            type="number"
            value={itemAmount}
            onChange={e => setItemAmount(Number(e.target.value))}
            fullWidth
            margin="dense"
            inputProps={{ min: 1 }}
          />
          <Button
            variant="contained"
            color="primary"
            onClick={() => onGiveItem(user, itemId, itemAmount)}
            sx={{ mt: 1, mb: 2 }}
            disabled={!itemId || itemAmount < 1}
          >
            아이템 지급
          </Button>
        </Box>
        <Box mt={2}>
          <Typography fontWeight="bold">유저 밴</Typography>
          <TextField
            label="밴 사유"
            value={banReason}
            onChange={e => setBanReason(e.target.value)}
            fullWidth
            margin="dense"
          />
          <Button
            variant="contained"
            color="error"
            onClick={() => onBanUser(user, banReason)}
            sx={{ mt: 1 }}
            disabled={!banReason}
          >
            유저 밴
          </Button>
        </Box>
        <Button onClick={onClose} sx={{ mt: 3 }}>닫기</Button>
      </Box>
    </Modal>
  );
}
