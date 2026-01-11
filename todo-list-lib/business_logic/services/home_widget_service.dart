// /lib/business_logic/services/home_widget_service.dart

import 'dart:convert';
import 'package:home_widget/home_widget.dart';
import 'package:intl/intl.dart';
import '../../data/data_sources/sqlite_helper.dart';
import '../../data/repositories/todo_repository_impl.dart';

class HomeWidgetService {
  // 위젯 데이터를 업데이트하고, 네이티브 위젯에 갱신 신호를 보냅니다.
  static Future<void> updateWidgetData() async {
    try {
      // Riverpod Provider 외부에서 Repository를 사용하기 위해 직접 인스턴스화합니다.
      final dbHelper = SQLiteHelper.instance;
      final todoRepo = TodoRepositoryImpl(dbHelper);

      // 오늘의 할 일 목록을 가져옵니다.
      final todayString = DateFormat('yyyy-MM-dd').format(DateTime.now());
      final todos = await todoRepo.getTodosByDate(todayString);

      // 할 일 목록을 JSON 형식으로 변환합니다.
      final todoListJson = todos.map((todo) => {
        'title': todo.title,
        'status': todo.status,
      }).toList();

      // home_widget을 통해 데이터를 저장합니다.
      // 1. 할 일 목록 JSON 문자열
      await HomeWidget.saveWidgetData<String>('todos_json', jsonEncode(todoListJson));
      // 2. 업데이트 시간 (선택 사항)
      await HomeWidget.saveWidgetData<String>('last_update', DateFormat('HH:mm:ss').format(DateTime.now()));

      // 네이티브 위젯을 업데이트하도록 요청합니다.
      await HomeWidget.updateWidget(
        name: 'HomeWidgetProvider', // Android Provider 클래스 이름
        iOSName: 'HomeWidget', // iOS 위젯 이름
      );
    } catch (e) {
      return;
    }
  }
}