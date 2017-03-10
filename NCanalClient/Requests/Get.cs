using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.alibaba.otter.canal.protocol
{
    [global::System.Serializable, global::ProtoBuf.ProtoContract(Name = @"Get")]
    public partial class Get : global::ProtoBuf.IExtensible
    {
        public Get() { }



        [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name = @"destination", DataFormat = global::ProtoBuf.DataFormat.Default)]
        [global::System.ComponentModel.DefaultValue("")]
        public string Destination { get; set; }



        [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name = @"client_id", DataFormat = global::ProtoBuf.DataFormat.Default)]
        [global::System.ComponentModel.DefaultValue("")]
        public string ClientId { get; set; }



        [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name = @"fetch_size", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
        [global::System.ComponentModel.DefaultValue(default(int))]
        public int FetchSize { get; set; }



        [global::ProtoBuf.ProtoMember(4, IsRequired = false, Name = @"timeout", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
        [global::System.ComponentModel.DefaultValue((long)-1)]
        public long Timeout { get; set; } = -1;



        [global::ProtoBuf.ProtoMember(5, IsRequired = false, Name = @"unit", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
        [global::System.ComponentModel.DefaultValue((int)2)]
        public int Unit { get; set; } = 2;



        [global::ProtoBuf.ProtoMember(6, IsRequired = false, Name = @"auto_ack", DataFormat = global::ProtoBuf.DataFormat.Default)]
        [global::System.ComponentModel.DefaultValue((bool)false)]
        public bool AutoAck { get; set; } = false;


        private global::ProtoBuf.IExtension extensionObject;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
        { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }
}
