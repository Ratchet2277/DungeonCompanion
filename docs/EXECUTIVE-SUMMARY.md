# Companion JDR — Executive Summary

**Projet:** Application compagnon pour JDR de table (D&D 3.5+)  
**Status:** Definition Complete, Ready for Spike Phase  
**Date:** 2026-05-28

---

## Vision

Créer un "second cerveau" de campagne qui centralise fiches personnages, base de connaissances (sorts, compétences, armes) et synchronisation multi-joueurs. **Pas un VTT**: complément à Roll20/MapTool pour le long terme + gestion offline.

---

## Positionnement produit

- **Pour qui**: MJ + Joueurs, initial D&D 3.5, extensible multi-systèmes.
- **Quand**: utilisée avant/pendant/après session.
- **Où**: mobile + desktop (web bonus).
- **Comment**: offline-first, sync cloud central, self-host options.

---

## Modèle business

- **Gratuit**: fiches, base référence, sync basique.
- **Premium**: scaling auto stats, dice roller, gestion sorts préparés (Patreon).
- **Self-host**: donnees perso local (hébergement minimum).

---

## Architecture validée

| Couche | Technologie | Justification |
|--------|-------------|--------------|
| **Client** | Flutter (Dart) | UX native mobile, offline robuste, web bonus |
| **Persistence locale** | Drift + SQLite | ORM typé, migrations, multi-plateforme |
| **State management** | Riverpod | providers, testabilité, scalabilité |
| **Sync strategy** | Offline-first + LWW | offline 100%, conflicts simples sauf champs critiques |
| **Backend** | .NET 8 + EntityFramework | OpenAPI first, entitlements, self-host Docker |
| **Extensibilité règles** | Plugin RuleSet pattern | D&D 3.5 isolé, Pathfinder/5e facile après |
| **Contract API** | OpenAPI 3.0 | source unique vérité, generation client Dart |

---

## Arbitrages clés (ADRs)

1. **ADR-001: Flutter > .NET MAUI**  
   Raison: meilleure UX mobile native + écosystème offline + web inclus.

2. **ADR-002: Offline-first + Last-Write-Wins**  
   Raison: pragmatisme V1 + cas de conflict rares + resolution manuelle pour critique.

3. **ADR-003: Plugin RuleSet**  
   Raison: extensibilité long-terme, découplage métier, pédagogique.

---

## Périmètre PMV

**Inclus:**
- ✅ Gestion fiches personnages.
- ✅ Base référence (sorts, compétences, armes) recherche + filtres.
- ✅ Sauvegarde locale offline.
- ✅ Sync cloud pour partage MJ/joueur.
- ✅ Partage fiche avec gestion droits.
- ✅ Journal d'événements campagne.

**Explicitement exclus v1:**
- ❌ VTT complet (carte, fog of war).
- ❌ Initiative/combat avancé.
- ❌ Intégrations Roll20/MapTool.
- ❌ Marketplace contenu.
- ❌ Audio/vidéo.

---

## Spikes technos planifiés

| Spike | Durée | Objective | Owner |
|-------|-------|-----------|-------|
| 01 - Drift Offline | 2 j | Valider persistence SQLite multi-plateforme | Flutter Lead |
| 02 - OpenAPI Gen | 1.5 j | Valider generation client Dart réduit boilerplate | Backend + Flutter liaison |
| 03 - Conflicts | 2.5 j | Valider LWW + UI resolution scenarios | Flutter Lead + Backend |

---

## Timeline estimée (Best case)

```
Spike (2 sem)
    ↓
Backend Foundation (3 sem) ┐
                          ├→ Client Core (4 sem) → RuleSet D&D 3.5 (3 sem)
Spikes (validé) ────────→┘                              ↓
                                        Premium Features (3 sem)
                                               ↓
                                        Self-Host (2 sem)
                                               ↓
                                        Polish & Release (3 sem)

Total: ~15-20 semaines → v1.0 octobre/novembre 2026
```

---

## Staffing minimal

- 1x Backend Lead (.NET).
- 1x Frontend Lead (Flutter).
- 1x QA (part-time).
- 1x DevOps (part-time).

Solo (full-stack dev): possible, durée +50%.

---

## Success Criteria v1.0

- [ ] Offline CRUD robuste (< 1% sync errors).
- [ ] D&D 3.5 character sheet complet + calculations.
- [ ] 100+ beta users, retention 30j > 40%.
- [ ] App load < 2s, sync < 500ms.
- [ ] Security: auth + data protection.

---

## Risques principaux

| Risque | Mitigation |
|--------|-----------|
| Drift Web instable | Early spike validation |
| Conflicts plus complex que prévu | Thorough spike-03 testing |
| D&D rules complexity | Modular implementation, early validation |
| Self-host adoption faible | Post-v1.0 concern, prioritize if needed |

---

## Documentation produite

✅ [specs.md](specs.md) — Vision + périmètre + business  
✅ [architecture-technique-v1.1.md](architecture-technique-v1.1.md) — Technical deep-dive  
✅ [ADR-001](adr/001-flutter-over-net-maui.md) — Flutter decision  
✅ [ADR-002](adr/002-offline-first-lww.md) — Offline-first decision  
✅ [ADR-003](adr/003-multi-ruleset-plugin.md) — RuleSet extensibility  
✅ [spike-01-drift-sync.md](spike/spike-01-drift-sync.md) — Offline persistence validation  
✅ [spike-02-openapi-generation.md](spike/spike-02-openapi-generation.md) — Code generation validation  
✅ [spike-03-conflict-resolution.md](spike/spike-03-conflict-resolution.md) — Sync conflicts validation  
✅ [NEXT-STEPS.md](NEXT-STEPS.md) — Roadmap d'exécution  
✅ [INDEX.md](INDEX.md) — Navigation docs  

---

## Prochaine action immédiate

1. **Assigner ownership** à chaque spike (backend, frontend, liaison).
2. **Créer branches Git** (feature/spike-01, 02, 03).
3. **Kickoff réunion** (1 heure) → distribuer spikes + aligner expectation.
4. **Checkpoint 1 semaine** (vendredi) → share spike results + decisions.

---

## Questions avant kickoff?

- Équipe confirmée?
- Date cible v1.0 correct (oct/nov 2026)?
- Budget infra ok?
- Patreon timeline?
