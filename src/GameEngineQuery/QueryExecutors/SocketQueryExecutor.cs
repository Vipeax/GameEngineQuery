using System.Net;

namespace GameEngineQuery.QueryExecutors
{
    public abstract class SocketQueryExecutor<TSI> : QueryExecutor<TSI>
    {
        protected SocketQueryExecutor(string ipAddress, ushort port) : base(ipAddress, port)
        {
        }

        protected SocketQueryExecutor(EndPoint endPoint) : base(endPoint)
        {
        }

        protected abstract byte[] HandleGameEngineQuery(byte[] request);
    }
}