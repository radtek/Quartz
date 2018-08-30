using Common.Logging;
using Quartz;
using Senparc.Weixin.MP.ISchedulers.ScheduleJob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ISchedulers.ScheduleJob
{
    public class SendMsgJob : IJob
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(SendMsgJob));



        /// <summary>
        /// 
        /// </summary>
        /// <param name="Template"></param>
        static void SendMsg()
        {
            Joson Joson = new Senparc.Weixin.MP.ISchedulers.ScheduleJob.Joson();
            Template Template = Template.流程催办提醒;
            Joson.SendMessage(Template);
        }



        public void Execute(IJobExecutionContext context)
        {

            String strMsg = "{0}SendMsgJob执行";

            //Joson Joson = new Senparc.Weixin.MP.ISchedulers.ScheduleJob.Joson();
            //strMsg += Joson.SendMessage() ? "成功" : "失败";
            //strMsg = String.Format(strMsg, DateTime.Now);

            Thread Thread = new Thread(SendMsg);
            Thread.Start();


            _logger.InfoFormat(strMsg);
            Console.WriteLine(strMsg);
        }

 
 

    }
}
