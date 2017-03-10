using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.alibaba.otter.canal.protocol
{
    /// <summary>
    /// 预留扩展
    /// </summary>
    [global::System.Serializable, global::ProtoBuf.ProtoContract(Name = @"Pair")]
    public partial class Pair : ProtoBuf.IExtensible
    {
        /// <summary>
        /// key
        /// </summary>
        [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name = @"key", DataFormat = global::ProtoBuf.DataFormat.Default)]
        [global::System.ComponentModel.DefaultValue("")]
        public string Key { get; set; }

        /// <summary>
        /// value
        /// </summary>
        [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name = @"value", DataFormat = global::ProtoBuf.DataFormat.Default)]
        [global::System.ComponentModel.DefaultValue("")]
        public string Value { get; set; }

        private global::ProtoBuf.IExtension extensionObject;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
        { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }

    }
}
