using GameEngineQuery.Constants.Enumerations;
using GameEngineQuery.Requests;

namespace GameEngineQuery.Factories
{
    internal class RequestFactory : IRequestFactory
    {
        public byte[] CreateRequestPacket(RequestPacketType packetType)
        {
            switch (packetType)
            {
                case RequestPacketType.A2SInfo:
                    return new A2SInfoRequestPacket().CreateRequest();
                default:
                    return new byte[0];
            }
        }
    }
}