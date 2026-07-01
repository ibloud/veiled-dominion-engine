# ♟️ Veiled Dominion

> *"Every game teaches you to become more powerful. What if the lesson was the opposite?"*

**Veiled Dominion** is an asymmetrical, 4-player chess variant serving as the mechanical and narrative anchor of the **Daddy's Little Mortis (DLM)** universe. It reimagines the traditional board game not as an arena of conquest, but as an exercise in structural restraint.

One player assumes control of **Rebirth**—an entity of immense, terrifying power who must win by learning mechanical restraint. The remaining three players command standard **Mortal Factions** attempting to survive her proximity, exploit her tactical inexperience, or achieve checkmate before she masters her aura.

---

## 🌑 The Core Thesis: The Restraint Fantasy

Traditional games follow a standard power escalation loop:


$$\text{Start Weak} \longrightarrow \text{Gain Power} \longrightarrow \text{Win by Dominance}$$

**Veiled Dominion** flips this paradigm into a **Restraint Fantasy**:


$$\text{Start Overpowered} \longrightarrow \text{Suppress Power} \longrightarrow \text{Win by Coexistence}$$

The signature mechanical engine is the **Radius of Ruin**—an area-of-effect (AoE) debuff field constantly projecting from the Rebirth piece. She does not merely threaten enemies; she inadvertently suppresses her own units. Victory is achieved not through aggressive capture, but through **Merciful Maneuvers**.

---

## 📜 The Official Playtest Rulebook (v0.1)

### 1. Setup & Components

* **The Board:** A 14x14 cross-shaped 4-player topology featuring a $2\times2$ neutral zone at the absolute center.
* **Player Count:** 4 Players (Asymmetrical Free-For-All by default).
* **Factions & Army Composition:**
* **3x Mortal Armies:** Standard chess loadouts (1 Leader, 1 Queen, 2 Rooks, 2 Bishops, 2 Knights, 8 Pawns).
* **1x Rebirth Army:** Modified loadout consisting of:
* `1x Leader` (Standard King logic)
* `1x Rebirth` (Replaces standard Queen)
* `1x Death` (Replaces left-side Rook)
* `1x Rook` / `2x Bishops` / `2x Knights` / `8x Pawns`




* **Resources:** Boon Tokens (Rebirth exclusive), Leadership Point (LP) Tracker.

### 2. Unique Piece Logic

#### `REBIRTH` (The Daughter)

* **Locomotion:** Infinite horizontal, vertical, and diagonal movement (Standard Queen pathing).
* **Capture:** Displacement rules.
* **Passive — Radius of Ruin:** Continuously emits a 1-square AoE aura (all 8 adjacent tiles). Any piece (friendly or enemy, excluding *Death* and *Rebirth*) that ends its turn inside this aura enters the **Veiled State**.

#### `DEATH` (The Mentor)

* **Locomotion:** 1 square in any direction (Standard King pathing).
* **Passive — The Void:** Invulnerable. Cannot be captured. Enemy movement into Death's coordinate is registered as an illegal move (identical to moving into check). Death may freely move into attacked squares.
* **Passive — Passable:** Friendly units can move *through* Death's coordinate, but cannot end their movement phase on his tile.
* **Passive — Sanctuary:** Friendly units situated within 1 square of Death are completely immune to the *Radius of Ruin*.

#### `THE VEILED STATE` (System Debuff)

