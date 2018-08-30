using Quartz.Entity;
using Senparc.Weixin.BLL;
using Senparc.Weixin.IBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Net.Common;
using System.Threading;
using System.Diagnostics;

namespace Quartz.Consoles
{


    public class Start
    {

        static String 接受者 = String.Empty;
        static String 微信内容 = String.Empty;
        static String 信息内容 = String.Empty;
        static String strMsg = String.Empty;
        static Boolean IsSendSuecssful = false;

        static IBPMSysMessagesQueueService _IBPMSysMessagesQueueService = new BPMSysMessagesQueueService();
        // IBPMSysMessagesSucceedService _IBPMSysMessagesSucceedService = new BPMSysMessagesSucceedService();

        //  IBPMSysSettingsService _IBPMSysSettingsService = new BPMSysSettingsService();
        static IQueryable<BPMSysMessagesQueue> IQBPMSysMessagesQueue = null;


        static int SendSuecssfulCounts = 0, taskCounts = 1, taskPageSize = 1000;
        static int QueueCounts = 0, Counts = 0, wCounts = 0, CurrentQueueCounts = 0;

        /// <summary>
        /// 子线程
        /// </summary>
        public static String SendMessagesQueue(Boolean isDebug = true)
        {
            #region SendMessagesQueue


                try
                {

                    IQBPMSysMessagesQueue = _IBPMSysMessagesQueueService.LoadPageEntities(1, taskPageSize, out QueueCounts, o => !Object.Equals(o.Title, null), true, o => o.MessageID);

                    foreach (BPMSysMessagesQueue MessagesQueue in IQBPMSysMessagesQueue.ToList())
                    {


                        接受者 = MessagesQueue.Address;
                        微信内容 = MessagesQueue.Title;
                        信息内容 = MessagesQueue.Message;

                        if (接受者.NotIsNullOrEmpty() && 微信内容.NotIsNullOrEmpty() && MessagesQueue.Title != null)
                        {

                            /*通过对象的属性传递参数*/
                            JosonSendMsg JosonSendMsg = new JosonSendMsg(MessagesQueue);
                            JosonSendMsg.JBPMSysMessagesQueue = MessagesQueue;
                            JosonSendMsg.isDebug = isDebug;
                            IsSendSuecssful = JosonSendMsg.SendMessage();
                            SendSuecssfulCounts = IsSendSuecssful ? SendSuecssfulCounts++ : SendSuecssfulCounts;

                        }


                        Thread.Sleep(1000);

                        if (MessagesQueue.Title.SplitString("|").Length > 1)
                            Counts++;
                        else
                            wCounts++;



                        strMsg = String.Format("共{0}条信息，正在发送 |->第{1}封邮件|->第{2}条微信， 还剩{3}条！", Convert.ToString(QueueCounts), Counts, wCounts, QueueCounts - (Counts + wCounts));

                        //Console.WriteLine(String.Format("当前状态：{0}", strMsg));

                    }

                    if (Counts + wCounts == IQBPMSysMessagesQueue.ToList().Count() && QueueCounts > 0)
                    {

                        strMsg = String.Format("执行定时任务{0}次！", Convert.ToString(taskCounts));

                    }

                }
                catch (Exception ex)
                {


                    strMsg = String.Format("{0}", ex.GetBaseException().Message + Environment.NewLine + Environment.NewLine + ex.StackTrace);
                    System.Diagnostics.EventLog log = new System.Diagnostics.EventLog("微信通知");
                    //首先应判断日志来源是否存在，一个日志来源只能同时与一个事件绑定

                    if (!EventLog.SourceExists("邮件微信通知"))
                        EventLog.CreateEventSource("邮件微信通知", "微信通知");

                    log.Source = "微信通知";
                    log.WriteEntry(strMsg, EventLogEntryType.Error);


                }
 

            return strMsg;

            #endregion

        }

    }





    /// <summary>
    /// JosonSendMsg
    /// </summary>
    public class JosonSendMsg
    {
        String 接受者 = String.Empty;
        String 微信内容 = String.Empty;
        String 信息内容 = String.Empty;


        public Boolean isDebug { get; set; }
        public BPMSysMessagesQueue JBPMSysMessagesQueue { get; set; }
        BPMSysMessagesQueue BPMSysMessagesQueue;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="BPMSysMessagesQueue"></param>
        public JosonSendMsg(BPMSysMessagesQueue BPMSysMessagesQueue)
        {
            this.BPMSysMessagesQueue = BPMSysMessagesQueue;
        }

        /// <summary>
        /// 供线程使用
        /// </summary>
        public bool SendMessages()
        {
            BPMSysMessagesQueue BPMSysMessagesQueue = this.BPMSysMessagesQueue;
            return SendMessage(BPMSysMessagesQueue);
        }


        /// <summary>
        /// 供线程使用
        /// </summary>
        public bool SendMessage()
        {
            BPMSysMessagesQueue BPMSysMessagesQueue = JBPMSysMessagesQueue;
            return SendMessage(BPMSysMessagesQueue);
        }


        /// <summary>
        /// SendMessage
        /// </summary>
        /// <param name="BPMSysMessagesQueue"></param>
        /// <returns></returns>
        public bool SendMessage(BPMSysMessagesQueue BPMSysMessagesQueue)
        {
            Boolean IsSendSuecssful = false;
            int ID = BPMSysMessagesQueue.MessageID;

            #region SendMessage

            if (BPMSysMessagesQueue != null)
            {

                接受者 = BPMSysMessagesQueue.Address;
                微信内容 = BPMSysMessagesQueue.Title;
                信息内容 = BPMSysMessagesQueue.Message;

                IsSendSuecssful = Quartz.SendMsgSerives.Start.Run(接受者, 微信内容, 信息内容, ID, isDebug);


                IBPMSysMessagesQueueService _IBPMSysMessagesQueueService = new BPMSysMessagesQueueService();
                //JosonEntity.JosonReflection JosonEntity = new JosonEntity.JosonReflection();

                if (IsSendSuecssful)
                {

                    BPMSysMessagesSucceed BPMSysMessages = JosonReflection.CopyFromEntity<BPMSysMessagesQueue, BPMSysMessagesSucceed>(BPMSysMessagesQueue);

                    IBPMSysMessagesSucceedService _IBPMSysMessagesSucceedService = new BPMSysMessagesSucceedService();
                    BPMSysMessages.SendAt = DateTime.Now;

                    if (_IBPMSysMessagesQueueService.DeleteEntity(BPMSysMessagesQueue))
                    {
                        _IBPMSysMessagesSucceedService.AddEntity(BPMSysMessages);
                        System.Threading.Thread.Sleep(100);
                    }

                }
                else
                {

                    BPMSysMessagesFailed BPMSysMessages = JosonReflection.CopyFromEntity<BPMSysMessagesQueue, BPMSysMessagesFailed>(BPMSysMessagesQueue);

                    IBPMSysMessagesFailedService _IBPMSysMessagesFailedService = new BPMSysMessagesFailedService();
                    BPMSysMessages.RemoveAt = DateTime.Now;
                    BPMSysMessages.Error = "发送失败";

                    if (_IBPMSysMessagesQueueService.DeleteEntity(BPMSysMessagesQueue))
                    {
                        _IBPMSysMessagesFailedService.AddEntity(BPMSysMessages);
                        System.Threading.Thread.Sleep(100);
                    }

                }

            }

            #endregion

            return IsSendSuecssful;
        }

    }
}
