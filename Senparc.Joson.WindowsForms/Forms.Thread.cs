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
using System.Runtime.Remoting.Messaging;

namespace Senparc.Joson.WindowsForms
{


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


        int iCounts = 1, SendSuecssfulCounts = 0, taskCounts = 1, taskPageSize = 10;
        int QueueCounts = 0, Counts = 0, wCounts = 0;


        public JosonJiang()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;

            WinTimer.Enabled = false;


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


        delegate int SendMsgDelegate();
        delegate int SendMessageLstDelegate(List<BPMSysMessagesQueue> IQBPMSysMessagesQueue);


        public void HanderSendMsg()
        {

            Counts = 0;
            wCounts = 0;

            labelTask.Text = String.Format("定时任务启动状态为{0}", WinTimer.Enabled);


            if (checkBoxs.Checked)
            {
                //开启线程线程池

                IQBPMSysMessagesQueue = _IBPMSysMessagesQueueService.LoadPageEntities(1, taskPageSize, out QueueCounts, o => !Object.Equals(o.Title, null), true, o => o.MessageID);

                //ThreadPool.QueueUserWorkItem(new WaitCallback(WaitCallBackMethod), IQBPMSysMessagesQueue);

                SendMessageLstDelegate SendMessageLstDelegate = new SendMessageLstDelegate(SendMessageLst);
                try
                {
                    //异步线程
                    IAsyncResult result = SendMessageLstDelegate.BeginInvoke(IQBPMSysMessagesQueue.ToList(), new AsyncCallback(Completed), null);
                }
                catch (Exception ex)
                {
                    //同步阻塞
                    IAsyncResult result = SendMessageLstDelegate.BeginInvoke(IQBPMSysMessagesQueue.ToList(), null, null);

                    while (!result.IsCompleted)
                    {
                        Thread.Sleep(500);
                        //Console.WriteLine("异步线程还没完成，主线程干其他事!");
                    }

                    SendMessageLstDelegate.EndInvoke(result);

                }
            }
            else
            {
                //开启子线程
                SendMsgDelegate SendMsgDelegate = new SendMsgDelegate(SendTheMsg);

                try
                {
                    //异步线程
                    IAsyncResult result = SendMsgDelegate.BeginInvoke(new AsyncCallback(Completed), null);
                }
                catch (Exception ex)
                {
                    //同步阻塞
                    IAsyncResult result = SendMsgDelegate.BeginInvoke(null, null);

                    //while (!result.IsCompleted)
                    //{
                    //    //Thread.Sleep(500);
                    //    //Console.WriteLine("异步线程还没完成，主线程干其他事!");

                    //    WinTimer.Enabled = false;
                    //}

                    WinTimer.Enabled = !result.IsCompleted ? false : true;
                    SendMsgDelegate.EndInvoke(result);

                }
            }

            //Thread.Sleep(2000);

            //WinTimer.Enabled = false;
            //labelTask.Text = String.Format("开启子线程后，当前定时任务状态为{0}", WinTimer.Enabled);
        }



        /// <summary>
        /// 供异步线程完成回调的方法
        /// </summary>
        /// <param name="result"></param>
        void Completed(IAsyncResult result)
        {
            int SuecssfulCounts = 0;
            //获取委托对象，调用EndInvoke方法获取运行结果
            AsyncResult _result = (AsyncResult)result;

            labelTask.Text = String.Format("子线程执行过程中，当前定时任务状态为{0}", WinTimer.Enabled);

            if (checkBoxs.Checked)
            {
                SendMessageLstDelegate myDelegaate = (SendMessageLstDelegate)_result.AsyncDelegate;
                //获得参数
                SuecssfulCounts = myDelegaate.EndInvoke(_result);
            }
            else
            {
                SendMsgDelegate myDelegaate = (SendMsgDelegate)_result.AsyncDelegate;
                //获得参数
                SuecssfulCounts = myDelegaate.EndInvoke(_result);
            }


            //Console.WriteLine(SucessfulSendCount);
            //异步线程执行完毕
            //Console.WriteLine("异步线程完成咯！");
            //Console.WriteLine("回调函数也是由" + Thread.CurrentThread.Name + "调用的！");

            WinTimer.Enabled = true;
            //WinTimer.Tick += new EventHandler(WinTimer_Tick);

            labels.Visible = true;
            labels.Text = String.Format("共{0}条信息，执行定时任务{4}次！发送|->{1}封邮件|->{2}条微信，成功{3}条！", Convert.ToString(QueueCounts), Counts, wCounts, SuecssfulCounts, Convert.ToString(taskCounts++));

            labelTask.Text = String.Format("子线程执行完毕，当前定时任务状态为【{0}】", WinTimer.Enabled);

        }



        private void butSend_Click(object sender, EventArgs e)
        {
            WinTimer.Enabled = true;
            WinTimer.Tick += new EventHandler(WinTimer_Tick);

            //HanderSendMsg();

            butSend.Enabled = false;
            buttons.Enabled = false;
            butSeting.Enabled = false;
            btnStopSend.Enabled = true;
            labels.Visible = true;
        }


