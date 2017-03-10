using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using com.alibaba.otter.canal.protocol;
using System.IO;
using NCanalClient.Protos;
using Google.ProtocolBuffers.Descriptors;
using Google.ProtocolBuffers;

using Proto = com.alibaba.otter.canal.protocol;
using NCanalClient.TcpHandler;

namespace NCanalClient
{
    public class SimpleCanalConnector : ICanalConnector
    {


        private string UserName;

        private string Password;

        /// <summary>
        /// 毫秒
        /// </summary>
        private int SoTimeout;
        /// <summary>
        /// 记录上一次的filter提交值,便于自动重试时提交
        /// </summary>
        private string Filter;
        /// <summary>
        /// 代表connected是否已正常执行，因为有HA，不代表在工作中
        /// </summary>
        private bool Connected = false;
        /// <summary>
        /// 是否在connect链接成功后，自动执行rollback操作
        /// </summary>
        private bool RollBackOnConnect = true;
        /// <summary>
        /// 是否在connect断开链接成功后，自动执行rollback操作
        /// </summary>
        private bool RollBackOnDisConnect = false;
        /// <summary>
        /// 运行控制
        /// </summary>
        private ClientRunningMonitor RunningMonitor;

        private TcpClient Client;
        //  private TcpSession _Client;

        private string Address;

        private readonly string Destination;

        private readonly string ClientId;

        public SimpleCanalConnector(string address, string username, string password, string destination, int soTimeout = 60000, string clientId = null)
        {
            Address = address;

            UserName = username;
            Password = password;
            SoTimeout = soTimeout;
            Destination = destination;
            ClientId = clientId ?? "0";

            //if (string.IsNullOrEmpty(Destination))
            //{
            //    Destination = "DefaultClientForDonetDestination";
            //}
        }

        public void Ack(long batchId)
        {
            Connect();
            ClientAck ca = ClientAck.CreateBuilder()
                .SetDestination(Destination)
                .SetClientId(ClientId)
                .SetBatchId(batchId)
                .Build();
            var p = Packet.CreateBuilder()
                .SetType(PacketType.CLIENTACK)
                .SetBody(ca.ToByteString())
                .Build()
                .ToByteArray();

            Client.Send(p);

        }

        public bool CheckValid()
        {
            /*
             检查下链接是否合法
 几种case下链接不合法:
 1. 链接canal server失败，一直没有一个可用的链接，返回false
 2. 当前客户端在进行running抢占的时候，做为备份节点存在，非处于工作节点，返回false
 
 说明：
 a. 当前客户端一旦做为备份节点存在，当前所有的对CanalConnector的操作都会处于阻塞状态，直到转为工作节点
 b. 所以业务方最好定时调用checkValid()方法用，比如调用CanalConnector所在线程的interrupt，直接退出CanalConnector，并根据自己的需要退出自己的资源
             */
            return true;
            throw new NotImplementedException();
        }

        public void Connect()
        {
            if (Connected)
            {
                return;
            }
            if (Client == null)
            {
                var ads = Address.Split(':');
                var host = ads[0];
                if (ads.Length > 1)
                {
                    var port = int.Parse(ads[1]);
                    Client = new TcpClient(ads[0], port, SoTimeout);
                    // _Client = new TcpSession(ads[0], port, SoTimeout);
                }
                else
                {
                    Client = new TcpClient(ads[0], timeOut: SoTimeout);
                    //_Client = new TcpSession(ads[0], timeOut: SoTimeout);

                }

                Client.Connect();
                //  _Client.StartConnection();
                Connected = Client.IsConnected;
                if (!Client.IsConnected)
                {
                    throw Client.LastException;
                }
                else
                {
                    DoConnect();
                }

            }
        }



