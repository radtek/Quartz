
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISchedulers.ScheduleJob
{
    using Quartz;
    using Quartz.Impl;


    public class Schedule
    {

        static IScheduler scheduler = null;

        static Schedule _instance = null;
        static object lockObj = new object();


        /// <summary>
        /// 线程安全的单例对象
        /// </summary>
        public static Schedule Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new Schedule();
                        }
                    }
                }
                return _instance;
            }
        }

 

        /// <summary>
        /// 数据库同步
        /// </summary>
        public static void StartCreateMessagesQueues()
        {
 
            //调度器构造工厂
            ISchedulerFactory factory = new StdSchedulerFactory();
 
            //第一步：构造调度器
            scheduler = factory.GetScheduler();
            scheduler.Start();//启动调度器


            //第二步：定义任务
            JobDetailImpl job = new JobDetailImpl("JosonJiangCreateMessagesQueues", typeof(HandleCreateMessagesQueues));

            //第三步：定义触发器
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("JosonJiangCreateMessagesQueues")//触发器名称
                .ForJob(job)
                .StartNow()
                .WithSimpleSchedule(x => x.RepeatForever()
                .WithIntervalInSeconds(1))                  //1s 执行一次 
                .Build();

            //将任务与触发器添加到调度器中：
            scheduler.ScheduleJob(job, trigger);

 

        }
 


        /// <summary>
        /// 数据库同步
        /// </summary>
        public static void StartOAuthsEmpUpDate()
        {

            // start up scheduler
            // construct a factory 
            ISchedulerFactory factory = new StdSchedulerFactory();
            // get a scheduler 
            scheduler = factory.GetScheduler();
            // start the scheduler 
            scheduler.Start();

            DateTimeOffset runTime = DateBuilder.EvenMinuteDate(DateTimeOffset.UtcNow.AddDays(-1));

            // create job  
            IJobDetail job = JobBuilder.Create<HandleOAuthsEmpUpDate>()
                        .WithIdentity("JosonJiang", "JosonJiang")
                        .Build();

            // create trigger  
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("JosonJiangJobTrigger", "JosonJiangJobs")
                // start at 7:30 every day
                //.StartAt(DateBuilder.DateOf(7, 30, 0))
                .StartAt(runTime)
                .WithSimpleSchedule(x => x.WithInterval(TimeSpan.FromMilliseconds(1)).WithRepeatCount(0))

                .Build();

            // Schedule the job using the job and trigger   
            scheduler.ScheduleJob(job, trigger);


        }

        /// <summary>
        /// 
        /// </summary>
        public static void Stop()
        {
            scheduler.Shutdown(true);
        }



        /// <summary>
        /// 运行标示
        /// </summary>
        public static void StartFlagClass()
        {

            //调度器构造工厂
            ISchedulerFactory factory = new StdSchedulerFactory();

            //第一步：构造调度器
            scheduler = factory.GetScheduler();
            scheduler.Start();//启动调度器

            //第二步：定义任务
            JobDetailImpl job = new JobDetailImpl("JosonJiang", typeof(HandleFlagClass));

            //第三步：定义触发器
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("JosonJiang")//触发器名称
                .ForJob(job)
                .StartNow()
                .WithSimpleSchedule(x => x.RepeatForever()
                .WithIntervalInSeconds(10))//10s 执行一次 
                .Build();

            //将任务与触发器添加到调度器中：
            scheduler.ScheduleJob(job, trigger);


        }

        /// <summary>
        /// 消息发送
        /// </summary>
        public static void StartSendMsg()
        {

            //调度器构造工厂
            ISchedulerFactory factory = new StdSchedulerFactory();

            //第一步：构造调度器
            scheduler = factory.GetScheduler();
            scheduler.Start();//启动调度器

            //第二步：定义任务
            JobDetailImpl job = new JobDetailImpl("JosonJiangHandleSendMsg", typeof(HandleSendMsg));

            //第三步：定义触发器
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("JosonJiangHandleSendMsg")//触发器名称
                .ForJob(job)
                .StartNow()
                .WithSimpleSchedule(x => x.RepeatForever()
                .WithIntervalInSeconds(3600))//3600s 执行一次 
                .Build();

            //将任务与触发器添加到调度器中：
            scheduler.ScheduleJob(job, trigger);


        }




        /// <summary>
        /// 日志测试
        /// </summary>
        public static void StartWriteLogJob()
        {

            //调度器构造工厂
            ISchedulerFactory factory = new StdSchedulerFactory();

            //第一步：构造调度器
            scheduler = factory.GetScheduler();
            scheduler.Start();//启动调度器

            //第二步：定义任务
            JobDetailImpl job = new JobDetailImpl("StartWriteLogJob", typeof(WriteLogJob));

            //第三步：定义触发器
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("StartWriteLogJob")//触发器名称
                .ForJob(job)
                .StartNow()
                .WithSimpleSchedule(x => x.RepeatForever()
                .WithIntervalInSeconds(1))//1s 执行一次 
                .Build();

            //将任务与触发器添加到调度器中：
            scheduler.ScheduleJob(job, trigger);

        }
    }


}
