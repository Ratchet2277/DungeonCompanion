# Companion JDR — Application Compagnon pour Jeux de Rôle sur Table

> **Status:** 🟢 Definition Complete | Ready for Phase 1 (Spike Technique)  
> **Date:** 2026-05-28

---

## 🎯 En 30 secondes

**Companion JDR** est une application mobile + desktop qui centralise:
- Fiches personnages (création, édition, persistance).
- Base de connaissances (sorts, compétences, armes, équipements).
- Synchronisation multi-joueurs (MJ ↔ Joueurs).
- Support offline complet (fonctionne partout, synchro quand connecté).

**Positionnement:** Complément à Roll20/MapTool, pas remplacement. Utilisée **avant/pendant/après** session.

**Cible initiale:** D&D 3.5 avec architecture extensible (Pathfinder, D&D 5e, etc.)

---

## 📚 Documentation

### Nouveaux venus? Commencez ici

1. **2 min** — [EXECUTIVE-SUMMARY.md](docs/EXECUTIVE-SUMMARY.md)  
   Vue d'ensemble produit + stack technique + timeline.

2. **15 min** — [specs.md](docs/specs.md)  
   Vision produit complète + périmètre + utilisateurs.

3. **30 min** — [architecture-technique-v1.1.md](docs/architecture-technique-v1.1.md)  
   Architecture layers, modèles Dart, API OpenAPI, sync engine.

4. **5 min** — [INDEX.md](docs/INDEX.md)  
   Navigation docs + guides par profil (backend, frontend, QA, etc.)

### Décisions techniques (pourquoi c'est comme ça)

- [ADR-001: Flutter over .NET MAUI](docs/adr/001-flutter-over-net-maui.md)
- [ADR-002: Offline-first + Last-Write-Wins](docs/adr/002-offline-first-lww.md)
- [ADR-003: Plugin RuleSet (extensibilité)](docs/adr/003-multi-ruleset-plugin.md)

### Prochaines étapes exécution

- [NEXT-STEPS.md](docs/NEXT-STEPS.md) — Roadmap 6-12 mois (phases, timeline, staffing).
- [spike-01-drift-sync.md](docs/spike/spike-01-drift-sync.md) — Validation offline persistence.
- [spike-02-openapi-generation.md](docs/spike/spike-02-openapi-generation.md) — Validation code generation.
- [spike-03-conflict-resolution.md](docs/spike/spike-03-conflict-resolution.md) — Validation sync conflicts.

---

## 🏗️ Stack technique

| Couche | Technology | Pourquoi |
|--------|-----------|---------|
| **Mobile + Desktop** | Flutter (Dart) | UX native, offline robuste, web inclus |
| **Persistence locale** | Drift + SQLite | ORM typé, migrations, multi-plateforme |
| **State management** | Riverpod | Providers, testabilité, scalabilité |
| **Synchronisation** | Offline-first + LWW | Offline 100%, conflicts pragmatiques |
| **Backend** | .NET 8 + EF Core | OpenAPI first, self-host possible |
| **API** | OpenAPI 3.0 | Source unique vérité, generation client |
| **Extensibilité** | Plugin RuleSet | D&D 3.5 isolé, autres systèmes faciles |
*Last updated: 2026-05-28*
