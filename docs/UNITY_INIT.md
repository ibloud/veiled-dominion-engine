# Unity Initialization Guide: Loptr Lab Prototype

## Environment Specifications
*   **Engine:** Unity 2022.3 LTS (or newer LTS)
*   **Render Pipeline:** Universal Render Pipeline (URP)
*   **Template:** 3D (URP)
*   **Input:** New Input System (optional for now, standard Input Manager is fine for grey-box)

## 1. Project Setup
1. Open Unity Hub -> New Project -> 3D (URP).
2. Name the project `VeiledDominionPrototype`.
3. Save the initial scene as `Scenes/GreyBox_Test.unity`.

## 2. Repository Folder Structure
Create the following folders in the `Assets/` directory to match our C# architecture:

```text
Assets/
├── _Project/
│   ├── Scripts/
│   │   ├── Core/          # BasePiece.cs, GridTopology.cs
│   │   ├── Systems/       # RadiusOfRuin.cs, VeiledStateManager.cs
│   │   └── UI/            # (Future: Turn indicators, LP tracker)
│   ├── Prefabs/
│   │   └── Pieces/        # (Future: Cube_Pawn, Sphere_Death, etc.)
│   ├── Materials/
│   │   ├── Core/          # Mat_Obsidian, Mat_MusouBlack, Mat_RebirthGlow
│   │   └── Debug/         # Mat_VeiledIndicator (e.g., bright red for testing)
│   └── Scenes/
├── Fonts/                 # (Future: Minimalist font for debug UI)
└── Textures/              # (Future: Grid lines, noise for obsidian)
```

## 3. C# Script Integration
1. Locate the core logic files in the root `/src/` directory of this repository.
2. Drag and drop `BasePiece.cs`, `GridTopology.cs`, `RadiusOfRuin.cs`, and `VeiledStateManager.cs` into their respective `_Project/Scripts/` folders in Unity.
3. **CRITICAL:** Open the Unity Console. Resolve any missing namespace or assembly definition errors. (We may need to create an `Assembly-CSharp` definition if we split into subfolders, but standard root folders should compile immediately).

## 4. Grey-Box Scene Setup (Immediate Action)
Do not import external 3D models. Use native Unity primitives.

1.  **Lighting:**
    *   Delete the default Directional Light.
    *   Create a new Directional Light: Color `#FFFFFF`, Intensity `0.3`.
    *   Create a Point Light at position `(0, 5, 0)`: Color `#D4A853` (Amber), Intensity `3.0`, Range `20`.
2.  **Board:**
    *   Create a 3D Plane. Scale `(14, 1, 14)`. Position `(0, 0, 0)`.
    *   Create a material `Mat_Obsidian`: Base Color `#1A1A1A`, Metallic `0.2`, Smoothness `0.8`. Apply to Plane.
3.  **Primitives (Placeholders):**
    *   **Death:** Create a Sphere. Scale `(0.8, 0.8, 0.8)`. Create material `Mat_MusouBlack`: Base Color `#000000`, Metallic `0`, Smoothness `1.0`. *(Note: True light absorption requires URP Shader Graph later, use dark grey for now if it disappears completely).*
    *   **Rebirth:** Create a Sphere. Scale `(0.8, 0.8, 0.8)`. Create material `Mat_RebirthGlow`: Base Color `#000000`, Emission Color `#D4A853`, Emission Intensity `3.0+`.
    *   **Pawn:** Create a Cube. Scale `(0.6, 0.6, 0.6)`. Material `Mat_MusouBlack` or standard black.
4.  **Aura Trigger (Testing):**
    *   Create an Empty GameObject. Parent it to the Rebirth Sphere. Name it `AuraTrigger`.
    *   Add a `BoxCollider`. Check `Is Trigger`. Size `(3, 2, 3)` (covers a 1-square radius on the X/Z plane).
    *   *(We will wire this to `RadiusOfRuin.cs` once compilation is stable).*

## 5. Next Milestone
Scene renders correctly with the Amber light bouncing off the dark grey board, and the Rebirth sphere glowing prominently against the Death sphere. Zero compiler errors in the console.
