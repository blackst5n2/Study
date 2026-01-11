class Shelf {
  final int? id;
  final String name; // 예: "헤어케어 A-1", "기초 스킨케어 B-2"
  final String? memo;

  Shelf({this.id, required this.name, this.memo});

  Map<String, dynamic> toMap() {
    return {
      'id': id,
      'name': name,
      'memo': memo,
    };
  }

  factory Shelf.fromMap(Map<String, dynamic> map) {
    return Shelf(
      id: map['id'] as int?,
      name: map['name'] as String,
      memo: map['memo'] as String?,
    );
  }
}