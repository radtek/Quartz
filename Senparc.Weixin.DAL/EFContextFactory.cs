using System;
using System.Collections.Generic;
using System.Data.Entity;
//using System.Data.Objects;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;


namespace Senparc.Weixin.DAL
{
    using Quartz.Entity;

    public class EFContextFactory
    {
        /// <summary>
        /// 帮我们返回当前线程内的数据库上下文，如果当前线程内没有上下文，那么创建一个上下文，并保证
        /// 上线问实例在线程内部是唯一的
        /// </summary>
        /// <returns></returns>
        public static DbContext GetCurrentDbContext()
        {

            var dbContext = CallContext.GetData("DbContext") as DbContext;
            if (dbContext == null)                              //线程在数据槽里面没有此上下文
            {
                dbContext = new JosonSenparcEntities();                     //创建一个EF上下文
                CallContext.SetData("DbContext", dbContext);
            }

           
            return dbContext;


            // OAuthContainer 继承  Entities

            //var Entities = CallContext.GetData("OAuthEntities") as DbContext;
            //if (Entities == null)                                       //线程在数据槽里面没有此上下文
            //{
            //    Entities = new OAuthContainer();                        //创建一个EF上下文
            //    CallContext.SetData("OAuthEntities", Entities);

            //}

            //return Entities;



            //var OAuthContainer = CallContext.GetData("OAuthContainer") as DbContext;
            //if (OAuthContainer == null)     //线程在数据槽里面没有此上下文                          
            //{
              
            //    //创建一个EF上下文
            //    OAuthContainer = new OAuthContainer("OAuthEntities");
            //    CallContext.SetData("OAuthContainer", OAuthContainer);

            //}

            //return OAuthContainer;

        }


        public static DbContext  GetCurrentDbContext<T>(T TEntities)
        {
            //TEntities =default(T);

            var dbContext = CallContext.GetData("DbContext") as DbContext;
            if (dbContext == null)                              //线程在数据槽里面没有此上下文
            {
                dbContext = TEntities as DbContext;             //创建一个EF上下文
                CallContext.SetData("DbContext", dbContext);
            }

            return dbContext;

        }
 
    }
}
