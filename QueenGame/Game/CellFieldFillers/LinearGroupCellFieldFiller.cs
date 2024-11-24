
using QueenGame.Game.Models;

namespace QueenGame.Game.CellFieldFiller
{
    public class LinearGroupCellFieldFiller : ICellFieldFiller
    {
        public IEnumerable<Cell> Fill(int size, IEnumerable<Cell> srcCells)
        {
            Random rand = new Random(Guid.NewGuid().GetHashCode());

            List<Cell> filledCells = new List<Cell>(size * size);
            filledCells.AddRange(srcCells);
            List<Cell> availableCells = new List<Cell>(srcCells);
            
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

                    filledCells.Add(cell);
                    filledCells.Add(areaCell);
                    availableCells.Add(areaCell);
                }
                else
                {
                    filledCells.Add(cell);
                }

                if (availableCells.Count == 0)
                {
                    filledCells = filledCells.DistinctBy(c => (c.Row, c.Column))
                        .OrderBy(c => c.Row * size + c.Column)
                        .ToList();
                    if (filledCells.Count < size * size)
                    {
                        List<Cell> toCoverCells = new List<Cell>();

                        int st, end;
                        int ip = -1, jp = -1;

                        if (filledCells.ElementAt(0).Row != 0 || filledCells.ElementAt(0).Column != 0)
                        {
                            toCoverCells.Add(new Cell
                            {
                                Row = 0,
                                Column = 0,
                                Group = -1,
                                State = CellState.NOT_SELECTED
                            });
                        }
                        ip = 0;
                        jp = 0;

                        foreach (var filledCell in filledCells)
                        {
                            end = filledCell.Row * size + filledCell.Column;
                            st = ip * size + jp;
                            if ((end) - (st) > 1)
                            {
                                for (int p = st + 1; p < end; p++)
                                {
                                    toCoverCells.Add(new Cell
                                    {
                                        Row = p / size,
                                        Column = p % size,
                                        Group = -1,
                                        State = CellState.NOT_SELECTED
                                    });
                                }
                            }
                            ip = filledCell.Row;
                            jp = filledCell.Column;
                        }

                        end = size * size;
                        st = ip * size + jp;
                        if ((end) - (st) > 1)
                        {
                            for (int p = st + 1; p < end; p++)
                            {
                                toCoverCells.Add(new Cell
                                {
                                    Row = p / size,
                                    Column = p % size,
                                    Group = -1,
                                    State = CellState.NOT_SELECTED
                                });
                            }
                        }
                        ip = size - 1;
                        jp = size - 1;

                        toCoverCells.ForEach(toCoverCell => 
                        {
                            availableCells.AddRange(filledCells.Where(c =>
                                Math.Abs(c.Row - cell.Row) + Math.Abs(c.Column - cell.Column) == 1
                                && c.Group != -1));
                        });

                        availableCells = availableCells
                            .DistinctBy(c => (c.Row, c.Column))
                            .OrderBy(c => (c.Row * rand.Next() + c.Column - c.Group) % rand.Next(1, size))
                            .ToList();
                    }
                }
            }

