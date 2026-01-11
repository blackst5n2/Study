import React, { useEffect, useState } from "react"; // 필요한 import만 유지
import axios from "axios";
import UserDetailModal from "./UserDetailModal";
import UserEditModal from "./UserEditModal";
import { DataGrid } from '@mui/x-data-grid';
import { Box, Button, Typography } from '@mui/material';
import AddIcon from '@mui/icons-material/Add';
import styles from './UserListPage.module.css';

export default function UserListPage() {
  const [users, setUsers] = useState([]);
  const [selectedUser, setSelectedUser] = useState(null);
  const [showDetail, setShowDetail] = useState(false);
  const [showEdit, setShowEdit] = useState(false);

  const fetchUsers = async () => {
    try {
      const res = await axios.get(
        process.env.REACT_APP_API_URL + "/api/player",
        { headers: { Authorization: `Bearer ${localStorage.getItem("admin_token")}` } }
      );
      setUsers(res.data);
    } catch (e) {
      setUsers([]);
    }
  };

  useEffect(() => {
    fetchUsers();
    // eslint-disable-next-line
  }, []);

  const handleRowClick = user => {
    setSelectedUser(user);
    setShowDetail(true);
  };

  const handleDelete = async user => {
    if (!window.confirm("정말로 삭제하시겠습니까?")) return;
    await axios.delete(
      process.env.REACT_APP_API_URL + "/api/player/" + user.id,
      { headers: { Authorization: `Bearer ${localStorage.getItem("admin_token")}` } }
    );
    setShowDetail(false);
    fetchUsers();
  };

  const handleEdit = user => {
    setShowDetail(false);
    setTimeout(() => {
      setSelectedUser(user);
      setShowEdit(true);
    }, 100);
  };

  const handleSave = async user => {
    await axios.put(
      process.env.REACT_APP_API_URL + "/api/player/" + user.id,
      user,
      { headers: { Authorization: `Bearer ${localStorage.getItem("admin_token")}` } }
    );
    setShowEdit(false);
    fetchUsers();
  };

  const handleAdd = () => {
    // TODO: implement handleAdd logic
  };

  const columns = [
    { field: 'id', headerName: 'ID', width: 100 },
    { field: 'nickname', headerName: '닉네임', flex: 1, minWidth: 160 },
    { field: 'email', headerName: '이메일', flex: 1, minWidth: 200 },
  ];

  return (
    <Box sx={{ maxHeight: 800, mx: 'auto', p: { xs: 2, md: 5 }, display: 'flex', flexDirection: 'column', alignItems: 'center' }}>
      <Typography variant="h3" fontWeight={900} color="#4f46e5" mb={4} sx={{ letterSpacing: -1.5, textShadow: '0 2px 12px #fff8, 0 1px 0 #fff8', fontSize: { xs: '2.2rem', md: '2.9rem' }, alignSelf: 'flex-start', pl: 1, borderRadius: 1 }}>
        유저 목록
      </Typography>
      <Box sx={{ background: '#fff', borderRadius: 2, boxShadow: '0 4px 24px #b6e0fe22', border: '1.5px solid #e3e8ee', p: 0, mt: 2, width: 800, height: 420, mx: 'auto' }}>
        <DataGrid
          rows={users}
          columns={columns}
          sx={{
            height: '100%',
            background: 'linear-gradient(90deg, #fafdff 0%, #e0e7ff 100%)',
            fontFamily: 'Montserrat, Pretendard, sans-serif',
            transition: 'none !important',
            '& .MuiDataGrid-columnHeaders': {
              background: 'linear-gradient(90deg, #a5b4fc 0%, #c7d2fe 100%)',
              color: '#4f46e5',
              fontWeight: 800,
              letterSpacing: -0.5,
              fontSize: '1.08rem',
            },
            '& .MuiDataGrid-row:hover': {
              background: '#e0e7ff44',
              cursor: 'pointer',
            },
            borderRadius: 2,
            border: 'none',
          }}
          pageSize={10}
          rowsPerPageOptions={[10, 20, 50]}
        />
      </Box>
      <UserDetailModal
        open={showDetail}
        user={selectedUser}
        onClose={() => setShowDetail(false)}
        onDelete={handleDelete}
        onEdit={handleEdit}
      />
      <UserEditModal
        open={showEdit}
        user={selectedUser}
        onClose={() => setShowEdit(false)}
        onSave={handleSave}
      />
    </Box>
  );
}
