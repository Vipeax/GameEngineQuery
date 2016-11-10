using System;
using System.Net.Sockets;
using System.Text;

namespace TMXmlRpcLib
{
	// Token: 0x02000007 RID: 7
	public class XmlRpc
	{
		// Token: 0x0600001E RID: 30 RVA: 0x000023E0 File Offset: 0x000013E0
		private static bool SendRpc(Socket in_socket, byte[] in_data)
		{
			int num = 0;
			int i = in_data.Length;
			bool result;
			try
			{
				while (i > 0)
				{
					int num2 = in_socket.Send(in_data, num, i, SocketFlags.None);
					i -= num2;
					num += num2;
				}
				result = true;
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002428 File Offset: 0x00001428
		private static byte[] ReceiveRpc(Socket in_socket, int in_length)
		{
			byte[] array = new byte[in_length];
			int num = 0;
			while (in_length > 0)
			{
				int num2 = Math.Min(in_length, 1024);
				byte[] array2 = new byte[num2];
				int num3 = in_socket.Receive(array2, 0, array2.Length, SocketFlags.None);
				Array.Copy(array2, 0, array, num, array2.Length);
				in_length -= num3;
				num += num3;
			}
			return array;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002480 File Offset: 0x00001480
		public static int SendCall(Socket in_socket, GbxCall in_call)
		{
			if (in_socket.Connected)
			{
				lock (in_socket)
				{
					try
					{
						byte[] bytes = Encoding.UTF8.GetBytes(in_call.Xml);
						byte[] bytes2 = BitConverter.GetBytes(bytes.Length);
						byte[] bytes3 = BitConverter.GetBytes(in_call.Handle);
						byte[] array = new byte[bytes2.Length + bytes3.Length + bytes.Length];
						Array.Copy(bytes2, 0, array, 0, bytes2.Length);
						Array.Copy(bytes3, 0, array, 4, bytes3.Length);
						Array.Copy(bytes, 0, array, 8, bytes.Length);
						in_socket.Send(array);
						int result = in_call.Handle;
						return result;
					}
					catch
					{
						int result = 0;
						return result;
					}
				}
			}
			throw new NotConnectedException();
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002540 File Offset: 0x00001540
		public static GbxCall ReceiveCall(Socket in_socket, byte[] inHeader)
		{
			if (in_socket.Connected)
			{
				lock (in_socket)
				{
					byte[] array = new byte[4];
					byte[] array2 = new byte[4];
					if (inHeader == null)
					{
						in_socket.Receive(array);
						in_socket.Receive(array2);
					}
					else
					{
						Array.Copy(inHeader, 0, array, 0, 4);
						Array.Copy(inHeader, 4, array2, 0, 4);
					}
					int in_length = BitConverter.ToInt32(array, 0);
					int in_handle = BitConverter.ToInt32(array2, 0);
					byte[] in_data = XmlRpc.ReceiveRpc(in_socket, in_length);
					return new GbxCall(in_handle, in_data);
				}
			}
			throw new NotConnectedException();
		}
	}
}
