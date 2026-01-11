// /lib/data/repositories/theme_repository.dart

import 'package:flutter/material.dart';
import 'package:shared_preferences/shared_preferences.dart';

class ThemeRepository {
  static const _themeModeKey = 'themeMode';

  // 저장된 테마 모드 불러오기
  Future<ThemeMode> loadThemeMode() async {
    final prefs = await SharedPreferences.getInstance();
    final themeString = prefs.getString(_themeModeKey) ?? ThemeMode.system.name;
    return ThemeMode.values.firstWhere((e) => e.name == themeString);
  }

  // 테마 모드 저장하기
  Future<void> saveThemeMode(ThemeMode themeMode) async {
    final prefs = await SharedPreferences.getInstance();
    await prefs.setString(_themeModeKey, themeMode.name);
  }
}