using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper.Models
{
    public class MinesweeperStatistics
    {
        public int TotalCountGames { get; set; }
        public int WinsGames { get; set; }
        public int LostGames { get; set; }
        public int BeginnerWinsGames { get; set; }
        public int IntermediateWinsGames { get; set; }
        public int ExpertWinsGames { get; set; }
        public int BestTime { get; set; }
    }
}
