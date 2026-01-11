import 'package:flutter/material.dart';
import '../core/database_helper.dart';
import '../models/product_model.dart';

class ProductProvider with ChangeNotifier {
  final DatabaseHelper _dbHelper = DatabaseHelper();

  List<Product> _products = [];
  List<Product> _filteredProducts = [];
  bool _isLoading = false;

  List<Product> get products => _products;
  List<Product> get filteredProducts => _filteredProducts;
  bool get isLoading => _isLoading;

  // ### 대시보드용 데이터 getter 추가 ###
  int get unassignedProductCount {
    return _products.where((p) => p.shelfId == null).length;
  }

  Future<void> fetchProducts() async {
    _isLoading = true;
    notifyListeners();

    _products = await _dbHelper.getAllProducts();
    _filteredProducts = _products;
    _isLoading = false;
    notifyListeners();
  }

  void searchProducts(String query) {
    if (query.isEmpty) {
      _filteredProducts = _products;
    } else {
      _filteredProducts = _products.where((product) {
        final queryLower = query.toLowerCase();
        final nameMatch = product.productName.toLowerCase().contains(queryLower);
        final brandMatch = product.brand?.toLowerCase().contains(queryLower) ?? false;
        final barcodeMatch = product.barcode?.contains(query) ?? false;
        return nameMatch || brandMatch || barcodeMatch;
      }).toList();
    }
    notifyListeners();
  }

  Future<void> addProduct(Product product) async {
    await _dbHelper.insertProduct(product);
    await fetchProducts();
  }

  Future<void> updateProduct(Product product) async {
    await _dbHelper.updateProduct(product);
    await fetchProducts();
  }

  Future<void> deleteProduct(int id) async {
    await _dbHelper.deleteProduct(id);
    await fetchProducts();
  }

  Future<Product?> findProductByBarcode(String barcode) async {
    if (_products.isEmpty) {
      await fetchProducts();
    }
    try {
      final product = _products.firstWhere((p) => p.barcode == barcode);
      return product;
    } catch (e) {
      return null;
    }
  }
}