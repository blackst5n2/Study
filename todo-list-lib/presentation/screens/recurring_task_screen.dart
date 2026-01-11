// /lib/presentation/screens/recurring_task_screen.dart

import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:intl/intl.dart';
import 'package:collection/collection.dart';
import '../../business_logic/providers.dart';
import '../../data/models/category.dart';
import '../../data/models/recurring_task.dart';
import 'habit_tracker_screen.dart';
import 'category_screen.dart'; // 카테고리 화면으로 이동하기 위해 import

class RecurringTaskListScreen extends ConsumerWidget {
  const RecurringTaskListScreen({super.key});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final recurringTasks = ref.watch(recurringTaskListProvider);

    return Scaffold(
      appBar: AppBar(
        title: const Text('반복 일정 관리'),
        actions: [
          IconButton(
            icon: const Icon(Icons.add),
            onPressed: () {
              Navigator.of(context).push(
                MaterialPageRoute(builder: (context) => const RecurringTaskEditScreen()),
              );
            },
          ),
        ],
      ),
      body: recurringTasks.when(
        data: (tasks) {
          if (tasks.isEmpty) {
            return const Center(child: Text('생성된 반복 일정이 없습니다.'));
          }
          return ListView.builder(
            itemCount: tasks.length,
            itemBuilder: (context, index) {
              final task = tasks[index];
              return Dismissible(
                key: ValueKey(task.id),
                direction: DismissDirection.endToStart,
                confirmDismiss: (direction) async {
                  return await showDialog<bool>(
                    context: context,
                    builder: (BuildContext context) {
                      return AlertDialog(
                        title: const Text('삭제 확인'),
                        content: const Text('이 반복 일정을 정말 삭제하시겠습니까?'),
                        actions: <Widget>[
                          TextButton(
                            onPressed: () => Navigator.of(context).pop(false),
                            child: const Text('아니오'),
                          ),
                          TextButton(
                            onPressed: () => Navigator.of(context).pop(true),
                            child: const Text('예'),
                          ),
                        ],
                      );
                    },
                  ) ?? false;
                },
                onDismissed: (direction) {
                  ref.read(recurringTaskListProvider.notifier).deleteRecurringTask(task.id!);
                },
                background: Container(
                  color: Colors.red,
                  alignment: Alignment.centerRight,
                  padding: const EdgeInsets.symmetric(horizontal: 20.0),
                  child: const Icon(Icons.delete, color: Colors.white),
                ),
                child: ListTile(
                  onTap: () {
                    Navigator.of(context).push(MaterialPageRoute(
                      builder: (context) => HabitTrackerScreen(
                        recurringTaskId: task.id!,
                        taskTitle: task.title,
                      ),
                    ));
                  },
                  leading: StreakDisplay(recurringTaskId: task.id!),
                  title: Text(task.title),
                  subtitle: Text(_getRecurrenceSummary(task)),
                  trailing: IconButton(
                    icon: const Icon(Icons.edit_outlined),
                    onPressed: () {
                      Navigator.of(context).push(MaterialPageRoute(
                        builder: (context) => RecurringTaskEditScreen(task: task),
                      )).then((_) {
                        ref.invalidate(streakProvider(task.id!));
                      });
                    },
                  ),
                ),
              );
            },
          );
        },
        loading: () => const Center(child: CircularProgressIndicator()),
        error: (e, st) => Center(child: Text('오류: $e')),
      ),
    );
  }

  String _getRecurrenceSummary(RecurringTask task) {
    switch (task.recurrenceType) {
      case 'daily':
        return '매일';
      case 'weekly':
        final days = task.recurrenceDetail?.split(',').map((d) {
          try {
            return ['월', '화', '수', '목', '금', '토', '일'][int.parse(d) - 1];
          } catch(e) {
            return '';
          }
        }).where((s) => s.isNotEmpty).join(', ');
        return '매주 $days';
      case 'monthly':
        return '매월 ${task.recurrenceDetail}일';
      default:
        return '알 수 없는 규칙';
    }
  }
}

class StreakDisplay extends ConsumerWidget {
  final int recurringTaskId;
  const StreakDisplay({super.key, required this.recurringTaskId});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final streakAsyncValue = ref.watch(streakProvider(recurringTaskId));

