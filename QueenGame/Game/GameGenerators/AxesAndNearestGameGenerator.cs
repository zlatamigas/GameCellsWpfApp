using Microsoft.EntityFrameworkCore.Metadata.Internal;
using QueenGame.Exceptions;
using QueenGame.Game.CellFieldFiller;
using QueenGame.Game.GameRules.RuleSets;
using QueenGame.Game.Models;
using QueenGame.Utils.AppColorConverter;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace QueenGame.Game.GameGenerator
{
    public class AxesAndNearestGameGenerator : IGameGenerator
    {
        private readonly BaseRuleSet _RuleSet;
        public BaseRuleSet RuleSet => _RuleSet;

        private Random rand;
        private ICellFieldFiller groupGenerator;

        public AxesAndNearestGameGenerator() : this(null) { }
        public AxesAndNearestGameGenerator(ICellFieldFiller? groupGenerator)
        {
            _RuleSet = new AxesAndNearestRuleSet();
            rand = new Random(Guid.NewGuid().GetHashCode()); // new System.DateTime().Millisecond
            this.groupGenerator = groupGenerator != null ? groupGenerator : new LinearGroupCellFieldFiller();
        }

        public Models.Game GenerateGameLevel(int size)
        {
            if (!CanGenerateGameLevel(size))
            {
                throw new GameGeneratorException($"Invalid size={size}");
            }

            Dictionary<int, Color> groupInfos = new Dictionary<int, Color>(size);
            List<Cell> answerCells = new List<Cell>();

            int group, row, column;
            HSL hsl;

            if (size > 4)
            {
                // Fill answer sequence


                int maxPart = size - size / 2;
                group = 0;
                for (int i = 0; i < size; i++)
                {
                    if (i % 2 == 0)
                    {
                        row = i / 2;
                    }
                    else
                    {
                        row = maxPart;
                        maxPart++;
                    }
                    column = i;
                    group++;

                    answerCells.Add(new Cell
                    {
                        Row = row,
                        Column = column,
                        Group = group,
                    });

                    hsl = new HSL(i * (360 / size),
                                  rand.Next(70, 100) * 0.01,
                                  rand.Next(40, 60) * 0.01);

                    groupInfos[group] = AppColorConverter.ToRGBFromHSL(hsl);
                }

                // Mix answer sequence

                for (int i = 0; i < rand.Next(10, 40); i++)
                {
                    switch (rand.Next(4))
                    {
                        case 0:
                            Transpose(answerCells);
                            break;
                        case 1:
                            SwapRows(answerCells);
                            break;
                        case 2:
                            SwapColumns(answerCells);
                            break;
                        case 3:
                            Rotate90(answerCells, size);
                            break;
                    }
                }
            }
            else if (size == 4)
            {
                (int row, int col)[] answers =
                {
                    (0, 1),
                    (1, 3),
                    (2, 0),
                    (3, 2),
                };

                // Fill answer sequence
                group = 0;
                for (int i = 0; i < answers.Length; i++)
                {
                    row = answers[i].row;
                    column = answers[i].col;
                    group++;

                    answerCells.Add(new Cell
                    {
                        Row = row,
                        Column = column,
                        Group = group,
                    });

                    hsl = new HSL(i * (360 / answers.Length),
                                    rand.Next(70, 100) * 0.01,
                                    rand.Next(40, 60) * 0.01);

                    groupInfos[group] = AppColorConverter.ToRGBFromHSL(hsl);
                }

                // Mix answer sequence

                if (rand.Next(2) == 0)
                {
                    Transpose(answerCells);
                }
            }


            // Fill groups

            IEnumerable<Cell> filledCells = groupGenerator.Fill(size, answerCells);

            Models.Game gameLevel = new Models.Game(null, size, GameType.AXES_DIAGONALONENEAREST, null,
                groupInfos, 
                filledCells);

            return gameLevel;
        }

        public bool CanGenerateGameLevel(int size)
        {
            if (size < 4 && size != 1)
            {
                return false;
            }

            return true;
        }

        // Matrix transformations


        private void Transpose(List<Cell> answerCells)
        {
            answerCells.ForEach(c =>
            {
                int tmp = c.Row;
                c.Row = c.Column;
                c.Column = tmp;
            });
        }

        private void SwapRows(List<Cell> answerCells)
        {
            bool isFinished = false;

            List<Cell> rows1ToCheck = answerCells.ToList();
            List<Cell> rowsChecked = new List<Cell>();

            while (!isFinished && rows1ToCheck.Count > 0)
            {
                Cell row1 = rows1ToCheck.ElementAt(rand.Next(rows1ToCheck.Count));
                rows1ToCheck.Remove(row1);

                int startPos = rand.Next(rows1ToCheck.Count);

                for (int i = startPos; i < startPos + rows1ToCheck.Count; i++)
                {
                    Cell row2 = rows1ToCheck.ElementAt(i % rows1ToCheck.Count);

                    bool invalidSwap = answerCells.Any(c =>
                    {
                        if (!(c.Row == row1.Row && c.Column == row1.Column)
                            && !(c.Row == row2.Row && c.Column == row2.Column)
                        )
                        {
                            return (Math.Abs(c.Row - row2.Row) == 1 && Math.Abs(c.Column - row1.Column) == 1)
                                || (Math.Abs(c.Row - row1.Row) == 1 && Math.Abs(c.Column - row2.Column) == 1);
                        }

                        return false;
                    });

                    if (!invalidSwap)
                    {
                        int tmp = row1.Row;
                        row1.Row = row2.Row;
                        row2.Row = tmp;

                        isFinished = true;
                        break;
                    }
                }

            }

        }

        private void SwapColumns(List<Cell> answerCells)
        {
            bool isFinished = false;

            List<Cell> columns1ToCheck = answerCells.ToList();
            List<Cell> columnsChecked = new List<Cell>();

            while (!isFinished && columns1ToCheck.Count > 0)
            {
                Cell column1 = columns1ToCheck.ElementAt(rand.Next(columns1ToCheck.Count));
                columns1ToCheck.Remove(column1);

                int startPos = rand.Next(columns1ToCheck.Count);

                for (int i = startPos; i < startPos + columns1ToCheck.Count; i++)
                {
                    Cell column2 = columns1ToCheck.ElementAt(i % columns1ToCheck.Count);

                    bool invalidSwap = answerCells.Any(c =>
                    {
                        if (!(c.Row == column1.Row && c.Column == column1.Column)
                            && !(c.Row == column2.Row && c.Column == column2.Column)
                        )
                        {
                            return (Math.Abs(c.Row - column1.Row) == 1 && Math.Abs(c.Column - column2.Column) == 1)
                                || (Math.Abs(c.Row - column2.Row) == 1 && Math.Abs(c.Column - column1.Column) == 1);
                        }

                        return false;
                    });

                    if (!invalidSwap)
                    {
                        int tmp = column1.Column;
                        column1.Column = column2.Column;
                        column2.Column = tmp;

                        isFinished = true;
                        break;
                    }
                }

            }

        }

        private void Rotate90(List<Cell> answerCells, int size)
        {
            answerCells.ForEach(c =>
            {
                int initRow = c.Row;
                int initCol = c.Column;

                c.Column = initRow;
                c.Row = size - initCol - 1;
            });
        }

    }
}
