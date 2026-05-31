# Spike-03: Conflict Resolution (LWW + Manual UI)

**Duration estimate:** 2.5 days  
**Owner:** TBD (Flutter lead + Backend liaison)  
**Status:** Not started  

## Objectif

Valider que stratégie LWW (Last-Write-Wins) + manual UI pour champs critiques fonctionnent en pratique. Spécifiquement:

1. Simuler offline modifications sur 2 appareils simultanément.
2. Implémenter merge logic serveur (timestamp comparison).
3. Implémenter conflict detection client.
4. Implémenter UI resolution pour champs critiques.
5. Valider cas edge: connectivity intermittente, desync long terme, multiple conflicts en cascade.

## Questions à résoudre

1. **Timestamp strategy**: 
   - UTC server-side source of truth?
   - Client clock skew implications?
   - Monotonic clock library needed?

2. **Conflict resolution rules**:
   - Non-critical: auto LWW (serveur gagne).
   - Critical: manual UI (user chooses).
   - Definition: qui décide si champ critique? Config static ou dynamic per entity?

3. **UI/UX conflict flow**:
   - When user sees conflict screen?
   - Option 1: blocking (can't continue till resolved).
   - Option 2: non-blocking (preview, resolve later).
   - Which is better for JDR use case?

4. **Cascading conflicts**: 
   - If level changed + derived stats (HP, AC) also out-of-sync.
   - How to prevent cascading errors?
   - Automatic recalc after manual resolution?

5. **Conflict audit trail**: 
   - Log all conflicts (who, what, when, resolution)?
   - User-facing visibility or backend-only?

## Test Scenarios

### Scenario 1: Simple non-critical conflict (notes)
```
Device A (offline):
  Character.notes = "Archer specializes in fire arrows"
  timestamp_A = 2026-05-28T10:00:00Z

Device B (offline):
  Character.notes = "Archer specializes in lightning arrows"
  timestamp_B = 2026-05-28T10:00:05Z

Server merge (LWW):
  timestamp_B > timestamp_A → accept B's version
  All devices eventually converge to "lightning arrows"

Expected result:
  Device A after sync: notes = "lightning arrows"
  Device B after sync: notes = "lightning arrows" (already has it)
  UI: No conflict dialog (auto-resolved)
```

### Scenario 2: Critical conflict (level)
```
Device A (offline):
  Character.level = 5
  timestamp_A = 2026-05-28T10:00:00Z

Device B (offline):
  Character.level = 6
  timestamp_B = 2026-05-28T10:00:05Z

Server merge (LWW):
  timestamp_B > timestamp_A → B wins logically
  BUT level is critical field → mark as conflict

Device A after attempted sync:
  Server returns: { status: "conflict", field: "level", clientValue: 5, serverValue: 6 }
  UI shows: "Conflict! Your level: 5, Server has: 6. Which to keep?"
  User picks: "Keep my 5"
  Client re-sends: { op: "update", field: "level", value: 5, forceOverride: true }
  Server accepts (user's choice binding)

Device B after sync:
  Receives broadcast: "level is now 5 (user resolved)"
  Automatic sync to Device B local DB
```

### Scenario 3: Derived field auto-recalc
```
Character.level changes from 5 → 6

System should auto-recalc:
  - Attack Bonus (depends on level + class)
  - Hit Points (if formula includes level)
  - Skill bonuses (if level-dependent)

Test:
  1. User resolves level conflict manually
  2. UI immediately recalculates derived fields
  3. If derived also in conflict → must resolve too (cascade handling)
```

### Scenario 4: Offline 7 days, then sync
```
Device offline for 7 days (poor connectivity):
  - Local modifications: 50 operations queued
  - No contact with server

When connectivity restored:
  Server has evolved (other users' changes)
  
Sync should:
  1. Pull server state (get conflicts vs local queue)
  2. Apply LWW for non-critical
  3. Flag critical for UI resolution
  4. Replay successful ops, queue failed ones
  5. User sees: "50 changes, 3 require your attention" (critical conflicts only)

Expected result:
  - 47 auto-merged
  - 3 manual resolutions shown
  - User picks options
  - Final state converged across all devices
```

### Scenario 5: Cascading conflicts with rollback
```
User resolves level conflict → keeps local value 5
System auto-recalcs Attack Bonus = 2 (from level 5)
But server has Attack Bonus = 4 (from its level 6)

Result: Attack Bonus now in conflict too!

Solution options:
  A) Auto-derive from master field (level) → no extra conflict
  B) Mark derived as also conflicting → user resolves again
  
Decision for MVP:
  → Option A (auto-derive from level after level resolved)
```

## Implementation Tasks

### Client side (Flutter)

1. **Sync metadata model**:
   ```dart
   class SyncFieldMeta {
     DateTime lastModified;
     String sourceClientId;
     dynamic lastKnownValue;
     SyncConflictState conflictState;  // null | 'conflict_with_server'
   }
   ```

2. **Conflict detector**:
   ```dart
   List<FieldConflict> detectConflicts(
     Character local,
     Character server,
     List<SyncFieldMeta> metadata
   ) {
     // Compare local vs server for each field
     // If timestamps differ → conflict
   }
   ```

3. **UI widget**:
   ```dart
   ConflictResolutionDialog(
     field: "level",
     localValue: 5,
     serverValue: 6,
     onKeepLocal: () { /* save choice */ },
     onAcceptServer: () { /* save choice */ }
   )
   ```

### Server side (.NET)

1. **Merge endpoint**:
   ```csharp
   [HttpPost("/api/v1/sync")]
   public async Task<SyncResponse> Sync(List<SyncOperation> ops)
   {
     var results = new List<SyncOperationResult>();
     foreach (var op in ops)
     {
       var serverVersion = await db.GetLatest(op.EntityId, op.Field);
       
       if (serverVersion.Timestamp > op.Timestamp)
       {
         // Conflict: server newer
         results.Add(new SyncOperationResult
         {
           OpId = op.Id,
           Status = SyncResultStatus.Conflict,
           ServerValue = serverVersion.Value
         });
       }
       else
       {
         // OK: client newer, accept
         await db.Update(op.EntityId, op.Field, op.Value, op.Timestamp);
         results.Add(new SyncOperationResult { OpId = op.Id, Status = SyncResultStatus.Synced });
       }
     }
     return new SyncResponse { Results = results };
   }
   ```

2. **Conflict resolution endpoint**:
   ```csharp
   [HttpPost("/api/v1/sync/resolve-conflict")]
   public async Task ResolveConflict(ConflictResolution resolution)
   {
     // User's choice override LWW
     await db.Update(
       resolution.EntityId,
       resolution.Field,
       resolution.ChosenValue,
       DateTime.UtcNow
     );
     
     // Broadcast to other clients
     await NotifyOtherClients(resolution.EntityId);
   }
   ```

## Livrables

1. **Prototype conflict detection** (client + server).
2. **UI resolution widget** (Flutter).
3. **Test scenarios** (all 5 cases above pass).
4. **Document**: "Conflict Resolution Guide for Contributors".

## Risques identifiés

1. **Cascading conflicts nightmare**: if resolution of field A creates conflicts in B, C, D...
   → Mitigation: auto-derive strategy for dependent fields (see scenario 5).

2. **User UX confusion**: if conflict dialog too frequent or unclear.
   → Mitigation: good UX design + testing with real users.

3. **Data loss perception**: if LWW silently overwrites (non-critical).
   → Mitigation: audit log visible to user (transparency).

4. **Performance**: if conflict detection on 100+ fields → slow sync.
   → Mitigation: only track delta (changed fields), not full entity comparison.

## Critères de succès

- [ ] Scenario 1 (non-critical auto-resolve): pass.
- [ ] Scenario 2 (critical manual resolve): pass, UI works.
- [ ] Scenario 3 (derived field recalc): pass, cascades handled.
- [ ] Scenario 4 (long offline sync): pass, prioritizes conflicts.
- [ ] Scenario 5 (cascading conflicts): pass, auto-derive prevents repeats.
- [ ] Integration test (E2E offline → sync → conflict): pass.
- [ ] UI UX review (conflict dialog clear): pass.
- [ ] Audit log records all conflicts + resolutions.
