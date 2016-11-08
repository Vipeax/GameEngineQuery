﻿using System;
using System.Linq;
using System.Text;

namespace GameEngineQuery.Extensions
{
    internal static class ByteArrayExtensions
    {
        internal static string GetNextString(this byte[] buffer, ref int i)
        {
            var result = Encoding.UTF8.GetString(buffer.Skip(i).TakeWhile(b => b != 0x00).ToArray());
            i += Encoding.UTF8.GetByteCount(result) + 1;
            return result;
        }

        internal static ushort GetNextUShort(this byte[] buffer, ref int i)
        {
            var result = BitConverter.ToUInt16(buffer, i);

            i += sizeof(ushort);

            return result;
        }

        internal static Version GetVersion(this byte[] buffer, ref int i)
        {
            try
            {
               return new Version(buffer.GetNextString(ref i));
            }
            catch (FormatException)
            {
                return new Version(0, 0, 0, 0);
            }
            catch (ArgumentException)
            {
                return new Version(0, 0, 0, 0);
            }
        }
    }
}