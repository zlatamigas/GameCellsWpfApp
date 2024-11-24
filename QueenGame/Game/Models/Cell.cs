namespace QueenGame.Game.Models
{
    public class Cell
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public int? Group { get; set; }
        public CellState State { get; set; }

        public bool IsStateChangable { get; set; } = true;

        public override string? ToString()
        {
            return $"Cell[({Row}; {Column}), Group={Group}, State={State}]";
        }
    }
}
