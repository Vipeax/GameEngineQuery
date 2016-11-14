using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace GameEngineQuery.Extensions
{
    internal static class ByteArrayExtensions
    {
        internal static string GetNextString(this byte[] buffer, ref int i, int skip = 0)
        {
            var result = Encoding.UTF8.GetString(buffer.Skip(i).TakeWhile(b => b != 0x00).ToArray());
            i += Encoding.UTF8.GetByteCount(result) + 1 + skip;
            return result;
        }
        
        internal static ushort GetNextUShort(this byte[] buffer, ref int i)
        {
            var result = BitConverter.ToUInt16(buffer, i);

            i += sizeof(ushort);

            return result;
        }
        
        internal static T GetNextValueOfType<T>(this byte[] buffer, ref int i) where T : struct
        {
            int j = 0;
            int size = Marshal.SizeOf<T>();
            var valueBuffer = buffer.Skip(i).TakeWhile(b => j++ != size).ToArray();

            i += size;

            if (typeof(T) == typeof(int))
            {
                return (T) Convert.ChangeType(BitConverter.ToInt32(valueBuffer, 0), typeof(T));
            }

            if (typeof(T) == typeof(float))
            {
                return (T)Convert.ChangeType(BitConverter.ToSingle(valueBuffer, 0), typeof(T));
            }

            if (typeof(T) == typeof(uint))
            {
                return (T)Convert.ChangeType(BitConverter.ToUInt32(valueBuffer, 0), typeof(T));
            }

            if (typeof(T) == typeof(long))
            {
                return (T) Convert.ChangeType(BitConverter.ToInt64(valueBuffer, 0), typeof(T));
            }

            return default(T);
        }

        internal static T[] GetNextArrayOfType<T>(this byte[] buffer, int count, ref int i) where T : struct
        {
            int j = 0;
            var result = buffer.Skip(i).TakeWhile(b => j++ != count).Select(ConvertValue<T, byte>).ToArray();
            i += result.Length;
            return result;
        }

        internal static Version GetVersion(this byte[] buffer, ref int i)
        {
            try
            {
               var version = new Version(buffer.GetNextString(ref i));

                return new Version(version.Major >= 0 ? version.Major : 0, version.Minor >= 0 ? version.Minor : 0, version.Build >= 0 ? version.Build : 0, version.Revision >= 0 ? version.Revision : 0);
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

        public static T ConvertValue<T, U>(U value) where U : IConvertible
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}