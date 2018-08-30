using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace Quartz.Consoles
{
    class Program
    {
        static void Main(string[] args)
        {

 
            #region ServicesToRun

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new Quartz.Consoles.Service() 

            };

            ServiceBase.Run(ServicesToRun);


            #endregion


#if DEBUG
            String strMsg = String.Empty;

            String 接受者 = "JiangXiaoPeng@skyworth.Com";
            String 流程名称及流程编号 = "流程名称及流程编号";
            Quartz.SendMsgSerives.Start.Run(接受者, 流程名称及流程编号);


            strMsg = Start.SendMessagesQueue(false);

            Console.WriteLine(String.Format("当前状态：{0}\n\r\n", strMsg));

#endif

           
        }
    }
}
