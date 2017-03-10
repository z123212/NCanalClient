using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;
using Google.ProtocolBuffers;
using Poro = com.alibaba.otter.canal.protocol;


namespace NCanalClient
{
    public class TempClass
    {
        static ILog Logger = LogManager.GetLogger<TempClass>();


        public static void TestMessage(SimpleCanalConnector connector)
        {
            var batchSize = 5 * 1024;
            var msg = connector.GetWithoutAck(batchSize);

            if (msg.Entries != null && msg.Entries.Count > 0)
            {
                foreach (var item in msg.Entries)
                {
                    var exeTime = item.Header.ExecuteTime;
                    var delayTime = GetTimeStamp() - exeTime;
                    var log = $"日志名:{item.Header.LogfileName};日志位置：{item.Header.LogfileOffset};执行时间：{exeTime},延迟时间:{delayTime};Schema名称:{item.Header.SchemaName};表名:{item.Header.TableName}";
                    if (item.EntryType == com.alibaba.otter.canal.protocol.EntryType.TRANSACTIONBEGIN || item.EntryType == com.alibaba.otter.canal.protocol.EntryType.TRANSACTIONEND)
                    {
                        if (item.EntryType == com.alibaba.otter.canal.protocol.EntryType.TRANSACTIONBEGIN)
                        {
                            var begin = Poro.TransactionBegin.ParseFrom(item.StoreValue);
                            Logger.Trace($"事务开始--->id:{begin.ThreadId};{log} ");
                        }
                        else if (item.EntryType == com.alibaba.otter.canal.protocol.EntryType.TRANSACTIONEND)
                        {
                            var end = Poro.TransactionEnd.ParseFrom(item.StoreValue);
                            
                            Logger.Trace($@"
-------------------------
事务结束------>id:{end.TransactionId}；{log}");
                        }
                        continue;
                    }
                    if (item.EntryType == Poro.EntryType.ROWDATA)
                    {
                        var row = Poro.RowChange.ParseFrom(item.StoreValue);
                        var evenType = row.EventType;
                        Console.WriteLine(log);
                        if (evenType == com.alibaba.otter.canal.protocol.EventType.QUERY || row.IsDdl)
                        {
                            Logger.Debug($"SQL-->{row.Sql} SEP");
                            continue;
                        }
                        foreach (var r in row.RowDatasList)
                        {
                            if (evenType == Poro.EventType.DELETE)
                            {
                                ConsoleColumn("删除的原始数据:", r.BeforeColumnsList);
                            }
                            else if (evenType == Poro.EventType.INSERT)
                            {
                                ConsoleColumn("新添加数据：", r.AfterColumnsList);
                            }
                            else if (evenType == Poro.EventType.UPDATE)
                            {
                                ConsoleColumn("更新前数据：", r.BeforeColumnsList);
                                ConsoleColumn("更新后数据：", r.AfterColumnsList);
                            }
                            else
                            {
                                ConsoleColumn($"操作类型{evenType.ToString()}", r.AfterColumnsList);
                            }
                        }
                    }
                }
            }

        }

        private static void ConsoleColumn(string prefix, ICollection<Poro.Column> cols)
        {
            var sb = new StringBuilder(prefix);
            sb.AppendLine();
            foreach (var item in cols)
            {
                sb.Append(item.Name).Append(":").Append(item.Value).Append("；type=").Append(item.MysqlType).Append(";update=").Append(item.Updated).AppendLine();
            }
            Logger.Warn(sb.ToString());
        }

        /// <summary>  
        /// 获取时间戳  
        /// </summary>  
        /// <returns></returns>  
        public static long GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds);
        }
    }
}
