using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace XmlRpcCore
{
    public class XmlRpcClient : IDisposable
    {
        public event GbxCallbackHandler EventGbxCallback;
        public event OnDisconnectHandler EventOnDisconnectCallback;
        
        private Socket tcpSocket;
        private int requests;
        private readonly byte[] buffer;
        private IAsyncResult asyncResult;
        private bool running;

        private readonly AutoResetEvent callRead;
        private readonly Hashtable responses;
        private readonly Hashtable callbackList;

        public XmlRpcClient(EndPoint endPoint)
        {
            this.callRead = new AutoResetEvent(false);
            this.responses = new Hashtable();
            this.callbackList = new Hashtable();
            this.running = true;

            if (!this.Connect(endPoint))
            {
                throw new Exception("Could not connect onto " + endPoint);
            }
            if (!this.Handshake())
            {
                throw new Exception("Handshake failed, check the server's protocol version!");
            }
            this.buffer = new byte[8];
            
            Task.Run(() =>
            {
                while (running)
                {
                    try
                    {
                        this.tcpSocket.Receive(this.buffer, 0, this.buffer.Length, SocketFlags.None);
                        GbxCall gbxCall = XmlRpc.ReceiveCall(this.tcpSocket, this.buffer);
                        if (gbxCall.MessageType == MessageTypes.Callback)
                        {
                            GbxCallbackEventArgs e = new GbxCallbackEventArgs(gbxCall);
                            this.OnGbxCallback(e);
                        }
                        else
                        {
                            lock (this)
                            {
                                this.responses.Add(gbxCall.Handle, gbxCall);
                            }
                            if (this.callbackList[gbxCall.Handle] != null)
                            {
                                ((GbxCallCallbackHandler)this.callbackList[gbxCall.Handle]).BeginInvoke(gbxCall, null, null);
                                this.callbackList.Remove(gbxCall.Handle);
                            }
                        }
                    }
                    catch
                    {
                        this.tcpSocket.Dispose();
                        this.OnDisconnectCallback();
                    }
                    finally
                    {
                        this.callRead.Set();
                    }
                }

            });
        }


        private void OnDisconnectCallback()
        {
            this.EventOnDisconnectCallback?.Invoke(this);
        }

        private void OnGbxCallback(GbxCallbackEventArgs e)
        {
            this.EventGbxCallback?.Invoke(this, e);
        }

        public void Dispose()
        {
            this.running = false;
            this.tcpSocket.Dispose();
            this.EventGbxCallback = null;
            this.EventOnDisconnectCallback = null;
        }

        public bool EnableCallbacks(bool inState)
        {
            GbxCall gbxCall = new GbxCall("EnableCallbacks", new object[]
            {
                inState
            }) {Handle = --this.requests};

            return XmlRpc.SendCall(this.tcpSocket, gbxCall) != 0;
        }

        public GbxCall Request(string inMethodName, object[] inParams)
        {
            this.callRead.Reset();
            GbxCall gbxCall = new GbxCall(inMethodName, inParams) {Handle = --this.requests};
            int num = XmlRpc.SendCall(this.tcpSocket, gbxCall);
            do
            {
                this.callRead.WaitOne();
            }
            while (this.responses[num] == null && this.tcpSocket.Connected);
            if (!this.tcpSocket.Connected)
            {
                throw new NotConnectedException();
            }
            return this.GetResponse(num);
        }

        public int AsyncRequest(string inMethodName, object[] inParams, GbxCallCallbackHandler callbackHandler)
        {
            GbxCall gbxCall = new GbxCall(inMethodName, inParams) {Handle = --this.requests};
            int num = XmlRpc.SendCall(this.tcpSocket, gbxCall);
            int result;
            lock (this)
            {
                if (num != 0)
                {
                    if (callbackHandler != null)
                    {
                        this.callbackList.Add(num, callbackHandler);
                    }
                    result = num;
                }
                else
                {
                    result = 0;
                }
            }
            return result;
        }

        public GbxCall GetResponse(int inHandle)
        {
            return (GbxCall)this.responses[inHandle];
        }

        public bool Connect(EndPoint endPoint)
        {
            try
            {
                this.tcpSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                this.tcpSocket.Connect(endPoint);
            }
            catch
            {
                throw new NotConnectedException();
            }

            return true;
        }

        public bool Handshake()
        {
            if (this.tcpSocket.Connected)
            {
                byte[] array = new byte[4];
                this.tcpSocket.Receive(array);
                int num = BitConverter.ToInt32(array, 0);
                byte[] array2 = new byte[num];
                this.tcpSocket.Receive(array2);
                string @string = Encoding.UTF8.GetString(array2);
                return @string.Equals("GBXRemote 2", StringComparison.OrdinalIgnoreCase);
            }

            throw new NotConnectedException();
        }
    }
}
