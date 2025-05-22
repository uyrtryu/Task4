using System;

namespace ChessApp.Models
{
    public abstract class ChessPiece
    {
        public string Color { get; set; } // "white" or "black"
        public (int X, int Y) Position { get; set; }

        protected ChessPiece(string color, (int X, int Y) position)
        {
            Color = color;
            Position = position;
        }

        public bool Move((int X, int Y) newPosition)
        {
            if (CanMove(newPosition))
            {
                Position = newPosition;
                return true;
            }
            return false;
        }

        protected abstract bool CanMove((int X, int Y) newPosition);

        public override string ToString()
        {
            return $"{GetType().Name} ({Color}, {Position.X},{Position.Y})";
        }
        public virtual string GetSymbol()
        {
            return "?"; 
        }

    }
}