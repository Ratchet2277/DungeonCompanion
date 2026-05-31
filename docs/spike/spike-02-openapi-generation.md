# Spike-02: OpenAPI Generation + Client Dart

**Duration estimate:** 1.5 days  
**Owner:** TBD (Backend + Flutter liaison)  
**Status:** Not started  

## Objectif

Valider que OpenAPI peut servir de source unique de vérité (source of truth) pour contrats API entre backend .NET et client Dart, réduisant duplication manuelle.

Spécifiquement:

1. Écrire OpenAPI spec minimale (3-4 endpoints: Character CRUD + Spell search).
2. Générer client HTTP Dart automatiquement (swagger_dart_code_generator).
3. Valider generated code qualité (type-safe, pas d'erreurs de compilation).
4. Mesurer effort: "OpenAPI first" vs "manual mapping".
5. Identifier gaps (si generation pas couvre tous les cas).

## Questions à résoudre

1. **OpenAPI tools Dart ecosystem**: quel generator choisir?
   - `swagger_dart_code_generator`: recommandé? Actif?
   - Alternative: openapi-generator (CLI Java-based)?
   - Critères: type-safety, freezed compat, maintien.

2. **API design patterns**: 
   - Idempotence (PUT vs PATCH pour sync)?
   - Batch operations (array inputs)?
   - Versioning strategy (v1, v2 endpoints)?
   - Error responses (standardized format)?

3. **DTO vs Domain**: 
   - Generated DTO classes suffisent ou custom modeling needed?
   - Comment mapper DTO → Domain (freezed models)?
   - Can generation + freezed coexist sans conflicts?

4. **Breaking changes**: 
   - Si OpenAPI change (field renamed), generated code auto-updates? 
   - Client old version compatibility?
   - Semver strategy?

5. **Generation CI/CD**: 
   - Où générer (dev machine vs CI pipeline)?
   - Trigger: manual, on PR, on tag?
   - Stockage: commit generated code ou .gitignore?

## Cas de test

### 1. Simple OpenAPI spec
```yaml
# openapi.yaml
openapi: 3.0.0
info:
  title: Companion JDR API
  version: 1.0.0

paths:
  /api/v1/characters:
    get:
      operationId: getCharacters
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
      operationId: createCharacter
      requestBody:
        required: true
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

components:
  schemas:
    CharacterDto:
      type: object
      required: [id, name, level]
      properties:
        id:
          type: string
        name:
          type: string
        level:
          type: integer
        attributes:
          type: object
          additionalProperties:
            type: integer
```

### 2. Generation Dart
```bash
# Install generator
flutter pub global activate swagger_dart_code_generator

# Generate
swagger_dart_code_generator \
  --input-file=openapi.yaml \
  --output-dir=lib/generated/api \
  --dart-package-name=companion_jdr_api

# Verify
ls lib/generated/api/
# Expected: api_client.dart, models/, etc.
```

### 3. Type-safety validation
```dart
// Generated code should compile without errors
final client = ApiClient();
client.setBasePath('http://localhost:3000');

// Type-safe call
final characters = await client.getCharacters(campaignId: 'camp-1');
// Expected: List<CharacterDto> (compiled type)

// Error if wrong type
// final bad = await client.getCharacters(campaignId: 123); // compile error!
```

### 4. DTO → Domain mapping
```dart
// Manually create mapper (not generated)
CharacterDto dto = ...;
Character domain = mapDtoToDomain(dto);

// Test round-trip
dto2 = mapDomainToDto(domain);
expect(dto, dto2);  // serialization stable
```

### 5. Breaking change scenario
```
# V1 spec: CharacterDto has field "level"
# Code generated, app deployed

# V2 spec: rename "level" → "characterLevel"
# Regenerate client code

# Test: old client can't deserialize new server response (intentional failure)
# Validate: semantic versioning + API versioning prevents silent corruption
```

### 6. CI/CD integration
```yaml
# Example GitHub Actions workflow
name: Generate API Client
on:
  push:
    paths:
      - 'openapi.yaml'

jobs:
  generate:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      - uses: subosito/flutter-action@v2
      - run: flutter pub global activate swagger_dart_code_generator
      - run: swagger_dart_code_generator --input-file=openapi.yaml --output-dir=lib/generated/api
      - run: flutter test  # Validate generated code compiles
      - run: git add lib/generated/api/ && git commit -m "Generated API client"
```

## Livrables

1. **Prototype OpenAPI spec** (minimal, 3-4 endpoints).
2. **Generated Dart client** (compilable, type-safe).
3. **Document**: "OpenAPI + Code Generation Setup Guide".
4. **Decision**: 
   - Generator recommendation (swagger_dart_code_generator vs alternative).
   - DTO/Domain mapping strategy.
   - CI/CD generation trigger + storage approach.

## Risques identifiés

1. **Generated code quality**: if swagger_dart_code_generator produces non-idiomatic Dart.
   → Mitigation: code review generated output early.

2. **Friction with freezed models**: generated DTOs vs hand-coded freezed models conflict.
   → Mitigation: test compat or use separate namespaces.

3. **Generation maintenance**: if swagger_dart_code_generator abandoned → look alternatives.
   → Mitigation: evaluate openapi-generator robustness.

4. **API design anti-patterns**: if OpenAPI spec poorly designed → generated code useless.
   → Mitigation: API design review in backend before generation.

## Critères de succès

- [ ] OpenAPI spec written (3-4 endpoints, documented).
- [ ] Client generated, compiles without errors.
- [ ] Generated DTO models type-safe (IDE autocomplete works).
- [ ] Mapper DTO → Domain implemented + tested.
- [ ] CI workflow triggers generation, validates output.
- [ ] Time measurement: "OpenAPI first" vs "manual" (document ROI).
- [ ] Decision documented: chosen generator + rationale.
