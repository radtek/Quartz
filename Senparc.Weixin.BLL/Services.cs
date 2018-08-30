   


 

using System.Text;
using System.Threading.Tasks;
 
namespace Senparc.Weixin.BLL
{
	using System.Collections.Generic;
	using System.Linq;
	using Senparc.Weixin.DAL;
	using Senparc.Weixin.IBLL;
	using Senparc.Weixin.IDAL;
	using Quartz.Entity;


	/// <summary>
    /// BPMProcNotifyRecService
    /// </summary>
	public partial class BPMProcNotifyRecService : BaseService<BPMProcNotifyRec>, IBPMProcNotifyRecService
    {
		//只要想操作数据库，我们只要拿到DBSession就行
        public override void SetCurrentRepository()
        {
            CurrentRepository = _DbSession.BPMProcNotifyRecRepository;
        }

        /// <summary>
        /// 执行Sql语句的方法  EF5.0的写法
        /// EF4.0的写法   int ExcuteSql(string strSql, ObjectParameter[] parameters);
        /// </summary>
        /// <param name="strSql">执行的SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>返回int类型</returns>
		public  int ExcuteSql(string strSql, System.Data.Common.DbParameter[] parameters)
        {

            return _DbSession.ExcuteSql(strSql, parameters);

        }


        /// <summary>
        /// 执行SQL语句或存储过程 返回IEnumerable<BPMProcNotifyRec>
        /// </summary>
        /// <typeparam name="BPMProcNotifyRec"></typeparam>
        /// <param name="strSql"></param>
        /// <param name="parameters"></param>
        /// <returns>IEnumerable<BPMProcNotifyRec></returns>
		public  IEnumerable<BPMProcNotifyRec> ExcuteSqlGetIEnumerable<BPMProcNotifyRec>(string strSql, System.Data.Common.DbParameter[] parameters)
        {

            return _DbSession.ExcuteSqlGetIEnumerable<BPMProcNotifyRec>(strSql, parameters);
        }


        /// <summary>
        /// 执行SQL语句或存储过程返回 IQueryable<BPMProcNotifyRec>
        /// </summary>
        /// <typeparam name="BPMProcNotifyRec"></typeparam>
        /// <param name="strSql"></param>
        /// <param name="parameters"></param>
        /// <returns>IQueryable<BPMProcNotifyRec></returns>
        public IQueryable<BPMProcNotifyRec> ExcuteSqlGetIQueryable<BPMProcNotifyRec>(string strSql, System.Data.Common.DbParameter[] parameters)
        {
            return _DbSession.ExcuteSqlGetIQueryable<BPMProcNotifyRec>(strSql, parameters);
        }



	}



	/// <summary>
    /// BPMProcNotifyRecHisService
    /// </summary>
	public partial class BPMProcNotifyRecHisService : BaseService<BPMProcNotifyRecHis>, IBPMProcNotifyRecHisService
    {
		//只要想操作数据库，我们只要拿到DBSession就行
        public override void SetCurrentRepository()
        {
            CurrentRepository = _DbSession.BPMProcNotifyRecHisRepository;
        }

        /// <summary>
        /// 执行Sql语句的方法  EF5.0的写法
        /// EF4.0的写法   int ExcuteSql(string strSql, ObjectParameter[] parameters);
        /// </summary>
        /// <param name="strSql">执行的SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>返回int类型</returns>
		public  int ExcuteSql(string strSql, System.Data.Common.DbParameter[] parameters)
        {

            return _DbSession.ExcuteSql(strSql, parameters);

        }


        /// <summary>
        /// 执行SQL语句或存储过程 返回IEnumerable<BPMProcNotifyRecHis>
        /// </summary>
        /// <typeparam name="BPMProcNotifyRecHis"></typeparam>
        /// <param name="strSql"></param>
        /// <param name="parameters"></param>
        /// <returns>IEnumerable<BPMProcNotifyRecHis></returns>
		public  IEnumerable<BPMProcNotifyRecHis> ExcuteSqlGetIEnumerable<BPMProcNotifyRecHis>(string strSql, System.Data.Common.DbParameter[] parameters)
        {

            return _DbSession.ExcuteSqlGetIEnumerable<BPMProcNotifyRecHis>(strSql, parameters);
        }


