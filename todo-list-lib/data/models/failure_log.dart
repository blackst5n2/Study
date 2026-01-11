// /lib/data/models/failure_log.dart

class FailureLog {
  final int id;
  final int todoId;
  final String reason;
  final String loggedAt;

  FailureLog({
    required this.id,
    required this.todoId,
    required this.reason,
    required this.loggedAt,
  });

  factory FailureLog.fromMap(Map<String, dynamic> map) {
    return FailureLog(
      id: map['id'],
      todoId: map['todo_id'],
      reason: map['reason'],
      loggedAt: map['logged_at'],
    );
  }
}