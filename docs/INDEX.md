# Documentation — Index et structure

## Objectif

Ce dossier `docs/` est le "second cerveau" du projet Companion JDR. Il contient toutes les décisions, architecture, et spécifications nécessaires pour comprendre et contribuer au projet.

Structure "source of truth" : ce qui est ici est prioritaire sur les commentaires de code.

---

## Structure du dossier

```
docs/
├── README.md                          ← Vous êtes ici
├── specs.md                           Vision produit + périmètre + modèle business
├── architecture-technique-v1.1.md     Architecture détaillée + modèles Dart + API
├── adr/
│   ├── 001-flutter-over-net-maui.md          ADR: Flutter vs .NET MAUI
│   ├── 002-offline-first-lww.md              ADR: Offline-first + Last-Write-Wins
│   └── 003-multi-ruleset-plugin.md           ADR: Extensibilité multi-systèmes
├── spike/
│   ├── spike-01-drift-sync.md         Test offline persistence + sync
│   ├── spike-02-openapi-generation.md Test generation client Dart
│   └── spike-03-conflict-resolution.md Test LWW + UI résolution
├── getting-started/
│   ├── setup-dev-env.md               Installation Flutter, .NET, Docker
│   ├── first-character.md             Tutoriel: créer votre première fiche
│   └── architecture-overview.md       Overview high-level pour nouveaux venus
└── howto/
    ├── add-ruleset.md                 Comment ajouter un nouveau système (ex: Pathfinder)
    ├── modify-data-model.md           Comment étendre le modèle (champs) sans casser sync
    ├── deploy-backend.md              Déployer le backend .NET
    └── self-host.md                   Comment self-hoster ses données
```

---

## Guides par profil

### Je suis contributeur backend (.NET)

1. Lire [specs.md](specs.md) (sections 1-5, focus périmètre + API).
2. Lire [architecture-technique-v1.1.md](architecture-technique-v1.1.md) (sections 5.1-5.2, focus OpenAPI et endpoints).
3. Consulter [ADR-002](adr/002-offline-first-lww.md) pour comprendre sync et conflits.
4. Lancer setup: voir [setup-dev-env.md](getting-started/setup-dev-env.md).

### Je suis contributeur client (Flutter)

1. Lire [specs.md](specs.md) (full).
2. Lire [architecture-technique-v1.1.md](architecture-technique-v1.1.md) (full).
3. Consulter [ADR-001](adr/001-flutter-over-net-maui.md) pour justification stack.
4. Lancer [spike-01-drift-sync.md](spike/spike-01-drift-sync.md) pour valider setup.

### Je veux ajouter un nouveau système de règles (ex: Pathfinder)

1. Consulter [ADR-003](adr/003-multi-ruleset-plugin.md) (architecture plugin).
2. Lire [howto/add-ruleset.md](howto/add-ruleset.md) (guide étape par étape).
3. Copier template DnD 3.5, adapter imports Pathfinder.

### Je suis testeur / QA

1. Lire [specs.md](specs.md) (focus PMV et use cases).
2. Consulter [spike-03-conflict-resolution.md](spike/spike-03-conflict-resolution.md) (scénarios de test).

---

## Hiérarchie de lecture recommandée (nouveau projet)

**Jour 1 — Compréhension produit (30 min)**
1. [specs.md](specs.md) — vision, périmètre, utilisateurs.

**Jour 2 — Compréhension technique (1-2 heures)**
2. [architecture-technique-v1.1.md](architecture-technique-v1.1.md) — layers, modèles, API, sync.
3. ADRs (3 x 15 min) pour justifications.

**Jour 3 — Mise en place (2 heures)**
4. [setup-dev-env.md](getting-started/setup-dev-env.md) — installer outils.
5. [first-character.md](getting-started/first-character.md) — test complet end-to-end.

**Jour 4+ — Contribution**
6. ADR ou spike pertinent pour le sujet de contribution.
7. Howtos si modifications schema/ruleset.

---

## Checklists par activité

### Feature nouvelle (ex: Dice Roller premium)

