// /lib/presentation/screens/home_screen.dart

import 'dart:async';
// min 함수 사용을 위해 추가

import 'package:flutter/foundation.dart' show listEquals;
import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_animate/flutter_animate.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:intl/intl.dart';
import 'package:table_calendar/table_calendar.dart';

import '../../business_logic/providers.dart';
import '../../data/models/category.dart';
import '../../data/models/failure_log.dart';
import '../../data/models/todo.dart';
import 'category_screen.dart';

class TodoWithCategory {
  final Todo todo;
  final Category? category;
  final FailureLog? failureLog;
  TodoWithCategory({required this.todo, this.category, this.failureLog});

  TodoWithCategory copyWith({Todo? todo, Category? category, FailureLog? failureLog}) {
    return TodoWithCategory(
      todo: todo ?? this.todo,
      category: category ?? this.category,
      failureLog: failureLog ?? this.failureLog,
    );
  }

  @override
  bool operator ==(Object other) {
    if (identical(this, other)) return true;
    return other is TodoWithCategory && todo == other.todo && category == other.category;
  }

  @override
  int get hashCode => Object.hash(todo, category);
}

class HierarchicalTodo {
  final TodoWithCategory parent;
  final List<TodoWithCategory> children;
  HierarchicalTodo({required this.parent, required this.children});

  HierarchicalTodo copyWith({TodoWithCategory? parent, List<TodoWithCategory>? children}) {
    return HierarchicalTodo(
      parent: parent ?? this.parent,
      children: children ?? this.children,
    );
  }

  @override
  bool operator ==(Object other) {
    if (identical(this, other)) return true;
    return other is HierarchicalTodo && parent == other.parent && listEquals(children, other.children);
  }

  @override
  int get hashCode => Object.hash(parent, Object.hashAll(children));
}

class DailyStats {
  final int totalTodos;
  final int completedTodos;
  DailyStats({required this.totalTodos, required this.completedTodos});
  double get progress => totalTodos == 0 ? 0 : completedTodos / totalTodos;
}

class HomeScreen extends ConsumerStatefulWidget {
  const HomeScreen({super.key});

  @override
  ConsumerState<HomeScreen> createState() => _HomeScreenState();
}

class _HomeScreenState extends ConsumerState<HomeScreen> {
  final ScrollController _scrollController = ScrollController();
  bool _isCalendarVisible = true;

  // 스크롤 감지를 위한 추가 변수들
  double _scrollVelocity = 0;
  double _lastScrollPosition = 0;
  Timer? _scrollTimer;
  bool _isScrolling = false;

  @override
  void dispose() {
    _scrollController.dispose();
    _scrollTimer?.cancel(); // 타이머 정리
    super.dispose();
  }

  // 캘린더 가시성 업데이트 로직
  void _updateCalendarVisibility(double delta, ScrollMetrics metrics) {
    const double threshold = 8.0; // 임계값을 8픽셀로 설정 (더 안정적)

    // 위로 스크롤 (캘린더 숨기기)
    if (delta < -threshold && _isCalendarVisible) {
      // 스크롤 위치가 어느 정도 내려간 상태에서만 숨기기
      if (metrics.pixels > 30) {
        setState(() => _isCalendarVisible = false);
      }
    }

    // 아래로 스크롤 (캘린더 보이기)
    else if (delta > threshold && !_isCalendarVisible) {
      // 거의 맨 위에 도달했을 때만 보이기
      if (metrics.pixels <= 20) {
        setState(() => _isCalendarVisible = true);
      }
    }
  }

