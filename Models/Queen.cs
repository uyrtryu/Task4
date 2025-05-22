using System;
using System.Collections.Generic;

namespace ChessAppNew.Models
{
    public class Queen : ChessPiece
    {
        public Queen(PieceColor color, (int X, int Y) initialPosition)
            : base(color, initialPosition)
        {
        }

        protected override bool IsValidMove((int X, int Y) newPosition, IEnumerable<ChessPiece> otherPieces)
        {
            if (!IsWithinBoard(newPosition))
                return false;

            int deltaX = Math.Abs(newPosition.X - Position.X);
            int deltaY = Math.Abs(newPosition.Y - Position.Y);

            // Ферзь может двигаться как по горизонтали/вертикали, так и по диагонали
            if (deltaX == 0 || deltaY == 0 || deltaX == deltaY)
            {
                return IsPathClear(Position, newPosition, otherPieces);
            }

            return false;
        }
    }
}