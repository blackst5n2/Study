import 'dart:io';
import 'package:flutter/services.dart';
import 'package:path/path.dart';
import 'package:path_provider/path_provider.dart';
import 'package:sqflite/sqflite.dart';
import '../models/map_object_model.dart';
import '../models/shelf_model.dart';
import '../models/product_model.dart';

class DatabaseHelper {
  static final DatabaseHelper _instance = DatabaseHelper._internal();
  factory DatabaseHelper() => _instance;
  DatabaseHelper._internal();

  static Database? _database;

  Future<Database> get database async {
    if (_database != null) return _database!;
    _database = await _initDatabase();
    return _database!;
  }

  Future<Database> _initDatabase() async {
    Directory documentsDirectory = await getApplicationDocumentsDirectory();
    String path = join(documentsDirectory.path, 'oy_pro_link.db');
    return await openDatabase(
      path,
      version: 1,
      onCreate: _onCreate,
    );
  }

  Future<void> _onCreate(Database db, int version) async {
    await db.transaction((txn) async {
      // knowledge 테이블
      await txn.execute('''
        CREATE TABLE knowledge (
          id INTEGER PRIMARY KEY AUTOINCREMENT,
          category TEXT NOT NULL,
          key TEXT NOT NULL,
          value TEXT NOT NULL,
          UNIQUE(category, key)
        )
      ''');

      // 진열대 테이블 (신규)
      await txn.execute('''
        CREATE TABLE shelves (
          id INTEGER PRIMARY KEY AUTOINCREMENT,
          name TEXT NOT NULL UNIQUE,
          memo TEXT
        )
      ''');

      // 제품 테이블 (수정)
      await txn.execute('''
        CREATE TABLE products (
          id INTEGER PRIMARY KEY AUTOINCREMENT,
          product_name TEXT NOT NULL,
          brand TEXT,
          barcode TEXT,
          ingredients TEXT,
          memo TEXT,
          image_url TEXT,
          shelf_id INTEGER,
          shelf_level INTEGER,
          FOREIGN KEY (shelf_id) REFERENCES shelves (id) ON DELETE SET NULL
        )
      ''');

      // 스마트 맵 테이블
      await txn.execute('''
        CREATE TABLE map_objects (
          id INTEGER PRIMARY KEY AUTOINCREMENT,
          shelf_id INTEGER NOT NULL,
          pos_x REAL NOT NULL,
          pos_y REAL NOT NULL,
          width REAL NOT NULL,
          height REAL NOT NULL,
          FOREIGN KEY (shelf_id) REFERENCES shelves (id) ON DELETE CASCADE
        )
      ''');
    });
    // 데이터 사전 로드
    await _preloadKnowledgeData(db);
  }

