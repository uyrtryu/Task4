namespace ChessApp.Models
{
    public class Rook : ChessPiece
    {
        public Rook(string color, (int X, int Y) position) : base(color, position) { }

        protected override bool CanMove((int X, int Y) newPosition)
        {
            return Position.X == newPosition.X || Position.Y == newPosition.Y;
        }
        public override string ToString() => $"{GetType().Name} ({Color}) at {Position}";
        public override string GetSymbol() => "♖";

    }
}