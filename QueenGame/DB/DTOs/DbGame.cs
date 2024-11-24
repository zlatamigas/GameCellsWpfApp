using System.ComponentModel.DataAnnotations;

namespace QueenGame.DB.DTOs
{
    public class DbGame
    {
        [Key]
        public int GameId { get; set; }

        // Rows = Columns = Groups (участвующие в игре)
        public int Size { get; set; }

        // ...;Id,Color;... - colors in hex: #HHHHHH
        public string GroupsDescriptionStr { get; set; }

        // ...;Row,Column,Group,State,IsStateChangable;...
        public string CellsDesctiptionStr { get; set; }

        public int GameType { get; set; }

        public int? FinishedGameDuration { get; set; }
    }
}
