using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Senparc.Weixin.IDAL
{
    public partial interface IDbSession
    {
        #region ---有模版自动生成---
        ////每个表对应的实体仓储对象
        //IDAL.IRoleRepository RoleRepository { get; }
        //IDAL.IUserInfoRepository UserInfoRepository { get; } 
        #endregion

        /// <summary>
        /// 将当前应用程序跟数据库的会话内所有实体的变化更新会数据库
        /// </summary>
        /// <returns>返回一个int类型的执行成功与否</returns>
        int SaveChanges();

        /// <summary>
        /// 执行Sql语句的方法  EF5.0的写法
        /// EF4.0的写法   int ExcuteSql(string strSql, ObjectParameter[] parameters);
        /// </summary>
        /// <param name="strSql">执行的SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>返回int类型</returns>
        int ExcuteSql(string strSql, DbParameter[] parameters);


        /// <summary>
        /// 执行SQL语句或存储过程 返回IEnumerable<T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strSql"></param>
        /// <param name="parameters"></param>
        /// <returns>IEnumerable<T></returns>
        IEnumerable<T> ExcuteSqlGetIEnumerable<T>(string strSql, System.Data.Common.DbParameter[] parameters);


        /// <summary>
        /// 执行SQL语句或存储过程返回 IQueryable<T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strSql"></param>
        /// <param name="parameters"></param>
        /// <returns>IQueryable<T></returns>
        IQueryable<T> ExcuteSqlGetIQueryable<T>(string strSql, System.Data.Common.DbParameter[] parameters);

    }
}
