# Spike-01: Drift + SQLite Offline Persistence

**Duration estimate:** 2 days  
**Owner:** TBD (Flutter lead)  
**Status:** Not started  

## Objectif

Valider que Drift (ORM Dart pour SQLite) peut être utilisé comme couche persistence fiable pour notre use case offline-first. Spécifiquement:

1. Setup Drift avec schema minimal (Character table).
2. Implémenter CRUD basique.
3. Tester persistence sur mobile + web (différences de stockage).
4. Valider abstraction Repository pattern pour découplage.
5. Identifier limitations Drift vs alternatives (Isar, Hive, Sembast).

## Questions à résoudre

1. **Drift sur Flutter Web**: IndexedDB backend vs SQLite compat?
   - Drift Web docs: https://drift.simonbinder.eu/docs/platforms/web/
   - Faisabilité: IndexedDB mapped to Drift schema OK? Performance?

2. **Migrations Drift**: comment versioner schema dans une app offline-first?
   - Déploiement mobile: user sur v1.0, reçoit MAJ v1.1 avec nouvelle table → migration auto?
   - Rollback possible si conf?

3. **Repository abstraction**: comment abstraire Drift pour multi-platform?
   - Local SQLite: `DriftCharacterRepository implements CharacterRepository`
   - Web IndexedDB: `WebCharacterRepository implements CharacterRepository` (même interface)
   - Tester injection par provider Riverpod.

4. **Query performance**: schema avec 1000 spells (reference) + 50 perso fiches → query time acceptable?

5. **Concurrence**: si 2 screens modifient Character simultanément → flutter streams/listeners gérent OK?

## Cas de test

### 1. CRUD basique
```
- Créer Character(id: "c1", name: "Aragorn", level: 5)
- Fermer app complètement
- Rouvrir app
- Vérifier Character persiste avec exact même values
- Modifier level = 6
- Vérifier modification persiste
- Supprimer Character
- Vérifier suppression persiste
```

### 2. Migrations
```
- Schema V1: Character(id, name, level)
- App V1 utilisée, données sauvegardées
- Push App V2: schema V2 avec champ nouveau (portrait_url TEXT NULL)
- Mettre à jour app
- Vérifier: ancien Character charge sans erreur (portrait_url NULL)
- Créer nouveau Character avec portrait_url populé
- Vérifier persistence correcte
```

### 3. Repository abstraction
```
- Créer interface: abstract CharacterRepository { Future<Character> getById(String id); }
- Créer impl1: DriftCharacterRepository (SQLite)
- Créer impl2: MockCharacterRepository (in-memory pour tests)
- Créer provider Riverpod qui inject impl correct selon plateforme
- Tester switch impl sans changer UI layer
```

### 4. Query performance (optional pour V1)
```
- Population: 1000 Spell records + 50 Character records avec joins
- Query: getCharactersByLevel(5) pour 50 characters
- Mesure: time < 100ms?
- Index strategy: ajouter indexes si nécessaire
```

### 5. Concurrent modifications (optional pour V1)
```
- 2 Riverpod consumers regardent Character(id: "c1")
- Consumer 1 click "level ++"
- Consumer 2 click "level ++"
- Vérifier final value = +2 (pas race condition)
```

## Livrables

1. **Prototype mini-app Flutter** (25 lignes Drift schema + 100 lignes CRUD).
2. **Document**: "Drift Setup Guide" (installation, migration pattern, Repository layer).
3. **Decision**: OK to use Drift vs alternatives (avec justification courte).

## Risques identifiés

1. **Drift Web immature**: si IndexedDB mapping pas stable, repenser architecture.
   → Mitigation: tester early sur web.

2. **Migration complexity**: scale à 10+ versions → maintenance nightmare.
   → Mitigation: mettre versioning strategy en place des V1.

3. **Performance degradation**: if 10K+ records → query slowness.
   → Mitigation: indexing + pagination early.

## Critères de succès

- [ ] Character CRUD fonctionne offline (mobile/web).
- [ ] Migration V1 → V2 schema automatique, données préservées.
- [ ] Repository pattern fonctionne (interface + 2+ impls).
- [ ] Tests unitaires couvrent CRUD + migrations (10+).
- [ ] Performance acceptable (queries < 100ms).
