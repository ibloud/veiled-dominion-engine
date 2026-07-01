Thanks — a few actionable requests to make this implementable by engine teams:

- Please add a short lifecycle diagram or an ordered sequence showing: PlayerIntent → OnValidateIntent → OnApplyMove → OnResolveCapture → Snapshot/Replay. A simple ASCII sequence or image placeholder is fine.
- Include a minimal example of the GhostPiece data model (serialized fields) and the lifecycle callbacks expected from engine code so implementers know what to wire up. For example, a Unity-style C# sketch:

```csharp
[Serializable]
public class GhostPiece {
  public string id;
  public Vector2Int position;
  public MovementPattern baseMovement;
  public List<VariantModifier> variantModifiers;

  // lifecycle hooks called by the engine
  public void OnValidateIntent(Intent intent) { /* ... */ }
  public void OnApplyMove(Move m) { /* ... */ }
  public void OnResolveCapture(CaptureInfo ci) { /* ... */ }
}
```

- Add a short "Determinism & Snapshot" section specifying the canonical snapshot fields and where snapshots should be taken (e.g., before ApplyMove with RNG seed). Example required fields: timestamp, RNG seed, all piece states (id/pos/owner/metadata), and active player index.

Please add these as small sections or appendices (diagram placeholder + code sketch + a one-paragraph snapshot format) so engine implementers can proceed without needing design clarifications.
