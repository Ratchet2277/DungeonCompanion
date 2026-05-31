# Next Steps — Roadmap d'exécution

**Date:** 2026-05-28  
**Horizon:** 6-12 mois (à partir d'aujourd'hui)

---

## Synthèse de ce qui a été fait

✅ **Phase Definition (COMPLETE)** — 1 jour

1. Vision produit + périmètre PMV.
2. 3 ADRs (Flutter, Offline-first LWW, Plugin RuleSet).
3. Architecture technique détaillée (layers, modèles, API).
4. Navigation + guides documentation.
5. 3 spikes technos (offline, generation, conflicts).

État: **Tous les arbitrages majeurs sont verrouillés.** Prêt à passer au code.

---

## Phase 1: Spike Technique (1-2 semaines)

**Objectif:** Valider les choix technos sur du code réel, pas sur du papier.

### Spike-01: Drift + SQLite (2 jours)
- [ ] Setup Flutter project vierge.
- [ ] Intégrer Drift, créer schema Character minimal.
- [ ] Implémenter CRUD.
- [ ] Tester offline persist + reopen app.
- [ ] Tester sur web (IndexedDB backend).
- [ ] ✅ Sign-off: "Drift is viable for offline persistence."

### Spike-02: OpenAPI Gen (1.5 jours)
- [ ] Écrire OpenAPI spec minimale (3-4 endpoints).
- [ ] Générer client Dart (swagger_dart_code_generator).
- [ ] Tester generation quality + type-safety.
- [ ] Mapper DTO → Domain avec freezed.
- [ ] ✅ Sign-off: "OpenAPI generation reduces boilerplate 60%+."

### Spike-03: Conflict Resolution (2.5 jours)
- [ ] Implémenter SyncEngine + conflict detection.
- [ ] Créer UI resolution widget.
- [ ] Simuler offline 2 devices → sync + conflict.
- [ ] Tester 5 scenarios (voir spike-03.md).
- [ ] ✅ Sign-off: "LWW + manual resolution strategy works."

**Livrables Phase 1:**
- 3 branches Git (une par spike), code commented.
- 3 decision documents (viability + recommendation).
- Prototype "hello world" Flutter app with Character CRUD + offline + sync.

---

## Phase 2: Backend Foundation (2-3 semaines)

**Objectif:** API .NET robuste et contractualisée.

### Tasks
- [ ] **Project setup**: .NET 8 project, Entity Framework Core.
- [ ] **OpenAPI spec** (finalized, auto-generated via Swashbuckle).
- [ ] **Core endpoints** (CRUD Character, Spell listing, Sync).
- [ ] **Auth minimal** (JWT tokens, local user registration).
- [ ] **Sync engine server-side** (merge logic, conflict detection, broadcast).
- [ ] **Database** (SQL Server or PostgreSQL, schema per ADRs).
- [ ] **Tests** (unit tests sync logic, API contract tests).
- [ ] **Docker** (containerize backend, ready for self-host).

**Livrables:**
- OpenAPI spec (runnable, integrates with client generation).
- Backend compiles, tests pass (> 70% coverage).
- Docker image buildable.

---

## Phase 3: Client Core (3-4 semaines)

**Objectif:** Flutter app avec CRUD + offline + sync.

### Tasks
- [ ] **Project setup** (Flutter, Riverpod, Drift, generated API client).
- [ ] **Drift schema** (full models: Character, Spell, Skill, Campaign, Share, etc.).
- [ ] **Repository layer** (abstract + local impl).
- [ ] **Sync engine** (pull/push orchestration, conflict resolution).
- [ ] **Auth UI** (register, login, local user fallback).
- [ ] **Character screens** (list, detail, create, edit, delete).
- [ ] **Spell library** (search, filter, attach to character).
- [ ] **Share UI** (invite MJ, view shared character).
- [ ] **Tests** (unit domain, integration sync, widget UI).

