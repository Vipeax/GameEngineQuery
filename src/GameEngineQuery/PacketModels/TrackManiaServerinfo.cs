namespace GameEngineQuery.PacketModels
{
    public class TrackManiaServerInfo : ServerInfo
    {
        public string Name { get; set; }
        public ushort CurrentMaxPlayers { get; set; }
        public ushort CurrentPlayerCount { get; set; }
        public string CurrentMapName { get; set; }
        public string MapType { get; set; }
        public string Mood { get; set; }
        public string Environment { get; set; }
        public uint CopperPrice { get; set; }
        public bool LapRace { get; set; }
        public uint BronzeTime { get; set; }
        public uint SilverTime { get; set; }
        public uint GoldTime { get; set; }
        public string UId { get; set; }
        public ushort NbCheckpoints { get; set; }
        public ushort NbLaps { get; set; }
        public string FileName { get; set; }
        public string Author { get; set; }
        public string MapStyle { get; set; }
        public uint AuthorTime { get; set; }

        public override string MapName => this.CurrentMapName;
        public override ushort MaxPlayerCount => this.CurrentMaxPlayers;
        public override ushort PlayerCount => this.CurrentPlayerCount;
        public override string ServerName => this.Name;
    }
}