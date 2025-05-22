using System;
using System.Collections.Generic;

namespace ChessAppNew.Models
{
    public class Rook : ChessPiece
    {
        public Rook(PieceColor color, (int X, int Y) initialPosition)
            : base(color, initialPosition)
        {
        }

        protected override bool IsValidMove((int X, int Y) newPosition, IEnumerable<ChessPiece> otherPieces)
        {
            if (!IsWithinBoard(newPosition))
                return false;

            // Ладья может двигаться только по горизонтали или вертикали
            if (newPosition.X == Position.X || newPosition.Y == Position.Y)
            {
                return IsPathClear(Position, newPosition, otherPieces);
            }

            return false;
        }
    }
}