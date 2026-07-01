# Contributing to Veiled Dominion Engine

Thank you for helping us build **Veiled Dominion**, the asymmetrical 4-player tactical chess-variant engine. This repository houses the core state machine, spatial mechanics pipelines, and network synchronization layers that power our broader transmedia ecosystem.

By contributing to this project, you help scale an extensible, decentralized framework for alternative strategy games. Please review this document completely before opening an issue or a pull request.

---

## ⚖️ 1. Licensing, IP Protection & Provenance Boundaries

To maintain a healthy open-source ecosystem while strictly protecting the commercial integrity of the broader brand lore, this repository enforces two distinct licensing boundaries:

### A. The Engine Code and Public Assets (`CC BY-NC-SA 4.0`)

All source code, public-domain design documentation, configuration tooling, and core state machine files in this repository are licensed under the **Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International** license.

* **What this means for you:** You are completely free to share, copy, remix, transform, and build upon these engine frameworks for *non-commercial* purposes, provided you give appropriate credit and distribute your contributions under this exact same license.
* **Commercial Exclusion:** You may *not* use this material for commercial advantage or monetary compensation without explicit written permission from Loptr Lab.
* **Full License Text:** See [`LICENSE.md`](./LICENSE.md) in the repository root.

### B. Commercial Brand Core & Intellectual Property Restrictions

The core narrative IP, specific fictional characters, visual world-building assets, and titles linked to the **Daddy's Little Mortis** digital ecosystem (including but not limited to the manga *The Diary of Death's Daughter* and the *Source Code* apparel collection) are explicitly excluded from this open-source grant.

* **Scope:** Contributors may write open-source variant logic using generic or public-domain motifs, but must *never* commit proprietary assets or commercial brand artwork into this public engine repository.
* **Enforcement:** Any pull request detected introducing commercial brand IP will be immediately rejected with a detailed explanation, and the contributor will be encouraged to apply to our design team directly at **questions@loptrlab.com**.

### C. Public Domain Literary Provenance Guardrails

When contributing code comments, thematic variables, or rulesets derived from historical literature or philosophical texts (e.g., Seneca, Epicurus, Poe), you must confirm clean provenance to avoid copyright risk.

* **Rule:** Every pull request introducing literary citations must include a stable public-domain source archive link (e.g., Project Gutenberg, Internet Archive).
* **Forbidden:** Modern copyrighted translations are strictly forbidden; only original texts or verifiably open public-domain translations may be used.
* **Example Format:** 
  ```csharp
  // "On the Nature of Restraint" — Seneca
  // Source: https://www.gutenberg.org/ebooks/3793
  public const string RestraintPhilosophy = "...";
  ```

---

## 🏗️ 2. Development Environment Setup

### Quick Start (Recommended)

We've automated the entire development environment using **Dev Containers** and **GitHub Codespaces**. No local installation required.

#### Option A: GitHub Codespaces (Cloud-Based, Zero Setup)
1. Click the green **Code** button on the repository page.
2. Select the **Codespaces** tab.
3. Click **Create codespace on main**.
4. The environment boots automatically with .NET 8, Node.js, and all extensions pre-installed.

#### Option B: Local Dev Container (VS Code)
1. Clone the repository: `git clone https://github.com/Loptr-Lab/veiled-dominion-engine.git`
2. Open in VS Code: `code .`
3. When prompted, click **Reopen in Container**.
4. Wait 3-5 minutes for the first-time build (subsequent launches are instant).

#### Environment Verification
Once your sandbox is ready, run:
```bash
dotnet --version    # Should output 8.0.x
node --version      # Should output 20.x
dotnet restore
dotnet test
```

For detailed environment documentation, see [`docs/DEVELOPMENT_ENVIRONMENT.md`](./docs/DEVELOPMENT_ENVIRONMENT.md).

---

## 🎯 3. Core Architectural Principles

### The Movement Calculation Loop

All piece locomotion must respect the canonical **Movement Calculation Pipeline**. Never hardcode valid moves directly. Instead, ensure all spatial navigation arrays pass through this strict sequence:

```
1. Calculate Base Movement Vectors
   ↓ (Apply piece's inherent movement rules)
   ↓
2. Evaluate Spatial State Modifiers
   ↓ (Check for `Veiled` debuff, sanctuaries, etc.)
   ↓
3. Apply Resource/Aura Overrides
   ↓ (e.g., `Radius of Ruin` field, `Soul Reservoir` upgrades)
   ↓
4. Return Validated Coordinate Set
   ↓ (Filter illegal moves before UI presentation)
```

### The Veiled State Machine

The `Veiled` status must be managed through a strict lifecycle:

