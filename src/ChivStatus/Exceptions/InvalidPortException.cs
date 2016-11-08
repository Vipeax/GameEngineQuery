using System;

namespace ChivStatus.Exceptions
{
    public class InvalidPortException : FormatException
    {
    }

    public class InvalidIpAddressException : FormatException
    {
    }

    public class InvalidAddressFormatException : FormatException
    {
    }

    public class QueryExecutorInitializationException : InvalidOperationException
    {
    }
}