        private void DoConnect()
        {
            //_Client.Recevie(ms =>
            //{
            //    var p = Packet.ParseFrom(ms);
            //    if (p != null)
            //    {
            //        if (p.Type != PacketType.HANDSHAKE)
            //        {
            //            throw new Exception("expect handshake but found other type.");
            //        }
            //        var data = CreatePacket(PacketType.CLIENTAUTHENTICATION, () =>
            //{
            //    return ClientAuth.CreateBuilder()
            //    .SetUsername(UserName ?? "")
            //    .SetNetReadTimeout(SoTimeout)
            //    .SetNetWriteTimeout(SoTimeout)
            //    .Build()
            //    .ToByteString();
            //});
            //        _Client.Send(data);
            //        _Client.Recevie(m =>
            //        {
            //            var pR = Packet.ParseFrom(ms);
            //            if (pR.Type != PacketType.ACK)
            //            {
            //                throw new Exception("unexpected packet type when ack is expected");
            //            }

            //            var ack = Proto.Ack.ParseFrom(pR.Body);
            //            if (ack.ErrorCode > 0)
            //            {
            //                throw new Exception("omething goes wrong when doing authentication:" + ack.ErrorMessage);
            //            }
            //            Connected = true;
            //        });
            //    }
            //});
            var r = Client.Receive();
            if (r.Count > 0)
            {
                var p = Packet.ParseFrom(r.ToArray());
                if (p != null)
                {
                    //if (p.HasVersion && p.Version != 1)
                    //{
                    //    throw new Exception("unsupported version at this client.");
                    //}

                    if (p.Type != PacketType.HANDSHAKE)//判断是否与服务端进行握手
                    {
                        throw new Exception("expect handshake but found other type.");
                    }
                    var data = CreatePacket(PacketType.CLIENTAUTHENTICATION, () =>
        {
            return ClientAuth.CreateBuilder()
            .SetUsername(UserName ?? "")
            .SetNetReadTimeout(SoTimeout)
            .SetNetWriteTimeout(SoTimeout)
            .Build()
            .ToByteString();
        });

                    var rPacket = TcpClient.Send(data, Client);

                    if (rPacket.Type != PacketType.ACK)
                    {
                        throw new Exception("unexpected packet type when ack is expected");
                    }

                    var ack = Proto.Ack.ParseFrom(rPacket.Body);
                    if (ack.ErrorCode > 0)
                    {
                        throw new Exception("omething goes wrong when doing authentication:" + ack.ErrorMessage);
                    }
                    Connected = true;

                }

            }
        }

        public void Disconnect()
        {
            Client.Disconnect();
            Connected = Client.IsConnected;
            //_Client.Disconnect();
            //Connected = false;

        }

        public Message Get(int batchSize)
        {
            return Get(batchSize, -1, TimeUnit.Microseconds);
        }

        public Message Get(int batchSize, long timeout, TimeUnit unit)
        {
            Message message = GetWithoutAck(batchSize, timeout, unit);
            Ack(message.Id);
            return message;
        }

        public Message GetWithoutAck(int batchSize)
        {

            return GetWithoutAck(batchSize, -1);
        }



        public Message GetWithoutAck(int batchSize, long timeout = -1, TimeUnit unit = TimeUnit.Microseconds)
        {
            Connect();
            int size = batchSize <= 0 ? 100 : batchSize;
            long time = (timeout < 0) ? -1 : timeout;

            var p = Packet.CreateBuilder()
                  .SetType(PacketType.GET)
                  .SetBody(
                com.alibaba.otter.canal.protocol.Get.CreateBuilder()
                .SetAutoAck(false)
                .SetDestination(Destination)
                .SetClientId(ClientId)
                .SetFetchSize(size)
                .SetTimeout(time)
                .SetUnit(unit.GetHashCode())
                .Build()
                .ToByteString()
                ).Build()
                .ToByteArray();

            // 采用新连接管理类
            // _Client.Send(p);
            //  return ReadMessage();

            //第一版本连接管理
            /*  var datas = ProtobufHelper.Serialize2Byte(packet);*/


            var pResult = TcpClient.Send(p, Client);
            switch (pResult.Type)
            {
                case PacketType.ACK:
                    Ack ack = com.alibaba.otter.canal.protocol.Ack.ParseFrom(pResult.Body); // ProtobufHelper.DeSerialize<Ack>(pResult.Body);
                    throw new Exception("出现错误: " + ack.ErrorMessage);
                case PacketType.MESSAGES:
                    if (pResult.Compression != Compression.NONE)
                        throw new Exception("compression is not supported in this connector");

                    var m = Messages.ParseFrom(pResult.Body); //ProtobufHelper.DeSerialize<Messages>(pResult.Body);
                    var result = new Message(m.BatchId, new List<Entry>());
                    foreach (var item in m.Messages_List)
                    {
                        var i = Entry.ParseFrom(item);//ProtobufHelper.DeSerialize<Entry>(item);
                        if (i != null)
                        {
                            result.Entries.Add(i);
                        }
                    }
                    return result;
                default:
                    throw new Exception("unexpected packet type:" + pResult.GetType().FullName);
            }

        }

