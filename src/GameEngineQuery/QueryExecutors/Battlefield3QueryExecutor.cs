using System.Net;
using GameEngineQuery.PacketModels;

namespace GameEngineQuery.QueryExecutors
{
    public class Battlefield3QueryExecutor : BattlefieldQueryExecutor<Battlefield3ServerInfo, BattlefieldPlayerInfo>
    {
        public Battlefield3QueryExecutor(string ipAddress, ushort port) : base(ipAddress, port)
        {
        }

        public Battlefield3QueryExecutor(EndPoint endPoint) : base(endPoint)
        {
        }
    }
}