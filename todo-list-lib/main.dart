// /lib/main.dart

import 'package:android_alarm_manager_plus/android_alarm_manager_plus.dart';
import 'package:daily_palette/data/data_sources/sqlite_helper.dart';
import 'package:daily_palette/data/repositories/todo_repository_impl.dart';
import 'package:flutter/material.dart';
import 'package:flutter_local_notifications/flutter_local_notifications.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:google_fonts/google_fonts.dart';
import 'package:home_widget/home_widget.dart';
import 'package:intl/date_symbol_data_local.dart';
import 'package:flutter_localizations/flutter_localizations.dart';
import 'package:flutter_native_splash/flutter_native_splash.dart';

import 'business_logic/providers.dart';
import 'business_logic/services/notification_service.dart';
import 'presentation/screens/main_screen.dart';

@pragma("vm:entry-point")
void backgroundCallback(Uri? uri) async {
  if (uri?.host == 'complete_todo') {
    final todoId = int.tryParse(uri?.queryParameters['id'] ?? '');
    if (todoId == null) return;

    final dbHelper = SQLiteHelper.instance;
    final todoRepo = TodoRepositoryImpl(dbHelper);

    final todo = await todoRepo.getTodoById(todoId);
    if (todo != null && todo.status == 'pending') {
      await todoRepo.updateTodo(todo.copyWith(
        status: 'completed',
        completedAt: DateTime.now().toIso8601String(),
      ));
    }
    // Note: This background callback cannot easily update the UI,
    // so we rely on the main app's lifecycle to refresh the widget.
  }
}

@pragma("vm:entry-point")
void autoFailTask(int id) async {
  final todoId = id;

  WidgetsFlutterBinding.ensureInitialized();
  final dbHelper = SQLiteHelper.instance;
  final todoRepo = TodoRepositoryImpl(dbHelper);

  final todo = await todoRepo.getTodoById(todoId);
  if (todo != null && todo.status == 'pending') {
    await todoRepo.batchUpdateStatus([todoId], 'failed');
    await todoRepo.batchAddFailureLogs([todoId], '타이머 시간이 만료되어 자동 실패 처리됨');

    final FlutterLocalNotificationsPlugin flutterLocalNotificationsPlugin = FlutterLocalNotificationsPlugin();
    const AndroidInitializationSettings androidSettings = AndroidInitializationSettings('@mipmap/ic_launcher');
    const DarwinInitializationSettings iosSettings = DarwinInitializationSettings();
    const InitializationSettings settings = InitializationSettings(android: androidSettings, iOS: iosSettings);

    await flutterLocalNotificationsPlugin.initialize(settings);

    await flutterLocalNotificationsPlugin.show(
      todoId,
      '타이머 종료!',
      "'${todo.title}' 할 일의 시간이 만료되었습니다.",
      const NotificationDetails(
        android: AndroidNotificationDetails(
          'daily_palette_timer_channel',
          'Timer Notifications',
          channelDescription: 'Timer expiration alerts',
          importance: Importance.max,
          priority: Priority.high,
        ),
        iOS: DarwinNotificationDetails(presentSound: true),
      ),
    );
  }
}

void main() async {
  // ================== [ 핵심 수정 부분 ] ==================
  // 1. 스플래시 화면을 유지하도록 설정합니다.
  WidgetsBinding widgetsBinding = WidgetsFlutterBinding.ensureInitialized();
  FlutterNativeSplash.preserve(widgetsBinding: widgetsBinding);

  // 2. 앱 실행에 필수적인 초기화만 남겨둡니다.
  await initializeDateFormatting();
  await AndroidAlarmManager.initialize();
  await NotificationService.instance.init();
  HomeWidget.registerInteractivityCallback(backgroundCallback);

  // 3. 무거웠던 데이터 처리 로직을 main 함수에서 제거합니다.
  //    (이 로직은 MainScreen으로 이동합니다.)

  // 4. 앱을 실행합니다.
  runApp(const ProviderScope(child: MyApp()));
  // ========================================================
}

class MyApp extends ConsumerWidget {
  const MyApp({super.key});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final themeMode = ref.watch(themeProvider);
    final navigatorKey = ref.watch(navigatorKeyProvider);

    return MaterialApp(
      navigatorKey: navigatorKey,
      localizationsDelegates: const [
        GlobalMaterialLocalizations.delegate,
        GlobalWidgetsLocalizations.delegate,
        GlobalCupertinoLocalizations.delegate,
      ],
      supportedLocales: const [
        Locale('ko', 'KR'),
      ],
      locale: const Locale('ko'),
      title: 'Daily Palette',
      theme: ThemeData(
        brightness: Brightness.light,
        colorScheme: ColorScheme.fromSeed(seedColor: const Color(0xFF2E7D32), brightness: Brightness.light),
        useMaterial3: true,
      ),
      darkTheme: ThemeData(
        brightness: Brightness.dark,
        colorScheme: ColorScheme.fromSeed(seedColor: const Color(0xFF66BB6A), brightness: Brightness.dark),
        useMaterial3: true,
      ),
      themeMode: themeMode,
      home: const MainScreen(),
    );
  }
}