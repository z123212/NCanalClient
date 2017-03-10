using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace com.alibaba.otter.canal.protocol
{
    using ProtoBuf;

    [Serializable, ProtoContract(Name = @"ClientAuth")]
    public partial class ClientAuth : IExtensible
    {



        [ProtoMember(1, IsRequired = false, Name = @"username", DataFormat = DataFormat.Default), DefaultValue("")]
        public string UserName { get; set; }
        
        [ProtoMember(2, IsRequired = false, Name = @"password", DataFormat = DataFormat.Default)]
        [DefaultValue(null)]
        public byte[] Password { get; set; }


        [ProtoMember(3, IsRequired = false, Name = @"net_read_timeout", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue((int)0)]
        public int NetReadTimeout { get; set; } 

        [ProtoMember(4, IsRequired = false, Name = @"net_write_timeout", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue((int)0)]
        public int NetWritTimeout { get; set; }

        [ProtoMember(5, IsRequired = false, Name = @"destination", DataFormat = DataFormat.Default)]
        [DefaultValue("")]
        public string Destination { get; set; }
       
        [ProtoMember(6, IsRequired = false, Name = @"client_id", DataFormat = DataFormat.Default)]
        [DefaultValue("")]
        public string ClientId { get; set; }
       
        [ProtoMember(7, IsRequired = false, Name = @"filter", DataFormat = DataFormat.Default)]
        [DefaultValue("")]
        public string Filter { get; set; }
       
        [ProtoMember(8, IsRequired = false, Name = @"start_timestamp", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(default(long))]
        public long StartTimestamp { get; set; }



        private IExtension extensionObject;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        { return Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }
}
