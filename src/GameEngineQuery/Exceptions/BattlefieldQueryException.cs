using System;
using GameEngineQuery.Constants.Enumerations;

namespace GameEngineQuery.Exceptions
{
    public class BattlefieldQueryException : Exception
    {
        internal BattlefieldQueryException(ExceptionType exceptionType, Exception ex) : base(ModifyMessage(exceptionType, ex.Message), ex)
        {
        }

        internal BattlefieldQueryException(ExceptionType exceptionType) : base(CreateMessage(exceptionType))
        {
        }

        private static string CreateMessage(ExceptionType exceptionType)
        {
            switch (exceptionType)
            {
                case ExceptionType.SocketSend:
                    return "Could not send Battlefield Server Info packet to server.";
                case ExceptionType.SocketReceive:
                    return "Could not receive Battlefield Info result packet from server.";
                case ExceptionType.CouldNotOpenBattlefieldTcpConnection:
                    return "Could not initiate TCP connection.";
                case ExceptionType.InvalidResponsePacketForBattlefieldServerInfoRequest:
                    return "Invalid response packet received for Battlefield Server Info request call.";
                case ExceptionType.InvalidResponsePacketForBattlefieldPlayerInfoRequest:
                    return "Invalid response packet received for Battlefield Player Info request call.";
            }

            return string.Empty;
        }

        private static string ModifyMessage(ExceptionType exceptionType, string message)
        {
            switch (exceptionType)
            {
                case ExceptionType.SocketSend:
                    return $"Could not send Battlefield Server packet to server {{{message}}}.";
                case ExceptionType.SocketReceive:
                    return $"Could not receive Battlefield Server result packet from serve {{{message}}}.";
                case ExceptionType.CouldNotOpenBattlefieldTcpConnection:
                    return $"Could not initiate TCP connection {{{message}}}.";
                case ExceptionType.InvalidResponsePacketForBattlefieldServerInfoRequest:
                    return $"Invalid response packet received for Battlefield Server Info request call {{{message}}}.";
                case ExceptionType.InvalidResponsePacketForBattlefieldPlayerInfoRequest:
                    return $"Invalid response packet received for Battlefield Player Info request call {{{message}}}.";
            }

            return message;
        }
    }
}