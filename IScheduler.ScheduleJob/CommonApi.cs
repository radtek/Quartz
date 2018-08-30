
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Senparc.Weixin.MP.ISchedulers.ScheduleJob
{
    using Senparc.Weixin.MP.AdvancedAPIs;
    using Senparc.Weixin.MP.CommonAPIs;

    public partial class AppConfigs
    {
        private dynamic _appConfig;

        public dynamic AppConfig
        {
            get
            {
                if (_appConfig == null)
                {
                    String Joson = Directory.GetCurrentDirectory() + "\\Config\\Joson.config";
                    //Joson = AppDomain.CurrentDomain.BaseDirectory;
                    //Joson = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

                    if (File.Exists(Joson))
                    {
                        var doc = XDocument.Load(Joson);
                        _appConfig = new
                        {
                            AppId = doc.Root.Element("AppId").Value,
                            Secret = doc.Root.Element("Secret").Value
                        };
                    }
                    else
                    {
                        _appConfig = new
                        {
                            AppId = "wxb33aa63ebdfa4622", //AppId
                            Secret = "9f86b945d7b30e15ac611cffe07d6990"//Secret
                        };
                    }
                }
                return _appConfig;
            }
        }

        internal string AppID
        {
            get { return AppConfig.AppId; }
        }

        protected string AppSecret
        {
            get { return AppConfig.Secret; }
        }

 

        private string _access_token = null;
        protected string _testOpenId = "oTkndw_4rN9NMJpaPzceyAQWmFp0";//"oTkndw9-3WNKWSn8lSgTHiSQi_VA";//换成实际关注者的OpenId

        /// <summary>
        /// 自动获取Openid
        /// </summary>
        /// <param name="getNew">是否从服务器上强制获取一个</param>
        /// <returns></returns>
        public string GetTestOpenID(bool getNew = true)
        {
            if (getNew || string.IsNullOrEmpty(_testOpenId))
            {
                var accessToken = AccessTokenContainer.GetAccessToken(AppID);
                var openIdResult = UserApi.Get(accessToken, null);
                _testOpenId = openIdResult.data.openid.First();
            }
            return _testOpenId;
        }


        public AppConfigs()
        {

            //全局只需注册一次
            AccessTokenContainer.Register(AppID, AppSecret);

            //v13.3.0之后，JsApiTicketContainer已经合并入AccessTokenContainer，已经不需要单独注册
            ////全局只需注册一次
            //JsApiTicketContainer.Register(_appId, _appSecret);
        }
    }
}
