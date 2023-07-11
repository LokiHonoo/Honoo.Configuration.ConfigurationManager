using System;
using System.Collections.Generic;

namespace Honoo.Configuration
{
    internal static class XValueHelper
    {
        internal static IList<byte[]> Copy(IList<byte[]> values)
        {
            if (values != null)
            {
                List<byte[]> result = new List<byte[]>();
                foreach (var value in values)
                {
                    result.Add((byte[])value.Clone());
                }
                return result;
            }
            return null;
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
            value = null;
            return false;
        }
    }
}