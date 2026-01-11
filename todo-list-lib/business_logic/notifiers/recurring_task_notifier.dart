// /lib/business_logic/notifiers/recurring_task_notifier.dart

import 'package:riverpod/riverpod.dart';
import '../../data/models/recurring_task.dart';
import '../../data/repositories/recurring_task_repository.dart';
import '../providers.dart';

class RecurringTaskListNotifier extends StateNotifier<AsyncValue<List<RecurringTask>>> {
  final RecurringTaskRepository _repository;
  final Ref _ref;

  RecurringTaskListNotifier(this._repository, this._ref) : super(const AsyncValue.loading()) {
    fetchRecurringTasks();
  }

  Future<void> fetchRecurringTasks() async {
    state = const AsyncValue.loading();
    try {
      final tasks = await _repository.getRecurringTasks();
      state = AsyncValue.data(tasks);
    } catch (e, st) {
      state = AsyncValue.error(e, st);
    }
  }

  Future<bool> addRecurringTask(RecurringTask task) async {
    try {
      // 1. 새로운 반복 규칙을 DB에 저장합니다.
      await _repository.addRecurringTask(task);

      // 2. 오늘의 할 일을 생성하는 서비스를 다시 실행합니다.
      await _ref.read(recurringTaskServiceProvider).createTodosForToday();

      // 3. 홈 화면의 Provider들을 무효화하여 새로고침하도록 신호를 보냅니다.
      _ref.invalidate(todoListProvider);
      _ref.invalidate(monthlyStatusProvider);

      // 4. 현재 화면(반복 일정 목록)도 새로고침합니다.
      fetchRecurringTasks();

      return true;
    } catch (e) {
      return false;
    }
  }

  Future<bool> deleteRecurringTask(int id) async {
    try {
      await _repository.deleteRecurringTask(id);
      // 삭제 성공 후, 목록을 새로고침하여 UI에 반영합니다.
      fetchRecurringTasks();
      return true;
    } catch (e) {
      return false;
    }
  }

  Future<bool> updateRecurringTask(RecurringTask task) async {
    try {
      await _repository.updateRecurringTask(task);
      fetchRecurringTasks();
      return true;
    } catch (e) {
      return false;
    }
  }
}