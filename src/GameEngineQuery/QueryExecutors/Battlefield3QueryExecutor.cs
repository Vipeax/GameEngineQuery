using System.Net;
using GameEngineQuery.PacketModels;

namespace GameEngineQuery.QueryExecutors
{
    public class Battlefield3QueryExecutor : QueryExecutor<A2SInfo>
    {
        public Battlefield3QueryExecutor(string ipAddress, ushort port) : base(ipAddress, port)
        {
        }

        public Battlefield3QueryExecutor(EndPoint endPoint) : base(endPoint)
        {
        }

        public override A2SInfo GetServerInfo()
        {
            return new A2SInfo();
        }

        protected override byte[] HandleGameEngineQuery(byte[] request)
        {
            return request;
        }
    }
}