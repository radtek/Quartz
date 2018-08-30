using Quartz.Entity;
using Senparc.Weixin.BLL;
using Senparc.Weixin.IBLL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Net.Common;

namespace Senparc.Joson.WindowsForm
{
    /*
     
     http://www.cnblogs.com/opencoder/archive/2010/02/23/1672043.html
     http://www.cnblogs.com/SkySoot/archive/2012/04/01/2429259.html
     http://www.cnblogs.com/henw/archive/2012/01/06/2314870.html
     http://www.cnblogs.com/mengxingxinqing/p/3191544.html
     http://www.cnblogs.com/SkySoot/archive/2012/04/02/2430295.html
     * 
    
     http://www.cnblogs.com/kissdodog/archive/2013/03/28/2986026.html
     
     */

    public partial class JosonJiang : Form
    {

        String 接受者 = String.Empty;
        String 微信内容 = String.Empty;
        String 信息内容 = String.Empty;
        Boolean IsSendSuecssful = false;

        IBPMSysMessagesQueueService _IBPMSysMessagesQueueService = new BPMSysMessagesQueueService();
        IBPMSysMessagesSucceedService _IBPMSysMessagesSucceedService = new BPMSysMessagesSucceedService();

        IBPMSysSettingsService _IBPMSysSettingsService = new BPMSysSettingsService();
        IQueryable<BPMSysMessagesQueue> IQBPMSysMessagesQueue = null;



      

        int i = 0, iCounts = 1;
        int Counts = 0, wCounts = 0;


        public JosonJiang()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;

            //WinTimer.Interval = 3000;
            WinTimer.Enabled = false;
        }


        Thread SendMsgThread = null;
        delegate bool SendMsgDelegate(BPMSysMessagesQueue BPMSysMessagesQueue);


        private void butSend_Click(object sender, EventArgs e)
        {

            //WinTimer.Tick += new EventHandler(WinTimer_Tick);
            //WinTimer.Enabled = true;



            if (checkBoxs.Checked)
            {
                //开启线程线程池
                int QueueCounts = 0;
                IQBPMSysMessagesQueue = _IBPMSysMessagesQueueService.LoadPageEntities(1, 20, out QueueCounts, o => !Object.Equals(o.Title, null), true, o => o.MessageID);
                ThreadPool.QueueUserWorkItem(new WaitCallback(WaitCallBackMethod), IQBPMSysMessagesQueue);
            }
            else
            {
                //开启子线程
                closeThread();
                SendMsgThread = new Thread(new ThreadStart(SendTheMsg));
                SendMsgThread.IsBackground = true;
                SendMsgThread.Start();
            }


        }


        private void WinTimer_Tick(object sender, EventArgs e)
        {
            if (checkBoxs.Checked)
            {
                //开启线程线程池
                int QueueCounts = 0;
                IQBPMSysMessagesQueue = _IBPMSysMessagesQueueService.LoadPageEntities(1, 20, out QueueCounts, o => !Object.Equals(o.Title, null), true, o => o.MessageID);
                ThreadPool.QueueUserWorkItem(new WaitCallback(WaitCallBackMethod), IQBPMSysMessagesQueue);
            }
            else
            {
                //开启子线程
                closeThread();
                SendMsgThread = new Thread(new ThreadStart(SendTheMsg));
                SendMsgThread.IsBackground = true;
                SendMsgThread.Start();
            }
 
        }



        void WaitCallBackMethod(object IQBPMSysMessagesQueue)
        {

            SendMessageLst((IQueryable<BPMSysMessagesQueue>)IQBPMSysMessagesQueue);

        }


        #region SendTheMessageLst

        void SendTheMessageLst(IQueryable<BPMSysMessagesQueue> IQBPMSysMessagesQueue)
        {
            foreach (BPMSysMessagesQueue MessagesQueue in IQBPMSysMessagesQueue.ToList())
            {

                /*通过对象的属性传递参数*/

                JosonSendMsg JosonSendMsg = new JosonSendMsg(MessagesQueue);
                JosonSendMsg.JBPMSysMessagesQueue = MessagesQueue;
                JosonSendMsg.SendMessage();

                Thread.Sleep(2000);
            }
        }

        #endregion

        #region SendMessageLst