    return streakAsyncValue.when(
      data: (streak) {
        if (streak == 0) {
          return const SizedBox(width: 40, child: Icon(Icons.sync_disabled, color: Colors.grey));
        }
        return SizedBox(
          width: 40,
          child: Row(
            mainAxisAlignment: MainAxisAlignment.center,
            children: [
              const Icon(Icons.local_fire_department, color: Colors.orange),
              const SizedBox(width: 2),
              Text('$streak', style: const TextStyle(fontWeight: FontWeight.bold)),
            ],
          ),
        );
      },
      loading: () => const SizedBox(width: 40, height: 24, child: Center(child: CircularProgressIndicator(strokeWidth: 2))),
      error: (e, st) => const SizedBox(width: 40, child: Icon(Icons.error_outline, color: Colors.red)),
    );
  }
}

class RecurringTaskEditScreen extends ConsumerStatefulWidget {
  final RecurringTask? task;
  const RecurringTaskEditScreen({super.key, this.task});

  @override
  ConsumerState<RecurringTaskEditScreen> createState() => _RecurringTaskEditScreenState();
}

class _RecurringTaskEditScreenState extends ConsumerState<RecurringTaskEditScreen> {
  final _formKey = GlobalKey<FormState>();
  late TextEditingController _titleController;
  late bool _isEditMode;

  Category? _selectedCategory;
  String _recurrenceType = 'daily';
  DateTime _startDate = DateTime.now();
  final List<bool> _weeklyDays = List.filled(7, false);
  int _monthlyDay = 1;

  List<String> _subtaskTemplates = [];

  @override
  void initState() {
    super.initState();
    _isEditMode = widget.task != null;
    _titleController = TextEditingController(text: widget.task?.title ?? '');

    if (_isEditMode) {
      final task = widget.task!;
      _recurrenceType = task.recurrenceType;
      _startDate = DateTime.parse(task.startDate);
      _subtaskTemplates = List.from(task.subtaskTemplates);

      if (task.recurrenceType == 'weekly' && task.recurrenceDetail != null) {
        task.recurrenceDetail!.split(',').forEach((day) {
          try {
            _weeklyDays[int.parse(day) - 1] = true;
          } catch (e) {}
        });
      } else if (task.recurrenceType == 'monthly' && task.recurrenceDetail != null) {
        _monthlyDay = int.tryParse(task.recurrenceDetail!) ?? 1;
      }
    }
  }

  void _showSubtaskDialog({int? editIndex}) {
    final isEditing = editIndex != null;
    final controller = TextEditingController(text: isEditing ? _subtaskTemplates[editIndex] : '');
    showDialog(
      context: context,
      builder: (context) {
        return AlertDialog(
          title: Text(isEditing ? '하위 작업 수정' : '하위 작업 추가'),
          content: TextField(
            controller: controller,
            autofocus: true,
            decoration: const InputDecoration(labelText: '내용'),
          ),
          actions: [
            TextButton(onPressed: () => Navigator.pop(context), child: const Text('취소')),
            TextButton(
              onPressed: () {
                if (controller.text.isNotEmpty) {
                  setState(() {
                    if (isEditing) {
                      _subtaskTemplates[editIndex] = controller.text;
                    } else {
                      _subtaskTemplates.add(controller.text);
                    }
                  });
                  Navigator.pop(context);
                }
              },
              child: Text(isEditing ? '저장' : '추가'),
            ),
          ],
        );
      },
    );
  }

