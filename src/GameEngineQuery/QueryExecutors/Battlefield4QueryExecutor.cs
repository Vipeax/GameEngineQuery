using System.Net;
using GameEngineQuery.PacketModels;

namespace GameEngineQuery.QueryExecutors
{
    public class Battlefield4QueryExecutor : BattlefieldQueryExecutor<Battlefield4ServerInfo>
    {
        public Battlefield4QueryExecutor(string ipAddress, ushort port) : base(ipAddress, port)
        {
        }

        public Battlefield4QueryExecutor(EndPoint endPoint) : base(endPoint)
        {
        }
    }
}