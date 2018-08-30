using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Senparc.Weixin.MP.ISchedulers.ScheduleJob
{
    using Quartz.Entity;
    using Senparc.Weixin.BLL;
    using Senparc.Weixin.IBLL;
    using Senparc.Weixin.MP;
    using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
    using Senparc.Weixin.MP.CommonAPIs;
    using Net.Common;
    using System.Diagnostics;
    using ISchedulers.ScheduleJob;


    //public enum Template
    //{
    //    微信解绑通知,
    //    待办任务提醒,
    //    待办事项提醒,
    //    流程待办提醒,
    //    流程催办提醒,
    //    流程办结提醒,
    //    审批结果提醒,
    //    用户登录提醒
    //}

    public class TemplateData
    {
        public TemplateDataItem first { get; set; }

        public TemplateDataItem keyword1 { get; set; }
        public TemplateDataItem keyword2 { get; set; }
        public TemplateDataItem keyword3 { get; set; }
        public TemplateDataItem keyword4 { get; set; }
        public TemplateDataItem keyword5 { get; set; }
        public TemplateDataItem keyword6 { get; set; }
        public TemplateDataItem time { get; set; }
        public TemplateDataItem ip { get; set; }
        public TemplateDataItem reason { get; set; }

        public TemplateDataItem remark { get; set; }

    }



    public class Joson : AppConfigs
    {
        String ConnectioniAnyWhere = System.Configuration.ConfigurationSettings.AppSettings["ConnectioniAnyWhere"];
        String ConnectionEmail = System.Configuration.ConfigurationSettings.AppSettings["ConnectionEmail"];

        // ConnectioniAnyWhere= System.Configuration.ConfigurationManager.AppSettings["ConnectioniAnyWhere"];
        // AppConfigs Configs = new AppConfigs();

        String OpenID = new AppConfigs().GetTestOpenID();
        //String AppID = new AppConfigs().AppConfig.AppId;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="EmpID"></param>
        /// <returns></returns>
        public String GetOpenIDByEmpID(String EmpID)
        {
            ISubscribeUsersService _ISubscribeUsersService = new SubscribeUsersService();
            IQueryable<SubscribeUsers> UserLst = _ISubscribeUsersService.LoadEntities(o => o.EmpID == EmpID);

            if (UserLst != null) { OpenID = UserLst.SingleOrDefault().OpenID; }

            return OpenID;

        }


        #region 指定发送人

        /// <summary>
        /// 测试时用 指定发送人
        /// </summary>
        /// <param name="PROCNAME"></param>
        /// <returns></returns>
        public String SendOpenID(String PROCNAME)
        {
            String EmpID = "SDT12872";
            String OpenID = String.Empty;

            switch (PROCNAME)
            {
                case "宽带事业订单评审": OpenID = GetOpenIDByEmpID(EmpID); break;
                case "宽带事业订单更改评审": OpenID = GetOpenIDByEmpID(EmpID); break;

                case "海外运营订单更改评审": OpenID = GetOpenIDByEmpID(EmpID); break;
                case "海外运营订单评审": OpenID = GetOpenIDByEmpID(EmpID); break;
                case "海外返工订单评审": OpenID = GetOpenIDByEmpID(EmpID); break;

                case "国内订单取消流程": OpenID = GetOpenIDByEmpID(EmpID); break;
                case "国内运营订单评审": OpenID = GetOpenIDByEmpID(EmpID); break;
                case "国内、宽带返工订单更改评审": OpenID = GetOpenIDByEmpID(EmpID); break;
                case "国内运营订单更改评审": OpenID = GetOpenIDByEmpID(EmpID); break;
                case "国内、宽带返工订单评审": OpenID = GetOpenIDByEmpID(EmpID); break;

                case "备损订单评审": OpenID = GetOpenIDByEmpID(EmpID); break;
                case "备损订单更改评审": OpenID = GetOpenIDByEmpID(EmpID); break;

                case "售后订单评审": OpenID = GetOpenIDByEmpID(EmpID); break;

                default:

                    OpenID = GetOpenIDByEmpID(EmpID); break;
            }

            OpenID = new AppConfigs().GetTestOpenID();

            return OpenID;
        }

        #endregion

        #region 创建消息模板
        /// <summary>
        /// 创建消息模板
        /// </summary>
        /// <param name="StrMsg"></param>
        /// <returns></returns>
        public TemplateData CreateTemplate(String StrMsg)
        {
            TemplateData TemplateData = new TemplateData();

            TemplateData.first = new TemplateDataItem("【任务提醒】您好，你有任务需处理", "#FF0000");
            TemplateData.remark = new TemplateDataItem("你有一个任务提醒哦，请及时处理。更详细信息，请到登录BPM电子流网站（http://ehome.SDT.com）查看！");

            if (StrMsg.NotIsNullOrEmpty())
            {
                int index = 1;
                string[] KeyWords = StrMsg.SplitString("\r\n");

                foreach (String kWord in KeyWords)
                {
                    TemplateDataItem TemplateDataItem = new TemplateDataItem(kWord);

                    #region 创建消息模板

                    switch (index)
                    {

                        case 1: TemplateData.keyword1 = TemplateDataItem; break;
                        case 2: TemplateData.keyword2 = TemplateDataItem; break;
                        case 3: TemplateData.keyword3 = TemplateDataItem; break;
                        case 4: TemplateData.keyword4 = TemplateDataItem; break;
                        case 5: TemplateData.keyword5 = TemplateDataItem; break;
                        case 6: TemplateData.keyword6 = TemplateDataItem; break;

                        default:
                            break;
                    }

                    #endregion

                    index++;

                }
            }
            return TemplateData;

        }

        #endregion


        /// <summary>
        /// 发送信息
        /// </summary>
        /// <param name="_OpenId"></param>
        /// <param name="Template"></param>
        public bool SendMessage(Template Template = Template.流程催办提醒, String strEmpID = null, String 信息内容 = null, int ID = 0)
        {
            String OpenID = String.Empty;
            String URL = String.Empty;

            TemplateData AutoCreateTemplate = new TemplateData();
            SubscribeUsers SubscribeUsers = null;

            //JosonEntity.JosonReflection JosonEntity = new JosonEntity.JosonReflection();

            string templateId = "uLJONvcsIA0JhaUc25nUsVeph9T-LLbQpjlOjqmAm3s";//换成已经在微信后台添加的模板Id
            string accessToken = String.Empty;

            String FirstMsg = "{0},你有一个流程已经被{1}";
            String RemarkMsg = "你的一个流程已经被{0},请点击查看详细内容，或登录BPM电子流查看相关信息。";

            var testData = new TemplateData()
            {
                first = new TemplateDataItem("【待办任务】您好，你有任务需处理", "#FF0000"),
                remark = new TemplateDataItem("客户端出现无法登录，请及时处理。更详细信息，请到登录BPM电子流网站（http://www.JosonJiang.com）查看！")
            };


            ISubscribeUsersService _ISubscribeUsersService = new SubscribeUsersService();
            IQueryable<SubscribeUsers> UserLst = _ISubscribeUsersService.LoadEntities(o => o.OpenID.Length > 0);

            SendTemplateMessageResult result = null;

            if (String.IsNullOrWhiteSpace(accessToken))
                accessToken = AccessTokenContainer.GetAccessToken(AppID);

            switch (Template)
            {

                case Template.流程办结提醒:

                    SubscribeUsers = _ISubscribeUsersService.LoadEntities(o => o.Email == strEmpID).SingleOrDefault();

                    templateId = "AqBXBXc1vVR-bMSOLvVYL9H_wWGJmJ6G6PJkLB_gnW0";
                    testData = new TemplateData()
                    {
                        first = new TemplateDataItem(String.Format("{0}您好，您发起申请的流程已处理结束。", SubscribeUsers.Remark), "#FF0000"),

                        keyword1 = new TemplateDataItem(信息内容),
                        keyword2 = new TemplateDataItem(DateTime.Now.ToString(), "#FF3300"),
                        remark = new TemplateDataItem("您可以进行后续的工作安排。更详细信息，请到登录BPM电子流网站查看！")
                    };

                    AutoCreateTemplate = CreateTemplate(信息内容);
                    AutoCreateTemplate = JosonReflection.CopyFromEntity<TemplateData, TemplateData>(testData);




                    if (SubscribeUsers != null)
                    {
                        OpenID = SubscribeUsers.OpenID;

                        URL = String.Format("{0}?ID={1}&isweixin=1", ConnectionEmail, ID);

                        if (!String.IsNullOrWhiteSpace(OpenID))
                        {
                            result = Senparc.Weixin.MP.AdvancedAPIs.TemplateApi.SendTemplateMessage(accessToken, OpenID, templateId, "#FFFF00", URL, AutoCreateTemplate);

                            if (ReturnCode.请求成功.Equals(result.errcode))
                            {
                                System.Threading.Thread.Sleep(1000);
                            }
                        }

                    }
                    break;

                case Template.流程待办提醒:

                    templateId = "oJVBysZMEzhop9jeReRDlJsTOJ3VVIJqrHrh1UQ1aMc";

                    //{{first.DATA}}
                    //流程编号：{{keyword1.DATA}}
                    //流程名称：{{keyword2.DATA}}
                    //发起时间：{{keyword3.DATA}}
                    //流程摘要：{{keyword4.DATA}}
                    //{{remark.DATA}}

                    //您好，您有一个流程待办需要处理。
                    //流程编号：YBHT-123456789
                    //流程名称：一般合同审批
                    //发起时间：2015.10.08  12:12:12
                    //流程摘要：部门 信息管理部  发起人 张三，  合同名称 同城灾备项目合同  紧急度 高  
                    //请尽快审批，截至当前，您还有3个待办事宜未处理。

                    testData.first = new TemplateDataItem("您好，您有一个流程待办需要处理");
                    testData.remark = new TemplateDataItem("请点击查看详细内容，或登录BPM电子流查看相关信息。");


                    AutoCreateTemplate = CreateTemplate(信息内容);
                    //AutoCreateTemplate = JosonEntity.CopyFromEntity<TemplateData, TemplateData>(testData,false);
                    //AutoCreateTemplate = AutoCreateTemplate.CopyEntity<TemplateData>(testData);
                    AutoCreateTemplate = AutoCreateTemplate.CopyFrom<TemplateData>(testData);

                    SubscribeUsers = _ISubscribeUsersService.LoadEntities(o => o.Email == strEmpID).SingleOrDefault();

                    if (SubscribeUsers != null)
                    {
                        OpenID = SubscribeUsers.OpenID;
                        URL = String.Format("{0}?ID={1}&isweixin=1", ConnectionEmail, ID);

                        if (!String.IsNullOrWhiteSpace(OpenID))
                        {
                            result = Senparc.Weixin.MP.AdvancedAPIs.TemplateApi.SendTemplateMessage(accessToken, OpenID, templateId, "#FFFF00", URL, AutoCreateTemplate);

                            if (ReturnCode.请求成功.Equals(result.errcode))
                            {
                                System.Threading.Thread.Sleep(1000);
                            }
                        }

                    }

                    break;

                case Template.流程待审批事项:

                    templateId = "oJVBysZMEzhop9jeReRDlJsTOJ3VVIJqrHrh1UQ1aMc";

                    testData.first = new TemplateDataItem("您好，您有一个流程待办需要处理");
                    testData.remark = new TemplateDataItem("请点击查看详细内容，或登录BPM电子流查看相关信息。");

                    AutoCreateTemplate = CreateTemplate(信息内容);
                    //AutoCreateTemplate = JosonEntity.CopyFromEntity<TemplateData, TemplateData>(testData,false);
                    //AutoCreateTemplate = AutoCreateTemplate.CopyEntity<TemplateData>(testData);
                    AutoCreateTemplate = AutoCreateTemplate.CopyFrom<TemplateData>(testData);

                    SubscribeUsers = _ISubscribeUsersService.LoadEntities(o => o.Email == strEmpID).SingleOrDefault();

                    if (SubscribeUsers != null)
                    {
                        OpenID = SubscribeUsers.OpenID;
                        URL = String.Format("{0}?ID={1}&isweixin=1", ConnectionEmail, ID);

                        if (!String.IsNullOrWhiteSpace(OpenID))
                        {
                            result = Senparc.Weixin.MP.AdvancedAPIs.TemplateApi.SendTemplateMessage(accessToken, OpenID, templateId, "#FFFF00", URL, AutoCreateTemplate);

                            if (ReturnCode.请求成功.Equals(result.errcode))
                            {
                                System.Threading.Thread.Sleep(1000);
                            }
                        }

                    }

                    break;

                case Template.撤销通知:

                    //审批结果提醒
                    //{{first.DATA}}
                    //审批事项：{{keyword1.DATA}}
                    //审批结果：{{keyword2.DATA}}

                    //孔连顺，您好：
                    //审批事项:『银电网络智能监控系统』项目差旅费报销，金额为￥215元整。
                    //审批结果:通过
                    //2014/8/25 10:12:22



                    SubscribeUsers = _ISubscribeUsersService.LoadEntities(o => o.Email == strEmpID).SingleOrDefault();
                    FirstMsg = String.Format(FirstMsg, SubscribeUsers.Remark, "撤销");
                    RemarkMsg = String.Format(RemarkMsg, "撤销");


                    templateId = "G4tEz6j-rRoPVgQZz3p5CQdsTLuS5yeppwPyBB6Xo0w";
                    testData.first = new TemplateDataItem(FirstMsg);
                    testData.remark = new TemplateDataItem(RemarkMsg);

                    AutoCreateTemplate = CreateTemplate(信息内容);
                    AutoCreateTemplate = AutoCreateTemplate.CopyFrom<TemplateData>(testData);

                    if (SubscribeUsers != null)
                    {
                        OpenID = SubscribeUsers.OpenID;
                        URL = String.Format("{0}?ID={1}&isweixin=1", ConnectionEmail, ID);

                        if (!String.IsNullOrWhiteSpace(OpenID))
                        {
                            result = Senparc.Weixin.MP.AdvancedAPIs.TemplateApi.SendTemplateMessage(accessToken, OpenID, templateId, "#FFFF00", URL, AutoCreateTemplate);

                            if (ReturnCode.请求成功.Equals(result.errcode))
                            {
                                System.Threading.Thread.Sleep(1000);
                            }
                        }

                    }

                    break;

                case Template.拒绝通知:


                    SubscribeUsers = _ISubscribeUsersService.LoadEntities(o => o.Email == strEmpID).SingleOrDefault();
                    FirstMsg = String.Format(FirstMsg, SubscribeUsers.Remark, "拒绝");
                    RemarkMsg = String.Format(RemarkMsg, "拒绝");


                    templateId = "G4tEz6j-rRoPVgQZz3p5CQdsTLuS5yeppwPyBB6Xo0w";
                    testData.first = new TemplateDataItem(FirstMsg);
                    testData.remark = new TemplateDataItem(RemarkMsg);

                    AutoCreateTemplate = CreateTemplate(信息内容);
                    AutoCreateTemplate = AutoCreateTemplate.CopyFrom<TemplateData>(testData);

                    if (SubscribeUsers != null)
                    {
                        OpenID = SubscribeUsers.OpenID;
                        URL = String.Format("{0}?ID={1}&isweixin=1", ConnectionEmail, ID);

                        if (!String.IsNullOrWhiteSpace(OpenID))
                        {
                            result = Senparc.Weixin.MP.AdvancedAPIs.TemplateApi.SendTemplateMessage(accessToken, OpenID, templateId, "#FFFF00", URL, AutoCreateTemplate);

                            if (ReturnCode.请求成功.Equals(result.errcode))
                            {
                                System.Threading.Thread.Sleep(1000);
                            }
                        }

                    }

                    break;

                case Template.同意通知:


                    SubscribeUsers = _ISubscribeUsersService.LoadEntities(o => o.Email == strEmpID).SingleOrDefault();

                    FirstMsg = String.Format("{0},你有一个流程已经被同意。", SubscribeUsers.Remark);
                    RemarkMsg = String.Format("你的一个流程已经被同意。请点击查看详细内容，或登录BPM电子流查看相关信息");


                    templateId = "G4tEz6j-rRoPVgQZz3p5CQdsTLuS5yeppwPyBB6Xo0w";
                    testData.first = new TemplateDataItem(FirstMsg);
                    testData.remark = new TemplateDataItem(RemarkMsg);

                    AutoCreateTemplate = CreateTemplate(信息内容);
                    AutoCreateTemplate = AutoCreateTemplate.CopyFrom<TemplateData>(testData);



                    if (SubscribeUsers != null)
                    {
                        OpenID = SubscribeUsers.OpenID;
                        URL = String.Format("{0}?ID={1}&isweixin=1", ConnectionEmail, ID);

                        if (!String.IsNullOrWhiteSpace(OpenID))
                        {
                            result = Senparc.Weixin.MP.AdvancedAPIs.TemplateApi.SendTemplateMessage(accessToken, OpenID, templateId, "#FFFF00", URL, AutoCreateTemplate);

                            if (ReturnCode.请求成功.Equals(result.errcode))
                            {
                                System.Threading.Thread.Sleep(1000);
                            }
                        }

                    }

                    break;

                case Template.删除通知:

                    SubscribeUsers = _ISubscribeUsersService.LoadEntities(o => o.Email == strEmpID).SingleOrDefault();
                    FirstMsg = String.Format(FirstMsg, SubscribeUsers.Remark, "删除");
                    RemarkMsg = String.Format(RemarkMsg, "删除");


                    templateId = "G4tEz6j-rRoPVgQZz3p5CQdsTLuS5yeppwPyBB6Xo0w";
                    testData.first = new TemplateDataItem(FirstMsg);
                    testData.remark = new TemplateDataItem(RemarkMsg);

                    AutoCreateTemplate = CreateTemplate(信息内容);
                    AutoCreateTemplate = AutoCreateTemplate.CopyFrom<TemplateData>(testData);

                    if (SubscribeUsers != null)
                    {
                        OpenID = SubscribeUsers.OpenID;
                        URL = String.Format("{0}?ID={1}&isweixin=1", ConnectionEmail, ID);

                        if (!String.IsNullOrWhiteSpace(OpenID))
                        {
                            result = Senparc.Weixin.MP.AdvancedAPIs.TemplateApi.SendTemplateMessage(accessToken, OpenID, templateId, "#FFFF00", URL, AutoCreateTemplate);

                            if (ReturnCode.请求成功.Equals(result.errcode))
                            {
                                System.Threading.Thread.Sleep(1000);
                            }
                        }

                    }

                    break;

                case Template.退回通知:

                    SubscribeUsers = _ISubscribeUsersService.LoadEntities(o => o.Email == strEmpID).SingleOrDefault();
                    FirstMsg = String.Format(FirstMsg, SubscribeUsers.Remark, "退回");
                    RemarkMsg = String.Format(RemarkMsg, "退回");


                    templateId = "G4tEz6j-rRoPVgQZz3p5CQdsTLuS5yeppwPyBB6Xo0w";
                    testData.first = new TemplateDataItem(FirstMsg);
                    testData.remark = new TemplateDataItem(RemarkMsg);

                    AutoCreateTemplate = CreateTemplate(信息内容);
                    AutoCreateTemplate = AutoCreateTemplate.CopyFrom<TemplateData>(testData);

                    if (SubscribeUsers != null)
                    {
                        OpenID = SubscribeUsers.OpenID;
                        URL = String.Format("{0}?ID={1}&isweixin=1", ConnectionEmail, ID);

                        if (!String.IsNullOrWhiteSpace(OpenID))
                        {
                            result = Senparc.Weixin.MP.AdvancedAPIs.TemplateApi.SendTemplateMessage(accessToken, OpenID, templateId, "#FFFF00", URL, AutoCreateTemplate);

                            if (ReturnCode.请求成功.Equals(result.errcode))
                            {
                                System.Threading.Thread.Sleep(1000);
                            }
                        }

                    }

                    break;

                case Template.阅示通知:

                    SubscribeUsers = _ISubscribeUsersService.LoadEntities(o => o.Email == strEmpID).SingleOrDefault();

                    FirstMsg = String.Format("{0},你有一个流程需要处理。", SubscribeUsers.Remark);
                    RemarkMsg = String.Format("请点击查看详细内容，或登录BPM电子流查看相关信息");


                    templateId = "G4tEz6j-rRoPVgQZz3p5CQdsTLuS5yeppwPyBB6Xo0w";
                    testData.first = new TemplateDataItem(FirstMsg);
                    testData.remark = new TemplateDataItem(RemarkMsg);

                    AutoCreateTemplate = CreateTemplate(信息内容);
                    AutoCreateTemplate = AutoCreateTemplate.CopyFrom<TemplateData>(testData);

                    if (SubscribeUsers != null)
                    {
                        OpenID = SubscribeUsers.OpenID;
                        URL = String.Format("{0}?ID={1}&isweixin=1", ConnectionEmail, ID);

                        if (!String.IsNullOrWhiteSpace(OpenID))
                        {
                            result = Senparc.Weixin.MP.AdvancedAPIs.TemplateApi.SendTemplateMessage(accessToken, OpenID, templateId, "#FFFF00", URL, AutoCreateTemplate);

                            if (ReturnCode.请求成功.Equals(result.errcode))
                            {
                                System.Threading.Thread.Sleep(1000);
                            }
                        }

                    }

                    break;

                case Template.通用信息提醒:

                    //流程操作通知
                    //{{first.DATA}}
                    //通知内容：{{keyword1.DATA}}
                    //通知时间：{{keyword2.DATA}}
                    //{{remark.DATA}}

                    //您有一个通知请查收。
                    //通知内容：部门经理审核通过，请进行下一步。
                    //通知时间：2015-09-23
                    //点击详情，查看明细。


                    SubscribeUsers = _ISubscribeUsersService.LoadEntities(o => o.Email == strEmpID).SingleOrDefault();

                    FirstMsg = String.Format("{0},你有一个流程需要处理。", SubscribeUsers.Remark);
                    RemarkMsg = String.Format("请点击查看详细内容，或登录BPM电子流查看相关信息");

                    templateId = "cC6lIBWSHR3AaDccY0sm4yXrJeLPzhpVhDZOSQoP0P0";
                    testData.first = new TemplateDataItem(FirstMsg);
                    testData.remark = new TemplateDataItem(RemarkMsg);


                    AutoCreateTemplate = CreateTemplate(信息内容);
                    AutoCreateTemplate = AutoCreateTemplate.CopyFrom<TemplateData>(testData);

                    if (SubscribeUsers != null)
                    {
                        OpenID = SubscribeUsers.OpenID;
                        URL = String.Format("{0}?ID={1}&isweixin=1", ConnectionEmail, ID);

                        if (!String.IsNullOrWhiteSpace(OpenID))
                        {
                            result = Senparc.Weixin.MP.AdvancedAPIs.TemplateApi.SendTemplateMessage(accessToken, OpenID, templateId, "#FFFF00", URL, AutoCreateTemplate);

                            if (ReturnCode.请求成功.Equals(result.errcode))
                            {
                                System.Threading.Thread.Sleep(1000);
                            }
                        }

                    }

                    break;

                case Template.流程催办提醒:


                    IBPMProcNotifyRecService _IBPMProcNotifyRecService = new BPMProcNotifyRecService();
                    IQueryable<BPMProcNotifyRec> BPMProcNotifyRecLst = _IBPMProcNotifyRecService.LoadEntities(o => o.STEPID > 0);

                    IBPMProcNotifyRecHisService _IBPMProcNotifyRecHisService = new BPMProcNotifyRecHisService();
                    BPMProcNotifyRecHis NotifyRecHis = new BPMProcNotifyRecHis();



                    var lstNotifyRec = from User in UserLst
                                       join NotifyRec in BPMProcNotifyRecLst
                                       on User.EmpID equals NotifyRec.NOTIFYUSER
                                       select new
                                       {
                                           UserMail = User.Email,
                                           OpenID = User.OpenID,
                                           MsgID = NotifyRec.MSGID,
                                           FLOWNO = NotifyRec.FLOWNO,
                                           PROCNAME = NotifyRec.PROCNAME,
                                           NODENAME = NotifyRec.NODENAME,
                                           OWNERCOUNT = NotifyRec.OWNERCOUNT,
                                           OWNERNAME = NotifyRec.OWNERNAME,
                                           NOTIFYHOUR = NotifyRec.NOTIFYHOUR,
                                           RECEIVEAT = NotifyRec.RECEIVEAT,
                                           NTYPE = NotifyRec.NTYPE,
                                           STEPID = NotifyRec.STEPID,
                                           OPDEPT = NotifyRec.OPDEPT,
                                           NOTIFYUSER = NotifyRec.NOTIFYUSER,
                                           NOTIFYUNAME = NotifyRec.NOTIFYUNAME,
                                           EXPWORKTIME = NotifyRec.EXPWORKTIME,
                                           NOTIFYCOUNT = NotifyRec.NOTIFYCOUNT,
                                           LASTTIME = DateTime.Now,
                                           OPSTATUS = NotifyRec.OPSTATUS,
                                           CREATETIME = NotifyRec.CREATETIME

                                       };


                    var lst = lstNotifyRec.ToList();
                    foreach (var NotifyRec in lst)
                    {
                        //流程催办提醒
                        templateId = "OIGvdsocfU7_1wytXnmso6H0cl8XpiS-4Q7jBS09Pyc";

                        OpenID = NotifyRec.OpenID;

                        if (!String.IsNullOrWhiteSpace(OpenID))
                        {

                            //流程催办提醒
                            templateId = "OIGvdsocfU7_1wytXnmso6H0cl8XpiS-4Q7jBS09Pyc";

                            testData = new TemplateData()
                            {
                                first = new TemplateDataItem("您好，以下流程处理进度需要您关注！", "#FF0000"),

                                keyword1 = new TemplateDataItem(NotifyRec.FLOWNO),
                                keyword2 = new TemplateDataItem(String.Format("{0}/{1}", NotifyRec.PROCNAME, NotifyRec.NODENAME)),
                                keyword3 = new TemplateDataItem(String.Format("{0} ({1})", NotifyRec.OWNERNAME, NotifyRec.OWNERCOUNT)),
                                keyword4 = new TemplateDataItem(Convert.ToString(NotifyRec.RECEIVEAT)),
                                keyword5 = new TemplateDataItem(String.Format("本流程节点已设置标准处理时间{0}WH，现已超时", NotifyRec.NOTIFYHOUR)),
                                remark = new TemplateDataItem("请关注该流程处理进度，并在适当时给予协助!" + DateTime.Now)

                            };

                            URL = String.Format("{0}?Joson={1}&isweixin=1", ConnectioniAnyWhere, NotifyRec.NTYPE == 2 ? NotifyRec.OWNERCOUNT : NotifyRec.NOTIFYUSER);

                            try
                            {
#if DEBUG
                                //throw new Exception("有意出错的");
#endif

                                result = Senparc.Weixin.MP.AdvancedAPIs.TemplateApi.SendTemplateMessage(accessToken, OpenID, templateId, "#FFFF00", URL, testData);

                                if (ReturnCode.请求成功.Equals(result.errcode))
                                {

                                    BPMProcNotifyRec oNotifyRec = _IBPMProcNotifyRecService.LoadEntities(o => o.FLOWNO == NotifyRec.FLOWNO && o.MSGID == NotifyRec.MsgID).SingleOrDefault();

                                    //JosonEntity.JosonReflection JosonEntity = new JosonEntity.JosonReflection();
                                    NotifyRecHis = JosonReflection.CopyFromEntity<BPMProcNotifyRec, BPMProcNotifyRecHis>(oNotifyRec);
                                    NotifyRecHis.LASTTIME = DateTime.Now;

                                    if (_IBPMProcNotifyRecService.DeleteEntity(oNotifyRec))
                                        _IBPMProcNotifyRecHisService.AddEntity(NotifyRecHis);


                                }

                            }
                            catch (Exception ex)
                            {
                                #region 邮件发送

                                String DisplayName = "BPM系统通知邮件" + NotifyRec.PROCNAME;
                                String JosonNotice = @"<BR><BR><div style='border:1px solid #efefef; padding:5px;'><font color='blue'>请关注微信服务号“创维数字移动办公”，将微信号和BPM账号绑定 ，本通知将会通过微信消息推送给您！</font><BR><font color='red'>本邮件为系统自动发送，请勿直接回复 <BR>" + DateTime.Now + "</font></div>";

                                String Receive = NotifyRec.UserMail,
                                    MSubject = NotifyRec.PROCNAME + NotifyRec.FLOWNO,
                                    MailBody = String.Format(@"{0}{1}你好,<BR><BR>由于无法进行微信推送信息服务，我们只能以邮件方式提醒你。<BR>
与你相关的<font color='red'>流程{2}（{3}）现已超时</font>，请你及时处理。<BR>
本流程节点已设置<font color='blue'>标准处理时间{4}WH</font>，现已超时!请关注该流程处理进度，并在适当时给予协助!<BR><BR>
{5} 谢谢 <BR><hr>
说明：重点流程超时提醒为公司要求，如对处理时间设置由异议，请与IT联系，欢迎关注“创维数字移动办公”微信公共号，通过微信方式接受推送消息 。" + JosonNotice
                                    , NotifyRec.OPDEPT
                                    , NotifyRec.OWNERNAME
                                    , NotifyRec.PROCNAME
                                    , NotifyRec.NODENAME
                                    , NotifyRec.NOTIFYHOUR
                                    , NotifyRec.NOTIFYUNAME == NotifyRec.OWNERNAME
                                    ? String.Empty
                                    : String.Format("该邮件将同时抄送{0},敬请恢复绑定微信通知<BR><BR>", NotifyRec.NOTIFYUNAME)

                                    );



                                //SendMail.MailConfig Config = new SendMail.MailConfig
                                //{
                                //    DispalyName = NotifyRec.PROCNAME,
                                //    FromEmail = "bpmserver@skyworth.com",
                                //    FromServer = "mail.skyworth.com",
                                //    PassWord = "Hello@Sky..2014"
                                //};

                                //Boolean isReceived = SendMail.SendEmail(Config, Receive, MSubject, MailBody); 

                                #endregion

#if DEBUG
                                //Receive = "Jiangxiaopeng@Skyworth.com";
#endif

                                Boolean isReceived = SendMail.SendEmail(DisplayName, Receive, MSubject, MailBody, false);

                                System.Threading.Thread.Sleep(1000);

                                if (isReceived)
                                {

                                    BPMProcNotifyRec oNotifyRec = _IBPMProcNotifyRecService.LoadEntities(o => o.FLOWNO == NotifyRec.FLOWNO && o.MSGID == NotifyRec.MsgID).SingleOrDefault();

                                    oNotifyRec.PROCNAME += "(SendEmail)" + Receive;
                                    //JosonEntity.JosonReflection JosonEntity = new JosonEntity.JosonReflection();
                                    NotifyRecHis = JosonReflection.CopyFromEntity<BPMProcNotifyRec, BPMProcNotifyRecHis>(oNotifyRec);
                                    NotifyRecHis.LASTTIME = DateTime.Now;

                                    if (_IBPMProcNotifyRecService.DeleteEntity(oNotifyRec))
                                        _IBPMProcNotifyRecHisService.AddEntity(NotifyRecHis);

                                }


                                ISchedulers.ScheduleJob.JosonException.CreateException(ex, NotifyRec);


                            }



                            System.Threading.Thread.Sleep(1000);

                        }

                    }

                    #region 流程催办提醒

                    //{{first.DATA}}
                    //流程编号：{{keyword1.DATA}}
                    //流程名称：{{keyword2.DATA}}
                    //待办人员：{{keyword3.DATA}}
                    //待办时间：{{keyword4.DATA}}
                    //备注信息：{{keyword5.DATA}}
                    //{{remark.DATA}}

                    //您好，以下流程处理进度需要您关注！
                    //流程编号：YBHT-12345678
                    //流程名称：一般合同审批/法务审核
                    //待办人员：SDT03982（蒋小鹏）
                    //待办时间：2015.10.08  12:52:52
                    //备注信息：本流程节点已设置标准处理时间8WH，现已超时。
                    //请关注该流程处理进度，并在适当时给予协助。

                    ////流程催办提醒
                    //templateId = "OIGvdsocfU7_1wytXnmso6H0cl8XpiS-4Q7jBS09Pyc"; 

                    #endregion

#if DEBUG

                    foreach (BPMProcNotifyRec NotifyRec in BPMProcNotifyRecLst.ToList())
                    {
                        OpenID = UserLst.Where(u => u.EmpID == NotifyRec.NOTIFYUSER).SingleOrDefault() == null
                                ? new AppConfigs().GetTestOpenID(false) : UserLst.Where(u => u.EmpID == NotifyRec.NOTIFYUSER).SingleOrDefault().OpenID;

                        //  OpenID = SendOpenID(NotifyRec.PROCNAME);
                        //  OpenID = GetOpenIDByEmpID(NotifyRec.OWNERCOUNT);

                        if (!String.IsNullOrWhiteSpace(OpenID))
                        {

                            //流程催办提醒
                            templateId = "OIGvdsocfU7_1wytXnmso6H0cl8XpiS-4Q7jBS09Pyc";

                            testData = new TemplateData()
                            {
                                first = new TemplateDataItem("您好，以下流程处理进度需要您关注！", "#FF0000"),

                                keyword1 = new TemplateDataItem(NotifyRec.FLOWNO),
                                keyword2 = new TemplateDataItem(String.Format("{0}/{1}", NotifyRec.PROCNAME, NotifyRec.NODENAME)),
                                keyword3 = new TemplateDataItem(String.Format("{0} ({1})", NotifyRec.OWNERNAME, NotifyRec.OWNERCOUNT)),
                                keyword4 = new TemplateDataItem(Convert.ToString(NotifyRec.RECEIVEAT)),
                                keyword5 = new TemplateDataItem(String.Format("本流程节点已设置标准处理时间{0}WH，现已超时", NotifyRec.NOTIFYHOUR)),
                                remark = new TemplateDataItem("请关注该流程处理进度，并在适当时给予协助!")

                            };

                            URL = String.Format("{0}?Joson={1}&isweixin=1", ConnectioniAnyWhere, NotifyRec.NTYPE == 2 ? NotifyRec.OWNERCOUNT : NotifyRec.NOTIFYUSER);
                            result = Senparc.Weixin.MP.AdvancedAPIs.TemplateApi.SendTemplateMessage(accessToken, OpenID, templateId, "#FFFF00", URL, testData);

                            if (ReturnCode.请求成功.Equals(result.errcode))
                            {
                                #region 已封装

                                // 已封装
                                //result = new SendTemplateMessageResult();
                                //JosonEntity.Entity EntityJoson = new JosonEntity.Entity();
                                //NotifyRecHis = EntityJoson.CopyFromEntity<BPMProcNotifyRec, BPMProcNotifyRecHis>(NotifyRec);

                                //同源拷贝
                                // NotifyRec.CopyEntity<BPMProcNotifyRec>(NotifyRec); 

                                #endregion

                                // JosonEntity.JosonReflection JosonEntity = new JosonEntity.JosonReflection();
                                NotifyRecHis = JosonReflection.CopyFromEntity<BPMProcNotifyRec, BPMProcNotifyRecHis>(NotifyRec);
                                NotifyRecHis.LASTTIME = DateTime.Now;

                                if (_IBPMProcNotifyRecService.DeleteEntity(NotifyRec))
                                    _IBPMProcNotifyRecHisService.AddEntity(NotifyRecHis);

                            }
                            else
                            {


                                LogHelper log = new LogHelper();
                                log.Info(result.errcode);
                                log.Warning(result.errcode);
                                log.Error(result.errcode);


                                //LogHelpers.Warning(result.errcode);
                                //LogHelpers.Info(result.errcode);
                                //LogHelpers.Error(result.errcode);
                                //LogHelpers.Debug(result.errcode);

                            }

                            System.Threading.Thread.Sleep(1000);

                        }
                    }
#endif

                    break;

                case Template.微信解绑通知:

                    //微信解绑通知
                    templateId = "V49ETtdeEd_4zGvE0EOj7giugOo-DBpiLEg2dR5ppPE";
                    testData = new TemplateData()
                    {
                        first = new TemplateDataItem("【消息提醒】您好，你的账号已解绑", "#FF0000"),

                        keyword1 = new TemplateDataItem("SDT03982"),
                        keyword2 = new TemplateDataItem("该微信已不能用作 iAnyWhere 登录"),
                        remark = new TemplateDataItem("你的账号已经被解绑，不能继续使用相关功能！哈哈，")
                    };

                    break;

                case Template.用户登录提醒:

                    //用户登录提醒
                    templateId = "GMEPM-Wg2Bsb3FhpZoBwOCp5WMAR4rEOW4gx58Y5JHk";

                    testData = new TemplateData()
                    {
                        first = new TemplateDataItem("【温馨提示】您好，你的账号SDTXXXX移动客户端在别处已登录！", "#FF0000"),
                        time = new TemplateDataItem(DateTime.Now.ToString(), "#FF3300"),
                        ip = new TemplateDataItem("202.1.5.8"),
                        reason = new TemplateDataItem("登录可能存在异常,如果本次登录不是您本人所为，说明您的帐号已经被盗！请点击本条消息，立即锁定帐号。"),
                        remark = new TemplateDataItem("测试信息，不要当真")
                    };

                    break;

            }


            return ReturnCode.请求成功.Equals(result == null ? ReturnCode.POST的数据包为空 : result.errcode);

            //Assert.AreEqual(ReturnCode.请求成功, result.errcode);

        }
    }
}
