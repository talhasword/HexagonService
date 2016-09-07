using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace HexagonService.Scheduler
{
    public class D_CONVERTJob : Job
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public override string GetName()
        {
            return this.GetType().Name;
        }

        public override void DoJob()
        {
            //SchedulerActions.Instance.ConvertProcessRun();
        }

        public override bool IsRepeatable()
        {
            return true;
        }

        public override int GetRepetitionIntervalTime()
        {
            return Convert.ToInt32(ConfigurationManager.AppSettings["SchedulerPeriod"]);
        }
    }
}