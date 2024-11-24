using QueenGame.Game.Models;

namespace QueenGame.Game.GameRules.Rules
{
    public class NoDiagonalNearestRule : IRule
    {
        public bool CheckRule(IEnumerable<Cell> cells, int size)
        {
            foreach (Cell cell in cells)
            {
                if (cells.Any(v =>
                {
                    if (cell == v)
                    {
                        return false;
                    }

                    int dr = Math.Abs(v.Row - cell.Row);
                    int dc = Math.Abs(v.Column - cell.Column);

                    return dr == dc && dr <= 1 && dc <= 1;
                }))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
