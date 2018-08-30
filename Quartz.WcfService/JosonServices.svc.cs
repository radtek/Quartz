using Net.Common;
using Quartz.Entity;
using Senparc.Weixin.BLL;
using Senparc.Weixin.IBLL;
using Senparc.Weixin.MP.ISchedulers.ScheduleJob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;

namespace Quartz.WcfService
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“JosonServices”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 JosonServices.svc 或 JosonServices.svc.cs，然后开始调试。
    //[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class JosonServices : IJosonServices
    {
        //String DispalyName = String.Empty;
        //String FromEmail = String.Empty;
        //String FromServer = String.Empty;
        //String PassWord = String.Empty;

        Boolean isSucessful = false;

        public Boolean SendMsg(String 接受者 = "SDT12872", String 微信内容 = "微信内容", String 信息内容 = "信息内容", int ID = 0, Boolean isDebug = true)
        {

            String[] Msg = 微信内容.Split('|');
            String DispalyName = "BPM系统通知邮件";
            String JosonNotice = @"<BR><BR><div style='border:1px solid #efefef; padding:5px;'><font color='blue'>请关注微信服务号“创维数字移动办公”，将微信号和BPM账号绑定 ，本通知将会通过微信消息推送给您！</font><BR><font color='red'>本邮件为系统自动发送，请勿直接回复 <BR>" + DateTime.Now + "</font></div>";


            string[] addrItems = 接受者.Split(new char[] { ';' });

            if (Msg.Length == 1)
            {

                微信内容 = 微信内容.Replace("\r\n", " ");
                微信内容 = 微信内容.Replace('\r', ' ');
                微信内容 = 微信内容.Replace('\n', ' ');

                信息内容 = 信息内容 + JosonNotice;

                foreach (string addrItem in addrItems)
                {
                    if (!isDebug)
                        isSucessful = SendMail.SendEmail(DispalyName, addrItem, 微信内容, 信息内容);

                }

            }
            else
            {

                ISubscribeUsersService _ISubscribeUsersService = new SubscribeUsersService();
                //IQueryable<SubscribeUsers> UserLst = _ISubscribeUsersService.LoadEntities(o => o.EmpID == 接受者);
                SubscribeUsers SubscribeUsers = _ISubscribeUsersService.LoadEntities(o => o.Email == 接受者).SingleOrDefault();

                String 发送内容 = 微信内容.Split('|')[1];

                if (SubscribeUsers == null)
                {
                    微信内容 = 微信内容.Split('|')[0];
                    //Net.Common.SendJMail.SendMail();

                    信息内容 = 信息内容 + JosonNotice;

                    if (!isDebug)
                    {
                        foreach (string addrItem in addrItems)
                        {
                            微信内容 = 微信内容.Replace("\r\n", " ");
                            微信内容 = 微信内容.Replace('\r', ' ');
                            微信内容 = 微信内容.Replace('\n', ' ');

                            isSucessful = SendMail.SendEmail(DispalyName, addrItem, 微信内容, 信息内容);

                        }
                    }
                }
                else
                {

                    String[] this发送内容 = 发送内容.SplitString("->");

                    String 微信类型 = this发送内容.Length > 1 ? 发送内容.SplitString("->")[1] : this发送内容[0];
                    String 微信的内容 = this发送内容[0];

                    Joson Joson = new Senparc.Weixin.MP.ISchedulers.ScheduleJob.Joson();
                    Template Template = Template.流程办结提醒;

                    switch (微信类型)
                    {
                        case "流程办结提醒": Template = Template.流程办结提醒; break;
                        case "待办任务提醒": Template = Template.待办任务提醒; break;
                        case "流程待办提醒": Template = Template.流程待办提醒; break;
                        case "审批结果提醒": Template = Template.审批结果提醒; break;
                        case "流程待审批事项": Template = Template.流程待审批事项; break;

                        case "拒绝通知": Template = Template.拒绝通知; break;
                        case "撤销通知": Template = Template.撤销通知; break;
                        case "删除通知": Template = Template.删除通知; break;
                        case "退回通知": Template = Template.退回通知; break;
                        case "阅示通知": Template = Template.阅示通知; break;
                        case "同意通知": Template = Template.同意通知; break;

                        default: Template = Template.通用信息提醒;
                            break;
                    }


                    isSucessful = Joson.SendMessage(Template, 接受者, 微信的内容, ID);

                }
            }

            return isSucessful;

        }


    }
}