- **Trigger:** Piece ends turn within 1 square of Rebirth
- **Duration:** Exactly 1 full round (cleared at start of owner's next turn)
- **Effect:** Movement restricted to 1 square forward, no captures allowed
- **Stacking:** Duration refreshes (does not stack) if re-entered

### Radius of Ruin AoE Calculations

The `RadiusOfRuin` system is the game's primary technical challenge. When implementing:

1. Use efficient spatial indexing (quadtree or grid-based lookup).
2. Evaluate all 8 adjacent squares to Rebirth's position each turn.
3. Apply state changes atomically at phase resolution time (not during player input).
4. Log all Veil applications for "The Fall" counter tracking.

For detailed architectural guidance, see [`README.md`](./README.md) and [`docs/DEVELOPMENT_ENVIRONMENT.md`](./docs/DEVELOPMENT_ENVIRONMENT.md).

---

## 🛠️ 4. Pull Request Submission Workflow

### Pre-Submission Checklist

Before opening a pull request, ensure every item is satisfied:

- [ ] **Branch Naming:** Follows semantic convention
  - Features: `feature/descriptive-name`
  - Bugfixes: `bugfix/issue-number-short-name`
  - Hotfixes: `hotfix/critical-issue`
  
- [ ] **Code Compiles:** `dotnet build` returns zero errors inside the devcontainer

- [ ] **Tests Pass:** `dotnet test` returns 100% pass rate
  - New piece mechanics require corresponding unit test blocks
  - Edge cases (e.g., Veil stacking, Dash interactions) must be explicitly tested

- [ ] **Documentation Updated:** 
  - Code comments are thorough and explain the *why* (not just the *what*)
  - If mechanics change, [`README.md`](./README.md) is updated to reflect truth

- [ ] **Provenance Compliance:** (If applicable)
  - All literary citations include stable public-domain archive links
  - No modern copyrighted translations or proprietary brand assets are included

- [ ] **IP Compliance Signed-Off:**
  - ✅ No commercial brand artwork (Diary of Death's Daughter, Source Code apparel, etc.)
  - ✅ No proprietary narrative IP from Daddy's Little Mortis ecosystem
  - ✅ Generic or public-domain motifs only

- [ ] **Project Board Updated:** The corresponding GitHub Project Board card has been moved to **In Review**

### PR Template

When opening a pull request, use the following template in your PR description:

```markdown
## 🎯 Purpose
[Brief description of what this PR accomplishes]

## 📋 Related Issue
Closes #[issue-number] (if applicable)

## 🧪 Testing
- [ ] Unit tests added/updated
- [ ] Manual testing completed
- [ ] Edge cases verified

## ♟️ Mechanical Changes (If Applicable)
[If this PR modifies game rules, explicitly explain:]
- Movement rules affected
- State machine changes
- Aura/resource interactions

## 📚 Documentation
- [ ] README.md updated (if mechanics changed)
- [ ] Code comments thorough
- [ ] Provenance links included (if literary references)

## ✅ Compliance Checklist
- [ ] Code compiles without errors
- [ ] All tests pass
- [ ] No commercial IP included
- [ ] Public domain provenance documented
- [ ] CC BY-NC-SA 4.0 compliance confirmed
```

---

## 📊 5. GitHub Project Board Structure

All development activity flows through our GitHub Project Board using this status pipeline:

```
┌──────────┐    ┌─────────┐    ┌──────────────┐    ┌───────────┐    ┌──────────┐
│ 📥 Triage│ ─→ │📋 Ready │ ─→ │ ⚙️ In Progress│ ─→ │🔍 Review │ ─→ │✅ Merged │
└──────────┘    └─────────┘    └──────────────┘    └───────────┘    └──────────┘
```

### Column Definitions

| Column | Purpose | Who Acts | Next Step |
|--------|---------|----------|-----------|
| **📥 Triage** | New feature requests, architectural discussions, long-term roadmap items. | Maintainers | Scoped & prioritized into **Ready** |
| **📋 Ready** | Well-defined, scoped tasks ready to claim. May include `good-first-issue` or `help-wanted` labels. | Contributors | Comment to request assignment, create feature branch |
| **⚙️ In Progress** | Active development. Issue should be assigned to you. Only one person per issue to avoid duplication. | Assigned Dev | Push commits, open PR when complete |
| **🔍 Review** | Pull request is open and awaiting code review. | Maintainers + Community | Address feedback, iterate on branch |
| **✅ Merged** | PR approved and merged to `main`. Archived for reference. | Maintainers | Celebrate! 🎉 |

### Rules

* **Never start work on an issue unless it's assigned to you.** Comment on the issue card first to request assignment.
* **Communicate blockers early.** If you're stuck, move the card back to **Ready** or comment with details so another contributor can help.
* **Link your PR to the issue.** Include `Closes #[issue-number]` in your PR description.

---

## 🤝 6. Community Guidelines & Conduct

### Be Respectful
This is a collaborative game design and engineering community. We value:
- Constructive feedback
- Curiosity about game mechanics
- Patience with diverse skillsets
- Giving credit generously

### No Toxic Behavior
We will not tolerate:
- Personal attacks or harassment
- Gatekeeping or elitism
- Dismissive language toward contributors
- Spam or commercial promotion

Any violation will result in immediate contributor removal and potential GitHub reporting.

### Asking for Help
This project welcomes questions. If you're stuck:
1. Check the [`docs/DEVELOPMENT_ENVIRONMENT.md`](./docs/DEVELOPMENT_ENVIRONMENT.md) guide first.
2. Search open and closed issues for similar problems.
3. Comment on the issue card with your specific blocker.
4. Email **questions@loptrlab.com** if you need urgent support.

---

## 📬 7. Contact & Escalation

### For Contribution Questions
- **GitHub Issues:** Open an issue with the `question` label
- **Email:** questions@loptrlab.com
- **Include:** Your GitHub handle, what you're working on, and what help you need

### For Commercial Licensing or Brand Partnerships
- **Email:** questions@loptrlab.com
- **Include:** Your project scope, timeline, and proposed terms

### For Security Vulnerabilities
- **Do NOT open a public issue.** Email security@loptrlab.com with details.
- We will investigate and patch before public disclosure.

---

## ✨ Recognition

Contributors who land merged PRs will be:
- Added to the **CONTRIBUTORS.md** file (coming soon)
- Credited in release notes
- Invited to our Discord/community spaces for ongoing collaboration

Thank you for helping build the future of constraint-based game design. 🎮♟️

---

**Last Updated:** 2026-07-01  
**Maintained by:** Loptr Lab (@ibloud)  
**License:** CC BY-NC-SA 4.0