* **Trigger:** Ending a turn within Rebirth’s 1-square radius.
* **Duration:** 1 game round (cleared instantly at the start of the affected piece-owner's next turn).
* **Effects:** The affected unit loses all unique class movement, can only move 1 tile forward, and is strictly prohibited from capturing enemy pieces.

---

### 3. Rebirth Player Abilities

| Ability | Type | Resource Cost / Trigger | Mechanical Effect |
| --- | --- | --- | --- |
| **Martyr’s Boon** | Active Action | **Cost:** Consumes Turn <br>

<br>**Target:** 1 Friendly Unit | Permanently sacrifices a friendly piece to the Graveyard to generate **1 Boon Token**. |
| **Aura Suppression** | Active Action | **Cost:** 1 Boon Token <br>

<br>**Trigger:** Turn Start | Disables the *Radius of Ruin* completely for the duration of the current turn. |
| **Soul Reservoir** | Passive | **Trigger:** Passive threshold at every 3 units in Graveyard (3, 6, 9...) | Unlocks **Rebirth Dash**. Rebirth can phase through occupied tiles as if empty. Passed units are unaffected by the *Radius of Ruin*. |

---

### 4. Turn Loop Architecture

Play transitions sequentially clockwise. Each player's turn executes across three distinct phases:

```text
[PHASE 1: START PHASE]
  └── Rebirth declares Aura Suppression (Optional Token Spend)
  └── Clear "Veiled" status from the active player's pieces
            │
            ▼
[PHASE 2: ACTION PHASE]
  └── Execute one standard piece movement OR cast Martyr's Boon
            │
            ▼
[PHASE 3: RESOLUTION PHASE]
  └── Evaluate Radius of Ruin spatial coordinates
  └── Apply "Veiled" states to valid adjacent targets
  └── Check End-State / Victory Conditions

```

---

### 5. Victory Conditions (End States)

#### Path A: Standard Checkmate (Mortal Victory)

* If any player's **Leader** is checkmated, that player is eliminated; their remaining units turn into immovable, capturable neutral obstacles.
* If **Rebirth's Leader** is checkmated, the Mortal Factions share a collective victory.

#### Path B: Leadership & Mercy (Rebirth Victory)

* Rebirth wins immediately upon accumulating **10 Leadership Points (LP)**.
* *LP Acquisition (+1 per action; maximum of 1 Coexistence LP per turn loop):*
* **Withdrawal:** Moving your unit out of an active enemy threat vector without capturing.
* **Shielding:** Interposing a unit directly between an enemy attack line and your Leader or Rebirth piece.
* **Coexistence:** Settling a movement phase on a tile directly adjacent to an enemy unit without capturing it.



#### Path C: The Fall (Mortal Structural Victory)

* If Rebirth accidentally applies the **Veiled State** to **5 of her own friendly units** over the course of a match, she loses systemic control. The Mortal Factions win collectively.

---

### 6. Edge Cases & Engine Logic

* **Capturing the Veiled:** While Veiled units cannot capture other pieces, they remain fully targetable and capturable by standard rules.
* **Stacking Rules:** The Veiled state does not stack linearly. Re-entering or remaining within the *Radius of Ruin* simply refreshes the 1-turn duration.
* **Collision Detection:** Rebirth's *Soul Reservoir* dash evaluates spatial proximity and triggers the *Radius of Ruin* **only** at her final landing destination tile, not on the units phased through.

---

## 🛠️ Repository Structure & Tech Stack

This repository serves as the digital prototyping sandbox, system state-machine engine, and foundational transmedia manifest for the *Daddy's Little Mortis* ecosystem.

```text
veiled-dominion/
├── architecture/                 # Physical & Industrial blueprints
│   └── apparel/
│       └── source-code/
│           └── veil-protocol-v0.1.md # SENSORY ARCHITECTURE / FOUNDRY SPECIFICATIONS
├── assets/                       # Engine-ready production files
│   ├── audio/                    # Ambient drones & state-change audio cues
│   └── models/                   # 3D assets (Refractive Rebirth / Void-Shader Death)
├── docs/                         # Extended universe documentation
│   ├── open-collective/          # Crowdfunding tiers & backer materials
│   ├── PITCH_DECK.md             # Premium television packaging outlines
│   └── QUEENS_JOURNEY.md         # Narrative worldbuilding & core thematic logic
└── src/                          # Unity (C#) / Unreal (C++) Prototyping Core
    ├── board/                    # 14x14 cross-grid coordinate systems
    ├── input/                    # Phase management controllers & UI hooks
    ├── pieces/                   # Polymorphic base piece classes & locomotion components
    └── systems/                  # RadiusofRuin.cs, State Machine, and LP tracking engines

```

---

## 🤝 Open Seats (How to Contribute)

We are hunting for systems-driven architecture thinkers who understand that constraints drive complex gameplay. If you have deep experience building deterministic grid logic, spatial math, or multi-agent state machines, step up.

### Immediate Focus Areas:

1. **Prototype Engineer:** Implementation of the 14x14 cross-grid coordinate transformations and the `RadiusOfRuin` spatial check scripts.
2. **Technical Artist:** Shader pipeline engineering for **Death** (100% light-absorbent, zero-sheen matte void shader) and **Rebirth** (dynamic internal refraction glow).
3. **Systems Designer:** Mathematical balancing of the "Martyr's Boon" economy curve and 4-player asymmetrical LP scaling.

### Contribution Guidelines

1. Fork this repository.
2. Spin up an explicit feature branch (`git checkout -b feature/radius-logic`).
3. Write clean, heavily commented, deterministic code. **Crucial:** If your code alters game-loop mechanics, update *this* rules section in the `README.md` within the same commit.
4. Open a PR detailing exactly how your code hooks into the core *Turn Structure Loop*.

---

## 📬 Contact

Direct pull requests, design manifestos, or structural inquiries to:

**questions@loptrlab.com** *(Include links to your portfolio or repository footprints. We respect the craft.)*

---

*© 2026 Daddy's Little Mortis. The universe is a delicate cycle.*
