using Common.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISchedulers.ScheduleJob
{
    public class DataUpdateJob:IJob
    {

        private readonly ILog _logger = LogManager.GetLogger(typeof(DataUpdateJob));

        public void Execute(IJobExecutionContext context)
        {
            _logger.InfoFormat("同步通讯录");

            DateTimeOffset runTime = DateBuilder.EvenMinuteDate(DateTimeOffset.UtcNow);

            int flag = 0;
            bool isSucessful = false;

            System.Data.IDataParameter[] DataParmameter = { new SqlParameter("@Task", "Task"), new SqlParameter("@RunAt", "RunAt") };

            SqlParameter[] parameters = { new SqlParameter("@Task", "Task"), new SqlParameter("@RunAt", "RunAt") };


            Net.DBUtility.DbHelperSQL.RunProcedure("OAuthsEmpUpDate", parameters, out flag);
            Net.DBUtility.DbHelperSQL.RunProcedure("AddressBooksUpDate", DataParmameter, out flag);

            isSucessful = flag == -1;

            Console.WriteLine(isSucessful + "同步通讯录， JosonJiang  -- start at 7:30 every day 执行1次 !" + runTime);
   

           
        }


    }
}
