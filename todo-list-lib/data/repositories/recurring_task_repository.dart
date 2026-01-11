// /lib/data/repositories/recurring_task_repository.dart

import '../models/recurring_task.dart';

abstract class RecurringTaskRepository {
  Future<List<RecurringTask>> getActiveRecurringTasks(DateTime date);
  Future<List<RecurringTask>> getRecurringTasks(); // 목록 조회를 위해 추가
  Future<int> addRecurringTask(RecurringTask task); // 생성을 위해 추가
  Future<int> deleteRecurringTask(int id);
  Future<int> updateRecurringTask(RecurringTask task);
}