  @override
  Widget build(BuildContext context) {
    final selectedDate = ref.watch(selectedDateProvider);
    final todosAsyncValue = ref.watch(todoListProvider);
    final stats = ref.watch(dailyStatsProvider);
    final isPastDate = selectedDate.isBefore(DateTime(DateTime.now().year, DateTime.now().month, DateTime.now().day));

    return Scaffold(
      floatingActionButton: isPastDate ? null : FloatingActionButton(
          onPressed: () => _showAddTodoBottomSheet(context, ref),
          child: const Icon(Icons.add)
      ),
      body: NotificationListener<ScrollNotification>(
        onNotification: (notification) {
          // 스크롤 시작 감지
          if (notification is ScrollStartNotification) {
            _isScrolling = true;
          }

          // 스크롤 업데이트 감지
          if (notification is ScrollUpdateNotification && notification.dragDetails != null) {
            final delta = notification.dragDetails!.primaryDelta;
            if (delta == null) return false;

            final currentPosition = notification.metrics.pixels;
            _scrollVelocity = currentPosition - _lastScrollPosition;
            _lastScrollPosition = currentPosition;

            // 빠른 스크롤일 때는 즉시 반응 (속도 15픽셀 이상)
            if (_scrollVelocity.abs() > 15) {
              _updateCalendarVisibility(delta, notification.metrics);
            } else {
              // 느린 스크롤일 때는 타이머로 지연 처리
              _scrollTimer?.cancel();
              _scrollTimer = Timer(Duration(milliseconds: 200), () {
                if (!_isScrolling) {
                  _updateCalendarVisibility(delta, notification.metrics);
                }
              });
            }
          }

          // 스크롤 종료 감지
          if (notification is ScrollEndNotification) {
            _isScrolling = false;
            // 스크롤이 끝났을 때 최종 상태 업데이트
            final delta = _scrollVelocity;
            _updateCalendarVisibility(delta, notification.metrics);
          }

          return false;
        },
        child: CustomScrollView(
          physics: const AlwaysScrollableScrollPhysics(),
          controller: _scrollController,
          slivers: [
            SliverAppBar(
              pinned: true,
              floating: true,
              title: const Text('Daily Palette'),
              actions: [
                IconButton(
                  icon: const Icon(Icons.today_outlined),
                  tooltip: '오늘로 이동',
                  onPressed: () {
                    final now = DateTime.now();
                    ref.read(focusedMonthProvider.notifier).state = now;
                    ref.read(selectedDateProvider.notifier).state = now;
                  },
                ),
                IconButton(
                  icon: const Icon(Icons.category_outlined),
                  tooltip: '카테고리 관리',
                  onPressed: () {
                    Navigator.of(context).push(MaterialPageRoute(
                        builder: (context) => const CategoryScreen()
                    ));
                  },
                ),
              ],
            ),
            SliverToBoxAdapter(
              child: AnimatedSize(
                duration: const Duration(milliseconds: 300),
                curve: Curves.easeInOut,
                child: Visibility(
                  visible: _isCalendarVisible,
                  child: TableCalendar(
                    locale: 'ko_KR',
                    focusedDay: ref.watch(focusedMonthProvider),
                    firstDay: DateTime.utc(2020, 1, 1),
                    lastDay: DateTime.utc(2030, 12, 31),
                    headerStyle: const HeaderStyle(
                        formatButtonVisible: false,
                        titleCentered: true
                    ),
                    selectedDayPredicate: (day) => isSameDay(selectedDate, day),
                    onDaySelected: (newSelectedDay, newFocusedDay) {
                      ref.read(focusedMonthProvider.notifier).state = newFocusedDay;
                      ref.read(selectedDateProvider.notifier).state = newSelectedDay;
                    },
                    onPageChanged: (newFocusedDay) {
                      ref.read(focusedMonthProvider.notifier).state = newFocusedDay;
                    },
                    onHeaderTapped: (focusedDay) async {
                      final pickedDate = await showDatePicker(
                        context: context,
                        initialDate: focusedDay,
                        firstDate: DateTime.utc(2020, 1, 1),
                        lastDate: DateTime.utc(2030, 12, 31),
                        initialDatePickerMode: DatePickerMode.year,
                        helpText: '날짜로 이동',
                      );
                      if (pickedDate != null) {
                        ref.read(focusedMonthProvider.notifier).state = pickedDate;
                        ref.read(selectedDateProvider.notifier).state = pickedDate;
                      }
                    },
                    calendarBuilders: CalendarBuilders(
                      defaultBuilder: (context, day, focusedDay) {
                        final monthlyStatus = ref.watch(monthlyStatusProvider);
                        Color? color;
                        if (monthlyStatus.hasValue) {
                          final statusMap = monthlyStatus.value!;
                          final dayWithoutTime = DateTime.utc(day.year, day.month, day.day);
                          final status = statusMap[dayWithoutTime] ?? DayStatus.none;
                          switch (status) {
                            case DayStatus.success:
                              color = Colors.green.withAlpha(102);
                              break;
                            case DayStatus.failure:
                              color = Colors.red.withAlpha(102);
                              break;
                            case DayStatus.mixed:
                              color = Colors.yellow.withAlpha(128);
                              break;
                            case DayStatus.none:
                              color = Colors.transparent;
                          }
                        } else {
                          color = Colors.transparent;
                        }
                        return Container(
                          margin: const EdgeInsets.all(4.0),
                          decoration: BoxDecoration(
                              color: color,
                              shape: BoxShape.circle
                          ),
                          child: Center(
                              child: Text(
                                  '${day.day}',
                                  style: const TextStyle()
                              )
                          ),
                        );
                      },
                    ),
                  ),
                ),
              ),
            ),
            // 진행률 바
            SliverPersistentHeader(
              pinned: true,
              delegate: _ProgressBarHeaderDelegate(
                stats: stats,
                height: 56.0,
              ),
            ),
            // Todo 리스트
            if (todosAsyncValue.hasValue)
              ...[
                if (todosAsyncValue.value!.isEmpty)
                  SliverFillRemaining(
                      hasScrollBody: false,
                      child: _EmptyState(displayDate: selectedDate)
                  )
                else
                  SliverReorderableList(
                    itemCount: todosAsyncValue.value!.length,
                    onReorder: (int oldIndex, int newIndex) {
                      if (isPastDate) return;
                      final todos = todosAsyncValue.value!;
                      final activeTodoCount = todos.where((t) => t.parent.todo.status != 'completed').length;
                      if (oldIndex >= activeTodoCount || newIndex >= activeTodoCount) return;
                      if (oldIndex < newIndex) newIndex -= 1;
                      ref.read(todoListProvider.notifier).reorderTodos(oldIndex, newIndex);
                    },
                    itemBuilder: (context, index) {
                      final todos = todosAsyncValue.value!;
                      return _TodoItem(
                        key: ValueKey(todos[index].parent.todo.id!),
                        hierarchicalTodo: todos[index],
                        isPastDate: isPastDate,
                      );
                    },
                  )
              ]
            else
              todosAsyncValue.when(
                data: (_) => const SliverToBoxAdapter(),
                loading: () => const SliverFillRemaining(
                    hasScrollBody: false,
                    child: Center(child: CircularProgressIndicator())
                ),
                error: (err, stack) => SliverFillRemaining(
                    hasScrollBody: false,
                    child: Center(
                        child: Text('할 일을 불러오는 중 에러가 발생했습니다: $err')
                    )
                ),
              ),
            const SliverToBoxAdapter(
              child: SizedBox(height: 80),
            ),
          ],
        ),
      ),
    );
  }
}

