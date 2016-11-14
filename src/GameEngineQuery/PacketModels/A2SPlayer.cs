namespace GameEngineQuery.PacketModels
{
    public class A2SPlayer : PlayerInfo
    {
        public byte Index { get; set; }
        public string Name { get; set; }
        public long Score { get; set; }
        public float Duration { get; set; }

        public override int Id => this.Index;
        public override string PlayerName => this.Name;
        public override long Points => this.Score;
    }
}