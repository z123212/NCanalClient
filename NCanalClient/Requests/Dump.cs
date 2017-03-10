using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.alibaba.otter.canal.protocol
{
    [global::System.Serializable, global::ProtoBuf.ProtoContract(Name = @"Dump")]
    public partial class Dump : global::ProtoBuf.IExtensible
    {
        public Dump() { }


        /// <summary>
        /// 
        /// </summary>
        [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name = @"journal", DataFormat = global::ProtoBuf.DataFormat.Default)]
        [global::System.ComponentModel.DefaultValue("")]
        public string Journal { get; set; }

        /// <summary>
        /// 获取位置
        /// </summary>
        [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name = @"position", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
        [global::System.ComponentModel.DefaultValue(default(long))]
        public long Position { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name = @"timestamp", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
        [global::System.ComponentModel.DefaultValue((long)0)]
        public long Timestamp { get; set; } = 0;



        private global::ProtoBuf.IExtension extensionObject;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
        { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }
}
