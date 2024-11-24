using QueenGame.Game.GameRules.Rules;
using QueenGame.Game.Models;

namespace QueenGame.Game.GameRules.RuleSets
{
    public class AxesRuleSet : BaseRuleSet
    {
        private Dictionary<CellState, IEnumerable<IRule>> _RulesForCellStateDict;
        protected override Dictionary<CellState, IEnumerable<IRule>> RulesForCellStateDict => _RulesForCellStateDict;

        public AxesRuleSet()
        {
            _RulesForCellStateDict = new Dictionary<CellState, IEnumerable<IRule>>
            {
                { 
                    CellState.SELECTED, 
                    new List<IRule>
                    {
                        new TotalCountRule(),
                        new CoverRowsRule(),
                        new CoverColumnsRule(),
                        new CoverGroupsRule(),
                    } 
                }
            };
        }
    }
}
