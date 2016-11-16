using System;
using System.Collections.Generic;
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
    public class SourceEngineQueryExecutor : SocketQueryExecutor<A2SInfo, A2SPlayer>
    {
        public SourceEngineQueryExecutor(string ipAddress, ushort port) : base(ipAddress, port)
        {
        }

        public SourceEngineQueryExecutor(EndPoint endPoint) : base(endPoint)
        {
        }

        public override A2SInfo GetServerInfo()
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


        public override IReadOnlyCollection<A2SPlayer> GetPlayerInfo()
        {
            var requestPacket = requestFactory.CreateRequestPacket(RequestPacketType.A2SPlayer);

            byte[] responsePacket;
            try
            {
                responsePacket = this.HandleGameEngineQuery(requestPacket);
            }
            catch (Exception exception)
            {
                throw new SourceEngineQueryException(ExceptionType.CouldNotInitiateA2SPlayerRequest, exception);
            }

            for (int i = 0; i < Values.ValveConstants.HeaderLength; i++)
            {
                if (responsePacket[i] != 0xFF)
                {
                    throw new SourceEngineQueryException(ExceptionType.InvalidResponsePacketForA2SPlayerRequest);
                }
            }

            if (responsePacket[4] != 0x41)
            {
                throw new SourceEngineQueryException(ExceptionType.InvalidResponseHeaderForA2SPlayerRequest);
            }
            
            requestPacket[4] = 0x55;

            for (int i = 5; i < 9; i++)
            {
                requestPacket[i] = responsePacket[i];
            }

            responsePacket = this.HandleGameEngineQuery(requestPacket);

            int index;

            for (index = 0; index < Values.ValveConstants.HeaderLength; index++)
            {
                if (responsePacket[index] != 0xFF)
                {
                    throw new SourceEngineQueryException(ExceptionType.InvalidResponsePacketForA2SPlayerRequest);
                }
            }

            if (responsePacket[index++] != 0x44)
            {
                throw new SourceEngineQueryException(ExceptionType.InvalidResponseHeaderForA2SPlayerRequest);
            }

            byte playerCount = responsePacket[index++];

            var players = new List<A2SPlayer>();

            for (int i = 0; i < playerCount; i++)
            {
                players.Add(new A2SPlayer
                {
                    Index = responsePacket[index++],
                    Name = responsePacket.GetNextString(ref index),
                    Score = responsePacket.GetNextValueOfType<int>(ref index),
                    Duration = responsePacket.GetNextValueOfType<float>(ref index),
                });
            }

            return players;
        }

        protected override byte[] HandleGameEngineQuery(byte[] request)
        {
            using (var s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
            {
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
}