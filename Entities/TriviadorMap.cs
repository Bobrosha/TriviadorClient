using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TriviadorClient.Entities
{
    public class TriviadorMap
    {
        public List<Cell> Cells { get; set; }
        public List<Player> Players { get; set; }

        public class Cell
        {
            public int Id { get; set; }
            public int Value { get; set; }
            public int? OwnerId { get; set; }
            public int Lvl { get; set; }
            public List<int> NearestCells { get; set; }
            public Castle Castle { get; set; }

            public Cell()
            {

            }

            public Cell(int id, int value, int ownerId, int lvl, List<int> nearestCells, Castle castle)
            {
                Id = id;
                Value = value;
                OwnerId = ownerId;
                Lvl = lvl;
                NearestCells = nearestCells;
                Castle = castle;
            }

            public Cell(int id, int value, int[] nearestCells, int lvl = 0)
            {
                Id = id;
                Value = value;
                Lvl = lvl;
                NearestCells = new List<int>();
                foreach (var el in nearestCells)
                {
                    NearestCells.Add(el);
                }

                OwnerId = null;
                Castle = null;
            }

            public void SetCastle(Castle castle)
            {
                Castle = castle;
                Value += castle.Value;
            }
        }

        public class Castle
        {
            public int Id { get; set; }
            public int OwnerId { get; set; }
            public int Lvl { get; set; }
            public int Hp { get; set; }
            public int Value { get; set; }

            [JsonConstructor]
            public Castle(int id, int ownerId, int value, int hp = 1000, int lvl = 0)
            {
                Id = id;
                OwnerId = ownerId;
                Value = value;
                Hp = hp;
                Lvl = lvl;
            }
        }

        public TriviadorMap()
        {
        }

        [JsonConstructor]
        public TriviadorMap(List<Cell> cells, List<Player> players)
        {
            Cells = cells;
            Players = players;
        }

        public TriviadorMap(bool flag)
        {
            if (flag)
            {
                Cells = CreateCellsList();
                Players = new List<Player>();
            }
        }

        public static List<Cell> CreateCellsList()
        {
            var list = new List<Cell>();

            //CreateCellAndAddInList(list, 1, new int[] { 2, 3, 4 });
            //CreateCellAndAddInList(list, 2, new int[] { 1, 3 });
            //CreateCellAndAddInList(list, 3, new int[] { 1, 2, 4 });
            //CreateCellAndAddInList(list, 4, new int[] { 1, 3, 6, 5 });
            //CreateCellAndAddInList(list, 5, new int[] { 4, 6, 7, 8 });
            //CreateCellAndAddInList(list, 6, new int[] { 1, 4, 5, 7, 8 });
            //CreateCellAndAddInList(list, 7, new int[] { 5, 6, 8, 9, 10, 11 });
            //CreateCellAndAddInList(list, 8, new int[] { 5, 6, 7, 11 });
            //CreateCellAndAddInList(list, 9, new int[] { 7, 10, 13 });
            //CreateCellAndAddInList(list, 10, new int[] { 7, 9, 11, 12, 13, 14 });
            //CreateCellAndAddInList(list, 11, new int[] { 7, 8, 10, 12 });
            //CreateCellAndAddInList(list, 12, new int[] { 10, 11, 14 });
            //CreateCellAndAddInList(list, 13, new int[] { 9, 10, 14 });
            //CreateCellAndAddInList(list, 14, new int[] { 10, 12, 13 });
            //CreateCellAndAddInList(list, 15, new int[] { 2, 3 });

            CreateCellAndAddInList(list, 1, new int[] { 2, 4 });
            CreateCellAndAddInList(list, 2, new int[] { 1, 3, 5 });
            CreateCellAndAddInList(list, 3, new int[] { 2, 6 });
            CreateCellAndAddInList(list, 4, new int[] { 1, 5, 7 });
            CreateCellAndAddInList(list, 5, new int[] { 2, 4, 6, 8 });
            CreateCellAndAddInList(list, 6, new int[] { 3, 5, 9 });
            CreateCellAndAddInList(list, 7, new int[] { 4, 8, 10 });
            CreateCellAndAddInList(list, 8, new int[] { 5, 7, 9, 11 });
            CreateCellAndAddInList(list, 9, new int[] { 6, 8, 12 });
            CreateCellAndAddInList(list, 10, new int[] { 7, 11, 13 });
            CreateCellAndAddInList(list, 11, new int[] { 8, 10, 12, 14 });
            CreateCellAndAddInList(list, 12, new int[] { 9, 11, 15 });
            CreateCellAndAddInList(list, 13, new int[] { 10, 14 });
            CreateCellAndAddInList(list, 14, new int[] { 11, 13, 15 });
            CreateCellAndAddInList(list, 15, new int[] { 12, 14 });

            InitializePlayerMap(list);

            return list;
        }

        public static void InitializePlayerMap(List<Cell> list)
        {
            list[2].OwnerId = 0;
            list[2].Value = 1000;

            list[12].OwnerId = 1;
            list[12].Value = 1000;
        }

        public static void CreateCellAndAddInList(List<Cell> list, int id, int[] arr, int score = 200)
        {
            list.Add(new Cell(id, score, arr));
        }
    }
}
