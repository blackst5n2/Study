// /lib/business_logic/notifiers/streak_notifier.dart

import 'package:flutter_riverpod/flutter_riverpod.dart';
import '../../data/repositories/settings_repository.dart';
import '../../data/repositories/todo_repository.dart';

class StreakNotifier extends StateNotifier<AsyncValue<int>> {
  final TodoRepository _todoRepository;
  final SettingsRepository _settingsRepository;
  final int _recurringTaskId;

  StreakNotifier(this._todoRepository, this._settingsRepository, this._recurringTaskId) : super(const AsyncValue.loading()) {
    calculateStreak();
  }

  Future<void> calculateStreak() async {
    state = const AsyncValue.loading();
    try {
      final todos = await _todoRepository.getTodosByRecurringTaskId(_recurringTaskId);
      if (todos.isEmpty) {
        state = const AsyncValue.data(0);
        return;
      }

      int currentStreak = 0;
      DateTime today = DateTime.now();
      DateTime todayDateOnly = DateTime(today.year, today.month, today.day);

      // 가장 최근 할 일의 날짜
      DateTime lastTodoDate = DateTime.parse(todos.first.targetDate);

      // 어제 날짜
      DateTime yesterdayDateOnly = todayDateOnly.subtract(const Duration(days: 1));

      // 1. 가장 최근 할 일이 오늘 또는 어제 것이 아니면, 연속이 끊긴 것이므로 스트릭은 0
      if (!lastTodoDate.isAtSameMomentAs(todayDateOnly) && !lastTodoDate.isAtSameMomentAs(yesterdayDateOnly)) {
        state = const AsyncValue.data(0);
        return;
      }

      // 2. 연속 달성일 계산
      DateTime expectedDate = todayDateOnly;
      bool firstChecked = false;

      for (final todo in todos) {
        final todoDate = DateTime.parse(todo.targetDate);

        // 오늘 할 일을 아직 안 했으면, 어제부터 카운트 시작
        if (!firstChecked && todoDate.isAtSameMomentAs(todayDateOnly) && todo.status != 'completed') {
          expectedDate = yesterdayDateOnly;
          firstChecked = true;
          continue; // 오늘 것은 건너뜀
        }

        firstChecked = true;

        if (todoDate.isAtSameMomentAs(expectedDate) && todo.status == 'completed') {
          currentStreak++;
          expectedDate = expectedDate.subtract(const Duration(days: 1));
        } else {
          break; // 연속이 끊기면 종료
        }
      }

      state = AsyncValue.data(currentStreak);

      final maxStreak = await _settingsRepository.loadMaxStreak();
      if (currentStreak > maxStreak) {
        await _settingsRepository.saveMaxStreak(currentStreak);
      }

    } catch (e, st) {
      state = AsyncValue.error(e, st);
    }
  }
}