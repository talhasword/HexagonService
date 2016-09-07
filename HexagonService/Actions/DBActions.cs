using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HexagonService.DAL;
using HexagonService.Entity;
using HexagonService.Enums;
using NHibernate;
using NHibernate.Criterion;

namespace HexagonService.Actions
{
    public class DBActions
    {
        #region declare

        private ISessionFactory SessionFactory { get; set; }

        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private DBActions()
        {
            SessionFactory = HexagonService.Config.FluentlyConfig.Instance.CreateSessionFactory();
        }

        public static DBActions Instance
        {
            get
            {
                return DBActionsFactory.instance;
            }
        }
        private class DBActionsFactory
        {
            static DBActionsFactory() { }
            internal static readonly DBActions instance = new DBActions();
        }

        #endregion

        public Player GetPlayerByPlayerId(string playerId)
        {
            using (UnitOfWork uow = new UnitOfWork(this.SessionFactory))
            {
                try
                {
                    var resp = uow.Session.CreateCriteria<Player>("plyr")
                        .Add(Restrictions.Eq("plyr.playerid", playerId))
                        .List<Player>();

                    return resp.SingleOrDefault();
                }
                catch (Exception ex)
                {
                    logger.Fatal("Hata", ex);
                    uow.RollBack();
                    throw;
                }
            }
        }

        public Player GetPlayerByPlayerId(string playerId, UnitOfWork uow)
        {
            try
            {
                var resp = uow.Session.CreateCriteria<Player>("plyr")
                    .Add(Restrictions.Eq("plyr.PlayerId", playerId))
                    .List<Player>();

                return resp.SingleOrDefault();
            }
            catch (Exception ex)
            {
                logger.Fatal("Hata", ex);
                uow.RollBack();
                throw;
            }
        }

        public Player GetPlayerByUsername(string username)
        {
            using (UnitOfWork uow = new UnitOfWork(this.SessionFactory))
            {
                try
                {
                    var resp = uow.Session.CreateCriteria<Player>("plyr")
                        .Add(Restrictions.Eq("plyr.Username", username))
                        .List<Player>();

                    return resp.SingleOrDefault();
                }
                catch (Exception ex)
                {
                    logger.Fatal("Hata", ex);
                    uow.RollBack();
                    throw;
                }
            }
        }

        public Player GetPlayerByUsername(string username, UnitOfWork uow)
        {
            try
            {
                var resp = uow.Session.CreateCriteria<Player>("plyr")
                    .Add(Restrictions.Eq("plyr.Username", username))
                    .List<Player>();

                return resp.SingleOrDefault();
            }
            catch (Exception ex)
            {
                logger.Fatal("Hata", ex);
                uow.RollBack();
                throw;
            }
        }

        public bool RegisterPlayer(string playerId, string username)
        {
            using (UnitOfWork uow = new UnitOfWork(this.SessionFactory))
            {
                Player plyr = GetPlayerByUsername(username, uow);

                if (plyr == null)
                {
                    plyr = new Player();
                    plyr.PlayerId = playerId;
                    plyr.Username = username;
                    plyr.RecordedTime = DateTime.Now;
                    plyr.UpdatedTime = DateTime.Now;
                }
                else
                {
                    if (!string.IsNullOrEmpty(username))
                    {
                        plyr.Username = username;
                        plyr.UpdatedTime = DateTime.Now;
                    }
                }
                uow.Session.Save(plyr);
                uow.Commit();
                return true;
            }
        }

        public Game GetGame(int firstPlayerId, int secondPlayerId, GameStatus status)
        {
            try
            {
                using (UnitOfWork uow = new UnitOfWork(this.SessionFactory))
                {
                    var resp = uow.Session.CreateCriteria<Game>("game")
                        .Add(Restrictions.Eq("game.FirstPlayerId", firstPlayerId))
                        .Add(Restrictions.Eq("game.SecondPlayerId", secondPlayerId))
                        .List<Game>();

                    return resp.SingleOrDefault();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Game GetActiveGame(Player firstPlayerId, Player secondPlayerId, GameStatus status, UnitOfWork uow)
        {
            try
            {
                var resp = uow.Session.CreateCriteria<Game>("game")
                    .Add(Restrictions.Eq("game.FirstPlayer", firstPlayerId))
                    .Add(Restrictions.Eq("game.SecondPlayer", secondPlayerId))
                   // .Add(Restrictions.Eq("game.Status", status))
                    .List<Game>();

                return resp.SingleOrDefault();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Game GetGameById(int gameId)
        {
            try
            {
                using (UnitOfWork uow = new UnitOfWork(this.SessionFactory))
                {
                    var resp = uow.Session.CreateCriteria<Game>("game")
                        .Add(Restrictions.Eq("game.id", gameId))
                        .List<Game>();

                    return resp.SingleOrDefault();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Game GetGameById(int gameId, UnitOfWork uow)
        {
            try
            {
                var resp = uow.Session.CreateCriteria<Game>("game")
                    .Add(Restrictions.Eq("game.id", gameId))
                    .List<Game>();

                return resp.SingleOrDefault();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int RegisterGame(Player firstPlayerId, Player secondPlayerId, GameStatus status, int wonPlayer, int score)
        {
            try
            {
                using (UnitOfWork uow = new UnitOfWork(this.SessionFactory))
                {
                    Game game = GetActiveGame(firstPlayerId, secondPlayerId, GameStatus.Playing, uow);

                    if (game == null)
                    {
                        game = new Game();
                        game.FirstPlayer = firstPlayerId;
                        game.SecondPlayer = secondPlayerId;
                        game.Score = score;
                        game.Status = (int)GameStatus.Start;
                        game.WonPlayer = wonPlayer;
                        game.RecordedTime = DateTime.Now;
                        game.UpdatedTime = DateTime.Now;
                        game.FinishedTime = DateTime.Now;
                        uow.Session.Save(game);
                        uow.Commit();
                        return game.Id;
                    }
                    return -1;
                }
            }
            catch (Exception)
            {
                return -1;
            }
        }
    }
}