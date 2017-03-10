using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace com.alibaba.otter.canal.protocol
{
    [global::System.Serializable, global::ProtoBuf.ProtoContract(Name = @"Entry")]
    public class Entry: IExtensible
    {
        /// <summary>
        /// 消息头
        /// </summary>
        [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name = @"header", DataFormat = global::ProtoBuf.DataFormat.Default)]
        [global::System.ComponentModel.DefaultValue(null)]
        public Header Header { get; set; }

        /// <summary>
        /// 打散后的事件类型
        /// </summary>
        [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name = @"entryType", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
        [global::System.ComponentModel.DefaultValue(EntryType.ROWDATA)]
        public EntryType EntryType { get; set; } = EntryType.ROWDATA;

        /// <summary>
        /// 传输的二进制数组
        /// </summary>
        [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name = @"storeValue", DataFormat = global::ProtoBuf.DataFormat.Default)]
        [global::System.ComponentModel.DefaultValue(null)]
        public byte[] StoreValue { get; set; }

        private global::ProtoBuf.IExtension extensionObject;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
        { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }
}
