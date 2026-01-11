// /lib/presentation/screens/statistics_screen.dart

import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:fl_chart/fl_chart.dart';
import '../../business_logic/providers.dart';
import '../../data/models/category.dart';
import '../widgets/daily_bar_chart.dart';

class CategoryStats {
  final Category category;
  final int totalCount;
  final int completedCount;
  final int failedCount;

  CategoryStats({
    required this.category,
    required this.totalCount,
    required this.completedCount,
    required this.failedCount,
  });

  double get completionRate => (completedCount + failedCount) == 0 ? 0 : completedCount / (completedCount + failedCount);
}

class DailyCompletionRate {
  final DateTime date;
  final double rate;
  DailyCompletionRate({required this.date, required this.rate});
}

class OverallStats {
  final int totalTodos;
  final int totalCompleted;
  final int totalFailed;
  final Category? mostProductiveCategory;
  final int maxStreak;
  OverallStats({ required this.totalTodos, required this.totalCompleted, required this.totalFailed, this.mostProductiveCategory, required this.maxStreak, });
  double get completionRate => (totalCompleted + totalFailed) == 0 ? 0 : totalCompleted / (totalCompleted + totalFailed);
}

class StatisticsState {
  final List<CategoryStats> categoryStats;
  final List<DailyCompletionRate> weeklyStats;
  final List<DailyCompletionRate> monthlyStats;
  final OverallStats overallStats;
  StatisticsState({ required this.categoryStats, required this.weeklyStats, required this.monthlyStats, required this.overallStats });
}

class StatisticsScreen extends ConsumerStatefulWidget {
  const StatisticsScreen({super.key});

  @override
  ConsumerState<StatisticsScreen> createState() => _StatisticsScreenState();
}

class _StatisticsScreenState extends ConsumerState<StatisticsScreen> with SingleTickerProviderStateMixin {
  late TabController _tabController;
  @override
  void initState() {
    super.initState();
    _tabController = TabController(length: 4, vsync: this);
  }
  @override
  void dispose() {
    _tabController.dispose();
    super.dispose();
  }
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('통계'),
        bottom: TabBar(
          controller: _tabController,
          isScrollable: true,
          tabs: const [
            Tab(text: '주간'), Tab(text: '월간'), Tab(text: '전체'), Tab(text: '카테고리별'),
          ],
        ),
      ),
      body: TabBarView(
        controller: _tabController,
        children: [
          PeriodStatsTab(period: Period.weekly),
          PeriodStatsTab(period: Period.monthly),
          const OverallStatsTab(),
          const CategoryStatsTab(),
        ],
      ),
    );
  }
}

enum Period { weekly, monthly }

class PeriodStatsTab extends ConsumerWidget {
  final Period period;
  const PeriodStatsTab({super.key, required this.period});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final statsAsyncValue = ref.watch(statisticsProvider);

    // <<< 핵심 수정 지점 >>>
    return statsAsyncValue.when(
      skipLoadingOnRefresh: true, // 새로고침 시 로딩 상태를 건너뛰어 깜빡임 방지
      data: (stats) {
        final data = period == Period.weekly ? stats.weeklyStats : stats.monthlyStats;
        final title = period == Period.weekly ? '이번 주 달성률' : '이번 달 달성률';
        return ListView(
          padding: const EdgeInsets.all(16),
          children: [
            Text(title, style: const TextStyle(fontSize: 20, fontWeight: FontWeight.bold)),
            const SizedBox(height: 16),
            DailyBarChart(dailyData: data),
          ],
        );
      },
      loading: () => const Center(child: CircularProgressIndicator()),
      error: (e, st) => Center(child: Text('데이터 로딩 실패: $e')),
    );
  }
}

class OverallStatsTab extends ConsumerWidget {
  const OverallStatsTab({super.key});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final statsAsyncValue = ref.watch(statisticsProvider);

    // <<< 핵심 수정 지점 >>>
    return statsAsyncValue.when(
      skipLoadingOnRefresh: true, // 새로고침 시 로딩 상태를 건너뛰어 깜빡임 방지
      data: (stats) {
        final overall = stats.overallStats;
        return Padding(
          padding: const EdgeInsets.all(16.0),
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.stretch,
            children: [
              Card(
                child: Padding(
                  padding: const EdgeInsets.all(16.0),
                  child: Column(
                    children: [
                      const Text('전체 달성률', style: TextStyle(fontSize: 18, fontWeight: FontWeight.bold)),
                      const SizedBox(height: 8),
                      Text('${(overall.completionRate * 100).toStringAsFixed(1)}%', style: const TextStyle(fontSize: 36, fontWeight: FontWeight.bold, color: Colors.green)),
                      Text('완료: ${overall.totalCompleted} / 실패: ${overall.totalFailed}'),
                    ],
                  ),
                ),
              ),
              Card(
                child: ListTile(
                  title: const Text('총 등록한 할 일'),
                  trailing: Text('${overall.totalTodos} 개', style: const TextStyle(fontSize: 18, fontWeight: FontWeight.bold)),
                ),
              ),
              Card(
                child: ListTile(
                  title: const Text('최고의 카테고리'),
                  trailing: Text(overall.mostProductiveCategory?.name ?? '없음', style: const TextStyle(fontSize: 18, fontWeight: FontWeight.bold)),
                ),
              ),
              Card(
                child: ListTile(
                  leading: const Icon(Icons.local_fire_department, color: Colors.orange),
                  title: const Text('최대 연속 달성'),
                  trailing: Text('${overall.maxStreak} 일', style: const TextStyle(fontSize: 18, fontWeight: FontWeight.bold)),
                ),
              ),
            ],
          ),
        );
      },
      loading: () => const Center(child: CircularProgressIndicator()),
      error: (e, st) => Center(child: Text('데이터 로딩 실패: $e')),
    );
  }
}

