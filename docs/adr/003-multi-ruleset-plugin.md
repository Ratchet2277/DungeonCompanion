# ADR-003: Extensibilité multi-systèmes de règles via plugin architecture

**Date:** 2026-05-28  
**Status:** ACCEPTED  
**Context:** App doit supporter DnD 3.5 initialement, puis d'autres systèmes (Pathfinder, d20 Modern, etc.)

## Contexte

Le produit doit:
1. Cibler DnD 3.5 en V1 (règles, calculs, compétences, sorts, etc.).
2. Rester **suffisamment malléable** pour intégrer d'autres systèmes sans refonte architecturale majeure.
3. Éviter le piège de "hardcoder DnD 3.5 partout" → refonte majeure en V2.

Trois approches:
1. **Hardcoded DnD 3.5** dans le coeur (simple court-terme, coûteux long-terme).
2. **Plugin/RuleSet abstrait** (plus complexe initialement, extensible).
3. **Configuration-driven** (metadata heavy, YAML/JSON-based rules, moins type-safe).

## Décision

**Nous choisissons architecture plugin/RuleSet abstraite.**

Raison: **extensibilité long-terme + maintenabilité + séparation concerns + valeur éducative.**

## Justification

### Avantages plugin/RuleSet
1. **Découplage métier**: logique spécifique DnD 3.5 isolée, pas mélangée au coeur app.
2. **Évolutivité**: ajouter Pathfinder, D&D 5e, ou custom ruleset sans toucher domain core.
3. **Pédagogique**: apprentissage pattern plugin, architectures modulaires, inversion de contrôle.
4. **Maintenabilité**: bugfix ou feature DnD 3.5 contenu dans son module.
5. **Testabilité**: chaque ruleset testable indépendamment.
6. **Réutilisabilité**: un mod custom d'un utilisateur peut être partagé (futur).

### Avantages hardcoded (non choisi)
1. **Simple en V1**: moins de code boilerplate.
2. **Type-safe**: pas de reflection/generics.

### Avantages configuration-driven (non choisi)
1. **Très flexible**: pas de recompilation pour new system.
2. **Non-dev users** peuvent créer rulesets (future).

### Trade-offs acceptés
1. **Complexité architecturale augmentée**.
   - Mitigation: abstractions claires, bien documentées.

2. **Indirection supplémentaire** (performance negligeable pour app JDR).
   - Mitigation: cache calculations, pas de reflection en critical path.

3. **Coût initial plus haut** que simple hardcoded.
   - ROI: si roadmap inclut d'autres systèmes (c'est le cas ici).

## Architecture proposée

### 1. Core domain (système-agnostique)
```
Domain/
├── Character.dart          (entité persistée, fields génériques)
├── RuleSet.dart            (interface: ce qu'un system doit implémenter)
├── CharacterCalculator.dart (demande au ruleset les calculs)
├── Spell.dart              (entity générique avec references)
└── Skill.dart              (entity générique avec metadonnées)
```

### 2. RuleSet Plugin
```
RuleSets/
├── DnD35RuleSet/
│   ├── DnD35RuleSet.dart        (implémente RuleSet interface)
│   ├── DnD35StatCalculator.dart (bonus attaque, AC, saves, etc.)
│   ├── DnD35SkillCalc.dart      (skills liés aux stats)
│   ├── DnD35Classes/
│   │   ├── Fighter.dart
│   │   ├── Wizard.dart
│   │   └── Cleric.dart
│   ├── DnD35Spells/
│   │   └── [data: 1000+ spells]
│   └── DnD35Skills/
│       └── [data: 35 skills standard]
├── PathfinderRuleSet/         (futur, même architecture)
└── D20ModernRuleSet/          (futur, même architecture)
```

### 3. RuleSet Interface (contrat)
```dart
abstract class RuleSet {
  String get id;                                    // "dnd-3.5"
  String get name;                                  // "Dungeons & Dragons 3.5"
  
  // Capabilities
  List<RuleClass> getAvailableClasses();
  List<RuleSkill> getAvailableSkills();
  List<RuleSpell> getAvailableSpells();
  
  // Calculations
  int calculateAttackBonus(Character char, Spell spell);
  int calculateSaveDC(Character char, Spell spell);
  List<String> validateCharacter(Character char);   // règles intégrité
  
  // Features
  bool supportsMulticlass();
  bool supportsPrestige();
  bool supportsPreparedSpells();
}
```

### 4. Injection au démarrage
```dart
// main.dart ou provider global
final ruleSetProvider = Provider<RuleSet>((ref) {
  final campaign = ref.watch(campaignProvider);
  switch (campaign.ruleSetId) {
    case 'dnd-3.5':
      return DnD35RuleSet();
    case 'pathfinder':
      return PathfinderRuleSet();
    default:
      throw Exception('Unknown ruleset: ${campaign.ruleSetId}');
  }
});
```

### 5. Utilisation dans domain
```dart
// Character calculation
final attackBonus = ruleSet.calculateAttackBonus(character, spell);
// Pattern: jamais d'if(ruleSetId == "dnd-3.5") directement en domain
```

## Impact données

### Character storage
```dart
Character {
  id, name, level, ...
  // Champs DnD 3.5 génériques (STR, DEX, etc.)
  attributes: Map<String, int>,
  // Champs system-specific via JSON extensible
  systemData: Map<String, dynamic>,  // ex: { "dnd35_prestige_class": "..."} 
}
```

### Migration entre ruleset
- Complexe: pas de conversion auto (D&D 5e a 6 stats, D&D 3.5 a 6 stats, mais calculs différents).
- Acceptable: rarement le cas en pratique (campagne "locked" sur un système).

## Cas de test requis

1. **DnD 3.5 calc**: level 1 Fighter → calcul attaque correct. ✓
2. **Spell validation**: sort 9e level sur wizard lvl 5 → error. ✓
3. **Multiclass rules**: Fighter/Wizard, gestion exp et level progression. ✓
4. **Character export**: export DnD 3.5 character, injecté dans autre app (format agnostique). ✓

## Roadmap
- **V1**: DnD 3.5 complet dans plugin, interface RuleSet verrouillée.
- **V2**: Pathfinder plugin, validation abstraction.
- **V3**: D&D 5e plugin, optimisations reflection/caching.
- **V4+**: utilisateurs créent custom rulesets via config YAML (optionnel).

## Références
- Strategy Pattern: https://refactoring.guru/design-patterns/strategy
- Plugin Architecture (Martin Fowler): https://martinfowler.com/articles/plugins.html
- Riverpod providers: https://riverpod.dev/
