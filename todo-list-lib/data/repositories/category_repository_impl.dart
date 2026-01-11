// /lib/data/repositories/category_repository_impl.dart

import 'package:sqflite/sqflite.dart';
import '../data_sources/sqlite_helper.dart';
import '../models/category.dart';
import 'category_repository.dart';

// CategoryRepository의 실제 구현 클래스
class CategoryRepositoryImpl implements CategoryRepository {
  final SQLiteHelper _dbHelper;

  CategoryRepositoryImpl(this._dbHelper);

  @override
  Future<List<Category>> getCategories() async {
    final db = await _dbHelper.database;
    // 'Categories' 테이블에서 모든 데이터를 조회합니다.
    final List<Map<String, dynamic>> maps = await db.query('Categories', orderBy: 'id DESC');

    // Map 목록을 Category 객체 목록으로 변환합니다.
    return List.generate(maps.length, (i) {
      return Category.fromMap(maps[i]);
    });
  }

  @override
  Future<int> addCategory(Category category) async {
    final db = await _dbHelper.database;
    // 'Categories' 테이블에 새로운 데이터를 추가합니다.
    return await db.insert('Categories', category.toMap(),
        conflictAlgorithm: ConflictAlgorithm.replace);
  }

  @override
  Future<int> updateCategory(Category category) async {
    final db = await _dbHelper.database;
    return await db.update('Categories', category.toMap(), where: 'id = ?', whereArgs: [category.id]);
  }

  // [추가] 카테고리 삭제
  @override
  Future<int> deleteCategory(int id) async {
    final db = await _dbHelper.database;
    return await db.delete('Categories', where: 'id = ?', whereArgs: [id]);
  }

  @override
  Future<Category?> getCategoryById(int categoryId) async{
    final db = await _dbHelper.database;
    final List<Map<String, dynamic>> maps = await db.query('Categories', where: 'id = ?', whereArgs: [categoryId]);
    if (maps.isNotEmpty) {
      return Category.fromMap(maps.first);
    }
    return null;
  }
}