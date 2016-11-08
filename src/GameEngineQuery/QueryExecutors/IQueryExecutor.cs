using GameEngineQuery.PacketModels;

namespace GameEngineQuery.QueryExecutors
{
    public interface IQueryExecutor
    {
        ServerInfo GetServerInfo();
    }
}
