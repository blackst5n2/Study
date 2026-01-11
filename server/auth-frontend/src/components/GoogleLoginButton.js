import React from 'react';
import { GoogleLogin } from '@react-oauth/google';

function GoogleLoginButton({ onSuccess }) {
  return (
    <div style={{ textAlign: 'center', margin: '0 auto' }}>
      <GoogleLogin
        onSuccess={onSuccess}
        onError={() => alert('구글 로그인 실패')}
        width="320"
        useOneTap
      />
    </div>
  );
}

export default GoogleLoginButton;
