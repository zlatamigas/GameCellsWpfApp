using QueenGame.Game.GameRules.RuleSets;
using QueenGame.Game.Models;

namespace QueenGame.Factories
{
    public static class RuleSetFactory
    {
        private static Dictionary<GameType, BaseRuleSet> RuleSetDict
            = new Dictionary<GameType, BaseRuleSet>
        {
            { GameType.AXES, new AxesRuleSet() },
            { GameType.AXES_DIAGONALONENEAREST, new AxesAndNearestRuleSet() },
        };

        public static BaseRuleSet? GetRuleSet(GameType gameType)
        {
            return RuleSetDict.TryGetValue(gameType, out BaseRuleSet? r) ? r : null;
        }
    }
}
