// /lib/presentation/screens/habit_tracker_screen.dart

import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:intl/intl.dart';
import 'dart:math';
import '../../business_logic/providers.dart'; // 이 부분은 실제 프로젝트에 맞게 수정해주세요.

class HabitTrackerScreen extends ConsumerStatefulWidget {
  final int recurringTaskId;
  final String taskTitle;

  const HabitTrackerScreen({
    super.key,
    required this.recurringTaskId,
    required this.taskTitle,
  });

  @override
  ConsumerState<HabitTrackerScreen> createState() => _HabitTrackerScreenState();
}

class _HabitTrackerScreenState extends ConsumerState<HabitTrackerScreen> {
  @override
  void initState() {
    super.initState();
    SystemChrome.setPreferredOrientations([
      DeviceOrientation.landscapeRight,
      DeviceOrientation.landscapeLeft,
    ]);
  }

  @override
  void dispose() {
    SystemChrome.setPreferredOrientations([
      DeviceOrientation.portraitUp,
      DeviceOrientation.portraitDown,
    ]);
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    final habitData = ref.watch(habitTrackerProvider(widget.recurringTaskId));
    final currentYear = DateTime.now().year;

    // ================== [ 핵심 수정 부분 ] ==================
    // Padding의 bottom 값에 viewPadding.bottom을 더해 시스템 네비게이션 바에 UI가 가려지는 것을 방지합니다.
    // Z Flip과 같은 기기에서 하단 영역이 잘리는 문제를 해결할 수 있습니다.
    return Scaffold(
      appBar: AppBar(
        title: Text(widget.taskTitle),
      ),
      body: Padding(
        padding: EdgeInsets.fromLTRB(24, 16, 24, 16 + MediaQuery.of(context).viewPadding.bottom),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Text(
              '$currentYear년 활동 기록',
              style: const TextStyle(fontSize: 22, fontWeight: FontWeight.bold),
            ),
            const SizedBox(height: 16),
            Expanded(
              child: habitData.when(
                data: (data) {
                  return data.isEmpty
                      ? const Center(child: Text('아직 완료 기록이 없습니다.'))
                      : _HeatmapLayout(datasets: data);
                },
                loading: () => const Center(child: CircularProgressIndicator()),
                error: (e, st) => Center(child: Text('오류: $e')),
              ),
            ),
          ],
        ),
      ),
    );
    // ========================================================
  }
}

// ...이하 코드는 기존과 동일...
class _HeatmapLayout extends StatelessWidget {
  final Map<DateTime, int> datasets;

  const _HeatmapLayout({required this.datasets});

