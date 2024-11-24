using QueenGame.Game.Models;

namespace QueenGame.Game.GameRules.Rules
{
    public class TotalCountRule : IRule
    {
        public bool CheckRule(IEnumerable<Cell> cells, int size)
        {
            return cells.Count() == size;
        }
    }
}
