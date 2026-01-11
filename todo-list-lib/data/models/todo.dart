// /lib/data/models/todo.dart
import 'package:equatable/equatable.dart';

class Todo extends Equatable {
  final int? id;
  final int categoryId;
  final String title;
  final String targetDate;
  final String? targetTime;
  final String status;
  final String createdAt;
  final String? completedAt;
  final int? priority;
  final int? parentId;
  final int? recurringTaskId;
  final int? displayOrder;
  final int timeSpentSeconds;
  final int? timerDurationMinutes;
  final String? timerEndTime;

  const Todo({
    this.id,
    required this.categoryId,
    required this.title,
    required this.targetDate,
    this.targetTime,
    this.status = 'pending',
    required this.createdAt,
    this.completedAt,
    this.priority,
    this.parentId,
    this.recurringTaskId,
    this.displayOrder,
    this.timeSpentSeconds = 0,
    this.timerDurationMinutes,
    this.timerEndTime,
  });

  @override
  List<Object?> get props => [id, categoryId, title, targetDate, targetTime, status, createdAt, completedAt, priority, parentId, recurringTaskId, displayOrder, timeSpentSeconds, timerDurationMinutes, timerEndTime];

  // [수정] 단순하고 표준적인 copyWith 메서드로 변경
  Todo copyWith({
    int? id, int? categoryId, String? title, String? targetDate,
    String? targetTime, bool clearTargetTime = false,
    String? status, String? createdAt, String? completedAt, bool clearCompletedAt = false,
    int? priority, int? parentId, int? recurringTaskId, int? displayOrder,
    int? timeSpentSeconds, int? timerDurationMinutes, String? timerEndTime, bool clearTimer = false,
  }) {
    return Todo(
      id: id ?? this.id,
      categoryId: categoryId ?? this.categoryId,
      title: title ?? this.title,
      targetDate: targetDate ?? this.targetDate,
      targetTime: clearTargetTime ? null : (targetTime ?? this.targetTime),
      status: status ?? this.status,
      createdAt: createdAt ?? this.createdAt,
      completedAt: clearCompletedAt ? null : (completedAt ?? this.completedAt),
      priority: priority ?? this.priority,
      parentId: parentId ?? this.parentId,
      recurringTaskId: recurringTaskId ?? this.recurringTaskId,
      displayOrder: displayOrder ?? this.displayOrder,
      timeSpentSeconds: timeSpentSeconds ?? this.timeSpentSeconds,
      timerDurationMinutes: clearTimer ? null : (timerDurationMinutes ?? this.timerDurationMinutes),
      timerEndTime: clearTimer ? null : (timerEndTime ?? this.timerEndTime),
    );
  }

  factory Todo.fromMap(Map<String, dynamic> map) {
    return Todo(
      id: map['id'], categoryId: map['category_id'], title: map['title'],
      targetDate: map['target_date'], targetTime: map['target_time'],
      status: map['status'], createdAt: map['created_at'],
      completedAt: map['completed_at'], priority: map['priority'],
      parentId: map['parent_id'], recurringTaskId: map['recurring_task_id'],
      displayOrder: map['display_order'],
      timeSpentSeconds: map['time_spent_seconds'] ?? 0,
      timerDurationMinutes: map['timer_duration_minutes'],
      timerEndTime: map['timer_end_time'],
    );
  }

  Map<String, dynamic> toMap() {
    return {
      'id': id, 'category_id': categoryId, 'title': title, 'target_date': targetDate,
      'target_time': targetTime, 'status': status, 'created_at': createdAt,
      'completed_at': completedAt, 'priority': priority, 'parent_id': parentId,
      'recurring_task_id': recurringTaskId, 'display_order': displayOrder,
      'time_spent_seconds': timeSpentSeconds, 'timer_duration_minutes': timerDurationMinutes,
      'timer_end_time': timerEndTime,
    };
  }
}