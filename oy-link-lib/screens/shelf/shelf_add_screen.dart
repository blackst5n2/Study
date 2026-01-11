import 'package:flutter/material.dart';
import 'package:oy_pro_link/models/shelf_model.dart';
import 'package:oy_pro_link/providers/shelf_provider.dart';
import 'package:provider/provider.dart';

class ShelfAddScreen extends StatefulWidget {
  final Shelf? shelf;

  const ShelfAddScreen({super.key, this.shelf});

  @override
  State<ShelfAddScreen> createState() => _ShelfAddScreenState();
}

class _ShelfAddScreenState extends State<ShelfAddScreen> {
  final _formKey = GlobalKey<FormState>();
  late TextEditingController _nameController;
  late TextEditingController _memoController;

  bool get _isEditing => widget.shelf != null;

  @override
  void initState() {
    super.initState();
    _nameController = TextEditingController(text: widget.shelf?.name ?? '');
    _memoController = TextEditingController(text: widget.shelf?.memo ?? '');
  }

  @override
  void dispose() {
    _nameController.dispose();
    _memoController.dispose();
    super.dispose();
  }

  Future<void> _saveShelf() async {
    if (_formKey.currentState!.validate()) {
      final shelfProvider = Provider.of<ShelfProvider>(context, listen: false);
      final newShelf = Shelf(
        id: widget.shelf?.id,
        name: _nameController.text,
        memo: _memoController.text,
      );

      if (_isEditing) {
        await shelfProvider.updateShelf(newShelf);
      } else {
        await shelfProvider.addShelf(newShelf);
      }

      if (mounted) Navigator.of(context).pop();
    }
  }

  // ### 삭제 기능 및 확인 다이얼로그 로직 추가 ###
  Future<void> _deleteShelf() async {
    final bool? confirm = await showDialog(
      context: context,
      builder: (context) => AlertDialog(
        title: const Text('진열대 삭제 확인'),
        content: const Text(
          '정말로 이 진열대를 삭제하시겠습니까?\n\n'
              '이 진열대에 지정된 모든 제품의 위치 정보가 초기화되고, 지도에 배치된 블록도 함께 삭제됩니다.',
        ),
        actions: [
          TextButton(
            onPressed: () => Navigator.of(context).pop(false),
            child: const Text('취소'),
          ),
          TextButton(
            onPressed: () => Navigator.of(context).pop(true),
            child: const Text('삭제', style: TextStyle(color: Colors.red)),
          ),
        ],
      ),
    );

    if (confirm == true && mounted) {
      final shelfProvider = Provider.of<ShelfProvider>(context, listen: false);
      await shelfProvider.deleteShelf(widget.shelf!.id!);
      Navigator.of(context).pop(); // 삭제 후 목록 화면으로 돌아가기
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text(_isEditing ? '진열대 수정' : '새 진열대 추가'),
        actions: [
          // ### 편집 모드일 때만 삭제 버튼 표시 ###
          if (_isEditing)
            IconButton(
              icon: const Icon(Icons.delete_outline, color: Colors.red),
              onPressed: _deleteShelf,
              tooltip: '진열대 삭제',
            ),
          IconButton(
            icon: const Icon(Icons.save_outlined),
            onPressed: _saveShelf,
          ),
        ],
      ),
      body: SingleChildScrollView(
        padding: const EdgeInsets.all(16.0),
        child: Form(
          key: _formKey,
          child: Column(
            children: [
              TextFormField(
                controller: _nameController,
                decoration: const InputDecoration(
                  labelText: '진열대 이름 *',
                  hintText: '예: 기초케어 A-1',
                ),
                validator: (value) {
                  if (value == null || value.isEmpty) {
                    return '진열대 이름을 입력해주세요.';
                  }
                  return null;
                },
              ),
              const SizedBox(height: 16),
              TextFormField(
                controller: _memoController,
                decoration: const InputDecoration(
                  labelText: '메모',
                ),
                maxLines: 3,
              ),
            ],
          ),
        ),
      ),
    );
  }
}