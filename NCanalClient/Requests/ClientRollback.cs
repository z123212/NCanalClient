using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.alibaba.otter.canal.protocol
{
    [global::System.Serializable, global::ProtoBuf.ProtoContract(Name = @"ClientRollback")]
    public partial class ClientRollback : global::ProtoBuf.IExtensible
    {
        public ClientRollback() { }



        [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name = @"destination", DataFormat = global::ProtoBuf.DataFormat.Default)]
        [global::System.ComponentModel.DefaultValue("")]
        public string Destination { get; set; }



        [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name = @"client_id", DataFormat = global::ProtoBuf.DataFormat.Default)]
        [global::System.ComponentModel.DefaultValue("")]
        public string ClientId { get; set; }


        [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name = @"batch_id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
        [global::System.ComponentModel.DefaultValue(default(long))]
        public long BatchId { get; set; }



        private global::ProtoBuf.IExtension extensionObject;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
        { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }
}
