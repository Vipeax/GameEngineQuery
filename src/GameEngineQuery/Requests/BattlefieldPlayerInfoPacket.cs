using GameEngineQuery.Constants;

namespace GameEngineQuery.Requests
{
    internal class BattlefieldPlayerInfoPacket : IRequestPacket
    {
        public byte[] CreateRequest()
        {
            return Values.BattlefieldConstants.PlayerInfo.PlayerInfoPacket;
        }
    }
}