class _TodoItem extends ConsumerStatefulWidget {
  final HierarchicalTodo hierarchicalTodo;
  final bool isPastDate;
  const _TodoItem({super.key, required this.hierarchicalTodo, required this.isPastDate});

  @override
  ConsumerState<_TodoItem> createState() => _TodoItemState();
}

class _TodoItemState extends ConsumerState<_TodoItem> with SingleTickerProviderStateMixin {
  bool _isExpanded = false;

  Color _getCategoryColor(Category? category) {
    if (category == null) return Colors.grey.shade300;
    return Color(int.parse(category.colorCode, radix: 16));
  }

  String _formatDuration(Duration duration) {
    String twoDigits(int n) => n.toString().padLeft(2, '0');
    final hours = twoDigits(duration.inHours);
    final minutes = twoDigits(duration.inMinutes.remainder(60));
    final seconds = twoDigits(duration.inSeconds.remainder(60));
    return [if (duration.inHours > 0) hours, minutes, seconds].join(':');
  }

  @override
  Widget build(BuildContext context) {
    final parentItem = widget.hierarchicalTodo.parent;
    final children = widget.hierarchicalTodo.children;
    final bool isParentFailed = parentItem.todo.status == 'failed';
    final bool isParentCompleted = parentItem.todo.status == 'completed';
    final bool isPastDate = widget.isPastDate;
    final bool isTimerActive = parentItem.todo.timerEndTime != null && DateTime.parse(parentItem.todo.timerEndTime!).isAfter(DateTime.now());

    return ReorderableDelayedDragStartListener(
      index: isParentCompleted ? -1 : 0,
      enabled: !isPastDate && !isParentCompleted && !isParentFailed,
      child: Dismissible(
        key: ValueKey('dismiss-${parentItem.todo.id!}'),
        direction: (isPastDate || isParentCompleted || isParentFailed) ? DismissDirection.none : DismissDirection.endToStart,
        onDismissed: (direction) {
          final notifier = ref.read(todoListProvider.notifier);
          notifier.deleteTodo(parentItem.todo.id!);
          ScaffoldMessenger.of(context).hideCurrentSnackBar();
          ScaffoldMessenger.of(context).showSnackBar(SnackBar(
            content: const Text('할 일을 삭제했습니다.'),
            behavior: SnackBarBehavior.floating,
            action: SnackBarAction(label: '실행 취소', onPressed: () => notifier.undoDelete()),
          ));
        },
        background: Container(
          color: Colors.red,
          alignment: Alignment.centerRight,
          padding: const EdgeInsets.symmetric(horizontal: 20.0),
          child: const Icon(Icons.delete, color: Colors.white),
        ),
        child: Card(
          margin: const EdgeInsets.symmetric(horizontal: 8, vertical: 4),
          child: Column(
            children: [
              ListTile(
                contentPadding: const EdgeInsets.only(left: 12, right: 16.0),
                leading: Row(
                  mainAxisSize: MainAxisSize.min,
                  children: [
                    Container(width: 6, height: 24, decoration: BoxDecoration(color: _getCategoryColor(parentItem.category), borderRadius: BorderRadius.circular(4))),
                    const SizedBox(width: 10),
                    Container(
                      width: 24,
                      height: 24,
                      alignment: Alignment.center,
                      child: isParentFailed
                          ? const Icon(Icons.cancel_outlined, color: Colors.red)
                          : Checkbox(
                        visualDensity: VisualDensity.compact,
                        value: isParentCompleted,
                        onChanged: (children.isNotEmpty || isParentCompleted)
                            ? null
                            : (value) {
                          if (value == true) HapticFeedback.mediumImpact();
                          ref.read(todoListProvider.notifier).updateTodoStatus(parentItem.todo.id!, 'completed');
                        },
                      ),
                    ),
                  ],
                ),
                title: Row(
                  children: [
                    if (parentItem.todo.priority == 1) const Icon(Icons.looks_one, color: Colors.blueAccent, size: 18),
                    if (parentItem.todo.priority == 2) const Icon(Icons.looks_two, color: Colors.blueAccent, size: 18),
                    if (parentItem.todo.priority == 3) const Icon(Icons.looks_3, color: Colors.blueAccent, size: 18),
                    if (parentItem.todo.priority != 4) const SizedBox(width: 4),
                    Expanded(
                      child: Text(
                        parentItem.todo.title,
                        overflow: TextOverflow.ellipsis,
                        style: TextStyle(decoration: isParentCompleted || isParentFailed ? TextDecoration.lineThrough : null, color: isParentCompleted || isParentFailed ? Colors.grey.shade600 : null),
                      ),
                    ),
                  ],
                ),
                trailing: Row(
                  mainAxisSize: MainAxisSize.min,
                  children: [
                    if (children.isNotEmpty) IconButton(icon: Icon(_isExpanded ? Icons.expand_less : Icons.expand_more), onPressed: () => setState(() => _isExpanded = !_isExpanded)),
                    if (parentItem.failureLog != null) IconButton(icon: const Icon(Icons.info_outline), tooltip: '실패 사유 보기', onPressed: () => _showFailureReasonDialog(context, parentItem.failureLog!)),
                    if (!isParentCompleted && !isParentFailed && !isPastDate)
                      PopupMenuButton<String>(
                        onSelected: (value) {
                          if (value == 'edit') {
                            _showAddTodoBottomSheet(context, ref, todoToEdit: parentItem);
                          } else if (value == 'addSubTask') {
                            _showAddSubTaskDialog(context, ref, parentItem.todo.id!);
                          } else if (value == 'fail') {
                            _showFailureLogDialog(context, ref, parentItem.todo);
                          }
                        },
                        itemBuilder: (context) => [
                          const PopupMenuItem(value: 'edit', child: Text('수정하기')),
                          const PopupMenuItem(value: 'addSubTask', child: Text('하위 작업 추가')),
                          const PopupMenuItem(value: 'fail', child: Text('실패로 표시')),
                        ],
                      ),
                  ],
                ),
                subtitle: (isTimerActive || parentItem.todo.timeSpentSeconds > 0)
                    ? Padding(
                  padding: const EdgeInsets.only(top: 4.0),
                  child: Row(
                    children: [
                      Icon(Icons.timer_outlined, size: 14, color: Colors.grey.shade600),
                      const SizedBox(width: 4),
                      if (isTimerActive)
                        _LiveTimerText(
                          targetTime: DateTime.parse(parentItem.todo.timerEndTime!),
                          onTimerExpired: () {
                            if (mounted) {
                              ref.read(todoListProvider.notifier).updateTodoStatus(
                                parentItem.todo.id!,
                                'failed',
                                reason: '타이머 시간이 만료되어 자동 실패 처리됨',
                              );
                            }
                          },
                        )
                      else
                        Text("총 소요 시간: ${_formatDuration(Duration(seconds: parentItem.todo.timeSpentSeconds))}", style: TextStyle(color: Colors.grey.shade700)),
                    ],
                  ),
                )
                    : null,
              ),
              Visibility(
                visible: _isExpanded,
                child: Column(
                  children: children.map((childItem) {
                    final bool isChildFailed = childItem.todo.status == 'failed';
                    final bool isChildCompleted = childItem.todo.status == 'completed';
                    return Dismissible(
                      key: ValueKey('dismiss-${childItem.todo.id!}'),
                      direction: (isPastDate || isParentCompleted) ? DismissDirection.none : DismissDirection.endToStart,
                      onDismissed: (direction) {
                        final notifier = ref.read(todoListProvider.notifier);
                        notifier.deleteTodo(childItem.todo.id!);
                        ScaffoldMessenger.of(context).hideCurrentSnackBar();
                        ScaffoldMessenger.of(context).showSnackBar(SnackBar(
                          content: const Text('하위 작업을 삭제했습니다.'),
                          behavior: SnackBarBehavior.floating,
                          action: SnackBarAction(label: '실행 취소', onPressed: () => notifier.undoDelete()),
                        ));
                      },
                      background: Container(color: Colors.red.withAlpha(204), alignment: Alignment.centerRight, padding: const EdgeInsets.symmetric(horizontal: 20.0), child: const Icon(Icons.delete, color: Colors.white)),
                      child: ListTile(
                        contentPadding: const EdgeInsets.only(left: 30, right: 16),
                        leading: Row(
                          mainAxisSize: MainAxisSize.min,
                          children: [
                            Container(width: 6, height: 24, decoration: BoxDecoration(color: _getCategoryColor(childItem.category).withAlpha(128), borderRadius: BorderRadius.circular(4))),
                            const SizedBox(width: 10),
                            Container(
                              width: 24,
                              height: 24,
                              alignment: Alignment.center,
                              child: isChildFailed
                                  ? const Icon(Icons.cancel_outlined, color: Colors.red, size: 24)
                                  : Checkbox(
                                visualDensity: VisualDensity.compact,
                                value: isChildCompleted,
                                onChanged: (isParentCompleted || isPastDate || isChildCompleted)
                                    ? null
                                    : (value) {
                                  if (value == true) HapticFeedback.mediumImpact();
                                  ref.read(todoListProvider.notifier).updateTodoStatus(childItem.todo.id!, 'completed');
                                },
                              ),
                            ),
                          ],
                        ),
                        title: Text(
                          childItem.todo.title,
                          style: TextStyle(decoration: isChildCompleted || isChildFailed ? TextDecoration.lineThrough : null, color: isParentCompleted || isChildFailed ? Colors.grey.shade600 : null),
                        ),
                        trailing: (isParentCompleted || isChildFailed || isPastDate)
                            ? null
                            : PopupMenuButton<String>(
                          onSelected: (value) {
                            if (value == 'edit') _showEditSubTaskDialog(context, ref, childItem.todo);
                          },
                          itemBuilder: (context) => [const PopupMenuItem(value: 'edit', child: Text('수정'))],
                        ),
                      ),
                    );
                  }).toList(),
                ),
              )
            ],
          ),
        ),
      ),
    ).animate().fadeIn(duration: 400.ms).slideY(begin: 0.5);
  }
}

