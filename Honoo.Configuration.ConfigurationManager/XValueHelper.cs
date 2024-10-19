using System;

namespace Honoo.Configuration
{
    internal static class XValueHelper
    {
        internal static byte[] Parse(string hex)
        {
            if (!string.IsNullOrWhiteSpace(hex))
            {
                hex = hex.Replace("-", null);
                if (hex.Length % 2 > 0)
                {
                    hex = hex.PadLeft(hex.Length + 1, '0');
                }
                byte[] result = new byte[hex.Length / 2];
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
                }
                return result;
            }
#if NET40
            return new byte[0];
#else
            return Array.Empty<byte>();
#endif
        }

        internal static bool TryParse(string hex, out byte[] value)
        {
            if (!string.IsNullOrWhiteSpace(hex))
            {
                try
                {
                    hex = hex.Replace("-", null);
                    if (hex.Length % 2 > 0)
                    {
                        hex = hex.PadLeft(hex.Length + 1, '0');
                    }
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