namespace GameEngineQuery.Constants
{
    internal static class Values
    {
        internal static class ValveConstants
        {
            internal const ushort HeaderLength = 4;
            internal const ushort PacketSize = 1400;

            internal static class A2SInfo
            {
                // https://developer.valvesoftware.com/wiki/Server_queries#Request_Format
                // 54 53 6F 75 72 63 65 20 45 6E 67 69 6E 65 20 51 75 65 72 79
                internal const string RequestPayload = "TSource Engine Query";
                internal const ushort RequestPacketSize = 25;
            }
        }

        internal static class ProjectConstants
        {
            internal const ushort SendTimeout = 3000;
            internal const ushort ReceiveTimeout = 3000;
        }
    }
}
