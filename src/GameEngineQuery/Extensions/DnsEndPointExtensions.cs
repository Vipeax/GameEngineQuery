using System;
using System.Net;

namespace GameEngineQuery.Extensions
{
    internal static class DnsEndPointExtensions
    {
        internal static IPEndPoint GetIPEndPointFromHostName(this DnsEndPoint dnsEndPoint, bool throwIfMoreThanOneIp = false)
        {
            var addresses = Dns.GetHostAddressesAsync(dnsEndPoint.Host).Result;

            if (addresses.Length == 0)
            {
                throw new ArgumentException(
                    "Unable to retrieve address from specified host name.",
                    nameof(dnsEndPoint)
                    );
            }

            if (throwIfMoreThanOneIp && addresses.Length > 1)
            {
                throw new ArgumentException(
                    "There is more that one IP address to the specified host.",
                    nameof(dnsEndPoint)
                    );
            }

            return new IPEndPoint(addresses[0], dnsEndPoint.Port);
        }
    }
}
