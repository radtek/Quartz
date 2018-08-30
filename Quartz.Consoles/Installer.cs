using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;

namespace Quartz.Consoles
{
    [RunInstaller(true)]
    public partial class Installer : System.Configuration.Install.Installer
    {

        private ServiceInstaller serviceInstaller;
        private ServiceProcessInstaller processInstaller;

        public Installer()
        {
            InitializeComponent();

            processInstaller = new ServiceProcessInstaller();
            serviceInstaller = new ServiceInstaller();

            processInstaller.Account = ServiceAccount.LocalSystem;
            serviceInstaller.StartType = ServiceStartMode.Automatic;

            serviceInstaller.ServiceName = "JosnJiang";
            serviceInstaller.Description = "微信流程通知，发送微信短信的服务，请不要给我关闭谢谢管理员啦";
            serviceInstaller.DisplayName = "微信流程通知服务";


            Installers.Add(serviceInstaller);
            Installers.Add(processInstaller);
        }
    }
}