class CategoryStatsTab extends ConsumerStatefulWidget {
  const CategoryStatsTab({super.key});

  @override
  ConsumerState<CategoryStatsTab> createState() => _CategoryStatsTabState();
}

class _CategoryStatsTabState extends ConsumerState<CategoryStatsTab> {
  int? _touchedIndex;

  @override
  Widget build(BuildContext context) {
    final statsAsyncValue = ref.watch(statisticsProvider);

    // <<< 핵심 수정 지점 >>>
    return statsAsyncValue.when(
      skipLoadingOnRefresh: true, // 새로고침 시 로딩 상태를 건너뛰어 깜빡임 방지
      data: (stats) {
        final categoryStats = stats.categoryStats;
        if (categoryStats.isEmpty) {
          return const Center(child: Text('분석할 데이터가 없습니다.'));
        }
        final chartData = categoryStats.where((s) => s.totalCount > 0).toList();

        return ListView(
          padding: const EdgeInsets.all(16),
          children: [
            const Text('카테고리별 할 일 분포', style: TextStyle(fontSize: 20, fontWeight: FontWeight.bold)),
            const SizedBox(height: 20),
            if (chartData.isEmpty)
              const SizedBox(height: 250, child: Center(child: Text('차트에 표시할 데이터가 없습니다.')))
            else
              SizedBox(
                height: 250,
                child: PieChart(
                  PieChartData(
                    pieTouchData: PieTouchData(
                      touchCallback: (FlTouchEvent event, pieTouchResponse) {
                        setState(() {
                          if (!event.isInterestedForInteractions || pieTouchResponse == null || pieTouchResponse.touchedSection == null) {
                            _touchedIndex = -1;
                            return;
                          }
                          _touchedIndex = pieTouchResponse.touchedSection!.touchedSectionIndex;
                        });
                      },
                    ),
                    sectionsSpace: 4,
                    centerSpaceRadius: 40,
                    sections: List.generate(chartData.length, (index) {
                      final stat = chartData[index];
                      final isTouched = index == _touchedIndex;
                      final radius = isTouched ? 90.0 : 80.0;
                      final fontSize = isTouched ? 18.0 : 16.0;

                      return PieChartSectionData(
                        color: Color(int.parse(stat.category.colorCode, radix: 16)),
                        value: stat.totalCount.toDouble(),
                        title: '${stat.totalCount}개',
                        radius: radius,
                        titleStyle: TextStyle(
                          fontSize: fontSize,
                          fontWeight: FontWeight.bold,
                          color: Colors.white,
                          shadows: [Shadow(color: Colors.black.withAlpha(128), blurRadius: 2)],
                        ),
                      );
                    }),
                  ),
                ),
              ),
            const SizedBox(height: 32),
            const Text('카테고리별 상세 정보', style: TextStyle(fontSize: 20, fontWeight: FontWeight.bold)),
            const SizedBox(height: 10),
            ListView.separated(
              shrinkWrap: true,
              physics: const NeverScrollableScrollPhysics(),
              itemCount: categoryStats.length,
              itemBuilder: (context, index) {
                final stat = categoryStats[index];
                return ListTile(
                  leading: CircleAvatar(backgroundColor: Color(int.parse(stat.category.colorCode, radix: 16)), radius: 10),
                  title: Text(stat.category.name),
                  subtitle: Text('완료: ${stat.completedCount} / 실패: ${stat.failedCount} (총 ${stat.totalCount}개)'),
                  trailing: Text('${(stat.completionRate * 100).toStringAsFixed(0)}%', style: const TextStyle(fontWeight: FontWeight.bold, fontSize: 16)),
                );
              },
              separatorBuilder: (context, index) => const Divider(),
            )
          ],
        );
      },
      loading: () => const Center(child: CircularProgressIndicator()),
      error: (e, st) => Center(child: Text('데이터 로딩 실패: $e')),
    );
  }
}