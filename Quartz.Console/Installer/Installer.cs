using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace Quartz.Console
{
    [RunInstaller(true)]
    public partial class Installer : System.Configuration.Install.Installer
    {
        public Installer()
        {
            InitializeComponent();

            processInstaller = new ServiceProcessInstaller();
            serviceInstaller = new ServiceInstaller();

           
            processInstaller.Account = ServiceAccount.LocalSystem;
            serviceInstaller.StartType = ServiceStartMode.Automatic;
            serviceInstaller.DelayedAutoStart = true;

            serviceInstaller.ServiceName = "创维数字  JosnJiang";
            serviceInstaller.Description = "同步通讯录，微信发送超时信息的服务，请不要给我关闭谢谢管理员啦";
            serviceInstaller.DisplayName = "创维数字 JosnJiang的服务";

  
            Installers.Add(serviceInstaller);
            Installers.Add(processInstaller);
        }

    }
}
