using QueenGame.Game.Models;

namespace QueenGame.Game.GameRules.Rules
{
    public class CoverGroupsRule : IRule
    {
        public bool CheckRule(IEnumerable<Cell> cells, int size)
        {
            return cells.DistinctBy(v => v.Group).Count() == size;
        }
    }
}
