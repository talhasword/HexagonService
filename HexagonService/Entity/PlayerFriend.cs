using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HexagonService.Entity
{
    public class PlayerFriend
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public int FriendId { get; set; }
    }
}