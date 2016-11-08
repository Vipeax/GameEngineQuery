using GameEngineQuery.Constants.Enumerations;

namespace GameEngineQuery.Factories
{
    public interface IRequestFactory
    {
        byte[] CreateRequestPacket(RequestPacketType packetType);
    }
}