        /// <summary>
        /// 执行SQL语句或存储过程返回 IQueryable<BPMProcNotifyRecHis>
        /// </summary>
        /// <typeparam name="BPMProcNotifyRecHis"></typeparam>
        /// <param name="strSql"></param>
        /// <param name="parameters"></param>
        /// <returns>IQueryable<BPMProcNotifyRecHis></returns>
        public IQueryable<BPMProcNotifyRecHis> ExcuteSqlGetIQueryable<BPMProcNotifyRecHis>(string strSql, System.Data.Common.DbParameter[] parameters)
        {
            return _DbSession.ExcuteSqlGetIQueryable<BPMProcNotifyRecHis>(strSql, parameters);
        }



	}



	/// <summary>
    /// BPMSysMessagesFailedService
    /// </summary>
	public partial class BPMSysMessagesFailedService : BaseService<BPMSysMessagesFailed>, IBPMSysMessagesFailedService
    {
		//只要想操作数据库，我们只要拿到DBSession就行
        public override void SetCurrentRepository()
        {
            CurrentRepository = _DbSession.BPMSysMessagesFailedRepository;
        }

        /// <summary>
        /// 执行Sql语句的方法  EF5.0的写法
        /// EF4.0的写法   int ExcuteSql(string strSql, ObjectParameter[] parameters);
        /// </summary>
        /// <param name="strSql">执行的SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>返回int类型</returns>
		public  int ExcuteSql(string strSql, System.Data.Common.DbParameter[] parameters)
        {

            return _DbSession.ExcuteSql(strSql, parameters);

        }


        /// <summary>
        /// 执行SQL语句或存储过程 返回IEnumerable<BPMSysMessagesFailed>
        /// </summary>
        /// <typeparam name="BPMSysMessagesFailed"></typeparam>
        /// <param name="strSql"></param>
        /// <param name="parameters"></param>
        /// <returns>IEnumerable<BPMSysMessagesFailed></returns>
		public  IEnumerable<BPMSysMessagesFailed> ExcuteSqlGetIEnumerable<BPMSysMessagesFailed>(string strSql, System.Data.Common.DbParameter[] parameters)
        {

            return _DbSession.ExcuteSqlGetIEnumerable<BPMSysMessagesFailed>(strSql, parameters);
        }


        /// <summary>
        /// 执行SQL语句或存储过程返回 IQueryable<BPMSysMessagesFailed>
        /// </summary>
        /// <typeparam name="BPMSysMessagesFailed"></typeparam>
        /// <param name="strSql"></param>
        /// <param name="parameters"></param>
        /// <returns>IQueryable<BPMSysMessagesFailed></returns>
        public IQueryable<BPMSysMessagesFailed> ExcuteSqlGetIQueryable<BPMSysMessagesFailed>(string strSql, System.Data.Common.DbParameter[] parameters)
        {
            return _DbSession.ExcuteSqlGetIQueryable<BPMSysMessagesFailed>(strSql, parameters);
        }



	}



	/// <summary>
    /// BPMSysMessagesQueueService
    /// </summary>
	public partial class BPMSysMessagesQueueService : BaseService<BPMSysMessagesQueue>, IBPMSysMessagesQueueService
    {
		//只要想操作数据库，我们只要拿到DBSession就行
        public override void SetCurrentRepository()
        {
            CurrentRepository = _DbSession.BPMSysMessagesQueueRepository;
        }

        /// <summary>
        /// 执行Sql语句的方法  EF5.0的写法
        /// EF4.0的写法   int ExcuteSql(string strSql, ObjectParameter[] parameters);
        /// </summary>
        /// <param name="strSql">执行的SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>返回int类型</returns>
		public  int ExcuteSql(string strSql, System.Data.Common.DbParameter[] parameters)
        {

            return _DbSession.ExcuteSql(strSql, parameters);

        }


        /// <summary>
        /// 执行SQL语句或存储过程 返回IEnumerable<BPMSysMessagesQueue>
        /// </summary>
        /// <typeparam name="BPMSysMessagesQueue"></typeparam>
        /// <param name="strSql"></param>
        /// <param name="parameters"></param>
        /// <returns>IEnumerable<BPMSysMessagesQueue></returns>
		public  IEnumerable<BPMSysMessagesQueue> ExcuteSqlGetIEnumerable<BPMSysMessagesQueue>(string strSql, System.Data.Common.DbParameter[] parameters)
        {

            return _DbSession.ExcuteSqlGetIEnumerable<BPMSysMessagesQueue>(strSql, parameters);
        }


