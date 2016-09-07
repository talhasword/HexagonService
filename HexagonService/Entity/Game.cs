using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HexagonService.Enums;

namespace HexagonService.Entity
{
    public class Game
    {
        public int Id { get; set; }
        public Player FirstPlayer { get; set; }
        public Player SecondPlayer { get; set; }
        public int Score { get; set; }
        public int WonPlayer { get; set; }
        public int Status { get; set; }
        public DateTime RecordedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        public DateTime FinishedTime { get; set; }


        public GameStatus GameStatus
        {
            get
            {
                return (GameStatus)this.Status;
            }
            set
            {
                this.Status = (int)value;
            }
        }
    }
}