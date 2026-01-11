import 'package:flutter/material.dart';
import 'package:oy_pro_link/models/product_model.dart';
import 'package:oy_pro_link/providers/product_provider.dart';
import 'package:provider/provider.dart';

import '../../providers/shelf_provider.dart';

class ProductAddScreen extends StatefulWidget {
  final Product? product;
  final String? barcode;

  // 생성자를 통해 기존 product 객체를 받을 수 있음 (수정 모드)
  const ProductAddScreen({super.key, this.product, this.barcode});

  @override
  State<ProductAddScreen> createState() => _ProductAddScreenState();
}

class _ProductAddScreenState extends State<ProductAddScreen> {
  final _formKey = GlobalKey<FormState>();

  // 각 입력 필드를 제어하기 위한 컨트롤러
  late TextEditingController _nameController;
  late TextEditingController _brandController;
  late TextEditingController _barcodeController;
  late TextEditingController _ingredientsController;
  late TextEditingController _memoController;
  late TextEditingController _shelfLevelController;
  int? _selectedShelfId;

  bool get _isEditing => widget.product != null;

  @override
  void initState() {
    super.initState();
    // 컨트롤러 초기화. 수정 모드일 경우 기존 데이터로 초기화
    _nameController = TextEditingController(text: widget.product?.productName ?? '');
    _brandController = TextEditingController(text: widget.product?.brand ?? '');
    _barcodeController = TextEditingController(text: widget.product?.barcode ?? widget.barcode ?? '');
    _ingredientsController = TextEditingController(text: widget.product?.ingredients ?? '');
    _memoController = TextEditingController(text: widget.product?.memo ?? '');
    _shelfLevelController = TextEditingController(text: widget.product?.shelfLevel?.toString() ?? '');
    _selectedShelfId = widget.product?.shelfId;
  }

  @override
  void dispose() {
    // 화면이 사라질 때 컨트롤러 리소스 해제
    _nameController.dispose();
    _brandController.dispose();
    _barcodeController.dispose();
    _ingredientsController.dispose();
    _memoController.dispose();
    _shelfLevelController.dispose();
    super.dispose();
  }

  Future<void> _saveProduct() async {
    // Form의 유효성 검사를 통과했는지 확인
    if (_formKey.currentState!.validate()) {
      final productProvider = Provider.of<ProductProvider>(context, listen: false);

      final newProduct = Product(
        id: widget.product?.id, // 수정 모드일 경우 기존 id 사용
        productName: _nameController.text,
        brand: _brandController.text,
        barcode: _barcodeController.text,
        ingredients: _ingredientsController.text,
        memo: _memoController.text,
        shelfId:  _selectedShelfId,
        shelfLevel: int.tryParse(_shelfLevelController.text),
      );

      try {
        if (_isEditing) {
          await productProvider.updateProduct(newProduct);
        } else {
          await productProvider.addProduct(newProduct);
        }

        // 작업 완료 후 이전 화면으로 돌아감
        if (mounted) {
          ScaffoldMessenger.of(context).showSnackBar(
            SnackBar(content: Text('제품이 성공적으로 저장되었습니다.')),
          );
          Navigator.of(context).pop();
        }
      } catch (e) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(content: Text('저장 중 오류가 발생했습니다: $e')),
        );
      }
    }
  }

  Future<void> _deleteProduct() async {
    final productProvider = Provider.of<ProductProvider>(context, listen: false);

    // 삭제 확인 다이얼로그 표시
    final bool? confirmDelete = await showDialog(
        context: context,
        builder: (context) => AlertDialog(
          title: Text('제품 삭제'),
          content: Text('${widget.product!.productName} 제품을 정말로 삭제하시겠습니까?'),
          actions: [
            TextButton(
              onPressed: () => Navigator.of(context).pop(false),
              child: Text('취소'),
            ),
            TextButton(
              onPressed: () => Navigator.of(context).pop(true),
              child: Text('삭제', style: TextStyle(color: Colors.red)),
            ),
          ],
        )
    );

    if (confirmDelete == true) {
      try {
        await productProvider.deleteProduct(widget.product!.id!);
        if (mounted) {
          ScaffoldMessenger.of(context).showSnackBar(
            SnackBar(content: Text('제품이 삭제되었습니다.')),
          );
          Navigator.of(context).pop();
        }
      } catch (e) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(content: Text('삭제 중 오류가 발생했습니다: $e')),
        );
      }
    }
  }


  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text(_isEditing ? '제품 수정' : '새 제품 추가'),
        actions: [
          if (_isEditing)
            IconButton(
              icon: Icon(Icons.delete_outline, color: Colors.red),
              onPressed: _deleteProduct,
            ),
          IconButton(
            icon: const Icon(Icons.save_outlined),
            onPressed: _saveProduct,
          ),
        ],
      ),
      body: SingleChildScrollView(
        padding: const EdgeInsets.all(16.0),
        child: Form(
          key: _formKey,
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.stretch,
            children: [
              TextFormField(
                controller: _nameController,
                decoration: const InputDecoration(labelText: '제품명 *'),
                validator: (value) {
                  if (value == null || value.isEmpty) {
                    return '제품명을 입력해주세요.';
                  }
                  return null;
                },
              ),
              const SizedBox(height: 16),
              TextFormField(
                controller: _brandController,
                decoration: const InputDecoration(labelText: '브랜드'),
              ),
              const SizedBox(height: 16),
              TextFormField(
                controller: _barcodeController,
                decoration: const InputDecoration(labelText: '바코드 번호'),
                keyboardType: TextInputType.number,
              ),
              const SizedBox(height: 16),
              TextFormField(
                controller: _ingredientsController,
                decoration: const InputDecoration(
                  labelText: '전성분',
                  alignLabelWithHint: true,
                ),
                maxLines: 5,
                keyboardType: TextInputType.multiline,
              ),
              const SizedBox(height: 16),
              // ### 진열대 선택 드롭다운 추가 ###
              Consumer<ShelfProvider>(
                builder: (context, shelfProvider, child) {
                  return DropdownButtonFormField<int>(
                    value: _selectedShelfId,
                    decoration: const InputDecoration(
                      labelText: '진열대 위치',
                      border: OutlineInputBorder(),
                    ),
                    hint: const Text('진열대를 선택하세요'),
                    items: shelfProvider.shelves.map((shelf) {
                      return DropdownMenuItem<int>(
                        value: shelf.id,
                        child: Text(shelf.name),
                      );
                    }).toList(),
                    onChanged: (value) {
                      setState(() {
                        _selectedShelfId = value;
                      });
                    },
                  );
                },
              ),
              const SizedBox(height: 16),
              // ### 진열대 칸 번호 입력 필드 추가 ###
              TextFormField(
                controller: _shelfLevelController,
                decoration: const InputDecoration(
                  labelText: '진열대 층수',
                  hintText: '예: 3',
                  border: OutlineInputBorder(),
                ),
                keyboardType: TextInputType.number,
              ),
              const SizedBox(height: 16),
              TextFormField(
                controller: _memoController,
                decoration: const InputDecoration(
                  labelText: '개인 메모',
                  alignLabelWithHint: true,
                ),
                maxLines: 3,
                keyboardType: TextInputType.multiline,
              ),
            ],
          ),
        ),
      ),
    );
  }
}