import 'package:fl_chart/fl_chart.dart';
import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
import '../screens/statistics_screen.dart';

class DailyBarChart extends StatelessWidget {
  final List<DailyCompletionRate> dailyData;
  const DailyBarChart({super.key, required this.dailyData});

  @override
  Widget build(BuildContext context) {
    return AspectRatio(
      aspectRatio: 1.7,
      child: Card(
        elevation: 2,
        shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(8)),
        child: Padding(
          padding: const EdgeInsets.all(16),
          child: BarChart(
            BarChartData(
              alignment: BarChartAlignment.spaceAround,
              maxY: 100,
              barTouchData: BarTouchData(
                touchTooltipData: BarTouchTooltipData(
                  getTooltipColor: (group) => Colors.blueGrey,
                  getTooltipItem: (group, groupIndex, rod, rodIndex) {
                    String weekDay = DateFormat('E', 'ko_KR').format(dailyData[groupIndex].date);
                    return BarTooltipItem(
                      '$weekDay\n',
                      const TextStyle(color: Colors.white, fontWeight: FontWeight.bold, fontSize: 14),
                      children: <TextSpan>[
                        TextSpan(
                          text: '${rod.toY.toStringAsFixed(0)}%',
                          style: const TextStyle(color: Colors.yellow, fontSize: 16, fontWeight: FontWeight.w500),
                        ),
                      ],
                    );
                  },
                ),
              ),
              titlesData: FlTitlesData(
                show: true,
                bottomTitles: AxisTitles(
                  sideTitles: SideTitles(
                    showTitles: true,
                    getTitlesWidget: (double value, TitleMeta meta) {
                      final day = dailyData[value.toInt()].date;
                      String text = '';
                      if (dailyData.length <= 7) {
                        text = DateFormat('E', 'ko_KR').format(day);
                      } else {
                        if (day.day == 1 || day.day % 5 == 0) text = '${day.day}일';
                      }
                      return SideTitleWidget(meta: meta, space: 4, child: Text(text, style: const TextStyle(fontWeight: FontWeight.bold, fontSize: 12)));
                    },
                    reservedSize: 38,
                  ),
                ),
                leftTitles: const AxisTitles(sideTitles: SideTitles(showTitles: false)),
                topTitles: const AxisTitles(sideTitles: SideTitles(showTitles: false)),
                rightTitles: const AxisTitles(sideTitles: SideTitles(showTitles: false)),
              ),
              gridData: const FlGridData(show: false),
              borderData: FlBorderData(show: false),
              barGroups: List.generate(dailyData.length, (index) {
                return BarChartGroupData(
                  x: index,
                  barRods: [
                    BarChartRodData(
                      toY: dailyData[index].rate * 100,
                      color: Colors.lightGreen,
                      width: 22,
                      borderRadius: BorderRadius.circular(4),
                    )
                  ],
                );
              }),
            ),
          ),
        ),
      ),
    );
  }
}