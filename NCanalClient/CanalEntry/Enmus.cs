using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.alibaba.otter.canal.protocol
{
    using ProtoBuf;
    /// <summary>
    /// 打散后的事件类型，主要用于标识事务的开始，变更数据，结束
    /// </summary>
    [ProtoContract(Name = @"EntryType")]
    public enum EntryType
    {
        [ProtoEnum(Name = @"TRANSACTIONBEGIN", Value = 1)]
        TRANSACTIONBEGIN = 1,
        [ProtoEnum(Name = @"ROWDATA", Value = 2)]
        ROWDATA = 2,

        [ProtoEnum(Name = @"TRANSACTIONEND", Value = 3)]
        TRANSACTIONEND = 3,

        /// <summary>
        /// 心跳类型，内部使用，外部暂不可见，可忽略 
        /// </summary>
        [ProtoEnum(Name = @"HEARTBEAT", Value = 4)]
        HEARTBEAT = 4
    }
    /// <summary>
    /// 事件类型
    /// </summary>
    [ProtoContract(Name = @"EventType")]
    public enum EventType
    {
        [ProtoEnum(Name = @"INSERT", Value = 1)]
        INSERT = 1,

        [ProtoEnum(Name = @"UPDATE", Value = 2)]
        UPDATE = 2,

        [ProtoEnum(Name = @"DELETE", Value = 3)]
        DELETE = 3,

        [ProtoEnum(Name = @"CREATE", Value = 4)]
        CREATE = 4,

        [ProtoEnum(Name = @"ALTER", Value = 5)]
        ALTER = 5,

        [ProtoEnum(Name = @"ERASE", Value = 6)]
        ERASE = 6,

        [ProtoEnum(Name = @"QUERY", Value = 7)]
        QUERY = 7,

        [ProtoEnum(Name = @"TRUNCATE", Value = 8)]
        TRUNCATE = 8,

        [ProtoEnum(Name = @"RENAME", Value = 9)]
        RENAME = 9,

        [ProtoEnum(Name = @"CINDEX", Value = 10)]
        CINDEX = 10,

        [ProtoEnum(Name = @"DINDEX", Value = 11)]
        DINDEX = 11
    }
    /// <summary>
    /// 数据库类型
    /// </summary>
    [ProtoContract(Name = @"Type")]
    public enum DbType
    {
        [ProtoEnum(Name = @"ORACLE", Value = 1)]
        ORACLE = 1,

        [ProtoEnum(Name = @"MYSQL", Value = 2)]
        MYSQL = 2,

        [ProtoEnum(Name = @"PGSQL", Value = 3)]
        PGSQL = 3
    }
}
