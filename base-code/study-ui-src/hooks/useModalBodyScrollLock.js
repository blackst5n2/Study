import { useEffect } from "react";

/**
 * useModalBodyScrollLock
 * 모달이 열릴 때 body의 스크롤을 막고, 닫히면 원복합니다.
 * @param {boolean} open - 모달 오픈 여부
 */
export default function useModalBodyScrollLock(open) {
  useEffect(() => {
    if (open) {
      // 기존 스크롤 위치 저장
      const scrollY = window.scrollY;
      document.body.style.position = 'fixed';
      document.body.style.top = `-${scrollY}px`;
      document.body.style.width = '100%';
      document.body.style.overflowY = 'hidden';
      return () => {
        document.body.style.position = '';
        document.body.style.top = '';
        document.body.style.width = '';
        document.body.style.overflowY = '';
        window.scrollTo(0, scrollY);
      };
    }
  }, [open]);
}
