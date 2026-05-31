# Technical Architecture — Companion JDR

> Version: 1.1  
> Date: 2026-05-28  
> Applies: ADR-001, ADR-002, ADR-003

## 1. Architecture globale

```
┌─────────────────────────────────────────────────────┐
│                   CLIENT FLUTTER                     │
│  ┌──────────────────────────────────────────────┐   │
│  │         Presentation Layer (UI)              │   │
│  │  Screens, Widgets, Navigation, State        │   │
│  └──────────────────────────────────────────────┘   │
│                        ↓                              │
│  ┌──────────────────────────────────────────────┐   │
│  │      Application / State Management         │   │
│  │  Riverpod Providers, UseCases               │   │
│  └──────────────────────────────────────────────┘   │
│                        ↓                              │
│  ┌──────────────────────────────────────────────┐   │
│  │        Domain Layer (Business Logic)        │   │
│  │  Entities, Repositories (abstract), Rules   │   │
│  └──────────────────────────────────────────────┘   │
│                        ↓                              │
│  ┌──────────────────────────────────────────────┐   │
│  │         Data Layer (Local + Remote)         │   │
│  │  ┌────────────────────────────────────────┐ │   │
│  │  │ Local: Drift/SQLite + Sync Queue      │ │   │
│  │  ├────────────────────────────────────────┤ │   │
│  │  │ Remote: HTTP Client (generated from   │ │   │
│  │  │ OpenAPI) + Auth Token Manager         │ │   │
│  │  └────────────────────────────────────────┘ │   │
│  └──────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────┘
                        ↕ (HTTP)
┌─────────────────────────────────────────────────────┐
│                    API .NET BACKEND                 │
│  ┌──────────────────────────────────────────────┐   │
│  │   Controllers (OpenAPI endpoints)           │   │
│  └──────────────────────────────────────────────┘   │
│                        ↓                              │
│  ┌──────────────────────────────────────────────┐   │
│  │      Domain Services (Sync, Share, Auth)    │   │
│  └──────────────────────────────────────────────┘   │
│                        ↓                              │
│  ┌──────────────────────────────────────────────┐   │
│  │  Data Layer (SQL Server / PostgreSQL)       │   │
│  └──────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────┘
```

## 2. Stack technique client (Flutter)

### Dependencies clés
```yaml
dependencies:
  flutter:
    sdk: flutter
  
  # State management
  riverpod: ^latest
  riverpod_generator: ^latest
  
  # Local persistence
  drift: ^latest
  sqlite3_flutter_libs: ^latest
  
  # Networking
  dio: ^latest
  (OU generated client from OpenAPI)
  
  # Auth
  flutter_secure_storage: ^latest
  
  # Utilities
  freezed_annotation: ^latest
  json_annotation: ^latest
  
dev_dependencies:
  build_runner:
  riverpod_generator:
  drift_dev:
  freezed:
  json_serializable:
```

## 3. Layers client

### 3.1 Presentation Layer
```
lib/
├── screens/
│   ├── character_list_screen.dart
│   ├── character_detail_screen.dart
│   ├── spell_search_screen.dart
│   └── campaign_screen.dart
├── widgets/
│   ├── character_form_widget.dart
│   ├── spell_card_widget.dart
│   └── sync_status_widget.dart
└── navigation/
    └── router.dart (GoRouter config)
```

### 3.2 Application / State Management Layer (Riverpod)
```
lib/
├── providers/
│   ├── campaign_provider.dart       (watch currentCampaign)
│   ├── character_provider.dart      (CRUD character + local cache)
│   ├── spell_library_provider.dart  (reference data search)
│   ├── ruleset_provider.dart        (inject RuleSet based on campaign)
│   ├── sync_provider.dart           (queue + conflict resolution)
│   └── auth_provider.dart           (token + local auth state)
├── use_cases/
│   ├── create_character_use_case.dart
│   ├── sync_character_use_case.dart
│   └── share_character_use_case.dart
```

### 3.3 Domain Layer
```
lib/
├── entities/
│   ├── character.dart               (Freezed model + Drift annotations)
│   ├── campaign.dart
│   ├── spell.dart
│   ├── skill.dart
│   ├── ruleset.dart                 (abstract interface)
│   └── sync_event.dart              (change tracking)
├── repositories/
│   ├── character_repository.dart    (abstract)
│   ├── spell_repository.dart        (abstract)
│   ├── sync_repository.dart         (abstract)
│   └── auth_repository.dart         (abstract)
├── rule_sets/
│   ├── ruleset_interface.dart
│   ├── dnd35_ruleset.dart           (concrete impl)
│   ├── dnd35/
│   │   ├── dnd35_classes.dart
│   │   ├── dnd35_skills.dart
│   │   ├── dnd35_spells.dart
│   │   └── dnd35_calculations.dart
```

