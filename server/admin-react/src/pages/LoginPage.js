import React, { useState, useRef, useEffect } from "react";
import axios from "axios";
import CircularProgress from "@mui/material/CircularProgress";
import styles from "./LoginPage.module.css";

export default function LoginPage() {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);
  const usernameRef = useRef();
  const passwordRef = useRef();
  const loginBtnRef = useRef();

  useEffect(() => {
    if (usernameRef.current) usernameRef.current.focus();
  }, []);

  const handleFieldKeyDown = (e) => {
    if (e.key === "Enter") {
      if (e.target === usernameRef.current) {
        passwordRef.current.focus();
      } else if (e.target === passwordRef.current) {
        loginBtnRef.current.focus();
      } else if (e.target === loginBtnRef.current) {
        handleSubmit(e);
      }
    } else if (e.key === "Tab") {
      // Allow normal tab
    } else if (e.key === "Escape") {
      setError("");
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (loading) return;
    setError("");
    setLoading(true);
    try {
      const res = await axios.post(
        process.env.REACT_APP_API_URL + "/api/auth/admin/login",
        { username, password }
      );
      localStorage.setItem("admin_token", res.data.token);
      window.location.href = "/";
    } catch (err) {
      setError("로그인 실패: 관리자 계정/비밀번호를 확인하세요.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className={styles.root}>
      <div className={styles.card}>
        <div className={styles.title}>관리자 로그인</div>
        {error && <div className={styles.error} aria-live="polite">{error}</div>}
        <input
          type="text"
          placeholder="관리자 계정"
          value={username}
          onChange={(e) => setUsername(e.target.value)}
          className={styles.input}
          ref={usernameRef}
          aria-label="관리자 계정"
          tabIndex={0}
          onKeyDown={handleFieldKeyDown}
          disabled={loading}
        />
        <input
          type="password"
          placeholder="비밀번호"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          className={styles.input}
          ref={passwordRef}
          aria-label="비밀번호"
          tabIndex={0}
          onKeyDown={handleFieldKeyDown}
          disabled={loading}
        />
        <button
          className={styles.button}
          onClick={handleSubmit}
          ref={loginBtnRef}
          aria-label="로그인"
          tabIndex={0}
          onKeyDown={handleFieldKeyDown}
          disabled={loading}
        >
          {loading ? <CircularProgress size={22} sx={{ color: '#fff' }} /> : '로그인'}
        </button>
      </div>
    </div>
  );
}
