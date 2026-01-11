// /lib/business_logic/providers.dart

import 'package:collection/collection.dart';
import 'package:daily_palette/business_logic/services/recurring_task_service.dart';
import 'package:riverpod/riverpod.dart';
import '../data/data_sources/sqlite_helper.dart';
import '../data/models/category.dart';
import '../data/models/recurring_task.dart';
import '../data/models/todo.dart';
import '../data/repositories/category_repository.dart';
import '../data/repositories/category_repository_impl.dart';
import '../data/repositories/recurring_task_repository.dart';
import '../data/repositories/recurring_task_repository_impl.dart';
import '../data/repositories/todo_repository.dart';
import '../data/repositories/todo_repository_impl.dart';
import 'notifiers/category_notifier.dart';
import 'notifiers/habit_tracker_notifier.dart';
import 'notifiers/recurring_task_notifier.dart';
import 'notifiers/todo_notifier.dart';
import '../presentation/screens/home_screen.dart';
import 'notifiers/statistics_notifier.dart';
import '../presentation/screens/statistics_screen.dart';
import 'package:flutter/material.dart';
import '../data/repositories/theme_repository.dart'; // import 추가
import 'notifiers/theme_notifier.dart';
import 'notifiers/settings_notifier.dart'; // import 추가
import '../data/repositories/settings_repository.dart';
import 'notifiers/streak_notifier.dart';

final navigatorKey = GlobalKey<NavigatorState>();

enum DayStatus {none, success, failure, mixed}
// --- 데이터베이스 ---
final sqliteHelperProvider = Provider<SQLiteHelper>((ref) {
  return SQLiteHelper.instance;
});


// --- Repository Providers ---

// TodoRepository 제공
final todoRepositoryProvider = Provider<TodoRepository>((ref) {
  final dbHelper = ref.watch(sqliteHelperProvider);
  return TodoRepositoryImpl(dbHelper);
});

// [추가된 부분] CategoryRepository 제공
final categoryRepositoryProvider = Provider<CategoryRepository>((ref) {
  final dbHelper = ref.watch(sqliteHelperProvider);
  return CategoryRepositoryImpl(dbHelper);
});


// --- StateNotifier Providers ---

// 할 일 목록의 상태 관리
final todoListProvider = StateNotifierProvider.autoDispose<TodoListNotifier, AsyncValue<List<HierarchicalTodo>>>((ref) {
  return TodoListNotifier(ref);
});

// [수정된 부분] 카테고리 목록의 상태 관리
final categoryListProvider = StateNotifierProvider.autoDispose<CategoryListNotifier, AsyncValue<List<Category>>>((ref) {
  final repository = ref.watch(categoryRepositoryProvider);
  // [오류 수정] Notifier 생성자에 ref를 전달합니다.
  return CategoryListNotifier(repository, ref);
});


// --- 기타 상태 ---

// 현재 선택된 날짜 관리
final selectedDateProvider = StateProvider.autoDispose<DateTime>((ref) => DateTime.now());

final dailyStatsProvider = Provider.autoDispose<DailyStats>((ref) {
  final todosAsyncValue = ref.watch(todoListProvider);

  return todosAsyncValue.when(
    data: (todos) {
      final total = todos.length;
      final completed = todos.where((todo) => todo.parent.todo.status == 'completed').length;
      return DailyStats(totalTodos: total, completedTodos: completed);
    },
    loading: () => DailyStats(totalTodos: 0, completedTodos: 0),
    error: (e, st) => DailyStats(totalTodos: 0, completedTodos: 0),
  );
});

final focusedMonthProvider = StateProvider.autoDispose<DateTime>((ref) => DateTime.now());

