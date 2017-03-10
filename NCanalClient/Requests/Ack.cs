using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace com.alibaba.otter.canal.protocol
{
    using ProtoBuf;


    [Serializable, ProtoContract(Name = @"Ack")]
    public partial class Ack : IExtensible
    {
        public Ack() { }


        [ProtoMember(1, IsRequired = false, Name = @"error_code", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue((int)0)]
        public int ErrorCode { get; set; }


        [ProtoMember(2, IsRequired = false, Name = @"error_message", DataFormat = DataFormat.Default)]
        [DefaultValue("")]
        public string ErrorMessage { get; set; }


        private IExtension extensionObject;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        { return Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }
}
