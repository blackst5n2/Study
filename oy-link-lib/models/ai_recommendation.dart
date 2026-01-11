import 'package:oy_pro_link/models/recommended_product_model.dart';

class AiRecommendation {
  // 'concern' 타입 결과를 위한 기존 필드
  final List<String> recommendedIngredients;
  final List<RecommendedProduct> matchingProducts;

  // 모든 결과 타입을 위한 신규 필드
  final String? resultType; // 결과 종류 (예: 'concern', 'ingredient_info')
  final String? resultTitle; // 결과의 제목 (예: '주름', '레티놀')
  final Map<String, dynamic>? singleItemInfo; // 'concern' 외 타입의 상세 정보

  AiRecommendation({
    this.recommendedIngredients = const [],
    this.matchingProducts = const [],
    this.resultType,
    this.resultTitle,
    this.singleItemInfo,
  });
}