class _LiveTimerText extends StatefulWidget {
  final DateTime targetTime;
  final VoidCallback onTimerExpired;

  const _LiveTimerText({required this.targetTime, required this.onTimerExpired});

  @override
  State<_LiveTimerText> createState() => _LiveTimerTextState();
}

class _LiveTimerTextState extends State<_LiveTimerText> {
  late Timer _timer;
  Duration _remaining = Duration.zero;

  @override
  void initState() {
    super.initState();
    _updateRemainingTime(isInitialCall: true);

    _timer = Timer.periodic(const Duration(seconds: 1), (timer) {
      _updateRemainingTime();
    });
  }

  void _updateRemainingTime({bool isInitialCall = false}) {
    if (!mounted) return;

    final now = DateTime.now();
    final newRemaining = widget.targetTime.isAfter(now) ? widget.targetTime.difference(now) : Duration.zero;

    if (!isInitialCall && _remaining > Duration.zero && newRemaining == Duration.zero) {
      widget.onTimerExpired();
    }

    setState(() {
      _remaining = newRemaining;
    });

    if (newRemaining == Duration.zero) {
      _timer.cancel();
    }
  }

  @override
  void dispose() {
    _timer.cancel();
    super.dispose();
  }

  String _formatDuration(Duration duration) {
    String twoDigits(int n) => n.toString().padLeft(2, '0');
    final hours = twoDigits(duration.inHours);
    final minutes = twoDigits(duration.inMinutes.remainder(60));
    final seconds = twoDigits(duration.inSeconds.remainder(60));
    return [if (duration.inHours > 0) hours, minutes, seconds].join(':');
  }

