// /lib/business_logic/notifiers/todo_notifier.dart

import 'dart:async';
import 'package:android_alarm_manager_plus/android_alarm_manager_plus.dart';
import 'package:flutter/material.dart';
import 'package:collection/collection.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:intl/intl.dart';

import '../../data/models/todo.dart';
import '../../data/models/failure_log.dart';
import '../../data/repositories/todo_repository.dart';
import '../../data/repositories/category_repository.dart';
import '../../presentation/screens/home_screen.dart';
import '../providers.dart';
import '../services/notification_service.dart';
import '../services/home_widget_service.dart';
import '../../main.dart';

class TodoListNotifier extends StateNotifier<AsyncValue<List<HierarchicalTodo>>> {
  final Ref _ref;
  final TodoRepository _todoRepository;
  final CategoryRepository _categoryRepository;
  ProviderSubscription? _subscription;

  // "실행 취소" 기능을 위한 임시 저장 변수
  HierarchicalTodo? _lastDeletedParent;
  TodoWithCategory? _lastDeletedSubtask;
  int? _lastDeletedParentIndex;
  int? _lastDeletedSubtaskIndex;
  Timer? _deleteTimer;

  TodoListNotifier(this._ref)
      : _todoRepository = _ref.read(todoRepositoryProvider),
        _categoryRepository = _ref.read(categoryRepositoryProvider),
        super(const AsyncValue.loading()) {
    // 선택된 날짜가 변경되면, 해당 날짜의 할 일 목록을 불러옵니다.
    _subscription = _ref.listen<DateTime>(selectedDateProvider, (previous, next) {
      _finalizePendingDelete(); // 날짜가 바뀌면 보류 중인 삭제를 즉시 확정
      fetchTodos(next);
    }, fireImmediately: true);
  }

  // 데이터 로딩 중 상태를 변경하지 않고 내부적으로만 처리 (깜빡임 방지)
  Future<void> _silentDbUpdate(Future<void> Function() dbOperation) async {
    try {
      await dbOperation();
      // DB 작업 후에는 위젯 데이터와 다른 provider들을 새로고침
      HomeWidgetService.updateWidgetData();
      _ref.invalidate(monthlyStatusProvider);
      _ref.invalidate(statisticsProvider);
    } catch (e) {
      // 에러 발생 시, 데이터 정합성을 위해 목록을 강제로 새로고침
      fetchTodos(_ref.read(selectedDateProvider));
    }
  }

  // 상태를 직접 조작하여 UI를 즉시 업데이트 (깜빡임 방지)
  void _optimisticUpdate(List<HierarchicalTodo> Function(List<HierarchicalTodo> currentList) updateFn) {
    // 현재 상태가 데이터가 있는 상태일 때만 실행
    if (state is! AsyncData<List<HierarchicalTodo>>) return;
    final currentList = state.asData!.value;
    final newList = updateFn(List.from(currentList)); // 수정 함수를 통해 새 목록 생성
    _sortTodoList(newList); // 목록 정렬
    state = AsyncValue.data(newList); // 정렬된 새 목록으로 상태 업데이트
  }

  // 할 일 목록 정렬 로직
  void _sortTodoList(List<HierarchicalTodo> list) {
    list.sort((a, b) {
      final aTodo = a.parent.todo;
      final bTodo = b.parent.todo;
      final isACompleted = aTodo.status == 'completed';
      final isBCompleted = bTodo.status == 'completed';

      if (isACompleted && !isBCompleted) return 1;
      if (!isACompleted && isBCompleted) return -1;

      if (isACompleted) { // 완료된 항목은 완료 시간 역순으로 정렬
        final aTime = aTodo.completedAt != null ? DateTime.tryParse(aTodo.completedAt!) : null;
        final bTime = bTodo.completedAt != null ? DateTime.tryParse(bTodo.completedAt!) : null;
        if (aTime != null && bTime != null) return bTime.compareTo(aTime);
        return 0;
      } else { // 활성 항목은 displayOrder 순으로 정렬
        return (aTodo.displayOrder ?? 999).compareTo(bTodo.displayOrder ?? 999);
      }
    });
  }

