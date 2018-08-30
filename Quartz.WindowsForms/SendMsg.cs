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



namespace Quartz.WindowsForms
{
    public partial class SendMsg : AlSkin.AlForm.AlBaseForm
    {

        String 接受者 = String.Empty;
        String 微信内容 = String.Empty;
        String 信息内容 = String.Empty;
        Boolean IsSendSuecssful = false;

        IBPMSysMessagesQueueService _IBPMSysMessagesQueueService = new BPMSysMessagesQueueService();
        IBPMSysMessagesSucceedService _IBPMSysMessagesSucceedService = new BPMSysMessagesSucceedService();

        IBPMSysSettingsService _IBPMSysSettingsService = new BPMSysSettingsService();

        IQueryable<BPMSysMessagesQueue> IQBPMSysMessagesQueue = null;



        static ArrayList MsgTo = new ArrayList();

        int i = 0, iCounts = 1;

        //System.Timers.Timer Timers = new System.Timers.Timer(10000);   //实例化Timer类，设置间隔时间为1000毫秒；   


        public SendMsg()
        {
            InitializeComponent();

            Control.CheckForIllegalCrossThreadCalls = false;

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



        }


        private void WinTimer_Tick(object sender, EventArgs e)
        {
            Application.DoEvents();

            //JSendMsg JosonSendMsg = new JSendMsg();
            //JosonSendMsg.Send();

            SendTheMsg();

            //SendTheMsgLst();

            //thisisTest();
        }


        #region 测试代码 垃圾

        public void theout(object source, System.Timers.ElapsedEventArgs e)
        {
            thisisTest();
        }



        void thisisTest()
        {

            labels.Visible = true;
            labels.Text = Convert.ToString((i++));

            ArrayList AutoTask = new ArrayList();
            AutoTask.Add("8:30:00");
            AutoTask.Add("10:20:00");
            AutoTask.Add("10:30:00");
            AutoTask.Add("11:34:15");

            for (int n = 0; n < 4; n++)
            {
                if (DateTime.Now.ToLongTimeString().Equals(AutoTask[n]))
                {
                    MessageBox.Show("现在时间是" + AutoTask[n]);
                }
            }

            // MessageBox.Show("OK!");

        }


