namespace GameEngineQuery.PacketModels
{
    public abstract class PlayerInfo
    {
        public abstract int Id { get; }
        public abstract string PlayerName { get; }
        public abstract long Points { get; }
    }
}