        /// <summary>
        /// 执行SQL语句或存储过程返回 IQueryable<BPMSysMessagesQueue>
        /// </summary>
        /// <typeparam name="BPMSysMessagesQueue"></typeparam>
        /// <param name="strSql"></param>
        /// <param name="parameters"></param>
        /// <returns>IQueryable<BPMSysMessagesQueue></returns>
        public IQueryable<BPMSysMessagesQueue> ExcuteSqlGetIQueryable<BPMSysMessagesQueue>(string strSql, System.Data.Common.DbParameter[] parameters)
        {
            return _DbSession.ExcuteSqlGetIQueryable<BPMSysMessagesQueue>(strSql, parameters);
        }



	}



	/// <summary>
    /// BPMSysMessagesSucceedService
    /// </summary>
	public partial class BPMSysMessagesSucceedService : BaseService<BPMSysMessagesSucceed>, IBPMSysMessagesSucceedService
    {
		//只要想操作数据库，我们只要拿到DBSession就行
        public override void SetCurrentRepository()
        {
            CurrentRepository = _DbSession.BPMSysMessagesSucceedRepository;
        }

        /// <summary>
        /// 执行Sql语句的方法  EF5.0的写法
        /// EF4.0的写法   int ExcuteSql(string strSql, ObjectParameter[] parameters);
        /// </summary>
        /// <param name="strSql">执行的SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>返回int类型</returns>
		public  int ExcuteSql(string strSql, System.Data.Common.DbParameter[] parameters)
        {

            return _DbSession.ExcuteSql(strSql, parameters);

        }


        /// <summary>
        /// 执行SQL语句或存储过程 返回IEnumerable<BPMSysMessagesSucceed>
        /// </summary>
        /// <typeparam name="BPMSysMessagesSucceed"></typeparam>
        /// <param name="strSql"></param>
        /// <param name="parameters"></param>
        /// <returns>IEnumerable<BPMSysMessagesSucceed></returns>
		public  IEnumerable<BPMSysMessagesSucceed> ExcuteSqlGetIEnumerable<BPMSysMessagesSucceed>(string strSql, System.Data.Common.DbParameter[] parameters)
        {

            return _DbSession.ExcuteSqlGetIEnumerable<BPMSysMessagesSucceed>(strSql, parameters);
        }


        /// <summary>
        /// 执行SQL语句或存储过程返回 IQueryable<BPMSysMessagesSucceed>
        /// </summary>
        /// <typeparam name="BPMSysMessagesSucceed"></typeparam>
        /// <param name="strSql"></param>
        /// <param name="parameters"></param>
        /// <returns>IQueryable<BPMSysMessagesSucceed></returns>
        public IQueryable<BPMSysMessagesSucceed> ExcuteSqlGetIQueryable<BPMSysMessagesSucceed>(string strSql, System.Data.Common.DbParameter[] parameters)
        {
            return _DbSession.ExcuteSqlGetIQueryable<BPMSysMessagesSucceed>(strSql, parameters);
        }



	}



	/// <summary>
    /// BPMSysSettingsService
    /// </summary>
	public partial class BPMSysSettingsService : BaseService<BPMSysSettings>, IBPMSysSettingsService
    {
		//只要想操作数据库，我们只要拿到DBSession就行
        public override void SetCurrentRepository()
        {
            CurrentRepository = _DbSession.BPMSysSettingsRepository;
        }

        /// <summary>
        /// 执行Sql语句的方法  EF5.0的写法
        /// EF4.0的写法   int ExcuteSql(string strSql, ObjectParameter[] parameters);
        /// </summary>
        /// <param name="strSql">执行的SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>返回int类型</returns>
		public  int ExcuteSql(string strSql, System.Data.Common.DbParameter[] parameters)
        {

            return _DbSession.ExcuteSql(strSql, parameters);

        }


        /// <summary>
        /// 执行SQL语句或存储过程 返回IEnumerable<BPMSysSettings>
        /// </summary>
        /// <typeparam name="BPMSysSettings"></typeparam>
        /// <param name="strSql"></param>
        /// <param name="parameters"></param>
        /// <returns>IEnumerable<BPMSysSettings></returns>
		public  IEnumerable<BPMSysSettings> ExcuteSqlGetIEnumerable<BPMSysSettings>(string strSql, System.Data.Common.DbParameter[] parameters)
        {

            return _DbSession.ExcuteSqlGetIEnumerable<BPMSysSettings>(strSql, parameters);
        }


