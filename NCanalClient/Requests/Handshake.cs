using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace com.alibaba.otter.canal.protocol
{
    using ProtoBuf;

    [Serializable, ProtoContract(Name = @"Handshake")]
    public partial class Handshake : IExtensible
    {
        public Handshake() { }

        [ProtoMember(1, IsRequired = false, Name = @"communication_encoding", DataFormat = DataFormat.Default)]
        [DefaultValue(@"utf8")]
        public string CommunicationEncoding { get; set; } = "utf8";
        private byte[] _seeds = null;
        [ProtoMember(2, IsRequired = false, Name = @"seeds", DataFormat = DataFormat.Default)]
        [DefaultValue(null)]
        public byte[] seeds
        {
            get { return _seeds; }
            set { _seeds = value; }
        }
        private readonly List<Compression> _supported_compressions = new List<Compression>();
        [ProtoMember(3, Name = @"supported_compressions", DataFormat = DataFormat.TwosComplement)]
        public List<Compression> SupportedCompressions
        {
            get { return _supported_compressions; }
        }

        private IExtension extensionObject;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        { return Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }

}
