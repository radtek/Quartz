   


 

namespace Senparc.Weixin.IDAL
{	
	using Quartz.Entity;

	    	    
		/// <summary>
	    /// BPMProcNotifyRec的数据仓储接口
	    /// </summary>
		public partial interface IBPMProcNotifyRecRepository : IBaseRepository<BPMProcNotifyRec>
		{
		}

	    	    
		/// <summary>
	    /// BPMProcNotifyRecHis的数据仓储接口
	    /// </summary>
		public partial interface IBPMProcNotifyRecHisRepository : IBaseRepository<BPMProcNotifyRecHis>
		{
		}

	    	    
		/// <summary>
	    /// BPMSysMessagesFailed的数据仓储接口
	    /// </summary>
		public partial interface IBPMSysMessagesFailedRepository : IBaseRepository<BPMSysMessagesFailed>
		{
		}

	    	    
		/// <summary>
	    /// BPMSysMessagesQueue的数据仓储接口
	    /// </summary>
		public partial interface IBPMSysMessagesQueueRepository : IBaseRepository<BPMSysMessagesQueue>
		{
		}

	    	    
		/// <summary>
	    /// BPMSysMessagesSucceed的数据仓储接口
	    /// </summary>
		public partial interface IBPMSysMessagesSucceedRepository : IBaseRepository<BPMSysMessagesSucceed>
		{
		}

	    	    
		/// <summary>
	    /// BPMSysSettings的数据仓储接口
	    /// </summary>
		public partial interface IBPMSysSettingsRepository : IBaseRepository<BPMSysSettings>
		{
		}

	    	    
		/// <summary>
	    /// LocationUser的数据仓储接口
	    /// </summary>
		public partial interface ILocationUserRepository : IBaseRepository<LocationUser>
		{
		}

	    	    
		/// <summary>
	    /// SubscribeUsers的数据仓储接口
	    /// </summary>
		public partial interface ISubscribeUsersRepository : IBaseRepository<SubscribeUsers>
		{
		}

}