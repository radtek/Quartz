using Quartz;
using Quartz.Entity;
using Senparc.Weixin.MP.ISchedulers.ScheduleJob;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace ISchedulers.ScheduleJob
{

    /// <summary>
    /// 工作类
    /// </summary>
    public class HandleCreateMessagesQueues : IJob
    {
        /// <summary>
        /// 任务调用的方法
        /// </summary>
        public void Execute(IJobExecutionContext context)
        {
            DateTimeOffset runTime = DateBuilder.EvenMinuteDate(DateTimeOffset.UtcNow);

            int rowsAffected = 0;
            bool isSucessful = false;

            Net.DBUtility.DbHelperSQL.RunProcedure("CreateMessagesQueues", out rowsAffected);

            isSucessful = rowsAffected > -1;

            Console.WriteLine(isSucessful + "CreateMessagesQueues， JosonJiang  -- start at 0:00   !" + runTime);

        }

    }



    /// <summary>
    /// 工作类
    /// </summary>
    public class HandleOAuthsEmpUpDate : IJob
    {
        /// <summary>
        /// 任务调用的方法
        /// </summary>
        public void Execute(IJobExecutionContext context)
        {
            DateTimeOffset runTime = DateBuilder.EvenMinuteDate(DateTimeOffset.UtcNow);

            int rowsAffected = 0;
            bool isSucessful = false;

            System.Data.IDataParameter[] DataParmameter = { new SqlParameter("@Task", "Task"), new SqlParameter("@RunAt", "RunAt") };
            SqlParameter[] parameters = { new SqlParameter("@Task", "Task"), new SqlParameter("@RunAt", "RunAt") };

            Net.DBUtility.DbHelperSQL.RunProcedure("CreateMessagesQueues", out rowsAffected);

            Net.DBUtility.DbHelperSQL.RunProcedure("OAuthsEmpUpDate", parameters, out rowsAffected);
            Console.WriteLine((rowsAffected > -1) + "同步通讯录， JosonJiang  -- !" + runTime);

            Net.DBUtility.DbHelperSQL.RunProcedure("AddressBooksUpDate", DataParmameter, out rowsAffected);
            isSucessful = rowsAffected > -1;

            Console.WriteLine(isSucessful + "同步通讯录， JosonJiang  -- start at 7:30 every day 执行1次 !" + runTime);

        }

    }






    /// <summary>
    /// HandleSendMsg
    /// 工作类
    /// </summary>
    public class HandleSendMsg : IJob
    {
        /// <summary>
        /// 任务调用的方法
        /// </summary>
        public void Execute(IJobExecutionContext context)
        {


            //String strMsg = "{0}SendMsgJob执行";
            //Joson Joson = new Senparc.Weixin.MP.ISchedulers.ScheduleJob.Joson();
            //strMsg += Joson.SendMessage() ? "成功" : "失败";
            //strMsg = String.Format(strMsg, DateTime.Now);

            Thread Thread = new Thread(SendMsg);
            Thread.Start();


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Template"></param>
        public static void SendMsg()
        {

            Joson Joson = new Senparc.Weixin.MP.ISchedulers.ScheduleJob.Joson();
            Template Template = Template.流程催办提醒;
            Joson.SendMessage(Template);

        }


    }



    /// <summary>
    /// 工作类
    /// </summary>
    public class HandleFlagClass : IJob
    {
        /// <summary>
        /// 任务调用的方法
        /// </summary>
        public void Execute(IJobExecutionContext context)
        {
            Console.WriteLine("10s 执行一次 这是运行标示 --JosonJiang !");
        }

    }


    /// <summary>
    /// 要调度的功能模块
    /// </summary>
    public class WriteLogJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            string fileLogPath = AppDomain.CurrentDomain.BaseDirectory + "log\\";
            string fileLogName = "TQuartz_" + DateTime.Now.ToLongDateString() + "_log.txt";

            DirectoryInfo Dir = new DirectoryInfo(fileLogPath);

            if (!Dir.Exists)
            {
                Dir.Create();
            }

            try
            {
                String FileName = fileLogPath + fileLogName;
                //FileInfo finfo = new FileInfo(FileName);

                StreamWriter strwriter = File.AppendText(FileName);
                strwriter.WriteLine("JosnJiang 的标示时间发生在: " + DateTime.Now.ToString());
                strwriter.Flush();
                strwriter.Close();

            }
            catch (Exception ex)
            {

                String Message = "发生错误:{0}";
                Message = String.Format(Message, ex.GetBaseException().Message + Environment.NewLine + ex.StackTrace);



                //寫入事件撿視器,方法一  
                if (!EventLog.SourceExists("JosonJiang Applications"))
                    EventLog.CreateEventSource("JosonJiang Applications", "JosonJiang");
                System.Diagnostics.EventLog.WriteEntry("JosonJiang Applications", Message, System.Diagnostics.EventLogEntryType.Warning);


                Message = String.Format("{0}", ex.GetBaseException().Message);
                System.Diagnostics.EventLog log = new System.Diagnostics.EventLog("JosonJiang");
                //  首先应判断日志来源是否存在，一个日志来源只能同时与一个事件绑定

                if (!EventLog.SourceExists("JosonJiang Application"))
                    EventLog.CreateEventSource("JosonJiang Application", "JosonJiang");
                try
                {
                    log.Source = "JosonJiang Application";
                    log.WriteEntry(Message, EventLogEntryType.FailureAudit);

                    //throw new System.IO.FileNotFoundException("readme.txt文件未找到");
                }
                catch (System.IO.FileNotFoundException exception)
                {
                    log.WriteEntry(exception.Message, EventLogEntryType.Error);
                }


                //FileInfo finfo = new FileInfo(fileLogPath + fileLogName);
                //using (FileStream fs = finfo.OpenWrite())
                //{
                //    //根据上面创建的文件流创建写数据流 
                //    using (StreamWriter strwriter = new StreamWriter(fs))
                //    {
                //        //设置写数据流的起始位置为文件流的末尾 
                //        strwriter.BaseStream.Seek(0, SeekOrigin.End);
                //        //写入相关记录信息
                //        strwriter.WriteLine("发生时间: " + ex.Message + DateTime.Now.ToString());
                //        strwriter.WriteLine("---------------------------------------------");
                //        strwriter.WriteLine();
                //        //清空缓冲区内容，并把缓冲区内容写入基础流 
                //        strwriter.Flush();
                //        strwriter.Close();
                //        fs.Close();

                //    }
                //}

            }

        }
    }


}