### 3.4 Data Layer
```
lib/
├── local/
│   ├── database.dart                (Drift DB schema)
│   ├── daos/
│   │   ├── character_dao.dart
│   │   ├── spell_dao.dart
│   │   └── sync_event_dao.dart
│   └── repositories/
│       ├── local_character_repository.dart
│       ├── local_spell_repository.dart
│       └── local_sync_repository.dart
├── remote/
│   ├── api_client.dart              (generated from OpenAPI or manual)
│   ├── interceptors/
│   │   ├── auth_interceptor.dart
│   │   └── retry_interceptor.dart
│   └── repositories/
│       ├── remote_character_repository.dart
│       ├── remote_sync_repository.dart
│       └── remote_share_repository.dart
├── sync/
│   ├── sync_engine.dart             (orchestrates sync)
│   ├── conflict_resolver.dart       (LWW + manual resolution)
│   └── sync_queue.dart              (persisted operation queue)
└── mappers/
    ├── character_dto_mapper.dart    (API DTO → Domain)
    ├── character_entity_mapper.dart (Domain → Local Entity)
    └── (other mappers...)
```

## 4. Modèle de données (Drift + Domain)

### 4.1 Character (principal)
```dart
// Domain entity (freezed)
@freezed
class Character with _$Character {
  const factory Character({
    required String id,
    required String campaignId,
    required String ownerId,              // propriétaire
    required String name,
    required String ruleSetId,            // "dnd-3.5"
    required int level,
    required int experience,
    required int currentHP,
    required int maxHP,
    
    // Stats génériques (DnD 3.5: STR, DEX, CON, INT, WIS, CHA)
    required Map<String, int> attributes,
    
    // Listes de références
    required List<String> spellIds,       // references
    required List<String> skillIds,       // references
    required List<String> featIds,        // references
    required List<String> weaponIds,      // references
    
    // System-specific data (JSON extensible)
    required Map<String, dynamic> systemData,   // ex: prestige_class, favored_enemy, etc.
    
    // Metadata sync (géré par SyncEngine)
    required DateTime updatedAt,
    required String? conflictState,       // null | 'conflict_level_vs_server' | ...
    required Map<String, SyncFieldMeta> fieldMeta,   // par champ
    
    // Notes et flavor
    String? notes,
    String? portraitUrl,
  }) = _Character;
}

// Local entity (Drift)
@DataClassName("CharacterLocal")
class Characters extends Table {
  TextColumn get id => text()();
  TextColumn get campaignId => text()();
  TextColumn get ownerId => text()();
  TextColumn get name => text()();
  TextColumn get ruleSetId => text()();
  IntColumn get level => integer()();
  IntColumn get experience => integer()();
  IntColumn get currentHP => integer()();
  IntColumn get maxHP => integer()();
  TextColumn get attributesJson => text()();    // JSON
  TextColumn get systemDataJson => text()();    // JSON
  DateTimeColumn get updatedAt => dateTime()();
  TextColumn get conflictState => text().nullable()();
  TextColumn get fieldMetaJson => text()();     // JSON
  TextColumn get notes => text().nullable()();
  
  @override
  Set<Column> get primaryKey => {id};
  @override
  List<Set<Column>> get uniqueKeys => [{campaignId, ownerId, name}];  // no duplicate names per owner
}
```

### 4.2 Spell (reference data)
```dart
@freezed
class Spell with _$Spell {
  const factory Spell({
    required String id,
    required String name,
    required String ruleSetId,       // "dnd-3.5"
    required int level,              // 0-9
    required String school,          // Evocation, Enchantment, etc.
    required String castingTime,     // "1 action", "1 bonus action", etc.
    required int range,              // feet
    String? component,               // V, S, M, F
    String? duration,
    String? savingThrow,             // Will, Fortitude, Reflex
    String? description,
    List<String>? tags,              // #concentration, #ritual, #prepared, etc.
  }) = _Spell;
}

// Local entity (Drift) — READ ONLY après import dataset
@DataClassName("SpellLocal")
class Spells extends Table {
  TextColumn get id => text()();
  TextColumn get name => text()();
  TextColumn get ruleSetId => text()();
  IntColumn get level => integer()();
  TextColumn get school => text()();
  TextColumn get castingTime => text()();
  IntColumn get range => integer()();
  TextColumn get component => text().nullable()();
  TextColumn get duration => text().nullable()();
  TextColumn get savingThrow => text().nullable()();
  TextColumn get description => text().nullable()();
  TextColumn get tagsJson => text().nullable()();    // JSON array
  DateTimeColumn get datasetVersion => dateTime()(); // versioning dataset
  
  @override
  Set<Column> get primaryKey => {ruleSetId, id};
}
```

