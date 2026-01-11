// /lib/business_logic/services/recurring_task_service.dart

import 'package:intl/intl.dart';
import '../../data/models/recurring_task.dart';
import '../../data/models/todo.dart';
import '../../data/repositories/recurring_task_repository.dart';
import '../../data/repositories/todo_repository.dart';

class RecurringTaskService {
  final RecurringTaskRepository _recurringRepo;
  final TodoRepository _todoRepo;

  RecurringTaskService({
    required RecurringTaskRepository recurringRepo,
    required TodoRepository todoRepo,
  })  : _recurringRepo = recurringRepo,
        _todoRepo = todoRepo;

  // 오늘의 반복 할 일을 생성하는 메인 메서드
  Future<void> createTodosForToday() async {
    final today = DateTime.now();
    final todayString = DateFormat('yyyy-MM-dd').format(today);

    // 1. 오늘 날짜에 해당하는 모든 활성 반복 규칙을 가져옵니다.
    final activeRules = await _recurringRepo.getActiveRecurringTasks(today);

    // 2. 오늘 날짜에 이미 생성된 모든 할 일을 가져옵니다.
    final existingTodos = await _todoRepo.getTodosByDate(todayString);

    for (final rule in activeRules) {
      // 3. 이 규칙으로 오늘 할 일이 이미 생성되었는지 확인합니다.
      final alreadyExists = existingTodos.any((todo) => todo.recurringTaskId == rule.id);
      if (alreadyExists) {
        continue; // 이미 생성되었다면 건너뜁니다.
      }

      // 4. 오늘이 규칙에 해당하는 날짜인지 확인합니다.
      if (_isScheduledForToday(rule, today)) {
        // 5. 조건에 맞으면 새로운 Todo를 생성합니다.
        final newTodo = Todo(
          categoryId: rule.categoryId,
          title: rule.title,
          targetDate: todayString,
          targetTime: rule.targetTime,
          createdAt: DateTime.now().toIso8601String(),
          recurringTaskId: rule.id, // 규칙 ID를 연결합니다.
        );
        final parentId = await _todoRepo.addTodo(newTodo);

        // [추가] 하위 작업 템플릿이 있으면 하위 작업 생성
        if (rule.subtaskTemplates.isNotEmpty) {
          for (final subtaskTitle in rule.subtaskTemplates) {
            final newSubtask = Todo(
              parentId: parentId, // 부모 ID 연결
              categoryId: rule.categoryId,
              title: subtaskTitle,
              targetDate: todayString,
              createdAt: DateTime.now().toIso8601String(),
              recurringTaskId: rule.id,
            );
            await _todoRepo.addTodo(newSubtask);
          }
        }
      }
    }
  }

  // 규칙이 오늘 실행되어야 하는지 확인하는 로직
  bool _isScheduledForToday(RecurringTask rule, DateTime today) {
    switch (rule.recurrenceType) {
      case 'daily':
        return true;
      case 'weekly':
      // '1,2,3,4,5,6,7' (월~일) 형식의 디테일과 오늘의 요일을 비교합니다.
        final weekdays = rule.recurrenceDetail?.split(',') ?? [];
        return weekdays.contains(today.weekday.toString());
      case 'monthly':
      // '15' (15일) 형식의 디테일과 오늘의 날짜를 비교합니다.
        return rule.recurrenceDetail == today.day.toString();
      default:
        return false;
    }
  }
}