  // 특정 날짜의 할 일 목록을 불러오는 기본 함수
  Future<void> fetchTodos(DateTime date) async {
    if (!mounted) return;
    state = const AsyncValue.loading();
    try {
      final formattedDate = DateFormat('yyyy-MM-dd').format(date);
      final todos = await _todoRepository.getTodosByDate(formattedDate);
      final categories = await _categoryRepository.getCategories();
      final categoryMap = {for (var c in categories) c.id: c};
      final failedTodoIds = todos.where((t) => t.status == 'failed').map((t) => t.id!).toList();
      final failureLogMap = await _todoRepository.getFailureLogsForTodos(failedTodoIds);

      final parentTodos = todos.where((t) => t.parentId == null).toList();
      final subTasksByParentId = groupBy(todos.where((t) => t.parentId != null), (Todo t) => t.parentId);

      final List<HierarchicalTodo> hierarchicalList = [];
      for (final pTodo in parentTodos) {
        final childrenWithCategory = (subTasksByParentId[pTodo.id] ?? []).map((sTodo) {
          return TodoWithCategory(todo: sTodo, category: categoryMap[sTodo.categoryId], failureLog: failureLogMap[sTodo.id]);
        }).toList();
        hierarchicalList.add(HierarchicalTodo(
            parent: TodoWithCategory(todo: pTodo, category: categoryMap[pTodo.categoryId], failureLog: failureLogMap[pTodo.id]),
            children: childrenWithCategory));
      }

      _sortTodoList(hierarchicalList);
      if (mounted) state = AsyncValue.data(hierarchicalList);
    } catch (e, st) {
      if (mounted) state = AsyncValue.error(e, st);
    }
  }

  // 할 일 추가
  Future<void> addTodo(String title, int categoryId, {TimeOfDay? time, int? priority, int? timerDuration}) async {
    _finalizePendingDelete();
    final selectedDate = _ref.read(selectedDateProvider);
    if (_isPastDate(selectedDate)) return;

    final String? timerEndTime = timerDuration != null ? DateTime.now().add(Duration(minutes: timerDuration)).toIso8601String() : null;
    final newTodo = Todo(
        title: title, categoryId: categoryId, targetDate: DateFormat('yyyy-MM-dd').format(selectedDate),
        createdAt: DateTime.now().toIso8601String(),
        targetTime: time != null ? '${time.hour.toString().padLeft(2, '0')}:${time.minute.toString().padLeft(2, '0')}' : null,
        priority: priority, timerDurationMinutes: timerDuration, timerEndTime: timerEndTime, displayOrder: 0);

    final category = await _categoryRepository.getCategoryById(categoryId);

    // DB에 추가하고 ID를 받아옴
    final newId = await _todoRepository.addTodo(newTodo);
    final newTodoWithId = newTodo.copyWith(id: newId);
    final newHierarchicalTodo = HierarchicalTodo(parent: TodoWithCategory(todo: newTodoWithId, category: category), children: []);

    // UI 즉시 업데이트
    _optimisticUpdate((currentList) => [newHierarchicalTodo, ...currentList]);

    // 백그라운드에서 알림 설정 및 기타 작업 수행
    _silentDbUpdate(() async {
      if (time != null) {
        final scheduleTime = DateTime(selectedDate.year, selectedDate.month, selectedDate.day, time.hour, time.minute);
        await NotificationService.instance.scheduleNotification(id: newId, title: '할 일 알림', body: title, scheduledDateTime: scheduleTime);
      }
      await _scheduleAutoFailTimer(newId, timerEndTime);
    });
  }

  // 하위 작업 추가
  Future<void> addSubTask(String title, int parentId) async {
    _finalizePendingDelete();
    final parentTodo = await _todoRepository.getTodoById(parentId);
    if (parentTodo == null) return;

    final newSubTask = Todo(
        title: title, categoryId: parentTodo.categoryId, targetDate: parentTodo.targetDate,
        createdAt: DateTime.now().toIso8601String(), parentId: parentId);

    final newId = await _todoRepository.addTodo(newSubTask);
    final newSubTaskWithId = newSubTask.copyWith(id: newId);
    final category = await _categoryRepository.getCategoryById(newSubTaskWithId.categoryId);
    final newSubtaskWithCategory = TodoWithCategory(todo: newSubTaskWithId, category: category);

    // UI 즉시 업데이트
    _optimisticUpdate((currentList) {
      final parentIndex = currentList.indexWhere((h) => h.parent.todo.id == parentId);
      if (parentIndex != -1) {
        final updatedChildren = [...currentList[parentIndex].children, newSubtaskWithCategory];
        currentList[parentIndex] = currentList[parentIndex].copyWith(children: updatedChildren);
      }
      return currentList;
    });

    _silentDbUpdate(() async {}); // Provider 새로고침 트리거
  }

