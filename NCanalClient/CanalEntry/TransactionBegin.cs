using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.alibaba.otter.canal.protocol
{
    /// <summary>
    /// 开始事务的一些信息
    /// </summary>
    [global::System.Serializable, global::ProtoBuf.ProtoContract(Name = @"TransactionBegin")]
    public partial class TransactionBegin : ProtoBuf.IExtensible
    {
        /// <summary>
        /// 执行的thread Id
        /// </summary>
        [global::ProtoBuf.ProtoMember(4, IsRequired = false, Name = @"threadId", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
        [global::System.ComponentModel.DefaultValue(default(long))]
        public long ThreadId { get; set; }
        /// <summary>
        /// 预留扩展
        /// </summary>
        [global::ProtoBuf.ProtoMember(3, Name = @"props", DataFormat = global::ProtoBuf.DataFormat.Default)]
        public List<Pair> Props { get; set; } = new List<Pair>();


        private global::ProtoBuf.IExtension extensionObject;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
        { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }

    }
    /// <summary>
    /// 结束事务的一些信息
    /// </summary>
    [global::System.Serializable, global::ProtoBuf.ProtoContract(Name = @"TransactionEnd")]
    public partial class TransactionEnd : ProtoBuf.IExtensible
    {
        /// <summary>
        /// 事务号
        /// </summary>
        [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name = @"transactionId", DataFormat = global::ProtoBuf.DataFormat.Default)]
        [global::System.ComponentModel.DefaultValue("")]
        public string TransactionId { get; set; }

        /// <summary>
        /// 预留扩展
        /// </summary>
        [global::ProtoBuf.ProtoMember(3, Name = @"props", DataFormat = global::ProtoBuf.DataFormat.Default)]
        public List<Pair> Props { get; set; } = new List<Pair>();

        private global::ProtoBuf.IExtension extensionObject;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
        { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }

    }
}
