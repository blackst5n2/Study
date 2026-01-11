// /lib/data/models/recurring_task.dart

import 'dart:convert'; // JSON 처리를 위해 import

class RecurringTask {
  final int? id;
  final int categoryId;
  final String title;
  final String? targetTime;
  final String recurrenceType;
  final String? recurrenceDetail;
  final String startDate;
  final String? endDate;
  final List<String> subtaskTemplates; // 하위 작업 템플릿 리스트 추가

  RecurringTask({
    this.id,
    required this.categoryId,
    required this.title,
    this.targetTime,
    required this.recurrenceType,
    this.recurrenceDetail,
    required this.startDate,
    this.endDate,
    this.subtaskTemplates = const [], // 기본값은 빈 리스트
  });

  Map<String, dynamic> toMap() {
    return {
      'id': id,
      'category_id': categoryId,
      'title': title,
      'target_time': targetTime,
      'recurrence_type': recurrenceType,
      'recurrence_detail': recurrenceDetail,
      'start_date': startDate,
      'end_date': endDate,
      'subtask_templates': jsonEncode(subtaskTemplates), // 리스트를 JSON 문자열로 변환
    };
  }

  factory RecurringTask.fromMap(Map<String, dynamic> map) {
    return RecurringTask(
      id: map['id'],
      categoryId: map['category_id'],
      title: map['title'],
      targetTime: map['target_time'],
      recurrenceType: map['recurrence_type'],
      recurrenceDetail: map['recurrence_detail'],
      startDate: map['start_date'],
      endDate: map['end_date'],
      // JSON 문자열을 리스트로 변환, null이거나 변환 실패 시 빈 리스트 반환
      subtaskTemplates: map['subtask_templates'] != null
          ? List<String>.from(jsonDecode(map['subtask_templates']))
          : [],
    );
  }
}