  // 할 일 정보 업데이트
  Future<void> updateTodo({required int todoId, required String newTitle, required int newCategoryId, required int newPriority, required TimeOfDay? newTime, required int? newTimerDuration}) async {
    _finalizePendingDelete();
    final originalTodo = await _todoRepository.getTodoById(todoId);
    if (originalTodo == null) return;

    final String? timerEndTime = newTimerDuration != null ? DateTime.now().add(Duration(minutes: newTimerDuration)).toIso8601String() : null;
    final String? formattedTime = newTime != null ? '${newTime.hour.toString().padLeft(2, '0')}:${newTime.minute.toString().padLeft(2, '0')}' : null;
    final updatedTodo = originalTodo.copyWith(
        title: newTitle, categoryId: newCategoryId, priority: newPriority,
        targetTime: formattedTime, clearTargetTime: newTime == null,
        timerDurationMinutes: newTimerDuration, timerEndTime: timerEndTime, clearTimer: newTimerDuration == null);

    final newCategory = await _categoryRepository.getCategoryById(newCategoryId);

    // UI 즉시 업데이트
    _optimisticUpdate((currentList) {
      final parentIndex = currentList.indexWhere((h) => h.parent.todo.id == todoId);
      if (parentIndex != -1) {
        final updatedParent = currentList[parentIndex].parent.copyWith(todo: updatedTodo, category: newCategory);
        currentList[parentIndex] = currentList[parentIndex].copyWith(parent: updatedParent);
      }
      return currentList;
    });

    // 백그라운드 DB 작업
    _silentDbUpdate(() async {
      await _todoRepository.updateTodo(updatedTodo);
      await NotificationService.instance.cancelNotification(todoId);
      if (newTime != null) {
        final scheduleTime = DateTime(DateTime.parse(updatedTodo.targetDate).year, DateTime.parse(updatedTodo.targetDate).month, DateTime.parse(updatedTodo.targetDate).day, newTime.hour, newTime.minute);
        await NotificationService.instance.scheduleNotification(id: todoId, title: '할 일 수정됨', body: newTitle, scheduledDateTime: scheduleTime);
      }
      await _scheduleAutoFailTimer(todoId, timerEndTime);
    });
  }

  // 할 일 제목만 업데이트 (하위 작업용)
  Future<void> updateTodoTitle(int todoId, String newTitle) async {
    _finalizePendingDelete();
    final originalTodo = await _todoRepository.getTodoById(todoId);
    if (originalTodo == null) return;
    final updatedTodo = originalTodo.copyWith(title: newTitle);

    _optimisticUpdate((currentList) {
      for (int i = 0; i < currentList.length; i++) {
        final subtaskIndex = currentList[i].children.indexWhere((child) => child.todo.id == todoId);
        if (subtaskIndex != -1) {
          final updatedChildren = List<TodoWithCategory>.from(currentList[i].children);
          updatedChildren[subtaskIndex] = updatedChildren[subtaskIndex].copyWith(todo: updatedTodo);
          currentList[i] = currentList[i].copyWith(children: updatedChildren);
          break;
        }
      }
      return currentList;
    });

    _silentDbUpdate(() => _todoRepository.updateTodo(updatedTodo));
  }

  // 할 일 상태 업데이트 (완료/실패 등)
  Future<void> updateTodoStatus(int todoId, String newStatus, {String? reason}) async {
    _finalizePendingDelete();
    int? parentIdToCheck;

    _optimisticUpdate((currentList) {
      for (int i = 0; i < currentList.length; i++) {
        final hTodo = currentList[i];
        if (hTodo.parent.todo.id == todoId) {
          final updatedParent = _updateTodoWithStatus(hTodo.parent, newStatus, reason: reason);
          currentList[i] = hTodo.copyWith(parent: updatedParent);
          break;
        }
        final subtaskIndex = hTodo.children.indexWhere((c) => c.todo.id == todoId);
        if (subtaskIndex != -1) {
          if (newStatus == 'completed') parentIdToCheck = hTodo.parent.todo.id;
          final updatedChild = _updateTodoWithStatus(hTodo.children[subtaskIndex], newStatus, reason: reason);
          hTodo.children[subtaskIndex] = updatedChild;
          break;
        }
      }
      return currentList;
    });

    _silentDbUpdate(() async {
      final originalTodo = await _todoRepository.getTodoById(todoId);
      if (originalTodo == null) return;
      final shouldClearTimer = newStatus == 'completed' || newStatus == 'failed';
      final updatedTodo = originalTodo.copyWith(
          status: newStatus,
          completedAt: newStatus == 'completed' ? DateTime.now().toIso8601String() : null,
          clearCompletedAt: newStatus != 'completed',
          clearTimer: shouldClearTimer);
      await _todoRepository.updateTodo(updatedTodo);

      if (newStatus == 'failed' && reason != null && reason.isNotEmpty) {
        await _todoRepository.addFailureLog(todoId, reason);
      }
      if (shouldClearTimer) {
        await NotificationService.instance.cancelNotification(todoId);
        await AndroidAlarmManager.cancel(todoId);
      }
    });

    if (parentIdToCheck != null) await _checkAndUpdateParentStatus(parentIdToCheck!);
  }

