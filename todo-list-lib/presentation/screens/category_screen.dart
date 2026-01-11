// /lib/presentation/screens/category_screen.dart

import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import '../../business_logic/providers.dart';
import '../../data/models/category.dart';

class CategoryScreen extends ConsumerWidget {
  const CategoryScreen({super.key});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final categoriesAsyncValue = ref.watch(categoryListProvider);

    return Scaffold(
      appBar: AppBar(
        title: const Text('카테고리 관리'),
      ),
      floatingActionButton: FloatingActionButton(
        onPressed: () => _showAddCategoryDialog(context, ref),
        child: const Icon(Icons.add),
      ),
      body: categoriesAsyncValue.when(
        data: (categories) {
          if (categories.isEmpty) {
            return const Center(child: Text('생성된 카테고리가 없습니다.\n아래 + 버튼을 눌러 추가해주세요.'));
          }
          // [수정] ListTile에 PopupMenuButton 추가
          return ListView.builder(
            itemCount: categories.length,
            itemBuilder: (context, index) {
              final category = categories[index];
              return ListTile(
                leading: CircleAvatar(
                  backgroundColor: Color(int.parse(category.colorCode, radix: 16)),
                  radius: 15,
                ),
                title: Text(category.name),
                trailing: PopupMenuButton<String>(
                  onSelected: (value) {
                    if (value == 'edit') {
                      _showAddCategoryDialog(context, ref, existingCategory: category);
                    } else if (value == 'delete') {
                      _showDeleteConfirmDialog(context, ref, category);
                    }
                  },
                  itemBuilder: (context) => [
                    const PopupMenuItem(value: 'edit', child: Text('수정')),
                    const PopupMenuItem(value: 'delete', child: Text('삭제')),
                  ],
                ),
              );
            },
          );
        },
        loading: () => const Center(child: CircularProgressIndicator()),
        error: (err, stack) => Center(child: Text('오류가 발생했습니다: $err')),
      ),
    );
  }
}

// [추가] 카테고리 삭제 확인 다이얼로그
void _showDeleteConfirmDialog(BuildContext context, WidgetRef ref, Category category) {
  showDialog(
    context: context,
    builder: (context) => AlertDialog(
      title: const Text('카테고리 삭제 경고'),
      content: Text.rich(
        TextSpan(
          style: DefaultTextStyle.of(context).style,
          children: <TextSpan>[
            const TextSpan(text: '정말로 '),
            TextSpan(text: '"${category.name}"', style: const TextStyle(fontWeight: FontWeight.bold)),
            const TextSpan(text: ' 카테고리를 삭제하시겠습니까?\n\n'),
            const TextSpan(
              text: '이 카테고리에 속한 모든 할 일과 반복 일정이 영구적으로 함께 삭제되며, 이 작업은 되돌릴 수 없습니다.',
              style: TextStyle(color: Colors.red, fontWeight: FontWeight.bold),
            ),
          ],
        ),
      ),
      actions: [
        TextButton(onPressed: () => Navigator.of(context).pop(), child: const Text('취소')),
        TextButton(
          onPressed: () {
            ref.read(categoryListProvider.notifier).deleteCategory(category.id!);
            Navigator.of(context).pop();
            ScaffoldMessenger.of(context).showSnackBar(
                SnackBar(content: Text('\'${category.name}\' 카테고리와 관련 데이터가 삭제되었습니다.'))
            );
          },
          child: const Text('삭제', style: TextStyle(color: Colors.red)),
        ),
      ],
    ),
  );
}


// _showAddCategoryDialog 코드는 변경 없음
void _showAddCategoryDialog(BuildContext context, WidgetRef ref, {Category? existingCategory}) {
  final isEditMode = existingCategory != null;
  final titleController = TextEditingController(text: existingCategory?.name ?? '');
  final predefinedColors = [
    Colors.red, Colors.orange, Colors.amber, Colors.green,
    Colors.blue, Colors.indigo, Colors.purple, Colors.grey,
  ];
  Color selectedColor = isEditMode ? Color(int.parse(existingCategory.colorCode, radix: 16)) : predefinedColors.first;

  showDialog(
    context: context,
    builder: (context) {
      return AlertDialog(
        title: Text(isEditMode ? '카테고리 수정' : '새 카테고리'),
        content: StatefulBuilder(
          builder: (context, setState) {
            return Column(
              mainAxisSize: MainAxisSize.min,
              children: [
                TextField(
                  controller: titleController,
                  decoration: const InputDecoration(labelText: '카테고리 이름'),
                  autofocus: true,
                ),
                const SizedBox(height: 20),
                Wrap(
                  spacing: 10, runSpacing: 10,
                  children: predefinedColors.map((color) {
                    return GestureDetector(
                      onTap: () => setState(() => selectedColor = color),
                      child: CircleAvatar(
                        radius: 18,
                        backgroundColor: color,
                        child: selectedColor.value == color.value ? const Icon(Icons.check, color: Colors.white) : null,
                      ),
                    );
                  }).toList(),
                ),
              ],
            );
          },
        ),
        actions: [
          TextButton(onPressed: () => Navigator.of(context).pop(), child: const Text('취소')),
          TextButton(
            onPressed: () {
              if (titleController.text.isNotEmpty) {
                final colorString = selectedColor.value.toRadixString(16).padLeft(8, 'f');
                final notifier = ref.read(categoryListProvider.notifier);

                if (isEditMode) {
                  final updatedCategory = Category(id: existingCategory.id, name: titleController.text, colorCode: colorString);
                  notifier.updateCategory(updatedCategory);
                } else {
                  final newCategory = Category(name: titleController.text, colorCode: colorString);
                  notifier.addCategory(newCategory);
                }
                Navigator.of(context).pop();
              }
            },
            child: const Text('저장'),
          ),
        ],
      );
    },
  );
}