        /// <summary>
        /// 执行SQL语句或存储过程返回 IQueryable<BPMSysSettings>
        /// </summary>
        /// <typeparam name="BPMSysSettings"></typeparam>
        /// <param name="strSql"></param>
        /// <param name="parameters"></param>
        /// <returns>IQueryable<BPMSysSettings></returns>
        public IQueryable<BPMSysSettings> ExcuteSqlGetIQueryable<BPMSysSettings>(string strSql, System.Data.Common.DbParameter[] parameters)
        {
            return _DbSession.ExcuteSqlGetIQueryable<BPMSysSettings>(strSql, parameters);
        }



	}



	/// <summary>
    /// LocationUserService
    /// </summary>
	public partial class LocationUserService : BaseService<LocationUser>, ILocationUserService
    {
		//只要想操作数据库，我们只要拿到DBSession就行
        public override void SetCurrentRepository()
        {
            CurrentRepository = _DbSession.LocationUserRepository;
        }

        /// <summary>
        /// 执行Sql语句的方法  EF5.0的写法
        /// EF4.0的写法   int ExcuteSql(string strSql, ObjectParameter[] parameters);
        /// </summary>
        /// <param name="strSql">执行的SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>返回int类型</returns>
		public  int ExcuteSql(string strSql, System.Data.Common.DbParameter[] parameters)
        {

            return _DbSession.ExcuteSql(strSql, parameters);

        }


        /// <summary>
        /// 执行SQL语句或存储过程 返回IEnumerable<LocationUser>
        /// </summary>
        /// <typeparam name="LocationUser"></typeparam>
        /// <param name="strSql"></param>
        /// <param name="parameters"></param>
        /// <returns>IEnumerable<LocationUser></returns>
		public  IEnumerable<LocationUser> ExcuteSqlGetIEnumerable<LocationUser>(string strSql, System.Data.Common.DbParameter[] parameters)
        {

            return _DbSession.ExcuteSqlGetIEnumerable<LocationUser>(strSql, parameters);
        }


        /// <summary>
        /// 执行SQL语句或存储过程返回 IQueryable<LocationUser>
        /// </summary>
        /// <typeparam name="LocationUser"></typeparam>
        /// <param name="strSql"></param>
        /// <param name="parameters"></param>
        /// <returns>IQueryable<LocationUser></returns>
        public IQueryable<LocationUser> ExcuteSqlGetIQueryable<LocationUser>(string strSql, System.Data.Common.DbParameter[] parameters)
        {
            return _DbSession.ExcuteSqlGetIQueryable<LocationUser>(strSql, parameters);
        }



	}



	/// <summary>
    /// SubscribeUsersService
    /// </summary>
	public partial class SubscribeUsersService : BaseService<SubscribeUsers>, ISubscribeUsersService
    {
		//只要想操作数据库，我们只要拿到DBSession就行
        public override void SetCurrentRepository()
        {
            CurrentRepository = _DbSession.SubscribeUsersRepository;
        }

        /// <summary>
        /// 执行Sql语句的方法  EF5.0的写法
        /// EF4.0的写法   int ExcuteSql(string strSql, ObjectParameter[] parameters);
        /// </summary>
        /// <param name="strSql">执行的SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>返回int类型</returns>
		public  int ExcuteSql(string strSql, System.Data.Common.DbParameter[] parameters)
        {

            return _DbSession.ExcuteSql(strSql, parameters);

        }


        /// <summary>
        /// 执行SQL语句或存储过程 返回IEnumerable<SubscribeUsers>
        /// </summary>
        /// <typeparam name="SubscribeUsers"></typeparam>
        /// <param name="strSql"></param>
        /// <param name="parameters"></param>
        /// <returns>IEnumerable<SubscribeUsers></returns>
		public  IEnumerable<SubscribeUsers> ExcuteSqlGetIEnumerable<SubscribeUsers>(string strSql, System.Data.Common.DbParameter[] parameters)
        {

            return _DbSession.ExcuteSqlGetIEnumerable<SubscribeUsers>(strSql, parameters);
        }


        /// <summary>
        /// 执行SQL语句或存储过程返回 IQueryable<SubscribeUsers>
        /// </summary>
        /// <typeparam name="SubscribeUsers"></typeparam>
        /// <param name="strSql"></param>
        /// <param name="parameters"></param>
        /// <returns>IQueryable<SubscribeUsers></returns>
        public IQueryable<SubscribeUsers> ExcuteSqlGetIQueryable<SubscribeUsers>(string strSql, System.Data.Common.DbParameter[] parameters)
        {
            return _DbSession.ExcuteSqlGetIQueryable<SubscribeUsers>(strSql, parameters);
        }



	}



}