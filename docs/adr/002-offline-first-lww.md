# ADR-002: Offline-first avec stratégie "last-write-wins" pour la résolution de conflits

**Date:** 2026-05-28  
**Status:** ACCEPTED  
**Context:** App compagnon JDR, utilisée en parallèle de VTT, connexion intermittente attendue.

## Contexte

Le projet doit supporter:
1. Fonctionnement totalement offline (sessions hors ligne).
2. Synchronisation fiable avec serveur central quand connexion disponible.
3. Résolution de conflits quand plusieurs appareils modifient le même champ.
4. Traces d'audit/journal pour debug et potentiel undo plus tard.

Deux stratégies principales:
1. **Last-Write-Wins (LWW)**: La modification la plus récente (timestamp) remplace la précédente.
2. **CRDT (Conflict-Free Replicated Data Types)**: structure de données résistante aux conflits par design.

## Décision

**Nous choisissons stratégie "last-write-wins" par champ + journal de modifications + résolution manuelle pour champs critiques.**

Raison: **pragmatisme V1 + coût d'implémentation acceptable + pédagogique.**

## Justification

### Avantages LWW
1. **Simple à implémenter** (timestamp comparison).
2. **Prédictible**: rule déterministe, pas d'ambiguïtés.
3. **Performance**: pas de structures de données complexes (CRDT coûteuses).
4. **Débuggable**: traces claires (qui, quand, quoi).
5. **Suffisant pour V1**: conflits simultanés rares en pratique pour app JDR.

### Cas d'usage JDR réels
- MJ édite description PNJ localement.
- Joueur édite ses stats localement.
- Même appareil probablement 95% du temps (pas de conflit).
- Conflits: surtout lors de re-synchronisation, rarement "vraiment concurrent".

### Avantages CRDT (non choisi)
1. Résolution automatique garantie (pas besoin de manuel).
2. Applicable à texte collaboratif (futures features).

### Trade-offs acceptés
1. **Champs critiques nécessitent résolution manuelle en cas de conflit**.
   - Exemple: si MJ et joueur ont modifié "level" simultanément, écran de fusion affiché.
   - Acceptable: cas rare, utilisateur comprend enjeu.

2. **Perte possible de modifications**.
   - Si deux edits simultanés, le plus ancien disparaît (par LWW).
   - Mitigation: journal local complet, potentiel undo futur.

3. **Pas d'undo global distribué**.
   - On garde l'historique localement, pas centralité serveur (contrairement CRDT).
   - Acceptable: contexte JDR (rarement besoin d'undo distant).

## Conséquences

### Positives
1. **V1 livrable rapidement** (pas infrastructure CRDT compliquée).
2. **UX claire** (pas d'opérations magiques invisibles).
3. **Pédagogique** (apprentissage sync patterns réalistes, pas framework "magique").
4. **Évolutif** (peut passer à CRDT en phase 2 si besoin).

### Négatives
1. **Perte de data en cas de conflit concurrent rare**.
   - Mitigation: UI avertit avant de "commit" offline (draft non sauvegardé).
2. **Champs critiques demandent UX de résolution** (complexité UI).
   - Acceptable: c'est le cas limite, pas la règle.

## Mécanismes d'implémentation

### 1. Versioning par champ
```
Character {
  id: String,
  name: String,
  level: int,
  // Métadonnées sync
  _updatedAt: DateTime,           // Dernière modification globale
  _changes: Map<String, Change>   // Par champ: { field: { value, timestamp, source } }
}
```

### 2. Sync queue et idempotence
- Client conserve liste d'opérations non synchronisées.
- Chaque op est atomique et idempotente (retry safe).
- Opération: `{ op: 'updateField', entityId, field, value, timestamp, clientId }`

### 3. Résolution au synchronisation
```
Côté serveur:
- Comparer timestamp client vs serveur.
- Si client plus récent: accepter, broadcast aux autres clients.
- Si client plus ancien: marquer comme conflict, renvoyer au client.

Côté client:
- Si conflict sur champ critique: afficher UI de fusion (voir vs local vs serveur).
- Si conflict sur champ non-critique: auto-accepter serveur (LWW).
```

### 4. Journal d'audit
- Toute modification enregistrée localement: `{ timestamp, field, oldValue, newValue, source }`
- Sert debug + base pour undo futur.

## Champs critiques (nécessitent résolution manuelle)
- Character.level
- Character.experience
- Character.maxHP
- Spell.prepared (pour mages)
- Tout champ affectant calculs ou rules engine.

## Champs non-critiques (auto-resolution LWW)
- Character.notes, description, flavor text.
- Character.proficiencies (non-dépendant des stats).
- Spell.notes, custom modifications.

## Cas de test requis

1. **Happy path**: modif offline + sync, pas de conflit. ✓
2. **Conflit champ non-critique**: deux clients modifient niveau décription → serveur gagne. ✓
3. **Conflit champ critique**: deux clients modifient level → UI résolution affichée. ✓
4. **Offline prolongé**: 10 jours sans sync, puis reconnexion → sync du journal complet. ✓
5. **Perte connexion mid-sync**: retry automatique sans corruption. ✓

## Évolutions futures
- Phase 2: ajouter CRDT pour collaborative editing (si MJ + Joueur éditent same field concurrent).
- Phase 3: undo distribué si business case valide.

## Références
- Last-Write-Wins pattern: https://en.wikipedia.org/wiki/Conflict-free_replicated_data_type
- Differential Sync (alternative): https://neil.fraser.name/writing/sync/
- Replicache (inspiration): https://replicache.dev/
