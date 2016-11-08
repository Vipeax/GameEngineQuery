using System.Net;
using GameEngineQuery.PacketModels;

namespace GameEngineQuery.QueryExecutors
{
    public class Battlefield3QueryExecutor : QueryExecutor
    {
        public Battlefield3QueryExecutor(string ipAddress, ushort port) : base(ipAddress, port)
        {
        }

        public Battlefield3QueryExecutor(EndPoint endPoint) : base(endPoint)
        {
        }

        public override ServerInfo GetServerInfo()
        {
            return new A2SInfo();
        }

        protected override byte[] HandleGameEngineQuery(byte[] request)
        {
            return request;
        }
    }
}