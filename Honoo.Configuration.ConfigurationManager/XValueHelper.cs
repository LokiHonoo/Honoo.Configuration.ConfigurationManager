using System;

namespace Honoo.Configuration
{
    internal static class XValueHelper
    {
        internal static byte[] Parse(string hex)
        {
            if (!string.IsNullOrWhiteSpace(hex))
            {
                byte[] result = new byte[hex.Length / 2];
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
                }
                return result;
            }
#if NETSTANDARD2_0_OR_GREATER
            return Array.Empty<byte>();
#else
            return new byte[0];
#endif
        }

        internal static bool TryParse(string hex, out byte[] value)
        {
            if (!string.IsNullOrWhiteSpace(hex))
            {
                try
                {
                    byte[] result = new byte[hex.Length / 2];
                    for (int i = 0; i < result.Length; i++)
                    {
                        result[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
                    }
                    value = result;
                    return true;
                }
                catch
                {
                }
            }
            value = default;
            return false;
        }
    }
}