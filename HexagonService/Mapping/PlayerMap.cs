using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Mapping;
using HexagonService.Entity;

namespace HexagonService.Mapping
{
    public class PlayerMap : ClassMap<Player>
    {
        public PlayerMap()
        {
            Table("Player");
            Id(x => x.Id).Column("id").GeneratedBy.Identity();
            Map(x => x.PlayerId).Column("playerid");
            Map(x => x.Username).Column("username");
            Map(x => x.RecordedTime).Column("recordedtime");
            Map(x => x.UpdatedTime).Column("updatedtime");
        }
    }
}