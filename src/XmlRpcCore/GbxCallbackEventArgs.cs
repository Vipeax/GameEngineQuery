using System;

namespace XmlRpcCore
{
	public class GbxCallbackEventArgs : EventArgs
	{
		public GbxCallbackEventArgs(GbxCall response)
		{
			this.Response = response;
		}

		public readonly GbxCall Response;
	}
}
