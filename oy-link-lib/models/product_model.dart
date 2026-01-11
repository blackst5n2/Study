class Product {
  final int? id;
  final String productName;
  final String? brand;
  final String? barcode;
  final String? ingredients;
  final String? memo;
  final String? imageUrl;
  final int? shelfId;
  final int? shelfLevel;

  Product({
    this.id,
    required this.productName,
    this.brand,
    this.barcode,
    this.ingredients,
    this.memo,
    this.imageUrl,
    this.shelfId,
    this.shelfLevel
  });

  // Dart 객체 -> Map (DB 저장을 위해)
  Map<String, dynamic> toMap() {
    return {
      'id': id,
      'product_name': productName,
      'brand': brand,
      'barcode': barcode,
      'ingredients': ingredients,
      'memo': memo,
      'image_url': imageUrl,
      'shelf_id': shelfId,
      'shelf_level': shelfLevel,
    };
  }

  // Map -> Dart 객체 (DB 조회를 위해)
  factory Product.fromMap(Map<String, dynamic> map) {
    return Product(
      id: map['id'] as int?,
      productName: map['product_name'] as String,
      brand: map['brand'] as String?,
      barcode: map['barcode'] as String?,
      ingredients: map['ingredients'] as String?,
      memo: map['memo'] as String?,
      imageUrl: map['image_url'] as String?,
      shelfId: map['shelf_id'] as int?,
      shelfLevel: map['shelf_level'] as int?,
    );
  }

  // 복사 생성자: 객체의 일부 필드만 변경하여 새로운 객체를 만들 때 유용
  Product copyWith({
    int? id,
    String? productName,
    String? brand,
    String? barcode,
    String? ingredients,
    String? memo,
    String? imageUrl,
  }) {
    return Product(
      id: id ?? this.id,
      productName: productName ?? this.productName,
      brand: brand ?? this.brand,
      barcode: barcode ?? this.barcode,
      ingredients: ingredients ?? this.ingredients,
      memo: memo ?? this.memo,
      imageUrl: imageUrl ?? this.imageUrl,
    );
  }
}