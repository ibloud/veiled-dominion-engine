# Architecture Overview

This document provides the foundational technical architecture blueprints for Veiled Dominion's runtime pipeline and component architecture. It's intended for systems engineers, technical artists, and variant designers integrating new piece behaviors or networking backends.

1. Core dynamic move calculation loop (high-level)

- Input stage
  - Player actions (local/remote): move intent, special ability triggers, veil toggles
  - Game state snapshot: board occupancy, piece metadata, active constraints
  - External hooks: simulation/AI suggestions, network-delivered events

- Processing stage (deterministic pipeline)
  - Validate intent: ensure legality and turn/order constraints
  - Compute candidate moves: apply piece movement rules, USA (unified spatial algebra) transforms for cross-topology movement
  - Conflict resolution: simultaneous interactions resolved via deterministic tie-break (timestamp + player priority) or speculative execution with rollback
  - Scoring & restraint evaluation: calculate restraint deltas, update scoring ledger
  - Event emission: produce canonical events for UI and networking (MoveApplied, CaptureResolved, VeilChanged, RestraintCheckpoint)

- Output stage
  - Commit to authoritative state
  - Broadcast events to UI/clients
  - Persist snapshots (for replay / debugging)

2. Extensible component blueprint (example: GhostPiece)

- Patterns
  - Composition over inheritance: small behavior components (MovementComponent, VisibilityComponent, InteractionComponent)
  - Data-driven rules: JSON/YAML rule tables for movement vectors, costs, and special flags
  - Scriptable hooks: expose lifecycle events (OnValidateIntent, OnApplyMove, OnResolveCapture)

- GhostPiece example (pseudocode)

```csharp
// GhostPiece.cs (Unity-like pseudocode)
using System.Collections.Generic;

public class GhostPiece : PieceComponent {
    public float veilDuration = 2.5f;
    public MovementPattern movementPattern;

    public override IEnumerable<Move> GetCandidateMoves(BoardState state) {
        // movement pattern respects cross-topology transformations
        return MovementEngine.ApplyPattern(state, this.movementPattern);
    }

    public override void OnApplyMove(Move move, GameState state) {
        // apply veil on successful non-capture move
        if (!move.IsCapture) {
            state.ScheduleEffect(new VeilEffect(this.Owner, veilDuration, this));
        }
    }
}
```

Notes
- Keep pure rule evaluation (MovementEngine, ScoringEngine) agnostic of rendering and networking.
- Components should be serializable and easily editable by designers.

3. Integration strategy (atproto / Improbable)

- Goals
  - Decouple authoritative simulation from client-side UI
  - Use atproto-like activity streams or Improbable-style authoritative worlds depending on scale & hosting

- Hook points
  - Authority adapter: layer translating local GameState commits into platform-specific RPCs/events
  - Event bus: canonical event format emitted after every committed step, consumed by network adapters and analytics
  - Snapshot & replay store: persist state diffs for debug and audit

- Deployment paths
  - Small-scale: host authoritative instance as a serverless process, use atproto feeds for player intents and spectating
  - Large-scale / persistent worlds: Improbable/GDK for spatial partitioning and authoritative workers

4. Testing & verification

- Determinism tests: replay random seeds across server/client implementations
- Fuzz testing: mutate move sequences and assert invariants (no negative pieces, consistent scoring)
- Integration smoke: verify authority adapter roundtrip and event bus delivery

5. Designer checklist before integrating new component
- Create JSON rule table for movement & interactions
- Write unit tests for MovementEngine and Scoring deltas
- Add visual debug overlay showing candidate moves and veil regions
- Document behavior in `docs/ARCHITECTURE_OVERVIEW.md` and link to `docs/Rules.md`
