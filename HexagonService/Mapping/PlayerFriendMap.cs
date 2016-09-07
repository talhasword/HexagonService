using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Mapping;
using HexagonService.Entity;

namespace HexagonService.Mapping
{
    public class PlayerFriendMap : ClassMap<PlayerFriend>
    {
        public PlayerFriendMap()
        {
            Table("Game");
            Id(X => X.Id).Column("Id").GeneratedBy.Identity();
            Map(x => x.PlayerId).Column("PlayerId");
            Map(x => x.FriendId).Column("FriendId");
        }
    }
}