  @override
  Widget build(BuildContext context) {
    final categories = ref.watch(categoryListProvider);

    return Scaffold(
      appBar: AppBar(
        title: Text(_isEditMode ? '반복 일정 수정' : '반복 일정 생성'),
        actions: [IconButton(icon: const Icon(Icons.save), onPressed: _saveRecurringTask)],
      ),
      body: categories.when(
        data: (categoryList) {
          // ================== [ 핵심 수정 부분 ] ==================
          if (categoryList.isEmpty) {
            return const _CategoryCreationPromptForRecurring();
          }
          // ========================================================

          if (_selectedCategory == null) {
            if (_isEditMode) {
              _selectedCategory = categoryList.firstWhereOrNull((c) => c.id == widget.task!.categoryId);
            }
            // 카테고리 목록이 비어있지 않음을 위에서 보장하므로, `first`를 안전하게 사용
            _selectedCategory ??= categoryList.first;
          }

          return Form(
            key: _formKey,
            child: ListView(
              padding: const EdgeInsets.all(16),
              children: [
                TextFormField(
                  controller: _titleController,
                  decoration: const InputDecoration(labelText: '제목'),
                  validator: (value) => (value?.isEmpty ?? true) ? '제목을 입력하세요' : null,
                ),
                DropdownButtonFormField<Category>(
                  value: _selectedCategory,
                  items: categoryList.map((c) => DropdownMenuItem(value: c, child: Text(c.name))).toList(),
                  onChanged: (value) => setState(() => _selectedCategory = value),
                  decoration: const InputDecoration(labelText: '카테고리'),
                  validator: (value) => value == null ? '카테고리를 선택하세요' : null,
                ),
                DropdownButtonFormField<String>(
                  value: _recurrenceType,
                  items: const [
                    DropdownMenuItem(value: 'daily', child: Text('매일')),
                    DropdownMenuItem(value: 'weekly', child: Text('매주')),
                    DropdownMenuItem(value: 'monthly', child: Text('매월')),
                  ],
                  onChanged: (value) => setState(() => _recurrenceType = value!),
                  decoration: const InputDecoration(labelText: '반복 유형'),
                ),
                if (_recurrenceType == 'weekly') _buildWeeklySelector(),
                if (_recurrenceType == 'monthly') _buildMonthlySelector(),
                const SizedBox(height: 16),
                ListTile(
                  contentPadding: EdgeInsets.zero,
                  title: const Text('시작일'),
                  subtitle: Text(DateFormat('yyyy-MM-dd').format(_startDate)),
                  onTap: () async {
                    final date = await showDatePicker(context: context, initialDate: _startDate, firstDate: DateTime(2020), lastDate: DateTime(2100));
                    if (date != null) setState(() => _startDate = date);
                  },
                ),
                const Divider(height: 32),
                _buildSubtaskManager(),
              ],
            ),
          );
        },
        loading: () => const Center(child: CircularProgressIndicator()),
        error: (e, st) => Center(child: Text('카테고리를 불러올 수 없습니다. \n$e')),
      ),
    );
  }

