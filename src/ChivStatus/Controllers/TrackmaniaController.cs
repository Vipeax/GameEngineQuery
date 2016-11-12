using System;
using System.Globalization;
using ChivStatus.Exceptions;
using ChivStatus.Helpers;
using GameEngineQuery.PacketModels;
using GameEngineQuery.QueryExecutors;
using Microsoft.AspNetCore.Mvc;

namespace ChivStatus.Controllers
{
    [Route("api/tm")]
    public class TrackmaniaController
    {
        protected string KeyFormatStringPrefix => "TM";

        [HttpGet("{ip}/{port}/{user}/{password}")]
        public virtual JsonResult Get(string ip, ushort port, string user, string password)
        {
            var key = string.Format(CultureInfo.InvariantCulture, "{0}-Address({1}:{2})", this.KeyFormatStringPrefix, ip,
                port);

            return new JsonResult(Cache.GetOrStoreInCache(key, () => GetQueryResult(ip, port, user, password)));
        }

        [HttpGet("{address}/{user}/{password}")]
        public virtual JsonResult Get(string address, string user, string password)
        {
            var key = string.Format(CultureInfo.InvariantCulture, "{0}-Address({1})", this.KeyFormatStringPrefix,
                address);

            return new JsonResult(Cache.GetOrStoreInCache(key, () => GetQueryResult(address, user, password)));
        }

        protected virtual TrackManiaServerInfo GetQueryResult(string address, string user, string password)
        {
            if (address.Contains(":"))
            {
                var splitted = address.Split(':');

                ushort port;
                if (ushort.TryParse(splitted[1], out port))
                {
                    try
                    {
                        return this.GetQueryResult(splitted[0], port, user, password);
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

        protected virtual TrackManiaServerInfo GetQueryResult(string ip, ushort port, string user, string password)
        {
            if (port == 0)
            {
                throw new InvalidPortException();
            }

            IQueryExecutor<TrackManiaServerInfo> queryExecutor = new TrackManiaQueryExecutor(ip, port, user, password);

            try
            {
                return queryExecutor.GetServerInfo();
            }
            catch (FormatException)
            {
                throw new InvalidIpAddressException();
            }
        }
    }
}