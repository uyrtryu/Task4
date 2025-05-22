using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessAppNew.Models
{
    public enum PieceColor
    {
        White,
        Black
    }

    public abstract class ChessPiece : IDynamicClass
    {
        public string ClassName => GetType().Name;
        public string Description => $"Шахматная фигура {ClassName}";

        public PieceColor Color { get; protected set; }
        public (int X, int Y) Position { get; protected set; }

        protected ChessPiece(PieceColor color, (int X, int Y) initialPosition)
        {
            if (!IsWithinBoard(initialPosition))
                throw new ArgumentException("Начальная позиция должна быть в пределах доски (0-7)");

            Color = color;
            Position = initialPosition;
        }

        public bool MakeMove((int X, int Y) newPosition, IEnumerable<ChessPiece> otherPieces)
        {
            if (IsValidMove(newPosition, otherPieces))
            {
                Position = newPosition;
                return true;
            }
            return false;
        }

        public IEnumerable<(int X, int Y)> GetPossibleMoves(IEnumerable<ChessPiece> otherPieces)
        {
            var possibleMoves = new List<(int X, int Y)>();

            // Проверяем все возможные позиции на доске
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    var position = (x, y);
                    if (IsValidMove(position, otherPieces))
                    {
                        possibleMoves.Add(position);
                    }
                }
            }

            return possibleMoves;
        }

        protected abstract bool IsValidMove((int X, int Y) newPosition, IEnumerable<ChessPiece> otherPieces);

        protected bool IsWithinBoard((int X, int Y) position)
        {
            return position.X >= 0 && position.X < 8 && position.Y >= 0 && position.Y < 8;
        }

        protected bool IsPathClear((int X, int Y) start, (int X, int Y) end, IEnumerable<ChessPiece> otherPieces)
        {
            int deltaX = Math.Sign(end.X - start.X);
            int deltaY = Math.Sign(end.Y - start.Y);

            int currentX = start.X + deltaX;
            int currentY = start.Y + deltaY;

            while (currentX != end.X || currentY != end.Y)
            {
                if (otherPieces.Any(p => p.Position.X == currentX && p.Position.Y == currentY))
                {
                    return false;
                }

                currentX += deltaX;
                currentY += deltaY;
            }

            // Проверяем конечную позицию
            var pieceAtEnd = otherPieces.FirstOrDefault(p => p.Position.X == end.X && p.Position.Y == end.Y);
            return pieceAtEnd == null || pieceAtEnd.Color != this.Color;
        }
    }
}