using System;
using System.Net.Sockets;

namespace TMXmlRpcLib
{
	//// Token: 0x02000006 RID: 6
	//public class XmlRpcServer_Client
	//{
	//	// Token: 0x06000015 RID: 21 RVA: 0x00002250 File Offset: 0x00001250
	//	public XmlRpcServer_Client(int in_id, Socket in_socket, XmlRpcServer in_parent)
	//	{
	//		this.m_id = in_id;
	//		this.m_socket = in_socket;
	//		this.m_parent_server = in_parent;
	//		this.AsyncReceiveSetup();
	//	}

	//	// Token: 0x06000016 RID: 22 RVA: 0x00002274 File Offset: 0x00001274
	//	public bool SendCallback(string in_method_name, object[] in_params)
	//	{
	//		bool result;
	//		try
	//		{
	//			GbxCall gbxCall = new GbxCall(in_method_name, in_params);
	//			gbxCall.Handle = 2;
	//			XmlRpc.SendCall(this.m_socket, gbxCall);
	//			result = true;
	//		}
	//		catch
	//		{
	//			result = false;
	//		}
	//		return result;
	//	}

	//	// Token: 0x06000017 RID: 23 RVA: 0x000022B8 File Offset: 0x000012B8
	//	private void AsyncReceiveSetup()
	//	{
	//		this.m_buffer = new byte[8];
	//		this.m_socket.BeginReceive(this.m_buffer, 0, 8, SocketFlags.None, new AsyncCallback(this.OnDataArrive), null);
	//	}

	//	// Token: 0x06000018 RID: 24 RVA: 0x000022E8 File Offset: 0x000012E8
	//	private void OnDataArrive(IAsyncResult Iar)
	//	{
	//		try
	//		{
	//			this.m_socket.EndReceive(Iar);
	//			GbxCall gbxCall = XmlRpc.ReceiveCall(this.m_socket, this.m_buffer);
	//			this.AsyncReceiveSetup();
	//			if (this.m_parent_server.m_reg_methods[gbxCall.MethodName] != null)
	//			{
	//				this.test = (dCallMethod)this.m_parent_server.m_reg_methods[gbxCall.MethodName];
	//				this.test.BeginInvoke(gbxCall, this, new AsyncCallback(this.SendResponse), null);
	//			}
	//		}
	//		catch
	//		{
	//			this.OnDisconnect();
	//		}
	//	}

	//	// Token: 0x06000019 RID: 25 RVA: 0x0000238C File Offset: 0x0000138C
	//	private void SendResponse(IAsyncResult Iar)
	//	{
	//		GbxCall in_call = this.test.EndInvoke(Iar);
	//		XmlRpc.SendCall(this.m_socket, in_call);
	//	}

	//	// Token: 0x0600001A RID: 26 RVA: 0x000023B3 File Offset: 0x000013B3
	//	public void Dispose()
	//	{
	//		this.m_socket.Close();
	//	}

	//	// Token: 0x0600001B RID: 27 RVA: 0x000023C0 File Offset: 0x000013C0
	//	private void OnDisconnect()
	//	{
	//		this.m_parent_server.ClientDisconnect(this);
	//	}

	//	// Token: 0x17000002 RID: 2
	//	// (get) Token: 0x0600001C RID: 28 RVA: 0x000023CE File Offset: 0x000013CE
	//	public Socket Sock
	//	{
	//		get
	//		{
	//			return this.m_socket;
	//		}
	//	}

	//	// Token: 0x17000003 RID: 3
	//	// (get) Token: 0x0600001D RID: 29 RVA: 0x000023D6 File Offset: 0x000013D6
	//	public int Id
	//	{
	//		get
	//		{
	//			return this.m_id;
	//		}
	//	}

	//	// Token: 0x0400000B RID: 11
	//	private XmlRpcServer m_parent_server;

	//	// Token: 0x0400000C RID: 12
	//	private int m_id;

	//	// Token: 0x0400000D RID: 13
	//	private Socket m_socket;

	//	// Token: 0x0400000E RID: 14
	//	private byte[] m_buffer;

	//	// Token: 0x0400000F RID: 15
	//	private dCallMethod test;
	//}
}
