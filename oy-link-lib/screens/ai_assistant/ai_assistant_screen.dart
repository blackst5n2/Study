import 'dart:async';

import 'package:flutter/material.dart';
import 'package:oy_pro_link/core/ai_assistant.dart';
import 'package:oy_pro_link/models/ai_recommendation.dart';
import 'package:oy_pro_link/models/recommended_product_model.dart';
import '../product/product_add_screen.dart';

class AiAssistantScreen extends StatefulWidget {
  const AiAssistantScreen({super.key});

  @override
  State<AiAssistantScreen> createState() => _AiAssistantScreenState();
}

class _AiAssistantScreenState extends State<AiAssistantScreen> {
  final _concernController = TextEditingController();
  final _assistant = AiAssistant();

  AiRecommendation? _recommendation;
  Map<String, List<String>> _groupedSuggestions = {};
  bool _isLoading = false;
  bool _hasSearched = false;
  Timer? _debounce;

  @override
  void initState() {
    super.initState();
    _loadConcernSuggestions();
    _concernController.addListener(_onSearchChanged);
  }

  void _onSearchChanged() {
    if (_debounce?.isActive ?? false) _debounce!.cancel();
    _debounce = Timer(const Duration(milliseconds: 500), () {
      final query = _concernController.text.trim();
      if (query.length >= 2) {
        _getRecommendations();
      } else {
        setState(() {
          _hasSearched = false;
          _recommendation = null;
          _isLoading = false;
        });
      }
    });
  }

  Future<void> _loadConcernSuggestions() async {
    final suggestions = await _assistant.getGroupedConcernSuggestions();
    if (mounted) {
      setState(() {
        _groupedSuggestions = suggestions;
      });
    }
  }

  void _getRecommendations() async {
    final concern = _concernController.text.trim();
    if (concern.length < 2) return;

    setState(() {
      _isLoading = true;
      _hasSearched = true;
      _recommendation = null;
    });

    final recommendation = await _assistant.getRecommendations(concern);

    if (mounted && _concernController.text.trim() == concern) {
      setState(() {
        _recommendation = recommendation;
        _isLoading = false;
      });
    }
  }

