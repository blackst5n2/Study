import React, { useEffect, useState } from "react"; // 필요한 import만 유지
import axios from "axios";
import QuestDetailModal from "./QuestDetailModal";
import QuestEditModal from "./QuestEditModal";
import { DataGrid } from '@mui/x-data-grid';
import { Box, Button, Typography } from '@mui/material';
import AddIcon from '@mui/icons-material/Add';

export default function QuestListPage() {
  const [quests, setQuests] = useState([]);
  const [selectedQuest, setSelectedQuest] = useState(null);
  const [showDetail, setShowDetail] = useState(false);
  const [showEdit, setShowEdit] = useState(false);

  const fetchQuests = async () => {
    try {
      const res = await axios.get(
        process.env.REACT_APP_API_URL + "/api/quest",
        { headers: { Authorization: `Bearer ${localStorage.getItem("admin_token")}` } }
      );
      setQuests(res.data);
    } catch (e) {
      setQuests([]);
    }
  };

  useEffect(() => {
    fetchQuests();
    // eslint-disable-next-line
  }, []);

  const handleRowClick = quest => {
    setSelectedQuest(quest);
    setShowDetail(true);
  };

  const handleDelete = async quest => {
    if (!window.confirm("정말로 삭제하시겠습니까?")) return;
    await axios.delete(
      process.env.REACT_APP_API_URL + "/api/quest/" + quest.id,
      { headers: { Authorization: `Bearer ${localStorage.getItem("admin_token")}` } }
    );
    setShowDetail(false);
    fetchQuests();
  };

  const handleEdit = quest => {
    setShowDetail(false);
    setTimeout(() => {
      setSelectedQuest(quest);
      setShowEdit(true);
    }, 100);
  };

  const handleSave = async quest => {
    await axios.put(
      process.env.REACT_APP_API_URL + "/api/quest/" + quest.id,
      quest,
      { headers: { Authorization: `Bearer ${localStorage.getItem("admin_token")}` } }
    );
    setShowEdit(false);
    fetchQuests();
  };

  const handleAdd = () => {
    // TODO: Add new quest logic
  };

  const columns = [
    { field: 'id', headerName: 'ID', width: 100 },
    { field: 'title', headerName: '제목', flex: 1, minWidth: 160 },
    { field: 'description', headerName: '설명', flex: 1, minWidth: 200 },
  ];

  return (
    <Box sx={{ maxWidth: 1400, mx: 'auto', p: { xs: 2, md: 5 } }}>
      <Typography variant="h4" fontWeight={900} color="#4f46e5" mb={3} sx={{ letterSpacing: -1.5, textShadow: '0 2px 12px #fff8, 0 1px 0 #fff8', fontSize: { xs: '2rem', md: '2.7rem' } }}>
        퀘스트 목록
      </Typography>
      <Box sx={{ display: 'flex', justifyContent: 'flex-end', mb: 2, gap: 1 }}>
        <Button
          variant="contained"
          startIcon={<AddIcon />}
          onClick={handleAdd}
          sx={{
            background: 'linear-gradient(90deg, #6366f1 0%, #a5b4fc 100%)',
            borderRadius: '22px',
            fontWeight: 800,
            fontSize: '1.09rem',
            px: 3,
            boxShadow: '0 2px 12px #a5b4fc33',
            letterSpacing: -0.5,
            '&:hover': {
              background: 'linear-gradient(90deg, #a5b4fc 0%, #6366f1 100%)',
            },
          }}
        >
          퀘스트 추가
        </Button>
      </Box>
      <Box sx={{ background: '#fff', borderRadius: 2, boxShadow: '0 4px 24px #b6e0fe22', border: '1.5px solid #e3e8ee', p: 0, mt: 2, width: 800, height: 420, mx: 'auto' }}>
        <DataGrid
          rows={quests}
          columns={columns}
          sx={{
            height: '100%',
            background: 'linear-gradient(90deg, #fafdff 0%, #e0e7ff 100%)',
            fontFamily: 'Montserrat, Pretendard, sans-serif',
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
      <QuestDetailModal
        open={showDetail}
        quest={selectedQuest}
        onClose={() => setShowDetail(false)}
        onDelete={handleDelete}
        onEdit={handleEdit}
      />
      <QuestEditModal
        open={showEdit}
        quest={selectedQuest}
        onClose={() => setShowEdit(false)}
        onSave={handleSave}
      />
    </Box>
  );
}

