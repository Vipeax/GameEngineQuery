namespace GameEngineQuery.Constants.Enumerations
{
    internal enum ExceptionType
    {
        SocketSend,
        SocketReceive,
        CouldNotOpenBattlefieldTcpConnection,
        CouldNotInitiateA2SInfoRequest,
        InvalidResponsePacketForA2SInfoRequest,
        InvalidResponseHeaderForA2SInfoRequest,
        InvalidResponsePacketForBattlefieldServerInfoRequest,
    }
}
