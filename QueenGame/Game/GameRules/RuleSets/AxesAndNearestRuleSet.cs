using QueenGame.Game.GameRules.Rules;
using QueenGame.Game.Models;

namespace QueenGame.Game.GameRules.RuleSets
{
    public class AxesAndNearestRuleSet : BaseRuleSet
    {
        private Dictionary<CellState, IEnumerable<IRule>> _RulesForCellStateDict;
        protected override Dictionary<CellState, IEnumerable<IRule>> RulesForCellStateDict => _RulesForCellStateDict;
    
        public AxesAndNearestRuleSet()
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
                        new NoDiagonalNearestRule(),
                    }
                }
            };
        }

    }
}
