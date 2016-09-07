using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HexagonService.Entity
{
    public class Player
    {
        public int Id { get; set; }
        public string PlayerId { get; set; }
        public string Username { get; set; }
        public DateTime RecordedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
    }
}