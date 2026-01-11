import '../data_sources/sqlite_helper.dart';
import '../models/failure_log.dart';
import '../models/todo.dart';
import 'todo_repository.dart';

class TodoRepositoryImpl implements TodoRepository {
  final SQLiteHelper _dbHelper;

  TodoRepositoryImpl(this._dbHelper);

  @override
  Future<List<Todo>> getTodosByDate(String date) async {
    final db = await _dbHelper.database;
    final List<Map<String, dynamic>> maps = await db.query(
      'Todos',
      where: 'target_date = ?',
      whereArgs: [date],
      orderBy: 'display_order ASC', // display_order 순으로 정렬
    );
    return List.generate(maps.length, (i) => Todo.fromMap(maps[i]));
  }

  @override
  Future<int> addTodo(Todo todo) async {
    final db = await _dbHelper.database;
    // 트랜잭션을 사용하여 원자성 보장
    return await db.transaction((txn) async {
      // 같은 날짜의 기존 할 일들의 display_order를 1씩 증가시킴
      await txn.rawUpdate(
        'UPDATE Todos SET display_order = display_order + 1 WHERE target_date = ?',
        [todo.targetDate],
      );
      // 새 할 일은 display_order를 0으로 설정하여 최상단에 위치시킴
      final newTodo = todo.copyWith(displayOrder: 0);
      return await txn.insert('Todos', newTodo.toMap());
    });
  }

  @override
  Future<int> updateTodo(Todo todo) async {
    final db = await _dbHelper.database;
    return await db.update(
      'Todos',
      todo.toMap(),
      where: 'id = ?',
      whereArgs: [todo.id],
    );
  }

  @override
  Future<int> deleteTodo(int id) async {
    final db = await _dbHelper.database;
    // 하위 작업도 함께 삭제되도록 CASCADE 설정이 DB에 되어있음
    await db.delete(
      'Todos',
      where: 'id = ? OR parent_id = ?',
      whereArgs: [id, id],
    );
    return 1;
  }

  @override
  Future<List<Todo>> getTodosForMonth(int year, int month) async {
    final db = await _dbHelper.database;
    final monthString = month.toString().padLeft(2, '0');
    final datePattern = '$year-$monthString-%';

    final List<Map<String, dynamic>> maps = await db.query(
      'Todos',
      where: 'target_date LIKE ?',
      whereArgs: [datePattern],
    );

    return List.generate(maps.length, (i) {
      return Todo.fromMap(maps[i]);
    });
  }

  @override
  Future<int> addFailureLog(int todoId, String reason) async {
    final db = await _dbHelper.database;
    return await db.insert('Failure_Logs', {
      'todo_id': todoId,
      'reason': reason,
      'logged_at': DateTime.now().toIso8601String(),
    });
  }

  @override
  Future<List<Todo>> getAllTodos() async {
    final db = await _dbHelper.database;
    final List<Map<String, dynamic>> maps = await db.query('Todos');
    return List.generate(maps.length, (i) => Todo.fromMap(maps[i]));
  }

  @override
  Future<List<Todo>> getSubTasks(int parentId) async {
    final db = await _dbHelper.database;
    final List<Map<String, dynamic>> maps = await db.query(
      'Todos',
      where: 'parent_id = ?',
      whereArgs: [parentId],
    );
    return List.generate(maps.length, (i) => Todo.fromMap(maps[i]));
  }

  @override
  Future<Todo?> getTodoById(int id) async {
    final db = await _dbHelper.database;
    final List<Map<String, dynamic>> maps = await db.query(
      'Todos',
      where: 'id = ?',
      whereArgs: [id],
    );
    if (maps.isNotEmpty) {
      return Todo.fromMap(maps.first);
    }
    return null;
  }

  @override
  Future<List<Todo>> getTodosByRecurringTaskId(int recurringTaskId) async {
    final db = await _dbHelper.database;
    final List<Map<String, dynamic>> maps = await db.query(
      'Todos',
      where: 'recurring_task_id = ?',
      whereArgs: [recurringTaskId],
      orderBy: 'target_date DESC',
    );
    return List.generate(maps.length, (i) => Todo.fromMap(maps[i]));
  }