  Widget _buildSubtaskManager() {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Row(
          mainAxisAlignment: MainAxisAlignment.spaceBetween,
          children: [
            const Text('하위 작업 템플릿', style: TextStyle(fontSize: 16, fontWeight: FontWeight.bold)),
            IconButton(
              icon: const Icon(Icons.add_circle_outline),
              onPressed: () => _showSubtaskDialog(),
            ),
          ],
        ),
        if (_subtaskTemplates.isEmpty)
          const Padding(
            padding: EdgeInsets.symmetric(vertical: 16.0),
            child: Center(
              child: Text('하위 작업이 없습니다.', style: TextStyle(color: Colors.grey)),
            ),
          )
        else
          ReorderableListView.builder(
            shrinkWrap: true,
            physics: const NeverScrollableScrollPhysics(),
            itemCount: _subtaskTemplates.length,
            itemBuilder: (context, index) {
              final title = _subtaskTemplates[index];
              return ListTile(
                key: ValueKey(title + index.toString()),
                leading: const Icon(Icons.subdirectory_arrow_right),
                title: Text(title),
                trailing: Row(
                  mainAxisSize: MainAxisSize.min,
                  children: [
                    IconButton(
                      icon: const Icon(Icons.edit_outlined),
                      onPressed: () => _showSubtaskDialog(editIndex: index),
                    ),
                    IconButton(
                      icon: const Icon(Icons.delete_outline, color: Colors.red),
                      onPressed: () {
                        setState(() {
                          _subtaskTemplates.removeAt(index);
                        });
                      },
                    ),
                  ],
                ),
              );
            },
            onReorder: (oldIndex, newIndex) {
              setState(() {
                if (oldIndex < newIndex) {
                  newIndex -= 1;
                }
                final item = _subtaskTemplates.removeAt(oldIndex);
                _subtaskTemplates.insert(newIndex, item);
              });
            },
          ),
      ],
    );
  }

  Widget _buildWeeklySelector() {
    final dayNames = ['월', '화', '수', '목', '금', '토', '일'];
    return Padding(
      padding: const EdgeInsets.symmetric(vertical: 8.0),
      child: Wrap(
        spacing: 8,
        children: List.generate(7, (index) {
          return FilterChip(
            label: Text(dayNames[index]),
            selected: _weeklyDays[index],
            onSelected: (selected) {
              setState(() {
                _weeklyDays[index] = selected;
              });
            },
          );
        }),
      ),
    );
  }

  Widget _buildMonthlySelector() {
    return DropdownButtonFormField<int>(
      value: _monthlyDay,
      items: List.generate(31, (index) => DropdownMenuItem(value: index + 1, child: Text('${index + 1}일'))),
      onChanged: (value) => setState(() => _monthlyDay = value!),
      decoration: const InputDecoration(labelText: '날짜 선택'),
    );
  }

  void _saveRecurringTask() {
    // 저장 버튼을 눌렀을 때, 카테고리 목록이 비어있으면 저장하지 않음.
    if (ref.read(categoryListProvider).valueOrNull?.isEmpty ?? true) {
      ScaffoldMessenger.of(context).showSnackBar(const SnackBar(content: Text('카테고리를 먼저 생성해주세요.')));
      return;
    }

    final category = _selectedCategory;
    if (_formKey.currentState?.validate() ?? false) {
      if (category == null) {
        ScaffoldMessenger.of(context).showSnackBar(const SnackBar(content: Text('카테고리를 선택해주세요.')));
        return;
      }
      String? detail;
      if (_recurrenceType == 'weekly') {
        detail = _weeklyDays.asMap().entries.where((e) => e.value).map((e) => e.key + 1).join(',');
        if (detail.isEmpty) {
          ScaffoldMessenger.of(context).showSnackBar(const SnackBar(content: Text('요일을 하나 이상 선택하세요.')));
          return;
        }
      } else if (_recurrenceType == 'monthly') {
        detail = _monthlyDay.toString();
      }

      final recurringTaskData = RecurringTask(
        id: widget.task?.id,
        title: _titleController.text,
        categoryId: category.id!,
        recurrenceType: _recurrenceType,
        recurrenceDetail: detail,
        startDate: DateFormat('yyyy-MM-dd').format(_startDate),
        subtaskTemplates: _subtaskTemplates,
      );

      final notifier = ref.read(recurringTaskListProvider.notifier);
      final future = _isEditMode
          ? notifier.updateRecurringTask(recurringTaskData)
          : notifier.addRecurringTask(recurringTaskData);

      future.then((success) {
        if (success && mounted) {
          ref.invalidate(todoCreationTriggerProvider);
          Navigator.of(context).pop();
        } else if (mounted) {
          ScaffoldMessenger.of(context).showSnackBar(const SnackBar(content: Text('저장에 실패했습니다.')));
        }
      });
    }
  }
}

// [추가] 반복 일정용 카테고리 생성 안내 위젯
class _CategoryCreationPromptForRecurring extends StatelessWidget {
  const _CategoryCreationPromptForRecurring();

  @override
  Widget build(BuildContext context) {
    return Center(
      child: Padding(
        padding: const EdgeInsets.all(32.0),
        child: Column(
          mainAxisSize: MainAxisSize.min,
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            const Icon(Icons.warning_amber_rounded, size: 50, color: Colors.orange),
            const SizedBox(height: 16),
            const Text(
              '카테고리가 필요해요!',
              style: TextStyle(fontSize: 18, fontWeight: FontWeight.bold),
              textAlign: TextAlign.center,
            ),
            const SizedBox(height: 8),
            const Text(
              '반복 일정을 추가하려면 먼저 카테고리를\n하나 이상 만들어야 합니다.',
              textAlign: TextAlign.center,
            ),
            const SizedBox(height: 24),
            ElevatedButton.icon(
              icon: const Icon(Icons.add_circle_outline),
              label: const Text('카테고리 만들러 가기'),
              onPressed: () {
                // 현재 화면을 닫지 않고, 카테고리 화면으로 이동
                Navigator.of(context).push(
                  MaterialPageRoute(builder: (context) => const CategoryScreen()),
                );
              },
            ),
          ],
        ),
      ),
    );
  }
}