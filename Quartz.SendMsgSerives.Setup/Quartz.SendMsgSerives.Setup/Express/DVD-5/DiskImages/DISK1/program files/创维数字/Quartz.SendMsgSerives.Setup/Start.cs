
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;


namespace Quartz.SendMsgSerives
{
    using Net.Common;
    using Net.Common.WCFService;
    using System.Diagnostics;


    class InvokeFactory
    {
        /// <summary>
        /// 要调用的服务契约
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public static T Invoke<T>(Func<IJosonService, T> func)
        {
            IServiceInvoker serviceInvoker = new WCFServiceInvoker();
            return serviceInvoker.InvokeService(func);
        }


        public static T Invokes<T>(Func<IJosonServices, T> func)
        {
            IServiceInvoker serviceInvoker = new WCFServiceInvoker();
            return serviceInvoker.InvokeService(func);
        }


        // 调用方法 (Add 为接口方法)
        // bool result1  =  InvokeFactory.Invokes(S => S.SendMsg(EmpID, strMsg));

    }



    public class Start
    {

        static readonly string OAuthWebService = System.Configuration.ConfigurationSettings.AppSettings["OAuthWebService"];
        static readonly string OAuthWebServices = System.Configuration.ConfigurationSettings.AppSettings["OAuthWebServices"];

        public static bool Run(String 接受者 = "SDT12872", String 微信内容 = "微信内容", String 信息内容 = "信息内容", int ID = 0, Boolean isDebug = true)
        {
            String EmpID = 接受者;
            String strMsg = 微信内容;
            String strInfo = 信息内容;
            String strRuslt = String.Empty;

            Boolean isSucessFul = false;


#if debug
            

            strRuslt = InvokeFactory.Invoke(S => S.DoWork("InvokeFactory.Invoke"));

            IJosonService IJosonService = InvokeContext.CreateService<IJosonService>(OAuthWebService);
            strRuslt = IJosonService.DoWork("InvokeContext.CreateService<IJosonService>");

            IJosonService WCFserver = InvokeContext.CreateServiceByURL<IJosonService>(OAuthWebService);
            strRuslt = WCFserver.DoWork("InvokeContext.CreateServiceByURL<IJosonService>");

            //IJosonService WCFservers = InvokeContext.CreateWCFServiceByURL<IJosonService>(OAuthWebService);
            //strRuslt = WCFservers.DoWork("InvokeContext.CreateWCFServiceByURL<IJosonService>");

#else
            try
            {

                isSucessFul = InvokeFactory.Invokes(S => S.SendMsg(EmpID, strMsg, strInfo, ID, isDebug));

                if (!isSucessFul)
                {

                    IJosonServices JosonService = InvokeContext.CreateService<IJosonServices>(OAuthWebServices);
                    isSucessFul = JosonService.SendMsg(EmpID, strMsg, strInfo, ID, isDebug);
                }

                if (!isSucessFul)
                {
                    IJosonServices JWCFserver = InvokeContext.CreateServiceByURL<IJosonServices>(OAuthWebServices);
                    isSucessFul = JWCFserver.SendMsg(EmpID, strMsg, strInfo, ID, isDebug);
                }

                //IJosonServices JWCFservers = InvokeContext.CreateWCFServiceByURL<IJosonServices>(OAuthWebService);
                //JWCFservers.SendMsg(EmpID, strMsg);
            }
            catch (Exception ex) {


                String Message = "发生错误:{0}{1}";
                Message = String.Format(Message, ex.GetBaseException().Message + Environment.NewLine + ex.StackTrace, Environment.NewLine + ex.Message);


                //寫入事件撿視器,方法一  
                if (!EventLog.SourceExists("服务接口"))
                    EventLog.CreateEventSource("服务接口", "微信发送服务接口");
                System.Diagnostics.EventLog.WriteEntry("服务接口", Message, System.Diagnostics.EventLogEntryType.Error);
            
            }

            return isSucessFul;


#endif

        }



    }

}
