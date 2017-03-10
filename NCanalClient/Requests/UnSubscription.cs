using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.alibaba.otter.canal.protocol
{
    [global::System.Serializable, global::ProtoBuf.ProtoContract(Name = @"Unsub")]
    public partial class UnSubscription : ProtoBuf.IExtensible
    { 
        [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name = @"destination", DataFormat = global::ProtoBuf.DataFormat.Default)]
        [global::System.ComponentModel.DefaultValue("")]
        public string Destination { get; set; }


        [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name = @"client_id", DataFormat = global::ProtoBuf.DataFormat.Default)]
        [global::System.ComponentModel.DefaultValue("")]
        public string ClientId { get; set; }


        [global::ProtoBuf.ProtoMember(7, IsRequired = false, Name = @"filter", DataFormat = global::ProtoBuf.DataFormat.Default)]
        [global::System.ComponentModel.DefaultValue("")]
        public string Filter { get; set; }


        private global::ProtoBuf.IExtension extensionObject;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
        { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }

    }
}
