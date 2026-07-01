using System;
using System.Collections.Generic;
using System.Linq;
using VeiledDominion.Board;

namespace VeiledDominion.Pieces
{
    /// <summary>
    /// Enumeration of all piece types in Veiled Dominion.
    /// </summary>
    public enum PieceType
    {
        Leader,      // King equivalent (1 per player)
        Queen,       // Standard queen (1 per mortal player)
        Rebirth,     // Unique to Rebirth faction (replaces Queen)
        Death,       // Unique to Rebirth faction (mentor, unpassable)
        Rook,        // Standard rook
        Bishop,      // Standard bishop
        Knight,      // Standard knight
        Pawn         // Standard pawn
    }

    /// <summary>
    /// Enumeration of piece factions (players).
    /// </summary>
    public enum PlayerFaction
    {
        North = 1,
        East = 2,
        West = 3,
        South = 4
    }

    /// <summary>
    /// Represents a movement pattern for a piece (e.g., Queen moves, Knight jumps).
    /// </summary>
    public enum MovementPattern
    {
        /// <summary>Orthogonal (up/down/left/right) infinite sliding.</summary>
        Orthogonal,

        /// <summary>Diagonal infinite sliding.</summary>
        Diagonal,

        /// <summary>Orthogonal + Diagonal (Queen-like).</summary>
        Combined,

        /// <summary>1 square in any direction (King-like).</summary>
        OneSquare,

        /// <summary>L-shaped jump (Knight).</summary>
        KnightJump,

        /// <summary>1 square forward (Pawn).</summary>
        OneForward,

        /// <summary>Cannot move (Death piece).</summary>
        Immobile
    }

    /// <summary>
    /// BasePiece is the abstract foundation for all game pieces.
    /// 
    /// Lifecycle:
    /// 1. ValidateMovement(targetCoord) → returns list of legal moves
    /// 2. ApplyMove(targetCoord) → actually move the piece and trigger state changes
    /// 3. OnResolveCapture(capturedPiece) → handle post-capture logic
    /// 
    /// All pieces respect the canonical Movement Calculation Loop:
    /// Base Vectors → State Modifiers → Resource Overrides → Validated Coordinates
    /// </summary>
    public abstract class BasePiece
    {
        public string Id { get; }
        public PieceType Type { get; protected set; }
        public PlayerFaction Faction { get; }
        public GridCoordinate Position { get; protected set; }
        public MovementPattern BaseMovement { get; protected set; }
        public bool IsAlive { get; protected set; }

        /// <summary>
        /// Tracks if this piece is currently in the Veiled debuff state.
        /// When true: Movement restricted to 1 square forward, cannot capture.
        /// </summary>
        public bool IsVeiled { get; set; }

        /// <summary>
        /// If Veiled, tracks how many turns remain before the debuff clears.
        /// Decremented at the start of the owner's turn.
        /// </summary>
        public int VeiledRoundsRemaining { get; set; }

        protected BasePiece(string id, PieceType type, PlayerFaction faction, GridCoordinate startPosition, MovementPattern movement)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Type = type;
            Faction = faction;
            Position = startPosition;
            BaseMovement = movement;
            IsAlive = true;
            IsVeiled = false;
            VeiledRoundsRemaining = 0;
        }

        /// <summary>
        /// Step 1 of the Movement Calculation Loop: Calculate base movement vectors.
        /// Returns all possible destination coordinates based on the piece's inherent movement rules,
        /// ignoring state modifiers and blockages.
        /// 
        /// Override this in subclasses to implement specific locomotion rules.
        /// </summary>
        protected virtual List<GridCoordinate> CalculateBaseMovement(GridCoordinate from)
        {
            return BaseMovement switch
            {
                MovementPattern.OneSquare => GridTopology.GetAdjacentSquares(from),
                MovementPattern.OneForward => CalculateOneForwardMovement(from),
                MovementPattern.Orthogonal => CalculateOrthogonalMovement(from),
                MovementPattern.Diagonal => CalculateDiagonalMovement(from),
                MovementPattern.Combined => CalculateCombinedMovement(from),
                MovementPattern.KnightJump => CalculateKnightJumps(from),
                MovementPattern.Immobile => new List<GridCoordinate>(),
                _ => throw new InvalidOperationException($"Unknown movement pattern: {BaseMovement}")
            };
        }