### 4.3 SyncEvent (journal des modifications)
```dart
@freezed
class SyncEvent with _$SyncEvent {
  const factory SyncEvent({
    required String id,
    required String entityType,           // "Character", "Spell", etc.
    required String entityId,
    required String field,                // "level", "name", etc.
    required dynamic oldValue,
    required dynamic newValue,
    required DateTime timestamp,
    required String sourceClientId,       // quelle app/device
    required String operationType,        // "update" | "delete" | "create"
    required SyncStatus status,           // pending, synced, conflicted, resolved
  }) = _SyncEvent;
}

enum SyncStatus { pending, synced, conflicted, resolved }

// Local entity (Drift)
@DataClassName("SyncEventLocal")
class SyncEvents extends Table {
  TextColumn get id => text()();
  TextColumn get entityType => text()();
  TextColumn get entityId => text()();
  TextColumn get field => text()();
  TextColumn get oldValueJson => text().nullable()();
  TextColumn get newValueJson => text().nullable()();
  DateTimeColumn get timestamp => dateTime()();
  TextColumn get sourceClientId => text()();
  TextColumn get operationType => text()();
  TextColumn get status => text()();    // converted to/from enum
  
  @override
  Set<Column> get primaryKey => {id};
  @override
  List<Set<Column>> get uniqueKeys => [{entityType, entityId, timestamp}];
}
```

### 4.4 Campaign et autres entités
```dart
@freezed
class Campaign with _$Campaign {
  const factory Campaign({
    required String id,
    required String name,
    required String ruleSetId,         // verrouillé une fois créé
    required String masterPlayerId,
    required List<String> playerIds,
    DateTime? createdAt,
    DateTime? updatedAt,
  }) = _Campaign;
}

// Similar structure pour Share, Session, etc.
```

## 5. OpenAPI et contrats API

### 5.1 Endpoints principaux (.NET backend)

```yaml
# openapi.yaml (simplified)

paths:
  /api/v1/characters:
    get:
      tags: [Character]
      summary: List characters for campaign
      parameters:
        - name: campaignId
          in: query
          required: true
          schema:
            type: string
      responses:
        '200':
          content:
            application/json:
              schema:
                type: array
                items:
                  $ref: '#/components/schemas/CharacterDto'
    post:
      tags: [Character]
      summary: Create character
      requestBody:
        content:
          application/json:
            schema:
              $ref: '#/components/schemas/CreateCharacterRequest'
      responses:
        '201':
          content:
            application/json:
              schema:
                $ref: '#/components/schemas/CharacterDto'

  /api/v1/characters/{id}:
    get:
      tags: [Character]
      summary: Get character by ID
      parameters:
        - name: id
          in: path
          required: true
          schema:
            type: string
      responses:
        '200':
          content:
            application/json:
              schema:
                $ref: '#/components/schemas/CharacterDto'
    put:
      tags: [Character]
      summary: Update character (idempotent)
      parameters:
        - name: id
          in: path
          required: true
          schema:
            type: string
      requestBody:
        content:
          application/json:
            schema:
              $ref: '#/components/schemas/UpdateCharacterRequest'
      responses:
        '200':
          content:
            application/json:
              schema:
                $ref: '#/components/schemas/CharacterDto'

  /api/v1/sync:
    post:
      tags: [Sync]
      summary: Sync changes (batch operations)
      requestBody:
        content:
          application/json:
            schema:
              type: array
              items:
                $ref: '#/components/schemas/SyncOperation'
      responses:
        '200':
          content:
            application/json:
              schema:
                type: array
                items:
                  $ref: '#/components/schemas/SyncOperationResult'

  /api/v1/characters/{id}/share:
    post:
      tags: [Share]
      summary: Share character with user
      parameters:
        - name: id
          in: path
          required: true
          schema:
            type: string
      requestBody:
        content:
          application/json:
            schema:
              $ref: '#/components/schemas/ShareCharacterRequest'
      responses:
        '200':
          content:
            application/json:
              schema:
                $ref: '#/components/schemas/ShareCharacterResponse'

  /api/v1/reference-data/spells:
    get:
      tags: [ReferenceData]
      summary: Get spells (paginated, by ruleSet)
      parameters:
        - name: ruleSetId
          in: query
          required: true
          schema:
            type: string
        - name: page
          in: query
          schema:
            type: integer
            default: 0
        - name: limit
          in: query
          schema:
            type: integer
            default: 50
      responses:
        '200':
          content:
            application/json:
              schema:
                $ref: '#/components/schemas/PagedSpellList'

components:
  schemas:
    CharacterDto:
      type: object
      required: [id, name, level, ruleSetId]
      properties:
        id:
          type: string
        name:
          type: string
        level:
          type: integer
        ruleSetId:
          type: string
        attributes:
          type: object
          additionalProperties:
            type: integer
        systemData:
          type: object
          additionalProperties: {}
        updatedAt:
          type: string
          format: date-time
        conflictState:
          type: string
          nullable: true
        fieldMeta:
          type: object
          additionalProperties:
            $ref: '#/components/schemas/SyncFieldMeta'
    
    SyncOperation:
      type: object
      required: [op, entityType, entityId, field, value, timestamp, clientId]
      properties:
        op:
          type: string
          enum: [create, update, delete]
        entityType:
          type: string
        entityId:
          type: string
        field:
          type: string
        value:
          nullable: true
        timestamp:
          type: string
          format: date-time
        clientId:
          type: string
```

