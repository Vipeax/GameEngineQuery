using System;
using System.Text;
using GameEngineQuery.Constants;

namespace GameEngineQuery.Requests
{
    internal class A2SInfoRequestPacket : IRequestPacket
    {
        public byte[] CreateRequest()
        {
            var requestPacket = new byte[Values.ValveConstants.A2SInfo.RequestPacketSize];

            // HEADER
            for (ushort i = 0; i < Values.ValveConstants.HeaderLength; i++)
            {
                requestPacket[i] = 0xFF;
            }

            Array.Copy(Encoding.UTF8.GetBytes(Values.ValveConstants.A2SInfo.RequestPayload), 0, requestPacket,
                Values.ValveConstants.HeaderLength, Values.ValveConstants.A2SInfo.RequestPayload.Length);

            return requestPacket;
        }
    }
}