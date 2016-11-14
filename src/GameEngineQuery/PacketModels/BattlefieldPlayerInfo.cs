namespace GameEngineQuery.PacketModels
{
    public class BattlefieldPlayerInfo : PlayerInfo
    {
        public byte Index { get; set; }
        public string Name { get; set; }
        public string Guid { get; set; }
        public string Team { get; set; }
        public string Squad { get; set; }
        public int Kills { get; set; }
        public int Deaths { get; set; }
        public int Score { get; set; }
        public int Rank { get; set; }
        public int Ping { get; set; }

        public override int Id => this.Index;
        public override string PlayerName => this.Name;
        public override long Points => this.Score;
    }
}