### 5.2 Génération client Dart
```bash
# CLI pour générer client Dart depuis OpenAPI
flutter pub run swagger_dart_code_generator:swagger_dart_code_generator \
  --input-file=openapi.yaml \
  --output-dir=lib/generated/api \
  --dart-package-name companion_jdr_api

# Résultat: lib/generated/api/client.dart avec tous les models et HTTP methods typés
```

## 6. Synchronisation et résolution de conflits

### 6.1 Sync Engine (orchestration)

```dart
/// Responsabilités:
/// - Maintenir queue de sync local
/// - Envoyer opérations batch au serveur
/// - Recevoir updates serveur
/// - Détecter et résoudre conflits
/// - Appliquer changements en local
class SyncEngine {
  final LocalDatabase db;
  final RemoteApi api;
  final ConflictResolver conflictResolver;
  
  /// Appel périodiquement (ex: toutes les 30s)
  Future<void> sync() async {
    // 1. Récupérer queue locale (pending)
    final pendingOps = await db.syncEventDao.getPendingOperations();
    
    // 2. Si queue vide, pull latest from serveur
    if (pendingOps.isEmpty) {
      await pullRemoteChanges();
      return;
    }
    
    // 3. Push operations au serveur
    final results = await api.sync(pendingOps);
    
    // 4. Traiter résultats
    for (final result in results) {
      if (result.status == SyncResultStatus.synced) {
        // Marquer opération comme syncée
        await db.syncEventDao.updateStatus(result.opId, SyncStatus.synced);
      } else if (result.status == SyncResultStatus.conflict) {
        // Marquer en confllit, déclencher résolution
        await handleConflict(result);
      }
    }
    
    // 5. Pull remote changements (autres clients)
    await pullRemoteChanges();
  }
  
  Future<void> pullRemoteChanges() async {
    final lastSync = await db.metadata.getLastSyncTimestamp();
    final remoteChanges = await api.getChanges(since: lastSync);
    
    for (final change in remoteChanges) {
      // Appliquer changement distant en local
      // (si conflit local pending, voir handleConflict)
      await applyRemoteChange(change);
    }
    
    await db.metadata.updateLastSyncTimestamp(DateTime.now());
  }
  
  Future<void> handleConflict(ConflictResult conflict) async {
    // Déterminer si champ est critique ou non
    final isCritical = criticalFields.contains(conflict.field);
    
    if (!isCritical) {
      // Auto-resolve: serveur gagne (LWW)
      await db.syncEventDao.updateStatus(conflict.opId, SyncStatus.resolved);
      // Appliquer valeur serveur localement
      await applyRemoteChange(conflict.serverVersion);
    } else {
      // Marquer comme "waiting for user"
      await db.character.updateConflictState(
        conflict.entityId,
        'conflict_${conflict.field}_vs_server'
      );
      // UI affichera écran de résolution
    }
  }
}

const criticalFields = [
  'level',
  'experience',
  'maxHP',
  'attributes', // stats de base
];
```