            return filledCells
                .DistinctBy(c => (c.Row, c.Column))
                .OrderBy(c => c.Row * size + c.Column);
        }

        private List<Cell> GetAvailableAreaCells(Cell cell, int size, List<Cell> filledCells)
        {
            List<Cell> availableCellArea = new List<Cell>(8);


            Dictionary<(int, int), Cell> filledCellArea = filledCells
                .DistinctBy(c => (c.Row, c.Column))
                .Where(c => Math.Abs(c.Row - cell.Row) + Math.Abs(c.Column - cell.Column) == 1)
                .ToDictionary(k => (k.Row, k.Column));

            Dictionary<(int row, int column), bool> movement = new Dictionary<(int row, int column), bool>()
            {
                { (cell.Row - 1, cell.Column), !filledCellArea.ContainsKey((cell.Row - 1, cell.Column))},
                { (cell.Row + 1, cell.Column), !filledCellArea.ContainsKey((cell.Row + 1, cell.Column))},
                { (cell.Row, cell.Column - 1), !filledCellArea.ContainsKey((cell.Row, cell.Column - 1))},
                { (cell.Row, cell.Column + 1), !filledCellArea.ContainsKey((cell.Row, cell.Column + 1))},
            };


            // single cell
            if (!filledCellArea.ContainsKey((cell.Row - 1, cell.Column))
                && !filledCellArea.ContainsKey((cell.Row + 1, cell.Column))
                && !filledCellArea.ContainsKey((cell.Row, cell.Column - 1))
                && !filledCellArea.ContainsKey((cell.Row, cell.Column + 1)))
            {
                availableCellArea.Add(new Cell() { Row = cell.Row + 1, Column = cell.Column, });
                availableCellArea.Add(new Cell() { Row = cell.Row - 1, Column = cell.Column, });
                availableCellArea.Add(new Cell() { Row = cell.Row, Column = cell.Column + 1, });
                availableCellArea.Add(new Cell() { Row = cell.Row, Column = cell.Column - 1, });
            }
            else
            {
                Cell? inCell, outCell;
                int randVal = new Random(Guid.NewGuid().GetHashCode()).Next();

                if (randVal % 3 != 1)
                {
                    // top
                    if (filledCellArea.TryGetValue((cell.Row - 1, cell.Column), out inCell) && inCell.Group == cell.Group)
                    {
                        movement[(cell.Row - 1, cell.Column)] = false;
                        if (!filledCellArea.TryGetValue((cell.Row + 1, cell.Column), out outCell))
                        {
                            if (cell.Row < size - 1)
                            {
                                availableCellArea.Add(new Cell() { Row = cell.Row + 1, Column = cell.Column, });
                            }
                            else
                            {
                                movement[(cell.Row + 1, cell.Column)] = false;
                            }
                        }
                    }
                    // bottom
                    if (filledCellArea.TryGetValue((cell.Row + 1, cell.Column), out inCell) && inCell.Group == cell.Group)
                    {
                        movement[(cell.Row + 1, cell.Column)] = false;
                        if (!filledCellArea.TryGetValue((cell.Row - 1, cell.Column), out outCell))
                        {
                            if (cell.Row > 0)
                            {
                                availableCellArea.Add(new Cell() { Row = cell.Row - 1, Column = cell.Column, });
                            }
                            else
                            {
                                movement[(cell.Row - 1, cell.Column)] = false;
                            }
                        }
                    }
                    // left
                    if (filledCellArea.TryGetValue((cell.Row, cell.Column - 1), out inCell) && inCell.Group == cell.Group)
                    {
                        movement[(cell.Row, cell.Column - 1)] = false;
                        if (!filledCellArea.TryGetValue((cell.Row, cell.Column + 1), out outCell))
                        {
                            if (cell.Column < size - 1)
                            {
                                availableCellArea.Add(new Cell() { Row = cell.Row, Column = cell.Column + 1, });
                            }
                            else
                            {
                                movement[(cell.Row, cell.Column + 1)] = false;
                            }
                        }
                    }
                    // right
                    if (filledCellArea.TryGetValue((cell.Row, cell.Column + 1), out inCell) && inCell.Group == cell.Group)
                    {
                        movement[(cell.Row, cell.Column + 1)] = false;
                        if (!filledCellArea.TryGetValue((cell.Row, cell.Column - 1), out outCell))
                        {
                            if (cell.Column > 0)
                            {
                                availableCellArea.Add(new Cell() { Row = cell.Row, Column = cell.Column - 1, });
                            }
                            else
                            {
                                movement[(cell.Row, cell.Column - 1)] = false;
                            }
                        }
                    }

                    // cannot move vertically
                    if (!movement[(cell.Row - 1, cell.Column)]
                        && !movement[(cell.Row + 1, cell.Column)])
                    {
                        if (!filledCellArea.ContainsKey((cell.Row, cell.Column - 1)))
                        {
                            availableCellArea.Add(new Cell() { Row = cell.Row, Column = cell.Column - 1, });
                        }
                        if (!filledCellArea.ContainsKey((cell.Row, cell.Column + 1)))
                        {
                            availableCellArea.Add(new Cell() { Row = cell.Row, Column = cell.Column + 1, });
                        }
                    }

                    // cannot move horizontally
                    if (!movement[(cell.Row, cell.Column - 1)]
                        && !movement[(cell.Row, cell.Column + 1)])
                    {
                        if (!filledCellArea.ContainsKey((cell.Row - 1, cell.Column)))
                        {
                            availableCellArea.Add(new Cell() { Row = cell.Row - 1, Column = cell.Column, });
                        }
                        if (!filledCellArea.ContainsKey((cell.Row + 1, cell.Column)))
                        {
                            availableCellArea.Add(new Cell() { Row = cell.Row + 1, Column = cell.Column, });
                        }
                    }
                }
                else
                {
                    if (movement[(cell.Row - 1, cell.Column)] && !movement[(cell.Row + 1, cell.Column)] && cell.Row > 0)
                    {
                        availableCellArea.Add(new Cell() { Row = cell.Row - 1, Column = cell.Column, });
                    }
                    if (movement[(cell.Row + 1, cell.Column)] && !movement[(cell.Row - 1, cell.Column)] && cell.Row < size - 1)
                    {
                        availableCellArea.Add(new Cell() { Row = cell.Row + 1, Column = cell.Column, });
                    }
                    if (movement[(cell.Row, cell.Column - 1)] && !movement[(cell.Row, cell.Column + 1)] && cell.Column > 0)
                    {
                        availableCellArea.Add(new Cell() { Row = cell.Row, Column = cell.Column - 1, });
                    }
                    if (movement[(cell.Row, cell.Column + 1)] && !movement[(cell.Row, cell.Column - 1)] && cell.Column < size - 1)
                    {
                        availableCellArea.Add(new Cell() { Row = cell.Row, Column = cell.Column + 1, });
                    }
                }
            }

            availableCellArea.ForEach(c => { c.Group = null; c.State = CellState.NOT_SELECTED; });

            return availableCellArea
                .Where(c => movement[(c.Row, c.Column)] 
                    && c.Row != -1 && c.Row != size 
                    && c.Column != -1 && c.Column != size)
                .ToList();
        }
    }
}
