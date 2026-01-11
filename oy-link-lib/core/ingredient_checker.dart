import 'dart:convert';
import 'package:oy_pro_link/core/database_helper.dart';
import 'package:oy_pro_link/models/compatibility_result.dart';

class IngredientChecker {
  final DatabaseHelper _dbHelper = DatabaseHelper();

  // 두 성분의 궁합을 확인하는 메인 함수
  Future<CompatibilityResult> check(String ingredientA, String ingredientB) async {
    // 입력값이 비어있는 경우 처리
    if (ingredientA.isEmpty || ingredientB.isEmpty) {
      return CompatibilityResult(
        type: CompatibilityType.neutral,
        message: '두 가지 성분을 모두 입력해주세요.',
      );
    }

    // 키워드 정규화 (공백 제거, 소문자 변환 등)
    final String keywordA = _normalizeKeyword(ingredientA);
    final String keywordB = _normalizeKeyword(ingredientB);

    // 데이터베이스 조회를 위한 일관된 키 생성 (알파벳 순서로 조합)
    List<String> sortedKeywords = [keywordA, keywordB]..sort();
    String dbKey = sortedKeywords.join('+');

    final result = await _dbHelper.getInteraction(dbKey);

    if (result != null) {
      final data = jsonDecode(result['value']);
      final typeString = data['type'] as String;
      final reason = data['reason'] as String;

      if (typeString == '시너지') {
        return CompatibilityResult(type: CompatibilityType.synergy, message: reason);
      } else if (typeString == '길항') {
        return CompatibilityResult(type: CompatibilityType.antagonistic, message: reason);
      }
    }

    // DB에 정보가 없는 경우
    return CompatibilityResult(
      type: CompatibilityType.neutral,
      message: '해당 조합에 대한 특별한 상호작용 정보가 없습니다. 일반적인 사용은 가능하지만, 두 성분 모두 활성 성분이라면 사용 간격을 두거나 피부 반응을 살피며 사용하세요.',
    );
  }

  // 검색 키워드를 표준화하는 내부 함수
  String _normalizeKeyword(String keyword) {
    // 예시: " 비타민 C " -> "비타민c"
    // 예시: "AHA/BHA" -> "aha/bha"
    return keyword.trim().toLowerCase();
  }
}