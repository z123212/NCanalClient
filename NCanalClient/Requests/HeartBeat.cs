using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace com.alibaba.otter.canal.protocol
{
    using ProtoBuf;

    [Serializable, ProtoContract(Name = @"HeartBeat")]
    public partial class HeartBeat : IExtensible
    {


        [ProtoMember(1, IsRequired = false, Name = @"send_timestamp", DataFormat = DataFormat.TwosComplement), DefaultValue(default(long))]
        public long SendTimestamp { get; set; }


        [ProtoMember(2, IsRequired = false, Name = @"start_timestamp", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(default(long))]
        public long StartTimestamp { get; set; }


        private IExtension extensionObject;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        { return Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }
}
