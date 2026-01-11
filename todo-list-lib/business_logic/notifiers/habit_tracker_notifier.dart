// /lib/business_logic/notifiers/habit_tracker_notifier.dart

import 'package:flutter_riverpod/flutter_riverpod.dart';
import '../providers.dart';

// 잔디 심기 화면의 상태를 관리하는 Notifier
class HabitTrackerNotifier extends StateNotifier<AsyncValue<Map<DateTime, int>>> {
  final Ref _ref;
  final int recurringTaskId;

  HabitTrackerNotifier(this._ref, this.recurringTaskId) : super(const AsyncValue.loading()) {
    fetchHabitData();
  }

  Future<void> fetchHabitData() async {
    state = const AsyncValue.loading();
    try {
      final todoRepository = _ref.read(todoRepositoryProvider);
      // [수정] 1년치가 아닌, 현재 연도 데이터를 요청하도록 변경
      final data = await todoRepository.getTodoCompletionStatusForHabit(recurringTaskId, DateTime.now().year);
      state = AsyncValue.data(data);
    } catch (e, st) {
      state = AsyncValue.error(e, st);
    }
  }
}