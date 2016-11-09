namespace GameEngineQuery.QueryExecutors
{
    public interface IQueryExecutor<TSI>
    {
        TSI GetServerInfo();
    }
}
