// /lib/presentation/screens/main_screen.dart

import 'package:flutter/material.dart';
import 'package:flutter_native_splash/flutter_native_splash.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';

import '../../business_logic/providers.dart';
import '../../business_logic/services/task_cleanup_service.dart';
import 'home_screen.dart';
import 'statistics_screen.dart';
import 'settings_screen.dart';


class MainScreen extends ConsumerStatefulWidget {
  const MainScreen({super.key});

  @override
  ConsumerState<MainScreen> createState() => _MainScreenState();
}

class _MainScreenState extends ConsumerState<MainScreen> with WidgetsBindingObserver {
  int _selectedIndex = 0;
  DateTime _lastCheckedDate = DateTime.now();

  static const List<Widget> _widgetOptions = <Widget>[
    HomeScreen(),
    StatisticsScreen(),
    SettingsScreen(),
  ];

  void _onItemTapped(int index) {
    setState(() {
      _selectedIndex = index;
    });
  }

  @override
  void initState() {
    super.initState();
    WidgetsBinding.instance.addObserver(this);
    _lastCheckedDate = DateTime(DateTime.now().year, DateTime.now().month, DateTime.now().day);

    // ================== [ 핵심 수정 부분 ] ==================
    // 앱의 첫 화면이 빌드된 후, 초기화 작업을 시작하고 스플래시 화면을 제거합니다.
    WidgetsBinding.instance.addPostFrameCallback((_) {
      _initializeApp();
    });
    // ========================================================
  }

  // ================== [ 핵심 추가 부분 ] ==================
  Future<void> _initializeApp() async {
    try {
      // main 함수에 있던 무거운 초기화 작업들을 여기서 실행합니다.
      final todoRepo = ref.read(todoRepositoryProvider);
      final cleanupService = TaskCleanupService(todoRepo: todoRepo);
      await cleanupService.autoFailOverdueTasks();

      final recurringTaskService = ref.read(recurringTaskServiceProvider);
      await recurringTaskService.createTodosForToday();

      // 관련 Provider들을 새로고침하여 최신 데이터를 반영합니다.
      ref.invalidate(todoListProvider);
      ref.invalidate(monthlyStatusProvider);
      ref.invalidate(statisticsProvider);

    } catch (e, stackTrace) { // [수정] 스택 트레이스도 함께 받습니다.
      // 초기화 중 에러가 발생하더라도 스플래시 화면은 제거되어야 앱이 멈추지 않습니다.
      // 에러 로그를 출력하여 디버깅을 돕습니다.
      // 실제 사용자에게는 간단한 에러 메시지를 보여주는 것이 좋습니다.
      debugPrint("App initialization error: $e");
      debugPrint("Stack trace: $stackTrace"); // [추가] 스택 트레이스 출력
      // 예를 들어, 에러 발생 시 사용자에게 알림
      // if (mounted) {
      //   ScaffoldMessenger.of(context).showSnackBar(
      //     const SnackBar(content: Text('앱 초기화 중 오류가 발생했습니다. 앱을 재시작해주세요.')),
      //   );
      // }
    } finally {
      // 모든 작업이 끝나면(성공하든 실패하든) 스플래시 화면을 제거합니다.
      FlutterNativeSplash.remove();
    }
  }
  // ========================================================

  @override
  void dispose() {
    WidgetsBinding.instance.removeObserver(this);
    super.dispose();
  }

  @override
  void didChangeAppLifecycleState(AppLifecycleState state) {
    super.didChangeAppLifecycleState(state);
    if (state == AppLifecycleState.resumed) {
      final now = DateTime.now();
      final today = DateTime(now.year, now.month, now.day);

      if (!isSameDay(today, _lastCheckedDate)) {
        _lastCheckedDate = today;
        _refreshDataForNewDay();
      }
    }
  }

  void _refreshDataForNewDay() {
    final now = DateTime.now();
    ref.read(selectedDateProvider.notifier).state = now;
    ref.read(focusedMonthProvider.notifier).state = now;
    ref.read(recurringTaskServiceProvider).createTodosForToday().then((_) {
      ref.invalidate(todoListProvider);
      ref.invalidate(monthlyStatusProvider);
      ref.invalidate(statisticsProvider);
    });
  }

  bool isSameDay(DateTime? a, DateTime? b) {
    if (a == null || b == null) {
      return false;
    }
    return a.year == b.year && a.month == b.month && a.day == b.day;
  }


  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: IndexedStack(
        index: _selectedIndex,
        children: _widgetOptions,
      ),
      bottomNavigationBar: BottomNavigationBar(
        items: const <BottomNavigationBarItem>[
          BottomNavigationBarItem(
            icon: Icon(Icons.edit_calendar),
            label: '일정',
          ),
          BottomNavigationBarItem(
            icon: Icon(Icons.bar_chart),
            label: '통계',
          ),
          BottomNavigationBarItem(
            icon: Icon(Icons.settings),
            label: '설정',
          ),
        ],
        currentIndex: _selectedIndex,
        onTap: _onItemTapped,
      ),
    );
  }
}