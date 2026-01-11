import 'package:flutter/material.dart';
import 'package:flutter_markdown/flutter_markdown.dart';
import 'package:oy_pro_link/core/database_helper.dart';
import 'package:markdown/markdown.dart' as md;
import 'package:scrollable_positioned_list/scrollable_positioned_list.dart';

// 사용자 정의 Syntax 클래스들은 변경사항 없습니다.
const sup = { '0': '⁰', '1': '¹', '2': '²', '3': '³', '4': '⁴', '5': '⁵', '6': '⁶', '7': '⁷', '8': '⁸', '9': '⁹' };
const sub = { '0': '₀', '1': '₁', '2': '₂', '3': '₃', '4': '₄', '5': '₅', '6': '₆', '7': '₇', '8': '₈', '9': '₉' };

class SuperscriptSyntax extends md.InlineSyntax {
  SuperscriptSyntax() : super(r'\^([^\^]+)\^');
  @override
  bool onMatch(md.InlineParser parser, Match match) {
    final content = match[1]!;
    String converted = '';
    for (var char in content.characters) { converted += sup[char] ?? char; }
    parser.addNode(md.Text(converted));
    return true;
  }
}

class SubscriptSyntax extends md.InlineSyntax {
  SubscriptSyntax() : super(r'~([^~]+)~');
  @override
  bool onMatch(md.InlineParser parser, Match match) {
    final content = match[1]!;
    String converted = '';
    for (var char in content.characters) { converted += sub[char] ?? char; }
    parser.addNode(md.Text(converted));
    return true;
  }
}

class LearningCenterScreen extends StatefulWidget {
  const LearningCenterScreen({super.key});
  @override
  State<LearningCenterScreen> createState() => _LearningCenterScreenState();
}

class _LearningCenterScreenState extends State<LearningCenterScreen> {
  late Future<List<String>> _paragraphsFuture;
  final DatabaseHelper _dbHelper = DatabaseHelper();

  final TextEditingController _searchController = TextEditingController();
  final ItemScrollController _itemScrollController = ItemScrollController();
  final ItemPositionsListener _itemPositionsListener = ItemPositionsListener.create();

  List<String> _paragraphs = [];
  List<int> _searchResultIndices = [];
  int _currentResultIndex = -1;
  String _currentQuery = '';

  // ### 추가된 상태 변수 ###
  bool _isSearching = false;
  final FocusNode _searchFocusNode = FocusNode();

  @override
  void initState() {
    super.initState();
    _paragraphsFuture = _dbHelper.getFullKnowledgeText().then((fullText) {
      _paragraphs = fullText.split(RegExp(r'\n\s*\n'));
      return _paragraphs;
    });
  }

  @override
  void dispose() {
    _searchController.dispose();
    _searchFocusNode.dispose(); // FocusNode를 dispose 해야 합니다.
    super.dispose();
  }

  void _searchText(String query) {
    _searchResultIndices.clear();
    _currentQuery = query;

    if (query.isNotEmpty) {
      for (int i = 0; i < _paragraphs.length; i++) {
        if (_paragraphs[i].toLowerCase().contains(query.toLowerCase())) {
          _searchResultIndices.add(i);
        }
      }
    }
    setState(() {
      _currentResultIndex = -1;
      if (_searchResultIndices.isNotEmpty) {
        _navigateToSearchResult(isNext: true);
      }
    });
  }

  void _navigateToSearchResult({required bool isNext}) {
    if (_searchResultIndices.isEmpty) return;

    if (isNext) {
      _currentResultIndex++;
      if (_currentResultIndex >= _searchResultIndices.length) {
        _currentResultIndex = 0;
      }
    } else {
      _currentResultIndex--;
      if (_currentResultIndex < 0) {
        _currentResultIndex = _searchResultIndices.length - 1;
      }
    }

    final targetParagraphIndex = _searchResultIndices[_currentResultIndex];

    _itemScrollController.scrollTo(
      index: targetParagraphIndex,
      duration: const Duration(milliseconds: 500),
      curve: Curves.easeInOutCubic,
    );
    setState(() {});
  }

  String _highlightQuery(String text) {
    if (_currentQuery.isEmpty) return text;
    final source = RegExp(RegExp.escape(_currentQuery), caseSensitive: false);
    return text.replaceAllMapped(source, (match) {
      if (match.group(0)!.contains('`')) {
        return match.group(0)!;
      }
      return '`${match.group(0)!}`';
    });
  }

