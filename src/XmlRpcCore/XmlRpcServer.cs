//using System;
//using System.Collections;
//using System.Net;
//using System.Net.Sockets;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace TMXmlRpcLib
//{
//	// Token: 0x02000005 RID: 5
//	public class XmlRpcServer
//	{
//        private bool running;

//		// Token: 0x0600000D RID: 13 RVA: 0x00002050 File Offset: 0x00001050
//		public XmlRpcServer(string in_address, int in_port)
//		{
//			this.m_address = in_address;
//			this.m_port = in_port;
//			this.m_connections = new ArrayList();
//		}

//		// Token: 0x0600000E RID: 14 RVA: 0x0000207C File Offset: 0x0000107C
//		public void Start()
//		{
//            running = true;
//			this.th_listen = new Task(this.Listen);
//			this.th_listen.Start();
//		}

//		// Token: 0x0600000F RID: 15 RVA: 0x000020A0 File Offset: 0x000010A0
//		public void Stop()
//		{
//            running = false;
//		}

//		// Token: 0x06000010 RID: 16 RVA: 0x000020B0 File Offset: 0x000010B0
//		public bool SendCallback(string in_method_name, object[] in_params)
//		{
//			bool result;
//			try
//			{
//				foreach (XmlRpcServer_Client xmlRpcServer_Client in this.m_connections)
//				{
//					if (!xmlRpcServer_Client.SendCallback(in_method_name, in_params))
//					{
//						result = false;
//						return result;
//					}
//				}
//				result = true;
//			}
//			catch
//			{
//				result = false;
//			}
//			return result;
//		}

//		// Token: 0x06000011 RID: 17 RVA: 0x00002128 File Offset: 0x00001128
//		private void Listen()
//		{
//			this.m_connections = new ArrayList();
//			this.m_listener = new TcpListener(IPAddress.Parse(this.m_address), this.m_port);
//			this.m_listener.Start();
//			while (running)
//			{
//				Socket socket = this.m_listener.AcceptSocketAsync().Result;
//				byte[] bytes = Encoding.UTF8.GetBytes("GBXRemote 2");
//				byte[] bytes2 = BitConverter.GetBytes(bytes.Length);
//				socket.Send(bytes2);
//				socket.Send(bytes);
//				XmlRpcServer_Client xmlRpcServer_Client = new XmlRpcServer_Client(++this.m_ccount, socket, this);
//				this.m_connections.Add(xmlRpcServer_Client);
//				if (this.onClientConnects != null)
//				{
//					this.onClientConnects.BeginInvoke(xmlRpcServer_Client, null, null);
//				}
//			}
//		}

//		// Token: 0x06000012 RID: 18 RVA: 0x000021DC File Offset: 0x000011DC
//		public void RegisterMethod(string in_method_name, dCallMethod in_delegate)
//		{
//			lock (this)
//			{
//				this.m_reg_methods.Add(in_method_name, in_delegate);
//			}
//		}

//		// Token: 0x06000013 RID: 19 RVA: 0x00002218 File Offset: 0x00001218
//		public void ClientDisconnect(XmlRpcServer_Client out_client)
//		{
//			out_client.Dispose();
//			this.m_connections.Remove(out_client);
//			if (this.onClientDisconnects != null)
//			{
//				this.onClientDisconnects.BeginInvoke(out_client, null, null);
//			}
//		}

//		// Token: 0x17000001 RID: 1
//		// (get) Token: 0x06000014 RID: 20 RVA: 0x00002243 File Offset: 0x00001243
//		public int Connections
//		{
//			get
//			{
//				return this.m_connections.Count;
//			}
//		}

//		// Token: 0x04000001 RID: 1
//		private const string PROTOCOL = "GBXRemote 2";

//		// Token: 0x04000002 RID: 2
//		private string m_address;

//		// Token: 0x04000003 RID: 3
//		private int m_port;

//		// Token: 0x04000004 RID: 4
//		private int m_ccount;

//		// Token: 0x04000005 RID: 5
//		private TcpListener m_listener;

//		// Token: 0x04000006 RID: 6
//		private ArrayList m_connections;

//		// Token: 0x04000007 RID: 7
//		private Task th_listen;

//		// Token: 0x04000008 RID: 8
//		public Hashtable m_reg_methods = new Hashtable();

//		// Token: 0x04000009 RID: 9
//		public dClientConnects onClientConnects;

//		// Token: 0x0400000A RID: 10
//		public dClientDisconnects onClientDisconnects;
//	}
//}