        /// <summary>
        /// Step 2 of the Movement Calculation Loop: Apply state modifiers.
        /// If this piece is Veiled, restrict movement to 1 square forward only.
        /// </summary>
        protected virtual List<GridCoordinate> ApplyStateModifiers(List<GridCoordinate> baseMovement)
        {
            if (IsVeiled)
            {
                // Veiled pieces can only move 1 square forward
                return CalculateOneForwardMovement(Position);
            }

            return baseMovement;
        }

        /// <summary>
        /// Step 3 of the Movement Calculation Loop: Apply resource/aura overrides.
        /// Base implementation does nothing. Override in subclasses for special abilities.
        /// Examples: Rebirth's Soul Reservoir (Dash ability), Death's Sanctuary immunity.
        /// </summary>
        protected virtual List<GridCoordinate> ApplyResourceOverrides(List<GridCoordinate> stateModifiedMovement)
        {
            return stateModifiedMovement;
        }

        /// <summary>
        /// Final step: Filter out blocked squares and return validated legal moves.
        /// The blockage check is provided as a callback to allow board-level occupancy tracking.
        /// </summary>
        protected virtual List<GridCoordinate> FilterBlockedSquares(
            List<GridCoordinate> candidates,
            Func<GridCoordinate, bool> isBlocked,
            Func<GridCoordinate, bool> isValidCapture)
        {
            var legal = new List<GridCoordinate>();

            foreach (var coord in candidates)
            {
                if (!GridTopology.IsValidCoordinate(coord) || GridTopology.IsNeutralZone(coord))
                    continue;

                bool blocked = isBlocked(coord);
                bool capturable = isValidCapture(coord);

                // If Veiled, cannot capture at all
                if (IsVeiled && capturable)
                    continue;

                // If not blocked, it's legal
                if (!blocked)
                {
                    legal.Add(coord);
                }
                // If blocked but capturable, it's also legal (capture move)
                else if (capturable)
                {
                    legal.Add(coord);
                }
            }

            return legal;
        }

        /// <summary>
        /// Public API: Validates all legal moves from current position.
        /// This is the complete Movement Calculation Loop.
        /// 
        /// Callbacks:
        /// - isBlocked: Returns true if a square has an occupying piece
        /// - isValidCapture: Returns true if a square contains an enemy piece
        /// </summary>
        public List<GridCoordinate> ValidateMovement(
            Func<GridCoordinate, bool> isBlocked,
            Func<GridCoordinate, bool> isValidCapture)
        {
            // Step 1: Calculate base movement vectors
            var baseMovement = CalculateBaseMovement(Position);

            // Step 2: Apply state modifiers (Veiled, etc.)
            var stateModified = ApplyStateModifiers(baseMovement);

            // Step 3: Apply resource/aura overrides (Dash, etc.)
            var resourceOverridden = ApplyResourceOverrides(stateModified);

            // Step 4: Filter blocked squares and return validated coordinates
            var legal = FilterBlockedSquares(resourceOverridden, isBlocked, isValidCapture);

            return legal;
        }

        /// <summary>
        /// Public API: Apply a move to this piece.
        /// Updates position and triggers any post-move logic (capturing, state changes).
        /// </summary>
        public virtual void ApplyMove(GridCoordinate targetCoord)
        {
            if (!GridTopology.IsValidCoordinate(targetCoord))
                throw new InvalidOperationException($"Invalid target coordinate: {targetCoord}");

            Position = targetCoord;
        }

