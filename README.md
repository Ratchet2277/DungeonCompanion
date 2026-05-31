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

---

## 🚀 Timeline (Best Case)

```
Spike Technique ────────────────────────────────┐ (2 sem)
                                                ↓
Backend Foundation (3 sem) ────┐
                               ├→ Client Core (4 sem) → RuleSet D&D 3.5 (3 sem)
Spikes validés ─────────────→┘                              ↓
                                        Premium Features (3 sem)
                                               ↓
                                        Self-Host (2 sem)
                                               ↓
                                        Polish & Release (3 sem)
                                        ✅ v1.0 (Oct/Nov 2026)

Total: ~15-20 semaines
```

---

## 👥 Staffing

**Minimal équipe:**
- Backend Lead (.NET) — 80% Phase 2-6
- Frontend Lead (Flutter) — 80% Phase 1, 3-4
- QA/Testing (part-time)
- DevOps (part-time)

**Solo developer:** possible, durée +50%.

---

## ✅ Checklist Démarrage Phase 1

- [ ] Lire [EXECUTIVE-SUMMARY.md](docs/EXECUTIVE-SUMMARY.md) (5 min).
- [ ] Lire [architecture-technique-v1.1.md](docs/architecture-technique-v1.1.md) (30 min).
- [ ] Assigner ownership spike-01, spike-02, spike-03.
- [ ] Créer branches Git (feature/spike-01, 02, 03).
- [ ] Kickoff réunion (1h) → distribuer spikes.
- [ ] Checkpoint 1 semaine (vendredi) → partager résultats.

---

## 📋 Structure Dossier

```
companion/
├── README.md                         ← Vous êtes ici
├── docs/
│   ├── INDEX.md                      ← Navigation docs
│   ├── EXECUTIVE-SUMMARY.md          ← Vue haut niveau
│   ├── specs.md                      ← Vision produit
│   ├── architecture-technique-v1.1.md ← Architecture détaillée
│   ├── NEXT-STEPS.md                 ← Roadmap exécution
│   ├── adr/
│   │   ├── 001-flutter-over-net-maui.md
│   │   ├── 002-offline-first-lww.md
│   │   └── 003-multi-ruleset-plugin.md
│   └── spike/
│       ├── spike-01-drift-sync.md
│       ├── spike-02-openapi-generation.md
│       └── spike-03-conflict-resolution.md
├── src/
│   ├── flutter/                      ← Client (à créer Phase 1)
│   └── backend/                      ← Backend .NET (à créer Phase 1)
└── .gitignore
```

---

## 🎓 Aspect Éducatif

Ce projet est conçu aussi comme **apprentissage**:

- **Offline-first architecture** (patterns réels, pas tutoriels jouets).
- **Sync conflicts** (last-write-wins, resolution manuelle).
- **Plugin architecture** (extensibilité, séparation concerns).
- **Full-stack** (Flutter + .NET + OpenAPI).
- **Clean architecture** (layers, dependency injection, testing).

Parfait pour portfolio senior engineering.

---

## 🤔 Questions Fréquentes

**Q: C'est un VTT?**  
R: Non. C'est un "second cerveau" de campagne. Utilisé **en parallèle** de Roll20/MapTool pour le long terme + gestion offline.

**Q: Ça remplace D&D Beyond / other character sheets?**  
R: Partiellement. Focus sur fiches + base référence + sync multi-joueurs. Pas de marketplace, pas d'intégrations profonds.

**Q: Offline obligatoire?**  
R: Oui. C'est core au design. L'app fonctionne 100% localement, sync cloud est optionnel/async.

**Q: Pourquoi Flutter et pas full C#?**  
R: UX mobile meilleure, offline écosystème mature, web inclus. Voir [ADR-001](docs/adr/001-flutter-over-net-maui.md).

**Q: On peut self-hoster?**  
R: Oui, au minimum données perso (backup). Partage cloud est optionnel. Docker support prévu Phase 6.

**Q: Quand v1.0?**  
R: Octobre/Novembre 2026 (best case, si timeline respectée).

---

## 💬 Contacts & Ownership

(À compléter avant kickoff)

- **Product Lead**: TBD
- **Architecture Lead**: TBD
- **Backend Lead**: TBD
- **Frontend Lead**: TBD
- **DevOps**: TBD

---

## 📝 License & Credits

(À définir: MIT, GPL, CC, etc.?)

---

## 🔗 Liens Utiles

- [Flutter Docs](https://flutter.dev/)
- [Drift ORM](https://drift.simonbinder.eu/)
- [Riverpod](https://riverpod.dev/)
- [OpenAPI 3.0](https://spec.openapis.org/oas/v3.0.0)
- [.NET 8](https://dotnet.microsoft.com/)

---

## 📅 Versionning

| Version | Date | Status | Notes |
|---------|------|--------|-------|
| 0.1 (Specs) | 2026-05-28 | ✅ Complete | Brainstorming → Architecture |
| Phase 1 (Spike) | TBD | ⏳ Next | Validation techniques |
| Phase 2+ (Dev) | TBD | 📋 Roadmap | Voir NEXT-STEPS.md |

---

**🚀 Ready to build? → See [NEXT-STEPS.md](docs/NEXT-STEPS.md)**

*Last updated: 2026-05-28*
