// lib/screens/smart_map/smart_map_screen.dart

import 'package:flutter/material.dart';
import 'package:oy_pro_link/models/map_object_model.dart';
import 'package:oy_pro_link/models/shelf_model.dart';
import 'package:oy_pro_link/providers/map_provider.dart';
import 'package:oy_pro_link/providers/shelf_provider.dart';
import 'package:provider/provider.dart';

import '../../models/product_model.dart';
import '../../providers/product_provider.dart';

// 제품 검색 UI와 로직을 담당하는 SearchDelegate
class ProductSearchResult {
  final int mapObjectId;
  final String shelfName;
  final int? shelfLevel;
  final String productName;

  ProductSearchResult({
    required this.mapObjectId,
    required this.shelfName,
    this.shelfLevel,
    required this.productName,
  });
}

// 제품 검색 UI와 로직을 담당하는 SearchDelegate
class ProductSearchDelegate extends SearchDelegate<ProductSearchResult?> { // 반환 타입 변경
  final List<Product> allProducts;
  final List<MapObject> allMapObjects;

  ProductSearchDelegate({required this.allProducts, required this.allMapObjects});

  @override
  String get searchFieldLabel => '제품명 또는 바코드 검색...';

  @override
  List<Widget>? buildActions(BuildContext context) {
    return [
      IconButton(icon: const Icon(Icons.clear), onPressed: () => query = ''),
    ];
  }

  @override
  Widget? buildLeading(BuildContext context) {
    return IconButton(
      icon: const Icon(Icons.arrow_back),
      onPressed: () => close(context, null),
    );
  }

  @override
  Widget buildResults(BuildContext context) => _buildSearchResults();

  @override
  Widget buildSuggestions(BuildContext context) => _buildSearchResults();

  Widget _buildSearchResults() {
    final suggestionList = query.isEmpty ? [] : allProducts.where((product) {
      final queryLower = query.toLowerCase();
      final nameMatch = product.productName.toLowerCase().contains(queryLower);
      final barcodeMatch = product.barcode?.contains(query) ?? false;
      return nameMatch || barcodeMatch;
    }).toList();

    // 지도에 배치된 진열대에 속한 제품만 필터링
    final results = suggestionList.where((product) {
      return product.shelfId != null && allMapObjects.any((obj) => obj.shelfId == product.shelfId);
    }).toList();

    if (query.isNotEmpty && results.isEmpty) {
      return const Center(child: Text('지도에 등록된 제품을 찾을 수 없습니다.'));
    }

    return ListView.builder(
      itemCount: results.length,
      itemBuilder: (context, index) {
        final product = results[index];
        return ListTile(
          title: Text(product.productName),
          subtitle: Text(product.brand ?? ''),
          onTap: () {
            // ### 2. 검색 결과로 'ProductSearchResult' 객체를 반환하도록 수정 ###
            final mapObject = allMapObjects.firstWhere((obj) => obj.shelfId == product.shelfId);
            final result = ProductSearchResult(
              mapObjectId: mapObject.id!,
              shelfName: mapObject.shelf?.name ?? '알 수 없는 진열대',
              shelfLevel: product.shelfLevel,
              productName: product.productName,
            );
            close(context, result);
          },
        );
      },
    );
  }
}


// 스마트 맵 메인 위젯
class SmartMapScreen extends StatefulWidget {
  const SmartMapScreen({super.key});

  @override
  State<SmartMapScreen> createState() => _SmartMapScreenState();
}

