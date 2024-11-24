using QueenGame.Exceptions;
using QueenGame.Game.CellFieldFiller;
using QueenGame.Game.GameRules.RuleSets;
using QueenGame.Game.Models;
using QueenGame.Utils.AppColorConverter;
using System.Drawing;

namespace QueenGame.Game.GameGenerator
{
    public class AxesGameGenerator : IGameGenerator
    {
        private readonly BaseRuleSet _RuleSet;
        public BaseRuleSet RuleSet => _RuleSet;


        private Random rand;
        private ICellFieldFiller groupGenerator;

        public AxesGameGenerator(): this(null) { }
        public AxesGameGenerator(ICellFieldFiller? groupGenerator)
        {
            _RuleSet = new AxesRuleSet();
            rand = new Random(Guid.NewGuid().GetHashCode()); // new System.DateTime().Millisecond
            this.groupGenerator = groupGenerator != null ? groupGenerator : new LinearGroupCellFieldFiller();
        }

        public Models.Game GenerateGameLevel(int size)
        {
            if (!CanGenerateGameLevel(size))
            {
                throw new GameGeneratorException($"Invalid size={size}");
            }

            // Fill answer sequence

            Dictionary<int, Color> groupInfos = new Dictionary<int, Color>(size);
            List<Cell> answerCells = new List<Cell>();

            List<int> rows = Enumerable.Range(0, size).ToList();
            List<int> columns = Enumerable.Range(0, size).ToList();

            int group, row, column;
            HSL hsl;

            for (int i = 0; i < size; i++)
            {
                group = i + 1;
                row = rows.ElementAt(rand.Next(0, size - i));
                column = columns.ElementAt(rand.Next(0, size - i));

                answerCells.Add(new Cell
                {
                    Row = row,
                    Column = column,
                    Group = group,
                });

                rows.Remove(row);
                columns.Remove(column);

                hsl = new HSL(i * (360 / size),
                              rand.Next(70, 100) * 0.01,
                              rand.Next(40, 60) * 0.01);

                groupInfos[group] = AppColorConverter.ToRGBFromHSL(hsl);
            }

            // Fill groups

            IEnumerable<Cell> filledCells = groupGenerator.Fill(size, answerCells);

            Models.Game gameLevel = new Models.Game(null, size, GameType.AXES, null,
                groupInfos, 
                filledCells);

            return gameLevel;
        }


        public bool CanGenerateGameLevel(int size)
        {
            if (size < 1)
            {
                return false;
            }

            return true;
        }
    }
}
