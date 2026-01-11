// /lib/data/data_sources/sqlite_helper.dart
import 'package:sqflite/sqflite.dart';
import 'package:path/path.dart';

class SQLiteHelper {
  SQLiteHelper._privateConstructor();
  static final SQLiteHelper instance = SQLiteHelper._privateConstructor();
  static Database? _database;

  Future<Database> get database async {
    if (_database != null && _database!.isOpen) return _database!;
    _database = await _initDB();
    return _database!;
  }

  Future<void> close() async {
    final db = await instance.database;
    await db.close();
    _database = null;
  }

  Future<Database> _initDB() async {
    String path = join(await getDatabasesPath(), 'daily_palette.db');
    return await openDatabase(
      path,
      // [수정] 데이터베이스 버전 4으로 변경
      version: 4,
      onConfigure: (db) async {
        await db.execute('PRAGMA foreign_keys = ON');
      },
      onCreate: _onCreate,
      onUpgrade: _onUpgrade,
    );
  }

  Future<void> _onUpgrade(Database db, int oldVersion, int newVersion) async {
    // 트랜잭션으로 스키마 변경을 안전하게 묶음
    await db.transaction((txn) async {
      if (oldVersion < 2) {
        // 버전 1 -> 2: Recurring_Tasks 테이블의 category_id의 NOT NULL 제약조건 제거
        await txn.execute('ALTER TABLE Recurring_Tasks RENAME TO _Recurring_Tasks_old_v1');
        await txn.execute('''
          CREATE TABLE Recurring_Tasks (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            category_id INTEGER,
            title TEXT NOT NULL,
            target_time TEXT,
            recurrence_type TEXT NOT NULL,
            recurrence_detail TEXT,
            start_date TEXT NOT NULL,
            end_date TEXT,
            FOREIGN KEY (category_id) REFERENCES Categories(id) ON DELETE SET NULL
          )
        ''');
        await txn.execute('''
          INSERT INTO Recurring_Tasks (id, category_id, title, target_time, recurrence_type, recurrence_detail, start_date, end_date)
          SELECT id, category_id, title, target_time, recurrence_type, recurrence_detail, start_date, end_date
          FROM _Recurring_Tasks_old_v1
        ''');
        await txn.execute('DROP TABLE _Recurring_Tasks_old_v1');
      }
      if (oldVersion < 3) {
        // 버전 2 -> 3: Todos, Recurring_Tasks 테이블의 FK를 ON DELETE CASCADE로 변경
        // Todos 테이블 변경
        await txn.execute('ALTER TABLE Todos RENAME TO _Todos_old_v2');
        await txn.execute('''
          CREATE TABLE Todos (
            id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
            category_id INTEGER,
            title TEXT NOT NULL,
            target_date TEXT NOT NULL,
            target_time TEXT,
            status TEXT NOT NULL CHECK(status IN ('pending', 'completed', 'failed')),
            created_at TEXT NOT NULL,
            completed_at TEXT,
            priority INTEGER,
            parent_id INTEGER,
            recurring_task_id INTEGER,
            display_order INTEGER,
            time_spent_seconds INTEGER NOT NULL DEFAULT 0,
            timer_duration_minutes INTEGER,
            timer_end_time TEXT,
            FOREIGN KEY (category_id) REFERENCES Categories(id) ON DELETE CASCADE,
            FOREIGN KEY (parent_id) REFERENCES Todos(id) ON DELETE CASCADE,
            FOREIGN KEY (recurring_task_id) REFERENCES Recurring_Tasks(id) ON DELETE SET NULL
          )
        ''');
        await txn.execute('''
          INSERT INTO Todos SELECT * FROM _Todos_old_v2
        ''');
        await txn.execute('DROP TABLE _Todos_old_v2');

        // Recurring_Tasks 테이블 변경
        await txn.execute('ALTER TABLE Recurring_Tasks RENAME TO _Recurring_Tasks_old_v2');
        await txn.execute('''
          CREATE TABLE Recurring_Tasks (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            category_id INTEGER,
            title TEXT NOT NULL,
            target_time TEXT,
            recurrence_type TEXT NOT NULL,
            recurrence_detail TEXT,
            start_date TEXT NOT NULL,
            end_date TEXT,
            FOREIGN KEY (category_id) REFERENCES Categories(id) ON DELETE CASCADE
          )
        ''');
        await txn.execute('''
          INSERT INTO Recurring_Tasks SELECT * FROM _Recurring_Tasks_old_v2
        ''');
        await txn.execute('DROP TABLE _Recurring_Tasks_old_v2');
      }
      if (oldVersion < 4) {
        // 버전 3 -> 4: Recurring_Tasks 테이블에 subtask_templates 컬럼 추가
        await txn.execute('ALTER TABLE Recurring_Tasks ADD COLUMN subtask_templates TEXT');
      }
    });
  }


  Future<void> _onCreate(Database db, int version) async {
    // 최초 생성 시의 스키마 (v4 기준)
    await db.execute('''
      CREATE TABLE Categories (
        id INTEGER PRIMARY KEY AUTOINCREMENT,
        name TEXT NOT NULL,
        color_code TEXT NOT NULL
      )
    ''');
    await db.execute('''
      CREATE TABLE Todos (
        id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
        category_id INTEGER,
        title TEXT NOT NULL,
        target_date TEXT NOT NULL,
        target_time TEXT,
        status TEXT NOT NULL CHECK(status IN ('pending', 'completed', 'failed')),
        created_at TEXT NOT NULL,
        completed_at TEXT,
        priority INTEGER,
        parent_id INTEGER,
        recurring_task_id INTEGER,
        display_order INTEGER,
        time_spent_seconds INTEGER NOT NULL DEFAULT 0,
        timer_duration_minutes INTEGER,
        timer_end_time TEXT,
        FOREIGN KEY (category_id) REFERENCES Categories(id) ON DELETE CASCADE,
        FOREIGN KEY (parent_id) REFERENCES Todos(id) ON DELETE CASCADE,
        FOREIGN KEY (recurring_task_id) REFERENCES Recurring_Tasks(id) ON DELETE SET NULL
      )
    ''');
    await db.execute('''
      CREATE TABLE Failure_Logs (
        id INTEGER PRIMARY KEY AUTOINCREMENT,
        todo_id INTEGER NOT NULL,
        reason TEXT NOT NULL,
        logged_at TEXT NOT NULL,
        FOREIGN KEY (todo_id) REFERENCES Todos(id) ON DELETE CASCADE
      )
    ''');
    await db.execute('''
      CREATE TABLE Recurring_Tasks (
        id INTEGER PRIMARY KEY AUTOINCREMENT,
        category_id INTEGER,
        title TEXT NOT NULL,
        target_time TEXT,
        recurrence_type TEXT NOT NULL,
        recurrence_detail TEXT,
        start_date TEXT NOT NULL,
        end_date TEXT,
        subtask_templates TEXT, -- [추가] 하위 작업 템플릿 (JSON 문자열)
        FOREIGN KEY (category_id) REFERENCES Categories(id) ON DELETE CASCADE
      )
    ''');
  }
}