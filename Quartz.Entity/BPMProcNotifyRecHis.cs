//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Quartz.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class BPMProcNotifyRecHis
    {
        public long MSGID { get; set; }
        public long STEPID { get; set; }
        public string FLOWNO { get; set; }
        public string PROCNAME { get; set; }
        public string NODENAME { get; set; }
        public string OPDEPT { get; set; }
        public string OWNERCOUNT { get; set; }
        public string OWNERNAME { get; set; }
        public Nullable<System.DateTime> RECEIVEAT { get; set; }
        public Nullable<decimal> EXPWORKTIME { get; set; }
        public Nullable<decimal> NOTIFYHOUR { get; set; }
        public Nullable<short> NOTIFYCOUNT { get; set; }
        public Nullable<System.DateTime> LASTTIME { get; set; }
        public Nullable<short> OPSTATUS { get; set; }
        public Nullable<System.DateTime> CREATETIME { get; set; }
        public string NOTIFYUSER { get; set; }
        public string NOTIFYUNAME { get; set; }
        public Nullable<short> NTYPE { get; set; }
    }
}
