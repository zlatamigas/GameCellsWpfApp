using QueenGame.Game.Models;

namespace QueenGame.Game.GameRules.Rules
{
    public interface IRule
    {
        public bool CheckRule(IEnumerable<Cell> cells, int size);
    }
}
