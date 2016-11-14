using System.Net;
using GameEngineQuery.PacketModels;

namespace GameEngineQuery.QueryExecutors
{
    public class BattlefieldHardlineQueryExecutor : BattlefieldQueryExecutor<BattlefieldHardlineServerInfo, BattlefieldPlayerInfo>
    {
        public BattlefieldHardlineQueryExecutor(string ipAddress, ushort port) : base(ipAddress, port)
        {
        }

        public BattlefieldHardlineQueryExecutor(EndPoint endPoint) : base(endPoint)
        {
        }
    }
}