namespace GameEngineQuery.PacketModels
{
    public abstract class ServerInfo
    {
        public abstract string ServerName { get; }
        public abstract string MapName { get; }
        public abstract ushort PlayerCount { get; }
        public abstract ushort MaxPlayerCount { get; }
    }
}