        private Message ReadMessage()
        {

            //var result = default(Message);
            //_Client.Recevie(ms =>
            // {
            //     var pR = Packet.ParseFrom(ms);
            //     switch (pR.Type)
            //     {
            //         case PacketType.ACK:
            //             Ack ack = com.alibaba.otter.canal.protocol.Ack.ParseFrom(pR.Body); // ProtobufHelper.DeSerialize<Ack>(pResult.Body);
            //             throw new Exception("something goes wrong with reason: " + ack.ErrorMessage);
            //         case PacketType.MESSAGES:
            //             if (pR.Compression != Compression.NONE)
            //                 throw new Exception("compression is not supported in this connector");

            //             var m = Messages.ParseFrom(pR.Body); //ProtobufHelper.DeSerialize<Messages>(pResult.Body);
            //             result = new Message(m.BatchId, new List<Entry>());
            //             foreach (var item in m.Messages_List)
            //             {
            //                 var i = Entry.ParseFrom(item);//ProtobufHelper.DeSerialize<Entry>(item);
            //                 if (i != null)
            //                 {
            //                     result.Entries.Add(i);
            //                 }
            //             }
            //             break;
            //         default:
            //             throw new Exception("unexpected packet type:" + pR.GetType().FullName);
            //     }
            // });
            //return result;

            throw new NotImplementedException();



        }

        public void Rollback()
        {
            Rollback(0L);
        }

        public void Rollback(long batchId)
        {
            Connect();
            var p = Packet.CreateBuilder()
                .SetType(PacketType.CLIENTROLLBACK)
                .SetBody(
                ClientRollback.CreateBuilder()
                .SetDestination(Destination)
                .SetClientId(ClientId)
                .SetBatchId(batchId)
                .Build().ToByteString()
                ).Build().ToByteArray();
            Client.Send(p);
            //  _Client.Send(p);

        }



        public void Subscribe()
        {
            Subscribe("");
        }

        public void Subscribe(string filter)
        {
            Connect();
            var d = CreatePacket(PacketType.SUBSCRIPTION, () =>
            {
                return Sub.CreateBuilder()
                .SetClientId(ClientId)
                .SetDestination(Destination)
                .SetFilter(filter ?? "")
                .Build()
                .ToByteString();
            });

            var p = TcpClient.Send(d, Client);
            var ack = Proto.Ack.ParseFrom(p.Body);
            if (ack.ErrorCode > 0)
            {
                var msg = $"订阅失败：{ack.ErrorMessage}";
                //  Console.WriteLine(msg);
                throw new Exception(msg);
            }
            else
            {
                Filter = filter;
            }
            // _Client.Send(d);



        }

