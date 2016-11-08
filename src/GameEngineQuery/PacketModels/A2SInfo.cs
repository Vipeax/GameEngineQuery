using System;
using GameEngineQuery.Constants.Enumerations;
using Environment = GameEngineQuery.Constants.Enumerations.Environment;

namespace GameEngineQuery.PacketModels
{
    internal class A2SInfo : ServerInfo
    {
        public byte Padding { get; set; }
        public byte Padding2 { get; set; }
        public byte Padding3 { get; set; }
        public byte Padding4 { get; set; }
        public byte Header { get; set; }
        public byte Protocol { get; set; }
        public string Name { get; set; }
        public string Map { get; set; }
        public string Folder { get; set; }
        public string Game { get; set; }
        public ushort Id { get; set; }
        public byte Players { get; set; }
        public byte MaxPlayers { get; set; }
        public byte Bots { get; set; }
        public ServerType ServerType { get; set; }
        public Environment Environment { get; set; }
        public Visibility Visibility { get; set; }
        public ValveAntiCheat ValveAntiCheat { get; set; }
        public Version Version { get; set; }
        public string Flags { get; set; }

        public override string MapName => this.Map;
        public override ushort PlayerCount => this.Players;
        public override ushort MaxPlayerCount => this.MaxPlayers;
        public override string ServerName => this.Name;
    }
}
