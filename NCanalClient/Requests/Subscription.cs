using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace com.alibaba.otter.canal.protocol
{
    using ProtoBuf;

    [Serializable, ProtoContract(Name = @"Sub")]
    public partial class Subscription : IExtensible
    {




        [ProtoMember(1, IsRequired = false, Name = @"destination", DataFormat = DataFormat.Default)]
        [DefaultValue("")]
        public string Destination { get; set; }


        [ProtoMember(2, IsRequired = false, Name = @"client_id", DataFormat = DataFormat.Default)]
        [DefaultValue("")]
        public string ClientId { get; set; }


        [ProtoMember(7, IsRequired = false, Name = @"filter", DataFormat = DataFormat.Default)]
        [DefaultValue("")]
        public string Filter { get; set; }


        private IExtension extensionObject;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        { return Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }
}
