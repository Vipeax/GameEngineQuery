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
        CouldNotInitiateA2SPlayerRequest,
        InvalidResponsePacketForA2SPlayerRequest,
        InvalidResponseHeaderForA2SPlayerRequest,
        InvalidResponsePacketForBattlefieldServerInfoRequest,
        InvalidResponsePacketForBattlefieldPlayerInfoRequest,
    }
}
