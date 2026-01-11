import React, { useState, useEffect } from "react";
import { Link, useNavigate, Outlet } from "react-router-dom";
import styles from "./Layout.module.css";
import Sidebar from "./Sidebar";
import HamburgerMenu from "./HamburgerMenu";

export default function Layout() {
  const [menuOpen, setMenuOpen] = useState(false);
  useEffect(() => {
    if (!menuOpen) return;
    const onKey = (e) => {
      if (e.key === "Escape") setMenuOpen(false);
    };
    window.addEventListener("keydown", onKey);
    return () => window.removeEventListener("keydown", onKey);
  }, [menuOpen]);
  const navigate = useNavigate();
  const handleLogout = () => {
    localStorage.removeItem("admin_token");
    navigate("/login");
  };
  return (
    <div className={styles.root}>
      <Sidebar open={menuOpen} onLogout={handleLogout} onClose={() => setMenuOpen(false)} style={{ gridArea: 'sidebar' }} />
      <header className={styles.header} style={{ gridArea: 'header' }}>
        <span>Admin</span>
        <HamburgerMenu onLogout={handleLogout} />
      </header>
      <main className={styles.main} style={{ gridArea: 'main' }}>
        <Outlet />
      </main>
    </div>
  );
}
