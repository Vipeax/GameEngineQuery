using System.Net;
using GameEngineQuery.PacketModels;

namespace GameEngineQuery.QueryExecutors
{
    public abstract class SocketQueryExecutor<TSI, TPI> : QueryExecutor<TSI, TPI> where TSI : ServerInfo, new()
        where TPI : PlayerInfo, new()
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