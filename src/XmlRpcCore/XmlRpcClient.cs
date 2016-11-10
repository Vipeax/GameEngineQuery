using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TMXmlRpcLib
{
    // Token: 0x02000008 RID: 8
    public class XmlRpcClient
    {
        // Token: 0x14000001 RID: 1
        // (add) Token: 0x06000023 RID: 35 RVA: 0x000025E8 File Offset: 0x000015E8
        // (remove) Token: 0x06000024 RID: 36 RVA: 0x00002601 File Offset: 0x00001601
        public event GbxCallbackHandler EventGbxCallback;

        // Token: 0x14000002 RID: 2
        // (add) Token: 0x06000025 RID: 37 RVA: 0x0000261A File Offset: 0x0000161A
        // (remove) Token: 0x06000026 RID: 38 RVA: 0x00002633 File Offset: 0x00001633
        public event OnDisconnectHandler EventOnDisconnectCallback;

        // Token: 0x06000027 RID: 39 RVA: 0x0000264C File Offset: 0x0000164C
        private void OnDisconnectCallback()
        {
            if (this.EventOnDisconnectCallback != null)
            {
                this.EventOnDisconnectCallback(this);
            }
        }

        // Token: 0x06000028 RID: 40 RVA: 0x00002662 File Offset: 0x00001662
        private void OnGbxCallback(GbxCallbackEventArgs e)
        {
            if (this.EventGbxCallback != null)
            {
                this.EventGbxCallback(this, e);
            }
        }

        // Token: 0x06000029 RID: 41 RVA: 0x00002679 File Offset: 0x00001679
        public void Dispose()
        {
            this.tcpSocket.Dispose();
            this.EventGbxCallback = null;
            this.EventOnDisconnectCallback = null;
        }

        // Token: 0x0600002A RID: 42 RVA: 0x00002694 File Offset: 0x00001694
        public XmlRpcClient(EndPoint endPoint)
        {
            if (!this.Connect(endPoint))
            {
                throw new Exception("Could not connect onto " + endPoint);
            }
            if (!this.Handshake())
            {
                throw new Exception("Handshake failed, check the server's protocol version!");
            }
            this.m_buffer = new byte[8];



            Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        this.tcpSocket.Receive(this.m_buffer, 0, this.m_buffer.Length, SocketFlags.None);
                        GbxCall gbxCall = XmlRpc.ReceiveCall(this.tcpSocket, this.m_buffer);
                        if (gbxCall.Type == MessageTypes.Callback)
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

            //this.asyncResult = this.tcpSocket.BeginReceive(this.m_buffer, 0, this.m_buffer.Length, SocketFlags.None, new AsyncCallback(this.OnDataArrive), null);
        }

        // Token: 0x0600002B RID: 43 RVA: 0x00002740 File Offset: 0x00001740
        //private void OnDataArrive(IAsyncResult iar)
        //{
        //	try
        //	{
        //		this.tcpSocket.EndReceive(iar);
        //		GbxCall gbxCall = XmlRpc.ReceiveCall(this.tcpSocket, this.m_buffer);
        //		this.m_buffer = new byte[8];
        //		this.asyncResult = this.tcpSocket.BeginReceive(this.m_buffer, 0, this.m_buffer.Length, SocketFlags.None, new AsyncCallback(this.OnDataArrive), null);
        //		if (gbxCall.Type == MessageTypes.Callback)
        //		{
        //			GbxCallbackEventArgs e = new GbxCallbackEventArgs(gbxCall);
        //			this.OnGbxCallback(e);
        //		}
        //		else
        //		{
        //			lock (this)
        //			{
        //				this.responses.Add(gbxCall.Handle, gbxCall);
        //			}
        //			if (this.callbackList[gbxCall.Handle] != null)
        //			{
        //				((GbxCallCallbackHandler)this.callbackList[gbxCall.Handle]).BeginInvoke(gbxCall, null, null);
        //				this.callbackList.Remove(gbxCall.Handle);
        //			}
        //		}
        //	}
        //	catch
        //	{
        //		this.tcpSocket.Close();
        //		this.OnDisconnectCallback();
        //	}
        //	finally
        //	{
        //		this.callRead.Set();
        //	}
        //}

        // Token: 0x0600002C RID: 44 RVA: 0x000028AC File Offset: 0x000018AC
        public bool EnableCallbacks(bool inState)
        {
            GbxCall gbxCall = new GbxCall("EnableCallbacks", new object[]
            {
                inState
            });
            gbxCall.Handle = --this.requests;
            return XmlRpc.SendCall(this.tcpSocket, gbxCall) != 0;
        }

        // Token: 0x0600002D RID: 45 RVA: 0x00002900 File Offset: 0x00001900
        public GbxCall Request(string inMethodName, object[] inParams)
        {
            this.callRead.Reset();
            GbxCall gbxCall = new GbxCall(inMethodName, inParams);
            gbxCall.Handle = --this.requests;
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

        // Token: 0x0600002E RID: 46 RVA: 0x0000298C File Offset: 0x0000198C
        public int AsyncRequest(string inMethodName, object[] inParams, GbxCallCallbackHandler callbackHandler)
        {
            GbxCall gbxCall = new GbxCall(inMethodName, inParams);
            gbxCall.Handle = --this.requests;
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

        // Token: 0x0600002F RID: 47 RVA: 0x00002A08 File Offset: 0x00001A08
        public GbxCall GetResponse(int inHandle)
        {
            return (GbxCall)this.responses[inHandle];
        }

        // Token: 0x06000030 RID: 48 RVA: 0x00002A20 File Offset: 0x00001A20
        public bool Connect(EndPoint endPoint)
        {
            bool result;
            try
            {
                this.tcpSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                this.tcpSocket.Connect(endPoint);
                result = true;
            }
            catch
            {
                throw new NotConnectedException();
            }
            return result;
        }

        // Token: 0x06000031 RID: 49 RVA: 0x00002A78 File Offset: 0x00001A78
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
                return !(@string != "GBXRemote 2");
            }
            throw new NotConnectedException();
        }

        // Token: 0x04000010 RID: 16
        private Socket tcpSocket;

        // Token: 0x04000011 RID: 17
        private int requests;

        // Token: 0x04000012 RID: 18
        private byte[] m_buffer;

        // Token: 0x04000013 RID: 19
        private IAsyncResult asyncResult;

        // Token: 0x04000014 RID: 20
        private AutoResetEvent callRead = new AutoResetEvent(false);

        // Token: 0x04000015 RID: 21
        private Hashtable responses = new Hashtable();

        // Token: 0x04000018 RID: 24
        private Hashtable callbackList = new Hashtable();
    }
}
