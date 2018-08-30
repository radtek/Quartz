
using log4net.Config;
using System;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using Topshelf;

namespace Quartz.Console
{

    class Program
    {
        static void Main(string[] args)
        {



#if !DEBUG
  
  System.Console.WriteLine("!DEBUG");

#endif
 
            #region ServicesToRun

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new Quartz.Console.Service() 

            };
            ServiceBase.Run(ServicesToRun);


            #endregion


 

            ////调度器构造工厂
            //ISchedulerFactory factory = new StdSchedulerFactory();

            ////第一步：构造调度器
            //IScheduler scheduler = factory.GetScheduler();
            //scheduler.Start();//启动调度器

            ////第二步：定义任务
            //JobDetailImpl job = new JobDetailImpl("JosonJiang", typeof(HandleClass));

            ////第三步：定义触发器
            //ITrigger trigger = TriggerBuilder.Create()
            //    .WithIdentity("JosonJiang")//触发器名称
            //    .ForJob(job)
            //    .StartNow()
            //    .WithSimpleSchedule(x => x.RepeatForever()
            //        .WithIntervalInSeconds(1))//1s 执行一次 
            //    .Build();

            ////将任务与触发器添加到调度器中：
            //scheduler.ScheduleJob(job, trigger);



            #region TownCrier

            //var logCfg = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config");
            //XmlConfigurator.ConfigureAndWatch(logCfg);

            //HostFactory.Run(x =>
            //{

            //    x.UseLog4Net();

            //    x.Service<TownCrier>(s =>
            //    {
            //        s.ConstructUsing(name => new TownCrier());
            //        s.WhenStarted(tc => tc.Start());
            //        s.WhenStopped(tc => tc.Stop());
            //    });
            //    x.RunAsLocalSystem();

            //    x.SetDescription("Sample Topshelf Host服务的描述");
            //    x.SetDisplayName("Stuff显示名称");
            //    x.SetServiceName("Stuff服务名称");
            //}); 

            #endregion



            #region ISchedulers.ScheduleJob.ServiceRunner

            //log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config"));
            //HostFactory.Run(x =>
            //{
            //    x.UseLog4Net();

            //    x.Service<ISchedulers.ScheduleJob.ServiceRunner>();

            //    x.SetDescription("同步通讯录，发送微信短信的服务，请不要给我关闭谢谢管理员啦");
            //    x.SetDisplayName("JosnJiang的服务");
            //    x.SetServiceName("JosnJiang");

            //    x.EnablePauseAndContinue();
            //}); 

            #endregion



#if DEBUG

            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config"));
            HostFactory.Run(x =>
            {
                x.UseLog4Net();

                x.Service<ISchedulers.ScheduleJob.ServiceRunner>();

                x.RunAsLocalSystem();
                x.SetDescription("同步通讯录，发送微信短信的服务，请不要给我关闭谢谢管理员啦");
                x.SetDisplayName("创维数字-JosnJiang的服务");
                x.SetServiceName("创维数字-JosnJiang");
                x.StartAutomatically();
                x.StartAutomaticallyDelayed();


                x.EnablePauseAndContinue();

                //ISchedulers.ScheduleJob.Schedule Schedule = new ISchedulers.ScheduleJob.Schedule();


                ISchedulers.ScheduleJob.Schedule.StartCreateMessagesQueues();
                ISchedulers.ScheduleJob.Schedule.StartOAuthsEmpUpDate();

                ISchedulers.ScheduleJob.Schedule.StartSendMsg();

                ISchedulers.ScheduleJob.Schedule.StartFlagClass();
                ISchedulers.ScheduleJob.Schedule.StartWriteLogJob();

            });

#endif

            //Console.ReadKey();

        }
    }

    ///// <summary>
    ///// 工作类
    ///// </summary>
    //public class HandleClass : IJob
    //{
    //    /// <summary>
    //    /// 任务调用的方法
    //    /// </summary>
    //    public void Execute(IJobExecutionContext context)
    //    {
    //        Console.WriteLine("1s 执行一次  --JosonJiang !");
    //    }
    //}



    //public class TownCrier
    //{
    //    private System.Timers.Timer _timer = null;
    //    readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(TownCrier));
    //    public TownCrier()
    //    {
    //        _timer = new System.Timers.Timer(1000) { AutoReset = true };

    //        _timer.Elapsed += (sender, eventArgs) => _log.Info(DateTime.Now);
    //        _timer.Elapsed += (sender, eventArgs) => Console.WriteLine("------------JosonJiang---------");

    //    }
    //    public void Start() { _timer.Start(); }
    //    public void Stop() { _timer.Stop(); }

    //}


}
