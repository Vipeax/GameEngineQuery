using GameEngineQuery.Constants;

namespace GameEngineQuery.Requests
{
    internal class Quake3GetStatusPacket : IRequestPacket
    {
        public byte[] CreateRequest()
        {
            return Values.Quake3Constants.GetStatus.GetStatusPacket;
        }
    }
}