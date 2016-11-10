using System;
using System.Net;
using GameEngineQuery.PacketModels;
using TMXmlRpcLib;
using System.Collections;

namespace GameEngineQuery.QueryExecutors
{
    public struct SPlayerInfo
    {
        public string Login;

        public string NickName;

        public int PlayerId;

        public int TeamId;

        public int LadderRanking;

        public int SpectatorStatus;

        public int Flags;

        public void SetDefault()
        {
            this.Login = "";
            this.NickName = "";
            this.PlayerId = 0;
            this.TeamId = 0;
            this.LadderRanking = 0;
            this.SpectatorStatus = 0;
            this.Flags = 0;
        }
    }

    public class TrackManiaQueryExecutor : QueryExecutor<TrackManiaServerInfo>
    {
        private string level;
        private string password;

        public TrackManiaQueryExecutor(string ipAddress, ushort port, string level, string password) : base(ipAddress, port)
        {
            this.level = level;
            this.password = password;
        }

        public TrackManiaQueryExecutor(EndPoint endPoint, string level, string password) : base(endPoint)
        {
            this.level = level;
            this.password = password;
        }

        public override TrackManiaServerInfo GetServerInfo()
        {
            var requestClient = new XmlRpcClient(this.endPoint);

            GbxCall gbxCall = requestClient.Request("Authenticate", new object[]
                            {
                                "SuperAdmin",
                                "SuperAdmin1fawefawefawefawefawefwefwa"
                            });

            var servername = GetServerName(requestClient);
            Console.WriteLine(servername);

            var playerList = GetPlayerList(requestClient, 32, 0, 1);

            return new TrackManiaServerInfo()
            {
                PlayerCount1 = (ushort)playerList.Length
            };
        }

        protected override byte[] HandleGameEngineQuery(byte[] request)
        {
            throw new NotImplementedException();
        }


        private string GetServerName(XmlRpcClient client)
        {
            GbxCall gbxCall = client.Request("GetServerName", new object[0]);
            return gbxCall.Params[0].ToString();
        }


        private SPlayerInfo[] GetPlayerList(XmlRpcClient client, int MaxCount, int FirstIndex, int StructVersion = 1)
        {
            checked
            {
                SPlayerInfo[] result;
                try
                {
                    GbxCall gbxCall = client.Request("GetPlayerList", new object[]
                    {
                        MaxCount,
                        FirstIndex,
                        StructVersion
                    });
                    ArrayList arrayList = new ArrayList();
                    arrayList = (ArrayList)gbxCall.Params[0];
                    SPlayerInfo[] array = new SPlayerInfo[arrayList.Count - 1 + 1];
                    int arg_6C_0 = 0;
                    int num = arrayList.Count - 1;
                    for (int i = arg_6C_0; i <= num; i++)
                    {

                        dynamic obj = arrayList[i];

                        array[i].Login = obj.Login.ToString();
                        array[i].NickName = obj.NickName.ToString();
                        array[i].PlayerId = int.Parse(obj.PlayerId.ToString());
                        array[i].TeamId = int.Parse(obj.TeamId.ToString());
                        array[i].SpectatorStatus = int.Parse(obj.SpectatorStatus.ToString());
                        array[i].LadderRanking = int.Parse(obj.LadderRanking.ToString());
                        array[i].Flags = int.Parse(obj.Flags.ToString());
                    }
                    result = array;
                }
                catch (Exception arg_1EB_0)
                {
                    result = new SPlayerInfo[0];
                }
                return result;
            }
        }

    }
}