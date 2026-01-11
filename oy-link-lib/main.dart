import 'package:flutter/material.dart';
import 'package:oy_pro_link/providers/map_provider.dart';
import 'package:oy_pro_link/providers/product_provider.dart';
import 'package:oy_pro_link/providers/shelf_provider.dart';
import 'package:oy_pro_link/screens/home_screen.dart';
import 'package:provider/provider.dart';

void main() {
  runApp(const MyApp());
}

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MultiProvider(
      providers: [
        ChangeNotifierProvider(create: (_) => ProductProvider()),
        ChangeNotifierProvider(create: (_) => MapProvider()),
        ChangeNotifierProvider(create: (_) => ShelfProvider()),
      ],
      child: MaterialApp(
        title: 'OY Pro-Link',
        theme: ThemeData(
          primaryColor: const Color(0xFF6AB344),
          scaffoldBackgroundColor: const Color(0xFFF7F7F7),
          colorScheme: ColorScheme.fromSeed(
            seedColor: const Color(0xFF6AB344),
            primary: const Color(0xFF6AB344),
            secondary: const Color(0xFFF47920),
          ),

          appBarTheme: const AppBarTheme(
            backgroundColor: Colors.white,
            foregroundColor: Colors.black,
            elevation: 1,
            centerTitle: true,
            titleTextStyle: TextStyle(
              color: Colors.black,
              fontSize: 18,
              fontWeight: FontWeight.bold,
            ),
          ),

          // ERROR FIX: CardTheme -> CardThemeData
          cardTheme: CardThemeData(
            elevation: 2,
            shape: RoundedRectangleBorder(
              borderRadius: BorderRadius.circular(12.0),
            ),
            margin: const EdgeInsets.symmetric(horizontal: 8.0, vertical: 4.0),
            color: Colors.white,
          ),

          floatingActionButtonTheme: const FloatingActionButtonThemeData(
            backgroundColor: Color(0xFF6AB344),
            foregroundColor: Colors.white,
          ),

          useMaterial3: true,
        ),
        debugShowCheckedModeBanner: false,
        home: const HomeScreen(),
      ),
    );
  }
}