        /// <summary>
        /// 
        /// </summary>
        /// <param name="BPMSysMessagesQueue"></param>
        /// <returns></returns>
        public void SendMessageLst(IQueryable<BPMSysMessagesQueue> IQBPMSysMessagesQueue)
        {

            int QueueCounts = IQBPMSysMessagesQueue.Count();

            foreach (BPMSysMessagesQueue MessagesQueue in IQBPMSysMessagesQueue.ToList())
            {
                int ID = MessagesQueue.MessageID;

                接受者 = MessagesQueue.Address;
                微信内容 = MessagesQueue.Title;
                信息内容 = MessagesQueue.Message;


                #region UI

                btnStopSend.Enabled = true;
                buttons.Enabled = false;
                msgAccount.Text = 接受者;
                textBoxTitle.Text = 微信内容;
                textBoxContent.Text = 信息内容;

                #endregion


                if (接受者.NotIsNullOrEmpty() && 微信内容.NotIsNullOrEmpty() && MessagesQueue.Title != null)
                {
                    /*通过对象的属性传递参数*/
                    JosonSendMsg JosonSendMsg = new JosonSendMsg(MessagesQueue);
                    JosonSendMsg.JBPMSysMessagesQueue = MessagesQueue;
                    JosonSendMsg.SendMessage();

                    //SendMessage(MessagesQueue);

                    System.Threading.Thread.Sleep(1000);

                }

                if (MessagesQueue.Title.SplitString("|").Length > 1)
                    Counts++;
                else
                    wCounts++;


                labels.Visible = true;
                labels.ForeColor = Color.Brown;
                labels.Text = String.Format("共{0}条信息，正在发送 |->第{1}封邮件|->第{3}条微信， 还剩{2}条！", Convert.ToString(QueueCounts), Counts, QueueCounts - (Counts + wCounts), wCounts);


            }

            if (Counts == IQBPMSysMessagesQueue.ToList().Count())
            {

                labelTask.Text = String.Format("执行定时任务{0}次！", Convert.ToString(iCounts++));

            }

        }

        #endregion


        /// <summary>
        /// 子线程
        /// </summary>
        public void SendTheMsg()
        {
            #region SendTheMsg
            try
            {

                int Counts = 0, wCounts = 0;
                int QueueCounts = 0;

                IQBPMSysMessagesQueue = _IBPMSysMessagesQueueService.LoadPageEntities(1, 20, out QueueCounts, o => !Object.Equals(o.Title, null), true, o => o.MessageID);

                Application.DoEvents();//响应窗口状态

                foreach (BPMSysMessagesQueue MessagesQueue in IQBPMSysMessagesQueue.ToList())
                {
                    Application.DoEvents();

                    接受者 = MessagesQueue.Address;
                    微信内容 = MessagesQueue.Title;
                    信息内容 = MessagesQueue.Message;

                    if (接受者.NotIsNullOrEmpty() && 微信内容.NotIsNullOrEmpty() && MessagesQueue.Title != null)
                    {
                        /*通过对象的属性传递参数*/
                        //JosonSendMsg JosonSendMsg = new JosonSendMsg(MessagesQueue);
                        //JosonSendMsg.JBPMSysMessagesQueue = MessagesQueue;
                        //JosonSendMsg.SendMessage();

                        SendMessage(MessagesQueue);

                    }


                    Thread.Sleep(100);

                    if (MessagesQueue.Title.SplitString("|").Length > 1)
                        Counts++;
                    else
                        wCounts++;


                    labels.Visible = true;
                    labels.ForeColor = Color.Brown;
                    labels.Text = String.Format("共{0}条信息，正在发送 |->第{1}封邮件|->第{3}条微信， 还剩{2}条！", Convert.ToString(QueueCounts), Counts, QueueCounts - (Counts + wCounts), wCounts);


                }

                if (Counts == IQBPMSysMessagesQueue.ToList().Count())
                {

                    labelTask.Text = String.Format("执行定时任务{0}次！", Convert.ToString(iCounts++));

                }


            }
            catch (Exception ex)
            {

                return;
            } 
            #endregion

            closeThread();

        }

