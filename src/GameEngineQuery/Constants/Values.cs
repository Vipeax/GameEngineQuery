using GameEngineQuery.Extensions;

namespace GameEngineQuery.Constants
{
    internal static class Values
    {
        internal static class ValveConstants
        {
            internal const ushort HeaderLength = 4;
            internal const ushort PacketSize = 16384;

            internal static class A2SInfo
            {
                // https://developer.valvesoftware.com/wiki/Server_queries#Request_Format
                // 54 53 6F 75 72 63 65 20 45 6E 67 69 6E 65 20 51 75 65 72 79
                internal const string RequestPayload = "TSource Engine Query";
                internal const ushort RequestPacketSize = 25;
            }
        }

        internal static class BattlefieldConstants
        {
            internal static class ServerInfo
            {
                internal static readonly byte[] ServerInfoPacket =
                {
                    0x0, 0x0, 0x0, 0x0, 0x1B, 0x0, 0x0, 0x0, 0x01, 0x0, 0x0, 0x0, 0x0A, 0x0, 0x0, 0x0,
                    (byte) 's', (byte) 'e', (byte) 'r', (byte) 'v', (byte) 'e', (byte) 'r',
                    (byte) 'I', (byte) 'n', (byte) 'f', (byte) 'o', 0x0
                };

                internal const ushort ResponsePacketSize = 512;
            }

            internal static class PlayerInfo
            {
                internal static readonly byte[] PlayerInfoPacket =
                {
                    0x0, 0x0, 0x0, 0x0, 0x24, 0x0, 0x0, 0x0, 0x02, 0x0, 0x0, 0x0, 0x0B, 0x0, 0x0, 0x0,
                    (byte) 'l', (byte) 'i', (byte) 's', (byte) 't',
                    (byte) 'P', (byte) 'l', (byte) 'a', (byte) 'y', (byte) 'e', (byte) 'r', (byte) 's',
                    0x0, 0x03, 0x0, 0x0, 0x0, (byte) 'a', (byte) 'l', (byte) 'l', 0x0
                };

                internal const ushort ResponsePacketSize = 16384;
            }
        }

        internal static class Quake3Constants
        {
            internal const ushort HeaderLength = 4;
            internal const ushort PacketSize = 16384;

            internal static class GetStatus
            {
                internal static readonly byte[] GetStatusPacket =
                {
                    0xFF, 0xFF, 0XFF, 0xFF,
                    (byte) 'g', (byte) 'e', (byte) 't',
                    (byte) 's', (byte) 't', (byte) 'a', (byte) 't', (byte) 'u', (byte) 's'
                };
            }
        }

        internal static class ProjectConstants
        {
            internal const ushort SendTimeout = 3000;
            internal const ushort ReceiveTimeout = 3000;
        }
    }
}
