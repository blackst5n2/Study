import 'package:flutter/material.dart';
import 'package:oy_pro_link/providers/product_provider.dart';
import 'package:oy_pro_link/screens/product/product_add_screen.dart';
import 'package:provider/provider.dart';

import '../barcode_scanner_screen.dart';

class ProductListScreen extends StatefulWidget {
  const ProductListScreen({super.key});

  @override
  State<ProductListScreen> createState() => _ProductListScreenState();
}

class _ProductListScreenState extends State<ProductListScreen> {
  bool _isSearching = false;
  final TextEditingController _searchController = TextEditingController();

  @override
  void initState() {
    super.initState();
    // 위젯이 빌드된 후 첫 프레임에서 딱 한 번만 데이터를 로드합니다.
    WidgetsBinding.instance.addPostFrameCallback((_) {
      Provider.of<ProductProvider>(context, listen: false).fetchProducts();
    });

    _searchController.addListener(() {
      Provider.of<ProductProvider>(context, listen: false)
          .searchProducts(_searchController.text);
    });
  }

  Future<void> _scanBarcode() async {
    // 스캐너 화면으로 이동하고 결과를 기다림
    final String? barcode = await Navigator.push<String>(
      context,
      MaterialPageRoute(builder: (_) => const BarcodeScannerScreen()),
    );

    if (barcode != null && mounted) {
      final productProvider = Provider.of<ProductProvider>(context, listen: false);
      final foundProduct = await productProvider.findProductByBarcode(barcode);

      if (foundProduct != null) {
        // 제품을 찾았으면 해당 제품의 수정 화면으로 바로 이동
        Navigator.push(
          context,
          MaterialPageRoute(builder: (_) => ProductAddScreen(product: foundProduct)),
        );
      } else {
        // 제품을 못 찾았으면, 바코드 번호가 미리 채워진 '새 제품 추가' 화면으로 이동
        final bool? createNew = await showDialog<bool>(
          context: context,
          builder: (context) => AlertDialog(
            title: const Text('제품 없음'),
            content: Text('\'$barcode\' 바코드를 가진 제품이 없습니다. 새로 추가하시겠습니까?'),
            actions: [
              TextButton(onPressed: () => Navigator.of(context).pop(false), child: const Text('취소')),
              TextButton(onPressed: () => Navigator.of(context).pop(true), child: const Text('새로 추가')),
            ],
          ),
        );

        if (createNew == true) {
          Navigator.push(
            context,
            MaterialPageRoute(builder: (_) => ProductAddScreen(barcode: barcode)),
          );
        }
      }
    }
  }

  AppBar _buildAppBar(BuildContext context) {
    if (_isSearching) {
      // 검색 모드일 때의 AppBar
      return AppBar(
        leading: IconButton(
          icon: const Icon(Icons.arrow_back),
          onPressed: () {
            setState(() {
              _isSearching = false;
              _searchController.clear();
            });
          },
        ),
        title: TextField(
          controller: _searchController,
          autofocus: true,
          decoration: const InputDecoration(
            hintText: '제품명, 브랜드, 바코드 번호 검색...',
            border: InputBorder.none,
          ),
          style: const TextStyle(color: Colors.black, fontSize: 18),
        ),
        actions: [
          IconButton(
            icon: const Icon(Icons.clear),
            onPressed: () {
              _searchController.clear();
            },
          ),
        ],
      );
    } else {
      // 기본 모드일 때의 AppBar
      return AppBar(
        title: const Text('내 제품 노트'),
        actions: [
          IconButton(
            icon: const Icon(Icons.qr_code_scanner),
            onPressed: _scanBarcode,
            tooltip: '바코드 스캔',
          ),
          IconButton(
            icon: const Icon(Icons.search),
            onPressed: () {
              setState(() {
                _isSearching = true;
              });
            },
          ),
        ],
      );
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: _buildAppBar(context),
      body: Consumer<ProductProvider>(
        builder: (context, productProvider, child) {
          if (productProvider.isLoading) {
            return const Center(child: CircularProgressIndicator());
          }

          // ### filteredProducts를 사용하여 목록 표시 ###
          final productsToShow = productProvider.filteredProducts;

          if (productsToShow.isEmpty) {
            return Center(
              child: Padding(
                padding: const EdgeInsets.all(16.0),
                child: Text(
                  _searchController.text.isNotEmpty
                      ? '검색 결과가 없습니다.'
                      : '저장된 제품이 없습니다.\n아래 + 버튼을 눌러 새 제품을 추가해보세요!',
                  textAlign: TextAlign.center,
                  style: const TextStyle(fontSize: 16, color: Colors.grey),
                ),
              ),
            );
          }

          return ListView.builder(
            padding: const EdgeInsets.all(8.0),
            itemCount: productsToShow.length,
            itemBuilder: (context, index) {
              final product = productsToShow[index];
              return Card(
                child: ListTile(
                  leading: const Icon(Icons.inventory_2_outlined, size: 40, color: Colors.grey),
                  title: Text(product.productName, style: const TextStyle(fontWeight: FontWeight.bold)),
                  // ### 수정된 부분 ###
                  subtitle: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      const SizedBox(height: 2), // 간격 조절
                      Text(product.brand ?? '브랜드 정보 없음'),
                      if (product.barcode != null && product.barcode!.isNotEmpty)
                        Padding(
                          padding: const EdgeInsets.only(top: 4.0),
                          child: Text(
                            '${product.barcode}',
                            style: TextStyle(color: Colors.black, fontSize: 15, fontWeight: FontWeight.w600),
                          ),
                        ),
                    ],
                  ),
                  trailing: const Icon(Icons.arrow_forward_ios, size: 16),
                  onTap: () {
                    Navigator.push(
                      context,
                      MaterialPageRoute(builder: (_) => ProductAddScreen(product: product)),
                    );
                  },
                ),
              );
            },
          );
        },
      ),
      floatingActionButton: FloatingActionButton(
        onPressed: () {
          Navigator.push(
            context,
            MaterialPageRoute(builder: (_) => const ProductAddScreen()),
          );
        },
        tooltip: '새 제품 추가',
        child: const Icon(Icons.add),
      ),
    );
  }
}