  Future<void> _preloadKnowledgeData(Database db) async {
    Batch batch = db.batch();

    // --- 1. 성분 궁합 데이터 (interaction) ---
    _addKnowledgeToBatch(batch, 'interaction', '레티놀+벤조일퍼옥사이드', '{"type": "길항", "reason": "벤조일퍼옥사이드가 레티놀을 산화시켜 비활성화함."}');
    _addKnowledgeToBatch(batch, 'interaction', '레티놀+AHA/BHA', '{"type": "길항", "reason": "과도한 자극 및 장벽 손상 위험 증가."}');
    _addKnowledgeToBatch(batch, 'interaction', '비타민C+AHA/BHA', '{"type": "길항", "reason": "두 성분 모두 산성이 강해 과도한 자극 유발 가능."}');
    _addKnowledgeToBatch(batch, 'interaction', '비타민C+벤조일퍼옥사이드', '{"type": "길항", "reason": "벤조일퍼옥사이드가 비타민C를 산화시켜 비활성화함."}');
    _addKnowledgeToBatch(batch, 'interaction', '나이아신아마이드+AHA/BHA', '{"type": "길항", "reason": "낮은 pH의 산이 나이아신아마이드를 니코틴산으로 전환시켜 일시적 홍조 유발 가능. 30분 간격 사용 권장."}');
    _addKnowledgeToBatch(batch, 'interaction', '비타민C+비타민E+페룰릭애씨드', '{"type": "시너지", "reason": "안정성 및 항산화 보호 효과 증대."}');
    _addKnowledgeToBatch(batch, 'interaction', '레티놀+나이아신아마이드', '{"type": "시너지", "reason": "나이아신아마이드가 레티놀로 인한 자극을 완화하고 장벽을 강화함."}');
    _addKnowledgeToBatch(batch, 'interaction', '레티놀+히알루론산/세라마이드', '{"type": "시너지", "reason": "보습 성분이 레티놀 사용으로 인한 건조함과 자극을 완화함."}');
    _addKnowledgeToBatch(batch, 'interaction', '자외선차단제+항산화제', '{"type": "시너지", "reason": "자외선 차단제가 막지 못하는 활성산소를 항산화제가 중화시켜 이중 방어."}');

    // --- 2. 피부/헤어/바디 고민 데이터 (concern, hair_concern, body_concern) ---
    _addKnowledgeToBatch(batch, 'concern', '주름', '레티노이드, 바쿠치올, 펩타이드, 비타민 C, AHA');
    _addKnowledgeToBatch(batch, 'concern', '탄력', '레티노이드, 바쿠치올, 펩타이드, 비타민 C');
    _addKnowledgeToBatch(batch, 'concern', '색소침착', '비타민 C, 트라넥삼산, 나이아신아마이드, 레티노이드');
    _addKnowledgeToBatch(batch, 'concern', '여드름', 'BHA, 벤조일퍼옥사이드, 나이아신아마이드, 레티노이드');
    _addKnowledgeToBatch(batch, 'concern', '모공', 'BHA, 나이아신아마이드, 레티노이드');
    _addKnowledgeToBatch(batch, 'concern', '건조', '히알루론산, 세라마이드, 나이아신아마이드, 판테놀');
    _addKnowledgeToBatch(batch, 'concern', '피부장벽', '세라마이드, 콜레스테롤, 유리 지방산, 나이아신아마이드, 판테놀');
    _addKnowledgeToBatch(batch, 'hair_concern', '모발 가늘어짐/탈락', '카페인, 비오틴 (단, 비오틴은 실제 결핍이 있는 경우에만 효과적)');
    _addKnowledgeToBatch(batch, 'hair_concern', '모발 손상', '가수분해 케라틴');
    _addKnowledgeToBatch(batch, 'hair_concern', '비듬/두피 각질', '살리실산(BHA)');
    _addKnowledgeToBatch(batch, 'body_concern', '몸드름', '살리실산(BHA), 벤조일퍼옥사이드');
    _addKnowledgeToBatch(batch, 'body_concern', '모공각화증', 'AHA(글리콜산/락틱애씨드), Urea, 세라마이드');

    // --- 3. 성분 정보 (ingredient_info) - 수정 및 보강 ---
    _addKnowledgeToBatch(batch, 'ingredient_info', '레티노이드', '{"type": "주름 개선 기능성 원료", "function": "콜라겐 생성 촉진, 턴오버 주기 정상화", "작용 기전": "피부 세포의 레티노산 수용체(RARs)에 결합하여 세포의 턴오버를 정상화하고 새로운 콜라겐 생성을 촉진합니다.", "description": "비타민 A 유도체의 총칭으로, 주름, 탄력, 모공, 피부결 개선에 효과적인 가장 강력한 안티에이징 성분 중 하나입니다.", "caution": "초기 사용 시 자극(레티놀 번)이 발생할 수 있으므로 저녁에 소량으로 시작해 적응 기간이 필요합니다. 낮에는 반드시 자외선 차단제를 사용해야 하며, 임산부는 사용을 피해야 합니다."}');
    _addKnowledgeToBatch(batch, 'ingredient_info', '바쿠치올', '{"type": "항산화/항염", "function": "레티놀 대체 가능 (주름, 색소침착 개선)", "임상 비교": "한 연구에서 0.5% 레티놀과 동등한 주름 및 색소침착 개선 효과를 보였으나, 따가움이나 각질 탈락 같은 자극은 더 적게 보고되었습니다."}');
    _addKnowledgeToBatch(batch, 'ingredient_info', '나이아신아마이드', '{"type": "미백/주름 개선 기능성 원료", "function": "미백, 트러블 완화, 피지 조절, 피부 장벽 강화", "작용 기전": "멜라닌 이동 억제(미백), 항염 작용(트러블), 피지 생성 조절, 세라마이드 합성 촉진(장벽 강화) 등 다재다능한 효과를 가집니다."}');
    _addKnowledgeToBatch(batch, 'ingredient_info', '비타민 C', '{"type": "항산화/미백 기능성 원료", "function": "항산화, 색소침착 개선, 콜라겐 생성 촉진", "description": "강력한 항산화제로, 자외선과 외부 유해 환경으로부터 피부를 보호하고 멜라닌 생성을 억제하여 피부톤을 밝게 합니다.", "제형 정보": "순수 비타민C(L-아스코빅애씨드)는 효과가 뛰어나지만, 안정성과 피부 침투를 위해 낮은 pH(3.5 미만)가 필요하여 자극을 유발할 수 있습니다."}');
    _addKnowledgeToBatch(batch, 'ingredient_info', '히알루론산', '{"type": "보습 성분", "function": "수분 공급 및 유지", "작용 원리": "자기 무게의 몇 배에 달하는 수분을 끌어당기는 보습 성분입니다. 고분자는 피부 표면에 보습막을 형성하고, 저분자는 피부에 더 깊이 침투하여 수분을 공급합니다."}');
    _addKnowledgeToBatch(batch, 'ingredient_info', '세라마이드', '{"type": "피부 구성 성분", "function": "피부 장벽 강화, 보습", "description": "피부 각질층의 세포간지질 약 50%를 차지하는 핵심 성분입니다.", "tip": "피부 장벽이 손상된 경우, 콜레스테롤, 유리 지방산과 생리학적 비율(약 3:1:1)로 배합될 때 가장 효과적으로 장벽을 복구합니다."}');
    _addKnowledgeToBatch(batch, 'ingredient_info', '펩타이드', '{"type": "탄력/재생", "function": "세포 신호 전달", "작용 기전": "피부 세포에 특정 기능을 수행하라는 메시지(예: 콜라겐 생성)를 전달하는 짧은 아미노산 사슬입니다."}');

    // --- 4. 신규 및 기존 성분 정보 (A-Z 정렬) ---
    _addKnowledgeToBatch(batch, 'ingredient_info', 'AHA', '{"type": "수용성 각질 제거 성분", "function": "표피 각질 제거, 피부결 개선", "description": "수용성으로 피부 표면에 작용하여 죽은 각질 세포의 연결을 끊어 탈락을 유도합니다. 광손상이나 피부결 개선에 이상적입니다.", "sub_types": "글리콜산, 락틱애씨드, 만델산 등이 있습니다.", "caution": "자외선에 대한 피부의 민감도를 높이므로 낮에는 반드시 자외선 차단제를 사용해야 합니다."}');
    _addKnowledgeToBatch(batch, 'ingredient_info', 'BHA', '{"type": "지용성 각질 제거 성분", "function": "모공 속 각질/피지 제거", "description": "지용성으로 피지가 많은 모공 깊숙이 침투하여 피지와 각질을 녹여내므로 여드름과 블랙헤드 관리에 탁월합니다.", "sub_types": "화장품에는 주로 \'살리실산\'이 사용됩니다.", "caution": "과도하게 사용하면 피부가 건조해지거나 자극될 수 있습니다. 아스피린 알러지가 있는 경우 사용에 주의해야 합니다."}');
    _addKnowledgeToBatch(batch, 'ingredient_info', 'Urea', '{"type": "보습/각질 연화 성분", "function": "강력 보습, 각질 제거", "description": "피부의 천연 보습 인자(NMF) 중 하나로, 강력한 보습 효과와 함께 각질을 부드럽게 만드는 효과가 있어 모공각화증(닭살 피부) 관리에 사용됩니다."}');
    _addKnowledgeToBatch(batch, 'ingredient_info', '가수분해 케라틴', '{"type": "모발 강화 성분", "function": "손상 모발 복구", "description": "케라틴 단백질을 모발에 침투 가능하도록 작게 분해한 성분입니다. 모발의 피질(cortex)에 침투하여 빈 공간을 채우고, 내부 구조를 강화합니다."}');
    _addKnowledgeToBatch(batch, 'ingredient_info', '글리콜산', '{"type": "AHA의 한 종류", "function": "각질 제거", "description": "AHA 중에서 분자량이 가장 작아 피부 침투력이 뛰어나고 각질 제거 효과가 강력합니다. 하지만 그만큼 자극 가능성도 있어 민감성 피부는 주의가 필요합니다."}');
    _addKnowledgeToBatch(batch, 'ingredient_info', '디메치콘', '{"type": "실리콘(제형)", "function": "발림성 개선, 피부결 보정", "description": "피부에 부드럽고 매끄러운 질감을 부여하고 잔주름이나 모공을 시각적으로 메워줌. 분자량이 커 피부에 흡수되지 않아 안전성이 높음."}');
    _addKnowledgeToBatch(batch, 'ingredient_info', '레티놀', '{"type": "레티노이드의 한 종류", "function": "주름 개선", "description": "화장품에 가장 널리 사용되는 비타민 A 유도체입니다. 자세한 내용은 \'레티노이드\' 항목을 참고하세요."}');
    _addKnowledgeToBatch(batch, 'ingredient_info', '벤조일퍼옥사이드', '{"type": "여드름 치료 성분", "function": "항균, 각질 제거", "description": "강력한 항균 작용으로 여드름균(P. acnes)을 사멸시키고, 각질을 제거하여 모공 막힘을 개선합니다."}');
    _addKnowledgeToBatch(batch, 'ingredient_info', '비오틴', '{"type": "비타민 B7", "function": "모발/손톱 건강 (결핍 시)", "description": "실제 비오틴 결핍 상태에서만 모발과 손톱 건강 개선에 효과가 있으며, 건강한 사람의 탈모에는 효과가 입증되지 않았습니다."}');
    _addKnowledgeToBatch(batch, 'ingredient_info', '비타민 E', '{"type": "항산화 성분", "function": "항산화, 보습, 피부 보호", "description": "대표적인 지용성 항산화제로, 세포막을 손상시키는 활성산소를 중화시켜 피부 노화를 방지하고 피부 장벽을 강화합니다.", "synergy": "비타민 C와 함께 사용 시 서로의 항산화 효과를 증폭시켜 시너지를 냅니다."}');
    _addKnowledgeToBatch(batch, 'ingredient_info', '살리실산', '{"type": "BHA의 한 종류", "function": "각질/피지 제거", "description": "화장품에 사용되는 대표적인 BHA 성분입니다. 지용성으로 모공 속 피지와 각질을 효과적으로 제거하여 여드름성 피부에 적합합니다."}');
    _addKnowledgeToBatch(batch, 'ingredient_info', '시카', '{"type": "피부 진정/재생 성분", "function": "진정, 손상 회복, 항염", "description": "\'센텔라 아시아티카(병풀)\'에서 추출한 성분을 통칭합니다. 핵심 활성 성분인 마데카소사이드 등은 손상된 피부 및 두피를 진정시키고 재생을 돕는 효과가 있습니다."}');
    _addKnowledgeToBatch(batch, 'ingredient_info', '아보벤존', '{"type": "화학적 자외선 차단 성분 (유기자차)", "function": "UVA 차단", "description": "광안정성이 매우 낮아 단독으로 사용되지 않고 옥토크릴렌 같은 광안정화제와 반드시 함께 사용해야 합니다."}');
    _addKnowledgeToBatch(batch, 'ingredient_info', '아이언옥사이드', '{"type": "색소/광차단", "function": "색상 구현, 블루라이트 차단", "description": "파운데이션의 피부톤을 구현하는 핵심 색소이며, 가시광선, 특히 블루라이트를 효과적으로 차단하는 부가 기능이 있습니다."}');
    _addKnowledgeToBatch(batch, 'ingredient_info', '에칠헥실메톡시신나메이트', '{"type": "화학적 자외선 차단 성분 (유기자차)", "function": "UVB 차단", "description": "전 세계적으로 가장 널리 사용되는 UVB 차단 필터 중 하나입니다."}');
    _addKnowledgeToBatch(batch, 'ingredient_info', '유리 지방산', '{"type": "피부 구성 성분", "function": "피부 장벽 강화", "description": "세라마이드, 콜레스테롤과 함께 피부 장벽을 구성하는 주요 지질 성분입니다."}');
    _addKnowledgeToBatch(batch, 'ingredient_info', '징크옥사이드', '{"type": "물리적 자외선 차단", "function": "광범위 UVA/UVB 차단", "description": "자외선을 반사/산란시키는 무기 필터로, 광안정성이 뛰어나고 피부 자극이 적습니다."}');
    _addKnowledgeToBatch(batch, 'ingredient_info', '카페인', '{"type": "두피 활성화 성분", "function": "모낭 신진대사 촉진, 탈모 완화", "description": "모낭의 신진대사를 활성화하고, 탈모 원인 중 하나인 5-알파 환원효소를 억제할 가능성이 있어 탈모 완화 기능성 샴푸/토닉에 사용됩니다."}');
    _addKnowledgeToBatch(batch, 'ingredient_info', '콜레스테롤', '{"type": "피부 구성 성분", "function": "피부 장벽 강화", "description": "세라마이드, 유리 지방산과 함께 피부 각질층의 세포간지질을 구성하는 3대 요소 중 하나입니다."}');
    _addKnowledgeToBatch(batch, 'ingredient_info', '티타늄디옥사이드', '{"type": "물리적 자외선 차단/색소", "function": "주로 UVB, 일부 UVA 차단, 백색 안료", "description": "자외선 차단과 함께 파운데이션의 커버력을 제공하는 백색 안료로도 사용됩니다."}');
    _addKnowledgeToBatch(batch, 'ingredient_info', '트라넥삼산', '{"type": "미백 기능성", "function": "기미, 색소침착 억제", "description": "플라스민 활성을 억제하여 멜라닌 생성을 막는 기전으로, 특히 기미 개선에 효과적입니다."}');
    _addKnowledgeToBatch(batch, 'ingredient_info', '트레티노인', '{"type": "레티노이드의 한 종류 (전문의약품)", "function": "주름, 여드름 치료", "description": "효과가 매우 강력하지만 자극도 강해 의사의 처방이 필요한 전문의약품 성분입니다."}');
    _addKnowledgeToBatch(batch, 'ingredient_info', '판테놀', '{"type": "보습/진정 성분", "function": "보습, 피부 장벽 강화, 항염", "description": "피부에 흡수되면 비타민 B5로 전환되는 성분입니다. 강력한 보습제로 수분을 끌어당기고 유지하며, 피부 장벽을 강화하고 가려움증이나 염증을 완화하는 진정 효과도 있습니다."}');
    _addKnowledgeToBatch(batch, 'ingredient_info', '페룰릭애씨드', '{"type": "항산화 성분", "function": "강력한 항산화, 자외선 손상 방어", "description": "식물에서 발견되는 항산화 성분으로, 자외선으로 인한 피부 손상을 예방하고 피부를 보호하는 능력이 뛰어납니다.", "synergy": "비타민 C와 비타민 E의 안정성을 높이고 항산화 능력을 극대화합니다."}');

    // --- 5. 과학/뷰티 개념 정보 (concept_info) ---
    _addKnowledgeToBatch(batch, 'concept_info', '피부 장벽 모델', '{"title": "벽돌과 시멘트 모델", "description": "피부 각질층을 설명하는 모델입니다. 각질세포가 \'벽돌\', 세라마이드(약 50%)/콜레스테롤(약 25%)/유리 지방산(약 15%)으로 구성된 세포간지질이 \'시멘트\' 역할을 합니다."}');
    _addKnowledgeToBatch(batch, 'concept_info', '피부 턴오버 주기', '{"alias": "각질 탈락 주기", "description": "기저층에서 생성된 세포가 각질층까지 이동해 탈락하기까지 약 40~56일이 소요됩니다. 이것이 화장품 효과에 시간이 걸리는 이유입니다."}');
    _addKnowledgeToBatch(batch, 'concept_info', '경피수분손실 (TEWL)', '{"definition": "Transepidermal Water Loss의 약자로, 피부를 통해 수분이 증발하는 현상을 의미합니다. 건강한 피부 장벽은 TEWL을 효과적으로 방지합니다."}');
    _addKnowledgeToBatch(batch, 'concept_info', '광노화 메커니즘', '{"title": "자외선으로 인한 노화 원리", "description": "자외선이 피부에 활성산소(ROS)를 만들고, 이것이 콜라겐 분해 효소(MMP)를 활성화시켜 진피의 콜라겐을 파괴하는 연쇄 반응을 일으킵니다."}');
    _addKnowledgeToBatch(batch, 'concept_info', '미세먼지와 피부', '{"effect": "피부에 산화 스트레스를 유발하고, 피부 장벽의 지질 구성을 변화시켜 장벽 기능을 손상시킬 수 있습니다."}');
    _addKnowledgeToBatch(batch, 'concept_info', '블루라이트와 피부', '{"alias": "HEV Light", "effect": "활성산소를 생성하여 과색소침착과 피부 노화를 가속화할 수 있다는 연구가 있습니다."}');
    _addKnowledgeToBatch(batch, 'concept_info', '환경 방어막', '{"strategy": "자외선, 미세먼지 등 복합적인 외부 유해 요인에 대응하기 위한 다각적인 피부 보호 전략입니다.", "components": "항산화제(비타민C 등) + 자외선차단제 + 장벽강화 크림(세라마이드 등)"}');
    _addKnowledgeToBatch(batch, 'concept_info', '리포좀 기술', '{"title": "유효 성분 전달 기술", "description": "불안정한 유효 성분을 인지질 미세 캡슐에 담아, 피부 장벽을 통과하여 더 깊은 곳까지 안정적으로 전달하는 기술입니다."}');
    _addKnowledgeToBatch(batch, 'concept_info', '클린 뷰티 인증 비교', '{"The Vegan Society": "동물 유래 원료 배제에 초점", "Leaping Bunny": "신규 동물 실험 금지에 초점", "EWG VERIFIED®": "인간 건강 및 환경 안전에 초점"}');
    _addKnowledgeToBatch(batch, 'concept_info', '논코메도제닉', '{"definition": "모공을 막지 않는다는 의미의 마케팅 용어", "fact": "미국 FDA에서 규제하지 않으며, 표준화된 테스트 방법이 없어 맹신하기보다 성분표를 직접 확인하는 것이 중요합니다."}');
    _addKnowledgeToBatch(batch, 'concept_info', '가성모낭염', '{"alias": "면도 트러블", "mechanism": "감염이 아닌, 날카롭게 잘린 수염이 다시 피부를 파고들면서 발생하는 염증성 이물 반응입니다."}');
    _addKnowledgeToBatch(batch, 'concept_info', '남성 피부 특징', '{"characteristics": "여성보다 약 20% 더 두껍고, 테스토스테론의 영향으로 피지 분비량이 많아 모공이 더 큽니다.", "pros_cons": "피지 덕분에 건조함이나 잔주름은 늦게 나타날 수 있지만, 지성 및 여드름성 경향이 높습니다."}');
    _addKnowledgeToBatch(batch, 'concept_info', '올인원 제품의 한계', '{"principle": "샴푸(세정)와 컨디셔너(보습)처럼 상충하는 기능을 한 제품에 담는 것은 편리하지만, 각 기능의 효과는 떨어지는 타협의 산물입니다."}');
    _addKnowledgeToBatch(batch, 'concept_info', '모발 구조', '{"layers": "모발은 큐티클(보호층), 피질(강도/색상), 수질(중심) 3개 층으로 구성됩니다."}');
    _addKnowledgeToBatch(batch, 'concept_info', '모발 성장 주기', '{"cycles": "모발은 성장기(Anagen), 퇴행기(Catagen), 휴지기(Telogen) 주기를 반복하며 성장하고 탈락합니다."}');
    _addKnowledgeToBatch(batch, 'concept_info', '두피의 특징', '{"characteristics": "얼굴 피부보다 모낭과 피지선의 밀도가 훨씬 높아, 비듬이나 유분 같은 특정 문제에 더 취약합니다."}');
    _addKnowledgeToBatch(batch, 'concept_info', '컨디셔너 vs 트리트먼트', '{"conditioner": "모발 표면의 큐티클을 코팅해 즉각적인 부드러움을 줍니다.", "treatment": "모발 내부의 피질까지 침투해 구조를 강화하는 근본적인 케어에 가깝습니다."}');
    _addKnowledgeToBatch(batch, 'concept_info', '장-피부 축', '{"definition": "장내 미생물 환경이 염증 조절 등을 통해 피부 건강에 영향을 미친다는 개념입니다."}');
    _addKnowledgeToBatch(batch, 'concept_info', '후성유전학', '{"definition": "DNA 자체는 바꾸지 않지만, 운동 등 생활 습관이 유전자 발현 방식에 영향을 미쳐 피부 건강에 기여할 수 있다는 개념입니다."}');

    // --- 6. 이너 뷰티 정보 (inner_beauty_info) ---
    _addKnowledgeToBatch(batch, 'inner_beauty_info', '경구용 콜라겐', '{"summary": "피부 수분, 탄력 개선에 도움이 된다는 일부 연구가 있으나, 산업계 지원 연구에서 긍정적 결과가 많아 비판적 접근이 필요합니다."}');
    _addKnowledgeToBatch(batch, 'inner_beauty_info', '경구용 히알루론산', '{"summary": "다수의 연구에서 피부 수분 증가 및 탄력 개선 효과가 일관되게 나타나, 콜라겐보다 근거가 명확한 편입니다."}');
    _addKnowledgeToBatch(batch, 'inner_beauty_info', '경구용 비오틴', '{"summary": "진단된 결핍증이 있는 경우에만 효과가 있으며, 고용량 섭취는 특정 혈액 검사 결과를 방해할 수 있어 주의가 필요합니다."}');


    // =======================================================================
    // ### 신규 학습 데이터 (2025-06-18) ###
    // =======================================================================

    // --- 피부 타입 정보 ---
    _addKnowledgeToBatch(batch, 'skin_type', '지성', '{"description": "오후만 되면 유분 폭발, 모공이 넓고 트러블이 잦음", "tip_question": "세안 후 몇 시간이 지나면 얼굴 전체가 번들거리나요?", "recommendation": "피지 조절, 모공 케어, 가벼운 제형"}');
    _addKnowledgeToBatch(batch, 'skin_type', '건성', '{"description": "세안 후 심하게 땅기고, 각질이 잘 일어나며 잔주름이 보임", "tip_question": "세안 후 바로 스킨케어를 안 하면 피부가 많이 땅기나요?", "recommendation": "강력 보습, 유수분 밸런스, 장벽 강화"}');
    _addKnowledgeToBatch(batch, 'skin_type', '복합성', '{"description": "T존(이마, 코)은 번들, U존(볼, 턱)은 건조함", "tip_question": "코는 번들거리는데 볼은 건조하거나 땅기지 않으세요?", "recommendation": "부위별 케어 (T존-가볍게, U존-촉촉하게)"}');
    _addKnowledgeToBatch(batch, 'skin_type', '수부지', '{"description": "속은 건조한데 겉은 유분이 도는 상태. 번들거리지만 각질이 있음", "tip_question": "피부가 번들거리는데, 각질이 일어나거나 속이 땅기는 느낌도 있나요?", "recommendation": "속보습 충전 (히알루론산), 유분 조절"}');
    _addKnowledgeToBatch(batch, 'skin_type', '민감성', '{"description": "작은 자극에도 쉽게 붉어지고, 가렵고, 따가움", "tip_question": "새로운 화장품을 사용했을 때 피부가 쉽게 뒤집어지나요?", "recommendation": "저자극, 진정 성분(시카, 판테놀), 성분 최소화"}');

    // --- 스킨케어 원칙 ---
    _addKnowledgeToBatch(batch, 'concept_info', '클렌징 원칙', '{"type": "스킨케어 3대 원칙", "description": "메이크업은 하는 것보다 지우는 것이 중요합니다. 고객의 피부 타입과 메이크업 정도에 맞는 클렌저(오일, 워터, 폼, 젤)를 추천하는 것이 기본입니다."}');
    _addKnowledgeToBatch(batch, 'concept_info', '보습 원칙', '{"type": "스킨케어 3대 원칙", "description": "모든 피부 문제의 근원은 건조함일 수 있습니다. 피부 장벽을 튼튼하게 유지하는 보습의 중요성을 항상 강조해야 합니다."}');
    _addKnowledgeToBatch(batch, 'concept_info', '자외선 차단 원칙', '{"type": "스킨케어 3대 원칙", "description": "최고의 안티에이징은 선크림입니다. 자외선은 노화, 색소침착의 주범이므로 1년 365일 사용을 습관화하도록 안내합니다."}');

    // --- 자외선 차단제 상세 정보 (기존 정보 업데이트 및 추가) ---
    _addKnowledgeToBatch(batch, 'concept_info', 'SPF', '{"type": "자외선 차단 지수", "function": "UVB 차단", "description": "Sun Protection Factor의 약자로, 피부를 태우고 화상을 입히는 UVB를 차단하는 지수입니다. 숫자가 높을수록 차단 시간이 길어집니다."}');
    _addKnowledgeToBatch(batch, 'concept_info', 'PA', '{"type": "자외선 차단 지수", "function": "UVA 차단", "description": "Protection Grade of UVA의 약자로, 피부 깊숙이 침투해 노화와 기미를 유발하는 UVA를 차단하는 등급입니다. + 개수가 많을수록 차단력이 강합니다."}');
    _addKnowledgeToBatch(batch, 'concept_info', '무기자차', '{"type": "자외선 차단제 타입", "alias": "물리적 차단제", "principle": "피부에 물리적인 막을 씌워 자외선을 튕겨냄", "ingredients": "징크옥사이드, 티타늄디옥사이드", "pros": "순해서 민감성 피부에 적합, 바른 직후 효과", "cons": "백탁 현상, 약간의 뻑뻑함"}');
    _addKnowledgeToBatch(batch, 'concept_info', '유기자차', '{"type": "자외선 차단제 타입", "alias": "화학적 차단제", "principle": "자외선을 흡수해 열에너지로 변환시켜 소멸시킴", "ingredients": "에칠헥실메톡시신나메이트, 아보벤존 등", "pros": "발림성이 좋고 백탁이 거의 없음", "cons": "눈 시림이나 트러블 유발 가능성(민감성 주의), 외출 20분 전 사용 권장"}');
    _addKnowledgeToBatch(batch, 'concept_info', '혼합자차', '{"type": "자외선 차단제 타입", "description": "무기자차와 유기자차의 장점을 결합한 제품으로, 발림성과 순한 사용감을 모두 개선한 형태입니다."}');

    // --- TPO 기반 추천 정보 ---
    _addKnowledgeToBatch(batch, 'recommendation_tpo', '일상용 선크림', '{"situation": "매일 출퇴근하는 직장인", "recommend_type": "유기자차/혼합자차, SPF30-50, PA+++ 이상", "selling_point": "백탁 없이 로션처럼 발리는 제품이라 메이크업 전에 사용하기 좋아요."}');
    _addKnowledgeToBatch(batch, 'recommendation_tpo', '야외활동용 선크림', '{"situation": "야외 활동, 레포츠", "recommend_type": "무기자차/혼합자차, SPF50+, PA++++, 워터프루프 기능", "selling_point": "땀과 물에 강하고, 피부에 순한 무기자차라 장시간 야외활동에도 안심이에요."}');
    _addKnowledgeToBatch(batch, 'recommendation_tpo', '민감성피부용 선크림', '{"situation": "민감성 피부, 어린이", "recommend_type": "무기자차 100%, SPF30-50, PA+++ 이상", "selling_point": "피부에 흡수되지 않고 자외선을 반사시키는 방식이라 민감한 피부에 가장 안전해요."}');

    // --- 색조 메이크업 정보 ---
    _addKnowledgeToBatch(batch, 'concept_info', '웜톤', '{"type": "퍼스널 컬러", "skin_tone": "노란빛, 아이보리빛이 감돎", "accessory": "골드", "color_palette": "오렌지, 코랄, 브라운, 베이지, 피치"}');
    _addKnowledgeToBatch(batch, 'concept_info', '쿨톤', '{"type": "퍼스널 컬러", "skin_tone": "핑크빛, 푸른빛이 감돎", "accessory": "실버", "color_palette": "핑크, 버건디, 라벤더, 플럼, 모브"}');
    _addKnowledgeToBatch(batch, 'selling_point', '파운데이션/쿠션', '{"checkpoints": ["커버력: 잡티/홍조 커버 vs 자연스러운 표현", "피니쉬: 보송한 마무리(매트) vs 은은한 광채(글로우)"]}');
    _addKnowledgeToBatch(batch, 'selling_point', '립 제품', '{"checkpoints": ["제형: 벨벳, 매트, 글로시, 틴트 등 발색과 지속력 차이 설명", "팁: 착색 정도, 건조함 여부에 따라 촉촉한 타입 추천"]}');
    _addKnowledgeToBatch(batch, 'selling_point', '아이섀도', '{"checkpoints": ["조합: 고객 톤에 맞는 팔레트 추천", "활용법: 베이스-메인-포인트-음영 컬러 활용법 설명"]}');

    // --- 뷰티 도구 정보 ---
    _addKnowledgeToBatch(batch, 'tool_info', '파운데이션 브러쉬', '{"pair_product": "리퀴드/크림 파운데이션", "selling_point": "얇고 균일하게 발려 피부 표현이 정교해져요. 모공 커버에 탁월해요."}');
    _addKnowledgeToBatch(batch, 'tool_info', '메이크업 스펀지', '{"pair_product": "파운데이션, 컨실러, 크림 블러셔", "selling_point": "물에 적셔 사용하면 촉촉한 물광 피부를, 마른 상태로 쓰면 커버력을 높일 수 있어요."}');
    _addKnowledgeToBatch(batch, 'tool_info', '파우더 퍼프', '{"pair_product": "루스/프레스드 파우더", "selling_point": "보송보송하고 얇게 유분을 잡아주어 메이크업 지속력을 높여줍니다."}');
    _addKnowledgeToBatch(batch, 'tool_info', '아이섀도 브러쉬', '{"pair_product": "파우더 타입 섀도", "selling_point": "총알 브러쉬는 포인트 컬러에, 넙적한 브러쉬는 베이스 컬러에 사용하면 좋아요."}');
    _addKnowledgeToBatch(batch, 'tool_info', '위생 관리', '{"selling_point": "브러쉬나 스펀지는 주기적으로 세척해주셔야 피부 트러블 없이 깨끗하게 메이크업하실 수 있어요. (클렌저 제품 연계 판매 가능)"}');

    // --- 제품 제형 정보 ---
    _addKnowledgeToBatch(batch, 'texture_info', '워터/토너', '{"characteristics": "물처럼 가볍고 흡수가 빠름", "recommendation": "모든 피부 타입, 특히 지성/수부지의 첫 단계"}');
    _addKnowledgeToBatch(batch, 'texture_info', '에센스/세럼', '{"characteristics": "유효 성분이 고농축된 묽은 제형", "recommendation": "모든 피부 타입, 특정 고민(미백, 주름) 집중 케어"}');
    _addKnowledgeToBatch(batch, 'texture_info', '젤', '{"characteristics": "유분기 없이 수분감만 전달, 시원한 쿨링감", "recommendation": "지성, 수부지, 여름철 스킨케어"}');
    _addKnowledgeToBatch(batch, 'texture_info', '로션/에멀젼', '{"characteristics": "수분과 유분이 적절히 배합된 부드러운 제형", "recommendation": "복합성, 수부지, 건성의 중간 단계"}');
    _addKnowledgeToBatch(batch, 'texture_info', '크림', '{"characteristics": "유분감이 풍부하고 보습막 형성에 탁월", "recommendation": "건성, 복합성의 U존, 나이트 케어"}');
    _addKnowledgeToBatch(batch, 'texture_info', '밤', '{"characteristics": "고체에 가까운 꾸덕한 제형, 강력한 보습막", "recommendation": "극건성, 심한 악건성 부위의 국소 사용"}');
    _addKnowledgeToBatch(batch, 'texture_info', '오일', '{"characteristics": "피부에 윤기와 영양 공급, 수분 증발 차단", "recommendation": "극건성, 크림에 섞어 보습력 강화"}');

    // --- 전체 학습 자료 텍스트 로드 ---
    try {
      String fullText = await rootBundle.loadString('assets/knowledge_base.txt');
      _addKnowledgeToBatch(batch, 'full_text', 'main', fullText);
    } catch (e) {
      // print("Error loading knowledge base text: $e");
    }

    await batch.commit(noResult: true);
  }

