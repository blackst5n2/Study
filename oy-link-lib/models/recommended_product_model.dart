import 'package:oy_pro_link/models/product_model.dart';

class RecommendedProduct {
  final Product product;
  final String matchedIngredient;

  RecommendedProduct({required this.product, required this.matchedIngredient});
}