        public void UnSubscribe()
        {
            Connect();
            var p = CreatePacket(PacketType.UNSUBSCRIPTION, () =>
            {
                return Unsub.CreateBuilder()
                .SetClientId(ClientId)
                .SetDestination(Destination)
                .Build()
                .ToByteString();
            });
            var pResult = TcpClient.Send(p, Client);
            if (pResult != null)
            {
                var ack = global::com.alibaba.otter.canal.protocol.Ack.ParseFrom(pResult.Body);
                if (ack.ErrorCode > 0)
                {
                    throw new Exception("取消订阅错误，信息：" + ack.ErrorMessage);
                }
            }

            //_Client.Send(p);
            //_Client.Recevie(ms =>
            //{
            //    var ack = Proto.Ack.ParseFrom(ms);
            //    if (ack.ErrorCode > 0)
            //    {
            //        throw new Exception("取消订阅错误，信息：" + ack.ErrorMessage);
            //    }
            //});
        }

        private byte[] CreatePacket(PacketType type, Func<ByteString> byteString)
        {
            return Packet.CreateBuilder()
                .SetType(type)
                .SetBody(byteString())
                .Build()
                .ToByteArray();
        }

    }


    public class TcpClient
    {
        private string HostAddress;

        private Socket _Socket;

        private int Port = 11111;

        /// <summary>
        /// 超时时间
        /// </summary>
        public int TimeOut { get; private set; }

        /// <summary>
        /// 异常信息
        /// </summary>
        public Exception LastException { get; private set; }


        /// <summary>
        /// 是否连接
        /// </summary>
        public bool IsConnected { get; private set; } = false;


        private SocketAsyncEventArgs mSAEA = new SocketAsyncEventArgs();

        /// <summary>
        /// 缓冲区长度
        /// </summary>
        public long BufferLength { get; private set; }


        public TcpClient(string host, int port = 11111, int timeOut = 6000)
        {
            HostAddress = host;
            Port = port;
            TimeOut = timeOut;
            mSAEA.SetBuffer(new byte[1024 * 2], 0, 1024 * 2);
            BufferLength = mSAEA.Buffer.LongLength;
        }

        public void Connect()
        {
            if (_Socket != null && _Socket.Connected)
            {
                return;
            }
            var ips = Dns.GetHostAddresses(HostAddress);
            if (ips.Length == 0)
            {
                throw new Exception("获取address出错");
            }
            try
            {
                _Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _Socket.ReceiveTimeout = TimeOut;
                _Socket.SendTimeout = TimeOut;
                _Socket.Connect(ips[0], Port);
                IsConnected = true;
            }
            catch (Exception ex)
            {
                Disconnect();
                LastException = ex;
                IsConnected = false;
            }
        }

        public void Disconnect()
        {
            try
            {
                IsConnected = false;
                _Socket?.Shutdown(SocketShutdown.Both);

                _Socket?.Close();
                if (_Socket != null)
                {
                    _Socket = null;
                }
            }
            catch (Exception ex)
            {
                LastException = ex;
                throw;
            }
        }

        public TcpReceiveArgs Receive(int count)
        {
            try
            {
                var total = count;

                using (MemoryStream ms = new MemoryStream())
                {
                    while (count > 0)
                    {
                        int rcount = _Socket.Receive(mSAEA.Buffer, (count > BufferLength ? (int)BufferLength : count), SocketFlags.None);
                        ms.Write(mSAEA.Buffer, 0, rcount);
                        count -= rcount;
                    }

                    return new TcpReceiveArgs()
                    {
                        Client = this,
                        Count = total,
                        Data = ms.ToArray(),
                        Offset = 0
                    };
                }
                //if (rcount == 0)
                //    throw new Exception(string.Format("{0} 连接已断开!", HostAddress));



            }
            catch (Exception e_)
            {
                Disconnect();
                throw e_;
            }
        }
        public TcpReceiveArgs Receive()
        {
            try
            {
                var lenth = ReadHeadLenth();
                Console.WriteLine("需要读取数据长度：" + lenth);
                return Receive(lenth);

            }
            catch (Exception e_)
            {
                Disconnect();
                throw e_;
            }
        }

        public int ReadHeadLenth()
        {
            var headBuf = new byte[4];
            _Socket.Receive(headBuf, 4, SocketFlags.None);
            Array.Reverse(headBuf);
            return BitConverter.ToInt32(headBuf, 0);
        }

