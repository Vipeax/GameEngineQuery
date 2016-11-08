using System;
using System.Net;
using System.Net.Sockets;
using GameEngineQuery.Constants;
using GameEngineQuery.Constants.Enumerations;
using GameEngineQuery.Exceptions;
using GameEngineQuery.Extensions;
using GameEngineQuery.PacketModels;
using Environment = GameEngineQuery.Constants.Enumerations.Environment;

namespace GameEngineQuery.QueryExecutors
{
    public class SourceEngineQueryExecutor : QueryExecutor
    {
        public SourceEngineQueryExecutor(string ipAddress, ushort port) : base(ipAddress, port)
        {
        }

        public SourceEngineQueryExecutor(EndPoint endPoint) : base(endPoint)
        {
        }

        public override ServerInfo GetServerInfo()
        {
            var requestPacket = requestFactory.CreateRequestPacket(RequestPacketType.A2SInfo);

            byte[] responsePacket;
            try
            {
                responsePacket = this.HandleGameEngineQuery(requestPacket);
            }
            catch (Exception exception)
            {
                throw new SourceEngineQueryException(ExceptionType.CouldNotInitiateA2SInfoRequest, exception);
            }

            for (int i = 0; i < Values.ValveConstants.HeaderLength; i++)
            {
                if (responsePacket[i] != 0xFF)
                {
                    throw new SourceEngineQueryException(ExceptionType.InvalidResponsePacketForA2SInfoRequest);
                }
            }

            if (responsePacket[4] != 0x49)
            {
                throw new SourceEngineQueryException(ExceptionType.InvalidResponseHeaderForA2SInfoRequest);
            }

            int index = 0;

            A2SInfo info = new A2SInfo
            {
                Padding = responsePacket[index++],
                Padding2 = responsePacket[index++],
                Padding3 = responsePacket[index++],
                Padding4 = responsePacket[index++],
                Header = responsePacket[index++],
                Protocol = responsePacket[index++],
                Name = responsePacket.GetNextString(ref index),
                Map = responsePacket.GetNextString(ref index),
                Folder = responsePacket.GetNextString(ref index),
                Game = responsePacket.GetNextString(ref index),
                Id = responsePacket.GetNextUShort(ref index),
                Players = responsePacket[index++],
                MaxPlayers = responsePacket[index++],
                Bots = responsePacket[index++],
                ServerType = (ServerType)responsePacket[index++],
                Environment = (Environment)responsePacket[index++],
                Visibility = (Visibility)responsePacket[index++],
                ValveAntiCheat = (ValveAntiCheat)responsePacket[index++],
                Version = responsePacket.GetVersion(ref index),
                Flags = responsePacket.GetNextString(ref index)
            };

            return info;
        }

        protected override byte[] HandleGameEngineQuery(byte[] request)
        {
            var s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            // Steam uses a packet size of 1400 bytes + IP/UDP headers. If a request or response needs more packets for the data it starts the packets with an additional header.
            int packetSize = Values.ValveConstants.PacketSize;

            s.SendTimeout = Values.ProjectConstants.SendTimeout;
            s.ReceiveTimeout = Values.ProjectConstants.ReceiveTimeout;

            try
            {
                s.SendTo(request, endPoint);
            }
            catch (SocketException se)
            {
                throw new SourceEngineQueryException(ExceptionType.SocketSend, se);
            }

            byte[] a2SResultBytes = new byte[packetSize];

            try
            {
                s.ReceiveFrom(a2SResultBytes, ref endPoint);
            }
            catch (SocketException se)
            {
                throw new SourceEngineQueryException(ExceptionType.SocketReceive, se);
            }

            return a2SResultBytes;
        }
    }
}