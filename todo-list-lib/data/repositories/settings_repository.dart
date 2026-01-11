// /lib/data/repositories/settings_repository.dart

import 'package:flutter/material.dart';
import 'package:shared_preferences/shared_preferences.dart';

class SettingsRepository {
  static const _reminderEnabledKey = 'reminderEnabled';
  static const _reminderTimeKey = 'reminderTime';
  static const _maxStreakKey = 'maxStreak';

  Future<void> saveReminderEnabled(bool isEnabled) async {
    final prefs = await SharedPreferences.getInstance();
    await prefs.setBool(_reminderEnabledKey, isEnabled);
  }

  Future<bool> loadReminderEnabled() async {
    final prefs = await SharedPreferences.getInstance();
    return prefs.getBool(_reminderEnabledKey) ?? false;
  }

  Future<void> saveReminderTime(TimeOfDay time) async {
    final prefs = await SharedPreferences.getInstance();
    final timeString = '${time.hour}:${time.minute}';
    await prefs.setString(_reminderTimeKey, timeString);
  }

  Future<TimeOfDay> loadReminderTime() async {
    final prefs = await SharedPreferences.getInstance();
    final timeString = prefs.getString(_reminderTimeKey) ?? '22:00';
    final parts = timeString.split(':');
    return TimeOfDay(hour: int.parse(parts[0]), minute: int.parse(parts[1]));
  }

  Future<void> saveMaxStreak(int streak) async {
    final prefs = await SharedPreferences.getInstance();
    await prefs.setInt(_maxStreakKey, streak);
  }

  // [수정] loadMaxStreak 메서드
  Future<int> loadMaxStreak() async {
    final prefs = await SharedPreferences.getInstance();
    // prefs.getInt()가 null을 반환할 경우, 기본값으로 0을 사용합니다.
    return prefs.getInt(_maxStreakKey) ?? 0;
  }
}