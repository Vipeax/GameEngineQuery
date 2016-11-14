using System;
using GameEngineQuery.Constants.Enumerations;

namespace GameEngineQuery.Exceptions
{
    public class SourceEngineQueryException : Exception
    {
        internal SourceEngineQueryException(ExceptionType exceptionType, Exception ex) : base(ModifyMessage(exceptionType, ex.Message), ex)
        {
        }

        internal SourceEngineQueryException(ExceptionType exceptionType) : base(CreateMessage(exceptionType))
        {
        }

        private static string CreateMessage(ExceptionType exceptionType)
        {
            switch (exceptionType)
            {
                case ExceptionType.SocketSend:
                    return "Could not send A2S packet to server.";
                case ExceptionType.SocketReceive:
                    return "Could not receive A2S packet from server.";
                case ExceptionType.CouldNotInitiateA2SInfoRequest:
                    return "Could not initiate A2SInfo request.";
                case ExceptionType.InvalidResponseHeaderForA2SInfoRequest:
                    return "Invalid response header received for A2S Info request call.";
                case ExceptionType.InvalidResponsePacketForA2SInfoRequest:
                    return "Invalid response packet received for A2S Player request call.";
                    case ExceptionType.CouldNotInitiateA2SPlayerRequest:
                    return "Could not initiate A2SPlayer request.";
                case ExceptionType.InvalidResponseHeaderForA2SPlayerRequest:
                    return "Invalid response header received for A2S Player request call.";
                case ExceptionType.InvalidResponsePacketForA2SPlayerRequest:
                    return "Invalid response packet received for A2S Player request call.";
            }

            return string.Empty;
        }

        private static string ModifyMessage(ExceptionType exceptionType, string message)
        {
            switch (exceptionType)
            {
                case ExceptionType.SocketSend:
                    return $"Could not send A2S packet to server {{{message}}}.";
                case ExceptionType.SocketReceive:
                    return $"Could not receive A2S packet from server {{{message}}}.";
                case ExceptionType.CouldNotInitiateA2SInfoRequest:
                    return $"Could not initiate A2SInfo request {{{message}}}.";
                case ExceptionType.InvalidResponseHeaderForA2SInfoRequest:
                    return $"Invalid response header received for A2S Info request call {{{message}}}.";
                case ExceptionType.InvalidResponsePacketForA2SInfoRequest:
                    return $"Invalid response packet received for A2S Info request call {{{message}}}.";
                case ExceptionType.CouldNotInitiateA2SPlayerRequest:
                    return $"Could not initiate A2SPlayer request {{{message}}}.";
                case ExceptionType.InvalidResponseHeaderForA2SPlayerRequest:
                    return $"Invalid response header received for A2S Player request call {{{message}}}.";
                case ExceptionType.InvalidResponsePacketForA2SPlayerRequest:
                    return $"Invalid response packet received for A2S Player request call {{{message}}}.";
            }

            return message;
        }
    }
}
