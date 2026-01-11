import 'dart:convert';
import 'package:oy_pro_link/core/database_helper.dart';
import 'package:oy_pro_link/models/ai_recommendation.dart';
import 'package:oy_pro_link/models/recommended_product_model.dart';

class AiAssistant {
  final DatabaseHelper _dbHelper = DatabaseHelper();

  Future<Map<String, List<String>>> getGroupedConcernSuggestions() async {
    final faceConcerns = await _dbHelper.getAllConcernKeys();
    final hairConcerns = await _dbHelper.getHairConcernKeys();
    final bodyConcerns = await _dbHelper.getBodyConcernKeys();

    final Map<String, List<String>> groupedSuggestions = {};

    if (faceConcerns.isNotEmpty) {
      groupedSuggestions['피부 고민'] = faceConcerns;
    }
    if (hairConcerns.isNotEmpty) {
      groupedSuggestions['헤어 고민'] = hairConcerns;
    }
    if (bodyConcerns.isNotEmpty) {
      groupedSuggestions['바디 고민'] = bodyConcerns;
    }
    return groupedSuggestions;
  }

  Future<AiRecommendation?> getRecommendations(String query) async {
    if (query.trim().isEmpty) return null;

    final db = await _dbHelper.database;
    final searchQuery = query.trim();

    // 검색할 카테고리와 우선순위 정의
    const searchCategories = [
      'concern',
      'hair_concern',
      'body_concern',
      'skin_type',
      'ingredient_info',
      'concept_info',
      'texture_info',
      'tool_info',
      'inner_beauty_info'
    ];

    for (final category in searchCategories) {
      final maps = await db.query(
        'knowledge',
        where: 'category = ? AND key LIKE ?',
        whereArgs: [category, '%$searchQuery%'],
        limit: 1,
      );

      if (maps.isNotEmpty) {
        final result = maps.first;
        final resultKey = result['key'] as String;

        // 'concern' 카테고리들이 매칭된 경우, 제품 추천 로직 수행
        if (category.contains('concern')) {
          final rawRecommendedIngredients = result['value'] as String;
          final keywords = rawRecommendedIngredients
              .replaceAll(RegExp(r'\(.*\)'), '')
              .split(',')
              .map((e) => e.trim().toLowerCase())
              .where((e) => e.isNotEmpty)
              .toList();

          final allProducts = await _dbHelper.getAllProducts();
          final matchingProducts = <RecommendedProduct>[];

          for (var product in allProducts) {
            if (product.ingredients == null || product.ingredients!.isEmpty) continue;
            final productIngredients = product.ingredients!.toLowerCase();
            for (var keyword in keywords) {
              if (productIngredients.contains(keyword)) {
                matchingProducts.add(RecommendedProduct(product: product, matchedIngredient: keyword));
                break;
              }
            }
          }

          return AiRecommendation(
            resultType: category, // 'concern', 'hair_concern' 등
            resultTitle: resultKey,
            recommendedIngredients: keywords,
            matchingProducts: matchingProducts,
          );
        }
        // 그 외 카테고리가 매칭된 경우, 정보 제공 로직 수행
        else {
          final value = jsonDecode(result['value'] as String);
          return AiRecommendation(
            resultType: category,
            resultTitle: resultKey,
            singleItemInfo: value,
          );
        }
      }
    }

    // 모든 카테고리에서 검색 결과가 없는 경우
    return null;
  }
}