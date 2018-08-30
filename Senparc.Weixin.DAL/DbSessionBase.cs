using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Senparc.Weixin.IDAL;
using System.Data.Entity.Validation;

namespace Senparc.Weixin.DAL
{
    //一次跟数据库交互的会话
    public partial class DbSession : IDbSession //代表应用程序跟数据库之间的一次会话，也是数据库访问层的统一入口
    {
        #region ----有模版自动生成----
        //public IDAL.IRoleRepository RoleRepository
        //{
        //    get { return new RoleRepository(); }
        //}

        //public IDAL.IUserInfoRepository UserInfoRepository
        //{
        //    get { return new UserInfoRepository(); }
        //} 
        #endregion

        /// <summary>
        /// 代表：当前应用程序跟数据库的会话内所有的实体的变化，更新会数据库
        /// </summary>
        /// <returns>返回受影响的行数</returns>
        public int SaveChanges()
        {
            int Result = 0;
            try
            {
                //写数据库调 用EF上下文的S
                Result = DAL.EFContextFactory.GetCurrentDbContext().SaveChanges();

            }
            catch (DbEntityValidationException dbEx)
            {
                //这样子我们就能看到 EntityValidationErrors 所有的
                //throw new Exception(dbEx.EntityValidationErrors.SingleOrDefault().ValidationErrors.ToList()[0].ErrorMessage);
            }

            return Result;
        }

        /// <summary>
        /// 执行Sql脚本的方法
        /// </summary>
        /// <param name="strSql">执行的SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>返回受影响的行数</returns>
        public int ExcuteSql(string strSql, System.Data.Common.DbParameter[] parameters)
        {

            int ReturnVal = 0;
            try
            {
                //Ef4.0的执行方法 ObjectContext
                //封装一个执行SQl脚本的代码
                //return DAL.EFContextFactory.GetCurrentDbContext().ExecuteFunction(strSql, parameters);

                //封装一个执行SQl脚本的代码

                //if (DAL.EFContextFactory.GetCurrentDbContext().Database.Connection.State != System.Data.ConnectionState.Open)
                //    DAL.EFContextFactory.GetCurrentDbContext().Database.Connection.Open();

                ReturnVal = DAL.EFContextFactory.GetCurrentDbContext().Database.ExecuteSqlCommand(strSql, parameters);
            }
            catch (Exception ex)
            {

                throw new NotImplementedException(ex.Message);
            }

            return ReturnVal;
        }


        /// <summary>
        /// 执行SQL语句或存储过程返回 IEnumerable<T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strSql"></param>
        /// <param name="parameters"></param>
        /// <returns>IEnumerable<T></returns>
        public IEnumerable<T> ExcuteSqlGetIEnumerable<T>(string strSql, System.Data.Common.DbParameter[] parameters)
        {
            //封装一个执行SQl脚本的代码

            return DAL.EFContextFactory.GetCurrentDbContext().Database.SqlQuery<T>(strSql, parameters);
        }

        /// <summary>
        /// 执行SQL语句或存储过程返回 IQueryable<T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strSql"></param>
        /// <param name="parameters"></param>
        /// <returns>IQueryable<T></returns>
        public IQueryable<T> ExcuteSqlGetIQueryable<T>(string strSql, System.Data.Common.DbParameter[] parameters)
        {
            //封装一个执行SQl脚本的代码
            return ExcuteSqlGetIEnumerable<T>(strSql, parameters).AsQueryable<T>();
            // return DAL.EFContextFactory.GetCurrentDbContext().Database.SqlQuery<T>(strSql, parameters).AsQueryable();
        }

    }
}
