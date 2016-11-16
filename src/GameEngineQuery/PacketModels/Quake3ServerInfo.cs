using System;
using System.Collections.Generic;

namespace GameEngineQuery.PacketModels
{
    public class Quake3ServerInfo : ServerInfo
    { 
        public ushort CurrentPlayerCount { get; set; }

        public Dictionary<string, string> Data { get; set; }

        public override string ServerName => this.Data["sv_hostname"];
        public override string MapName => this.Data["mapname"];
        public override ushort PlayerCount => this.CurrentPlayerCount;
        public override ushort MaxPlayerCount => Convert.ToUInt16(this.Data["sv_maxclients"]);
    }
}