Here is the cleaned-up, public-facing version of the Pennywise variant, formatted specifically for your GitHub repository. 

I’ve stripped out the "Internal Only" warnings and reframed it to appeal directly to the engineers and designers reading the repo by highlighting *why* this variant is a critical stress-test for the C# engine.

Save this as `docs/variants/BACK_IN_DERRY_VARIANT.md` in your repo.

***

```markdown
# Variant: Back in Derry (Horror-Core Edition)

## 1.0 Concept Overview
A brutal, aggressive reskin of the Veiled Dominion engine mapped to the chaotic, cyclical terror of a predatory entity. This variant shifts the design philosophy from "restraint" to "predation." 

**Why this variant exists in the repo:** While the core engine handles "debuffs" (Veiling) perfectly, the *Back in Derry* variant introduces a new systemic requirement: **Forced Movement / Lure Mechanics**. It serves as a proof-of-concept to test if our `BasePiece` logic can be overridden by external board states (The Red Balloon).

## 2.0 Mechanic Translations (Systems Mapping)

*   **Rebirth ➔ The Eater (Pennywise)**
    *   Maintains standard Queen movement. The "Radius of Ruin" is reskinned as **"The Deadlights."** Mechanics remain identical: pieces caught in the 1-square radius are Paralyzed (lose special abilities, move like a Pawn).

*   **Death ➔ The Cistern (The Storm Drain)**
    *   Maintains standard King movement. "The Sanctuary" is reskinned as **"Down The Drain."** Mechanics remain identical: pieces within 1 square are immune to The Deadlights.

*   **Soul Reservoir ➔ The 27-Year Cycle**
    *   Mechanics remain identical. For every 3 pieces in the Graveyard, The Eater unlocks "The Wake" (Rebirth Dash - phasing through pieces).

*   **Martyr's Boon ➔ The Red Balloon (NEW SYSTEM REQUIREMENT)**
    *   *Base Logic:* You sacrifice a friendly piece (replacing it with a "Red Balloon" token).
    *   *Engine Challenge:* Any enemy piece within a 2-square radius of the Red Balloon token must have their `CalculateBaseMovement()` overridden on their next turn. Instead of calculating their standard moves, their valid moves are restricted *only* to squares that move them closer to the Balloon. 
    *   *Purpose:* Forces enemies to walk directly into The Eater's Deadlights. This tests the engine's ability to apply directional movement constraints based on an environmental object, rather than just a piece state.

*   **Mercy Victory ➔ The Ritual of Chüd**
    *   Mechanics remain identical. Winning via Leadership Points (maneuvering without capturing).

## 3.0 Visual Language (Grey-Box / Prototype Prep)

If this variant moves to the Unity prototype phase, the visual language shifts drastically from the core game to test high-contrast horror aesthetics.

*   **The Board:** Wet, dark grey concrete texture (high roughness, low specular). Grid lines should look like cracked chalk or yellowed caution tape.
*   **The Eater (Rebirth):** A hyper-reflective, porcelain-white material with a harsh, unshielded red emission (The Deadlights). No soft amber glow.
*   **The Cistern (Death):** Heavy, rusted iron material (dark orange/brown metallic). Complete matte finish.
*   **The Red Balloon (Token):** A bright, floating red sphere with a subtle bobbing animation and a glowing point light to draw the enemy's visual attention.

## 4.0 Engineering Takeaway
Do not build the Red Balloon mechanic until the core `VeiledStateManager` is 100% stable. Once stable, the Red Balloon is the perfect module to test expanding the `ApplyStateModifiers()` step in `BasePiece.cs` to accept external board vectors, proving the engine is truly modular.
```

***
