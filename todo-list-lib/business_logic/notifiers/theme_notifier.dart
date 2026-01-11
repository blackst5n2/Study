// /lib/business_logic/notifiers/theme_notifier.dart

import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import '../../data/repositories/theme_repository.dart';

class ThemeNotifier extends StateNotifier<ThemeMode> {
  final ThemeRepository _repository;

  ThemeNotifier(this._repository) : super(ThemeMode.system) {
    _loadTheme();
  }

  // 저장된 테마를 불러와 상태를 초기화
  Future<void> _loadTheme() async {
    state = await _repository.loadThemeMode();
  }

  // 새로운 테마 모드를 설정하고 저장
  Future<void> setThemeMode(ThemeMode themeMode) async {
    if (state == themeMode) return;
    state = themeMode;
    await _repository.saveThemeMode(themeMode);
  }
}