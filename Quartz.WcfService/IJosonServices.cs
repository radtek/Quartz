using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Quartz.WcfService
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IJosonServices”。
    [ServiceContract]
    public interface IJosonServices
    {
        [OperationContract]
        Boolean SendMsg(String 接受者 = "SDT12872", String 微信内容 = "微信内容", String 信息内容 = "信息内容", int ID = 0, Boolean isDebug = true);
    }
}