  @override
  void dispose() {
    _concernController.removeListener(_onSearchChanged);
    _concernController.dispose();
    _debounce?.cancel();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('뷰티 AI 어시스턴트'),
      ),
      body: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          _buildSearchBar(),
          Expanded(
            child: _hasSearched
                ? _buildResultArea()
                : _buildSuggestionArea(),
          ),
        ],
      ),
    );
  }

  Widget _buildSearchBar() {
    return Padding(
      padding: const EdgeInsets.fromLTRB(16, 16, 16, 8),
      child: Row(
        children: [
          Expanded(
            child: TextField(
              controller: _concernController,
              decoration: const InputDecoration(
                hintText: '피부 고민, 성분, 용어 등 검색',
                border: OutlineInputBorder(),
                contentPadding: EdgeInsets.symmetric(horizontal: 16.0),
              ),
              onSubmitted: (_) => _getRecommendations(),
            ),
          ),
          const SizedBox(width: 8),
          IconButton(
            icon: const Icon(Icons.search),
            onPressed: _getRecommendations,
            style: IconButton.styleFrom(
              backgroundColor: Theme.of(context).primaryColor,
              foregroundColor: Colors.white,
              padding: const EdgeInsets.all(14),
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildSuggestionArea() {
    if (_groupedSuggestions.isEmpty) {
      return const Center(
        child: Text('궁금한 점을 검색해주세요.'),
      );
    }
    return ListView(
      padding: const EdgeInsets.symmetric(horizontal: 16),
      children: _groupedSuggestions.entries.map((entry) {
        final title = entry.key;
        final keywords = entry.value;
        return Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Padding(
              padding: const EdgeInsets.only(top: 16.0, bottom: 8.0),
              child: Text(
                title,
                style: const TextStyle(fontWeight: FontWeight.bold, fontSize: 16, color: Colors.black54),
              ),
            ),
            Wrap(
              spacing: 8.0,
              runSpacing: 4.0,
              children: keywords.map((concern) {
                return ActionChip(
                  label: Text(concern),
                  onPressed: () {
                    _concernController.text = concern;
                    _getRecommendations();
                  },
                  shape: RoundedRectangleBorder(
                    borderRadius: BorderRadius.circular(20.0),
                    side: BorderSide(color: Colors.grey.shade300),
                  ),
                  backgroundColor: Colors.grey.shade100,
                );
              }).toList(),
            ),
          ],
        );
      }).toList(),
    );
  }

  Widget _buildResultArea() {
    if (_isLoading) {
      return const Center(child: CircularProgressIndicator());
    }

    if (_recommendation == null) {
      return const Center(
        child: Text('검색 결과가 없습니다.'),
      );
    }

    if (_recommendation!.resultType!.contains('concern')) {
      return _buildConcernResult(_recommendation!);
    } else {
      return _buildInfoResult(_recommendation!);
    }
  }

  Widget _buildSectionTitle(String title, IconData icon) {
    return Padding(
      padding: const EdgeInsets.only(bottom: 12.0),
      child: Row(
        children: [
          Icon(icon, color: Colors.grey.shade700, size: 22),
          const SizedBox(width: 8),
          Expanded(
            child: Text(
              title,
              style: const TextStyle(fontSize: 19, fontWeight: FontWeight.bold),
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildConcernResult(AiRecommendation recommendation) {
    return ListView(
      padding: const EdgeInsets.fromLTRB(16, 16, 16, 16),
      children: [
        _buildSectionTitle('추천 유효 성분', Icons.star_outline),
        _buildRecommendedIngredientChips(recommendation.recommendedIngredients),
        const SizedBox(height: 24),
        _buildSectionTitle('내 노트 기반 추천 제품', Icons.inventory_2_outlined),
        _buildMatchingProductList(recommendation.matchingProducts),
      ],
    );
  }

  Widget _buildInfoResult(AiRecommendation recommendation) {
    final title = recommendation.resultTitle ?? '정보';
    final data = recommendation.singleItemInfo ?? {};

    const keyMap = {
      'type': '종류', 'alias': '별칭', 'function': '주요 기능',
      'description': '상세 설명', '작용 기전': '작용 원리', 'principle': '원리',
      'ingredients': '주요 성분', 'pros': '장점', 'cons': '단점',
      'characteristics': '특징', 'recommendation': '추천 대상', 'skin_tone': '피부톤 특징',
      'accessory': '어울리는 액세서리', 'color_palette': '추천 컬러', 'tip_question': '진단 질문 TIP',
      'pair_product': '함께 쓰면 좋은 제품', 'selling_point': '추천 포인트', 'sub_types': '하위 종류',
      'caution': '주의사항', 'synergy': '시너지 성분', '임상 비교': '임상 비교', '제형 정보': '제형 정보',
      'title': '제목', 'effect': '영향', 'strategy': '전략', 'components': '구성 요소',
      'definition': '정의', 'fact': '사실', 'mechanism': '메커니즘', 'pros_cons': '장단점',
      'layers': '구조', 'cycles': '주기', 'summary': '요약'
    };

    final keyOrder = keyMap.keys.toList();
    final List<Widget> infoWidgets = [];

    for (var key in keyOrder) {
      if (data.containsKey(key) && data[key].toString().isNotEmpty) {
        infoWidgets.add(
            Padding(
              padding: const EdgeInsets.symmetric(vertical: 8.0),
              child: Row(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  SizedBox(
                    width: 100,
                    child: Text(
                      keyMap[key] ?? key,
                      style: TextStyle(fontWeight: FontWeight.bold, color: Theme.of(context).primaryColor),
                    ),
                  ),
                  const SizedBox(width: 12),
                  Expanded(
                    child: Text(data[key].toString(), style: const TextStyle(fontSize: 15, height: 1.5)),
                  ),
                ],
              ),
            )
        );
        infoWidgets.add(const Divider(height: 1, thickness: 0.5));
      }
    }
    if (infoWidgets.isNotEmpty) infoWidgets.removeLast();

    return ListView(
      padding: const EdgeInsets.fromLTRB(16, 16, 16, 16),
      children: [
        _buildSectionTitle(title, Icons.info_outline),
        Card(
          elevation: 2,
          child: Padding(
            padding: const EdgeInsets.all(16.0),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: infoWidgets.isEmpty ? [Text(data.toString())] : infoWidgets,
            ),
          ),
        ),
      ],
    );
  }

  Widget _buildRecommendedIngredientChips(List<String> ingredients) {
    return Card(
      child: Padding(
        padding: const EdgeInsets.all(12.0),
        child: Wrap(
          spacing: 8.0,
          runSpacing: 4.0,
          children: ingredients.map((ingredient) => Chip(
            label: Text(ingredient, style: const TextStyle(fontWeight: FontWeight.w500)),
            backgroundColor: Theme.of(context).primaryColor.withOpacity(0.1),
            side: BorderSide(color: Theme.of(context).primaryColor.withOpacity(0.3)),
          )).toList(),
        ),
      ),
    );
  }

  Widget _buildMatchingProductList(List<RecommendedProduct> products) {
    if (products.isEmpty) {
      return const Card(
        child: Padding(
          padding: EdgeInsets.all(16.0),
          child: Center(
            child: Text('추천 성분을 포함하는 제품이\n내 노트에 없습니다.', textAlign: TextAlign.center, style: TextStyle(color: Colors.grey)),
          ),
        ),
      );
    }

    return Column(
      children: products.map((recProduct) {
        final product = recProduct.product;
        return Card(
          margin: const EdgeInsets.only(bottom: 8.0),
          child: ListTile(
            title: Text(product.productName, style: const TextStyle(fontWeight: FontWeight.bold)),
            subtitle: Text("추천 이유: '${recProduct.matchedIngredient}' 성분 포함", style: TextStyle(color: Theme.of(context).primaryColor, fontWeight: FontWeight.w500)),
            trailing: const Icon(Icons.arrow_forward_ios, size: 16),
            onTap: () {
              Navigator.push(context, MaterialPageRoute(builder: (_) => ProductAddScreen(product: product)));
            },
          ),
        );
      }).toList(),
    );
  }
}