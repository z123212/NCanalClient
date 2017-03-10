using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NCanalClient.TcpHandler
{
    public class TcpSession
    {
        private static ManualResetEvent ConnectDone = new ManualResetEvent(false);

        private static ManualResetEvent SendDone = new ManualResetEvent(false);

        private static ManualResetEvent ReceiveDone = new ManualResetEvent(false);


        public Socket _Socket = null;

        public const int PREFIX_SIZE = 4;

        public string Host { get; private set; }

        public int Port { get; private set; } = 11111;
        public int TimeOut { get; private set; }
        public bool IsConnected { get; private set; }

        public TcpSession(string host, int port = 11111, int timeOut = 6000)
        {
            Host = host;
            Port = port;
            TimeOut = timeOut;
        }

        public void StartConnection()
        {
            Connect();
            ConnectDone.WaitOne();
            Recevie(null);

        }
        /// <summary>
        /// 开始连接
        /// </summary>
        private void Connect()
        {
            var ips = Dns.GetHostAddresses(Host);
            if (ips.Length == 0)
            {
                throw new Exception("获取address出错");
            }
            try
            {
                EndPoint endPoint = new IPEndPoint(ips[0], Port);
                _Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _Socket.ReceiveTimeout = TimeOut;
                _Socket.SendTimeout = TimeOut;
                _Socket.BeginConnect(endPoint, new AsyncCallback(ConnectCallBack), _Socket);
                //_Socket.Connect(ips[0], Port);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ConnectCallBack(IAsyncResult ar)
        {
            Socket socket = ar.AsyncState as Socket;
            socket.EndConnect(ar);
            ConnectDone.Set();
        }

        public void Recevie(Action<byte[]> callBack = null)
        {
            try
            {
                ReceiveDone.Reset();
                StateObject so = new StateObject();
                so.WorkSocket = _Socket;
                //第一次读取数据的总长度
                var result = _Socket.BeginReceive(so.Buffer, 0, PREFIX_SIZE, 0,
                       ar => PreRecevied(ar, callBack)
                       , so);
                result.AsyncWaitHandle.WaitOne();
                ReceiveDone.WaitOne();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void PreRecevied(IAsyncResult ar, Action<byte[]> callBack = null)
        {
            StateObject so = (StateObject)ar.AsyncState;
            Socket client = so.WorkSocket;
            int readSize = client.EndReceive(ar);//结束读取，返回已读取的缓冲区里的字节数组长度 
            int curPrefix = 0;
            using (var reader = new MemoryStream())
            {
                reader.Write(so.Buffer, 0, readSize);
                reader.Position = 0;
                byte[] presixBytes = new byte[PREFIX_SIZE];
                reader.Read(presixBytes, 0, PREFIX_SIZE);
                Array.Reverse(presixBytes);
                curPrefix = BitConverter.ToInt32(presixBytes, 0);
                if (curPrefix > 0)
                {
                    var r = client.BeginReceive(so.Buffer, 0, StateObject.BufferSize, 0,
                               //new AsyncCallback(ReceivedCallBack, callBack)
                               a => AfterPreReceiveCallBack(a, reader)
                               , so);
                    r.AsyncWaitHandle.WaitOne();
                    Console.WriteLine("等待完毕");
                    //if (reader.Length - PREFIX_SIZE < totalLentg)
                    //{
                    //    goto Re;
                    //}
                    //reader.Position = PREFIX_SIZE;
                    //var bytes = new byte[totalLentg];
                    //reader.Read(bytes, 0, bytes.Length);
                    //callBack?.Invoke(bytes);
                }
                else
                {
                    ReceiveDone.Set();
                }
            }
        }

        private void AfterPreReceiveCallBack(IAsyncResult ar, MemoryStream ms, Action<byte[]> callBack = null)
        {
            StateObject obj = ar.AsyncState as StateObject;
            var currentClient = obj.WorkSocket;
            var size = currentClient.EndReceive(ar);
            
            ms.Write(obj.Buffer, 0, size);

            if (ms.Length - PREFIX_SIZE < size)
            {
                Console.WriteLine($"未读取完毕，当前参数值len:{ms.Length},头:{PREFIX_SIZE},当前size:{size},继续下一次读取");
                var r = currentClient.BeginReceive(obj.Buffer, 0, StateObject.BufferSize, SocketFlags.None, a => AfterPreReceiveCallBack(a, ms, callBack), obj);
                r.AsyncWaitHandle.WaitOne();
            }
            else
            {

                ms.Position = PREFIX_SIZE;
                var bytes = new byte[ms.Length - PREFIX_SIZE];
                Console.WriteLine($"读取数据完毕。总共读取长度{ms.Length}，返回调用数据{bytes.Length}");
                ms.Read(bytes, 0, bytes.Length);
                callBack?.Invoke(bytes);
                ReceiveDone.Set();
            }
        }


        public void ReceivedCallBack(IAsyncResult ar, Action<MemoryStream> callBack = null)
        {
            try
            {
                StateObject so = (StateObject)ar.AsyncState;
                Socket client = so.WorkSocket;
                int readSize = client.EndReceive(ar);//结束读取，返回已读取的缓冲区里的字节数组长度
                bool isPresix = true;
                int curPrefix = 0;
                using (var receiveData = new MemoryStream())
                {
                    receiveData.Write(so.Buffer, 0, readSize);
                    // receiveData.Position = 0;
                    while (true)
                    {
                        //读取前置长度，只读取一次
                        if ((int)receiveData.Length >= PREFIX_SIZE && isPresix)
                        {
                            byte[] presixBytes = new byte[PREFIX_SIZE];
                            receiveData.Read(presixBytes, 0, PREFIX_SIZE);
                            Array.Reverse(presixBytes);
                            curPrefix = BitConverter.ToInt32(presixBytes, 0);
                            isPresix = false;
                        }
                        else
                        {

                        }

                        if (receiveData.Length - PREFIX_SIZE < curPrefix)
                        {
                            //如果数据没有读取完毕，调整Position到最后，接着读取。
                            //  receiveData.Position = receiveData.Length;
                            continue;
                        }
                        else
                        {
                            if (callBack != null)
                            {
                                callBack(receiveData);
                            }
                            // receiveData.Read(datas, 0, datas.Length);
                            ////有压缩的话需要先解压，然后在操作。
                            //byte[] finallyBytes = decompress(datas);
                            //String str = Encoding.UTF8.GetString(finallyBytes);
                            break;
                        }
                    }

                }                                   //将每次读取的数据，写入内存流里


            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                ReceiveDone.Set();
            }
        }

        public void Send(byte[] data)
        {
            if (_Socket == null)
            {
                Connect();
            }
            Send(_Socket, data);
            SendDone.WaitOne();
        }

        public static void Send(Socket client, byte[] data)
        {
            try
            {
                client.BeginSend(data, 0, data.Length, 0, ar => SendCallBack(ar), client);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static void SendCallBack(IAsyncResult ar)
        {
            try
            {
                var socket = ar.AsyncState as Socket;
                if (ar.IsCompleted)
                {
                    var p = socket.EndSend(ar);
                    Console.WriteLine($"发送数据结束，结束点:{p}");
                    SendDone.Set();
                }
            }
            catch (Exception)
            {
                SendDone.Set();
                throw;
            }
        }

        // State object for receiving data from remote device.
        public class StateObject
        {
            // Client socket.
            public Socket WorkSocket = null;
            // Size of receive buffer.
            public const int BufferSize = 1024;
            // Receive buffer.
            public byte[] Buffer = new byte[BufferSize];
        }

        internal void Disconnect()
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
            catch (Exception)
            {
                throw;
            }
        }
    }
}