  @override
  Widget build(BuildContext context) {
    final today = DateTime.now();
    final currentYear = today.year;
    final firstDayOfYear = DateTime(currentYear, 1, 1);

    final firstDayWeekday = firstDayOfYear.weekday;
    final startDate = firstDayOfYear.subtract(Duration(days: firstDayWeekday % 7));

    return LayoutBuilder(builder: (context, constraints) {
      const double monthHeaderHeight = 20.0;
      const double legendHeight = 20.0;
      const double verticalPadding = 12.0;
      const double legendTopPadding = 16.0;
      const double weekDayHeaderWidth = 35.0;
      const double horizontalGridPadding = 10.0;
      const double cellSpacing = 4.0;
      const int rowCount = 7;
      const int columnCount = 53;

      final double availableHeight = constraints.maxHeight;
      final double availableGridHeight = availableHeight - monthHeaderHeight - verticalPadding - legendHeight - legendTopPadding;

      if (availableGridHeight <= 0) {
        return const Center(child: Text("세로 공간이 부족합니다."));
      }

      final double cellHeight = (availableGridHeight - (rowCount - 1) * cellSpacing) / rowCount;
      if (cellHeight < 2) {
        return const Center(child: Text("화면 공간이 부족합니다."));
      }
      final double cellWidth = cellHeight;

      final double totalGridWidth = columnCount * cellWidth + (columnCount - 1) * cellSpacing;
      final double totalWidgetWidth = weekDayHeaderWidth + horizontalGridPadding + totalGridWidth;

      final Widget heatmapContent = Column(
        mainAxisSize: MainAxisSize.min,
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          SizedBox(
            width: totalWidgetWidth,
            height: monthHeaderHeight + verticalPadding + availableGridHeight,
            child: Stack(
              children: [
                Positioned(
                  top: 0,
                  left: weekDayHeaderWidth + horizontalGridPadding,
                  width: totalGridWidth,
                  height: monthHeaderHeight,
                  child: _buildMonthHeaders(context, startDate, currentYear, cellWidth, cellSpacing, columnCount),
                ),
                Positioned(
                  top: monthHeaderHeight + verticalPadding,
                  left: 0,
                  width: weekDayHeaderWidth,
                  height: availableGridHeight,
                  child: _buildWeekDayHeaders(context, cellHeight),
                ),
                Positioned(
                  top: monthHeaderHeight + verticalPadding,
                  left: weekDayHeaderWidth + horizontalGridPadding,
                  width: totalGridWidth,
                  height: availableGridHeight,
                  child: _buildHeatmapGrid(
                    context: context,
                    datasets: datasets,
                    startDate: startDate,
                    currentYear: currentYear,
                    columnCount: columnCount,
                    rowCount: rowCount,
                    cellSpacing: cellSpacing,
                  ),
                ),
              ],
            ),
          ),
          const SizedBox(height: legendTopPadding),
          SizedBox(
            width: totalWidgetWidth,
            child: _buildLegend(context, cellHeight),
          ),
        ],
      );

      return SingleChildScrollView(
        scrollDirection: Axis.horizontal,
        child: heatmapContent,
      );
    });
  }

  Widget _buildMonthHeaders(BuildContext context, DateTime startDate, int currentYear, double cellWidth, double cellSpacing, int columnCount) {
    final List<Widget> headerWidgets = [];
    int lastMonth = -1;
    int lastLabelIndex = -5;

    for (int i = 0; i < columnCount; i++) {
      final dateInWeek = startDate.add(Duration(days: i * 7 + 3));
      if (dateInWeek.month != lastMonth && dateInWeek.year == currentYear) {
        if (i < lastLabelIndex + 4 && headerWidgets.isNotEmpty) {
          continue;
        }
        headerWidgets.add(
          Positioned(
            left: i * (cellWidth + cellSpacing),
            child: Text(
              DateFormat('MMM', 'ko_KR').format(dateInWeek),
              style: Theme.of(context).textTheme.bodySmall,
            ),
          ),
        );
        lastMonth = dateInWeek.month;
        lastLabelIndex = i;
      }
    }
    return Stack(children: headerWidgets);
  }

  Widget _buildWeekDayHeaders(BuildContext context, double cellHeight) {
    final dayLabels = ['일', '월', '화', '수', '목', '금', '토'];
    return Column(
      mainAxisAlignment: MainAxisAlignment.spaceAround,
      children: List.generate(7, (index) {
        return SizedBox(
          height: cellHeight,
          child: Center(
            child: Text(dayLabels[index], style: Theme.of(context).textTheme.bodySmall?.copyWith(fontSize: 12)),
          ),
        );
      }),
    );
  }

  Widget _buildHeatmapGrid({
    required BuildContext context,
    required Map<DateTime, int> datasets,
    required DateTime startDate,
    required int currentYear,
    required int columnCount,
    required int rowCount,
    required double cellSpacing,
  }) {
    final int totalCellCount = columnCount * rowCount;
    return GridView.builder(
      scrollDirection: Axis.horizontal,
      physics: const NeverScrollableScrollPhysics(),
      gridDelegate: SliverGridDelegateWithFixedCrossAxisCount(
        crossAxisCount: rowCount,
        mainAxisSpacing: cellSpacing,
        crossAxisSpacing: cellSpacing,
      ),
      itemCount: totalCellCount,
      itemBuilder: (context, index) {
        final date = startDate.add(Duration(days: index));

        if (date.year != currentYear) {
          return const SizedBox.shrink();
        }

        final utcDate = DateTime.utc(date.year, date.month, date.day);
        final level = datasets[utcDate] ?? 0;
        final color = _getColorForLevel(level, Theme.of(context));
        final statusText = level > 0 ? "완료" : "기록 없음";

        return Tooltip(
          message: "${DateFormat('yyyy년 MM월 dd일 (E)', 'ko_KR').format(date)}\n$statusText",
          padding: const EdgeInsets.all(8),
          textStyle: const TextStyle(color: Colors.white, fontSize: 13),
          decoration: BoxDecoration(color: Colors.black.withOpacity(0.8), borderRadius: BorderRadius.circular(4)),
          child: LayoutBuilder(builder: (context, constraints) {
            return Container(
              decoration: BoxDecoration(
                color: color,
                borderRadius: BorderRadius.circular(max(1.0, constraints.maxWidth / 4.5)),
              ),
            );
          }),
        );
      },
    );
  }

  Color _getColorForLevel(int level, ThemeData theme) {
    if (level > 0) {
      return theme.primaryColor;
    }
    return theme.colorScheme.surfaceContainerHighest.withAlpha(155);
  }

  Widget _buildLegend(BuildContext context, double cellHeight) {
    final theme = Theme.of(context);
    final double size = max(10, cellHeight * 0.8);
    return Row(
      mainAxisAlignment: MainAxisAlignment.end,
      children: [
        Text('기록 없음', style: theme.textTheme.bodySmall),
        const SizedBox(width: 4),
        Container(width: size, height: size, decoration: BoxDecoration(color: _getColorForLevel(0, theme), borderRadius: BorderRadius.circular(size / 5))),
        const SizedBox(width: 12),
        Text('완료', style: theme.textTheme.bodySmall),
        const SizedBox(width: 4),
        Container(width: size, height: size, decoration: BoxDecoration(color: _getColorForLevel(1, theme), borderRadius: BorderRadius.circular(size / 5))),
      ],
    );
  }
}