        /// <summary>
        /// Public API: Handle post-capture logic (e.g., Rebirth capturing enemy pieces).
        /// Override in subclasses for special capture rules.
        /// </summary>
        public virtual void OnResolveCapture(BasePiece capturedPiece)
        {
            // Base implementation: no special logic
        }

        /// <summary>
        /// Clear the Veiled debuff from this piece.
        /// Called at the start of the owner's turn.
        /// </summary>
        public virtual void ClearVeiledState()
        {
            IsVeiled = false;
            VeiledRoundsRemaining = 0;
        }

        /// <summary>
        /// Apply the Veiled debuff to this piece.
        /// Duration: exactly 1 full round (cleared at start of owner's next turn).
        /// </summary>
        public virtual void ApplyVeiledState()
        {
            IsVeiled = true;
            VeiledRoundsRemaining = 1;
        }

        /// <summary>
        /// Mark this piece as captured/dead.
        /// </summary>
        public virtual void Capture()
        {
            IsAlive = false;
            Position = new GridCoordinate(-1, -1); // Mark as off-board
        }

        // ========== Helper Methods for Movement Calculation ==========

        private List<GridCoordinate> CalculateOneForwardMovement(GridCoordinate from)
        {
            // Direction depends on faction
            int dy = Faction switch
            {
                PlayerFaction.North => -1,  // Move toward row 0
                PlayerFaction.South => 1,   // Move toward row 13
                PlayerFaction.West => -1,   // Move toward column 0 (treat as "forward")
                PlayerFaction.East => 1,    // Move toward column 13 (treat as "forward")
                _ => 0
            };

            var forward = new GridCoordinate(from.X, from.Y + dy);
            return GridTopology.IsValidCoordinate(forward) ? new List<GridCoordinate> { forward } : new List<GridCoordinate>();
        }

        private List<GridCoordinate> CalculateOrthogonalMovement(GridCoordinate from)
        {
            var moves = new List<GridCoordinate>();
            var directions = new[] { (0, -1), (1, 0), (0, 1), (-1, 0) }; // N, E, S, W

            foreach (var (dx, dy) in directions)
            {
                AddSlidingMoves(from, dx, dy, moves);
            }

            return moves;
        }

        private List<GridCoordinate> CalculateDiagonalMovement(GridCoordinate from)
        {
            var moves = new List<GridCoordinate>();
            var directions = new[] { (1, -1), (1, 1), (-1, 1), (-1, -1) }; // NE, SE, SW, NW

            foreach (var (dx, dy) in directions)
            {
                AddSlidingMoves(from, dx, dy, moves);
            }

            return moves;
        }

        private List<GridCoordinate> CalculateCombinedMovement(GridCoordinate from)
        {
            var moves = new List<GridCoordinate>();
            moves.AddRange(CalculateOrthogonalMovement(from));
            moves.AddRange(CalculateDiagonalMovement(from));
            return moves;
        }

        private List<GridCoordinate> CalculateKnightJumps(GridCoordinate from)
        {
            var jumps = new List<GridCoordinate>();
            var knightOffsets = new[]
            {
                (2, 1), (2, -1), (-2, 1), (-2, -1),
                (1, 2), (1, -2), (-1, 2), (-1, -2)
            };

            foreach (var (dx, dy) in knightOffsets)
            {
                var target = new GridCoordinate(from.X + dx, from.Y + dy);
                if (GridTopology.IsValidCoordinate(target))
                {
                    jumps.Add(target);
                }
            }

            return jumps;
        }

        private void AddSlidingMoves(GridCoordinate from, int dx, int dy, List<GridCoordinate> moves)
        {
            var current = new GridCoordinate(from.X + dx, from.Y + dy);

            while (GridTopology.IsValidCoordinate(current) && !GridTopology.IsNeutralZone(current))
            {
                moves.Add(current);
                current = new GridCoordinate(current.X + dx, current.Y + dy);
            }
        }

        public override string ToString()
        {
            return $"{Type} ({Faction}) at {Position} [ID: {Id}]";
        }
    }
}