        private byte[] GetRecviceData(byte[] source)
        {
            var head = source.Skip(0).Take(4).ToArray();
            Array.Reverse(head);
            var curPrefix = BitConverter.ToInt32(head, 0);
            return source.Skip(4).Take(curPrefix).ToArray();
        }

        private int ConvertBigEnc(byte[] value)
        {
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(value);
            }
            return BitConverter.ToInt32(value, 0);
        }

        private byte[] HeadByte(int lenth)
        {
            //  var t = System.Net.IPAddress.NetworkToHostOrder(lenth);//.HostToNetworkOrder
            var data = System.BitConverter.GetBytes(lenth);
            Array.Reverse(data);
            return data;
        }
        public bool Send(string value)
        {
            return Send(value, Encoding.UTF8);
        }

        public bool Send(string value, Encoding coding)
        {
            return Send(coding.GetBytes(value));
        }

        public bool Send(byte[] data)
        {
            return Send(data, 0, data.Length);
        }

        public bool Send(byte[] data, int offset, int count)
        {
            Connect();
            if (IsConnected)
            {
                try
                {
                    var head = HeadByte(count);
                    var send = _Socket.Send(head);
                    while (count > 0)
                    {
                        int sends = _Socket.Send(data, offset, count, SocketFlags.None);
                        count -= sends;
                        offset += sends;
                    }
                    return true;
                }
                catch (Exception e_)
                {
                    Disconnect();
                    LastException = e_;
                    return false;
                }
            }
            return false;

        }

        public bool Send(ArraySegment<byte> data)
        {
            return Send(data.Array, data.Offset, data.Count);

        }


        internal static Packet Send(byte[] datas, TcpClient client)//T Send<T>(MessageDescriptor dsc, byte[] datas, TcpClient client) where T : class, Google.ProtocolBuffers.IMessage
        {

            try
            {

                if (!client.Send(datas, 0, datas.Length))
                {
                    throw new Exception(string.Format("{0} client disconnect!", client.HostAddress));
                }
            }
            catch (Exception e_)
            {
                client.LastException = e_;

                throw new Exception(string.Format("send to {0} error!", client.HostAddress), e_);
            }

            #region 注释代码
            //MemoryStream ms = new MemoryStream();
            //try
            //{
            //    while (true)
            //    {
            //        TcpReceiveArgs res = client.Receive();
            //        if (res.Count == 0)
            //        {
            //            break;
            //        }
            //        ms.Write(res.Data, res.Offset, res.Count);

            //        if (res.Count < res.Client.BufferLength)
            //        {
            //            break;
            //        }
            //    }
            //    var data = new byte[ms.Length];
            //    ms.Read(data, 0, data.Length);
            //    var head = data.Skip(0).Take(4).ToArray();
            //    Array.Reverse(head);
            //    var curPrefix = BitConverter.ToInt32(head, 0);
            //    return Packet.ParseFrom(data.Skip(curPrefix).ToArray());
            //    // return Google.ProtocolBuffers.DynamicMessage.ParseFrom(dsc, ms) as T;//( new T().DescriptorForType,)
            //    return Packet.ParseFrom(ms);
            //    // return Serializer.Deserialize<T>(ms);
            //    throw new NotImplementedException();
            //}
            //catch (Exception e_)
            //{

            //    client.LastException = e_;
            //    throw new Exception(string.Format("receive {0} error!", client.HostAddress), e_);
            //}
            //finally
            //{
            //    ms.Close();
            //    ms.Dispose();
            //} 
            #endregion


            var data = client.Receive();
            return Packet.ParseFrom(data.ToArray());

        }

    }
    public static class ExtensionClass
    {
        public static byte[] RemoveEmptyByte(this byte[] by, int length)
        {
            byte[] returnByte = new byte[length];

            for (int i = 0; i < length; i++)
            {
                returnByte[i] = by[i];
            }
            return returnByte;

        }
    }
}
