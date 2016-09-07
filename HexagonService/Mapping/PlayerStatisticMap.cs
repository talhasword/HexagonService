using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Mapping;
using HexagonService.Entity;

namespace HexagonService.Mapping
{
    public class PlayerStatisticMap:ClassMap<PlayerStatistic>
    {
        public PlayerStatisticMap()
        {
            Table("PlayerStatistic");
            Id(x=>x.Id).Column("Id").GeneratedBy.Identity();
            //Map(x=>x.PlayerId).Column("PlayerId");
            References(x => x.Player).Column("PlayerId");
            Map(x=>x.WonGame).Column("WonGame");
            Map(x=>x.LostGame).Column("LostGame");
            Map(x=>x.LastUpdatetedTime).Column("LastUpdatetedTime");
        }
    }
}