  // 상태 변경된 TodoWithCategory 객체를 생성하는 헬퍼 함수
  TodoWithCategory _updateTodoWithStatus(TodoWithCategory item, String newStatus, {String? reason}) {
    final shouldClearTimer = newStatus == 'completed' || newStatus == 'failed';
    return item.copyWith(
      todo: item.todo.copyWith(
        status: newStatus,
        completedAt: newStatus == 'completed' ? DateTime.now().toIso8601String() : null,
        clearCompletedAt: newStatus != 'completed',
        clearTimer: shouldClearTimer,
      ),
      failureLog: newStatus == 'failed' ? FailureLog(id: -1, todoId: item.todo.id!, reason: reason ?? '', loggedAt: DateTime.now().toIso8601String()) : null,
    );
  }

  // 모든 하위 작업 완료 시 부모 작업 자동 완료
  Future<void> _checkAndUpdateParentStatus(int parentId) async {
    final subTasks = await _todoRepository.getSubTasks(parentId);
    if (subTasks.isNotEmpty && subTasks.every((task) => task.status == 'completed')) {
      updateTodoStatus(parentId, 'completed');
    }
  }

  // 할 일 삭제 (Undo 기능 포함)
  Future<void> deleteTodo(int todoId) async {
    _finalizePendingDelete();
    _optimisticUpdate((currentList) {
      for (int i = 0; i < currentList.length; i++) {
        if (currentList[i].parent.todo.id == todoId) {
          _lastDeletedParentIndex = i;
          _lastDeletedParent = currentList.removeAt(i);
          break;
        }
        final subtaskIndex = currentList[i].children.indexWhere((child) => child.todo.id == todoId);
        if (subtaskIndex != -1) {
          _lastDeletedParentIndex = i;
          _lastDeletedSubtaskIndex = subtaskIndex;
          _lastDeletedSubtask = currentList[i].children.removeAt(subtaskIndex);
          break;
        }
      }
      return currentList;
    });

    _deleteTimer = Timer(const Duration(seconds: 4), _permanentlyDelete);
  }

  // 삭제 실행 취소
  void undoDelete() {
    _deleteTimer?.cancel();
    if (_lastDeletedParent == null && _lastDeletedSubtask == null) return;

    _optimisticUpdate((currentList) {
      if (_lastDeletedParent != null && _lastDeletedParentIndex != null) {
        currentList.insert(_lastDeletedParentIndex!, _lastDeletedParent!);
      } else if (_lastDeletedSubtask != null && _lastDeletedParentIndex != null && _lastDeletedSubtaskIndex != null) {
        currentList[_lastDeletedParentIndex!].children.insert(_lastDeletedSubtaskIndex!, _lastDeletedSubtask!);
      }
      return currentList;
    });

    _lastDeletedParent = null;
    _lastDeletedSubtask = null;
  }

  // 보류 중인 삭제를 영구적으로 처리
  void _finalizePendingDelete() {
    if (_deleteTimer?.isActive ?? false) {
      _deleteTimer?.cancel();
      _permanentlyDelete();
    }
  }

  // DB에서 실제 삭제 수행
  void _permanentlyDelete() {
    int? idToDelete;
    if (_lastDeletedParent != null) {
      idToDelete = _lastDeletedParent!.parent.todo.id!;
    } else if (_lastDeletedSubtask != null) idToDelete = _lastDeletedSubtask!.todo.id!;

    if (idToDelete != null) {
      _silentDbUpdate(() async {
        await _todoRepository.deleteTodo(idToDelete!);
        NotificationService.instance.cancelNotification(idToDelete);
        AndroidAlarmManager.cancel(idToDelete);
      });
    }

    _lastDeletedParent = null;
    _lastDeletedSubtask = null;
  }

  // 할 일 순서 재정렬
  Future<void> reorderTodos(int oldIndex, int newIndex) async {
    _finalizePendingDelete();
    List<Todo> orderedParentTodos = [];

    _optimisticUpdate((currentList) {
      final item = currentList.removeAt(oldIndex);
      currentList.insert(newIndex, item);
      orderedParentTodos = currentList.map((ht) => ht.parent.todo).toList();
      return currentList;
    });

    _silentDbUpdate(() => _todoRepository.updateTodoOrder(orderedParentTodos));
  }

  // 타이머 자동 실패 스케줄링
  Future<void> _scheduleAutoFailTimer(int todoId, String? timerEndTime) async {
    await AndroidAlarmManager.cancel(todoId);
    if (timerEndTime != null) {
      final endTime = DateTime.parse(timerEndTime);
      if (endTime.isAfter(DateTime.now())) {
        await AndroidAlarmManager.oneShotAt(endTime, todoId, autoFailTask, exact: true, wakeup: true);
      }
    }
  }

  bool _isPastDate(DateTime date) {
    final now = DateTime.now();
    return date.isBefore(DateTime(now.year, now.month, now.day));
  }

  @override
  void dispose() {
    _subscription?.close();
    _deleteTimer?.cancel();
    super.dispose();
  }
}