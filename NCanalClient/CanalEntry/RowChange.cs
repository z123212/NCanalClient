using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.alibaba.otter.canal.protocol
{
    [global::System.Serializable, global::ProtoBuf.ProtoContract(Name = @"RowChange")]
    public partial class RowChange : ProtoBuf.IExtensible
    {

        /// <summary>
        /// tableId,由数据库产生
        /// </summary>
        [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name = @"tableId", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
        [global::System.ComponentModel.DefaultValue(default(long))]
        public long TableId { get; set; }

        /// <summary>
        /// 数据变更类型
        /// </summary>
        [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name = @"eventType", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
        [global::System.ComponentModel.DefaultValue(EventType.UPDATE)]
        public EventType EventType { get; set; } = EventType.UPDATE;

        /// <summary>
        /// 标识是否是ddl语句
        /// </summary>
        [global::ProtoBuf.ProtoMember(10, IsRequired = false, Name = @"isDdl", DataFormat = global::ProtoBuf.DataFormat.Default)]
        [global::System.ComponentModel.DefaultValue((bool)false)]
        public bool IsDdl { get; set; } = false;

        /// <summary>
        /// ddl/query的sql语句
        /// </summary>
        [global::ProtoBuf.ProtoMember(11, IsRequired = false, Name = @"sql", DataFormat = global::ProtoBuf.DataFormat.Default)]
        [global::System.ComponentModel.DefaultValue("")]
        public string Sql { get; set; }

        /// <summary>
        /// 一次数据库变更可能存在多行
        /// </summary>
        [global::ProtoBuf.ProtoMember(12, Name = @"rowDatas", DataFormat = global::ProtoBuf.DataFormat.Default)]
        public List<RowData> RowDatas { get; set; } = new List<RowData>();

        /// <summary>
        /// ddl/query的schemaName，会存在跨库ddl，需要保留执行ddl的当前schemaName 
        /// </summary>
        [global::ProtoBuf.ProtoMember(14, IsRequired = false, Name = @"ddlSchemaName", DataFormat = global::ProtoBuf.DataFormat.Default)]
        [global::System.ComponentModel.DefaultValue("")]
        public string DdlSchemaName { get; set; }
        /// <summary>
        /// 预留扩展
        /// </summary>
        [global::ProtoBuf.ProtoMember(13, Name = @"props", DataFormat = global::ProtoBuf.DataFormat.Default)]
        public List<Pair> Props { get; set; } = new List<Pair>();


        private global::ProtoBuf.IExtension extensionObject;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
        { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }
}
