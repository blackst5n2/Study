import 'package:flutter/material.dart';
import 'package:oy_pro_link/core/database_helper.dart';
import 'package:oy_pro_link/models/map_object_model.dart';

class MapProvider with ChangeNotifier {
  final DatabaseHelper _dbHelper = DatabaseHelper();

  List<MapObject> _objects = [];
  bool _isEditMode = false;
  int? _selectedObjectId;
  int? _highlightedObjectId; // ### 검색 결과 하이라이트 전용 상태 추가 ###

  List<MapObject> get objects => _objects;
  bool get isEditMode => _isEditMode;
  int? get selectedObjectId => _selectedObjectId;
  int? get highlightedObjectId => _highlightedObjectId; // ### getter 추가 ###

  MapProvider();

  void toggleEditMode() {
    _isEditMode = !_isEditMode;
    _selectedObjectId = null;
    _highlightedObjectId = null; // 편집 모드 변경 시 하이라이트도 해제
    notifyListeners();
  }

  // 편집을 위한 '선택' 메서드 (편집 모드일 때만 동작)
  void selectObject(int? objectId) {
    if (!_isEditMode) {
      _selectedObjectId = null; // 편집 모드가 아니면 선택 상태는 항상 null
    } else {
      _selectedObjectId = objectId;
    }
    _highlightedObjectId = null; // 편집 선택 시 검색 하이라이트는 해제
    notifyListeners();
  }

  // ### 검색 결과를 위한 '하이라이트' 메서드 추가 (모드와 상관없이 동작) ###
  void highlightObject(int? objectId) {
    _highlightedObjectId = objectId;
    _selectedObjectId = null; // 검색 하이라이트 시 편집 선택은 해제
    notifyListeners();
  }

  Future<void> loadMapObjects() async {
    _objects = await _dbHelper.getAllMapObjects();
    notifyListeners();
  }

  Future<void> addNewObject(int shelfId, Offset position) async {
    final newObject = MapObject(shelfId: shelfId, dx: position.dx, dy: position.dy);
    await _dbHelper.addObject(newObject);
    await loadMapObjects();
  }

  Future<void> updateObject(MapObject object) async {
    final index = _objects.indexWhere((o) => o.id == object.id);
    if (index != -1) {
      _objects[index] = object;
      await _dbHelper.updateObject(object);
      notifyListeners();
    }
  }

  Future<void> deleteSelectedObject() async {
    if (_selectedObjectId != null) {
      await _dbHelper.deleteObject(_selectedObjectId!);
      _selectedObjectId = null;
      await loadMapObjects();
    }
  }
}