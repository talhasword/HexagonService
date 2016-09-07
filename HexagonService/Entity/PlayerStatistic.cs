using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HexagonService.Entity
{
    public class PlayerStatistic
    {
        public int Id { get; set; }
        public Player Player { get; set; }
        public int WonGame { get; set; }
        public int LostGame { get; set; }
        public DateTime LastUpdatetedTime { get; set; }
    }
}