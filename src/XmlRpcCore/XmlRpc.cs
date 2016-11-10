using System;
using System.Net.Sockets;
using System.Text;

namespace XmlRpcCore
{
	public class XmlRpc
	{
		private static byte[] ReceiveRpc(Socket socket, int length)
		{
			byte[] array = new byte[length];
			int num = 0;
			while (length > 0)
			{
				int num2 = Math.Min(length, 1024);
				byte[] array2 = new byte[num2];
				int num3 = socket.Receive(array2, 0, array2.Length, SocketFlags.None);
				Array.Copy(array2, 0, array, num, array2.Length);
				length -= num3;
				num += num3;
			}
			return array;
		}

		public static int SendCall(Socket socket, GbxCall call)
		{
			if (socket.Connected)
			{
				lock (socket)
				{
					try
					{
						byte[] bytes = Encoding.UTF8.GetBytes(call.Xml);
						byte[] bytes2 = BitConverter.GetBytes(bytes.Length);
						byte[] bytes3 = BitConverter.GetBytes(call.Handle);
						byte[] array = new byte[bytes2.Length + bytes3.Length + bytes.Length];
						Array.Copy(bytes2, 0, array, 0, bytes2.Length);
						Array.Copy(bytes3, 0, array, 4, bytes3.Length);
						Array.Copy(bytes, 0, array, 8, bytes.Length);
						socket.Send(array);
						int result = call.Handle;
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

		public static GbxCall ReceiveCall(Socket socket, byte[] header)
		{
			if (socket.Connected)
			{
				lock (socket)
				{
					byte[] array = new byte[4];
					byte[] array2 = new byte[4];
					if (header == null)
					{
						socket.Receive(array);
						socket.Receive(array2);
					}
					else
					{
						Array.Copy(header, 0, array, 0, 4);
						Array.Copy(header, 4, array2, 0, 4);
					}
					int length = BitConverter.ToInt32(array, 0);
					int handle = BitConverter.ToInt32(array2, 0);
					byte[] data = XmlRpc.ReceiveRpc(socket, length);
					return new GbxCall(handle, data);
				}
			}
			throw new NotConnectedException();
		}
	}
}
