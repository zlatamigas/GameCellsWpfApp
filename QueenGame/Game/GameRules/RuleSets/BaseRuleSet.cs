using QueenGame.Game.GameRules.Rules;
using QueenGame.Game.Models;

namespace QueenGame.Game.GameRules.RuleSets
{
     abstract public class BaseRuleSet
     {
        abstract protected Dictionary<CellState, IEnumerable<IRule>> RulesForCellStateDict { get; }

        public bool CheckRuleSet(IEnumerable<Cell> cellField, int fieldSize) 
        {
            return  RulesForCellStateDict == null 
                || !RulesForCellStateDict.ToList()
                .SelectMany(ruleSet =>
                {
                    IEnumerable<Cell> cellsInState = cellField.Where(c => c.State == ruleSet.Key);
                    return ruleSet.Value.Select(r => r.CheckRule(cellsInState, fieldSize));
                })
                .Where(r => r == false)
                .Any();
        }
    }
}
