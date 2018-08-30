    


 
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
		   
	    /// <summary>
        /// BPMProcNotifyRec表对应的实体仓储对象
        /// </summary>
		IBPMProcNotifyRecRepository BPMProcNotifyRecRepository { get; }

	     
	    /// <summary>
        /// BPMProcNotifyRecHis表对应的实体仓储对象
        /// </summary>
		IBPMProcNotifyRecHisRepository BPMProcNotifyRecHisRepository { get; }

	     
	    /// <summary>
        /// BPMSysMessagesFailed表对应的实体仓储对象
        /// </summary>
		IBPMSysMessagesFailedRepository BPMSysMessagesFailedRepository { get; }

	     
	    /// <summary>
        /// BPMSysMessagesQueue表对应的实体仓储对象
        /// </summary>
		IBPMSysMessagesQueueRepository BPMSysMessagesQueueRepository { get; }

	     
	    /// <summary>
        /// BPMSysMessagesSucceed表对应的实体仓储对象
        /// </summary>
		IBPMSysMessagesSucceedRepository BPMSysMessagesSucceedRepository { get; }

	     
	    /// <summary>
        /// BPMSysSettings表对应的实体仓储对象
        /// </summary>
		IBPMSysSettingsRepository BPMSysSettingsRepository { get; }

	     
	    /// <summary>
        /// LocationUser表对应的实体仓储对象
        /// </summary>
		ILocationUserRepository LocationUserRepository { get; }

	     
	    /// <summary>
        /// SubscribeUsers表对应的实体仓储对象
        /// </summary>
		ISubscribeUsersRepository SubscribeUsersRepository { get; }

	  }
}