  @override
  Widget build(BuildContext context) {
    if (_remaining == Duration.zero) {
      return Text('시간 만료', style: TextStyle(color: Colors.red.shade700, fontWeight: FontWeight.bold));
    }
    return Row(
      mainAxisSize: MainAxisSize.min,
      children: [
        Icon(Icons.timer, size: 14, color: Theme.of(context).primaryColor),
        const SizedBox(width: 4),
        Text(
          '${_formatDuration(_remaining)} 남음',
          style: TextStyle(color: Theme.of(context).primaryColor, fontWeight: FontWeight.bold),
        ),
      ],
    );
  }
}

void _showAddTodoBottomSheet(BuildContext context, WidgetRef ref, {TodoWithCategory? todoToEdit}) {
  showModalBottomSheet(
    context: context,
    isScrollControlled: true,
    useSafeArea: true,
    shape: const RoundedRectangleBorder(
      borderRadius: BorderRadius.vertical(top: Radius.circular(20)),
    ),
    builder: (context) {
      return Consumer(
        builder: (context, ref, child) {
          final categoriesAsync = ref.watch(categoryListProvider);

          return categoriesAsync.when(
            loading: () => const Padding(
              padding: EdgeInsets.all(48.0),
              child: Center(child: CircularProgressIndicator()),
            ),
            error: (err, stack) => Padding(
              padding: const EdgeInsets.all(32.0),
              child: Center(child: Text('카테고리를 불러오는 중 오류가 발생했습니다.\n$err')),
            ),
            data: (categoryList) {
              if (categoryList.isEmpty) {
                return Padding(
                  padding: EdgeInsets.only(bottom: MediaQuery.of(context).padding.bottom),
                  child: _CategoryCreationPrompt(),
                );
              }

              return _TodoForm(
                categoryList: categoryList,
                todoToEdit: todoToEdit,
              );
            },
          );
        },
      );
    },
  );
}

class _CategoryCreationPrompt extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return Container(
      padding: const EdgeInsets.all(32.0),
      child: Column(
        mainAxisSize: MainAxisSize.min,
        mainAxisAlignment: MainAxisAlignment.center,
        children: [
          const Icon(Icons.warning_amber_rounded, size: 50, color: Colors.orange),
          const SizedBox(height: 16),
          const Text(
            '카테고리가 필요해요!',
            style: TextStyle(fontSize: 18, fontWeight: FontWeight.bold),
            textAlign: TextAlign.center,
          ),
          const SizedBox(height: 8),
          const Text(
            '할 일을 추가하려면 먼저 카테고리를\n하나 이상 만들어야 합니다.',
            textAlign: TextAlign.center,
          ),
          const SizedBox(height: 24),
          ElevatedButton.icon(
            icon: const Icon(Icons.add_circle_outline),
            label: const Text('카테고리 만들러 가기'),
            onPressed: () {
              Navigator.of(context).pop();
              Navigator.of(context).push(MaterialPageRoute(builder: (context) => const CategoryScreen()));
            },
          ),
        ],
      ),
    );
  }
}

class _TodoForm extends ConsumerStatefulWidget {
  final List<Category> categoryList;
  final TodoWithCategory? todoToEdit;

