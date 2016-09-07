using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Mapping;
using HexagonService.Entity;

namespace HexagonService.Mapping
{
    public class PlayerQualityMap:ClassMap<PlayerQuality>
    {
        public PlayerQualityMap()
        {
            Table("PlayerQuality");
            Id(x=>x.Id).Column("Id").GeneratedBy.Identity();
            //Map(x=>x.PlayerId).Column("PlayerId");
            References(x => x.Player).Column("PlayerId");
            Map(x=>x.Clour).Column("Clour");
            Map(x=>x.RecordedTime).Column("RecordedTime");
            Map(x=>x.UpdatedTime).Column("UpdatedTime");
        }
    }
}