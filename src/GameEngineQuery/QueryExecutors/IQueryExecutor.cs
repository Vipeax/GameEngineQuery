namespace GameEngineQuery.QueryExecutors
{
    public interface IQueryExecutor<P>
    {
        P GetServerInfo();
    }
}
