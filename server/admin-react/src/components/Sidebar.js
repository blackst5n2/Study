import React from "react"; // 필요한 import만 유지
import { Link, useLocation } from "react-router-dom";
import styles from "./Sidebar.module.css";
import { FaInbox, FaStar, FaCalendarAlt, FaHashtag, FaUsers, FaTrash, FaSun, FaChartLine } from "react-icons/fa";

const navItems = [
  { to: "/", icon: <FaInbox />, label: "대시보드" },
  { to: "/users", icon: <FaUsers />, label: "유저" },
  { to: "/items", icon: <FaHashtag />, label: "아이템" },
  { to: "/quests", icon: <FaStar />, label: "퀘스트" },
];

export default function Sidebar({ open, onLogout, onClose }) {
  const location = useLocation();
  // 반응형: 모바일에서만 오버레이와 슬라이드
  return (
    <>
      {/* 오버레이: 모바일에서만, 사이드바 열릴 때 */}
      <div
        className={open ? styles.sidebarOverlay : ''}
        style={{ display: open ? 'block' : 'none' }}
        onClick={onClose}
        tabIndex={-1}
      />
      <aside className={open ? styles.sidebar + ' ' + styles.open : styles.sidebar}>
        <div className={styles.windowBar}>
        <span className={styles.dot} style={{ background: "#ff5f56" }} />
        <span className={styles.dot} style={{ background: "#ffbd2e" }} />
        <span className={styles.dot} style={{ background: "#27c93f" }} />
      </div>
      <nav className={styles.navSection}>
        {navItems.map(item => (
          <Link 
            key={item.to}
            to={item.to}
            className={location.pathname === item.to ? styles.activeNav : styles.navItem}
          >
            <span className={styles.icon}>{item.icon}</span>
            {item.label}
          </Link>
        ))}
      </nav>

    </aside>
    </>
  );
}
