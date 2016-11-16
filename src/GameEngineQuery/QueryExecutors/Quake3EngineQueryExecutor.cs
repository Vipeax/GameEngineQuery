using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using GameEngineQuery.Constants;
using GameEngineQuery.Constants.Enumerations;
using GameEngineQuery.Exceptions;
using GameEngineQuery.Extensions;
using GameEngineQuery.PacketModels;

namespace GameEngineQuery.QueryExecutors
{
    public class Quake3EngineQueryExecutor : SocketQueryExecutor<Quake3ServerInfo, Quake3PlayerInfo>
    {
        private readonly Guid challenge;

        public Quake3EngineQueryExecutor(string ipAddress, ushort port) : base(ipAddress, port)
        {
            this.challenge = Guid.NewGuid();
        }

        public Quake3EngineQueryExecutor(EndPoint endPoint) : base(endPoint)
        {
            this.challenge = Guid.NewGuid();
        }

        public override Quake3ServerInfo GetServerInfo()
        {
            int index;
            var responseData = ExtractResponseData(out index);

            var dataTable = new Dictionary<string, string>();
            ushort playerCount;
            if (this.CheckChallenge(responseData))
            {
                var splittedResponseData = responseData.Split('\n');
                index = 0;
                if (splittedResponseData[index].Equals("statusResponse", StringComparison.OrdinalIgnoreCase))
                {
                    index++;
                }

                var splittedServerInfoData = splittedResponseData[index].Split('\\');
                for (int i = 1; i < splittedServerInfoData.Length; i++)
                {
                    dataTable.Add(splittedServerInfoData[i], splittedServerInfoData[i+1]);
                    i++;
                }

                index++;

                playerCount = (ushort) (splittedResponseData.Length - index - 1);
            }
            else
            {
                throw new Quake3EngineQueryException(ExceptionType.InvalidResponseChallengeForGetStatusRequest);
            }

            return new Quake3ServerInfo
            {
                CurrentPlayerCount = playerCount,
                Data = dataTable
            };
        }

        public override IReadOnlyCollection<Quake3PlayerInfo> GetPlayerInfo()
        {
            var players = new List<Quake3PlayerInfo>();

            int index;
            var responseData = ExtractResponseData(out index);
            if (this.CheckChallenge(responseData))
            {
                var splittedResponseData = responseData.Split('\n');
                index = 0;
                if (splittedResponseData[index].Equals("statusResponse", StringComparison.OrdinalIgnoreCase))
                {
                    index++;
                }

                index++;

                for (int i = index; i < splittedResponseData.Length - 1; i++)
                {
                    var playerData = splittedResponseData[i];

                    var splitIndexPing = playerData.IndexOf(" ", StringComparison.Ordinal);
                    var frags = playerData.Substring(0, splitIndexPing);
                    var splitIndexName = playerData.Substring(splitIndexPing + 1).IndexOf(" ", StringComparison.Ordinal);
                    var ping = playerData.Substring(splitIndexPing + 1, splitIndexName);
                    var name = playerData.Substring(splitIndexName + splitIndexPing + 1);

                    var nameIndex = name.IndexOf('"') + 1;
                    name = name.Substring(nameIndex);
                    name = name.Substring(0, name.IndexOf('"'));

                    players.Add(new Quake3PlayerInfo
                    {
                        Index = (ushort) (i - index + 1),
                        Name = name,
                        Ping = Convert.ToInt32(ping),
                        Score = Convert.ToInt64(frags)
                    });
                }
            }
            else
            {
                throw new Quake3EngineQueryException(ExceptionType.InvalidResponseChallengeForGetStatusRequest);
            }

            return players;
        }

        private string ExtractResponseData(out int index)
        {
            var baseRequestPacket = requestFactory.CreateRequestPacket(RequestPacketType.Quake3GetStatus);
            var challengeData = this.challenge.ToString("N");
            var requestPacket = new byte[baseRequestPacket.Length + challengeData.Length + 1];

            for (int i = 0; i < baseRequestPacket.Length; i++)
            {
                requestPacket[i] = baseRequestPacket[i];
            }

            requestPacket[baseRequestPacket.Length] = (byte) " "[0];

            for (int i = 0; i < challengeData.Length; i++)
            {
                requestPacket[i + baseRequestPacket.Length + 1] = (byte) challengeData[i];
            }

            byte[] responsePacket;
            try
            {
                responsePacket = this.HandleGameEngineQuery(requestPacket);
            }
            catch (Exception exception)
            {
                throw new Quake3EngineQueryException(ExceptionType.CouldNotInitiateGetStatusRequest, exception);
            }

            for (int i = 0; i < Values.Quake3Constants.HeaderLength; i++)
            {
                if (responsePacket[i] != 0xFF)
                {
                    throw new Quake3EngineQueryException(ExceptionType.InvalidResponsePacketForGetStatusRequest);
                }
            }

            index = Values.Quake3Constants.HeaderLength;
            string responseData = responsePacket.GetNextString(ref index);
            return responseData;
        }

        protected override byte[] HandleGameEngineQuery(byte[] request)
        {
            using (var s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
            {
                int packetSize = Values.Quake3Constants.PacketSize;

                s.SendTimeout = Values.ProjectConstants.SendTimeout;
                s.ReceiveTimeout = Values.ProjectConstants.ReceiveTimeout;

                try
                {
                    s.SendTo(request, endPoint);
                }
                catch (SocketException se)
                {
                    throw new Quake3EngineQueryException(ExceptionType.SocketSend, se);
                }

                byte[] getStatusResultBytes = new byte[packetSize];

                try
                {
                    s.ReceiveFrom(getStatusResultBytes, ref endPoint);
                }
                catch (SocketException se)
                {
                    throw new Quake3EngineQueryException(ExceptionType.SocketReceive, se);
                }

                return getStatusResultBytes;
            }
        }

        private bool CheckChallenge(string responseData)
        {
            var splittedResponseData = responseData.Split('\\');
            for (int index = 0; index < splittedResponseData.Length; index++)
            {
                var key = splittedResponseData[index];
                if (key.Equals("challenge", StringComparison.OrdinalIgnoreCase))
                {
                    var value = splittedResponseData[index + 1];
                    return value.Equals(challenge.ToString("N"));
                }
            }

            return false;
        }
    }
}