  void _addKnowledgeToBatch(Batch batch, String category, String key, String value) {
    batch.insert('knowledge', {
      'category': category,
      'key': key,
      'value': value,
    }, conflictAlgorithm: ConflictAlgorithm.replace);
  }

  Future<int> insertShelf(Shelf shelf) async {
    final db = await database;
    return await db.insert('shelves', shelf.toMap(), conflictAlgorithm: ConflictAlgorithm.replace);
  }

  Future<List<Shelf>> getAllShelves() async {
    final db = await database;
    final List<Map<String, dynamic>> maps = await db.query('shelves', orderBy: 'name ASC');
    return List.generate(maps.length, (i) => Shelf.fromMap(maps[i]));
  }

  Future<int> updateShelf(Shelf shelf) async {
    final db = await database;
    return await db.update('shelves', shelf.toMap(), where: 'id = ?', whereArgs: [shelf.id]);
  }

  Future<int> deleteShelf(int id) async {
    final db = await database;
    return await db.delete('shelves', where: 'id = ?', whereArgs: [id]);
  }

  Future<int> insertProduct(Product product) async {
    final db = await database;
    return await db.insert('products', product.toMap(), conflictAlgorithm: ConflictAlgorithm.replace);
  }

  Future<List<Product>> getAllProducts() async {
    final db = await database;
    final List<Map<String, dynamic>> maps = await db.query('products', orderBy: 'id DESC');
    if (maps.isEmpty) {
      return [];
    }
    return List.generate(maps.length, (i) => Product.fromMap(maps[i]));
  }

