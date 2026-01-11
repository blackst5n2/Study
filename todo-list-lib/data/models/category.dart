// /lib/data/models/category.dart

import 'package:equatable/equatable.dart';

class Category extends Equatable{
  final int? id;
  final String name;
  final String colorCode;

  const Category({
    this.id,
    required this.name,
    required this.colorCode,
  });

  @override
  List<Object?> get props => [id, name, colorCode];

  // [추가] copyWith 메소드
  Category copyWith({
    int? id,
    String? name,
    String? colorCode,
  }) {
    return Category(
      id: id ?? this.id,
      name: name ?? this.name,
      colorCode: colorCode ?? this.colorCode,
    );
  }

  factory Category.fromMap(Map<String, dynamic> map) {
    return Category(
      id: map['id'],
      name: map['name'],
      colorCode: map['color_code'],
    );
  }

  Map<String, dynamic> toMap() {
    return {
      'id': id,
      'name': name,
      'color_code': colorCode,
    };
  }
}