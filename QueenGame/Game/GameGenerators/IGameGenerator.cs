using QueenGame.Game.GameRules.RuleSets;

namespace QueenGame.Game.GameGenerator
{
    public interface IGameGenerator
    {
        public BaseRuleSet RuleSet { get; }

        public Task<Models.Game> GenerateGameLevelAsync(int size)
        {
            return Task<Models.Game>.Run<Models.Game>(() => { return GenerateGameLevel(size); });
        }

        public Models.Game GenerateGameLevel(int size);

        public bool CanGenerateGameLevel(int size);
    }
}