- [ ] Créer ADR-NNN dans `docs/adr/` (contexte, décision, conséquences).
- [ ] Valider impact sur specs (ajouter ou modifier section relevant).
- [ ] Valider impact sur modèle Dart (ajouter entités si besoin).
- [ ] Valider impact sur API (ajouter endpoints si besoin).
- [ ] Créer spike ou prototype dans `docs/spike/` si complexité.
- [ ] Implémenter code.

### Bug ou divergence

- [ ] Créer issue avec diagnostic.
- [ ] Identifier si bug specs ou code.
- [ ] Si specs incorrectes: créer ADR "correction", mettre à jour specs.
- [ ] Si code incorrect: fixer + add test.

### Modification schéma DB

- [ ] Consulter [howto/modify-data-model.md](howto/modify-data-model.md).
- [ ] Valider impact sync (champs meta, conflits).
- [ ] Versioner migration Drift.
- [ ] Ajouter test de migration si pertinent.

---

## Définitions et termes clés

**PMV (Produit Minimum Viable)**
: Version 1 avec fonctionnalités fondamentales gratuites. Voir [specs.md](specs.md) section 3.

**Offline-first**
: App fonctionne entièrement en local (SQLite), sync serveur est optionnel. Voir [ADR-002](adr/002-offline-first-lww.md).

**Last-Write-Wins (LWW)**
: En cas de conflit, modification la plus récente remplace l'autre. Voir [ADR-002](adr/002-offline-first-lww.md).

**RuleSet**
: Implémentation des règles d'un système JDR (ex: DnD 3.5, Pathfinder). Voir [ADR-003](adr/003-multi-ruleset-plugin.md).

**Drift**
: ORM Dart pour SQLite, génération de code type-safe.

**OpenAPI / Swagger**
: Spécification contracts API backend. Source of truth pour endpoint generation.

**Riverpod**
: State management Flutter, dépendance injection, providers.

**DTO (Data Transfer Object)**
: Modèle transport API, mappé vers Domain model en local.

---

## Maintenance docs

- **Synchro code ↔ docs**: Les docs doivent refleter réalité du code. En cas de divergence, corriger docs d'abord, puis code.
- **Versioning docs**: Chaque ADR est immutable (jamais edité après accept, créer nouveau ADR si changement).
- **Archivage**: Les versions superseded des spécs/architecture sont archivées dans `docs/archive/` (non supprimées).
- **Reviews**: tout changement dans `docs/` doit être reviewé (même simplicité syntaxe).

---

## Questions fréquentes

**Q: J'ai trouvé une incohérence entre specs et code. Qui a raison?**  
R: Les specs ont priorité. Le code est la source incorrecte et doit être corrigé. Ouvrir issue + PR pour ajuster.

**Q: Comment proposer une architecture alternative?**  
R: Créer issue + ADR dans `docs/adr/` avec contexte, alternatives rejetées, et justification. Demander feedback avant implémentation.

**Q: Peut-on modifier une ADR après acceptance?**  
R: Non. Une ADR est immutable, documenting la décision à un moment donné. Si contexte change, créer nouvelle ADR et référencer l'ancienne.

**Q: Je dois ajouter Pathfinder, par où commencer?**  
R: 1) Lire [ADR-003](adr/003-multi-ruleset-plugin.md), 2) Lire [howto/add-ruleset.md](howto/add-ruleset.md), 3) Copier dossier `lib/rule_sets/dnd35_ruleset/` en `pathfinder_ruleset/`, adapter.

---

## Status du projet

- Phase: **Definition & Spike** (Specs + ADRs + Architecture technique)
- Architecture client: **Flutter** (décision prise, voir [ADR-001](adr/001-flutter-over-net-maui.md))
- Sync strategy: **Offline-first + LWW** (décision prise, voir [ADR-002](adr/002-offline-first-lww.md))
- RuleSet extensibility: **Plugin pattern** (décision prise, voir [ADR-003](adr/003-multi-ruleset-plugin.md))

Prochaine étape: Spike technique (validation offline persistence, sync, conflicts).

---

## Contributeurs and contacts

(À compléter: liste de mainteneurs par domaine, points de contact)

- **Architecture & Décisions**: (lead architect)
- **Backend .NET**: (lead backend)
- **Client Flutter**: (lead frontend)
- **RuleSet DnD 3.5**: (rule designer)
