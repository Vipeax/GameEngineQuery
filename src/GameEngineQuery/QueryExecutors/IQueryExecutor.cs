using System.Collections.Generic;
using GameEngineQuery.PacketModels;

namespace GameEngineQuery.QueryExecutors
{
    public interface IQueryExecutor<TSI, PSI> where TSI : ServerInfo, new() where PSI : PlayerInfo, new()
    {
        TSI GetServerInfo();
        IReadOnlyCollection<PSI> GetPlayerInfo();
    }
}
