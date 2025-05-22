using System;

namespace ChessApp.Models
{
    public class Bishop : ChessPiece
    {
        public Bishop(string color, (int X, int Y) position) : base(color, position) { }

        protected override bool CanMove((int X, int Y) newPosition)
        {
            int dx = Math.Abs(newPosition.X - Position.X);
            int dy = Math.Abs(newPosition.Y - Position.Y);
            return dx == dy;
        }
        public override string ToString() => $"{GetType().Name} ({Color}) at {Position}";
        public override string GetSymbol() => "♗";

    }
}