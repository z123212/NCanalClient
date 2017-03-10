using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace com.alibaba.otter.canal.protocol
{
    using ProtoBuf;
    [Serializable, ProtoContract(Name = @"Packet")]
    public partial class Packet : IExtensible
    {
        [ProtoMember(1, IsRequired = false, Name = @"magic_number", DataFormat = DataFormat.TwosComplement), DefaultValue((int)17)]
        public int MagicNumber { get; set; } = 17;


        [ProtoMember(2, IsRequired = false, Name = @"version", DataFormat = DataFormat.TwosComplement), DefaultValue((int)1)]
        public int Version { get; set; } = 1;

        [ProtoMember(3, IsRequired = false, Name = @"type", DataFormat = DataFormat.TwosComplement), DefaultValue(PacketType.HANDSHAKE)]
        public PacketType Type { get; set; } = PacketType.HANDSHAKE;

        [ProtoMember(4, IsRequired = false, Name = @"compression", DataFormat = DataFormat.TwosComplement), DefaultValue(Compression.NONE)]
        public Compression Compression { get; set; } = Compression.NONE;

        [ProtoMember(5, IsRequired = false, Name = @"body", DataFormat = DataFormat.Default), DefaultValue(null)]
        public byte[] Body { get; set; } = null;

        private IExtension extensionObject;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
        }
    }
}
