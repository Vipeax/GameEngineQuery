using System;
using System.Globalization;
using ChivStatus.Exceptions;
using ChivStatus.Helpers;
using GameEngineQuery.PacketModels;
using GameEngineQuery.QueryExecutors;
using Microsoft.AspNetCore.Mvc;
using JsonResult = ChivStatus.CustomTypes.JsonResult;

namespace ChivStatus.Controllers
{
    public abstract class QueryController<TQE,TSI> : Controller where TQE : IQueryExecutor<TSI> where TSI : ServerInfo
    {
        protected abstract string KeyFormatStringPrefix { get; }

        [HttpGet("{ip}/{port}")]
        public virtual JsonResult Get(string ip, ushort port)
        {
            var key = string.Format(CultureInfo.InvariantCulture, "{0}-Address({1}:{2})", this.KeyFormatStringPrefix, ip,
                port);

            return new JsonResult(Cache.GetOrStoreInCache(key, () => GetQueryResult(ip, port)));
        }

        // GET api/bf3/12.34.12.34:27015
        [HttpGet("{address}")]
        public virtual JsonResult Get(string address)
        {
            var key = string.Format(CultureInfo.InvariantCulture, "{0}-Address({1})", this.KeyFormatStringPrefix,
                address);

            return new JsonResult(Cache.GetOrStoreInCache(key, () => GetQueryResult(address)));
        }

        protected virtual TSI GetQueryResult(string address)
        {
            if (address.Contains(":"))
            {
                var splitted = address.Split(':');

                ushort port;
                if (ushort.TryParse(splitted[1], out port))
                {
                    try
                    {
                        return this.GetQueryResult(splitted[0], port);
                    }
                    catch (FormatException)
                    {
                        throw new InvalidIpAddressException();
                    }
                }

                throw new InvalidPortException();
            }

            throw new InvalidAddressFormatException();
        }

        protected virtual TSI GetQueryResult(string ip, ushort port)
        {
            if (port == 0)
            {
                throw new InvalidPortException();
            }

            IQueryExecutor<TSI> queryExecutor = Activator.CreateInstance(typeof(TQE), ip, port) as IQueryExecutor<TSI>;

            if (queryExecutor != null)
            {
                try
                {
                    return queryExecutor.GetServerInfo();
                }
                catch (FormatException)
                {
                    throw new InvalidIpAddressException();
                }
            }

            throw new QueryExecutorInitializationException();
        }
    }
}