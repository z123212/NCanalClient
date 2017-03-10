using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace com.alibaba.otter.canal.protocol
{
    [global::System.Serializable, global::ProtoBuf.ProtoContract(Name = @"Header")]
    public partial class Header: ProtoBuf.IExtensible
    {
        /// <summary>
        /// 协议的版本号
        /// </summary>
        [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name = @"version", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
        [global::System.ComponentModel.DefaultValue((int)1)]
        public int Version { get; set; } = 1;

        /// <summary>
        /// binlog/redolog 文件名
        /// </summary>
        [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name = @"logfileName", DataFormat = global::ProtoBuf.DataFormat.Default)]
        [global::System.ComponentModel.DefaultValue("")]
        public string LogfileName { get; set; }

        /// <summary>
        /// binlog/redolog 文件的偏移位置
        /// </summary>
        [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name = @"logfileOffset", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
        [global::System.ComponentModel.DefaultValue(default(long))]
        public long LogfileOffset { get; set; }

        /// <summary>
        /// 服务端serverId
        /// </summary>
        [global::ProtoBuf.ProtoMember(4, IsRequired = false, Name = @"serverId", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
        [global::System.ComponentModel.DefaultValue(default(long))]
        public long ServerId { get; set; }

        /// <summary>
        /// 变更数据的编码
        /// </summary>
        [global::ProtoBuf.ProtoMember(5, IsRequired = false, Name = @"serverenCode", DataFormat = global::ProtoBuf.DataFormat.Default)]
        [global::System.ComponentModel.DefaultValue("")]
        public string ServerenCode { get; set; }


        /// <summary>
        /// 变更数据的执行时间
        /// </summary>
        [global::ProtoBuf.ProtoMember(6, IsRequired = false, Name = @"executeTime", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
        [global::System.ComponentModel.DefaultValue(default(long))]
        public long ExecuteTime { get; set; }

        /// <summary>
        /// 变更数据的来源
        /// </summary>
        [global::ProtoBuf.ProtoMember(7, IsRequired = false, Name = @"sourceType", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
        [global::System.ComponentModel.DefaultValue(DbType.MYSQL)]
        public DbType SourceType { get; set; } = DbType.MYSQL;


        /// <summary>
        /// 变更数据的schemaname
        /// </summary>
        [global::ProtoBuf.ProtoMember(8, IsRequired = false, Name = @"schemaName", DataFormat = global::ProtoBuf.DataFormat.Default)]
        [global::System.ComponentModel.DefaultValue("")]
        public string SchemaName { get; set; }

        /// <summary>
        /// 变更数据的tablename
        /// </summary>
        [global::ProtoBuf.ProtoMember(9, IsRequired = false, Name = @"tableName", DataFormat = global::ProtoBuf.DataFormat.Default)]
        [global::System.ComponentModel.DefaultValue("")]
        public string TableName { get; set; }

        /// <summary>
        /// 每个event的长度
        /// </summary>
        [global::ProtoBuf.ProtoMember(10, IsRequired = false, Name = @"eventLength", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
        [global::System.ComponentModel.DefaultValue(default(long))]
        public long EventLength { get; set; }

        /// <summary>
        /// 数据变更类型
        /// </summary>
        [global::ProtoBuf.ProtoMember(11, IsRequired = false, Name = @"eventType", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
        [global::System.ComponentModel.DefaultValue(EventType.UPDATE)]
        public EventType EventType { get; set; } = EventType.UPDATE;
        /// <summary>
        /// 预留扩展
        /// </summary>
        [global::ProtoBuf.ProtoMember(12, Name = @"props", DataFormat = global::ProtoBuf.DataFormat.Default)]
        public ICollection<Pair> Props { get; set; }

        private global::ProtoBuf.IExtension extensionObject;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
        { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }
}
