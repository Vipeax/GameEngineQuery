using System;
using System.Collections.Generic;
using System.Net;
using GameEngineQuery.Extensions;
using GameEngineQuery.Factories;
using GameEngineQuery.PacketModels;

namespace GameEngineQuery.QueryExecutors
{
    public abstract class QueryExecutor<TSI, TPI> : IQueryExecutor<TSI, TPI> where TSI : ServerInfo, new() where TPI : PlayerInfo, new()
    {
        protected readonly IRequestFactory requestFactory;

        protected EndPoint endPoint;

        protected QueryExecutor(string ipAddress, ushort port) : this(new IPEndPoint(IPAddress.Parse(ipAddress), port))
        {
        }

        protected QueryExecutor(EndPoint endPoint)
        {
            var point = endPoint as DnsEndPoint;
            if (point != null)
            {
                this.endPoint = point.GetIPEndPointFromHostName();
            }
            else if (endPoint is IPEndPoint)
            {
                this.endPoint = endPoint;
            }
            else
            {
                throw new ArgumentException($"The supplied EndPoint type ({typeof(EndPoint).FullName}) is not supported by this library.", nameof(endPoint));
            }

            this.requestFactory = new RequestFactory();
        }

        public abstract TSI GetServerInfo();

        public abstract IReadOnlyCollection<TPI> GetPlayerInfo();
    }
}