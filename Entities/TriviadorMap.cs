using System.Collections.Generic;

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
        }

        public class Castle
        {
            public int Id { get; set; }
            public Player Owner { get; set; }
            public int Lvl { get; set; }
            public int Hp { get; set; }
            public int Value { get; set; }
        }
    }
}
