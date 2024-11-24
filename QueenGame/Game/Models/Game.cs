using QueenGame.Factories;
using QueenGame.Game.GameRules.RuleSets;
using System.Drawing;

namespace QueenGame.Game.Models
{
    public class Game
    {
        private BaseRuleSet? ruleSet;

        public GameInfo GameInfo { get; }


        private Dictionary<int, Color> _Groups;
        public IEnumerable<KeyValuePair<int, Color>> Groups => _Groups.ToList();

        public Color? GetGroupColor(int groupId)
        {
            if (_Groups != null && _Groups.ContainsKey(groupId))
            {
                return _Groups[groupId];
            }
            return null;
        }

        private List<Cell> _GameField;
        public IEnumerable<Cell> GameField => _GameField.ToList();

        public void SetGameFieldToDefault() 
        {
            foreach (var cell in GameField) 
            {
                if (cell.IsStateChangable) 
                { 
                    cell.State = CellState.NOT_SELECTED;
                }
            }
        }

        public bool IsSuccessfulField => ruleSet?.CheckRuleSet(GameField, GameInfo.Size) ?? true;


        public Game(int? gameId, int size, GameType gameType, int? finishedGameDuration, Dictionary<int, Color> groups, IEnumerable<Cell> gameField)
        {
            GameInfo = new GameInfo(gameId, size, gameType, finishedGameDuration);

            ruleSet = RuleSetFactory.GetRuleSet(gameType);
            _Groups = groups != null ? groups : new Dictionary<int, Color>(size);
            _GameField = gameField != null ? gameField.ToList() : new List<Cell>(size * size);
        }

        public void ChangeCellState(int row, int column, CellState newCellState) 
        {
            Cell? cell = _GameField.FirstOrDefault(c => c.Row == row && c.Column == column);
            if ( cell != null)
            {
                cell.State = newCellState;
            }
        }
    }
}
