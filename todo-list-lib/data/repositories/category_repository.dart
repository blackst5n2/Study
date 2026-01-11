// /lib/data/repositories/category_repository.dart

import '../models/category.dart';

// 카테고리 데이터에 접근하기 위한 추상 클래스(인터페이스)
abstract class CategoryRepository {
  Future<List<Category>> getCategories();
  Future<int> addCategory(Category category);
  Future<int> updateCategory(Category category);
  Future<int> deleteCategory(int id);
  Future<Category?> getCategoryById(int categoryId);
}