        private void WinTimer_Tick(object sender, EventArgs e)
        {

            HanderSendMsg();
            Application.DoEvents();


        }

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
        private void btnStopSend_Click(object sender, EventArgs e)
        {
            butSend.Enabled = true;
            btnStopSend.Enabled = false;

            WinTimer.Enabled = false;
            WinTimer.Stop();
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



        #region 子线程 发送信息
        /// <summary>
        /// 发送信息子线程
        /// </summary>
        /// <param name="BPMSysMessagesQueue"></param>
        /// <returns></returns>
        public int SendMessageLst(IEnumerable<BPMSysMessagesQueue> IQBPMSysMessagesQueue)
        {

            QueueCounts = IQBPMSysMessagesQueue.ToList().Count();

            foreach (BPMSysMessagesQueue MessagesQueue in IQBPMSysMessagesQueue.ToList())
            {
                int ID = MessagesQueue.MessageID;

                接受者 = MessagesQueue.Address;
                微信内容 = MessagesQueue.Title;
                信息内容 = MessagesQueue.Message;

                if (接受者.NotIsNullOrEmpty() && 微信内容.NotIsNullOrEmpty() && MessagesQueue.Title != null)
                {

                    /*通过对象的属性传递参数*/
                    //JosonSendMsg JosonSendMsg = new JosonSendMsg(MessagesQueue);
                    //JosonSendMsg.JBPMSysMessagesQueue = MessagesQueue;
                    //IsSendSuecssful = JosonSendMsg.SendMessage();

                    IsSendSuecssful = SendMessage(MessagesQueue);

                    SendSuecssfulCounts = IsSendSuecssful ? SendSuecssfulCounts + 1 : SendSuecssfulCounts;

                }

                if (MessagesQueue.Title.SplitString("|").Length > 1)
                    Counts++;
                else
                    wCounts++;

                //Application.DoEvents();

                labels.Visible = true;
                labels.ForeColor = Color.Brown;
                labels.Text = String.Format("共{0}条信息，正在发送 |->第{1}封邮件|->第{3}条微信，还剩{2}条！", Convert.ToString(QueueCounts), Counts, QueueCounts - (Counts + wCounts), wCounts);

                System.Threading.Thread.Sleep(1000);

            }

            if (Counts == IQBPMSysMessagesQueue.ToList().Count())
            {

                labelTask.Text = String.Format("执行定时任务{0}次！", Convert.ToString(iCounts++));

            }

            return SendSuecssfulCounts;
        }


        /// <summary>
        /// 发送信息子线程
        /// </summary>
        public int SendTheMsg()
        {
            try
            {

                //int Counts = 0, wCounts = 0;
                //int QueueCounts = 0;

                //IBPMSysMessagesQueueService _IBPMSysMessagesQueueService = new BPMSysMessagesQueueService();
                IQBPMSysMessagesQueue = _IBPMSysMessagesQueueService.LoadPageEntities(1, taskPageSize, out QueueCounts, o => !Object.Equals(o.Title, null), true, o => o.MessageID);

                Application.DoEvents();//响应窗口状态

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

                        IsSendSuecssful = SendMessage(MessagesQueue);

                        SendSuecssfulCounts = IsSendSuecssful ? SendSuecssfulCounts + 1 : SendSuecssfulCounts;

                    }


                    Thread.Sleep(1000);

                    //if (MessagesQueue.Title.SplitString("|").Length > 1)
                    //    Counts++;
                    //else
                    //    wCounts++;


                    //labels.Visible = true;
                    //labels.ForeColor = Color.Brown;
                    //labels.Text = String.Format("共{0}条信息，正在发送 |->第{1}封邮件|->第{3}条微信， 还剩{2}条！", Convert.ToString(QueueCounts), Counts, QueueCounts - (Counts + wCounts), wCounts);


                }

                //if (Counts == IQBPMSysMessagesQueue.ToList().Count())
                //{

                //    labelTask.Text = String.Format("执行定时任务{0}次！", Convert.ToString(iCounts++));

                //}


            }
            catch (Exception ex)
            {

            }

            return SendSuecssfulCounts;

        }

        #endregion


        /// <summary>
        /// 发送信息处理方法
        /// </summary>
        /// <param name="BPMSysMessagesQueue"></param>
        /// <returns></returns>
        public bool SendMessage(BPMSysMessagesQueue BPMSysMessagesQueue)
        {

            int ID = BPMSysMessagesQueue.MessageID;

            #region 发送信息处理方法


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


                if (BPMSysMessagesQueue.Title.SplitString("|").Length > 1)
                    Counts++;
                else
                    wCounts++;


                WinTimer.Enabled = false;
                labelTask.Text = String.Format("子线程执行任务中，当前定时状态为{0}", WinTimer.Enabled);


                labels.Visible = true;
                labels.ForeColor = Color.Brown;
                labels.Text = String.Format("共{0}条信息，正在发送 |->第{1}封邮件|->第{3}条微信, 还剩{2}条！", Convert.ToString(QueueCounts), Counts, QueueCounts - (Counts + wCounts), wCounts);


                #endregion

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


            #endregion

            return IsSendSuecssful;
        }



        /// <summary>
        /// 窗体关闭时，关闭线程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void JosonJiang_FormClosing(object sender, FormClosingEventArgs e)
        {
            WinTimer.Enabled = false;
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
