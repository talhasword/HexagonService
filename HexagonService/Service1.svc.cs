using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using HexagonService.Actions;
using HexagonService.Entity;
using HexagonService.Enums;

namespace HexagonService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        //public string GetData(int value)
        //{
        //    return string.Format("You entered: {0}", value);
        //}

        //public CompositeType GetDataUsingDataContract(CompositeType composite)
        //{
        //    if (composite == null)
        //    {
        //        throw new ArgumentNullException("composite");
        //    }
        //    if (composite.BoolValue)
        //    {
        //        composite.StringValue += "Suffix";
        //    }
        //    return composite;
        //}

        public bool RegisterPlayer(string playerId, string username)
        {
            try
            {
                return DBActions.Instance.RegisterPlayer(playerId, username);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public string SendToMessage(string playerId, string username)
        {
            return "talha";
        }

        public void RegisterGame(string firstPlayerUsername, string secondPlayerUsername, GameStatus status, int wonPlayer, int score)
        {
            Player firstPlayer = DBActions.Instance.GetPlayerByUsername(firstPlayerUsername);
            Player secondPlayer = DBActions.Instance.GetPlayerByUsername(secondPlayerUsername);
            DBActions.Instance.RegisterGame(firstPlayer, secondPlayer, status, wonPlayer, score);

        }
    }
}
