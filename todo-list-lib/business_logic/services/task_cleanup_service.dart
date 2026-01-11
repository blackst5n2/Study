// /lib/business_logic/services/task_cleanup_service.dart

import 'package:intl/intl.dart';
import '../../data/repositories/todo_repository.dart';

class TaskCleanupService {
  final TodoRepository _todoRepo;

  TaskCleanupService({required TodoRepository todoRepo}) : _todoRepo = todoRepo;

  Future<void> autoFailOverdueTasks() async {
    try {
      final today = DateTime.now();
      final todayString = DateFormat('yyyy-MM-dd').format(today);

      final overdueTodos = await _todoRepo.getPendingTodosBefore(todayString);

      if (overdueTodos.isNotEmpty) {
        final idsToFail = overdueTodos.map((todo) => todo.id!).toList();

        // 1. 상태를 일괄적으로 'failed'로 업데이트합니다.
        await _todoRepo.batchUpdateStatus(idsToFail, 'failed');

        // 2. [추가] 실패 기록을 일괄적으로 추가합니다.
        const autoReason = '기한이 지나 자동 실패 처리됨';
        await _todoRepo.batchAddFailureLogs(idsToFail, autoReason);

      }
    } catch (e) {}
  }
}