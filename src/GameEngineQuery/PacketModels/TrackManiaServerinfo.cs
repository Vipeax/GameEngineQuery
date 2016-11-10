using System;

namespace GameEngineQuery.PacketModels
{
    public class TrackManiaServerInfo : ServerInfo
    {

        public string Map { get; set; }
        public ushort MaxPlayerCount1 { get; set; }
        public ushort PlayerCount1 { get; set; }
        public string ServerName5 { get; set; }



        public override string MapName => this.Map;

        public override ushort MaxPlayerCount => MaxPlayerCount1;

        public override ushort PlayerCount => PlayerCount1;

        public override string ServerName => ServerName5;
    }
}
