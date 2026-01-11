import React, { useState, useEffect, useRef } from "react";
import IconButton from "@mui/material/IconButton";
import Menu from "@mui/material/Menu";
import MenuItem from "@mui/material/MenuItem";
import MenuIcon from "@mui/icons-material/Menu";
import LogoutIcon from "@mui/icons-material/Logout";
// 필요한 import만 유지
import useMediaQuery from '@mui/material/useMediaQuery';

import Badge from '@mui/material/Badge';
import Dialog from '@mui/material/Dialog';
import DialogTitle from '@mui/material/DialogTitle';
import DialogActions from '@mui/material/DialogActions';
import Button from '@mui/material/Button';
import CircularProgress from '@mui/material/CircularProgress';

export default function HamburgerMenu({ onLogout, newNotification }) {
  const isMobile = useMediaQuery('(max-width:600px)');
  const [anchorEl, setAnchorEl] = useState(null);
  const [confirmOpen, setConfirmOpen] = useState(false);
  const [loading, setLoading] = useState(false);
  const open = Boolean(anchorEl);
  const buttonRef = useRef();
  const cancelRef = useRef();
  const confirmRef = useRef();
  // 스크롤 잠금 처리
  useEffect(() => {
    if (open) {
      document.body.style.overflow = 'hidden';
    } else {
      document.body.style.overflow = '';
    }
    return () => { document.body.style.overflow = ''; };
  }, [open]);

  const handleClick = (event) => {
    setAnchorEl(event.currentTarget);
  };
  const handleKeyDown = (event) => {
    if ((event.key === 'Enter' || event.key === ' ') && !open) {
      setAnchorEl(event.currentTarget);
    }
  };
  const handleClose = () => {
    setAnchorEl(null);
  };
  const handleLogout = () => {
    setConfirmOpen(true);
  };
  const handleConfirmLogout = async () => {
    setLoading(true);
    try {
      await new Promise((resolve) => setTimeout(resolve, 900)); // 실제 API 연동시 대체
      setLoading(false);
      setConfirmOpen(false);
      handleClose();
      onLogout();
    } catch {
      setLoading(false);
      setConfirmOpen(false);
    }
  };
  const handleCancelLogout = () => {
    setConfirmOpen(false);
  };

  return (
    <>
      <Badge
        color="error"
        variant={newNotification ? "dot" : undefined}
        overlap="circular"
        anchorOrigin={{ vertical: 'top', horizontal: 'right' }}
        sx={{
          mr: 0.5,
          '& .MuiBadge-dot': {
            width: 11,
            height: 11,
            background: 'linear-gradient(135deg, #f87171 60%, #facc15 100%)',
            boxShadow: '0 1px 4px #f8717166',
          },
        }}
      >
        <IconButton
          aria-label="메뉴 열기"
          aria-controls={open ? 'header-menu' : undefined}
          aria-haspopup="true"
          aria-expanded={open ? 'true' : undefined}
          onClick={handleClick}
          onKeyDown={handleKeyDown}
          ref={buttonRef}
          tabIndex={0}
          sx={{
            color: '#fff',
            background: 'linear-gradient(135deg, #6366f1 60%, #60a5fa 100%)',
            border: '2px solid #6366f1',
            borderRadius: '50%',
            padding: isMobile ? '1px' : '2px',
            zIndex: 2000,
            boxShadow: '0 4px 16px #6366f155',
            marginRight: isMobile ? '2px' : '80px',
            width: isMobile ? 36 : 44,
            height: isMobile ? 36 : 44,
            transition: 'transform 0.18s, box-shadow 0.18s',
            outline: 'none',
            '&:focus-visible': {
              outline: '3px solid #6366f1',
              outlineOffset: '2px',
              boxShadow: '0 0 0 4px #a5b4fc88',
            },
            '&:hover': {
              transform: 'scale(1.09)',
              boxShadow: '0 8px 32px #6366f1aa',
              background: 'linear-gradient(135deg, #818cf8 60%, #38bdf8 100%)',
            },
          }}
          size="large"
        >
          <MenuIcon fontSize={isMobile ? 'medium' : 'large'} sx={{ color: '#fff', filter: 'drop-shadow(0 1px 2px #6366f1aa)' }} />
        </IconButton>
      </Badge>
      <Menu
        id="header-menu"
        anchorEl={anchorEl}
        open={open}
        onClose={handleClose}
        anchorOrigin={{ vertical: 'bottom', horizontal: 'right' }}
        transformOrigin={{ vertical: 'top', horizontal: 'right' }}
        PaperProps={{
          sx: {
            borderRadius: 2.5,
            boxShadow: '0 6px 32px #6366f166',
            minWidth: 160,
            mt: 1,
            p: 0.5,
            background: 'linear-gradient(135deg, #f8fafc 70%, #e0e7ff 100%)',
          },
        }}
      >
        <MenuItem
          onClick={handleLogout}
          tabIndex={0}
          sx={{
            borderRadius: 1.5,
            mx: 0.5,
            my: 0.5,
            fontWeight: 500,
            color: '#6366f1',
            transition: 'background 0.15s',
            outline: 'none',
            '&:focus-visible': {
              outline: '2.5px solid #f87171',
              outlineOffset: '2px',
              background: '#fff7',
            },
            '&:hover': {
              background: 'linear-gradient(90deg, #6366f122 40%, #60a5fa22 100%)',
              color: '#2563eb',
            },
          }}
          onKeyDown={e => {
            if (e.key === 'Enter' || e.key === ' ') handleLogout();
          }}
        >
          {loading ? (
            <CircularProgress size={24} sx={{ color: '#f87171', mr: 2 }} />
          ) : (
            <LogoutIcon sx={{ mr: 1, color: '#f87171' }} />
          )}
          로그아웃
        </MenuItem>
      </Menu>

      {/* 로그아웃 확인 다이얼로그 */}
      <Dialog
        open={confirmOpen}
        onClose={loading ? undefined : handleCancelLogout}
        aria-labelledby="logout-confirm-title"
        onKeyDown={(e) => {
          if (loading) return;
          if (e.key === 'Escape') {
            e.stopPropagation();
            handleCancelLogout();
          }
          if (e.key === 'Tab') {
            // Trap focus
            const focusable = [cancelRef.current, confirmRef.current].filter(Boolean);
            if (focusable.length === 0) return;
            const first = focusable[0];
            const last = focusable[focusable.length - 1];
            if (e.shiftKey) {
              if (document.activeElement === first) {
                e.preventDefault();
                last.focus();
              }
            } else {
              if (document.activeElement === last) {
                e.preventDefault();
                first.focus();
              }
            }
          }
          if ((e.key === 'Enter' || e.key === ' ') && document.activeElement === confirmRef.current) {
            e.preventDefault();
            handleConfirmLogout();
          }
        }}
        disableEscapeKeyDown={!!loading}
        disableEnforceFocus={false}
        disableAutoFocus={false}
        disableRestoreFocus={false}
      >
        <DialogTitle id="logout-confirm-title" sx={{ fontWeight: 600, fontSize: '1.13rem', color: '#f87171', textAlign: 'center' }}>
          정말 로그아웃 하시겠습니까?
        </DialogTitle>
        <DialogActions sx={{ justifyContent: 'center', gap: 2, mb: 1 }}>
          <Button
            onClick={handleCancelLogout}
            disabled={loading}
            ref={cancelRef}
            variant="outlined"
            color="primary"
            tabIndex={0}
            autoFocus={!loading}
          >취소</Button>
          <Button
            onClick={handleConfirmLogout}
            disabled={loading}
            ref={confirmRef}
            variant="contained"
            color="error"
            sx={{ color: '#fff' }}
            tabIndex={0}
            autoFocus={!!loading}
          >
            {loading ? <CircularProgress size={20} sx={{ color: '#fff' }} /> : '로그아웃'}
          </Button>
        </DialogActions>
      </Dialog>
    </>
  );
}
