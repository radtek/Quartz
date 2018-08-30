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

    public partial class JosonJiang : AlSkin.AlForm.AlBaseForm
    {
        /// <summary>
        /// 执行SQL测试
        /// </summary>
        void ExcuteSQL()
        {

            #region 执行SQL语句

            IBPMSysMessagesQueueService _IBPMSysMessagesQueueService = new BPMSysMessagesQueueService();

            string strSQL = "delete from  BPMProcNotifyRecHis where PROCNAME like @PROCNAME and NOTIFYUSER like @NOTIFYUSER";
            strSQL = "select * from  BPMProcNotifyRecHis";

            System.Data.SqlClient.SqlParameter[] SqlParameter = new System.Data.SqlClient.SqlParameter[2];

            SqlParameter[0] = new System.Data.SqlClient.SqlParameter("@PROCNAME", SqlDbType.NVarChar);
            SqlParameter[0].Value = "%国内%";
            SqlParameter[1] = new System.Data.SqlClient.SqlParameter("@NOTIFYUSER", SqlDbType.NVarChar);
            SqlParameter[1].Value = "%SDT12872%";

            //SqlParameter[0] = new System.Data.SqlClient.SqlParameter("@STEPID", SqlDbType.Int);
            //SqlParameter[0].Value = "5259224";


            //System.Data.SqlClient.SqlParameter[] Params ={ 
            //                        new System.Data.SqlClient.SqlParameter("@userID",SqlDbType.Int),
            //                        new System.Data.SqlClient.SqlParameter("@itemKey",SqlDbType.Int)};

            //Params[0].Value = "userID";
            //Params[1].Value = "itemKey";


            Counts = _IBPMSysMessagesQueueService.ExcuteSqlGetIQueryable<BPMProcNotifyRecHis>(strSQL, SqlParameter).Count();

            strSQL = "select * from  BPMSysMessagesQueue";
            Counts = _IBPMSysMessagesQueueService.ExcuteSqlGetIEnumerable<BPMSysMessagesQueue>(strSQL, SqlParameter).Count();

            _IBPMSysMessagesQueueService.ExcuteSql(strSQL, SqlParameter);

            #endregion

        }


        String 接受者 = String.Empty;
        String 微信内容 = String.Empty;
        String 信息内容 = String.Empty;
        Boolean IsSendSuecssful = false;

        IBPMSysMessagesQueueService _IBPMSysMessagesQueueService = new BPMSysMessagesQueueService();
        IBPMSysMessagesSucceedService _IBPMSysMessagesSucceedService = new BPMSysMessagesSucceedService();

        IBPMSysSettingsService _IBPMSysSettingsService = new BPMSysSettingsService();
        IQueryable<BPMSysMessagesQueue> IQBPMSysMessagesQueue = null;


        int SendSuecssfulCounts = 0, taskCounts = 0, taskPageSize = 200;
        int QueueCounts = 0, Counts = 0, wCounts = 0, CurrentQueueCounts = 0;



        public JosonJiang()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;

            #region InitializeComponent

            // IList<object> lst = Net.Common.JosonEnum.ListEnum<Entity.Template>();
            comboBoxs.DataSource = _IBPMSysSettingsService.LoadEntities(e => e.ItemName.Length > 0).ToList();
            comboBoxs.ValueMember = "ItemValue";
            comboBoxs.DisplayMember = "ItemName";


            msgAccount.Text = "JiangXiaoPeng@skyWorth.Com";
            textBoxTitle.Text = "微信内容\r\n邮件标题|微信内容\r\n流程名称及流程编号->流程办结提醒";
            textBoxContent.Text = "邮件信息内容";
            //butSend.Enabled = false;
            labels.Visible = false;

            butSend.Enabled = false;
            buttons.Enabled = true;
            butSeting.Enabled = false;
            btnStopSend.Enabled = false;

            #endregion

        }

        //定义一个发送线程
        Thread SendMsgThread = null;

        //定义一个委托发送
        delegate bool SendMsgDelegate(BPMSysMessagesQueue BPMSysMessagesQueue);

        //定义一个事件
        public event EventHandler Completed;

        #region AutoResetEvent

        static AutoResetEvent AutoResetEvent = new AutoResetEvent(true);
        void EventHandler(object sender, EventArgs e)
        {
            if (QueueCounts > 0)
            {
                labelTask.Text = String.Format("执行定时任务{0}次！共计发送{1},成功发送{2}次", taskCounts, taskCounts * taskPageSize, SendSuecssfulCounts);
            }

            JOSONCompleted();

            //唤醒主线程
            AutoResetEvent.Set();

        }



        void JOSONCompleted()
        {

            this.Completed += EventHandler;

            if (checkBoxs.Checked)
            {

                //开启线程线程池

                IQBPMSysMessagesQueue = _IBPMSysMessagesQueueService.LoadPageEntities(1, taskPageSize, out QueueCounts, o => !Object.Equals(o.Title, null), true, o => o.MessageID);

                ThreadPool.QueueUserWorkItem(new WaitCallback(WaitCallBackMethod), IQBPMSysMessagesQueue);
                ThreadPool.SetMaxThreads(200, 100);

                CurrentQueueCounts = ((IQueryable<BPMSysMessagesQueue>)IQBPMSysMessagesQueue).Count();


                //队列放入线程
                //Thread MsgThread = new Thread(new ParameterizedThreadStart(WaitCallBackMethod));
                //MsgThread.Start(IQBPMSysMessagesQueue);
                ////MsgThread.Join(Timeout.Infinite);//阻塞当前线程

            }
            else
            {
                //开启子线程
                //closeThread();

                SendMsgThread = new Thread(new ThreadStart(SendMessagesQueue));
                SendMsgThread.IsBackground = true;
                SendMsgThread.Start();

            }

            //继续主线程
            Thread.Sleep(1000);

            //等待辅助线程完成
            AutoResetEvent.WaitOne();

            //Console.WriteLine("主线程完成!");

        }
        #endregion

        private void buttons_Click(object sender, EventArgs e)
        {
            接受者 = String.IsNullOrWhiteSpace(msgAccount.Text) ? "JiangXiaoPeng@skyWorth.Com" : msgAccount.Text;
            微信内容 = String.IsNullOrWhiteSpace(textBoxTitle.Text) ? "流程名称及流程编号" : textBoxTitle.Text;
            信息内容 = String.IsNullOrWhiteSpace(textBoxContent.Text) ? "邮件信息内容" : textBoxContent.Text;


            IsSendSuecssful = Quartz.SendMsgSerives.Start.Run(接受者, 微信内容, 信息内容);

            #region 测试服务器

            if (IsSendSuecssful)
            {
                labels.Text = "服务正常，发送信息成功";
                butSend.Text = "启动发送任务";

                buttons.Enabled = false;
                butSeting.Enabled = false;
                butSend.Enabled = true;
                labels.Visible = true;
                labels.ForeColor = Color.Blue;

            }
            else
            {
                labels.Text = "服务异常暂时无法启动，请联系管理员！";
                butSend.Text = "暂时无法启动";

                buttons.Enabled = true;
                butSeting.Enabled = false;
                butSend.Enabled = false;

                labels.Visible = false;
                labels.ForeColor = Color.Red;
            }

            #endregion

        }


        #region 终止线程池线程

        private static void CancellingAWorkItem()
        {
            CancellationTokenSource cts = new CancellationTokenSource();

            // Pass the CancellationToken and the number-to-count-to into the operation
            ThreadPool.QueueUserWorkItem(o => Count(cts.Token, 1000));

            //Console.WriteLine("Press <Enter> to cancel the operation.");
            //Console.ReadLine();
            cts.Cancel();
            // If Count returned already, Cancel has no effect on it
            // Cancel returns immediately, and the method continues running here...

            //Console.ReadLine();  // For testing purposes
        }

        private static void Count(CancellationToken token, Int32 countTo)
        {
            for (Int32 count = 0; count < countTo; count++)
            {
                if (token.IsCancellationRequested)
                {
                    //Console.WriteLine("Count is cancelled");
                    break; // Exit the loop to stop the operation
                }

                //Console.WriteLine(count);
                Thread.Sleep(200);   // For demo, waste some time
            }

            //Console.WriteLine("Count is done");
        }

        #endregion


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStopSend_Click(object sender, EventArgs e)
        {
            butSend.Enabled = true;
            btnStopSend.Enabled = false;

            //AutoResetEvent.Reset();

            if (checkBoxs.Checked)
            {
                //AutoResetEvent.Set();
                //ThreadPool.SetMaxThreads(0,0);
                //AutoResetEvent.WaitOne();

                closeThread();
                Thread.Sleep(5000);

            }
            else
            {
                closeThread();
            }

        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void butSeting_Click(object sender, EventArgs e)
        {
            #region 更新模板

            Type t = comboBoxs.SelectedItem.GetType();
            string ItemName = t.GetProperty("ItemName").GetValue(comboBoxs.SelectedItem, null).ToString();

            BPMSysSettings BPMSysSettings = _IBPMSysSettingsService.LoadEntities(s => s.ItemName == ItemName).SingleOrDefault();
            BPMSysSettings.ItemValue = textBoxContent.Text;

            bool flag = _IBPMSysSettingsService.UpdateEntity(BPMSysSettings);

            if (!flag)
            {
                DialogResult DialogResult = MessageBox.Show("更新模板失败", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

            }
            else
            {
                butSend.Enabled = true;
                buttons.Enabled = false;
                butSeting.Enabled = false;

                MessageBox.Show(ItemName + "模板更新成功", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            #endregion

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxs_SelectedIndexChanged(object sender, EventArgs e)
        {
            butSend.Enabled = false;
            buttons.Enabled = false;
            butSeting.Enabled = true;

            #region 模板设置

            try
            {
                Type t = comboBoxs.SelectedItem.GetType();

                //var O = comboBoxs.SelectedItem == null ? "" : comboBoxs.SelectedItem;

                string ItemName = t.GetProperty("ItemName").GetValue(comboBoxs.SelectedItem, null).ToString();
                string ItemValue = t.GetProperty("ItemValue").GetValue(comboBoxs.SelectedItem, null).ToString();

                textBoxTitle.Text = ItemName;
                textBoxContent.Text = comboBoxs.SelectedValue.ToString();



            }
            catch (Exception ex)
            {
                textBoxTitle.Text = ex.Message;
                textBoxContent.Text = ex.Message;
            }

            #endregion

        }


        private void butSend_Click(object sender, EventArgs e)
        {

            JOSONCompleted();

            butSend.Enabled = false;
            buttons.Enabled = false;
            butSeting.Enabled = false;
            btnStopSend.Enabled = true;
            labels.Visible = true;
        }


        void WaitCallBackMethod(object IQBPMSysMessagesQueue)
        {
           // CurrentQueueCounts = ((IQueryable<BPMSysMessagesQueue>)IQBPMSysMessagesQueue).Count();
            SendMessageLst((IQueryable<BPMSysMessagesQueue>)IQBPMSysMessagesQueue);

        }


        #region SendMessageLst

        /// <summary>
        /// 
        /// </summary>
        /// <param name="BPMSysMessagesQueue"></param>
        /// <returns></returns>
        public void SendMessageLst(IQueryable<BPMSysMessagesQueue> IQBPMSysMessagesQueue)
        {

            CurrentQueueCounts = IQBPMSysMessagesQueue.Count();
            Counts = 0; wCounts = 0;


            foreach (BPMSysMessagesQueue MessagesQueue in IQBPMSysMessagesQueue.ToList())
            {

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
                    System.Threading.Thread.Sleep(100);

                }

            }

            //当辅助线程完成时，触发已完成事件
            if (Completed != null)
            {
                taskCounts++;
                Completed(this, new EventArgs());
            }

        }

        #endregion


        /// <summary>
        /// 子线程
        /// </summary>
        public void SendMessagesQueue()
        {
            #region SendMessagesQueue
            try
            {

                Counts = 0; wCounts = 0;
                int QueueCounts = 0;

                IQBPMSysMessagesQueue = _IBPMSysMessagesQueueService.LoadPageEntities(1, taskPageSize, out QueueCounts, o => !Object.Equals(o.Title, null), true, o => o.MessageID);

                foreach (BPMSysMessagesQueue MessagesQueue in IQBPMSysMessagesQueue.ToList())
                {
                    Application.DoEvents();//响应窗口状态

                    接受者 = MessagesQueue.Address;
                    微信内容 = MessagesQueue.Title;
                    信息内容 = MessagesQueue.Message;

                    if (接受者.NotIsNullOrEmpty() && 微信内容.NotIsNullOrEmpty() && MessagesQueue.Title != null)
                    {

                        /*通过对象的属性传递参数*/
                        JosonSendMsg JosonSendMsg = new JosonSendMsg(MessagesQueue);
                        JosonSendMsg.JBPMSysMessagesQueue = MessagesQueue;
                        IsSendSuecssful = JosonSendMsg.SendMessage();
                        SendSuecssfulCounts = IsSendSuecssful ? SendSuecssfulCounts++ : SendSuecssfulCounts;

                    }


                    Thread.Sleep(1000);

                    if (MessagesQueue.Title.SplitString("|").Length > 1)
                        Counts++;
                    else
                        wCounts++;


                    labels.Visible = true;
                    labels.ForeColor = Color.CornflowerBlue;
                    labels.Text = String.Format("共{0}条信息，正在发送 |->第{1}封邮件|->第{2}条微信， 还剩{3}条！", Convert.ToString(QueueCounts), Counts, wCounts, QueueCounts - (Counts + wCounts));


                }

                if (Counts + wCounts == IQBPMSysMessagesQueue.ToList().Count() && QueueCounts > 0)
                {

                    labelTask.Text = String.Format("执行定时任务{0}次！", Convert.ToString(taskCounts));

                }

            }
            catch (Exception ex)
            {

                return;
            }
            #endregion


            //当辅助线程完成时，触发已完成事件
            if (Completed != null)
            {
                taskCounts++;
                Completed(this, new EventArgs());
            }

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


                    IsSendSuecssful = Quartz.SendMsgSerives.Start.Run(接受者, 微信内容, 信息内容, ID);

                    SendSuecssfulCounts = IsSendSuecssful ? SendSuecssfulCounts + 1 : SendSuecssfulCounts;


                    #region UI

                    btnStopSend.Enabled = true;
                    buttons.Enabled = false;
                    msgAccount.Text = 接受者;
                    textBoxTitle.Text = 微信内容;
                    textBoxContent.Text = 信息内容;

                    if (BPMSysMessagesQueue.Title.SplitString("|").Length > 1)
                        Counts++;
                    else
                        wCounts++;

                    labels.Visible = true;
                    labels.ForeColor = Color.CadetBlue;
                    labels.Text = String.Format("共{0}条信息，当前任务{4}条 正在发送 |->第{1}封邮件|->第{2}条微信， 还剩{3}条！"
                        , Convert.ToString(QueueCounts)
                        , Counts
                        , wCounts
                        , QueueCounts - (Counts + wCounts)
                        , CurrentQueueCounts);


                    #endregion


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
                            System.Threading.Thread.Sleep(1000);
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
            //AutoResetEvent.Reset();
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

                IsSendSuecssful = Quartz.SendMsgSerives.Start.Run(接受者, 微信内容, 信息内容, ID);


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
