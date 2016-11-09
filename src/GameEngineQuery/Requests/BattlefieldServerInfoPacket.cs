using GameEngineQuery.Constants;

namespace GameEngineQuery.Requests
{
    internal class BattlefieldServerInfoPacket : IRequestPacket
    {
        public byte[] CreateRequest()
        {
            return Values.BattlefieldConstants.ServerInfo.ServerInfoPacket;
        }
    }
}