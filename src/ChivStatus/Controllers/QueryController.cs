using System;
using System.Collections.Generic;
using System.Globalization;
using ChivStatus.Exceptions;
using ChivStatus.Helpers;
using GameEngineQuery.PacketModels;
using GameEngineQuery.QueryExecutors;
using Microsoft.AspNetCore.Mvc;
using JsonResult = ChivStatus.CustomTypes.JsonResult;

namespace ChivStatus.Controllers
{
    public abstract class QueryController<TQE,TSI, TPI> : Controller where TQE : IQueryExecutor<TSI, TPI> where TSI : ServerInfo, new() where TPI : PlayerInfo, new()
    {
        protected abstract string KeyFormatStringPrefix { get; }

        [HttpGet("{ip}/{port}")]
        public virtual JsonResult GetServerInfo(string ip, ushort port)
        {
            var key = string.Format(CultureInfo.InvariantCulture, "SI-{0}-Address({1}:{2})", this.KeyFormatStringPrefix, ip,
                port);

            return new JsonResult(Cache.GetOrStoreInCache(key, () => this.GetServerInfoQueryResult(ip, port)));
        }

        [HttpGet("{address}")]
        public virtual JsonResult GetServerInfo(string address)
        {
            var key = string.Format(CultureInfo.InvariantCulture, "SI-{0}-Address({1})", this.KeyFormatStringPrefix,
                address);

            return new JsonResult(Cache.GetOrStoreInCache(key, () => this.GetServerInfoQueryResult(address)));
        }

        [HttpGet("pi/{ip}/{port}")]
        public virtual JsonResult GetPlayerInfo(string ip, ushort port)
        {
            var key = string.Format(CultureInfo.InvariantCulture, "PI-{0}-Address({1}:{2})", this.KeyFormatStringPrefix, ip,
                port);

            return new JsonResult(Cache.GetOrStoreInCache(key, () => this.GetPlayerInfoQueryResult(ip, port)));
        }

        [HttpGet("pi/{address}")]
        public virtual JsonResult GetPlayerInfo(string address)
        {
            var key = string.Format(CultureInfo.InvariantCulture, "PI-{0}-Address({1})", this.KeyFormatStringPrefix,
                address);

            return new JsonResult(Cache.GetOrStoreInCache(key, () => this.GetPlayerInfoQueryResult(address)));
        }

        protected virtual TSI GetServerInfoQueryResult(string address)
        {
            if (address.Contains(":"))
            {
                var splitted = address.Split(':');

                ushort port;
                if (ushort.TryParse(splitted[1], out port))
                {
                    try
                    {
                        return this.GetServerInfoQueryResult(splitted[0], port);
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

        protected virtual TSI GetServerInfoQueryResult(string ip, ushort port)
        {
            if (port == 0)
            {
                throw new InvalidPortException();
            }

            IQueryExecutor<TSI,TPI> queryExecutor = Activator.CreateInstance(typeof(TQE), ip, port) as IQueryExecutor<TSI, TPI>;

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

        protected virtual IReadOnlyCollection<TPI> GetPlayerInfoQueryResult(string address)
        {
            if (address.Contains(":"))
            {
                var splitted = address.Split(':');

                ushort port;
                if (ushort.TryParse(splitted[1], out port))
                {
                    try
                    {
                        return this.GetPlayerInfoQueryResult(splitted[0], port);
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

        protected virtual IReadOnlyCollection<TPI> GetPlayerInfoQueryResult(string ip, ushort port)
        {
            if (port == 0)
            {
                throw new InvalidPortException();
            }

            IQueryExecutor<TSI, TPI> queryExecutor = Activator.CreateInstance(typeof(TQE), ip, port) as IQueryExecutor<TSI, TPI>;

            if (queryExecutor != null)
            {
                try
                {
                    return queryExecutor.GetPlayerInfo();
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