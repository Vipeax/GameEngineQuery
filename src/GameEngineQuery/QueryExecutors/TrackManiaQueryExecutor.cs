using System;
using System.Net;
using GameEngineQuery.PacketModels;
using System.Collections;
using System.Collections.Generic;
using XmlRpcCore;

namespace GameEngineQuery.QueryExecutors
{
    public class TrackManiaQueryExecutor : QueryExecutor<TrackManiaServerInfo, TrackManiaPlayerInfo>
    {
        private readonly string user;
        private readonly string password;

        public TrackManiaQueryExecutor(string ipAddress, ushort port, string user, string password) : base(ipAddress, port)
        {
            this.user = user;
            this.password = password;
        }

        public TrackManiaQueryExecutor(EndPoint endPoint, string user, string password) : base(endPoint)
        {
            this.user = user;
            this.password = password;
        }

        public override TrackManiaServerInfo GetServerInfo()
        {
            using (var requestClient = new XmlRpcClient(this.endPoint))
            {

                GbxCall gbxCall = requestClient.Request("Authenticate", new object[]
                {
                    this.user,
                    this.password
                });

                if (gbxCall.HasError == false && gbxCall.Params[0].Equals(true))
                {
                    var count = this.GetPlayerCount(requestClient, 256, 0).GetValueOrDefault(0);
                    var mapInfo = this.GetCurrentMapInfo(requestClient);
                    var serverOptions = this.GetServerOptions(requestClient);

                    if (mapInfo.Count > 0)
                    {
                        return new TrackManiaServerInfo
                        {
                            Name = serverOptions.Item1,
                            CurrentMapName = mapInfo["Name"] as string,
                            CurrentMaxPlayers = serverOptions.Item2,
                            CurrentPlayerCount = count,
                            MapType = mapInfo["MapType"] as string,
                            Mood = mapInfo["Mood"] as string,
                            Environment = mapInfo["Environment"] as string,
                            CopperPrice = Convert.ToUInt32(mapInfo["CopperPrice"]),
                            LapRace = Convert.ToBoolean(mapInfo["LapRace"]),
                            BronzeTime = Convert.ToUInt32(mapInfo["BronzeTime"]),
                            SilverTime = Convert.ToUInt32(mapInfo["SilverTime"]),
                            GoldTime = Convert.ToUInt32(mapInfo["GoldTime"]),
                            UId = mapInfo["UId"] as string,
                            NbCheckpoints = Convert.ToUInt16(mapInfo["NbCheckpoints"]),
                            NbLaps = Convert.ToUInt16(mapInfo["NbLaps"]),
                            FileName = mapInfo["FileName"] as string,
                            Author = mapInfo["Author"] as string,
                            MapStyle = mapInfo["MapStyle"] as string,
                            AuthorTime = Convert.ToUInt32(mapInfo["AuthorTime"])
                        };
                    }
                }

                return new TrackManiaServerInfo();
            }
        }

        public override IReadOnlyCollection<TrackManiaPlayerInfo> GetPlayerInfo()
        {
            using (var requestClient = new XmlRpcClient(this.endPoint))
            {
                return this.GetPlayers(requestClient, 256, 0);
            }
        }

        private Tuple<string, ushort> GetServerOptions(XmlRpcClient client)
        {
            GbxCall gbxCall = client.Request("GetServerOptions", new object[0]);

            if (gbxCall.HasError == false)
            {
                var hashTable = gbxCall.Params[0] as Hashtable;

                if (hashTable != null)
                {
                    return new Tuple<string, ushort>(hashTable["Name"] as string, Convert.ToUInt16(hashTable["CurrentMaxPlayers"]));
                }
            }

            return new Tuple<string, ushort>(default(string), default(ushort));
        }

        private Hashtable GetCurrentMapInfo(XmlRpcClient client)
        {
            GbxCall gbxCall = client.Request("GetCurrentMapInfo", new object[0]);

            if (gbxCall.HasError == false)
            {
                return gbxCall.Params[0] as Hashtable;
            }

            return new Hashtable();
        }

        private ushort? GetPlayerCount(XmlRpcClient client, int maxCount, int firstIndex, int structVersion = 1)
        {
            GbxCall gbxCall = client.Request("GetPlayerList", new object[]
            {
                maxCount,
                firstIndex,
                structVersion
            });

            if (gbxCall.HasError == false)
            {
                return (ushort?) (gbxCall.Params[0] as ArrayList)?.Count;
            }

            return 0;
        }

        private IReadOnlyCollection<TrackManiaPlayerInfo> GetPlayers(XmlRpcClient client, int maxCount, int firstIndex, int structVersion = 1)
        {
            GbxCall gbxCall = client.Request("GetPlayerList", new object[]
            {
                maxCount,
                firstIndex,
                structVersion
            });

            if (gbxCall.HasError == false)
            {
                var playerList = gbxCall.Params[0] as ArrayList;

                if (playerList != null)
                {
                    var players = new List<TrackManiaPlayerInfo>();

                    for (byte i = 0; i < playerList.Count; i++)
                    {
                        var playerData = playerList[i] as Hashtable;

                        if (playerData != null)
                        {
                            players.Add(new TrackManiaPlayerInfo
                            {
                                Index = i,
                                Flags = playerData["Flags"] as int? ?? 0,
                                TeamId = playerData["TeamId"] as int? ?? 0,
                                Login = playerData["Login"] as string,
                                NickName = playerData["NickName"] as string,
                                SpectatorStatus = playerData["SpectatorStatus"] as byte? ?? 0,
                                PlayerId = playerData["PlayerId"] as int? ?? 0,
                                LadderRanking = playerData["LadderRanking"] as int? ?? 0
                        });
                        }
                    }

                    return players;
                }
            }

            return new List<TrackManiaPlayerInfo>();
        }
    }
}