using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.alibaba.otter.canal.protocol
{
    using ProtoBuf;

    [ProtoContract(Name = @"Compression")]
    public enum Compression
    {

        [ProtoEnum(Name = @"NONE", Value = 1)]
        NONE = 1,

        [ProtoEnum(Name = @"ZLIB", Value = 2)]
        ZLIB = 2,

        [ProtoEnum(Name = @"GZIP", Value = 3)]
        GZIP = 3,

        [ProtoEnum(Name = @"LZF", Value = 4)]
        LZF = 4
    }

    [ProtoContract(Name = @"PacketType")]
    public enum PacketType
    {

        [ProtoEnum(Name = @"HANDSHAKE", Value = 1)]
        HANDSHAKE = 1,

        [ProtoEnum(Name = @"CLIENTAUTHENTICATION", Value = 2)]
        CLIENTAUTHENTICATION = 2,

        [ProtoEnum(Name = @"ACK", Value = 3)]
        ACK = 3,

        [ProtoEnum(Name = @"SUBSCRIPTION", Value = 4)]
        SUBSCRIPTION = 4,

        [ProtoEnum(Name = @"UNSUBSCRIPTION", Value = 5)]
        UNSUBSCRIPTION = 5,

        [ProtoEnum(Name = @"GET", Value = 6)]
        GET = 6,

        [ProtoEnum(Name = @"MESSAGES", Value = 7)]
        MESSAGES = 7,

        [ProtoEnum(Name = @"CLIENTACK", Value = 8)]
        CLIENTACK = 8,

        [ProtoEnum(Name = @"SHUTDOWN", Value = 9)]
        SHUTDOWN = 9,

        [ProtoEnum(Name = @"DUMP", Value = 10)]
        DUMP = 10,

        [ProtoEnum(Name = @"HEARTBEAT", Value = 11)]
        HEARTBEAT = 11,

        [ProtoEnum(Name = @"CLIENTROLLBACK", Value = 12)]
        CLIENTROLLBACK = 12
    }

}
