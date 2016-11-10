using System;

namespace TMXmlRpcLib
{
	// Token: 0x0200000D RID: 13
	public class GbxCallbackEventArgs : EventArgs
	{
		// Token: 0x0600003F RID: 63 RVA: 0x00002AEC File Offset: 0x00001AEC
		public GbxCallbackEventArgs(GbxCall response)
		{
			this.Response = response;
		}

		// Token: 0x04000019 RID: 25
		public readonly GbxCall Response;
	}
}
