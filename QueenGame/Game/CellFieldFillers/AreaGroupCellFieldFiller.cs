
using QueenGame.Game.Models;

namespace QueenGame.Game.CellFieldFiller
{
    /// <summary>
    /// Uses nearest area to build connected group.
    /// Considers accessebility of dioganal elements.
    /// </summary>
    public class AreaGroupCellFieldFiller : ICellFieldFiller
    {
        public IEnumerable<Cell> Fill(int size, IEnumerable<Cell> answerCells)
        {
            Random rand = new Random(Guid.NewGuid().GetHashCode());

            List<Cell> filledCells = new List<Cell>(size * size);
            filledCells.AddRange(answerCells);
            List<Cell> availableCells = new List<Cell>(answerCells);

            while (availableCells.Count > 0)
            {
                int pos = rand.Next(availableCells.Count);
                Cell cell = availableCells.ElementAt(pos);
                availableCells.RemoveAt(pos);

                List<Cell> availableAreaCells = GetAvailableAreaCells(cell, size, filledCells);
                if (availableAreaCells.Count > 0)
                {
                    Cell areaCell = availableAreaCells.ElementAt(rand.Next(availableAreaCells.Count));
                    areaCell.Group = cell.Group;

                    filledCells.Add(areaCell);
                    availableCells.Add(areaCell);
                    availableCells.Add(cell);
                }
                else
                {
                    filledCells.Add(cell);
                }
            }

            return filledCells
                .DistinctBy(c => (c.Row, c.Column))
                .OrderBy(c => c.Row * size + c.Column);
        }

        private List<Cell> GetAvailableAreaCells(Cell cell, int size, List<Cell> filledCells)
        {
            IEnumerable<Cell> filledCellArea = filledCells.Where(c =>
                Math.Abs(c.Row - cell.Row) <= 1
                && Math.Abs(c.Column - cell.Column) <= 1);

            List<Cell> availableCellArea = new List<Cell>();

            // direct access by line (vertical or horizontal)

            Cell? checkCell;

            // top
            if ((checkCell = filledCellArea.FirstOrDefault(c => c.Row == cell.Row - 1 && c.Column == cell.Column)) == null)
            {
                availableCellArea.Add(new Cell { Row = cell.Row - 1, Column = cell.Column });
            }
            else
            {
                if (checkCell.Group == cell.Group)
                {
                    // top left
                    if (!filledCellArea.Any(c => c.Row == cell.Row - 1 && c.Column == cell.Column - 1))
                        availableCellArea.Add(new Cell { Row = cell.Row - 1, Column = cell.Column - 1 });
                    // top right
                    if (!filledCellArea.Any(c => c.Row == cell.Row - 1 && c.Column == cell.Column + 1))
                        availableCellArea.Add(new Cell { Row = cell.Row - 1, Column = cell.Column + 1 });
                }
            }
            // bottom
            if ((checkCell = filledCellArea.FirstOrDefault(c => c.Row == cell.Row + 1 && c.Column == cell.Column)) == null)
            {
                availableCellArea.Add(new Cell { Row = cell.Row + 1, Column = cell.Column });
            }
            else
            {
                if (checkCell.Group == cell.Group)
                {
                    // bottom left
                    if (!filledCellArea.Any(c => c.Row == cell.Row + 1 && c.Column == cell.Column - 1))
                        availableCellArea.Add(new Cell { Row = cell.Row + 1, Column = cell.Column - 1 });
                    // bottom right
                    if (!filledCellArea.Any(c => c.Row == cell.Row + 1 && c.Column == cell.Column + 1))
                        availableCellArea.Add(new Cell { Row = cell.Row + 1, Column = cell.Column + 1 });
                }
            }
            // left
            if ((checkCell = filledCellArea.FirstOrDefault(c => c.Row == cell.Row && c.Column == cell.Column - 1)) == null)
            {
                availableCellArea.Add(new Cell { Row = cell.Row, Column = cell.Column - 1 });
            }
            else
            {
                if (checkCell.Group == cell.Group)
                {
                    // top left
                    if (!filledCellArea.Any(c => c.Row == cell.Row - 1 && c.Column == cell.Column - 1))
                        availableCellArea.Add(new Cell { Row = cell.Row - 1, Column = cell.Column - 1 });
                    // bottom left
                    if (!filledCellArea.Any(c => c.Row == cell.Row + 1 && c.Column == cell.Column - 1))
                        availableCellArea.Add(new Cell { Row = cell.Row + 1, Column = cell.Column - 1 });
                }
            }
            // right
            if ((checkCell = filledCellArea.FirstOrDefault(c => c.Row == cell.Row && c.Column == cell.Column + 1)) == null)
            {
                availableCellArea.Add(new Cell { Row = cell.Row, Column = cell.Column + 1 });
            }
            else
            {
                if (checkCell.Group == cell.Group)
                {
                    // top right
                    if (!filledCellArea.Any(c => c.Row == cell.Row - 1 && c.Column == cell.Column + 1))
                        availableCellArea.Add(new Cell { Row = cell.Row - 1, Column = cell.Column + 1 });
                    // bottom right
                    if (!filledCellArea.Any(c => c.Row == cell.Row + 1 && c.Column == cell.Column + 1))
                        availableCellArea.Add(new Cell { Row = cell.Row + 1, Column = cell.Column + 1 });
                }
            }

            // remove artificial cells and dioganal duplicates
            availableCellArea = availableCellArea
                .Where(c => c.Row != -1 && c.Row != size && c.Column != -1 && c.Column != size)
                .DistinctBy(c => (c.Row, c.Column))
                .ToList();
            availableCellArea.ForEach(c => { c.Group = null; c.State = CellState.NOT_SELECTED; });

            return availableCellArea;
        }
    }
}