**Livrables:**
- App mobile (Android + Web testable).
- CRUD fonctionne offline.
- Sync works (tested with Phase 2 backend).
- Test coverage > 60%.

---

## Phase 4: RuleSet Plugin (DnD 3.5) (2-3 semaines)

**Objectif:** Première implémentation complète des règles D&D 3.5.

### Tasks
- [ ] **RuleSet interface** (finalized from ADR-003).
- [ ] **DnD35RuleSet class** (calculator, class definitions, skill system).
- [ ] **Classes** (Fighter, Wizard, Cleric, ... all 11).
- [ ] **Skills** (all 35 skills, modifiers per stat).
- [ ] **Spells** (dataset import, ~3500 spells for D&D 3.5).
- [ ] **Feats** (common feats dataset).
- [ ] **Character sheet** (stat block display, calculations visible).
- [ ] **Tests** (validate character integrity, calculations match rules).

**Livrables:**
- D&D 3.5 character sheet complete + functional.
- Dataset spells/skills/feats imported.
- UI displays stat block with calculated modifiers.
- Test coverage > 70% (especially calculations).

---

## Phase 5: Premium Features (2-3 semaines)

**Objectif:** Montie en gamme freemium → premium.

### Features
- [ ] **Entitlements system** (backend flags active subscriptions).
- [ ] **Dice Roller** (integrated 3D dice, rolls logged to journal).
- [ ] **Scaling auto** (stat adjustments per character concept).
- [ ] **Prepared spells** (wizard prepared spells management).
- [ ] **Premium UI unlock** (paywalling features).
- [ ] **Patreon integration** (webhook sync entitlements).

**Livrables:**
- Premium features gated correctly.
- Patreon sync working.
- UX clearly indicates premium-only.

---

## Phase 6: Self-Host Foundation (1-2 semaines)

**Objectif:** Utilisateurs peuvent héberger leurs données (options minimales).

### Tasks
- [ ] **Helm chart** ou **Docker Compose** pour backend (data local).
- [ ] **Configuration docs** (how to self-host + sync cloud for shared data).
- [ ] **Data export** (backup campaign as JSON + attached files).
- [ ] **Data import** (restore from backup).
- [ ] **Tests** (self-host + sync cloud scenario).

**Livrables:**
- docker-compose.yml (backend + DB).
- Docs pour self-host simple (10 minutes setup).
- Export/import working.

---

## Phase 7: Polish & Release (2-3 semaines)

**Objectif:** MVP v1.0 production-ready.

### Tasks
- [ ] **Performance tuning** (app startup < 2s, queries < 100ms).
- [ ] **UI/UX review** (with real DnD players).
- [ ] **Localisation** (French UI at least, extensible).
- [ ] **Mobile app store prep** (Google Play signing, metadata).
- [ ] **Analytics** (crash reporting, usage patterns).
- [ ] **Documentation** (user guide, FAQ, troubleshooting).
- [ ] **Beta testing** (closed group: 20-50 testers).
- [ ] **Bug fixes** (target < 5 critical bugs for v1.0).

**Livrables:**
- v1.0 release candidate.
- Available on Google Play, iOS App Store (or APK + web).
- User documentation published.

---

## Timeline Estimée (Best Case)

| Phase | Duration | Deadline |
|-------|----------|----------|
| 1. Spike Technique | 1-2 sem | Late June 2026 |
| 2. Backend Foundation | 2-3 sem | Mid July 2026 |
| 3. Client Core | 3-4 sem | Mid August 2026 |
| 4. RuleSet DnD 3.5 | 2-3 sem | Early September 2026 |
| 5. Premium Features | 2-3 sem | Late September 2026 |
| 6. Self-Host | 1-2 sem | Early October 2026 |
| 7. Polish & Release | 2-3 sem | Late October 2026 |
| **TOTAL** | **15-20 sem** | **v1.0 ~ End Oct/Nov 2026** |