  Future<int> updateProduct(Product product) async {
    final db = await database;
    return await db.update('products', product.toMap(), where: 'id = ?', whereArgs: [product.id]);
  }

  Future<int> deleteProduct(int id) async {
    final db = await database;
    return await db.delete('products', where: 'id = ?', whereArgs: [id]);
  }

  Future<Map<String, dynamic>?> getInteraction(String key) async {
    final db = await database;
    final List<Map<String, dynamic>> maps = await db.query(
      'knowledge',
      where: 'category = ? AND key = ?',
      whereArgs: ['interaction', key],
    );
    if (maps.isNotEmpty) {
      return maps.first;
    }
    return null;
  }

  /// ### [수정] AI 어시스턴트의 추천 검색어(피부고민) 목록을 가져오는 함수 ###
  Future<List<String>> getAllConcernKeys() async {
    final db = await database;
    final List<Map<String, dynamic>> maps = await db.query(
      'knowledge',
      where: 'category = ?',
      whereArgs: ['concern'],
      columns: ['key'],
    );
    if (maps.isNotEmpty) {
      return maps.map((map) => map['key'] as String).toList();
    }
    return [];
  }

  Future<List<String>> getHairConcernKeys() async {
    final db = await database;
    final List<Map<String, dynamic>> maps = await db.query(
      'knowledge',
      where: 'category = ?',
      whereArgs: ['hair_concern'],
      columns: ['key'],
    );
    if (maps.isNotEmpty) {
      return maps.map((map) => map['key'] as String).toList();
    }
    return [];
  }