  @override
  Future<void> updateTodoOrder(List<Todo> orderedTodos) async {
    final db = await _dbHelper.database;
    final batch = db.batch();
    for (int i = 0; i < orderedTodos.length; i++) {
      final todo = orderedTodos[i];
      batch.update(
        'Todos',
        {'display_order': i}, // <-- 여기는 0부터 시작하는 인덱스 'i'
        where: 'id = ?',
        whereArgs: [todo.id],
      );
    }
    await batch.commit(noResult: true);
  }

  @override
  Future<List<Todo>> getPendingTodosBefore(String date) async {
    final db = await _dbHelper.database;
    final List<Map<String, dynamic>> maps = await db.query(
      'Todos',
      where: 'target_date < ? AND status = ?',
      whereArgs: [date, 'pending'],
    );
    return List.generate(maps.length, (i) => Todo.fromMap(maps[i]));
  }

  // [추가] 여러 할 일의 상태를 한 번에 업데이트합니다. (효율적)
  @override
  Future<void> batchUpdateStatus(List<int> todoIds, String newStatus) async {
    final db = await _dbHelper.database;
    final batch = db.batch();
    for (final id in todoIds) {
      batch.update('Todos', {'status': newStatus}, where: 'id = ?', whereArgs: [id]);
    }
    await batch.commit(noResult: true);
  }

  // [추가] 여러 todoId에 해당하는 실패 기록을 한 번에 가져와 Map으로 반환
  @override
  Future<Map<int, FailureLog>> getFailureLogsForTodos(List<int> todoIds) async {
    if (todoIds.isEmpty) return {};
    final db = await _dbHelper.database;
    // '?' 바인딩을 사용하여 IN 절을 안전하게 구성
    final placeholders = List.filled(todoIds.length, '?').join(',');
    final List<Map<String, dynamic>> maps = await db.query(
      'Failure_Logs',
      where: 'todo_id IN ($placeholders)',
      whereArgs: todoIds,
    );

    final logMap = <int, FailureLog>{};
    for (final map in maps) {
      final log = FailureLog.fromMap(map);
      logMap[log.todoId] = log;
    }
    return logMap;
  }

  // [추가] 여러 개의 실패 기록을 Batch를 사용해 한 번에 추가합니다.
  @override
  Future<void> batchAddFailureLogs(List<int> todoIds, String reason) async {
    final db = await _dbHelper.database;
    final batch = db.batch();
    final loggedAt = DateTime.now().toIso8601String();
    for (final id in todoIds) {
      batch.insert('Failure_Logs', {'todo_id': id, 'reason': reason, 'logged_at': loggedAt});
    }
    await batch.commit(noResult: true);
  }

  @override
  Future<void> addTimeToTodo(int todoId, int secondsToAdd) async {
    final db = await _dbHelper.database;
    await db.rawUpdate(
      'UPDATE Todos SET time_spent_seconds = time_spent_seconds + ? WHERE id = ?',
      [secondsToAdd, todoId],
    );
  }

  @override
  Future<int> clearTimer(int todoId) async {
    final db = await _dbHelper.database;
    return await db.update(
      'Todos',
      {
        'timer_duration_minutes': null,
        'timer_end_time': null,
      },
      where: 'id = ?',
      whereArgs: [todoId],
    );
  }

  @override
  Future<Map<DateTime, int>> getTodoCompletionStatusForHabit(int recurringTaskId, int year) async {
    final db = await _dbHelper.database;
    final startDate = '$year-01-01';
    final endDate = '$year-12-31';

    final List<Map<String, dynamic>> maps = await db.query(
      'Todos',
      columns: ['target_date', 'status'],
      where: 'recurring_task_id = ? AND target_date BETWEEN ? AND ?',
      whereArgs: [recurringTaskId, startDate, endDate],
    );

    final Map<DateTime, int> statusMap = {};
    for (final map in maps) {
      final date = DateTime.parse(map['target_date'] as String);
      final status = map['status'] as String;
      // 완료된 경우에만 1로 표시, 나머지는 잔디를 심지 않음
      if (status == 'completed') {
        statusMap[DateTime.utc(date.year, date.month, date.day)] = 1;
      }
    }
    return statusMap;
  }
}