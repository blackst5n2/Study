// lib/models/map_object_model.dart

import 'package:oy_pro_link/models/shelf_model.dart';

class MapObject {
  final int? id;
  final int shelfId;
  double dx; // x 좌표
  double dy; // y 좌표
  double width;
  double height;

  final Shelf? shelf; // JOIN 결과용

  MapObject({
    this.id,
    required this.shelfId,
    required this.dx,
    required this.dy,
    this.width = 100.0, // 기본 너비
    this.height = 50.0,  // 기본 높이
    this.shelf,
  });

  MapObject copyWith({
    int? id,
    int? shelfId,
    double? dx,
    double? dy,
    double? width,
    double? height,
    Shelf? shelf,
  }) {
    return MapObject(
      id: id ?? this.id,
      shelfId: shelfId ?? this.shelfId,
      dx: dx ?? this.dx,
      dy: dy ?? this.dy,
      width: width ?? this.width,
      height: height ?? this.height,
      shelf: shelf ?? this.shelf,
    );
  }

  Map<String, dynamic> toMap() {
    return {
      'id': id,
      'shelf_id': shelfId,
      'pos_x': dx,
      'pos_y': dy,
      'width': width,
      'height': height,
    };
  }

  factory MapObject.fromMap(Map<String, dynamic> map) {
    return MapObject(
      id: map['id'] as int?,
      shelfId: map['shelf_id'] as int,
      dx: map['pos_x'] as double,
      dy: map['pos_y'] as double,
      width: map['width'] as double,
      height: map['height'] as double,
      shelf: map.containsKey('name') ? Shelf.fromMap(map) : null,
    );
  }
}