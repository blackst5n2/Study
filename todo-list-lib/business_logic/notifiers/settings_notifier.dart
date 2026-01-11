// /lib/business_logic/notifiers/settings_notifier.dart

import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import '../../data/repositories/settings_repository.dart';
import '../services/notification_service.dart';

// 설정 화면의 상태를 담을 클래스
class AppSettings {
  final bool isReminderEnabled;
  final TimeOfDay reminderTime;

  AppSettings({required this.isReminderEnabled, required this.reminderTime});

  AppSettings copyWith({bool? isReminderEnabled, TimeOfDay? reminderTime}) {
    return AppSettings(
      isReminderEnabled: isReminderEnabled ?? this.isReminderEnabled,
      reminderTime: reminderTime ?? this.reminderTime,
    );
  }
}

class SettingsNotifier extends StateNotifier<AppSettings> {
  final SettingsRepository _repository;

  SettingsNotifier(this._repository) : super(AppSettings(isReminderEnabled: false, reminderTime: const TimeOfDay(hour: 22, minute: 0))) {
    _loadSettings();
  }

  Future<void> _loadSettings() async {
    final isEnabled = await _repository.loadReminderEnabled();
    final time = await _repository.loadReminderTime();
    state = AppSettings(isReminderEnabled: isEnabled, reminderTime: time);
  }

  Future<void> setReminderEnabled(bool isEnabled) async {
    state = state.copyWith(isReminderEnabled: isEnabled);
    await _repository.saveReminderEnabled(isEnabled);
    _updateScheduledNotification();
  }

  Future<void> setReminderTime(TimeOfDay time) async {
    state = state.copyWith(reminderTime: time);
    await _repository.saveReminderTime(time);
    _updateScheduledNotification();
  }

  // 설정값에 따라 알림을 예약하거나 취소하는 중앙 로직
  void _updateScheduledNotification() {
    if (state.isReminderEnabled) {
      NotificationService.instance.scheduleDailySummaryNotification(
        time: state.reminderTime,
        title: '오늘 하루는 어땠나요?  Palette',
        body: '남은 할 일을 확인하고 하루를 마무리해보세요!',
      );
    } else {
      NotificationService.instance.cancelDailySummaryNotification();
    }
  }
}