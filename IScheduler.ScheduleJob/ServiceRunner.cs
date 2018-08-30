﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ISchedulers.ScheduleJob
{
    using Quartz;
    using Quartz.Impl;
    using Topshelf;

    /// <summary>
    /// 任务调度服务
    /// </summary>
    public sealed class ServiceRunner : ServiceControl, ServiceSuspend
    {
        private readonly IScheduler scheduler;

        public ServiceRunner()
        {
            scheduler = StdSchedulerFactory.GetDefaultScheduler();
        }

        public bool Start(HostControl hostControl)
        {
            scheduler.Start();
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            scheduler.Shutdown(false);
            return true;
        }

        public bool Continue(HostControl hostControl)
        {
            scheduler.ResumeAll();
            return true;
        }

        public bool Pause(HostControl hostControl)
        {
            scheduler.PauseAll();
            return true;
        }


    }
}
