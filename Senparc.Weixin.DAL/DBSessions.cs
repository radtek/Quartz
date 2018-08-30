   

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Senparc.Weixin.DAL
{
	using Senparc.Weixin.IDAL;

    //一次跟数据库交互的会话
    public partial class DbSession : IDbSession //代表应用程序跟数据库之间的一次会话，也是数据库访问层的统一入口
    {

		public IBPMProcNotifyRecRepository BPMProcNotifyRecRepository
		{
			get { return new BPMProcNotifyRecRepository(); }
		}

			public IBPMProcNotifyRecHisRepository BPMProcNotifyRecHisRepository
		{
			get { return new BPMProcNotifyRecHisRepository(); }
		}

			public IBPMSysMessagesFailedRepository BPMSysMessagesFailedRepository
		{
			get { return new BPMSysMessagesFailedRepository(); }
		}

			public IBPMSysMessagesQueueRepository BPMSysMessagesQueueRepository
		{
			get { return new BPMSysMessagesQueueRepository(); }
		}

			public IBPMSysMessagesSucceedRepository BPMSysMessagesSucceedRepository
		{
			get { return new BPMSysMessagesSucceedRepository(); }
		}

			public IBPMSysSettingsRepository BPMSysSettingsRepository
		{
			get { return new BPMSysSettingsRepository(); }
		}

			public ILocationUserRepository LocationUserRepository
		{
			get { return new LocationUserRepository(); }
		}

			public ISubscribeUsersRepository SubscribeUsersRepository
		{
			get { return new SubscribeUsersRepository(); }
		}

	}
}