  const _TodoForm({required this.categoryList, this.todoToEdit});

  @override
  ConsumerState<_TodoForm> createState() => __TodoFormState();
}

class __TodoFormState extends ConsumerState<_TodoForm> {
  late final TextEditingController titleController;
  late final TextEditingController customTimerController;
  final formKey = GlobalKey<FormState>();

  TimeOfDay? selectedTime;
  late int selectedPriority;
  int? selectedTimerDuration;
  bool isCustomDuration = false;
  Category? selectedCategory;

  @override
  void initState() {
    super.initState();

    final todo = widget.todoToEdit?.todo;

    titleController = TextEditingController(text: todo?.title ?? '');
    customTimerController = TextEditingController();

    if (todo?.targetTime != null) {
      final timeParts = todo!.targetTime!.split(':');
      selectedTime = TimeOfDay(hour: int.parse(timeParts[0]), minute: int.parse(timeParts[1]));
    }

    selectedPriority = todo?.priority ?? 4;
    selectedTimerDuration = todo?.timerDurationMinutes;
    isCustomDuration = ![5, 10, 15, 30, 60].contains(selectedTimerDuration) && selectedTimerDuration != null;
    if (isCustomDuration) {
      customTimerController.text = selectedTimerDuration.toString();
    }

    if (widget.todoToEdit != null) {
      selectedCategory = widget.todoToEdit!.category;
    } else if (widget.categoryList.isNotEmpty) {
      selectedCategory = widget.categoryList.first;
    }
  }

