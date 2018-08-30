using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Timers;

namespace Quartz.Consoles
{

    partial class Service : ServiceBase
    {
        readonly Timer _timer;
        private static readonly string FileName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\" + "Joson.txt";

        public Service()
        {
            InitializeComponent();

            _timer = new Timer(1000)
            {
                AutoReset = true,
                Enabled = true
            };

            _timer.Elapsed += delegate(object sender, ElapsedEventArgs e)
            {
                this.JosonService(string.Format("服务运行时间 ：{0}", DateTime.Now));
            };
        }


        protected override void OnStart(string[] args)
        {
            this.JosonService(string.Format("服务启动时间： {0}", DateTime.Now));

        }

        protected override void OnStop()
        {
            this.JosonService(string.Format("服务停止时间： {0}", DateTime.Now) + Environment.NewLine);
            base.OnStop();
        }

        protected override void OnPause()
        {
            //服务暂停执行代码
            this.JosonService(string.Format("服务暂停执行 DateTime {0}", DateTime.Now) + Environment.NewLine);
            base.OnPause();
        }
        protected override void OnContinue()
        {
            //服务恢复执行代码
            this.JosonService(string.Format("服务恢复执行 DateTime {0}", DateTime.Now) + Environment.NewLine);
            base.OnContinue();
        }
        protected override void OnShutdown()
        {
            //系统即将关闭执行代码
            this.JosonService(string.Format("系统即将关闭执行 DateTime {0}", DateTime.Now) + Environment.NewLine);
            base.OnShutdown();
        }


        void JosonService(string context)
        {

            try
            {

                String strMsg = Start.SendMessagesQueue(false);
                if (!String.IsNullOrWhiteSpace(strMsg))
                {
                    Console.WriteLine(String.Format("当前状态：{0}", strMsg));
                }


                StreamWriter StreamWriter = File.AppendText(FileName);
                StreamWriter.WriteLine(context);
                StreamWriter.Flush();
                StreamWriter.Close();


                if (!EventLog.SourceExists("微信流程通知"))
                    EventLog.CreateEventSource("微信流程通知", "邮件微信流程通知");
                System.Diagnostics.EventLog log = new System.Diagnostics.EventLog("邮件微信流程通知");

                log.Source = "微信流程通知";
                log.WriteEntry(context, EventLogEntryType.SuccessAudit);


            }
            catch (Exception ex)
            {
                String Message = String.Format("{0}", ex.GetBaseException().Message + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace);
                System.Diagnostics.EventLog log = new System.Diagnostics.EventLog("邮件微信流程通知");
                //首先应判断日志来源是否存在，一个日志来源只能同时与一个事件绑定

                if (!EventLog.SourceExists("流程通知"))
                    EventLog.CreateEventSource("流程通知", "邮件微信流程通知");

                log.Source = "流程通知";
                log.WriteEntry(Message, EventLogEntryType.Error);


            }


        }

    }
}
