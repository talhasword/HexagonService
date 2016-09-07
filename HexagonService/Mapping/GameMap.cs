using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Mapping;
using HexagonService.Entity;

namespace HexagonService.Mapping
{
    public class GameMap : ClassMap<Game>
    {
        public GameMap()
        {
            Table("Game");
            Id(x => x.Id).Column("id").GeneratedBy.Identity();
            //Map(x => x.FirstPlayerId).Column("FirstPlayerId");
            //Map(x => x.SecondPlayerId).Column("SecondPlayerId");
            References(x => x.FirstPlayer).Column("firstplayerid");
            References(x => x.SecondPlayer).Column("secondplayerid");
            Map(x => x.Score).Column("score");
            Map(x => x.WonPlayer).Column("wonplayer");
            Map(x => x.Status).Column("status");
            Map(x => x.RecordedTime).Column("recordedtime");
            Map(x => x.UpdatedTime).Column("updatedtime");
            Map(x => x.FinishedTime).Column("finishedtime");
        }
    }
}