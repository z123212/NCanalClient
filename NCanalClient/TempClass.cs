/*
 * 订阅及消费canal服务端消息实例,注意仅仅是一个示例.
 */

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
            var batchId = msg?.Id ?? 0;
            if (msg.Entries != null && msg.Entries.Count > 0)
            {
                foreach (var item in msg.Entries)
                {
                    var exeTime = item.Header.ExecuteTime;
                    var delayTime = GetTimeStamp(DateTime.Now,13) - exeTime;
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
                /*
                 * 注意:
                 * 经生测试,获取到的消息需要确认消费掉,如果不确认,服务端会将该消息存在内存中,数据量多了后
                 * 会产生Canal主进程运行,但是对应的实例却死掉.造成无法长期进行服务,所以一定要注意*确认消息消费*
                 */
                if (batchId > 0)
                {
                    connector.Ack(batchId);
                    Logger.Trace($"调用connector.Ack({batchId})");
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
        public static long GetTimeStamp(DateTime time, int len = 10)
        {
            if (len > 18 || len <= 0)
            {
                throw new ArgumentOutOfRangeException("len", "长度不能大于18,或小于等于0");
            }
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = (time.Ticks - startTime.Ticks);// / 10000;   //除10000调整为13位   
            string tStr = t.ToString();
            var tLen = tStr.Length;
            var less = tLen - len;
            if (less == 0)
            {
                return t;
            }
            else if (less > 0)
            {
                return long.Parse(tStr.Substring(0, len));
            }
            else
            {
                return long.Parse(tStr + FixedZeroStr(0 - less));
            }
            //  return t;
        }

        private static string FixedZeroStr(int len)
        {
            if (len <= 0)
            {
                return "";
            }
            char[] res = new char[len];
            while (len-- > 0)
            {
                res[len] = '0';
            }

            return new string(res);
        }
    }
}
