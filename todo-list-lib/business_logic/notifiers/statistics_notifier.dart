// /lib/business_logic/notifiers/statistics_notifier.dart

import 'package:collection/collection.dart';
import 'package:daily_palette/data/repositories/settings_repository.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:intl/intl.dart';
import '../../data/models/category.dart';
import '../../data/models/todo.dart';
import '../../data/repositories/category_repository.dart';
import '../../data/repositories/todo_repository.dart';
import '../../presentation/screens/statistics_screen.dart';

class StatisticsNotifier extends StateNotifier<AsyncValue<StatisticsState>> {
  final TodoRepository _todoRepo;
  final CategoryRepository _categoryRepo;
  final SettingsRepository _settingsRepo;

  StatisticsNotifier(this._todoRepo, this._categoryRepo, this._settingsRepo) : super(const AsyncValue.loading()) {
    fetchStats();
  }

  Future<void> fetchStats() async {
    // [수정] 위젯이 마운트된 상태가 아니면 더 이상 진행하지 않음
    if (!mounted) return;
    state = const AsyncValue.loading();

    try {
      final allCategories = await _categoryRepo.getCategories();
      final allTodos = await _todoRepo.getAllTodos();

      // 위젯이 데이터를 기다리는 동안 dispose될 수 있으므로, 다시 확인
      if (!mounted) return;

      final categoryStats = _calculateCategoryStats(allCategories, allTodos);
      final weeklyStats = _calculateWeeklyStats(allTodos);
      final monthlyStats = _calculateMonthlyStats(allTodos);
      final overallStats = await _calculateOverallStats(allTodos, categoryStats);

      // 최종 상태를 업데이트하기 전에도 다시 확인
      if (mounted) {
        state = AsyncValue.data(StatisticsState(
          categoryStats: categoryStats,
          weeklyStats: weeklyStats,
          monthlyStats: monthlyStats,
          overallStats: overallStats,
        ));
      }

    } catch (e, st) {
      if (mounted) {
        state = AsyncValue.error(e, st);
      }
    }
  }

  // --- 헬퍼 함수들 ---

  List<CategoryStats> _calculateCategoryStats(List<Category> allCategories, List<Todo> allTodos) {
    final todosByCategoryId = groupBy(allTodos, (Todo todo) => todo.categoryId);
    final List<CategoryStats> stats = [];
    for (final category in allCategories) {
      final todosForCategory = todosByCategoryId[category.id] ?? [];
      final completedCount = todosForCategory.where((t) => t.status == 'completed').length;
      final failedCount = todosForCategory.where((t) => t.status == 'failed').length;

      stats.add(CategoryStats(
        category: category,
        totalCount: todosForCategory.length,
        completedCount: completedCount,
        failedCount: failedCount,
      ));
    }
    stats.sort((a, b) => b.totalCount.compareTo(a.totalCount));
    return stats;
  }

  // [수정] 이번 주(일요일~토요일) 통계를 계산하는 함수
  List<DailyCompletionRate> _calculateWeeklyStats(List<Todo> allTodos) {
    final today = DateTime.now();
    final todosByDate = groupBy(allTodos, (Todo todo) => todo.targetDate);
    final List<DailyCompletionRate> stats = [];

    // Dart에서 weekday는 월요일=1, 일요일=7 입니다.
    // 이번 주 일요일을 찾습니다. (today.weekday % 7)은 일요일(0)부터 토요일(6)까지의 오프셋을 줍니다.
    final startOfWeek = today.subtract(Duration(days: today.weekday % 7));

    for (int i = 0; i < 7; i++) {
      final date = startOfWeek.add(Duration(days: i));
      final dateString = DateFormat('yyyy-MM-dd').format(date);

      final todosForDay = todosByDate[dateString] ?? [];
      final completed = todosForDay.where((t) => t.status == 'completed').length;
      final failed = todosForDay.where((t) => t.status == 'failed').length;
      final totalRelevant = completed + failed;

      stats.add(DailyCompletionRate(
        date: date,
        rate: totalRelevant == 0 ? 0.0 : completed / totalRelevant,
      ));
    }
    return stats;
  }

  // [수정] 이번 달(1일~말일) 통계를 계산하는 함수
  List<DailyCompletionRate> _calculateMonthlyStats(List<Todo> allTodos) {
    final today = DateTime.now();
    final todosByDate = groupBy(allTodos, (Todo todo) => todo.targetDate);
    final List<DailyCompletionRate> stats = [];

    // 이번 달의 첫째 날과 마지막 날을 구합니다.
    final firstDayOfMonth = DateTime(today.year, today.month, 1);
    final lastDayOfMonth = DateTime(today.year, today.month + 1, 0);
    final daysInMonth = lastDayOfMonth.day;

    for (int i = 0; i < daysInMonth; i++) {
      final date = firstDayOfMonth.add(Duration(days: i));
      final dateString = DateFormat('yyyy-MM-dd').format(date);

      final todosForDay = todosByDate[dateString] ?? [];
      final completed = todosForDay.where((t) => t.status == 'completed').length;
      final failed = todosForDay.where((t) => t.status == 'failed').length;
      final totalRelevant = completed + failed;

      stats.add(DailyCompletionRate(
          date: date,
          rate: totalRelevant == 0 ? 0.0 : completed / totalRelevant
      ));
    }
    return stats;
  }

  Future<OverallStats> _calculateOverallStats(List<Todo> allTodos, List<CategoryStats> categoryStats) async {
    // ... (기존과 동일)
    final completed = allTodos.where((t) => t.status == 'completed').length;
    final failed = allTodos.where((t) => t.status == 'failed').length;

    Category? mostProductiveCategory;
    if (categoryStats.isNotEmpty) {
      final tempStats = [...categoryStats];
      tempStats.sort((a, b) => b.completedCount.compareTo(a.completedCount));
      if(tempStats.first.completedCount > 0) {
        mostProductiveCategory = tempStats.first.category;
      }
    }

    final maxStreak = await _settingsRepo.loadMaxStreak();

    return OverallStats(
      totalTodos: allTodos.length,
      totalCompleted: completed,
      totalFailed: failed,
      mostProductiveCategory: mostProductiveCategory,
      maxStreak: maxStreak,
    );
  }
}