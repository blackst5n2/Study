// /lib/business_logic/notifiers/category_notifier.dart

import 'package:riverpod/riverpod.dart';
import '../../data/models/category.dart';
import '../../data/repositories/category_repository.dart';
import '../providers.dart'; // ref.invalidate를 위해 추가

class CategoryListNotifier extends StateNotifier<AsyncValue<List<Category>>> {
  final CategoryRepository _repository;
  // [추가] 다른 Provider를 무효화하기 위해 Ref를 받음
  final Ref _ref;

  CategoryListNotifier(this._repository, this._ref) : super(const AsyncValue.loading()) {
    fetchCategories();
  }


  Future<void> fetchCategories() async {
    if (!mounted) return;

    state = const AsyncValue.loading();
    try {
      final categories = await _repository.getCategories();
      if (mounted) {
        state = AsyncValue.data(categories);
      }
    } catch (e, st) {
      if (mounted) {
        state = AsyncValue.error(e, st);
      }
    }
  }

  Future<void> addCategory(Category category) async {
    try {
      await _repository.addCategory(category);
      await fetchCategories();
    } catch (e) {
      // 에러 처리
    }
  }

  Future<bool> updateCategory(Category category) async {
    try {
      await _repository.updateCategory(category);
      // [수정] 카테고리 수정 시, 할 일 목록도 새로고침하여 변경된 카테고리 색상/이름을 반영
      _ref.invalidate(todoListProvider);
      await fetchCategories();
      return true;
    } catch (e) {
      return false;
    }
  }

  // [수정] 카테고리 삭제 로직 단순화
  Future<bool> deleteCategory(int id) async {
    try {
      // DB에서 카테고리와 관련된 모든 데이터가 삭제됨
      await _repository.deleteCategory(id);
      // 카테고리 목록을 새로고침
      await fetchCategories();
      // [추가] 할 일 목록 Provider를 무효화하여 화면을 새로고침
      _ref.invalidate(todoListProvider);
      // [추가] 통계 데이터도 무효화
      _ref.invalidate(statisticsProvider);
      return true;
    } catch (e) {
      return false;
    }
  }
}