### 6.2 Conflict Resolver (UI)

```dart
/// Widget affiché si conflit critique en attente
class ConflictResolutionWidget extends StatelessWidget {
  final String entityId;
  final String field;
  final dynamic localValue;
  final dynamic serverValue;
  
  @override
  Widget build(BuildContext context) {
    return AlertDialog(
      title: Text('Conflit de synchronisation: $field'),
      content: Column(
        mainAxisSize: MainAxisSize.min,
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Text('Votre modification locale: $localValue'),
          SizedBox(height: 16),
          Text('Modification serveur: $serverValue'),
          SizedBox(height: 16),
          Text('Lequel garder?'),
        ],
      ),
      actions: [
        TextButton(
          onPressed: () => resolveConflict(context, localValue),
          child: Text('Garder ma version'),
        ),
        TextButton(
          onPressed: () => resolveConflict(context, serverValue),
          child: Text('Accepter serveur'),
        ),
      ],
    );
  }
  
  void resolveConflict(BuildContext context, dynamic chosenValue) async {
    // Envoyer décision au serveur (force-set)
    // Serveur accepte + broadcast aux autres clients
    // Marquer conflict comme resolved localement
  }
}
```

## 7. RuleSet Plugin (extensibilité)

### 7.1 Interface abstraite

```dart
abstract class RuleSet {
  String get id;           // "dnd-3.5"
  String get name;         // "D&D 3.5 Edition"
  String get version;      // "3.5.1"
  
  // Available content
  List<RuleClass> getAvailableClasses();
  List<RuleSkill> getAvailableSkills();
  List<RuleSpell> getAvailableSpells();
  List<RuleFeat> getAvailableFeats();
  
  // Calculations
  int calculateAttackBonus(Character char, {Weapon? weapon});
  int calculateArmorClass(Character char);
  List<SavingThrow> calculateSavingThrows(Character char);
  int calculateSkillModifier(Character char, String skillId);
  
  // Validations
  List<String> validateCharacter(Character char);  // returns errors
  bool canPrepareSpell(Character char, Spell spell);
  bool canMulticlass(Character existing, String newClass);
  
  // Features
  bool supportsMulticlass();
  bool supportsPrestigeClasses();
  bool supportsPreparedSpells();
  bool supportsFavoritedEnemy();  // DnD 3.5 Ranger only
}
```

### 7.2 Implémentation DnD 3.5

```dart
class DnD35RuleSet implements RuleSet {
  @override
  String get id => 'dnd-3.5';
  
  @override
  List<RuleClass> getAvailableClasses() {
    return [
      DnD35ClassFighter(),
      DnD35ClassWizard(),
      DnD35ClassCleric(),
      // ... 11 classes
    ];
  }
  
  @override
  int calculateAttackBonus(Character char, {Weapon? weapon}) {
    // Base Attack Bonus from level (class-dependent)
    final baseAB = getClassBAB(char.class, char.level);
    
    // Ability modifier
    final ability = weapon?.abilityUsed ?? 'STR';
    final modifier = (char.attributes[ability]! - 10) ~/ 2;
    
    return baseAB + modifier;
  }
  
  // ... other implementations
}
```

## 8. Observabilité et logging

### 8.1 Events loggés

- Sync operations (push, pull, conflict)
- User actions (create, edit, delete, share)
- Errors (network, validation, data integrity)
- Performance (sync duration, DB query time, UI frame drops)

### 8.2 Format

```dart
enum LogLevel { debug, info, warning, error, critical }

class LogEvent {
  final LogLevel level;
  final String category;      // "sync", "ui", "data", "network"
  final String message;
  final dynamic data;         // contexte supplémentaire
  final DateTime timestamp;
  final String? stackTrace;
}

// Local sqlite logger, peut être uploadé serveur pour analytics
```

## 9. Checklist de validation

Avant "end of phase 1":

- [ ] Drift schema compilé et migrations testées
- [ ] Character CRUD fonctionne offline
- [ ] Sync engine push/pull implémenté (au moins 1 entité: Character)
- [ ] LWW conflict resolution pour champs non-critiques
- [ ] OpenAPI generation client Dart valide (au moins endpoints Character)
- [ ] RuleSet plugin structure validée (DnD 3.5 class skeleton)
- [ ] Tests unitaires couche domain (20+ cas)
- [ ] Tests d'intégration sync (offline scénarios)
- [ ] UI basic: Character list, detail, edit screens
- [ ] Auth minimale (local user ID ou token mock)