  @override
  void dispose() {
    titleController.dispose();
    customTimerController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    final selectedDate = ref.watch(selectedDateProvider);
    final isToday = isSameDay(selectedDate, DateTime.now());
    final isEditMode = widget.todoToEdit != null;

    final String timeText = selectedTime?.format(context) ?? '알림 시간 설정';
    final bool isAlarmSet = selectedTime != null;
    final bool isTimerSet = selectedTimerDuration != null;

    return Padding(
      padding: EdgeInsets.fromLTRB(20, 20, 20, MediaQuery.of(context).viewInsets.bottom + MediaQuery.of(context).padding.bottom),
      child: SingleChildScrollView(
        child: Form(
          key: formKey,
          child: Column(mainAxisSize: MainAxisSize.min, crossAxisAlignment: CrossAxisAlignment.start, children: [
            Center(child: Text(isEditMode ? '할 일 수정' : '새로운 할 일 추가', style: const TextStyle(fontSize: 18, fontWeight: FontWeight.bold))),
            const SizedBox(height: 16),
            TextFormField(
              controller: titleController,
              decoration: const InputDecoration(labelText: '할 일 내용'),
              validator: (v) => (v == null || v.isEmpty) ? "내용을 입력해주세요." : null,
            ),
            const SizedBox(height: 16),
            DropdownButtonFormField<Category>(
              value: selectedCategory,
              decoration: const InputDecoration(labelText: '카테고리'),
              items: widget.categoryList.map((c) => DropdownMenuItem(value: c, child: Text(c.name))).toList(),
              onChanged: (value) => setState(() => selectedCategory = value),
              validator: (value) => value == null ? '카테고리를 선택해주세요' : null,
            ),
            ListTile(
              contentPadding: EdgeInsets.zero,
              enabled: !isTimerSet,
              leading: Icon(Icons.notifications_outlined, color: !isTimerSet ? null : Colors.grey),
              title: Text(timeText, style: TextStyle(color: !isTimerSet ? null : Colors.grey)),
              subtitle: isTimerSet ? const Text('타이머와 중복 설정할 수 없습니다.', style: TextStyle(color: Colors.grey, fontSize: 12)) : null,
              trailing: isAlarmSet ? IconButton(icon: const Icon(Icons.clear), tooltip: '알림 삭제', onPressed: () => setState(() => selectedTime = null)) : null,
              onTap: isTimerSet
                  ? null
                  : () async {
                final time = await showTimePicker(context: context, initialTime: selectedTime ?? TimeOfDay.now());
                if (time != null) setState(() => selectedTime = time);
              },
            ),
            const Divider(),
            const Text('중요도', style: TextStyle(fontWeight: FontWeight.bold)),
            const SizedBox(height: 8),
            Row(mainAxisAlignment: MainAxisAlignment.spaceAround, children: [
              ChoiceChip(label: const Text('없음'), selected: selectedPriority == 4, onSelected: (selected) => setState(() => selectedPriority = 4)),
              ChoiceChip(label: const Text('낮음'), selected: selectedPriority == 3, onSelected: (selected) => setState(() => selectedPriority = 3)),
              ChoiceChip(label: const Text('중간'), selected: selectedPriority == 2, onSelected: (selected) => setState(() => selectedPriority = 2)),
              ChoiceChip(label: const Text('높음'), selected: selectedPriority == 1, onSelected: (selected) => setState(() => selectedPriority = 1)),
            ]),
            if (isToday) ...[
              const Divider(),
              Text('타이머 (자동 실패)', style: TextStyle(color: isAlarmSet ? Colors.grey : null)),
              const SizedBox(height: 8),
              Opacity(
                opacity: isAlarmSet ? 0.5 : 1.0,
                child: AbsorbPointer(
                  absorbing: isAlarmSet,
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      if (isAlarmSet) const Text('수동 알람과 중복 설정할 수 없습니다.', style: TextStyle(color: Colors.grey, fontSize: 12)),
                      Wrap(
                        spacing: 8.0,
                        children: [
                          ...[5, 10, 15, 30, 60].map((duration) => ChoiceChip(
                            label: Text('$duration분'),
                            selected: !isCustomDuration && selectedTimerDuration == duration,
                            onSelected: (selected) {
                              setState(() {
                                isCustomDuration = false;
                                selectedTimerDuration = selected ? duration : null;
                                if (selected) customTimerController.clear();
                              });
                            },
                          )),
                          ChoiceChip(
                            label: const Text('직접 입력'),
                            selected: isCustomDuration,
                            onSelected: (selected) {
                              setState(() {
                                isCustomDuration = selected;
                                if(selected) {
                                  selectedTimerDuration = int.tryParse(customTimerController.text);
                                } else {
                                  customTimerController.clear();
                                }
                              });
                            },
                          ),
                        ],
                      ),
                      if(isCustomDuration)
                        Padding(
                          padding: const EdgeInsets.only(top: 8.0),
                          child: TextFormField(
                            controller: customTimerController,
                            keyboardType: TextInputType.number,
                            decoration: const InputDecoration(labelText: '시간 입력 (분)', suffixText: '분'),
                            validator: (value) {
                              if(!isCustomDuration) return null;
                              if (value == null || value.isEmpty) return '시간을 입력해주세요.';
                              final number = int.tryParse(value);
                              if (number == null) return '숫자만 입력 가능합니다.';
                              if (number <= 0) return '0보다 큰 값을 입력해주세요.';
                              return null;
                            },
                            onChanged: (value) {
                              setState(() {
                                selectedTimerDuration = int.tryParse(value);
                              });
                            },
                          ),
                        ),
                    ],
                  ),
                ),
              ),
            ],
            const SizedBox(height: 20),
            ElevatedButton(
              style: ElevatedButton.styleFrom(minimumSize: const Size(double.infinity, 50)),
              onPressed: () {
                if (formKey.currentState?.validate() ?? false) {
                  if (selectedCategory == null) {
                    ScaffoldMessenger.of(context).showSnackBar(const SnackBar(content: Text('카테고리를 먼저 생성하고 선택해주세요.')));
                    return;
                  }

                  int? finalTimerDuration = selectedTimerDuration;
                  if (isToday && finalTimerDuration != null) {
                    final now = DateTime.now();
                    final endOfToday = DateTime(now.year, now.month, now.day, 23, 59, 59);
                    final potentialEndTime = now.add(Duration(minutes: finalTimerDuration));

                    if (potentialEndTime.isAfter(endOfToday)) {
                      final newDuration = endOfToday.difference(now).inMinutes;
                      finalTimerDuration = (newDuration > 0) ? newDuration : 0;
                      ScaffoldMessenger.of(context).showSnackBar(SnackBar(content: Text('타이머가 오늘을 넘지 않도록 $finalTimerDuration분으로 자동 조정되었습니다.')));
                    }
                  }

                  final notifier = ref.read(todoListProvider.notifier);
                  if (isEditMode) {
                    notifier.updateTodo(todoId: widget.todoToEdit!.todo.id!, newTitle: titleController.text, newCategoryId: selectedCategory!.id!, newPriority: selectedPriority, newTime: selectedTime, newTimerDuration: finalTimerDuration);
                  } else {
                    notifier.addTodo(titleController.text, selectedCategory!.id!, time: selectedTime, priority: selectedPriority, timerDuration: finalTimerDuration);
                  }
                  Navigator.of(context).pop();
                }
              },
              child: const Text('저장'),
            ),
          ]),
        ),
      ),
    );
  }
}

void _showFailureLogDialog(BuildContext context, WidgetRef ref, Todo todo) {
  final TextEditingController reasonController = TextEditingController();
  showDialog(
      context: context,
      builder: (context) => AlertDialog(
        title: Text(todo.title),
        content: TextField(controller: reasonController, decoration: const InputDecoration(labelText: '실패 사유'), autofocus: true),
        actions: [
          TextButton(onPressed: () => Navigator.of(context).pop(), child: const Text('취소')),
          TextButton(
              onPressed: () {
                ref.read(todoListProvider.notifier).updateTodoStatus(todo.id!, 'failed', reason: reasonController.text);
                Navigator.of(context).pop();
              },
              child: const Text('저장')),
        ],
      ));
}

void _showAddSubTaskDialog(BuildContext context, WidgetRef ref, int parentId) {
  final TextEditingController titleController = TextEditingController();
  showDialog(
      context: context,
      builder: (context) => AlertDialog(
        title: const Text('하위 작업 추가'),
        content: TextField(controller: titleController, autofocus: true, decoration: const InputDecoration(labelText: '하위 작업 내용')),
        actions: [
          TextButton(onPressed: () => Navigator.of(context).pop(), child: const Text('취소')),
          TextButton(
              onPressed: () {
                if (titleController.text.isNotEmpty) {
                  ref.read(todoListProvider.notifier).addSubTask(titleController.text, parentId);
                  Navigator.of(context).pop();
                }
              },
              child: const Text('추가')),
        ],
      ));
}

