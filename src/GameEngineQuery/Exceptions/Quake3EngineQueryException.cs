using System;
using GameEngineQuery.Constants.Enumerations;

namespace GameEngineQuery.Exceptions
{
    public class Quake3EngineQueryException : Exception
    {
        internal Quake3EngineQueryException(ExceptionType exceptionType, Exception ex) : base(ModifyMessage(exceptionType, ex.Message), ex)
        {
        }

        internal Quake3EngineQueryException(ExceptionType exceptionType) : base(CreateMessage(exceptionType))
        {
        }

        private static string CreateMessage(ExceptionType exceptionType)
        {
            switch (exceptionType)
            {
                case ExceptionType.SocketSend:
                    return "Could not send GetStatus packet to server.";
                case ExceptionType.SocketReceive:
                    return "Could not receive GetStatus packet from server.";
                case ExceptionType.CouldNotInitiateGetStatusRequest:
                    return "Could not initiate GetStatus request.";
                case ExceptionType.InvalidResponsePacketForGetStatusRequest:
                    return "Invalid response packet received for GetStatus request call.";
                case ExceptionType.InvalidResponseChallengeForGetStatusRequest:
                    return "Invalid response challenge received for GetStatus request call.";
            }

            return string.Empty;
        }

        private static string ModifyMessage(ExceptionType exceptionType, string message)
        {
            switch (exceptionType)
            {
                case ExceptionType.SocketSend:
                    return $"Could not send GetStatus packet to server {{{message}}}.";
                case ExceptionType.SocketReceive:
                    return $"Could not receive GetStatus packet from server {{{message}}}.";
                case ExceptionType.CouldNotInitiateGetStatusRequest:
                    return $"Could not initiate GetStatus request {{{message}}}.";
                case ExceptionType.InvalidResponsePacketForGetStatusRequest:
                    return $"Invalid response packet received for GetStatus request call {{{message}}}.";
                case ExceptionType.InvalidResponseChallengeForGetStatusRequest:
                    return $"Invalid response challenge received for GetStatus request call {{{message}}}.";
            }

            return message;
        }
    }
}