// /lib/business_logic/services/database_service.dart

import 'dart:io';
import 'package:flutter/material.dart';
import 'package:path/path.dart';
import 'package:share_plus/share_plus.dart';
import 'package:file_picker/file_picker.dart';
import 'package:sqflite/sqflite.dart';
import '../../data/data_sources/sqlite_helper.dart';

class DatabaseService {
  final BuildContext context;

  DatabaseService(this.context);

  // 현재 데이터베이스 파일의 경로를 가져오는 함수
  Future<String> getDatabasePath() async {
    const dbName = 'daily_palette.db';
    final dbPath = await getDatabasesPath();
    return join(dbPath, dbName);
  }

  // 데이터베이스 백업
  Future<void> backupDatabase() async {
    try {
      final dbPath = await getDatabasePath();
      final file = XFile(dbPath);
      await Share.shareXFiles([file], text: 'Daily Palette 데이터 백업 파일');
    } catch (e) {
      _showErrorDialog('백업 실패: $e');
    }
  }

  // 데이터베이스 복원
  Future<void> restoreDatabase() async {
    // 1. 사용자에게 경고 메시지 표시
    final confirmed = await _showRestoreConfirmDialog();
    if (!confirmed) return;

    try {
      // 2. 파일 피커를 열어 .db 파일을 선택하게 함
      final result = await FilePicker.platform.pickFiles();
      if (result == null || result.files.single.path == null) {
        _showSnackBar('파일이 선택되지 않았습니다.');
        return;
      }

      final pickedFile = File(result.files.single.path!);
      final dbPath = await getDatabasePath();

      // 3. 현재 DB 연결을 닫음 (매우 중요)
      await SQLiteHelper.instance.close();

      // 4. 선택한 파일을 앱의 DB 경로로 복사하여 덮어씀
      await pickedFile.copy(dbPath);

      _showSnackBar('복원이 완료되었습니다. 앱을 재시작해주세요.');

    } catch (e) {
      _showErrorDialog('복원 실패: $e');
    }
  }

  // 사용자에게 스낵바를 보여주는 헬퍼 함수
  void _showSnackBar(String message) {
    ScaffoldMessenger.of(context).showSnackBar(SnackBar(content: Text(message)));
  }

  // 에러 다이얼로그
  Future<void> _showErrorDialog(String message) async {
    return showDialog(context: context, builder: (context) => AlertDialog(
      title: const Text('오류'), content: Text(message),
      actions: [TextButton(onPressed: () => Navigator.of(context).pop(), child: const Text('확인'))],
    ));
  }

  // 복원 확인 다이얼로그
  Future<bool> _showRestoreConfirmDialog() async {
    return await showDialog<bool>(
        context: context,
        builder: (context) => AlertDialog(
          title: const Text('데이터 복원'),
          content: const Text('정말로 데이터를 복원하시겠습니까?\n현재 앱의 모든 데이터가 선택한 파일의 데이터로 덮어씌워집니다. 이 작업은 되돌릴 수 없습니다.'),
          actions: [
            TextButton(onPressed: () => Navigator.of(context).pop(false), child: const Text('취소')),
            TextButton(onPressed: () => Navigator.of(context).pop(true), child: const Text('복원', style: TextStyle(color: Colors.red))),
          ],
        )
    ) ?? false;
  }
}