  // ### AppBar를 빌드하는 메소드 (신규) ###
  AppBar _buildAppBar() {
    if (_isSearching) {
      // '검색 모드' AppBar
      return AppBar(
        leading: IconButton(
          icon: const Icon(Icons.arrow_back),
          onPressed: () {
            setState(() {
              _isSearching = false;
              _searchController.clear();
              _searchText(''); // 검색 결과 초기화
            });
          },
        ),
        title: TextField(
          controller: _searchController,
          focusNode: _searchFocusNode,
          autofocus: true,
          decoration: const InputDecoration(
            hintText: '학습 내용 검색...',
            border: InputBorder.none,
            hintStyle: TextStyle(color: Colors.black),
          ),
          style: const TextStyle(color: Colors.black, fontSize: 18.0),
          onSubmitted: _searchText,
        ),
        actions: [
          IconButton(
            icon: const Icon(Icons.clear),
            onPressed: () {
              _searchController.clear();
              // 검색창 내용만 지우고 검색 모드는 유지
            },
          )
        ],
      );
    } else {
      // '일반 모드' AppBar
      return AppBar(
        title: const Text('학습 센터'),
        actions: [
          IconButton(
            icon: const Icon(Icons.search),
            onPressed: () {
              setState(() {
                _isSearching = true;
              });
              // 검색창이 빌드된 후 포커스를 주기 위해 약간의 딜레이를 줍니다.
              WidgetsBinding.instance.addPostFrameCallback((_) {
                _searchFocusNode.requestFocus();
              });
            },
          )
        ],
      );
    }
  }

  // ### 검색 컨트롤 위젯 (신규) ###
  Widget _buildSearchControls() {
    // 검색 결과가 있을 때만 컨트롤 버튼들을 보여줍니다.
    if (_searchResultIndices.isEmpty) {
      return const SizedBox.shrink(); // 아무것도 보여주지 않음
    }

    return Padding(
      padding: const EdgeInsets.symmetric(horizontal: 8.0, vertical: 4.0),
      child: Row(
        mainAxisAlignment: MainAxisAlignment.end,
        children: [
          if (_currentResultIndex != -1)
            Text('${_currentResultIndex + 1}/${_searchResultIndices.length}'),

          IconButton(
            icon: const Icon(Icons.arrow_upward),
            onPressed: () => _navigateToSearchResult(isNext: false),
            tooltip: '이전 검색 결과',
          ),
          IconButton(
            icon: const Icon(Icons.arrow_downward),
            onPressed: () => _navigateToSearchResult(isNext: true),
            tooltip: '다음 검색 결과',
          ),
        ],
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: _buildAppBar(), // 상태에 따라 동적으로 AppBar 빌드
      body: Column(
        children: [
          // ### 검색 컨트롤 위젯을 여기에 배치 ###
          _buildSearchControls(),
          Expanded(
            child: FutureBuilder<List<String>>(
              future: _paragraphsFuture,
              builder: (context, snapshot) {
                if (snapshot.connectionState == ConnectionState.waiting) {
                  return const Center(child: CircularProgressIndicator());
                }
                if (snapshot.hasError || !snapshot.hasData || snapshot.data!.isEmpty) {
                  return const Center(child: Text('학습 자료를 불러올 수 없습니다.'));
                }

                return ScrollablePositionedList.builder(
                  itemCount: _paragraphs.length,
                  itemScrollController: _itemScrollController,
                  itemPositionsListener: _itemPositionsListener,
                  itemBuilder: (context, index) {
                    final paragraph = _paragraphs[index];
                    final String displayData = _highlightQuery(paragraph);

                    return Container(
                      padding: const EdgeInsets.symmetric(horizontal: 16.0, vertical: 8.0),
                      child: MarkdownBody(
                        data: displayData,
                        selectable: true,
                        inlineSyntaxes: [
                          SuperscriptSyntax(),
                          SubscriptSyntax(),
                        ],
                        styleSheet: MarkdownStyleSheet.fromTheme(Theme.of(context)).copyWith(
                          p: const TextStyle(fontSize: 18.0, height: 1.6),
                          h1: const TextStyle(fontSize: 26.0, fontWeight: FontWeight.bold),
                          h2: const TextStyle(fontSize: 24.0, fontWeight: FontWeight.bold),
                          h3: const TextStyle(fontSize: 22.0, fontWeight: FontWeight.bold),
                          h4: const TextStyle(fontSize: 20.0, fontWeight: FontWeight.bold),
                          horizontalRuleDecoration: BoxDecoration(
                            border: Border(top: BorderSide(width: 1.5, color: Colors.grey[300]!)),
                          ),
                          code: TextStyle(
                            backgroundColor: Colors.yellow.withAlpha(51),
                            color: Colors.black,
                            fontFamily: Theme.of(context).textTheme.bodyMedium?.fontFamily,
                            fontSize: Theme.of(context).textTheme.bodyMedium?.fontSize,
                          ),
                        ),
                      ),
                    );
                  },
                );
              },
            ),
          ),
        ],
      ),
    );
  }
}