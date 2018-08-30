using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Topshelf;

namespace Quartz.Console
{
    partial class Service : ServiceBase
    {


        readonly Timer _timer;
        static readonly string fileLogPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        String FileName = String.Empty;
        DirectoryInfo DirectoryName = new DirectoryInfo(fileLogPath);

        Boolean ConStringisInternet = Convert.ToString(ConfigurationManager.AppSettings["ConStringisInternet"]) == "true";

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

                this.JosonJobService(string.Format("Run DateTime {0}", DateTime.Now));
            };
        }


        /// <summary>
        /// OnStart
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {

            this.JosonJobService(string.Format("Start DateTime {0}", DateTime.Now));
        }

        /// <summary>
        /// OnStop
        /// </summary>
        protected override void OnStop()
        {
            this.JosonJobService(string.Format("Stop DateTime {0}", DateTime.Now) + Environment.NewLine);
        }


        protected override void OnPause()
        {
            //服务暂停执行代码
            this.JosonJobService(string.Format("服务暂停执行 DateTime {0}", DateTime.Now) + Environment.NewLine);
            base.OnPause();
        }
        protected override void OnContinue()
        {
            //服务恢复执行代码
            this.JosonJobService(string.Format("服务恢复执行 DateTime {0}", DateTime.Now) + Environment.NewLine);
            base.OnContinue();
        }
        protected override void OnShutdown()
        {
            //系统即将关闭执行代码
            this.JosonJobService(string.Format("系统即将关闭执行 DateTime {0}", DateTime.Now) + Environment.NewLine);
            base.OnShutdown();
        }





        /// <summary>
        /// JosonJobService
        /// </summary>
        /// <param name="context"></param>
        private void JosonJobService(string context)
        {

            try
            {

                log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config"));
                HostFactory.Run(x =>
                {
                    x.UseLog4Net();

                    x.Service<ISchedulers.ScheduleJob.ServiceRunner>();

                    x.RunAsLocalSystem();
                    x.SetDescription("同步通讯录，发送微信短信的服务，请不要给我关闭谢谢管理员啦!");
                    x.SetDisplayName("创维数字-JosnJiang的服务");
                    x.SetServiceName("创维数字-JosnJiang");
                    x.StartAutomatically();
                    x.StartAutomaticallyDelayed();


                    x.EnablePauseAndContinue();

                    if (ConStringisInternet)
                    {
                        ISchedulers.ScheduleJob.Schedule.StartCreateMessagesQueues();
                        ISchedulers.ScheduleJob.Schedule.StartOAuthsEmpUpDate();
                    }

                    ISchedulers.ScheduleJob.Schedule.StartSendMsg();

                    ISchedulers.ScheduleJob.Schedule.StartFlagClass();
                    ISchedulers.ScheduleJob.Schedule.StartWriteLogJob();

                });

                System.Console.WriteLine("JosonJobService");



                if (!DirectoryName.Exists)
                {
                    DirectoryName.Create();
                }

                FileName = fileLogPath + @"\log\" + "Joson.txt";
                FileInfo finfo = new FileInfo(FileName);

                StreamWriter sw = File.AppendText(FileName);
                sw.WriteLine(context);
                sw.Flush();
                sw.Close();



            }
            catch (Exception ex)
            {
                string fileLogPath = AppDomain.CurrentDomain.BaseDirectory + "log\\";
                string fileLogName = "Joson_" + DateTime.Now.ToLongDateString() + "_log.txt";

                DirectoryInfo Dir = new DirectoryInfo(fileLogPath);

                if (!Dir.Exists)
                {
                    Dir.Create();
                }

                FileInfo finfo = new FileInfo(fileLogPath + fileLogName);

                using (FileStream fs = finfo.OpenWrite())
                {
                    //根据上面创建的文件流创建写数据流 
                    StreamWriter strwriter = new StreamWriter(fs);
                    //设置写数据流的起始位置为文件流的末尾 
                    strwriter.BaseStream.Seek(0, SeekOrigin.End);
                    //写入相关记录信息
                    strwriter.WriteLine("发生时间: " + ex.Message + DateTime.Now.ToString());
                    strwriter.WriteLine("---------------------------------------------");
                    strwriter.WriteLine();
                    //清空缓冲区内容，并把缓冲区内容写入基础流 
                    strwriter.Flush();
                    strwriter.Close();
                    fs.Close();
                }

            }




        }



    }
}
