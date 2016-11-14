namespace GameEngineQuery.PacketModels
{
    public class TrackManiaPlayerInfo : PlayerInfo
    {
        public byte Index { get; set; }
        public int Flags { get; set; }
        public int TeamId { get; set; }
        public string Login { get; set; }
        public string NickName { get; set; }
        public byte SpectatorStatus { get; set; }
        public int PlayerId { get; set; }
        public int LadderRanking { get; set; }

        public override int Id => this.PlayerId;
        public override string PlayerName => this.NickName;
        public override long Points => 0;
    }
}