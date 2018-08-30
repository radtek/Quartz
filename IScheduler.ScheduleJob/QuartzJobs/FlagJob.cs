using Common.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISchedulers.ScheduleJob
{
    public sealed class FlagJob : IJob
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(FlagJob));

        public void Execute(IJobExecutionContext context)
        {
            _logger.InfoFormat("FlagJob测试");

            //Console.WriteLine("TestJob成功执行");
        }
    }
}
