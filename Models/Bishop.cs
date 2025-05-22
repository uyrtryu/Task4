using System;
using System.Collections.Generic;

namespace ChessAppNew.Models
{
    public class Bishop : ChessPiece
    {
        public Bishop(PieceColor color, (int X, int Y) initialPosition)
            : base(color, initialPosition)
        {
        }

        protected override bool IsValidMove((int X, int Y) newPosition, IEnumerable<ChessPiece> otherPieces)
        {
            if (!IsWithinBoard(newPosition))
                return false;

            int deltaX = Math.Abs(newPosition.X - Position.X);
            int deltaY = Math.Abs(newPosition.Y - Position.Y);

            // Слон может двигаться только по диагонали
            if (deltaX == deltaY)
            {
                return IsPathClear(Position, newPosition, otherPieces);
            }

            return false;
        }
    }
}