Copy the text in the block below and save it locally as `README.md`.

```markdown
# ♟️ Veiled Dominion

> *"Every game teaches you to become more powerful. What if the lesson was the opposite?"*

**Veiled Dominion** is a 4-player, asymmetrical chess variant and the core game inside the *Daddy's Little Mortis* universe. It reimagines the board not as a battlefield of conquest, but as a classroom for the cosmos. 

One player controls **Rebirth**—a piece of immense, terrifying power who must learn restraint. The other three control **Mortal Factions** trying to survive her radiance, exploit her inexperience, or checkmate her before she masters her aura.

This repository contains the digital prototype, systems architecture, and design documentation for the project.

---

## 🌑 The Core Thesis

Traditional games follow a power fantasy: *You start weak → You gain power → You win by dominance.*

Veiled Dominion introduces a **Restraint Fantasy**: *You start overpowered → You win by not using your power → The board is your victim, not your enemy.*

The signature mechanic, **The Radius of Ruin**, acts as an AoE debuff field around the Rebirth piece. She doesn't just threaten the enemy—she accidentally suppresses her own allies. The game is won not through aggressive capturing, but through "Merciful Maneuvers."

---

## 📜 The Official Playtest Rulebook (v0.1)

*This section serves as the absolute source of truth for all mechanical implementations.*

### 1. Setup & Components
*   **Board:** 14x14 cross-shaped 4-player grid (standard 4-player chess topology with a 2x2 neutral center).
*   **Players:** 4 (Asymmetrical Free-For-All by default).
*   **Factions:** 
    *   3x Mortal Armies (Standard chess sets: 1 Leader, 1 Queen, 2 Rooks, 2 Bishops, 2 Knights, 8 Pawns).
    *   1x Rebirth Army (1 Leader, 1 Rebirth [replaces Queen], 1 Death [replaces Rook], 1 Rook, 2 Bishops, 2 Knights, 8 Pawns).
*   **Resources:** Boon Tokens (Rebirth player only), Leadership Point (LP) Tracker.

### 2. Unique Piece Logic

**REBIRTH (The Daughter)**
*   **Locomotion:** Standard Queen movement (infinite horizontal, vertical, diagonal).
*   **Capture:** Displacement (Standard Queen capture rules).
*   **Passive - Radius of Ruin:** Emits a 1-square aura (8 adjacent squares). Any piece (friendly or enemy, excluding Death and Rebirth) ending its turn within this radius enters the **Veiled** state.

**DEATH (The Mentor)**
*   **Locomotion:** Standard King movement (1 square any direction).
*   **Passive - The Void:** Cannot be captured. Enemy movement into Death's square is treated as an illegal move (like moving into check). Death can move into attacked squares.
*   **Passive - Passable:** Friendly pieces can move *through* Death, but cannot end their turn on his square.
*   **Passive - Sanctuary:** Any friendly piece within 1 square of Death is immune to the Radius of Ruin.

**THE VEILED STATE (System Debuff)**
*   **Trigger:** Ending a turn within Rebirth's 1-square Radius.
*   **Duration:** Lasts exactly 1 full round (cleared at the start of the affected piece's owner's next turn).
*   **Effect:** Piece loses all special movement. Can only move 1 square forward. Cannot capture.

### 3. Player Abilities (Rebirth Only)

**Martyr’s Boon (Resource Generation)**
*   **Action:** On your turn, *instead of moving*, remove one friendly piece (excluding Rebirth, Death, Leader) to your Graveyard.
*   **Reward:** Gain 1 Boon Token.

**Aura Suppression (Resource Sink)**
*   **Action:** Spend 1 Boon Token at the *start* of your turn.
*   **Effect:** The Radius of Ruin is disabled for the duration of that turn.

**Soul Reservoir (Passive Upgrade)**
*   **Trigger:** For every 3 friendly pieces in your Graveyard (thresholds at 3, 6, 9, etc.).
*   **Effect:** Unlocks *Rebirth Dash*. Rebirth may move *through* occupied squares (friendly or enemy) as if they were empty. She still cannot end her movement on an occupied square. Pieces passed through are not affected by the Radius.

### 4. Turn Structure Loop
Play proceeds clockwise. Each turn consists of:
1.  **Start Phase:** Rebirth player declares Aura Suppression (if spending a token). Clear "Veiled" status from active player's pieces.
2.  **Action Phase:** Player makes one standard move OR casts Martyr's Boon.
3.  **Resolution Phase:** Evaluate Radius of Ruin. Apply "Veiled" state. Check Victory Conditions.

### 5. Victory Conditions (End States)

**Path A: Standard Checkmate**
*   If any player's Leader is checkmated, that player is eliminated. Their pieces become neutral obstacles (immovable, capturable). 
*   If Rebirth's Leader is checkmated, Mortals win collectively.

**Path B: Rebirth Wins (Leadership / Mercy)**
*   Rebirth accumulates **Leadership Points (LP)**. First to 10 LP wins immediately.
*   *Earning LP (+1 per action, limit 1 Coexistence LP per turn):*
    *   **Withdrawal:** Moving a piece out of an enemy's attack range without capturing.
    *   **Shielding:** Moving a piece between an enemy and your Leader/Rebirth, breaking the line of attack.
    *   **Coexistence:** Ending your turn adjacent to an enemy piece without capturing.

**Path C: Mortals Win (The Fall)**
*   If Rebirth accidentally applies the "Veiled" state to **5 of her own friendly pieces** over the course of the game, she loses control. Mortals win collectively.

### 6. Edge Cases & Engine Logic
*   **Capturing the Veiled:** Veiled pieces cannot capture, but they *can* be captured by standard rules.
*   **Stacking Veil:** Duration does not stack. Entering the Radius refreshes the 1-turn duration.
*   **Dash Interaction:** Rebirth Dash does *not* trigger the Veil on pieces passed through; it only evaluates the destination square's adjacent cells.

---

## 🛠️ Repository Structure & Tech Stack

*This project is currently in the **Prototype Phase**. We are building the digital sandbox to prove the 4-player geometry and AoE logic before committing to physical manufacturing.*

```text
/veiled-dominion
├── /docs                   # Design bibles, lore, aesthetic references
│   ├── QUEENS_JOURNEY.md   # Raw worldbuilding & thematic logic
│   └── PITCH_DECK.md       # Visual/Investor presentation logic
├── /src                    # Core game engine
│   ├── /board              # 14x14 cross-grid topology & coordinate logic
│   ├── /pieces             # Base piece classes & locomotion rules
│   ├── /systems            # Radius of Ruin AoE, Veil state machine, LP tracker
│   └── /input              # Turn phase management, UI state hooks
├── /assets                 # 3D models, VFX, SFX
│   ├── /models             # Rebirth (translucent), Death (Musou Black/Absorption shader)
│   └── /audio              # Drone/ambient SFX, state-change audio cues
└── README.md               # You are here
```

**Target Engine:** Unity (C#) or Unreal (C++). *Decision to be made by lead prototype engineer.*

---

## 🤝 Open Seats (How to Contribute)

We are specifically looking for systems thinkers who understand that constraints create fun. If you have experience building spatial logic, AoE calculations, or asynchronous multiplayer architectures, we need you.

**Immediate Needs:**
1.  **Prototype Engineer:** Build the 14x14 grid, piece locomotion, and the `RadiusOfRuin.cs` state machine.
2.  **Technical Artist:** Create the shader for Death (light absorption/void) and Rebirth (internal refraction/glow).
3.  **Systems Designer:** Run the mathematical simulations on the "Martyr's Boon" economy and 4-player LP scaling.

### Contribution Guidelines
1.  Fork the repo.
2.  Create a feature branch (`git checkout -b feature/radius-logic`).
3.  Implement your changes with clean, heavily commented code. If you change a mechanic, update *this* README first.
4.  Submit a PR with a clear explanation of how your code interacts with the Turn Structure Loop.

---

## 📬 Contact

Direct applications, hate mail, or existential questions about game design:  
**questions@loptrlab.com**  
*(Include links to your portfolio or previous shipped titles. We respect the craft.)*

---
*© Daddy's Little Mortis. The universe is a delicate cycle.*
```