        /// <summary>
        /// 子线程
        /// </summary>
        /// <param name="BPMSysMessagesQueue"></param>
        /// <returns></returns>
        public bool SendMessage(BPMSysMessagesQueue BPMSysMessagesQueue)
        {
            int ID = BPMSysMessagesQueue.MessageID;

            #region SendMessage

            if (labels.InvokeRequired || labelTask.InvokeRequired)
            {

                SendMsgDelegate SendMsgDelegate = new SendMsgDelegate(SendMessage);

                this.Invoke(SendMsgDelegate, new object[] { BPMSysMessagesQueue });
                //this.BeginInvoke(SendMsgDelegate, new object[] { BPMSysMessagesQueue });
            }
            else
            {
                if (BPMSysMessagesQueue != null)
                {

                    接受者 = BPMSysMessagesQueue.Address;
                    微信内容 = BPMSysMessagesQueue.Title;
                    信息内容 = BPMSysMessagesQueue.Message;

                    #region UI

                    btnStopSend.Enabled = true;
                    buttons.Enabled = false;
                    msgAccount.Text = 接受者;
                    textBoxTitle.Text = 微信内容;
                    textBoxContent.Text = 信息内容;

                    #endregion

                    IsSendSuecssful = Quartz.SendMsgSerives.Start.Run(接受者, 微信内容, 信息内容, ID);


                    IBPMSysMessagesQueueService _IBPMSysMessagesQueueService = new BPMSysMessagesQueueService();
                    JosonEntity.JosonReflection JosonEntity = new JosonEntity.JosonReflection();

                    if (IsSendSuecssful)
                    {

                        BPMSysMessagesSucceed BPMSysMessages = JosonEntity.CopyFromEntity<BPMSysMessagesQueue, BPMSysMessagesSucceed>(BPMSysMessagesQueue);

                        IBPMSysMessagesSucceedService _IBPMSysMessagesSucceedService = new BPMSysMessagesSucceedService();
                        BPMSysMessages.SendAt = DateTime.Now;

                        if (_IBPMSysMessagesQueueService.DeleteEntity(BPMSysMessagesQueue))
                        {
                            _IBPMSysMessagesSucceedService.AddEntity(BPMSysMessages);
                            System.Threading.Thread.Sleep(1000);
                        }

                    }
                    else
                    {

                        BPMSysMessagesFailed BPMSysMessages = JosonEntity.CopyFromEntity<BPMSysMessagesQueue, BPMSysMessagesFailed>(BPMSysMessagesQueue);

                        IBPMSysMessagesFailedService _IBPMSysMessagesFailedService = new BPMSysMessagesFailedService();
                        BPMSysMessages.RemoveAt = DateTime.Now;
                        BPMSysMessages.Error = "发送失败";

                        if (_IBPMSysMessagesQueueService.DeleteEntity(BPMSysMessagesQueue))
                        {
                            _IBPMSysMessagesFailedService.AddEntity(BPMSysMessages);
                            System.Threading.Thread.Sleep(1000);
                        }

                    }

                }
            }

            #endregion

            return IsSendSuecssful;
        }


        /// <summary>
        /// 结束子线程
        /// </summary>
        private void closeThread()
        {
            if (SendMsgThread != null)
            {
                if (SendMsgThread.IsAlive)
                {
                    SendMsgThread.Abort();
                }
            }

        }

        /// <summary>
        /// 窗体关闭时，关闭子线程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void JosonJiang_FormClosing(object sender, FormClosingEventArgs e)
        {
            closeThread();
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
        public void SendMessages()
        {
            BPMSysMessagesQueue BPMSysMessagesQueue = this.BPMSysMessagesQueue;
            SendMessage(BPMSysMessagesQueue);
        }


        /// <summary>
        /// 供线程使用
        /// </summary>
        public void SendMessage()
        {
            BPMSysMessagesQueue BPMSysMessagesQueue = JBPMSysMessagesQueue;
            SendMessage(BPMSysMessagesQueue);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="BPMSysMessagesQueue"></param>
        /// <returns></returns>
        public bool SendMessage(BPMSysMessagesQueue BPMSysMessagesQueue)
        {
            Boolean IsSendSuecssful = false;
            int ID = BPMSysMessagesQueue.MessageID;

            if (BPMSysMessagesQueue != null)
            {

                接受者 = BPMSysMessagesQueue.Address;
                微信内容 = BPMSysMessagesQueue.Title;
                信息内容 = BPMSysMessagesQueue.Message;

                IsSendSuecssful = Quartz.SendMsgSerives.Start.Run(接受者, 微信内容, 信息内容, ID);


                IBPMSysMessagesQueueService _IBPMSysMessagesQueueService = new BPMSysMessagesQueueService();
                JosonEntity.JosonReflection JosonEntity = new JosonEntity.JosonReflection();

                if (IsSendSuecssful)
                {

                    BPMSysMessagesSucceed BPMSysMessages = JosonEntity.CopyFromEntity<BPMSysMessagesQueue, BPMSysMessagesSucceed>(BPMSysMessagesQueue);

                    IBPMSysMessagesSucceedService _IBPMSysMessagesSucceedService = new BPMSysMessagesSucceedService();
                    BPMSysMessages.SendAt = DateTime.Now;

                    if (_IBPMSysMessagesQueueService.DeleteEntity(BPMSysMessagesQueue))
                    {
                        _IBPMSysMessagesSucceedService.AddEntity(BPMSysMessages);
                        System.Threading.Thread.Sleep(1000);
                    }

                }
                else
                {

                    BPMSysMessagesFailed BPMSysMessages = JosonEntity.CopyFromEntity<BPMSysMessagesQueue, BPMSysMessagesFailed>(BPMSysMessagesQueue);

                    IBPMSysMessagesFailedService _IBPMSysMessagesFailedService = new BPMSysMessagesFailedService();
                    BPMSysMessages.RemoveAt = DateTime.Now;
                    BPMSysMessages.Error = "发送失败";

                    if (_IBPMSysMessagesQueueService.DeleteEntity(BPMSysMessagesQueue))
                    {
                        _IBPMSysMessagesFailedService.AddEntity(BPMSysMessages);
                        System.Threading.Thread.Sleep(1000);
                    }

                }

            }

            return IsSendSuecssful;
        }

    }


}
