using System;

namespace ChessApp.Models
{
    public class Queen : ChessPiece
    {
        public Queen(string color, (int X, int Y) position) : base(color, position) { }

        protected override bool CanMove((int X, int Y) newPosition)
        {
            int dx = Math.Abs(newPosition.X - Position.X);
            int dy = Math.Abs(newPosition.Y - Position.Y);
            return dx == dy || dx == 0 || dy == 0;
        }
        public override string GetSymbol() => "♕";
    }
}