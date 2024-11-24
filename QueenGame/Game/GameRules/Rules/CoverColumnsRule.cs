using QueenGame.Game.Models;

namespace QueenGame.Game.GameRules.Rules
{
    public class CoverColumnsRule : IRule
    {
        public bool CheckRule(IEnumerable<Cell> cells, int size)
        {
            return cells.DistinctBy(v => v.Column).Count() == size;
        }
    }
}
