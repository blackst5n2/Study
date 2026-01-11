import 'package:flutter/material.dart';
import 'package:oy_pro_link/core/database_helper.dart';
import 'package:oy_pro_link/models/shelf_model.dart';

class ShelfProvider with ChangeNotifier {
  final DatabaseHelper _dbHelper = DatabaseHelper();
  List<Shelf> _shelves = [];
  bool _isLoading = false;

  List<Shelf> get shelves => _shelves;
  bool get isLoading => _isLoading;

  ShelfProvider();

  Future<void> fetchAllShelves() async {
    _isLoading = true;
    notifyListeners();
    _shelves = await _dbHelper.getAllShelves();
    _isLoading = false;
    notifyListeners();
  }

  // ### CRUD 메서드 추가 ###
  Future<void> addShelf(Shelf shelf) async {
    await _dbHelper.insertShelf(shelf);
    await fetchAllShelves(); // 목록 새로고침
  }

  Future<void> updateShelf(Shelf shelf) async {
    await _dbHelper.updateShelf(shelf);
    await fetchAllShelves();
  }

  Future<void> deleteShelf(int id) async {
    await _dbHelper.deleteShelf(id);
    await fetchAllShelves();
  }
}