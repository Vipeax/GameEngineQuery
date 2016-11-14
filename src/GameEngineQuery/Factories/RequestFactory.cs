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
                case RequestPacketType.A2SPlayer:
                    return new A2SPlayerRequestPacket().CreateRequest();
                case RequestPacketType.BattlefieldServerInfo:
                    return new BattlefieldServerInfoPacket().CreateRequest();
                case RequestPacketType.BattlefieldPlayerInfo:
                    return new BattlefieldPlayerInfoPacket().CreateRequest();
                default:
                    return new byte[0];
            }
        }
    }
}