void _showEditSubTaskDialog(BuildContext context, WidgetRef ref, Todo todo) {
  final TextEditingController titleController = TextEditingController(text: todo.title);
  showDialog(
    context: context,
    builder: (context) => AlertDialog(
      title: const Text('하위 작업 수정'),
      content: TextField(controller: titleController, autofocus: true, decoration: const InputDecoration(labelText: '하위 작업 내용')),
      actions: [
        TextButton(onPressed: () => Navigator.of(context).pop(), child: const Text('취소')),
        TextButton(
            onPressed: () {
              if (titleController.text.isNotEmpty) {
                ref.read(todoListProvider.notifier).updateTodoTitle(todo.id!, titleController.text);
                Navigator.of(context).pop();
              }
            },
            child: const Text('저장')),
      ],
    ),
  );
}

class _EmptyState extends ConsumerWidget {
  final DateTime displayDate;
  const _EmptyState({required this.displayDate});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final now = DateTime.now();
    final today = DateTime(now.year, now.month, now.day);
    final selectedDay = DateTime(displayDate.year, displayDate.month, displayDate.day);
    IconData icon;
    String message;
    bool showAddButton;
    if (selectedDay.isBefore(today)) {
      icon = Icons.history_toggle_off_outlined;
      message = '이 날에는 등록된 할 일이 없었어요.';
      showAddButton = false;
    } else if (selectedDay.isAtSameMomentAs(today)) {
      icon = Icons.check_circle_outline;
      message = '오늘의 모든 할 일을 마쳤거나\n아직 할 일이 없어요!';
      showAddButton = true;
    } else {
      icon = Icons.edit_calendar_outlined;
      message = '이 날의 할 일을 추가해보세요!';
      showAddButton = true;
    }
    return Center(
      child: Column(
        mainAxisAlignment: MainAxisAlignment.center,
        children: [
          Icon(icon, size: 80, color: Colors.grey.shade300),
          const SizedBox(height: 16),
          Text(message, textAlign: TextAlign.center, style: TextStyle(fontSize: 16, color: Colors.grey.shade500)),
          if (showAddButton)
            Padding(
              padding: const EdgeInsets.only(top: 24.0),
              child: ElevatedButton.icon(
                icon: const Icon(Icons.add),
                label: const Text('새 할 일 추가하기'),
                onPressed: () {
                  _showAddTodoBottomSheet(context, ref);
                },
              ),
            ),
        ],
      ),
    );
  }
}

void _showFailureReasonDialog(BuildContext context, FailureLog log) {
  showDialog(
    context: context,
    builder: (context) => AlertDialog(
      title: const Text('실패 사유'),
      content: Column(
        mainAxisSize: MainAxisSize.min,
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Text(log.reason),
          const SizedBox(height: 16),
          Text(
            '기록 시간: ${DateFormat('yyyy-MM-dd HH:mm').format(DateTime.parse(log.loggedAt))}',
            style: Theme.of(context).textTheme.bodySmall,
          ),
        ],
      ),
      actions: [
        TextButton(onPressed: () => Navigator.of(context).pop(), child: const Text('닫기')),
      ],
    ),
  );
}

bool isSameDay(DateTime? a, DateTime? b) {
  if (a == null || b == null) {
    return false;
  }
  return a.year == b.year && a.month == b.month && a.day == b.day;
}


// ================== [ 추가된 클래스 ] ==================
// SliverPersistentHeader의 동작을 정의하는 Delegate 클래스
class _ProgressBarHeaderDelegate extends SliverPersistentHeaderDelegate {
  final DailyStats stats;
  final double height;

  _ProgressBarHeaderDelegate({required this.stats, required this.height});

  @override
  Widget build(BuildContext context, double shrinkOffset, bool overlapsContent) {
    return Container(
      // 위젯의 배경색과 경계선 설정
      decoration: BoxDecoration(
        color: Theme.of(context).scaffoldBackgroundColor,
        border: Border(
          bottom: BorderSide(
            color: Theme.of(context).dividerColor,
            width: 1,
          ),
        ),
      ),
      padding: const EdgeInsets.symmetric(horizontal: 16.0),
      // Column을 Center로 감싸서 세로 중앙 정렬
      child: Center(
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Text('달성률: ${(stats.progress * 100).toStringAsFixed(0)}% (${stats.completedTodos}/${stats.totalTodos})', style: const TextStyle(fontWeight: FontWeight.bold)),
            const SizedBox(height: 8),
            LinearProgressIndicator(value: stats.progress, minHeight: 10, borderRadius: BorderRadius.circular(5)),
          ],
        ),
      ),
    );
  }

  // 헤더의 최소 높이
  @override
  double get minExtent => height;

  // 헤더의 최대 높이
  @override
  double get maxExtent => height;

  // 헤더를 다시 빌드해야 할지 결정하는 로직
  @override
  bool shouldRebuild(covariant _ProgressBarHeaderDelegate oldDelegate) {
    return stats.totalTodos != oldDelegate.stats.totalTodos ||
        stats.completedTodos != oldDelegate.stats.completedTodos ||
        height != oldDelegate.height;
  }
}