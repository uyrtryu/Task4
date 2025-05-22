using ChessApp.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace ChessApp.ViewModels
{
    public class MainWindowViewModel
    {
        public ObservableCollection<ChessPiece> Pieces { get; set; }

        public MainWindowViewModel()
        {
            Pieces = new ObservableCollection<ChessPiece>
            {
                new Queen("white", (0, 3)),
                new Rook("black", (7, 0)),
                new Bishop("white", (2, 0))
            };
        }

        public bool MovePiece(int index, int x, int y)
        {
            if (index < 0 || index >= Pieces.Count)
                return false;

            return Pieces[index].Move((x, y));
        }
        public ChessPiece? GetPieceAt(int x, int y)
        {
            return Pieces.FirstOrDefault(p => p.Position.X == x && p.Position.Y == y);
        }

        public int GetPieceIndexAt(int x, int y)
        {
            for (int i = 0; i < Pieces.Count; i++)
            {
                if (Pieces[i].Position.X == x && Pieces[i].Position.Y == y)
                    return i;
            }
            return -1;
        }

    }
}