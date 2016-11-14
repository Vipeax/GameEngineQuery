using System;
using System.Collections.Generic;
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
        public virtual JsonResult GetServerInfo(string ip, ushort port, string user, string password)
        {
            var key = string.Format(CultureInfo.InvariantCulture, "SI-{0}-Address({1}:{2})", this.KeyFormatStringPrefix, ip,
                port);

            return new JsonResult(Cache.GetOrStoreInCache(key, () => this.GetServerInfoQueryResult(ip, port, user, password)));
        }

        [HttpGet("{address}/{user}/{password}")]
        public virtual JsonResult GetServerInfo(string address, string user, string password)
        {
            var key = string.Format(CultureInfo.InvariantCulture, "SI-{0}-Address({1})", this.KeyFormatStringPrefix,
                address);

            return new JsonResult(Cache.GetOrStoreInCache(key, () => this.GetServerInfoQueryResult(address, user, password)));
        }

        [HttpGet("pi/{ip}/{port}/{user}/{password}")]
        public virtual JsonResult GetPlayerInfo(string ip, ushort port, string user, string password)
        {
            var key = string.Format(CultureInfo.InvariantCulture, "PI-{0}-Address({1}:{2})", this.KeyFormatStringPrefix, ip,
                port);

            return new JsonResult(Cache.GetOrStoreInCache(key, () => this.GetPlayerInfoQueryResult(ip, port, user, password)));
        }

        [HttpGet("pi/{address}/{user}/{password}")]
        public virtual JsonResult GetPlayerInfo(string address, string user, string password)
        {
            var key = string.Format(CultureInfo.InvariantCulture, "PI-{0}-Address({1})", this.KeyFormatStringPrefix,
                address);

            return new JsonResult(Cache.GetOrStoreInCache(key, () => this.GetPlayerInfoQueryResult(address, user, password)));
        }

        protected virtual TrackManiaServerInfo GetServerInfoQueryResult(string address, string user, string password)
        {
            if (address.Contains(":"))
            {
                var splitted = address.Split(':');

                ushort port;
                if (ushort.TryParse(splitted[1], out port))
                {
                    try
                    {
                        return this.GetServerInfoQueryResult(splitted[0], port, user, password);
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

        protected virtual TrackManiaServerInfo GetServerInfoQueryResult(string ip, ushort port, string user, string password)
        {
            if (port == 0)
            {
                throw new InvalidPortException();
            }

            IQueryExecutor<TrackManiaServerInfo, TrackManiaPlayerInfo> queryExecutor = new TrackManiaQueryExecutor(ip, port, user, password);

            try
            {
                return queryExecutor.GetServerInfo();
            }
            catch (FormatException)
            {
                throw new InvalidIpAddressException();
            }
        }

        protected virtual IReadOnlyCollection<TrackManiaPlayerInfo> GetPlayerInfoQueryResult(string address, string user, string password)
        {
            if (address.Contains(":"))
            {
                var splitted = address.Split(':');

                ushort port;
                if (ushort.TryParse(splitted[1], out port))
                {
                    try
                    {
                        return this.GetPlayerInfoQueryResult(splitted[0], port, user, password);
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

        protected virtual IReadOnlyCollection<TrackManiaPlayerInfo> GetPlayerInfoQueryResult(string ip, ushort port, string user, string password)
        {
            if (port == 0)
            {
                throw new InvalidPortException();
            }

            IQueryExecutor<TrackManiaServerInfo, TrackManiaPlayerInfo> queryExecutor = new TrackManiaQueryExecutor(ip, port, user, password);

            try
            {
                return queryExecutor.GetPlayerInfo();
            }
            catch (FormatException)
            {
                throw new InvalidIpAddressException();
            }
        }
    }
}