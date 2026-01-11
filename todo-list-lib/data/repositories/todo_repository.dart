// /data/repositories/todo_repository.dart

import '../models/failure_log.dart';
import '../models/todo.dart';

abstract class TodoRepository {
  Future<List<Todo>> getTodosByDate(String date);
  Future<List<Todo>> getTodosForMonth(int year, int month);
  Future<List<Todo>> getAllTodos();
  Future<int> addTodo(Todo todo);
  Future<int> updateTodo(Todo todo);
  Future<int> deleteTodo(int id);
  Future<int> addFailureLog(int todoId, String reason);
  Future<Todo?> getTodoById(int id);
  Future<List<Todo>> getSubTasks(int parentId);
  Future<List<Todo>> getTodosByRecurringTaskId(int recurringTaskId);
  Future<void> updateTodoOrder(List<Todo> orderedTodos);
  Future<List<Todo>> getPendingTodosBefore(String date); // [추가]
  Future<void> batchUpdateStatus(List<int> todoIds, String newStatus); // [추가]
  Future<Map<int, FailureLog>> getFailureLogsForTodos(List<int> todoIds);
  Future<void> batchAddFailureLogs(List<int> todoIds, String reason);
  Future<void> addTimeToTodo(int todoId, int secondsToAdd);
  Future<int> clearTimer(int todoId);
  Future<Map<DateTime, int>> getTodoCompletionStatusForHabit(int recurringTaskId, int year);
}