  Future<List<String>> getBodyConcernKeys() async {
    final db = await database;
    final List<Map<String, dynamic>> maps = await db.query(
      'knowledge',
      where: 'category = ?',
      whereArgs: ['body_concern'],
      columns: ['key'],
    );
    if (maps.isNotEmpty) {
      return maps.map((map) => map['key'] as String).toList();
    }
    return [];
  }

  Future<String> getFullKnowledgeText() async {
    final db = await database;
    final List<Map<String, dynamic>> maps = await db.query(
      'knowledge',
      where: 'category = ? AND key = ?',
      whereArgs: ['full_text', 'main'],
      limit: 1,
    );
    if (maps.isNotEmpty) {
      return maps.first['value'] as String;
    }
    return '학습 자료를 불러올 수 없습니다.';
  }

  Future<MapObject> addObject(MapObject object) async {
    final db = await database;
    final id = await db.insert('map_objects', object.toMap());
    return object.copyWith(id: id);
  }

  Future<List<MapObject>> getAllMapObjects() async {
    final db = await database;
    final List<Map<String, dynamic>> maps = await db.rawQuery('''
    SELECT 
      mo.*,
      s.name, s.memo
    FROM map_objects mo
    LEFT JOIN shelves s ON mo.shelf_id = s.id
  ''');
    return List.generate(maps.length, (i) => MapObject.fromMap(maps[i]));
  }

  Future<int> updateObject(MapObject object) async {
    final db = await database;
    return await db.update(
      'map_objects',
      object.toMap(),
      where: 'id = ?', whereArgs: [object.id],
    );
  }

  Future<int> deleteObject(int id) async {
    final db = await database;
    return await db.delete('map_objects', where: 'id = ?', whereArgs: [id]);
  }
}