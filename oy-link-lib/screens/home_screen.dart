import 'package:flutter/material.dart';
import 'package:oy_pro_link/providers/map_provider.dart';
import 'package:oy_pro_link/providers/product_provider.dart';
import 'package:oy_pro_link/providers/shelf_provider.dart';
import 'package:oy_pro_link/screens/product/product_list_screen.dart';
import 'package:oy_pro_link/screens/shelf/shelf_list_screen.dart';
import 'package:oy_pro_link/screens/smart_map/smart_map_screen.dart';
import 'package:oy_pro_link/widgets/menu_card.dart';
import 'package:provider/provider.dart';

import 'ai_assistant/ai_assistant_screen.dart';
import 'ingredient_checker/ingredient_checker_screen.dart';
import 'learning_center/learning_center_screen.dart';

class HomeScreen extends StatelessWidget {
  const HomeScreen({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('OY Pro-Link'),
      ),
      body: SingleChildScrollView(
        child: Padding(
          padding: const EdgeInsets.all(16.0),
          child: Column(
            children: [
              // ### 대시보드 위젯 추가 ###
              _buildDashboard(context),
              const SizedBox(height: 16),
              // ### 기존 메뉴 그리드 ###
              GridView.count(
                crossAxisCount: 2,
                crossAxisSpacing: 16.0,
                mainAxisSpacing: 16.0,
                shrinkWrap: true, // SingleChildScrollView 안에서 사용하기 위해 추가
                physics: const NeverScrollableScrollPhysics(), // SingleChildScrollView 안에서 사용하기 위해 추가
                children: <Widget>[
                  MenuCard(
                    icon: Icons.inventory,
                    title: '진열대 관리',
                    onTap: () {
                      Navigator.push(
                        context,
                        MaterialPageRoute(builder: (_) => const ShelfListScreen()),
                      );
                    },
                  ),
                  MenuCard(
                    icon: Icons.map_outlined,
                    title: '우리 매장 스마트 맵',
                    onTap: () {
                      Navigator.push(
                        context,
                        MaterialPageRoute(builder: (_) => const SmartMapScreen()),
                      );
                    },
                  ),
                  MenuCard(
                    icon: Icons.note_alt_outlined,
                    title: '내 손안의 제품 노트',
                    onTap: () {
                      Navigator.push(
                        context,
                        MaterialPageRoute(builder: (_) => const ProductListScreen()),
                      );
                    },
                  ),
                  MenuCard(
                    icon: Icons.psychology_outlined,
                    title: '뷰티 AI 어시스턴트',
                    onTap: () {
                      Navigator.push(
                        context,
                        MaterialPageRoute(builder: (_) => const AiAssistantScreen()),
                      );
                    },
                  ),
                  MenuCard(
                    icon: Icons.science_outlined,
                    title: '성분 궁합 체크',
                    onTap: () {
                      Navigator.push(
                        context,
                        MaterialPageRoute(builder: (_) => const IngredientCheckerScreen()),
                      );
                    },
                  ),
                  MenuCard(
                    icon: Icons.school_outlined,
                    title: '학습 센터',
                    onTap: () {
                      Navigator.push(
                        context,
                        MaterialPageRoute(builder: (_) => const LearningCenterScreen()),
                      );
                    },
                  ),
                ],
              ),
            ],
          ),
        ),
      ),
    );
  }

  // ### 대시보드 위젯을 만드는 함수 ###
  Widget _buildDashboard(BuildContext context) {
    final productProvider = context.watch<ProductProvider>();
    final shelfProvider = context.watch<ShelfProvider>();
    final mapProvider = context.watch<MapProvider>();

    final unassignedCount = productProvider.unassignedProductCount;

    return Card(
      elevation: 4,
      child: Padding(
        padding: const EdgeInsets.all(16.0),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            const Text(
              '매장 현황 요약',
              style: TextStyle(fontSize: 18, fontWeight: FontWeight.bold),
            ),
            const Divider(height: 24),
            Row(
              mainAxisAlignment: MainAxisAlignment.spaceAround,
              children: [
                _buildDashboardItem(context, '총 제품', productProvider.products.length.toString(), Icons.note_alt_outlined),
                _buildDashboardItem(context, '총 진열대', shelfProvider.shelves.length.toString(), Icons.inventory_2_outlined),
                _buildDashboardItem(context, '지도 위 진열대', mapProvider.objects.length.toString(), Icons.map_outlined),
              ],
            ),
            if (unassignedCount > 0) ...[
              const SizedBox(height: 16),
              Container(
                padding: const EdgeInsets.symmetric(horizontal: 12, vertical: 8),
                decoration: BoxDecoration(
                  color: Colors.orange.withOpacity(0.15),
                  borderRadius: BorderRadius.circular(8),
                ),
                child: Row(
                  children: [
                    Icon(Icons.warning_amber_rounded, color: Colors.orange.shade800, size: 20),
                    const SizedBox(width: 8),
                    Expanded(
                      child: Text(
                        '위치가 지정되지 않은 제품이 $unassignedCount개 있습니다.',
                        style: TextStyle(color: Colors.orange.shade900, fontWeight: FontWeight.w500),
                      ),
                    ),
                  ],
                ),
              )
            ]
          ],
        ),
      ),
    );
  }

  Widget _buildDashboardItem(BuildContext context, String title, String value, IconData icon) {
    return Column(
      children: [
        Icon(icon, size: 28, color: Theme.of(context).primaryColor),
        const SizedBox(height: 4),
        Text(title, style: const TextStyle(fontSize: 13, color: Colors.grey)),
        const SizedBox(height: 2),
        Text(value, style: const TextStyle(fontSize: 18, fontWeight: FontWeight.bold)),
      ],
    );
  }
}