class _SmartMapScreenState extends State<SmartMapScreen> {
  @override
  void initState() {
    super.initState();
    WidgetsBinding.instance.addPostFrameCallback((_) {
      Provider.of<ProductProvider>(context, listen: false).fetchProducts();
      Provider.of<ShelfProvider>(context, listen: false).fetchAllShelves();
      Provider.of<MapProvider>(context, listen: false).loadMapObjects();
    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('스마트 맵 에디터'),
        actions: [
          Consumer<MapProvider>(
            builder: (context, mapProvider, child) {
              return IconButton(
                icon: const Icon(Icons.search),
                onPressed: () async {
                  final productProvider = context.read<ProductProvider>();
                  // ### 3. 검색 결과(ProductSearchResult)를 받고 스낵바 표시 ###
                  final searchResult = await showSearch<ProductSearchResult?>(
                    context: context,
                    delegate: ProductSearchDelegate(
                      allProducts: productProvider.products,
                      allMapObjects: mapProvider.objects,
                    ),
                  );

                  if (searchResult != null && mounted) {
                    // 1. 지도 위의 블록 하이라이트
                    mapProvider.highlightObject(searchResult.mapObjectId);

                    // 2. 하단에 상세 위치 정보 스낵바 표시
                    ScaffoldMessenger.of(context).showSnackBar(
                      SnackBar(
                        content: Text(
                            "'${searchResult.productName}'은(는) "
                                "'${searchResult.shelfName}' 진열대 "
                                "${searchResult.shelfLevel ?? '?'}번째 층에 있습니다."
                        ),
                        duration: const Duration(seconds: 4),
                        action: SnackBarAction(
                          label: '확인',
                          onPressed: () {
                            ScaffoldMessenger.of(context).hideCurrentSnackBar();
                          },
                        ),
                      ),
                    );
                  }
                },
              );
            },
          ),

          // 2. 편집 모드일 때만 '추가/삭제' 버튼 표시
          Consumer<MapProvider>(builder: (context, map, child) {
            if (!map.isEditMode) return const SizedBox.shrink();
            return Row(
              children: [
                IconButton(
                  icon: const Icon(Icons.add_box_outlined),
                  onPressed: () => _showShelfSelectionDialog(context),
                  tooltip: '진열대 블록 추가',
                ),
                IconButton(
                  icon: const Icon(Icons.delete_forever, color: Colors.red),
                  onPressed: map.selectedObjectId == null ? null : () {
                    context.read<MapProvider>().deleteSelectedObject();
                  },
                  tooltip: '선택한 블록 삭제',
                ),
              ],
            );
          }),

          // 3. 편집 모드 토글 스위치
          const Padding(
            padding: EdgeInsets.symmetric(vertical: 16.0),
            child: VerticalDivider(),
          ),
          const Text('편집', style: TextStyle(fontSize: 12)),
          Consumer<MapProvider>(builder: (context, map, child) {
            return Switch(
              value: map.isEditMode,
              onChanged: (_) => map.toggleEditMode(),
            );
          }),
        ],
      ),
      body: const MapCanvas(),
    );
  }

  Future<void> _showShelfSelectionDialog(BuildContext context) async {
    final mapProvider = context.read<MapProvider>();
    final shelves = context.read<ShelfProvider>().shelves;

    final unmappedShelves = shelves.where((shelf) {
      return !mapProvider.objects.any((obj) => obj.shelfId == shelf.id);
    }).toList();

    if (unmappedShelves.isEmpty) {
      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(content: Text('모든 진열대가 이미 지도에 추가되었습니다.')),
      );
      return;
    }

    final selectedShelf = await showDialog<Shelf>(
        context: context,
        builder: (context) => AlertDialog(
          title: const Text('지도에 추가할 진열대 선택'),
          content: SizedBox(
            width: double.maxFinite,
            child: ListView.builder(
              itemCount: unmappedShelves.length,
              shrinkWrap: true,
              itemBuilder: (context, i) => ListTile(
                title: Text(unmappedShelves[i].name),
                onTap: () => Navigator.of(context).pop(unmappedShelves[i]),
              ),
            ),
          ),
        ));

    if (selectedShelf != null) {
      mapProvider.addNewObject(selectedShelf.id!, const Offset(150, 150));
    }
  }
}

class MapCanvas extends StatelessWidget {
  const MapCanvas({super.key});

