// /lib/presentation/screens/settings_screen.dart

import 'package:daily_palette/presentation/screens/category_screen.dart';
import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:permission_handler/permission_handler.dart';
import '../../business_logic/providers.dart';
import '../../business_logic/services/database_service.dart';
import 'recurring_task_screen.dart';

class SettingsScreen extends ConsumerWidget {
  const SettingsScreen({super.key});

  String _getPermissionStatusText(PermissionStatus status) {
    switch (status) {
      case PermissionStatus.granted:
        return '허용됨';
      case PermissionStatus.denied:
        return '거부됨 (탭하여 권한 요청)';
      case PermissionStatus.permanentlyDenied:
      case PermissionStatus.restricted:
        return '영구적으로 거부됨 (탭하여 설정으로 이동)';
      default:
        return '알 수 없음';
    }
  }

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final dbService = DatabaseService(context);
    final settings = ref.watch(settingsProvider);
    final settingsNotifier = ref.read(settingsProvider.notifier);

    // [수정] 테마 설정을 위해 themeProvider를 watch 합니다.
    final themeMode = ref.watch(themeProvider);

    return Scaffold(
      appBar: AppBar(
        title: const Text('설정'),
      ),
      body: ListView(
        children: [
          ListTile(
            leading: const Icon(Icons.brightness_6_outlined),
            title: const Text('테마'),
            trailing: DropdownButton<ThemeMode>(
              // [수정] value를 settings.themeMode가 아닌 themeMode로 변경합니다.
              value: themeMode,
              items: const [
                DropdownMenuItem(value: ThemeMode.system, child: Text('시스템 설정')),
                DropdownMenuItem(value: ThemeMode.light, child: Text('라이트 모드')),
                DropdownMenuItem(value: ThemeMode.dark, child: Text('다크 모드')),
              ],
              onChanged: (mode) {
                if (mode != null) {
                  ref.read(themeProvider.notifier).setThemeMode(mode);
                }
              },
            ),
          ),
          ListTile(
            leading: const Icon(Icons.category),
            title: const Text('카테고리 관리'),
            onTap: () {
              Navigator.of(context).push(
                MaterialPageRoute(builder: (context) => const CategoryScreen()),
              );
            }
          ),
          ListTile(
            leading: const Icon(Icons.sync),
            title: const Text('반복 일정 관리'),
            onTap: () {
              Navigator.of(context).push(
                MaterialPageRoute(builder: (context) => const RecurringTaskListScreen()),
              );
            },
          ),
          const Divider(),
          const Padding(
            padding: EdgeInsets.fromLTRB(16, 16, 16, 0),
            child: Text('알림', style: TextStyle(fontWeight: FontWeight.bold, color: Colors.grey)),
          ),
          FutureBuilder<PermissionStatus>(
              future: Permission.notification.status,
              builder: (context, snapshot) {
                return ListTile(
                  leading: const Icon(Icons.notifications_active_outlined),
                  title: const Text('알림 권한'),
                  subtitle: Text(snapshot.hasData ? _getPermissionStatusText(snapshot.data!) : '확인 중...'),
                  onTap: () async => await openAppSettings(),
                );
              }
          ),
          SwitchListTile(
            title: const Text('취침 전 리마인더'),
            subtitle: const Text('매일 정해진 시간에 남은 할 일을 알려줘요.'),
            value: settings.isReminderEnabled,
            onChanged: (bool value) {
              settingsNotifier.setReminderEnabled(value);
            },
          ),
          ListTile(
            enabled: settings.isReminderEnabled,
            leading: const Icon(Icons.access_time_outlined),
            title: const Text('리마인더 시간'),
            subtitle: Text(settings.reminderTime.format(context)),
            onTap: () async {
              final time = await showTimePicker(
                context: context,
                initialTime: settings.reminderTime,
              );
              if (time != null) {
                settingsNotifier.setReminderTime(time);
              }
            },
          ),
          const Divider(),
          const Padding(
            padding: EdgeInsets.fromLTRB(16, 16, 16, 0),
            child: Text('데이터 관리', style: TextStyle(fontWeight: FontWeight.bold, color: Colors.grey)),
          ),
          ListTile(
            leading: const Icon(Icons.backup_outlined),
            title: const Text('데이터 백업'),
            subtitle: const Text('현재 데이터를 파일로 내보냅니다.'),
            onTap: dbService.backupDatabase,
          ),
          ListTile(
            leading: const Icon(Icons.restore_page_outlined),
            title: const Text('데이터 복원'),
            subtitle: const Text('백업 파일로부터 데이터를 가져옵니다.'),
            onTap: dbService.restoreDatabase,
          ),
        ],
      ),
    );
  }
}