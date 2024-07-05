using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 配置属性。
    /// </summary>
    public sealed class HonooProperty
    {
        private readonly XElement _content;
        private readonly bool _isArray;
        private readonly string _key;
        private readonly object _value;
        private XComment _comment;

        /// <summary>
        /// 获取一个的值，指示配置属性是否是数组。
        /// </summary>
        public bool IsArray => _isArray;

        /// <summary>
        /// 获取配置属性的键。
        /// </summary>
        public string Key => _key;

        /// <summary>
        /// 获取配置属性的值。值可能是 <see cref="string"/> 或 <see cref="string"/> 数组。
        /// </summary>
        public object Value => _value;

        internal XComment Comment => _comment;
        internal XElement Content => _content;

        #region Construction

        /// <summary>
        /// 创建 HonooProperty 的新实例。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        public HonooProperty(string key, string value)
        {
            _content = GetElement(key, value);
            _comment = null;
            _key = key ?? throw new ArgumentNullException(nameof(key));
            _value = value ?? throw new ArgumentNullException(nameof(value));
            _isArray = false;
        }

        /// <summary>
        /// 创建 HonooProperty 的新实例。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        public HonooProperty(string key, IList<string> values)
        {
            _content = GetElement(key, values);
            _comment = null;
            _key = key ?? throw new ArgumentNullException(nameof(key));
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }
            _value = values.ToArray();
            _isArray = true;
        }

        internal HonooProperty(XElement content, XComment comment)
        {
            _content = content;
            _comment = comment;
            _key = content.Attribute("key").Value;
            if (content.Attribute("value") is XAttribute attribute)
            {
                _value = attribute.Value;
                _isArray = false;
            }
            else
            {
                List<string> values = new List<string>();
                foreach (var value in content.Elements(HonooSettingsManager.Namespace + "value"))
                {
                    values.Add(value.Value);
                }
                _value = values.ToArray();
                _isArray = true;
            }
        }

        #endregion Construction

        #region TryGetValue

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        public bool TryGetValue(out string value)
        {
            if (!_isArray)
            {
                value = (string)_value;
                return true;
            }
            value = default;
            return false;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        public bool TryGetValue(out bool value)
        {
            if (!_isArray)
            {
                return bool.TryParse((string)_value, out value);
            }
            value = default;
            return false;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        public bool TryGetValue(out sbyte value)
        {
            if (!_isArray)
            {
                return sbyte.TryParse((string)_value, out value);
            }
            value = default;
            return false;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        public bool TryGetValue(out byte value)
        {
            if (!_isArray)
            {
                return byte.TryParse((string)_value, out value);
            }
            value = default;
            return false;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        public bool TryGetValue(out short value)
        {
            if (!_isArray)
            {
                return short.TryParse((string)_value, out value);
            }
            value = default;
            return false;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        public bool TryGetValue(out ushort value)
        {
            if (!_isArray)
            {
                return ushort.TryParse((string)_value, out value);
            }
            value = default;
            return false;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        public bool TryGetValue(out int value)
        {
            if (!_isArray)
            {
                return int.TryParse((string)_value, out value);
            }
            value = default;
            return false;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        public bool TryGetValue(out uint value)
        {
            if (!_isArray)
            {
                return uint.TryParse((string)_value, out value);
            }
            value = default;
            return false;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        public bool TryGetValue(out long value)
        {
            if (!_isArray)
            {
                return long.TryParse((string)_value, out value);
            }
            value = default;
            return false;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        public bool TryGetValue(out ulong value)
        {
            if (!_isArray)
            {
                return ulong.TryParse((string)_value, out value);
            }
            value = default;
            return false;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        public bool TryGetValue(out float value)
        {
            if (!_isArray)
            {
                return float.TryParse((string)_value, out value);
            }
            value = default;
            return false;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        public bool TryGetValue(out double value)
        {
            if (!_isArray)
            {
                return double.TryParse((string)_value, out value);
            }
            value = default;
            return false;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        public bool TryGetValue(out decimal value)
        {
            if (!_isArray)
            {
                return decimal.TryParse((string)_value, out value);
            }
            value = default;
            return false;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        public bool TryGetValue(out char value)
        {
            if (!_isArray)
            {
                return char.TryParse((string)_value, out value);
            }
            value = default;
            return false;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        public bool TryGetValue(out byte[] value)
        {
            if (!_isArray)
            {
                return XValueHelper.TryParse((string)_value, out value);
            }
            value = default;
            return false;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        public bool TryGetValue<TEnum>(out TEnum value) where TEnum : struct
        {
            if (!_isArray && typeof(TEnum).BaseType.FullName == "System.Enum")
            {
                return Enum.TryParse((string)_value, false, out value);
            }
            value = default;
            return false;
        }

        #endregion TryGetValue

        #region TryGetArrayValue

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetArrayValue(out string[] values)
        {
            if (_isArray)
            {
                values = (string[])_value;
                return true;
            }
            values = default;
            return false;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetArrayValue(out bool[] values)
        {
            if (_isArray)
            {
                List<bool> result = new List<bool>();
                foreach (string value in (string[])_value)
                {
                    if (bool.TryParse(value, out bool val))
                    {
                        result.Add(val);
                    }
                    else
                    {
                        values = default;
                        return false;
                    }
                }
                values = result.ToArray();
                return true;
            }
            values = default;
            return false;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetArrayValue(out sbyte[] values)
        {
            if (_isArray)
            {
                List<sbyte> result = new List<sbyte>();
                foreach (string value in (string[])_value)
                {
                    if (sbyte.TryParse(value, out sbyte val))
                    {
                        result.Add(val);
                    }
                    else
                    {
                        values = default;
                        return false;
                    }
                }
                values = result.ToArray();
                return true;
            }
            values = default;
            return false;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetArrayValue(out byte[] values)
        {
            if (_isArray)
            {
                List<byte> result = new List<byte>();
                foreach (string value in (string[])_value)
                {
                    if (byte.TryParse(value, out byte val))
                    {
                        result.Add(val);
                    }
                    else
                    {
                        values = default;
                        return false;
                    }
                }
                values = result.ToArray();
                return true;
            }
            values = default;
            return false;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetArrayValue(out short[] values)
        {
            if (_isArray)
            {
                List<short> result = new List<short>();
                foreach (string value in (string[])_value)
                {
                    if (short.TryParse(value, out short val))
                    {
                        result.Add(val);
                    }
                    else
                    {
                        values = default;
                        return false;
                    }
                }
                values = result.ToArray();
                return true;
            }
            values = default;
            return false;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetArrayValue(out ushort[] values)
        {
            if (_isArray)
            {
                List<ushort> result = new List<ushort>();
                foreach (string value in (string[])_value)
                {
                    if (ushort.TryParse(value, out ushort val))
                    {
                        result.Add(val);
                    }
                    else
                    {
                        values = default;
                        return false;
                    }
                }
                values = result.ToArray();
                return true;
            }
            values = default;
            return false;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetArrayValue(out int[] values)
        {
            if (_isArray)
            {
                List<int> result = new List<int>();
                foreach (string value in (string[])_value)
                {
                    if (int.TryParse(value, out int val))
                    {
                        result.Add(val);
                    }
                    else
                    {
                        values = default;
                        return false;
                    }
                }
                values = result.ToArray();
                return true;
            }
            values = default;
            return false;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetArrayValue(out uint[] values)
        {
            if (_isArray)
            {
                List<uint> result = new List<uint>();
                foreach (string value in (string[])_value)
                {
                    if (uint.TryParse(value, out uint val))
                    {
                        result.Add(val);
                    }
                    else
                    {
                        values = default;
                        return false;
                    }
                }
                values = result.ToArray();
                return true;
            }
            values = default;
            return false;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetArrayValue(out long[] values)
        {
            if (_isArray)
            {
                List<long> result = new List<long>();
                foreach (string value in (string[])_value)
                {
                    if (long.TryParse(value, out long val))
                    {
                        result.Add(val);
                    }
                    else
                    {
                        values = default;
                        return false;
                    }
                }
                values = result.ToArray();
                return true;
            }
            values = default;
            return false;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetArrayValue(out ulong[] values)
        {
            if (_isArray)
            {
                List<ulong> result = new List<ulong>();
                foreach (string value in (string[])_value)
                {
                    if (ulong.TryParse(value, out ulong val))
                    {
                        result.Add(val);
                    }
                    else
                    {
                        values = default;
                        return false;
                    }
                }
                values = result.ToArray();
                return true;
            }
            values = default;
            return false;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetArrayValue(out float[] values)
        {
            if (_isArray)
            {
                List<float> result = new List<float>();
                foreach (string value in (string[])_value)
                {
                    if (float.TryParse(value, out float val))
                    {
                        result.Add(val);
                    }
                    else
                    {
                        values = default;
                        return false;
                    }
                }
                values = result.ToArray();
                return true;
            }
            values = default;
            return false;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetArrayValue(out double[] values)
        {
            if (_isArray)
            {
                List<double> result = new List<double>();
                foreach (string value in (string[])_value)
                {
                    if (double.TryParse(value, out double val))
                    {
                        result.Add(val);
                    }
                    else
                    {
                        values = default;
                        return false;
                    }
                }
                values = result.ToArray();
                return true;
            }
            values = default;
            return false;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetArrayValue(out decimal[] values)
        {
            if (_isArray)
            {
                List<decimal> result = new List<decimal>();
                foreach (string value in (string[])_value)
                {
                    if (decimal.TryParse(value, out decimal val))
                    {
                        result.Add(val);
                    }
                    else
                    {
                        values = default;
                        return false;
                    }
                }
                values = result.ToArray();
                return true;
            }
            values = default;
            return false;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetArrayValue(out char[] values)
        {
            if (_isArray)
            {
                List<char> result = new List<char>();
                foreach (string value in (string[])_value)
                {
                    if (char.TryParse(value, out char val))
                    {
                        result.Add(val);
                    }
                    else
                    {
                        values = default;
                        return false;
                    }
                }
                values = result.ToArray();
                return true;
            }
            values = default;
            return false;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetArrayValue(out byte[][] values)
        {
            if (_isArray)
            {
                List<byte[]> result = new List<byte[]>();
                foreach (string value in (string[])_value)
                {
                    if (XValueHelper.TryParse(value, out byte[] val))
                    {
                        result.Add(val);
                    }
                    else
                    {
                        values = default;
                        return false;
                    }
                }
                values = result.ToArray();
                return true;
            }
            values = default;
            return false;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetArrayValue<TEnum>(out TEnum[] values) where TEnum : struct
        {
            if (_isArray && typeof(TEnum).BaseType.FullName == "System.Enum")
            {
                List<TEnum> result = new List<TEnum>();
                foreach (string value in (string[])_value)
                {
                    if (Enum.TryParse(value, false, out TEnum val))
                    {
                        result.Add(val);
                    }
                    else
                    {
                        values = default;
                        return false;
                    }
                }
                values = result.ToArray();
                return true;
            }
            values = default;
            return false;
        }

        #endregion TryGetArrayValue

        #region GetValue

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="defaultValue">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public string GetValue(string defaultValue)
        {
            return TryGetValue(out string value) ? value : defaultValue;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="defaultValue">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool GetValue(bool defaultValue)
        {
            return TryGetValue(out bool value) ? value : defaultValue;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="defaultValue">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public sbyte GetValue(sbyte defaultValue)
        {
            return TryGetValue(out sbyte value) ? value : defaultValue;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="defaultValue">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public byte GetValue(byte defaultValue)
        {
            return TryGetValue(out byte value) ? value : defaultValue;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="defaultValue">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public short GetValue(short defaultValue)
        {
            return TryGetValue(out short value) ? value : defaultValue;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="defaultValue">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public ushort GetValue(ushort defaultValue)
        {
            return TryGetValue(out ushort value) ? value : defaultValue;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="defaultValue">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public int GetValue(int defaultValue)
        {
            return TryGetValue(out int value) ? value : defaultValue;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="defaultValue">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public uint GetValue(uint defaultValue)
        {
            return TryGetValue(out uint value) ? value : defaultValue;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="defaultValue">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public long GetValue(long defaultValue)
        {
            return TryGetValue(out long value) ? value : defaultValue;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="defaultValue">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public ulong GetValue(ulong defaultValue)
        {
            return TryGetValue(out ulong value) ? value : defaultValue;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="defaultValue">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public float GetValue(float defaultValue)
        {
            return TryGetValue(out float value) ? value : defaultValue;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="defaultValue">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public double GetValue(double defaultValue)
        {
            return TryGetValue(out double value) ? value : defaultValue;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="defaultValue">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public decimal GetValue(decimal defaultValue)
        {
            return TryGetValue(out decimal value) ? value : defaultValue;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="defaultValue">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public char GetValue(char defaultValue)
        {
            return TryGetValue(out char value) ? value : defaultValue;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="defaultValue">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public byte[] GetValue(byte[] defaultValue)
        {
            return TryGetValue(out byte[] value) ? value : defaultValue;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="defaultValue">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public TEnum GetValue<TEnum>(TEnum defaultValue) where TEnum : struct
        {
            return TryGetValue(out TEnum value) ? value : defaultValue;
        }

        #endregion GetValue

        #region GetArrayValue

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="defaultValues">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public string[] GetArrayValue(string[] defaultValues)
        {
            return TryGetArrayValue(out string[] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="defaultValues">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool[] GetArrayValue(bool[] defaultValues)
        {
            return TryGetArrayValue(out bool[] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="defaultValues">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public sbyte[] GetArrayValue(sbyte[] defaultValues)
        {
            return TryGetArrayValue(out sbyte[] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="defaultValues">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public byte[] GetArrayValue(byte[] defaultValues)
        {
            return TryGetArrayValue(out byte[] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="defaultValues">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public short[] GetArrayValue(short[] defaultValues)
        {
            return TryGetArrayValue(out short[] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="defaultValues">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public ushort[] GetArrayValue(ushort[] defaultValues)
        {
            return TryGetArrayValue(out ushort[] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="defaultValues">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public int[] GetArrayValue(int[] defaultValues)
        {
            return TryGetArrayValue(out int[] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="defaultValues">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public uint[] GetArrayValue(uint[] defaultValues)
        {
            return TryGetArrayValue(out uint[] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="defaultValues">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public long[] GetArrayValue(long[] defaultValues)
        {
            return TryGetArrayValue(out long[] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="defaultValues">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public ulong[] GetArrayValue(ulong[] defaultValues)
        {
            return TryGetArrayValue(out ulong[] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="defaultValues">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public float[] GetArrayValue(float[] defaultValues)
        {
            return TryGetArrayValue(out float[] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="defaultValues">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public double[] GetArrayValue(double[] defaultValues)
        {
            return TryGetArrayValue(out double[] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="defaultValues">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public decimal[] GetArrayValue(decimal[] defaultValues)
        {
            return TryGetArrayValue(out decimal[] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="defaultValues">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public char[] GetArrayValue(char[] defaultValues)
        {
            return TryGetArrayValue(out char[] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="defaultValues">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public byte[][] GetArrayValue(byte[][] defaultValues)
        {
            return TryGetArrayValue(out byte[][] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="defaultValues">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public TEnum[] GetArrayValue<TEnum>(TEnum[] defaultValues) where TEnum : struct
        {
            return TryGetArrayValue(out TEnum[] values) ? values : defaultValues;
        }

        #endregion GetArrayValue

        #region Comment

        /// <summary>
        /// 获取注释。如果没有找到注释，返回 <see langword="null"/>。
        /// </summary>
        /// <returns></returns>
        public string GetComment()
        {
            return TryGetComment(out string comment) ? comment : null;
        }

        /// <summary>
        /// 删除注释。如果注释成功删除，返回 <see langword="true"/>。如果没有找到注释节点，则返回 <see langword="false"/>。
        /// </summary>
        /// <returns></returns>
        public bool RemoveComment()
        {
            if (_comment != null)
            {
                _comment.Remove();
                _comment = null;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 添加或更新注释。
        /// </summary>
        /// <param name="comment">注释文本。</param>
        /// <exception cref="Exception"/>
        public void SetComment(string comment)
        {
            if (comment == null)
            {
                if (_comment != null)
                {
                    _comment.Remove();
                    _comment = null;
                }
            }
            else if (_comment == null)
            {
                _comment = new XComment(comment);
                _content.AddBeforeSelf(_comment);
            }
            else
            {
                _comment.Value = comment;
            }
        }

        /// <summary>
        /// 获取注释。如果没有找到注释，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="comment">注释文本。</param>
        /// <returns></returns>
        public bool TryGetComment(out string comment)
        {
            if (_comment != null)
            {
                comment = _comment.Value;
                return true;
            }
            comment = null;
            return false;
        }

        #endregion Comment

        /// <summary>
        /// 方法已重写。返回节点的缩进 XML 文本。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _content.ToString();
        }

        private static XElement GetElement(string key, string value)
        {
            XElement element = new XElement(HonooSettingsManager.Namespace + "property");
            element.SetAttributeValue("key", key);
            element.SetAttributeValue("value", value);
            return element;
        }

        private static XElement GetElement(string key, IList<string> values)
        {
            XElement element = new XElement(HonooSettingsManager.Namespace + "property");
            element.SetAttributeValue("key", key);
            foreach (string value in values)
            {
                element.Add(new XElement(HonooSettingsManager.Namespace + "value", value));
            }
            return element;
        }
    }
}