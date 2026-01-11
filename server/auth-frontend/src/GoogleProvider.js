import React from 'react';
import { GoogleOAuthProvider } from '@react-oauth/google';

// TODO: 실제 클라이언트 ID로 교체
const GOOGLE_CLIENT_ID = 'test';

export default function GoogleProvider({ children }) {
  return (
    <GoogleOAuthProvider clientId={GOOGLE_CLIENT_ID}>
      {children}
    </GoogleOAuthProvider>
  );
}
