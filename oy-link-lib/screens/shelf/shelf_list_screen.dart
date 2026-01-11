import 'package:flutter/material.dart';
import 'package:oy_pro_link/providers/shelf_provider.dart';
import 'package:oy_pro_link/screens/shelf/shelf_add_screen.dart';
import 'package:provider/provider.dart';

class ShelfListScreen extends StatefulWidget {
  const ShelfListScreen({super.key});

  @override
  State<ShelfListScreen> createState() => _ShelfListScreenState();
}

class _ShelfListScreenState extends State<ShelfListScreen> {
  @override
  void initState() {
    super.initState();
    WidgetsBinding.instance.addPostFrameCallback((_) {
      Provider.of<ShelfProvider>(context, listen: false).fetchAllShelves();
    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('진열대 관리'),
      ),
      body: Consumer<ShelfProvider>(
        builder: (context, shelfProvider, child) {
          if (shelfProvider.isLoading) {
            return const Center(child: CircularProgressIndicator());
          }
          if (shelfProvider.shelves.isEmpty) {
            return const Center(child: Text('등록된 진열대가 없습니다.'));
          }
          return ListView.builder(
            itemCount: shelfProvider.shelves.length,
            itemBuilder: (context, index) {
              final shelf = shelfProvider.shelves[index];
              return ListTile(
                title: Text(shelf.name),
                subtitle: shelf.memo?.isNotEmpty == true ? Text(shelf.memo!) : null,
                trailing: const Icon(Icons.edit_outlined, size: 20),
                onTap: () {
                  Navigator.push(
                    context,
                    MaterialPageRoute(builder: (_) => ShelfAddScreen(shelf: shelf)),
                  );
                },
              );
            },
          );
        },
      ),
      floatingActionButton: FloatingActionButton(
        tooltip: '새 진열대 추가',
        onPressed: () {
          Navigator.push(
            context,
            MaterialPageRoute(builder: (_) => const ShelfAddScreen()),
          );
        },
        child: const Icon(Icons.add),
      ),
    );
  }
}