        private void SendMsg_Load(object sender, EventArgs e)
        {

            //System.Timers.Timer Timers = new System.Timers.Timer(10000);                        //实例化Timer类，设置间隔时间为10000毫秒；   

            //Timers.Elapsed += new System.Timers.ElapsedEventHandler(StartSendTheMsg);             //到达时间的时候执行事件；   
            //Timers.AutoReset = true;                                                              //设置是执行一次（false）还是一直执行(true)；   
            //Timers.Enabled = true;                                                                //是否执行System.Timers.Timer.Elapsed事件；
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxs_SelectedIndexChanged(object sender, EventArgs e)
        {

            Type t = comboBoxs.SelectedItem.GetType();

            //var O = comboBoxs.SelectedItem == null ? "" : comboBoxs.SelectedItem;

            string ItemName = Convert.ToString(t.GetProperty("ItemName").GetValue(comboBoxs.SelectedItem, null));
            string ItemValue = Convert.ToString(t.GetProperty("ItemValue").GetValue(comboBoxs.SelectedItem, null));

            textBoxTitle.Text = ItemName;
            textBoxContent.Text = comboBoxs.SelectedValue.ToString();

            butSend.Enabled = false;
            buttons.Enabled = false;
            butSeting.Enabled = true;


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStopSend_Click(object sender, EventArgs e)
        {
            butSend.Enabled = true;
            btnStopSend.Enabled = false;

            //Timers.Enabled = false;
            //Timers.Stop();

            WinTimer.Enabled = false;
            WinTimer.Stop();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void butSend_Click(object sender, EventArgs e)
        {
            //ISubscribeUsersService _ISubscribeUsersService = new SubscribeUsersService();
            //IQueryable<SubscribeUsers> UserLst = _ISubscribeUsersService.LoadEntities(o => o.EmpID == 接受者);
            //SubscribeUsers SubscribeUsers = _ISubscribeUsersService.LoadEntities(o => o.Email == 接受者).SingleOrDefault();

            butSend.Enabled = false;
            buttons.Enabled = false;
            butSeting.Enabled = false;
            btnStopSend.Enabled = true;



#if debug

            IQueryable<BPMSysMessagesQueue> IQBPMSysMessagesQueue = _IBPMSysMessagesQueueService.LoadEntities(o => o.Title.Length > 0);

            foreach (BPMSysMessagesQueue MessagesQueue in IQBPMSysMessagesQueue)
            {
                //System.Threading.Thread.Sleep(5000);

            #region Temp

                //buttons.Enabled = false;
                //msgAccount.Text = MessagesQueue.Address;
                //textBoxTitle.Text = MessagesQueue.Title;
                //textBoxContent.Text = MessagesQueue.Message;

                /* ParameterizedThreadStart要求参数类型必须为object */
                //ParameterizedThreadStart Ts = new ParameterizedThreadStart(SendMessages);
                //Thread Thread = new Thread(Ts);
                //Thread.Start(MessagesQueue);



                /*通过构造函数传参数*/
                //JosonSendMsg JosonSendMsg = new JosonSendMsg(MessagesQueue);
                //Thread t = new Thread(new ThreadStart(JosonSendMsg.SendMessages));
                //t.Start();


                /*通过对象的属性传递参数*/
                //JosonSendMsg JosonSendMsg = new JosonSendMsg(MessagesQueue);
                //JosonSendMsg.JBPMSysMessagesQueue = MessagesQueue;
                //Thread J = new Thread(new ThreadStart(JosonSendMsg.SendMessage));
                //J.Start();


                //JosonSendMsg.SendMessage(MessagesQueue); 
            #endregion

            }
#endif

            //Thread J = new Thread(new ThreadStart(new JSendMsg().Send));
            //J.Start();
            //J.Join(Timeout.Infinite);

            //this.backgroundWorkers.RunWorkerAsync();

            //SendTheMsg();

            WinTimer.Tick += new EventHandler(WinTimer_Tick);

            WinTimer.Interval = 50000;

            //Timers.Enabled = true;
            //Timers.Start();


        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorkers_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Timers.Elapsed += new System.Timers.ElapsedEventHandler(StartSendTheMsg);   //到达时间的时候执行事件；   
            //Timers.AutoReset = true;                                                    //设置是执行一次（false）还是一直执行(true)；   
            //Timers.Enabled = true;                                                      //是否执行System.Timers.Timer.Elapsed事件；

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        public void StartSendTheMsg(object source, System.Timers.ElapsedEventArgs e)
        {
            SendTheMsg();
            Thread.Sleep(5000);
        }



        /// <summary>
        /// 
        /// </summary>
        public void SendTheMsg()
        {
            //labels.Text = String.Format("暂时无法发送任务");


            int Counts = 0, wCounts = 0;
            int QueueCounts = 0; //IQBPMSysMessagesQueue.ToList().Count;

            //IQueryable<BPMSysMessagesQueue> IQBPMSysMessagesQueue = _IBPMSysMessagesQueueService.LoadEntities(o => o.Title.Length > 0);
            //IQBPMSysMessagesQueue = _IBPMSysMessagesQueueService.LoadPageEntities(1, 200, out QueueCounts, o => o.Title.Length > 0, true, o => o.MessageID);
            IQBPMSysMessagesQueue = _IBPMSysMessagesQueueService.LoadPageEntities(1, 200, out QueueCounts, o => !Object.Equals(o.Title, null), true, o => o.MessageID);

            Application.DoEvents();//响应窗口状态

            foreach (BPMSysMessagesQueue MessagesQueue in IQBPMSysMessagesQueue.ToList())
            {

                DateTime Dtime = DateTime.Now;
                while (Dtime.AddSeconds(1) > DateTime.Now)
                {
                    接受者 = MessagesQueue.Address;
                    微信内容 = MessagesQueue.Title;
                    信息内容 = MessagesQueue.Message;


                    if (接受者.NotIsNullOrEmpty() && 微信内容.NotIsNullOrEmpty() && MessagesQueue.Title != null)
                    {

                        HanderMsg HanderMsg = new HanderMsg(MessagesQueue);
                        Thread t = Joson.CreateSendMsg(new Joson.SendMsg(HanderMsg.MailMethod), MessagesQueue);
                        t.IsBackground = true;
                        t.Start();
                        //t.Abort();
                        t.Join(Timeout.Infinite);

                        ThreadState ThreadState = t.ThreadState;

                        labelTask.Text = String.Format("当前线程:{0}", Thread.CurrentThread.Name + ThreadState.ToStrings());
                    }

                    btnStopSend.Enabled = true;
                    buttons.Enabled = false;
                    msgAccount.Text = MessagesQueue.Address;
                    textBoxTitle.Text = MessagesQueue.Title;
                    textBoxContent.Text = MessagesQueue.Message;


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
                    //labels.Text = String.Format("共发送完毕成功{0}条信息！", Convert.ToString(QueueCounts));
                    //MessageBox.Show("信息发送完毕成功", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    labelTask.Text = String.Format("执行定时任务{0}次！", Convert.ToString(iCounts++));

                }

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

        public void SendTheMsgLst()
        {
            int QueueCounts = 0;
            IQBPMSysMessagesQueue = _IBPMSysMessagesQueueService.LoadPageEntities(1, 200, out QueueCounts, o => o.Title.Length > 0, true, o => o.MessageID);

            HanderSendMsg HanderSendMsg = new HanderSendMsg(IQBPMSysMessagesQueue.ToList());

            HanderSendMsg.MailMethod();

        }


    }



    class Joson
    {
        public delegate bool SendMsg();
        public delegate bool SendMsgLst();

        private class Message
        {
            public BPMSysMessagesQueue MessagesQueue;
            public List<BPMSysMessagesQueue> MessagesQueueLst;


            public SendMsg SendThisMsg;         //delegate instance
            public SendMsgLst SendThisMsgLst;   //delegate instance

            public void StartSend()
            {
                SendThisMsg();
            }


            public void StartSendLst()
            {
                SendThisMsgLst();
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oSendEmail"></param>
        /// <param name="MessagesQueue"></param>
        /// <returns></returns>
        public static Thread CreateSendMsg(SendMsg oSendEmail, BPMSysMessagesQueue MessagesQueue)
        {
            Message oMail = new Message();

            oMail.MessagesQueue = MessagesQueue;
            oMail.SendThisMsg = oSendEmail;

            Thread T = new Thread(new ThreadStart(oMail.StartSend));

            return T;
        }



        /// <summary>
        ///  
        /// </summary>
        /// <param name="oSendEmail"></param>
        /// <param name="MessagesQueue"></param>
        /// <returns></returns>
        public static Thread CreateSendMsgLst(SendMsgLst oSendEmail, List<BPMSysMessagesQueue> MessagesQueue)
        {
            Message oMail = new Message();

            oMail.MessagesQueueLst = MessagesQueue;
            oMail.SendThisMsgLst = oSendEmail;

            Thread T = new Thread(new ThreadStart(oMail.StartSendLst));

            return T;
        }



    }

    class HanderMsg
    {
        BPMSysMessagesQueue MessagesQueue;

        public HanderMsg(BPMSysMessagesQueue MessagesQueue)
        {
            this.MessagesQueue = MessagesQueue;
        }

        public bool MailMethod()
        {
            bool flag = false;
            BPMSysMessagesQueue MessagesQueue = this.MessagesQueue;
            JosonSendMsg JosonSendMsg = new JosonSendMsg(MessagesQueue);
            flag = JosonSendMsg.SendMessage(MessagesQueue);

            return flag;
        }
    }


    class HanderSendMsg
    {
        List<BPMSysMessagesQueue> MessagesQueue;

        public HanderSendMsg(List<BPMSysMessagesQueue> MessagesQueue)
        {
            this.MessagesQueue = MessagesQueue;
        }

        public bool MailMethod()
        {
            bool flag = false;
            List<BPMSysMessagesQueue> MessagesQueue = this.MessagesQueue;

            JSendMsg JosonSendMsg = new JSendMsg();
            JosonSendMsg.Send(MessagesQueue);


            return flag;
        }
    }





    /// <summary>
    /// JSendMsg
    /// </summary>
    public class JSendMsg
    {

        //String 接受者 = String.Empty;
        //String 微信内容 = String.Empty;
        //String 信息内容 = String.Empty;
        //Boolean IsSendSuecssful = false;

        //IBPMSysMessagesQueueService _IBPMSysMessagesQueueService = new BPMSysMessagesQueueService();
        //IBPMSysMessagesSucceedService _IBPMSysMessagesSucceedService = new BPMSysMessagesSucceedService();
        //IBPMSysSettingsService _IBPMSysSettingsService = new BPMSysSettingsService();

        //IQueryable<BPMSysMessagesQueue> IQBPMSysMessagesQueue = null;

        public void Send(List<BPMSysMessagesQueue> MessagesQueueLst)
        {

            //int QueueCounts = 0;  

            //IQBPMSysMessagesQueue = _IBPMSysMessagesQueueService.LoadPageEntities(1, 200, out QueueCounts, o => o.Title.Length > 0, true, o => o.MessageID);

            //List<BPMSysMessagesQueue> MessagesQueueLst = IQBPMSysMessagesQueue.ToList();

            HanderSendMsg HanderSendMsg = new HanderSendMsg(MessagesQueueLst);
            Joson.SendMsgLst JosonSendMsgLst = new Joson.SendMsgLst(HanderSendMsg.MailMethod);
            Thread t = Joson.CreateSendMsgLst(JosonSendMsgLst, MessagesQueueLst);

            t.Start();
            t.Join(Timeout.Infinite);



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

            return IsSendSuecssful;
        }

    }
}
