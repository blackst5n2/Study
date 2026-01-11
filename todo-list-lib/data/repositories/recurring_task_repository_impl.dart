// /lib/data/repositories/recurring_task_repository_impl.dart

import 'package:intl/intl.dart';
import '../data_sources/sqlite_helper.dart';
import '../models/recurring_task.dart';
import 'recurring_task_repository.dart';

class RecurringTaskRepositoryImpl implements RecurringTaskRepository {
  final SQLiteHelper _dbHelper;

  RecurringTaskRepositoryImpl(this._dbHelper);

  @override
  Future<List<RecurringTask>> getActiveRecurringTasks(DateTime date) async {
    final db = await _dbHelper.database;
    final dateString = DateFormat('yyyy-MM-dd').format(date);

    // 시작일이 오늘 이전이고, 종료일이 없거나 오늘 이후인 규칙만 조회
    final List<Map<String, dynamic>> maps = await db.query(
      'Recurring_Tasks',
      where: 'start_date <= ? AND (end_date IS NULL OR end_date >= ?)',
      whereArgs: [dateString, dateString],
    );
    return List.generate(maps.length, (i) => RecurringTask.fromMap(maps[i]));
  }

  @override
  Future<List<RecurringTask>> getRecurringTasks() async {
    final db = await _dbHelper.database;
    final List<Map<String, dynamic>> maps = await db.query('Recurring_Tasks', orderBy: 'id DESC');
    return List.generate(maps.length, (i) => RecurringTask.fromMap(maps[i]));
  }

  @override
  Future<int> addRecurringTask(RecurringTask task) async {
    final db = await _dbHelper.database;
    return await db.insert('Recurring_Tasks', task.toMap());
  }

  @override
  Future<int> deleteRecurringTask(int id) async {
    final db = await _dbHelper.database;
    return await db.delete(
      'Recurring_Tasks',
      where: 'id = ?',
      whereArgs: [id],
    );
  }

  @override
  Future<int> updateRecurringTask(RecurringTask task) async {
    final db = await _dbHelper.database;
    return await db.update('Recurring_Tasks', task.toMap(), where: 'id = ?', whereArgs: [task.id]);
  }
}