using GameEngineQuery.Constants;

namespace GameEngineQuery.Requests
{
    internal class A2SPlayerRequestPacket : IRequestPacket
    {
        public byte[] CreateRequest()
        {
            var requestPacket = new byte[Values.ValveConstants.A2SInfo.RequestPacketSize];

            // HEADER
            for (ushort i = 0; i < Values.ValveConstants.HeaderLength; i++)
            {
                requestPacket[i] = 0xFF;
            }

            requestPacket[4] = 0x55;

            for (ushort i = 5; i < 9; i++)
            {
                requestPacket[i] = 0xFF;
            }

            return requestPacket;
        }
    }
}