final monthlyStatusProvider = FutureProvider.autoDispose<Map<DateTime, DayStatus>>((ref) async {
  final focusedMonth = ref.watch(focusedMonthProvider);
  final repository = ref.watch(todoRepositoryProvider);

  // 현재 보이는 월의 모든 할 일을 가져옵니다.
  final todosOfMonth = await repository.getTodosForMonth(focusedMonth.year, focusedMonth.month);

  // 날짜별로 할 일 목록을 그룹화합니다.
  final groupedTodos = groupBy(todosOfMonth, (Todo todo) {
    final date = DateTime.parse(todo.targetDate);
    // 시간 정보를 제외한 날짜(자정)를 기준으로 그룹화합니다.
    return DateTime.utc(date.year, date.month, date.day);
  });

  // 그룹화된 데이터를 DayStatus로 변환하여 새로운 Map을 생성합니다.
  return groupedTodos.map((date, todos) {
    if (todos.any((todo) => todo.status == 'failed')) {
      return MapEntry(date, DayStatus.failure);
    }
    if (todos.every((todo) => todo.status == 'completed')) {
      return MapEntry(date, DayStatus.success);
    }
    return MapEntry(date, DayStatus.mixed);
  });
});

// [추가] RecurringTaskRepository 제공
final recurringTaskRepositoryProvider = Provider<RecurringTaskRepository>((ref) {
  final dbHelper = ref.watch(sqliteHelperProvider);
  return RecurringTaskRepositoryImpl(dbHelper);
});

// [추가] 반복 일정 목록 상태 관리
final recurringTaskListProvider = StateNotifierProvider.autoDispose<RecurringTaskListNotifier, AsyncValue<List<RecurringTask>>>((ref) {
  final repository = ref.watch(recurringTaskRepositoryProvider);
  return RecurringTaskListNotifier(repository, ref);
});

final recurringTaskServiceProvider = Provider<RecurringTaskService>((ref) {
  final todoRepo = ref.watch(todoRepositoryProvider);
  final recurringRepo = ref.watch(recurringTaskRepositoryProvider);
  return RecurringTaskService(recurringRepo: recurringRepo, todoRepo: todoRepo);
});

final todoCreationTriggerProvider = FutureProvider<void>((ref) async {
  await ref.watch(recurringTaskServiceProvider).createTodosForToday();
});

final statisticsProvider = StateNotifierProvider.autoDispose<StatisticsNotifier, AsyncValue<StatisticsState>>((ref) {
  final todoRepo = ref.watch(todoRepositoryProvider);
  final categoryRepo = ref.watch(categoryRepositoryProvider);
  final settingsRepo = ref.watch(settingsRepositoryProvider);
  return StatisticsNotifier(todoRepo, categoryRepo, settingsRepo);
});

// [추가] ThemeRepository 제공
final themeRepositoryProvider = Provider<ThemeRepository>((ref) => ThemeRepository());

// [추가] 테마 상태 관리
final themeProvider = StateNotifierProvider<ThemeNotifier, ThemeMode>((ref) {
  final repository = ref.watch(themeRepositoryProvider);
  return ThemeNotifier(repository);
});

// [추가] SettingsRepository 제공
final settingsRepositoryProvider = Provider<SettingsRepository>((ref) => SettingsRepository());

// [추가] 앱 설정 상태 관리
final settingsProvider = StateNotifierProvider<SettingsNotifier, AppSettings>((ref) {
  final repository = ref.watch(settingsRepositoryProvider);
  return SettingsNotifier(repository);
});

final streakProvider = StateNotifierProvider.autoDispose.family<StreakNotifier, AsyncValue<int>, int>((ref, recurringTaskId) {
  final todoRepo = ref.watch(todoRepositoryProvider);
  final settingsRepo = ref.watch(settingsRepositoryProvider);
  return StreakNotifier(todoRepo, settingsRepo, recurringTaskId);
});

final habitTrackerProvider = StateNotifierProvider.autoDispose.family<HabitTrackerNotifier, AsyncValue<Map<DateTime, int>>, int>((ref, recurringTaskId) {
  return HabitTrackerNotifier(ref, recurringTaskId);
});

final navigatorKeyProvider = Provider<GlobalKey<NavigatorState>>((ref) => navigatorKey);