namespace GameEngineQuery.PacketModels
{
    public abstract class BattlefieldServerInfo : ServerInfo
    {
        public byte Padding { get; set; }
        public byte Padding2 { get; set; }
        public byte Padding3 { get; set; }
        public byte Padding4 { get; set; }
        public byte Padding5 { get; set; }
        public byte Padding6 { get; set; }
        public byte Padding7 { get; set; }
        public byte Padding8 { get; set; }
        public byte Padding9 { get; set; }
        public byte Padding10 { get; set; }
        public byte Padding11 { get; set; }
        public byte Padding12 { get; set; }
        public string Response { get; set; }
        public string Name { get; set; }
        public ushort Players { get; set; }
        public ushort MaxPlayers { get; set; }
        public string GameType { get; set; }
        public string Map { get; set; }
        public ushort RoundsPlayed { get; set; }
        public ushort RoundsTotal { get; set; }
        public float[] Tickets { get; set; }
        public float TargetScore { get; set; }
        public bool Online { get; set; }
        public bool Ranked { get; set; }
        public bool Punkbuster { get; set; }
        public bool Passworded { get; set; }
        public uint Uptime { get; set; }
        public uint RoundTime { get; set; }
        public string Address { get; set; }
        public string PunkbusterVersion { get; set; }
        public bool JoinQueue { get; set; }
        public string Region { get; set; }

        public override string MapName => this.Map;
        public override ushort PlayerCount => this.Players;
        public override ushort MaxPlayerCount => this.MaxPlayers;
        public override string ServerName => this.Name;
    }
}