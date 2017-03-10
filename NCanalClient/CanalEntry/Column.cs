using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace com.alibaba.otter.canal.protocol
{
    /// <summary>
    /// 每个字段的数据结构
    /// </summary>
    [global::System.Serializable, global::ProtoBuf.ProtoContract(Name = @"Column")]
    public partial class Column: ProtoBuf.IExtensible
    {
        /// <summary>
        /// 字段下标
        /// </summary>
        [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name = @"index", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
        [global::System.ComponentModel.DefaultValue(default(int))]
        public int Index { get; set; }

        /// <summary>
        /// 字段类型
        /// </summary>
        [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name = @"sqlType", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
        [global::System.ComponentModel.DefaultValue(default(int))]
        public int SqlType { get; set; }

        /// <summary>
        /// 字段名称(忽略大小写)，在mysql中是没有的
        /// </summary>
        [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name = @"name", DataFormat = global::ProtoBuf.DataFormat.Default)]
        [global::System.ComponentModel.DefaultValue("")]
        public string Name { get; set; }

        /// <summary>
        /// 是否是主键
        /// </summary>
        [global::ProtoBuf.ProtoMember(4, IsRequired = false, Name = @"isKey", DataFormat = global::ProtoBuf.DataFormat.Default)]
        [global::System.ComponentModel.DefaultValue(default(bool))]
        public bool IsKey { get; set; }

        /// <summary>
        /// 如果EventType=UPDATE,用于标识这个字段值是否有修改
        /// </summary>
        [global::ProtoBuf.ProtoMember(5, IsRequired = false, Name = @"updated", DataFormat = global::ProtoBuf.DataFormat.Default)]
        [global::System.ComponentModel.DefaultValue(default(bool))]
        public bool Updated { get; set; }

        /// <summary>
        /// 标识是否为空
        /// </summary>
        [global::ProtoBuf.ProtoMember(6, IsRequired = false, Name = @"isNull", DataFormat = global::ProtoBuf.DataFormat.Default)]
        [global::System.ComponentModel.DefaultValue((bool)false)]
        public bool IsNull { get; set; }

        /// <summary>
        /// 字段值,timestamp,Datetime是一个时间格式的文本
        /// </summary>
        [global::ProtoBuf.ProtoMember(8, IsRequired = false, Name = @"value", DataFormat = global::ProtoBuf.DataFormat.Default)]
        [global::System.ComponentModel.DefaultValue("")]
        public string Value { get; set; }

        /// <summary>
        /// 对应数据对象原始长度
        /// </summary>
        [global::ProtoBuf.ProtoMember(9, IsRequired = false, Name = @"length", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
        [global::System.ComponentModel.DefaultValue(default(int))]
        public int Length { get; set; }


        /// <summary>
        /// 字段mysql类型
        /// </summary>
        [global::ProtoBuf.ProtoMember(10, IsRequired = false, Name = @"mysqlType", DataFormat = global::ProtoBuf.DataFormat.Default)]
        [global::System.ComponentModel.DefaultValue("")]
        public string MysqlType { get; set; }
        /// <summary>
        /// 预留扩展
        /// </summary>
        [global::ProtoBuf.ProtoMember(7, Name = @"props", DataFormat = global::ProtoBuf.DataFormat.Default)]
        public ICollection<Pair> Props { get; set; }



        private global::ProtoBuf.IExtension extensionObject;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
        { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }
}