  @override
  Widget build(BuildContext context) {
    final mapProvider = context.watch<MapProvider>();
    return InteractiveViewer(
      minScale: 0.2,
      maxScale: 5.0,
      child: GestureDetector(
        onTap: () {
          // 배경 탭 시, 선택 및 하이라이트 모두 해제
          mapProvider.selectObject(null);
          mapProvider.highlightObject(null);
        },
        child: Container(
          color: Colors.grey[200],
          width: 2000,
          height: 2000,
          child: Stack(
            children: mapProvider.objects
                .map((obj) => DraggableResizableObject(object: obj))
                .toList(),
          ),
        ),
      ),
    );
  }
}

class DraggableResizableObject extends StatelessWidget {
  final MapObject object;
  const DraggableResizableObject({super.key, required this.object});

  @override
  Widget build(BuildContext context) {
    final mapProvider = context.watch<MapProvider>();
    // ### 수정: isSelected와 isHighlighted 상태를 모두 확인 ###
    final isSelected = mapProvider.selectedObjectId == object.id;
    final isHighlighted = mapProvider.highlightedObjectId == object.id;

    // 선택 또는 하이라이트 상태일 때 시각적 강조 효과 적용
    final bool isEmphasized = isSelected || isHighlighted;

    return Positioned(
      left: object.dx,
      top: object.dy,
      width: object.width,
      height: object.height,
      child: GestureDetector(
        onPanUpdate: (details) {
          if (mapProvider.isEditMode) {
            mapProvider.updateObject(object..dx += details.delta.dx ..dy += details.delta.dy);
          }
        },
        onTap: () {
          // 탭하면 '선택' 상태가 됨 (편집 모드에서만)
          mapProvider.selectObject(object.id);
        },
        child: Stack(
          clipBehavior: Clip.none,
          children: [
            Container(
              decoration: BoxDecoration(
                // ### 수정: isEmphasized 값에 따라 색상 변경 ###
                color: isHighlighted
                    ? Colors.green.withOpacity(0.7)
                    : isSelected
                    ? Colors.blue.withOpacity(0.5)
                    : Colors.grey.withOpacity(0.7),
                border: Border.all(
                  color: isEmphasized ? Colors.black : Colors.black54,
                  width: isEmphasized ? 2 : 1,
                ),
                borderRadius: BorderRadius.circular(4),
              ),
              child: Center(
                child: Padding(
                  padding: const EdgeInsets.all(4.0),
                  child: Text(
                    object.shelf?.name ?? 'N/A',
                    style: const TextStyle(
                        color: Colors.white, fontWeight: FontWeight.bold, fontSize: 12),
                    textAlign: TextAlign.center,
                    overflow: TextOverflow.ellipsis,
                  ),
                ),
              ),
            ),
            if (mapProvider.isEditMode && isSelected)
              Positioned(
                right: -10,
                bottom: -10,
                child: ResizeHandle(
                  onDrag: (details) {
                    final updatedObject = object;
                    updatedObject.width = (updatedObject.width + details.delta.dx).clamp(50.0, 1000.0);
                    updatedObject.height = (updatedObject.height + details.delta.dy).clamp(50.0, 1000.0);
                    mapProvider.updateObject(updatedObject);
                  },
                ),
              ),
          ],
        ),
      ),
    );
  }
}

class ResizeHandle extends StatelessWidget {
  final void Function(DragUpdateDetails) onDrag;
  const ResizeHandle({super.key, required this.onDrag});

  @override
  Widget build(BuildContext context) {
    return GestureDetector(
      onPanUpdate: onDrag,
      child: Container(
        width: 20,
        height: 20,
        decoration: BoxDecoration(
          color: Colors.blue.withAlpha(204),
          shape: BoxShape.circle,
          border: Border.all(color: Colors.white, width: 2),
        ),
        child: const Icon(Icons.open_with, color: Colors.white, size: 12),
      ),
    );
  }
}