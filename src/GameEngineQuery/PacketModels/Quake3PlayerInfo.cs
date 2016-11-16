namespace GameEngineQuery.PacketModels
{
    public class Quake3PlayerInfo : PlayerInfo
    {
        public ushort Index { get; set; }
        public string Name { get; set; }
        public long Score { get; set; }
        public int Ping { get; set; }

        public override int Id => this.Index;
        public override string PlayerName => this.Name;
        public override long Points => this.Score;
    }
}