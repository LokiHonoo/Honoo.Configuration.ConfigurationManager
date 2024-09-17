using System;
using System.Collections.Generic;
using System.Globalization;

namespace Honoo.Configuration
{
    internal static class XValueHelper
    {
        #region Convert 1

        internal static string[] Convert(IList<bool> values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }
            List<string> result = new List<string>();
            foreach (bool value in values)
            {
                result.Add(value.ToString());
            }
            return result.ToArray();
        }

        internal static string[] Convert(IList<sbyte> values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }
            List<string> result = new List<string>();
            foreach (sbyte value in values)
            {
                result.Add(value.ToString(CultureInfo.InvariantCulture));
            }
            return result.ToArray();
        }

        internal static string[] Convert(IList<byte> values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }
            List<string> result = new List<string>();
            foreach (byte value in values)
            {
                result.Add(value.ToString(CultureInfo.InvariantCulture));
            }
            return result.ToArray();
        }

        internal static string[] Convert(IList<short> values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }
            List<string> result = new List<string>();
            foreach (short value in values)
            {
                result.Add(value.ToString(CultureInfo.InvariantCulture));
            }
            return result.ToArray();
        }

        internal static string[] Convert(IList<ushort> values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }
            List<string> result = new List<string>();
            foreach (ushort value in values)
            {
                result.Add(value.ToString(CultureInfo.InvariantCulture));
            }
            return result.ToArray();
        }

        internal static string[] Convert(IList<int> values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }
            List<string> result = new List<string>();
            foreach (int value in values)
            {
                result.Add(value.ToString(CultureInfo.InvariantCulture));
            }
            return result.ToArray();
        }

        internal static string[] Convert(IList<uint> values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }
            List<string> result = new List<string>();
            foreach (uint value in values)
            {
                result.Add(value.ToString(CultureInfo.InvariantCulture));
            }
            return result.ToArray();
        }

        internal static string[] Convert(IList<long> values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }
            List<string> result = new List<string>();
            foreach (long value in values)
            {
                result.Add(value.ToString(CultureInfo.InvariantCulture));
            }
            return result.ToArray();
        }

        internal static string[] Convert(IList<ulong> values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }
            List<string> result = new List<string>();
            foreach (ulong value in values)
            {
                result.Add(value.ToString(CultureInfo.InvariantCulture));
            }
            return result.ToArray();
        }

        internal static string[] Convert(IList<float> values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }
            List<string> result = new List<string>();
            foreach (float value in values)
            {
                result.Add(value.ToString(CultureInfo.InvariantCulture));
            }
            return result.ToArray();
        }

        internal static string[] Convert(IList<double> values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }
            List<string> result = new List<string>();
            foreach (double value in values)
            {
                result.Add(value.ToString(CultureInfo.InvariantCulture));
            }
            return result.ToArray();
        }

        internal static string[] Convert(IList<decimal> values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }
            List<string> result = new List<string>();
            foreach (decimal value in values)
            {
                result.Add(value.ToString(CultureInfo.InvariantCulture));
            }
            return result.ToArray();
        }

        internal static string[] Convert(IList<char> values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }
            List<string> result = new List<string>();
            foreach (char value in values)
            {
                result.Add(value.ToString());
            }
            return result.ToArray();
        }

        internal static string[] Convert(IList<Binaries> values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }
            List<string> result = new List<string>();
            foreach (Binaries value in values)
            {
                result.Add(value.Hex);
            }
            return result.ToArray();
        }

        internal static string[] Convert<TEnum>(IList<TEnum> values) where TEnum : Enum
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }
            List<string> result = new List<string>();
            foreach (TEnum value in values)
            {
                result.Add(value.ToString());
            }
            return result.ToArray();
        }

        #endregion Convert 1

        #region Convert 2

        internal static string[][] Convert(bool[][] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }
            List<string[]> result = new List<string[]>();
            foreach (bool[] value in values)
            {
                List<string> res = new List<string>();
                foreach (bool val in value)
                {
                    res.Add(val.ToString());
                }
                result.Add(res.ToArray());
            }
            return result.ToArray();
        }

        internal static string[][] Convert(sbyte[][] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }
            List<string[]> result = new List<string[]>();
            foreach (sbyte[] value in values)
            {
                List<string> res = new List<string>();
                foreach (sbyte val in value)
                {
                    res.Add(val.ToString(CultureInfo.InvariantCulture));
                }
                result.Add(res.ToArray());
            }
            return result.ToArray();
        }

        internal static string[][] Convert(byte[][] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }
            List<string[]> result = new List<string[]>();
            foreach (byte[] value in values)
            {
                List<string> res = new List<string>();
                foreach (byte val in value)
                {
                    res.Add(val.ToString(CultureInfo.InvariantCulture));
                }
                result.Add(res.ToArray());
            }
            return result.ToArray();
        }

        internal static string[][] Convert(short[][] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }
            List<string[]> result = new List<string[]>();
            foreach (short[] value in values)
            {
                List<string> res = new List<string>();
                foreach (short val in value)
                {
                    res.Add(val.ToString(CultureInfo.InvariantCulture));
                }
                result.Add(res.ToArray());
            }
            return result.ToArray();
        }

        internal static string[][] Convert(ushort[][] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }
            List<string[]> result = new List<string[]>();
            foreach (ushort[] value in values)
            {
                List<string> res = new List<string>();
                foreach (ushort val in value)
                {
                    res.Add(val.ToString(CultureInfo.InvariantCulture));
                }
                result.Add(res.ToArray());
            }
            return result.ToArray();
        }

        internal static string[][] Convert(int[][] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }
            List<string[]> result = new List<string[]>();
            foreach (int[] value in values)
            {
                List<string> res = new List<string>();
                foreach (int val in value)
                {
                    res.Add(val.ToString(CultureInfo.InvariantCulture));
                }
                result.Add(res.ToArray());
            }
            return result.ToArray();
        }

        internal static string[][] Convert(uint[][] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }
            List<string[]> result = new List<string[]>();
            foreach (uint[] value in values)
            {
                List<string> res = new List<string>();
                foreach (uint val in value)
                {
                    res.Add(val.ToString(CultureInfo.InvariantCulture));
                }
                result.Add(res.ToArray());
            }
            return result.ToArray();
        }

        internal static string[][] Convert(long[][] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }
            List<string[]> result = new List<string[]>();
            foreach (long[] value in values)
            {
                List<string> res = new List<string>();
                foreach (long val in value)
                {
                    res.Add(val.ToString(CultureInfo.InvariantCulture));
                }
                result.Add(res.ToArray());
            }
            return result.ToArray();
        }

        internal static string[][] Convert(ulong[][] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }
            List<string[]> result = new List<string[]>();
            foreach (ulong[] value in values)
            {
                List<string> res = new List<string>();
                foreach (ulong val in value)
                {
                    res.Add(val.ToString(CultureInfo.InvariantCulture));
                }
                result.Add(res.ToArray());
            }
            return result.ToArray();
        }

        internal static string[][] Convert(float[][] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }
            List<string[]> result = new List<string[]>();
            foreach (float[] value in values)
            {
                List<string> res = new List<string>();
                foreach (float val in value)
                {
                    res.Add(val.ToString(CultureInfo.InvariantCulture));
                }
                result.Add(res.ToArray());
            }
            return result.ToArray();
        }

        internal static string[][] Convert(double[][] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }
            List<string[]> result = new List<string[]>();
            foreach (double[] value in values)
            {
                List<string> res = new List<string>();
                foreach (double val in value)
                {
                    res.Add(val.ToString(CultureInfo.InvariantCulture));
                }
                result.Add(res.ToArray());
            }
            return result.ToArray();
        }

        internal static string[][] Convert(decimal[][] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }
            List<string[]> result = new List<string[]>();
            foreach (decimal[] value in values)
            {
                List<string> res = new List<string>();
                foreach (decimal val in value)
                {
                    res.Add(val.ToString(CultureInfo.InvariantCulture));
                }
                result.Add(res.ToArray());
            }
            return result.ToArray();
        }

        internal static string[][] Convert(char[][] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }
            List<string[]> result = new List<string[]>();
            foreach (char[] value in values)
            {
                List<string> res = new List<string>();
                foreach (char val in value)
                {
                    res.Add(val.ToString());
                }
                result.Add(res.ToArray());
            }
            return result.ToArray();
        }

        internal static string[][] Convert(Binaries[][] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }
            List<string[]> result = new List<string[]>();
            foreach (Binaries[] value in values)
            {
                List<string> res = new List<string>();
                foreach (Binaries val in value)
                {
                    res.Add(val.ToString());
                }
                result.Add(res.ToArray());
            }
            return result.ToArray();
        }

        internal static string[][] Convert<TEnum>(TEnum[][] values) where TEnum : Enum
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }
            List<string[]> result = new List<string[]>();
            foreach (TEnum[] value in values)
            {
                List<string> res = new List<string>();
                foreach (TEnum val in value)
                {
                    res.Add(val.ToString());
                }
                result.Add(res.ToArray());
            }
            return result.ToArray();
        }

        #endregion Convert 2

        #region Convert 3

        internal static string[][][] Convert(bool[][][] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }
            List<string[][]> resultX = new List<string[][]>();
            foreach (bool[][] value in values)
            {
                List<string[]> result = new List<string[]>();
                foreach (bool[] val in value)
                {
                    List<string> res = new List<string>();
                    foreach (bool v in val)
                    {
                        res.Add(v.ToString());
                    }
                    result.Add(res.ToArray());
                }
                resultX.Add(result.ToArray());
            }
            return resultX.ToArray();
        }

        internal static string[][][] Convert(sbyte[][][] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }
            List<string[][]> resultX = new List<string[][]>();
            foreach (sbyte[][] value in values)
            {
                List<string[]> result = new List<string[]>();
                foreach (sbyte[] val in value)
                {
                    List<string> res = new List<string>();
                    foreach (sbyte v in val)
                    {
                        res.Add(v.ToString(CultureInfo.InvariantCulture));
                    }
                    result.Add(res.ToArray());
                }
                resultX.Add(result.ToArray());
            }
            return resultX.ToArray();
        }

        internal static string[][][] Convert(byte[][][] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }
            List<string[][]> resultX = new List<string[][]>();
            foreach (byte[][] value in values)
            {
                List<string[]> result = new List<string[]>();
                foreach (byte[] val in value)
                {
                    List<string> res = new List<string>();
                    foreach (byte v in val)
                    {
                        res.Add(v.ToString(CultureInfo.InvariantCulture));
                    }
                    result.Add(res.ToArray());
                }
                resultX.Add(result.ToArray());
            }
            return resultX.ToArray();
        }

        internal static string[][][] Convert(short[][][] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }
            List<string[][]> resultX = new List<string[][]>();
            foreach (short[][] value in values)
            {
                List<string[]> result = new List<string[]>();
                foreach (short[] val in value)
                {
                    List<string> res = new List<string>();
                    foreach (short v in val)
                    {
                        res.Add(v.ToString(CultureInfo.InvariantCulture));
                    }
                    result.Add(res.ToArray());
                }
                resultX.Add(result.ToArray());
            }
            return resultX.ToArray();
        }

        internal static string[][][] Convert(ushort[][][] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }
            List<string[][]> resultX = new List<string[][]>();
            foreach (ushort[][] value in values)
            {
                List<string[]> result = new List<string[]>();
                foreach (ushort[] val in value)
                {
                    List<string> res = new List<string>();
                    foreach (ushort v in val)
                    {
                        res.Add(v.ToString(CultureInfo.InvariantCulture));
                    }
                    result.Add(res.ToArray());
                }
                resultX.Add(result.ToArray());
            }
            return resultX.ToArray();
        }

        internal static string[][][] Convert(int[][][] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }
            List<string[][]> resultX = new List<string[][]>();
            foreach (int[][] value in values)
            {
                List<string[]> result = new List<string[]>();
                foreach (int[] val in value)
                {
                    List<string> res = new List<string>();
                    foreach (int v in val)
                    {
                        res.Add(v.ToString(CultureInfo.InvariantCulture));
                    }
                    result.Add(res.ToArray());
                }
                resultX.Add(result.ToArray());
            }
            return resultX.ToArray();
        }

        internal static string[][][] Convert(uint[][][] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }
            List<string[][]> resultX = new List<string[][]>();
            foreach (uint[][] value in values)
            {
                List<string[]> result = new List<string[]>();
                foreach (uint[] val in value)
                {
                    List<string> res = new List<string>();
                    foreach (uint v in val)
                    {
                        res.Add(v.ToString(CultureInfo.InvariantCulture));
                    }
                    result.Add(res.ToArray());
                }
                resultX.Add(result.ToArray());
            }
            return resultX.ToArray();
        }

        internal static string[][][] Convert(long[][][] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }
            List<string[][]> resultX = new List<string[][]>();
            foreach (long[][] value in values)
            {
                List<string[]> result = new List<string[]>();
                foreach (long[] val in value)
                {
                    List<string> res = new List<string>();
                    foreach (long v in val)
                    {
                        res.Add(v.ToString(CultureInfo.InvariantCulture));
                    }
                    result.Add(res.ToArray());
                }
                resultX.Add(result.ToArray());
            }
            return resultX.ToArray();
        }

        internal static string[][][] Convert(ulong[][][] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }
            List<string[][]> resultX = new List<string[][]>();
            foreach (ulong[][] value in values)
            {
                List<string[]> result = new List<string[]>();
                foreach (ulong[] val in value)
                {
                    List<string> res = new List<string>();
                    foreach (ulong v in val)
                    {
                        res.Add(v.ToString(CultureInfo.InvariantCulture));
                    }
                    result.Add(res.ToArray());
                }
                resultX.Add(result.ToArray());
            }
            return resultX.ToArray();
        }

        internal static string[][][] Convert(float[][][] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }
            List<string[][]> resultX = new List<string[][]>();
            foreach (float[][] value in values)
            {
                List<string[]> result = new List<string[]>();
                foreach (float[] val in value)
                {
                    List<string> res = new List<string>();
                    foreach (float v in val)
                    {
                        res.Add(v.ToString(CultureInfo.InvariantCulture));
                    }
                    result.Add(res.ToArray());
                }
                resultX.Add(result.ToArray());
            }
            return resultX.ToArray();
        }

        internal static string[][][] Convert(double[][][] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }
            List<string[][]> resultX = new List<string[][]>();
            foreach (double[][] value in values)
            {
                List<string[]> result = new List<string[]>();
                foreach (double[] val in value)
                {
                    List<string> res = new List<string>();
                    foreach (double v in val)
                    {
                        res.Add(v.ToString(CultureInfo.InvariantCulture));
                    }
                    result.Add(res.ToArray());
                }
                resultX.Add(result.ToArray());
            }
            return resultX.ToArray();
        }

        internal static string[][][] Convert(decimal[][][] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }
            List<string[][]> resultX = new List<string[][]>();
            foreach (decimal[][] value in values)
            {
                List<string[]> result = new List<string[]>();
                foreach (decimal[] val in value)
                {
                    List<string> res = new List<string>();
                    foreach (decimal v in val)
                    {
                        res.Add(v.ToString(CultureInfo.InvariantCulture));
                    }
                    result.Add(res.ToArray());
                }
                resultX.Add(result.ToArray());
            }
            return resultX.ToArray();
        }

        internal static string[][][] Convert(char[][][] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }
            List<string[][]> resultX = new List<string[][]>();
            foreach (char[][] value in values)
            {
                List<string[]> result = new List<string[]>();
                foreach (char[] val in value)
                {
                    List<string> res = new List<string>();
                    foreach (char v in val)
                    {
                        res.Add(v.ToString());
                    }
                    result.Add(res.ToArray());
                }
                resultX.Add(result.ToArray());
            }
            return resultX.ToArray();
        }

        internal static string[][][] Convert(Binaries[][][] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }
            List<string[][]> resultX = new List<string[][]>();
            foreach (Binaries[][] value in values)
            {
                List<string[]> result = new List<string[]>();
                foreach (Binaries[] val in value)
                {
                    List<string> res = new List<string>();
                    foreach (Binaries v in val)
                    {
                        res.Add(v.Hex);
                    }
                    result.Add(res.ToArray());
                }
                resultX.Add(result.ToArray());
            }
            return resultX.ToArray();
        }

        internal static string[][][] Convert<TEnum>(TEnum[][][] values) where TEnum : Enum
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }
            List<string[][]> resultX = new List<string[][]>();
            foreach (TEnum[][] value in values)
            {
                List<string[]> result = new List<string[]>();
                foreach (TEnum[] val in value)
                {
                    List<string> res = new List<string>();
                    foreach (TEnum v in val)
                    {
                        res.Add(v.ToString());
                    }
                    result.Add(res.ToArray());
                }
                resultX.Add(result.ToArray());
            }
            return resultX.ToArray();
        }

        #endregion Convert 3

        internal static byte[] Parse(string hex)
        {
            if (!string.IsNullOrWhiteSpace(hex))
            {
                byte[] result = new byte[hex.Length / 2];
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = System.Convert.ToByte(hex.Substring(i * 2, 2), 16);
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
                        result[i] = System.Convert.ToByte(hex.Substring(i * 2, 2), 16);
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