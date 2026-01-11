import 'package:flutter/material.dart';
import 'package:oy_pro_link/core/ingredient_checker.dart';
import 'package:oy_pro_link/models/compatibility_result.dart';
import 'package:oy_pro_link/models/product_model.dart';
import 'package:oy_pro_link/providers/product_provider.dart';
import 'package:provider/provider.dart';

class IngredientCheckerScreen extends StatefulWidget {
  const IngredientCheckerScreen({super.key});

  @override
  State<IngredientCheckerScreen> createState() => _IngredientCheckerScreenState();
}

class _IngredientCheckerScreenState extends State<IngredientCheckerScreen> {
  final _ingredientAController = TextEditingController();
  final _ingredientBController = TextEditingController();
  final _checker = IngredientChecker();

  CompatibilityResult? _result;
  bool _isLoading = false;

  @override
  void initState() {
    super.initState();
    // 위젯이 빌드된 후 첫 프레임에서 데이터를 로드하여 build 중에 상태가 변경되는 오류를 방지합니다.
    WidgetsBinding.instance.addPostFrameCallback((_) {
      Provider.of<ProductProvider>(context, listen: false).fetchProducts();
    });
  }

  @override
  void dispose() {
    _ingredientAController.dispose();
    _ingredientBController.dispose();
    super.dispose();
  }

  // '내 노트에서 불러오기'를 눌렀을 때 제품 선택 다이얼로그를 띄우는 함수
  Future<void> _selectProductForIngredient(TextEditingController controller) async {
    final products = Provider.of<ProductProvider>(context, listen: false).products;
    if (products.isEmpty) {
      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(content: Text('먼저 \'내 제품 노트\'에 제품을 추가해주세요.')),
      );
      return;
    }

    // 제품 목록을 보여주는 다이얼로그 실행
    final selectedProduct = await showDialog<Product>(
      context: context,
      builder: (context) => AlertDialog(
        title: const Text('제품 선택'),
        content: SizedBox(
          width: double.maxFinite,
          child: ListView.builder(
            itemCount: products.length,
            shrinkWrap: true,
            itemBuilder: (context, index) {
              final product = products[index];
              return ListTile(
                title: Text(product.productName),
                onTap: () => Navigator.of(context).pop(product),
              );
            },
          ),
        ),
      ),
    );

    if (selectedProduct != null) {
      // 제품을 선택하면 해당 제품의 전성분으로 텍스트 필드를 업데이트
      setState(() {
        controller.text = selectedProduct.ingredients ?? '';
      });
    }
  }

  // '궁합 확인' 버튼을 눌렀을 때 실행되는 함수
  void _checkCompatibility() async {
    FocusScope.of(context).unfocus(); // 키보드 숨기기
    setState(() {
      _isLoading = true;
      _result = null;
    });

    final result = await _checker.check(
      _ingredientAController.text,
      _ingredientBController.text,
    );

    setState(() {
      _result = result;
      _isLoading = false;
    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('성분 궁합 체크'),
      ),
      body: SingleChildScrollView(
        padding: const EdgeInsets.all(20.0),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.stretch,
          children: [
            // 첫 번째 성분 입력 위젯
            _buildIngredientInput(
              controller: _ingredientAController,
              labelText: '첫 번째 성분 / 전성분',
              hintText: '궁합을 확인할 성분이나 전성분을 입력하세요',
            ),
            const Padding(
              padding: EdgeInsets.symmetric(vertical: 16.0),
              child: Icon(Icons.add, size: 28, color: Colors.grey),
            ),
            // 두 번째 성분 입력 위젯
            _buildIngredientInput(
              controller: _ingredientBController,
              labelText: '두 번째 성분 / 전성분',
              hintText: '궁합을 확인할 성분이나 전성분을 입력하세요',
            ),
            const SizedBox(height: 24),
            ElevatedButton.icon(
              icon: const Icon(Icons.science_outlined),
              label: const Text('궁합 확인'),
              onPressed: _checkCompatibility,
              style: ElevatedButton.styleFrom(
                padding: const EdgeInsets.symmetric(vertical: 16),
                textStyle: const TextStyle(fontSize: 16, fontWeight: FontWeight.bold),
                backgroundColor: Theme.of(context).primaryColor,
                foregroundColor: Colors.white,
              ),
            ),
            const SizedBox(height: 32),
            // 로딩 중이거나 결과가 있을 때 결과 카드 표시
            if (_isLoading)
              const Center(child: CircularProgressIndicator())
            else if (_result != null)
              _buildResultCard(_result!),
          ],
        ),
      ),
    );
  }

  // 성분 입력 필드와 '불러오기' 버튼을 함께 묶은 위젯
  Widget _buildIngredientInput({
    required TextEditingController controller,
    required String labelText,
    required String hintText,
  }) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.stretch,
      children: [
        TextField(
          controller: controller,
          decoration: InputDecoration(
            labelText: labelText,
            hintText: hintText,
            border: const OutlineInputBorder(),
          ),
          minLines: 3,
          maxLines: 5,
        ),
        const SizedBox(height: 8),
        Align(
          alignment: Alignment.centerRight,
          child: TextButton(
            onPressed: () => _selectProductForIngredient(controller),
            child: const Text('내 노트에서 불러오기'),
          ),
        ),
      ],
    );
  }

  // 궁합 분석 결과를 보여주는 카드 위젯
  Widget _buildResultCard(CompatibilityResult result) {
    IconData icon;
    Color color;
    String title;

    switch (result.type) {
      case CompatibilityType.synergy:
        icon = Icons.auto_awesome;
        color = Colors.blue.shade700;
        title = '시너지 효과!';
        break;
      case CompatibilityType.antagonistic:
        icon = Icons.warning_amber_rounded;
        color = Colors.orange.shade800;
        title = '주의 필요!';
        break;
      case CompatibilityType.neutral:
        icon = Icons.info_outline;
        color = Colors.grey.shade600;
        title = '확인 결과';
        break;
    }

    return Card(
      elevation: 4,
      color: color.withOpacity(0.08),
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.circular(12),
        side: BorderSide(color: color, width: 1.5),
      ),
      child: Padding(
        padding: const EdgeInsets.all(16.0),
        child: Column(
          children: [
            Row(
              mainAxisAlignment: MainAxisAlignment.center,
              children: [
                Icon(icon, color: color, size: 28),
                const SizedBox(width: 8),
                Text(
                  title,
                  style: TextStyle(
                    fontSize: 20,
                    fontWeight: FontWeight.bold,
                    color: color,
                  ),
                ),
              ],
            ),
            const Divider(height: 24),
            Text(
              result.message,
              textAlign: TextAlign.center,
              style: const TextStyle(fontSize: 15, height: 1.5),
            ),
          ],
        ),
      ),
    );
  }
}