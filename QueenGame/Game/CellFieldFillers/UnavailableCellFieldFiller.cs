using QueenGame.Game.Models;

namespace QueenGame.Game.CellFieldFiller
{
    public class UnavailableCellFieldFiller : ICellFieldFiller
    {
        public IEnumerable<Cell> Fill(int size, IEnumerable<Cell> srcCells)
        {
            List<Cell> matrix = new List<Cell>(size * size);

            if (srcCells != null && srcCells.Any())
            {
                List<Cell> cells = srcCells
                    .DistinctBy(x => (x.Row, x.Column))
                    .OrderBy(c => c.Row * size + c.Column)
                    .ToList();

                Cell? headCell = cells.FirstOrDefault();
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        while (headCell != null
                            && (headCell.Row < 0
                                || headCell.Row >= size
                                || headCell.Column < 0
                                || headCell.Column >= size))
                        {
                            cells.Remove(headCell);
                            headCell = cells.FirstOrDefault();
                        }

                        if (headCell != null && headCell.Row == i && headCell.Column == j)
                        {
                            matrix.Add(headCell);
                            cells.Remove(headCell);
                            headCell = cells.FirstOrDefault();
                        }
                        else
                        {
                            matrix.Add(new Cell
                            {
                                Row = i,
                                Column = j,
                                Group = null,
                                State = CellState.UNAVAILABLE,
                            });
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        matrix.Add(new Cell
                        {
                            Row = i,
                            Column = j,
                            Group = null,
                            State = CellState.UNAVAILABLE,
                        });
                    }
                }
            }

            return matrix;
        }

    }
}
