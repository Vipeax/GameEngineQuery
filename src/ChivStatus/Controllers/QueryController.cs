using System;
using System.Globalization;
using ChivStatus.Exceptions;
using ChivStatus.Helpers;
using GameEngineQuery.QueryExecutors;
using Microsoft.AspNetCore.Mvc;
using JsonResult = ChivStatus.CustomTypes.JsonResult;

namespace ChivStatus.Controllers
{
    public abstract class QueryController<T> : Controller where T : IQueryExecutor
    {
        protected abstract string KeyFormatStringPrefix { get; }

        [HttpGet("{ip}/{port}")]
        public virtual JsonResult Get(string ip, ushort port)
        {
            var key = string.Format(CultureInfo.InvariantCulture, "{0}-Address({1}:{2})", this.KeyFormatStringPrefix, ip,
                port);

            return Cache.GetOrStoreInCache(key, () => GetQueryResult(ip, port));
        }

        // GET api/bf3/12.34.12.34:27015
        [HttpGet("{address}")]
        public virtual JsonResult Get(string address)
        {
            var key = string.Format(CultureInfo.InvariantCulture, "{0}-Address({1})", this.KeyFormatStringPrefix,
                address);

            return Cache.GetOrStoreInCache(key, () => GetQueryResult(address));
        }

        protected virtual JsonResult GetQueryResult(string address)
        {
            if (address.Contains(":"))
            {
                var splitted = address.Split(':');

                ushort port;
                if (ushort.TryParse(splitted[1], out port))
                {
                    try
                    {
                        return this.Get(splitted[0], port);
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

        protected virtual JsonResult GetQueryResult(string ip, ushort port)
        {
            if (port == 0)
            {
                throw new InvalidPortException();
            }

            IQueryExecutor queryExecutor = Activator.CreateInstance(typeof(T), ip, port) as IQueryExecutor;

            if (queryExecutor != null)
            {
                try
                {
                    var serverInfo = queryExecutor.GetServerInfo();
                    return new JsonResult(serverInfo);
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