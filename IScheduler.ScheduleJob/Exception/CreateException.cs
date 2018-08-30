using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Senparc.Weixin.MP.ISchedulers.ScheduleJob
{
    public class JosonException : System.Exception
    {

        public static void CreateException<T>(Exception ex, T NotifyRec)
        {


            String Message = "发生错误:{0}{1}";
            Message = String.Format(Message, ex.GetBaseException().Message + Environment.NewLine + ex.StackTrace, Environment.NewLine + NotifyRec.ToString());



            //寫入事件撿視器,方法一  
            if (!EventLog.SourceExists("JosonJiang Applications"))
                EventLog.CreateEventSource("JosonJiang Applications", "JosonJiang");
            System.Diagnostics.EventLog.WriteEntry("JosonJiang Applications", Message, System.Diagnostics.EventLogEntryType.Error);


            Message = String.Format("{0}", ex.GetBaseException().Message + Environment.NewLine + NotifyRec.ToString() + Environment.NewLine + ex.StackTrace);
            System.Diagnostics.EventLog log = new System.Diagnostics.EventLog("JosonJiang");
            //  首先应判断日志来源是否存在，一个日志来源只能同时与一个事件绑定

            if (!EventLog.SourceExists("JosonJiang Application"))
                EventLog.CreateEventSource("JosonJiang Application", "JosonJiang");
            try
            {
                log.Source = "JosonJiang Application";
                log.WriteEntry(Message, EventLogEntryType.Error);

                //throw new System.IO.FileNotFoundException("readme.txt文件未找到");
            }
            catch (System.IO.FileNotFoundException exception)
            {
                log.WriteEntry(exception.Message, EventLogEntryType.Error);
            }


        }


    }
}
