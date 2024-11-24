using QueenGame.Game.Models;

namespace QueenGame.Game.CellFieldFiller
{
    public interface ICellFieldFiller
    {
        IEnumerable<Cell> Fill(int size, IEnumerable<Cell> srcCells);
    }
}
