using QueenGame.DB.DTOs;
using QueenGame.Game.CellFieldFiller;
using QueenGame.Game.Models;
using QueenGame.Utils.AppColorConverter;
using System.Drawing;

namespace QueenGame.DB
{
    internal static class DbParser
    {
        private static readonly ICellFieldFiller cellFieldFiller = new UnavailableCellFieldFiller();


        public static GeneratedGameResult ParseGeneratedGameResult(DbGeneratedGameResult dbGeneratedGameResult)
        {
            return new GeneratedGameResult 
            { 
                Size = dbGeneratedGameResult.Size,
                Duration = dbGeneratedGameResult.Duration
            };
        }

        public static DbGeneratedGameResult ParseDbGeneratedGameResult(GeneratedGameResult GeneratedGameResult)
        {
            return new DbGeneratedGameResult 
            { 
                Size = GeneratedGameResult.Size,
                Duration = GeneratedGameResult.Duration
            };
        }


        public static GameInfo ParseGameLevelInfo(DbGame dbGameLevel)
        {
            GameInfo gameLevel = new GameInfo(
                dbGameLevel.GameId,
                dbGameLevel.Size,
                (GameType)dbGameLevel.GameType,
                dbGameLevel.FinishedGameDuration
            );

            return gameLevel;
        }

        public static Game.Models.Game ParseGameLevel(DbGame dbGameLevel)
        {
            Game.Models.Game gameLevel = new Game.Models.Game(
                dbGameLevel.GameId,
                dbGameLevel.Size,
                (GameType)dbGameLevel.GameType,
                dbGameLevel.FinishedGameDuration,
                ParseToGroups(dbGameLevel.GroupsDescriptionStr),
                ParseToCellMatrix(dbGameLevel.Size, dbGameLevel.CellsDesctiptionStr)
            );

            return gameLevel;
        }

     

        private static IEnumerable<Cell> ParseToCellMatrix(int size, string cellsDesctiptionStr)
        {
            return cellFieldFiller.Fill(size, ParseToCells(cellsDesctiptionStr));
        }

        private static IEnumerable<Cell> ParseToCells(string cellsDataStr)
        {
            List<Cell> cells = new List<Cell>();
            string[] cellInfo;


            string[] cellStrs = cellsDataStr.Trim(';').Split(';');
            foreach (string str in cellStrs)
            {
                cellInfo = str.Split(',');
                cells.Add(new Cell
                {
                    Row = int.Parse(cellInfo[0]),   // Row
                    Column = int.Parse(cellInfo[1]),    // Column
                    Group = cellInfo[2].ToLowerInvariant() == "null" ? null : int.Parse(cellInfo[2]), // Group
                    State = (CellState)Enum.Parse(typeof(CellState), cellInfo[3]),  // State
                    IsStateChangable = bool.Parse(cellInfo[4]), // IsStateChangable
                });
            }

            return cells;
        }

        public static IEnumerable<Cell> FillCellMatrix(int size, IEnumerable<Cell> srcCells)
        {
            List<Cell> matrix = new List<Cell>(size * size);

            List<Cell> cells = srcCells
                .DistinctBy(x => (x.Row, x.Column))
                .OrderBy(v => v.Row * size + v.Column)
                .ToList();

            Cell? headCell = cells.FirstOrDefault();
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    while (headCell != null
                        && (headCell.Row < 0
                            || headCell.Row >= size
                            || headCell.Column < 0
                            || headCell.Column >= size))
                    {
                        cells.Remove(headCell);
                        headCell = cells.FirstOrDefault();
                    }

                    if (headCell != null && headCell.Row == i && headCell.Column == j)
                    {
                        matrix.Add(headCell);
                        cells.Remove(headCell);
                        headCell = cells.FirstOrDefault();
                    }
                    else
                    {
                        matrix.Add(new Cell
                        {
                            Row = i,
                            Column = j,
                            Group = -1,
                            State = CellState.UNAVAILABLE,
                        });
                    }
                }
            }

            return matrix;
        }

        private static Dictionary<int, Color> ParseToGroups(string groupsDesctiptionStr)
        {
            Dictionary<int, Color> groups = new Dictionary<int, Color>();

            string[] groupInfo;


            string[] groupStrs = groupsDesctiptionStr.Trim(';').Split(';');
            foreach (string str in groupStrs)
            {
                groupInfo = str.Split(',');


                groups[int.Parse(groupInfo[0])] = AppColorConverter.ToRGBFromHexStr(groupInfo[1]);
            }

            return groups;
        }


    }
}
