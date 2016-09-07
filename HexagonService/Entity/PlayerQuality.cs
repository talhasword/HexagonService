using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HexagonService.Entity
{
    public class PlayerQuality
    {
        public int Id { get; set; }
        public Player Player { get; set; }
        public string Clour { get; set; }
        public DateTime RecordedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
    }
}