---

## Dépendances entre phases

```
Spike (validation) → Phase 2 (Backend) ↘
                                         → Phase 3 (Client) → Phase 4 (Rules)
                                                          ↓
                                         Phase 5 (Premium) → Phase 7 (Release)
                                                          ↑
                                         Phase 6 (Self-host)
```

**Chemin critique:** Spike → Backend → Client → Rules → Premium → Release  
(Core frontend/backend work blocks final premium + release decisions)

---

## Staffing Recommendation

Taille équipe minimale:

- **1x Backend Lead** (.NET) — 80% Phase 2-6, 20% architecture decisions.
- **1x Frontend Lead** (Flutter) — 80% Phase 1, 3-4, 20% architecture + spike.
- **1x QA/Testing** — 20% Phases 1-7, full-time from Phase 5+.
- **1x DevOps/Infra** (optional) — 10% Phase 1, 30% Phase 2, 60% Phase 6+.
- **1x Product Owner/Designer** (optional) — 20% Phases 1-3, 80% Phase 7.

Possible solo (1x full-stack dev): tout c'est possible mais durée x1.5 → v1.0 late January 2027.

---

## Risques majeurs & mitigations

| Risque | Probabilité | Impact | Mitigation |
|--------|-------------|--------|-----------|
| Drift multi-plateforme non stable (Web) | Medium | High | Spike-01 early validation |
| OpenAPI gen poor quality | Low | Medium | Spike-02 + fallback manual mapping |
| Sync conflicts more complex than LWW | Medium | High | Spike-03 thorough testing |
| DnD 3.5 rules complexity underestimated | High | High | Early rule validation, modular implementation |
| Self-host adoption lower than expected | Medium | Low | Post-v1.0 concern, postpone if needed |
| Patreon integration delays | Low | Medium | Fallback to manual entitlements |

---

## Success Criteria v1.0

- [ ] Offline CRUD works (mobile + web).
- [ ] Sync robust (< 1% error rate over 1000 ops).
- [ ] D&D 3.5 character sheet complete + calculations accurate.
- [ ] 100+ beta users on-boarded, < 10 critical bugs.
- [ ] Performance: app load < 2s, sync < 500ms.
- [ ] Security: auth working, no data leaks.
- [ ] Retention: 30-day active > 40%.

---

## Post-v1.0 Roadmap (Outline)

### v1.1 (November 2026)
- Pathfinder RuleSet (plugin reuse).
- UI improvements (beta feedback).
- Cloud backup (auto-save).

### v1.2 (December 2026 - Jan 2027)
- D&D 5e RuleSet.
- Collaborative real-time editing (MJ + Player simultaneous).
- Mobile app store submissions (iOS + Android).

### v2.0 (Q2 2027)
- VTT lite integration (simple map).
- Marketplace content (community-created rulesets).
- Audio/video chat (in-app).

---

## Décisions à prendre avant Phase 1

1. **Équipe**: effectifs alloués? Lead technique choisi?
2. **Budget**: infrastructure, tools, services?
3. **Plateforme mobile prioritaire**: iOS ou Android d'abord, ou parity?
4. **Date cible v1.0**: end Oct 2026 (ci-dessus) ou plus long?
5. **Patreon account**: créé maintenant ou phase 5?

---

## Contacts & Ownership

(À remplir: qui est le tech lead, qui reporte à qui, qui arbitre les décisions)

- **Product Lead**: TBD
- **Architecture Lead**: TBD
- **Backend Lead**: TBD
- **Frontend Lead**: TBD
- **DevOps/Infra Lead**: TBD

---

**Status:** READY FOR PHASE 1 KICKOFF ✅

Pour démarrer:
1. Assigner ownership à chaque spike.
2. Créer branches Git feature/spike-01, feature/spike-02, feature/spike-03.
3. First sync: 1 semaine (vendredi), checkpoint spikes.
