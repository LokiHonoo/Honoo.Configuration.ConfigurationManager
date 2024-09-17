using System;
using System.Collections.Generic;
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
        /// 获取配置属性的值。值可能是 <see cref="string"/>、 <see cref="string"/>[]、 <see cref="string"/>[][] ...。
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1721:属性名不应与 get 方法匹配", Justification = "<挂起>")]
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
        public HonooProperty(string key, string[] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }
            _content = GetElement(key, values);
            _comment = null;
            _key = key ?? throw new ArgumentNullException(nameof(key));
            _value = values;
            _isArray = true;
        }

        /// <summary>
        /// 创建 HonooProperty 的新实例。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        public HonooProperty(string key, string[][] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }
            _content = GetElement(key, values);
            _comment = null;
            _key = key ?? throw new ArgumentNullException(nameof(key));
            _value = values;
            _isArray = true;
        }

        /// <summary>
        /// 创建 HonooProperty 的新实例。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        public HonooProperty(string key, string[][][] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }
            _content = GetElement(key, values);
            _comment = null;
            _key = key ?? throw new ArgumentNullException(nameof(key));
            _value = values;
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
            else if (content.HasElements)
            {
                if (GetValue(content, out string[] values1))
                {
                    _value = values1;
                }
                else if (GetValue(content, out string[][] values2))
                {
                    _value = values2;
                }
                else if (GetValue(content, out string[][][] values3))
                {
                    _value = values3;
                }
                _isArray = true;
            }
            else
            {
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
        public bool TryGetValue(out Binaries value)
        {
            if (!_isArray)
            {
                if (XValueHelper.TryParse((string)_value, out byte[] val))
                {
                    value = new Binaries(val);
                    return true;
                }
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

        #region TryGetArrayValue1

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(out string[] values)
        {
            if (_isArray && _value is string[] valueX)
            {
                values = valueX;
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
        public bool TryGetValue(out bool[] values)
        {
            if (_isArray && _value is string[] valueX)
            {
                List<bool> result = new List<bool>();
                foreach (string value in valueX)
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
        public bool TryGetValue(out sbyte[] values)
        {
            if (_isArray && _value is string[] valueX)
            {
                List<sbyte> result = new List<sbyte>();
                foreach (string value in valueX)
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
        public bool TryGetValue(out byte[] values)
        {
            if (_isArray && _value is string[] valueX)
            {
                List<byte> result = new List<byte>();
                foreach (string value in valueX)
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
        public bool TryGetValue(out short[] values)
        {
            if (_isArray && _value is string[] valueX)
            {
                List<short> result = new List<short>();
                foreach (string value in valueX)
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
        public bool TryGetValue(out ushort[] values)
        {
            if (_isArray && _value is string[] valueX)
            {
                List<ushort> result = new List<ushort>();
                foreach (string value in valueX)
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
        public bool TryGetValue(out int[] values)
        {
            if (_isArray && _value is string[] valueX)
            {
                List<int> result = new List<int>();
                foreach (string value in valueX)
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
        public bool TryGetValue(out uint[] values)
        {
            if (_isArray && _value is string[] valueX)
            {
                List<uint> result = new List<uint>();
                foreach (string value in valueX)
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
        public bool TryGetValue(out long[] values)
        {
            if (_isArray && _value is string[] valueX)
            {
                List<long> result = new List<long>();
                foreach (string value in valueX)
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
        public bool TryGetValue(out ulong[] values)
        {
            if (_isArray && _value is string[] valueX)
            {
                List<ulong> result = new List<ulong>();
                foreach (string value in valueX)
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
        public bool TryGetValue(out float[] values)
        {
            if (_isArray && _value is string[] valueX)
            {
                List<float> result = new List<float>();
                foreach (string value in valueX)
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
        public bool TryGetValue(out double[] values)
        {
            if (_isArray && _value is string[] valueX)
            {
                List<double> result = new List<double>();
                foreach (string value in valueX)
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
        public bool TryGetValue(out decimal[] values)
        {
            if (_isArray && _value is string[] valueX)
            {
                List<decimal> result = new List<decimal>();
                foreach (string value in valueX)
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
        public bool TryGetValue(out char[] values)
        {
            if (_isArray && _value is string[] valueX)
            {
                List<char> result = new List<char>();
                foreach (string value in valueX)
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
        public bool TryGetValue(out Binaries[] values)
        {
            if (_isArray && _value is string[] valueX)
            {
                List<Binaries> result = new List<Binaries>();
                foreach (string value in valueX)
                {
                    if (XValueHelper.TryParse(value, out byte[] val))
                    {
                        result.Add(new Binaries(val));
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
        public bool TryGetValue<TEnum>(out TEnum[] values) where TEnum : struct
        {
            if (_isArray && _value is string[] valueX && typeof(TEnum).BaseType.FullName == "System.Enum")
            {
                List<TEnum> result = new List<TEnum>();
                foreach (string value in valueX)
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

        #endregion TryGetArrayValue1

        #region TryGetArrayValue2

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(out string[][] values)
        {
            if (_isArray && _value is string[][] valueX)
            {
                values = valueX;
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
        public bool TryGetValue(out bool[][] values)
        {
            if (_isArray && _value is string[][] valueX)
            {
                List<bool[]> resultX = new List<bool[]>();
                foreach (string[] value in valueX)
                {
                    List<bool> result = new List<bool>();
                    foreach (string val in value)
                    {
                        if (bool.TryParse(val, out bool v))
                        {
                            result.Add(v);
                        }
                        else
                        {
                            values = default;
                            return false;
                        }
                    }
                    resultX.Add(result.ToArray());
                }
                values = resultX.ToArray();
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
        public bool TryGetValue(out sbyte[][] values)
        {
            if (_isArray && _value is string[][] valueX)
            {
                List<sbyte[]> resultX = new List<sbyte[]>();
                foreach (string[] value in valueX)
                {
                    List<sbyte> result = new List<sbyte>();
                    foreach (string val in value)
                    {
                        if (sbyte.TryParse(val, out sbyte v))
                        {
                            result.Add(v);
                        }
                        else
                        {
                            values = default;
                            return false;
                        }
                    }
                    resultX.Add(result.ToArray());
                }
                values = resultX.ToArray();
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
        public bool TryGetValue(out byte[][] values)
        {
            if (_isArray && _value is string[][] valueX)
            {
                List<byte[]> resultX = new List<byte[]>();
                foreach (string[] value in valueX)
                {
                    List<byte> result = new List<byte>();
                    foreach (string val in value)
                    {
                        if (byte.TryParse(val, out byte v))
                        {
                            result.Add(v);
                        }
                        else
                        {
                            values = default;
                            return false;
                        }
                    }
                    resultX.Add(result.ToArray());
                }
                values = resultX.ToArray();
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
        public bool TryGetValue(out short[][] values)
        {
            if (_isArray && _value is string[][] valueX)
            {
                List<short[]> resultX = new List<short[]>();
                foreach (string[] value in valueX)
                {
                    List<short> result = new List<short>();
                    foreach (string val in value)
                    {
                        if (short.TryParse(val, out short v))
                        {
                            result.Add(v);
                        }
                        else
                        {
                            values = default;
                            return false;
                        }
                    }
                    resultX.Add(result.ToArray());
                }
                values = resultX.ToArray();
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
        public bool TryGetValue(out ushort[][] values)
        {
            if (_isArray && _value is string[][] valueX)
            {
                List<ushort[]> resultX = new List<ushort[]>();
                foreach (string[] value in valueX)
                {
                    List<ushort> result = new List<ushort>();
                    foreach (string val in value)
                    {
                        if (ushort.TryParse(val, out ushort v))
                        {
                            result.Add(v);
                        }
                        else
                        {
                            values = default;
                            return false;
                        }
                    }
                    resultX.Add(result.ToArray());
                }
                values = resultX.ToArray();
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
        public bool TryGetValue(out int[][] values)
        {
            if (_isArray && _value is string[][] valueX)
            {
                List<int[]> resultX = new List<int[]>();
                foreach (string[] value in valueX)
                {
                    List<int> result = new List<int>();
                    foreach (string val in value)
                    {
                        if (int.TryParse(val, out int v))
                        {
                            result.Add(v);
                        }
                        else
                        {
                            values = default;
                            return false;
                        }
                    }
                    resultX.Add(result.ToArray());
                }
                values = resultX.ToArray();
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
        public bool TryGetValue(out uint[][] values)
        {
            if (_isArray && _value is string[][] valueX)
            {
                List<uint[]> resultX = new List<uint[]>();
                foreach (string[] value in valueX)
                {
                    List<uint> result = new List<uint>();
                    foreach (string val in value)
                    {
                        if (uint.TryParse(val, out uint v))
                        {
                            result.Add(v);
                        }
                        else
                        {
                            values = default;
                            return false;
                        }
                    }
                    resultX.Add(result.ToArray());
                }
                values = resultX.ToArray();
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
        public bool TryGetValue(out long[][] values)
        {
            if (_isArray && _value is string[][] valueX)
            {
                List<long[]> resultX = new List<long[]>();
                foreach (string[] value in valueX)
                {
                    List<long> result = new List<long>();
                    foreach (string val in value)
                    {
                        if (long.TryParse(val, out long v))
                        {
                            result.Add(v);
                        }
                        else
                        {
                            values = default;
                            return false;
                        }
                    }
                    resultX.Add(result.ToArray());
                }
                values = resultX.ToArray();
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
        public bool TryGetValue(out ulong[][] values)
        {
            if (_isArray && _value is string[][] valueX)
            {
                List<ulong[]> resultX = new List<ulong[]>();
                foreach (string[] value in valueX)
                {
                    List<ulong> result = new List<ulong>();
                    foreach (string val in value)
                    {
                        if (ulong.TryParse(val, out ulong v))
                        {
                            result.Add(v);
                        }
                        else
                        {
                            values = default;
                            return false;
                        }
                    }
                    resultX.Add(result.ToArray());
                }
                values = resultX.ToArray();
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
        public bool TryGetValue(out float[][] values)
        {
            if (_isArray && _value is string[][] valueX)
            {
                List<float[]> resultX = new List<float[]>();
                foreach (string[] value in valueX)
                {
                    List<float> result = new List<float>();
                    foreach (string val in value)
                    {
                        if (float.TryParse(val, out float v))
                        {
                            result.Add(v);
                        }
                        else
                        {
                            values = default;
                            return false;
                        }
                    }
                    resultX.Add(result.ToArray());
                }
                values = resultX.ToArray();
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
        public bool TryGetValue(out double[][] values)
        {
            if (_isArray && _value is string[][] valueX)
            {
                List<double[]> resultX = new List<double[]>();
                foreach (string[] value in valueX)
                {
                    List<double> result = new List<double>();
                    foreach (string val in value)
                    {
                        if (double.TryParse(val, out double v))
                        {
                            result.Add(v);
                        }
                        else
                        {
                            values = default;
                            return false;
                        }
                    }
                    resultX.Add(result.ToArray());
                }
                values = resultX.ToArray();
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
        public bool TryGetValue(out decimal[][] values)
        {
            if (_isArray && _value is string[][] valueX)
            {
                List<decimal[]> resultX = new List<decimal[]>();
                foreach (string[] value in valueX)
                {
                    List<decimal> result = new List<decimal>();
                    foreach (string val in value)
                    {
                        if (decimal.TryParse(val, out decimal v))
                        {
                            result.Add(v);
                        }
                        else
                        {
                            values = default;
                            return false;
                        }
                    }
                    resultX.Add(result.ToArray());
                }
                values = resultX.ToArray();
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
        public bool TryGetValue(out char[][] values)
        {
            if (_isArray && _value is string[][] valueX)
            {
                List<char[]> resultX = new List<char[]>();
                foreach (string[] value in valueX)
                {
                    List<char> result = new List<char>();
                    foreach (string val in value)
                    {
                        if (char.TryParse(val, out char v))
                        {
                            result.Add(v);
                        }
                        else
                        {
                            values = default;
                            return false;
                        }
                    }
                    resultX.Add(result.ToArray());
                }
                values = resultX.ToArray();
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
        public bool TryGetValue(out Binaries[][] values)
        {
            if (_isArray && _value is string[][] valueX)
            {
                List<Binaries[]> resultX = new List<Binaries[]>();
                foreach (string[] value in valueX)
                {
                    List<Binaries> result = new List<Binaries>();
                    foreach (string val in value)
                    {
                        if (XValueHelper.TryParse(val, out byte[] v))
                        {
                            result.Add(new Binaries(v));
                        }
                        else
                        {
                            values = default;
                            return false;
                        }
                    }
                    resultX.Add(result.ToArray());
                }
                values = resultX.ToArray();
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
        public bool TryGetValue<TEnum>(out TEnum[][] values) where TEnum : struct
        {
            if (_isArray && _value is string[][] valueX && typeof(TEnum).BaseType.FullName == "System.Enum")
            {
                List<TEnum[]> resultX = new List<TEnum[]>();
                foreach (string[] value in valueX)
                {
                    List<TEnum> result = new List<TEnum>();
                    foreach (string val in value)
                    {
                        if (Enum.TryParse(val, false, out TEnum v))
                        {
                            result.Add(v);
                        }
                        else
                        {
                            values = default;
                            return false;
                        }
                    }
                    resultX.Add(result.ToArray());
                }
                values = resultX.ToArray();
                return true;
            }
            values = default;
            return false;
        }

        #endregion TryGetArrayValue2

        #region TryGetArrayValue3

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(out string[][][] values)
        {
            if (_isArray && _value is string[][][] valueX)
            {
                values = valueX;
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
        public bool TryGetValue(out bool[][][] values)
        {
            if (_isArray && _value is string[][][] valueX)
            {
                List<bool[][]> resultX = new List<bool[][]>();
                foreach (string[][] value in valueX)
                {
                    List<bool[]> result = new List<bool[]>();
                    foreach (string[] val in value)
                    {
                        List<bool> res = new List<bool>();
                        foreach (string v in val)
                        {
                            if (bool.TryParse(v, out bool o))
                            {
                                res.Add(o);
                            }
                            else
                            {
                                values = default;
                                return false;
                            }
                        }
                        result.Add(res.ToArray());
                    }
                    resultX.Add(result.ToArray());
                }
                values = resultX.ToArray();
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
        public bool TryGetValue(out sbyte[][][] values)
        {
            if (_isArray && _value is string[][][] valueX)
            {
                List<sbyte[][]> resultX = new List<sbyte[][]>();
                foreach (string[][] value in valueX)
                {
                    List<sbyte[]> result = new List<sbyte[]>();
                    foreach (string[] val in value)
                    {
                        List<sbyte> res = new List<sbyte>();
                        foreach (string v in val)
                        {
                            if (sbyte.TryParse(v, out sbyte o))
                            {
                                res.Add(o);
                            }
                            else
                            {
                                values = default;
                                return false;
                            }
                        }
                        result.Add(res.ToArray());
                    }
                    resultX.Add(result.ToArray());
                }
                values = resultX.ToArray();
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
        public bool TryGetValue(out byte[][][] values)
        {
            if (_isArray && _value is string[][][] valueX)
            {
                List<byte[][]> resultX = new List<byte[][]>();
                foreach (string[][] value in valueX)
                {
                    List<byte[]> result = new List<byte[]>();
                    foreach (string[] val in value)
                    {
                        List<byte> res = new List<byte>();
                        foreach (string v in val)
                        {
                            if (byte.TryParse(v, out byte o))
                            {
                                res.Add(o);
                            }
                            else
                            {
                                values = default;
                                return false;
                            }
                        }
                        result.Add(res.ToArray());
                    }
                    resultX.Add(result.ToArray());
                }
                values = resultX.ToArray();
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
        public bool TryGetValue(out short[][][] values)
        {
            if (_isArray && _value is string[][][] valueX)
            {
                List<short[][]> resultX = new List<short[][]>();
                foreach (string[][] value in valueX)
                {
                    List<short[]> result = new List<short[]>();
                    foreach (string[] val in value)
                    {
                        List<short> res = new List<short>();
                        foreach (string v in val)
                        {
                            if (short.TryParse(v, out short o))
                            {
                                res.Add(o);
                            }
                            else
                            {
                                values = default;
                                return false;
                            }
                        }
                        result.Add(res.ToArray());
                    }
                    resultX.Add(result.ToArray());
                }
                values = resultX.ToArray();
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
        public bool TryGetValue(out ushort[][][] values)
        {
            if (_isArray && _value is string[][][] valueX)
            {
                List<ushort[][]> resultX = new List<ushort[][]>();
                foreach (string[][] value in valueX)
                {
                    List<ushort[]> result = new List<ushort[]>();
                    foreach (string[] val in value)
                    {
                        List<ushort> res = new List<ushort>();
                        foreach (string v in val)
                        {
                            if (ushort.TryParse(v, out ushort o))
                            {
                                res.Add(o);
                            }
                            else
                            {
                                values = default;
                                return false;
                            }
                        }
                        result.Add(res.ToArray());
                    }
                    resultX.Add(result.ToArray());
                }
                values = resultX.ToArray();
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
        public bool TryGetValue(out int[][][] values)
        {
            if (_isArray && _value is string[][][] valueX)
            {
                List<int[][]> resultX = new List<int[][]>();
                foreach (string[][] value in valueX)
                {
                    List<int[]> result = new List<int[]>();
                    foreach (string[] val in value)
                    {
                        List<int> res = new List<int>();
                        foreach (string v in val)
                        {
                            if (int.TryParse(v, out int o))
                            {
                                res.Add(o);
                            }
                            else
                            {
                                values = default;
                                return false;
                            }
                        }
                        result.Add(res.ToArray());
                    }
                    resultX.Add(result.ToArray());
                }
                values = resultX.ToArray();
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
        public bool TryGetValue(out uint[][][] values)
        {
            if (_isArray && _value is string[][][] valueX)
            {
                List<uint[][]> resultX = new List<uint[][]>();
                foreach (string[][] value in valueX)
                {
                    List<uint[]> result = new List<uint[]>();
                    foreach (string[] val in value)
                    {
                        List<uint> res = new List<uint>();
                        foreach (string v in val)
                        {
                            if (uint.TryParse(v, out uint o))
                            {
                                res.Add(o);
                            }
                            else
                            {
                                values = default;
                                return false;
                            }
                        }
                        result.Add(res.ToArray());
                    }
                    resultX.Add(result.ToArray());
                }
                values = resultX.ToArray();
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
        public bool TryGetValue(out long[][][] values)
        {
            if (_isArray && _value is string[][][] valueX)
            {
                List<long[][]> resultX = new List<long[][]>();
                foreach (string[][] value in valueX)
                {
                    List<long[]> result = new List<long[]>();
                    foreach (string[] val in value)
                    {
                        List<long> res = new List<long>();
                        foreach (string v in val)
                        {
                            if (long.TryParse(v, out long o))
                            {
                                res.Add(o);
                            }
                            else
                            {
                                values = default;
                                return false;
                            }
                        }
                        result.Add(res.ToArray());
                    }
                    resultX.Add(result.ToArray());
                }
                values = resultX.ToArray();
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
        public bool TryGetValue(out ulong[][][] values)
        {
            if (_isArray && _value is string[][][] valueX)
            {
                List<ulong[][]> resultX = new List<ulong[][]>();
                foreach (string[][] value in valueX)
                {
                    List<ulong[]> result = new List<ulong[]>();
                    foreach (string[] val in value)
                    {
                        List<ulong> res = new List<ulong>();
                        foreach (string v in val)
                        {
                            if (ulong.TryParse(v, out ulong o))
                            {
                                res.Add(o);
                            }
                            else
                            {
                                values = default;
                                return false;
                            }
                        }
                        result.Add(res.ToArray());
                    }
                    resultX.Add(result.ToArray());
                }
                values = resultX.ToArray();
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
        public bool TryGetValue(out float[][][] values)
        {
            if (_isArray && _value is string[][][] valueX)
            {
                List<float[][]> resultX = new List<float[][]>();
                foreach (string[][] value in valueX)
                {
                    List<float[]> result = new List<float[]>();
                    foreach (string[] val in value)
                    {
                        List<float> res = new List<float>();
                        foreach (string v in val)
                        {
                            if (float.TryParse(v, out float o))
                            {
                                res.Add(o);
                            }
                            else
                            {
                                values = default;
                                return false;
                            }
                        }
                        result.Add(res.ToArray());
                    }
                    resultX.Add(result.ToArray());
                }
                values = resultX.ToArray();
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
        public bool TryGetValue(out double[][][] values)
        {
            if (_isArray && _value is string[][][] valueX)
            {
                List<double[][]> resultX = new List<double[][]>();
                foreach (string[][] value in valueX)
                {
                    List<double[]> result = new List<double[]>();
                    foreach (string[] val in value)
                    {
                        List<double> res = new List<double>();
                        foreach (string v in val)
                        {
                            if (double.TryParse(v, out double o))
                            {
                                res.Add(o);
                            }
                            else
                            {
                                values = default;
                                return false;
                            }
                        }
                        result.Add(res.ToArray());
                    }
                    resultX.Add(result.ToArray());
                }
                values = resultX.ToArray();
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
        public bool TryGetValue(out decimal[][][] values)
        {
            if (_isArray && _value is string[][][] valueX)
            {
                List<decimal[][]> resultX = new List<decimal[][]>();
                foreach (string[][] value in valueX)
                {
                    List<decimal[]> result = new List<decimal[]>();
                    foreach (string[] val in value)
                    {
                        List<decimal> res = new List<decimal>();
                        foreach (string v in val)
                        {
                            if (decimal.TryParse(v, out decimal o))
                            {
                                res.Add(o);
                            }
                            else
                            {
                                values = default;
                                return false;
                            }
                        }
                        result.Add(res.ToArray());
                    }
                    resultX.Add(result.ToArray());
                }
                values = resultX.ToArray();
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
        public bool TryGetValue(out char[][][] values)
        {
            if (_isArray && _value is string[][][] valueX)
            {
                List<char[][]> resultX = new List<char[][]>();
                foreach (string[][] value in valueX)
                {
                    List<char[]> result = new List<char[]>();
                    foreach (string[] val in value)
                    {
                        List<char> res = new List<char>();
                        foreach (string v in val)
                        {
                            if (char.TryParse(v, out char o))
                            {
                                res.Add(o);
                            }
                            else
                            {
                                values = default;
                                return false;
                            }
                        }
                        result.Add(res.ToArray());
                    }
                    resultX.Add(result.ToArray());
                }
                values = resultX.ToArray();
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
        public bool TryGetValue(out Binaries[][][] values)
        {
            if (_isArray && _value is string[][][] valueX)
            {
                List<Binaries[][]> resultX = new List<Binaries[][]>();
                foreach (string[][] value in valueX)
                {
                    List<Binaries[]> result = new List<Binaries[]>();
                    foreach (string[] val in value)
                    {
                        List<Binaries> res = new List<Binaries>();
                        foreach (string v in val)
                        {
                            if (XValueHelper.TryParse(v, out byte[] o))
                            {
                                res.Add(new Binaries(o));
                            }
                            else
                            {
                                values = default;
                                return false;
                            }
                        }
                        result.Add(res.ToArray());
                    }
                    resultX.Add(result.ToArray());
                }
                values = resultX.ToArray();
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
        public bool TryGetValue<TEnum>(out TEnum[][][] values) where TEnum : struct
        {
            if (_isArray && _value is string[][][] valueX && typeof(TEnum).BaseType.FullName == "System.Enum")
            {
                List<TEnum[][]> resultX = new List<TEnum[][]>();
                foreach (string[][] value in valueX)
                {
                    List<TEnum[]> result = new List<TEnum[]>();
                    foreach (string[] val in value)
                    {
                        List<TEnum> res = new List<TEnum>();
                        foreach (string v in val)
                        {
                            if (Enum.TryParse(v, false, out TEnum o))
                            {
                                res.Add(o);
                            }
                            else
                            {
                                values = default;
                                return false;
                            }
                        }
                        result.Add(res.ToArray());
                    }
                    resultX.Add(result.ToArray());
                }
                values = resultX.ToArray();
                return true;
            }
            values = default;
            return false;
        }

        #endregion TryGetArrayValue3

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
        public Binaries GetValue(Binaries defaultValue)
        {
            return TryGetValue(out Binaries value) ? value : defaultValue;
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

        #region GetArrayValue1

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="defaultValues">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public string[] GetValue(string[] defaultValues)
        {
            return TryGetValue(out string[] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="defaultValues">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool[] GetValue(bool[] defaultValues)
        {
            return TryGetValue(out bool[] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="defaultValues">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public sbyte[] GetValue(sbyte[] defaultValues)
        {
            return TryGetValue(out sbyte[] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="defaultValues">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public byte[] GetValue(byte[] defaultValues)
        {
            return TryGetValue(out byte[] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="defaultValues">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public short[] GetValue(short[] defaultValues)
        {
            return TryGetValue(out short[] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="defaultValues">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public ushort[] GetValue(ushort[] defaultValues)
        {
            return TryGetValue(out ushort[] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="defaultValues">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public int[] GetValue(int[] defaultValues)
        {
            return TryGetValue(out int[] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="defaultValues">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public uint[] GetValue(uint[] defaultValues)
        {
            return TryGetValue(out uint[] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="defaultValues">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public long[] GetValue(long[] defaultValues)
        {
            return TryGetValue(out long[] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="defaultValues">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public ulong[] GetValue(ulong[] defaultValues)
        {
            return TryGetValue(out ulong[] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="defaultValues">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public float[] GetValue(float[] defaultValues)
        {
            return TryGetValue(out float[] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="defaultValues">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public double[] GetValue(double[] defaultValues)
        {
            return TryGetValue(out double[] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="defaultValues">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public decimal[] GetValue(decimal[] defaultValues)
        {
            return TryGetValue(out decimal[] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="defaultValues">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public char[] GetValue(char[] defaultValues)
        {
            return TryGetValue(out char[] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="defaultValues">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public Binaries[] GetValue(Binaries[] defaultValues)
        {
            return TryGetValue(out Binaries[] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="defaultValues">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public TEnum[] GetValue<TEnum>(TEnum[] defaultValues) where TEnum : struct
        {
            return TryGetValue(out TEnum[] values) ? values : defaultValues;
        }

        #endregion GetArrayValue1

        #region GetArrayValue2

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="defaultValues">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public string[][] GetValue(string[][] defaultValues)
        {
            return TryGetValue(out string[][] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="defaultValues">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool[][] GetValue(bool[][] defaultValues)
        {
            return TryGetValue(out bool[][] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="defaultValues">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public sbyte[][] GetValue(sbyte[][] defaultValues)
        {
            return TryGetValue(out sbyte[][] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="defaultValues">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public byte[][] GetValue(byte[][] defaultValues)
        {
            return TryGetValue(out byte[][] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="defaultValues">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public short[][] GetValue(short[][] defaultValues)
        {
            return TryGetValue(out short[][] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="defaultValues">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public ushort[][] GetValue(ushort[][] defaultValues)
        {
            return TryGetValue(out ushort[][] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="defaultValues">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public int[][] GetValue(int[][] defaultValues)
        {
            return TryGetValue(out int[][] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="defaultValues">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public uint[][] GetValue(uint[][] defaultValues)
        {
            return TryGetValue(out uint[][] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="defaultValues">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public long[][] GetValue(long[][] defaultValues)
        {
            return TryGetValue(out long[][] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="defaultValues">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public ulong[][] GetValue(ulong[][] defaultValues)
        {
            return TryGetValue(out ulong[][] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="defaultValues">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public float[][] GetValue(float[][] defaultValues)
        {
            return TryGetValue(out float[][] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="defaultValues">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public double[][] GetValue(double[][] defaultValues)
        {
            return TryGetValue(out double[][] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="defaultValues">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public decimal[][] GetValue(decimal[][] defaultValues)
        {
            return TryGetValue(out decimal[][] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="defaultValues">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public char[][] GetValue(char[][] defaultValues)
        {
            return TryGetValue(out char[][] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="defaultValues">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public Binaries[][] GetValue(Binaries[][] defaultValues)
        {
            return TryGetValue(out Binaries[][] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="defaultValues">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public TEnum[][] GetValue<TEnum>(TEnum[][] defaultValues) where TEnum : struct
        {
            return TryGetValue(out TEnum[][] values) ? values : defaultValues;
        }

        #endregion GetArrayValue2

        #region GetArrayValue3

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="defaultValues">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public string[][][] GetValue(string[][][] defaultValues)
        {
            return TryGetValue(out string[][][] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="defaultValues">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool[][][] GetValue(bool[][][] defaultValues)
        {
            return TryGetValue(out bool[][][] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="defaultValues">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public sbyte[][][] GetValue(sbyte[][][] defaultValues)
        {
            return TryGetValue(out sbyte[][][] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="defaultValues">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public byte[][][] GetValue(byte[][][] defaultValues)
        {
            return TryGetValue(out byte[][][] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="defaultValues">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public short[][][] GetValue(short[][][] defaultValues)
        {
            return TryGetValue(out short[][][] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="defaultValues">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public ushort[][][] GetValue(ushort[][][] defaultValues)
        {
            return TryGetValue(out ushort[][][] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="defaultValues">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public int[][][] GetValue(int[][][] defaultValues)
        {
            return TryGetValue(out int[][][] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="defaultValues">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public uint[][][] GetValue(uint[][][] defaultValues)
        {
            return TryGetValue(out uint[][][] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="defaultValues">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public long[][][] GetValue(long[][][] defaultValues)
        {
            return TryGetValue(out long[][][] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="defaultValues">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public ulong[][][] GetValue(ulong[][][] defaultValues)
        {
            return TryGetValue(out ulong[][][] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="defaultValues">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public float[][][] GetValue(float[][][] defaultValues)
        {
            return TryGetValue(out float[][][] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="defaultValues">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public double[][][] GetValue(double[][][] defaultValues)
        {
            return TryGetValue(out double[][][] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="defaultValues">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public decimal[][][] GetValue(decimal[][][] defaultValues)
        {
            return TryGetValue(out decimal[][][] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="defaultValues">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public char[][][] GetValue(char[][][] defaultValues)
        {
            return TryGetValue(out char[][][] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="defaultValues">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public Binaries[][][] GetValue(Binaries[][][] defaultValues)
        {
            return TryGetValue(out Binaries[][][] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="defaultValues">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public TEnum[][][] GetValue<TEnum>(TEnum[][][] defaultValues) where TEnum : struct
        {
            return TryGetValue(out TEnum[][][] values) ? values : defaultValues;
        }

        #endregion GetArrayValue3

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

        private static XElement GetElement(string key, string[] values)
        {
            XElement element = new XElement(HonooSettingsManager.Namespace + "property");
            element.SetAttributeValue("key", key);
            foreach (string value in values)
            {
                element.Add(new XElement(HonooSettingsManager.Namespace + "value", value));
            }
            return element;
        }

        private static XElement GetElement(string key, string[][] values)
        {
            XElement element = new XElement(HonooSettingsManager.Namespace + "property");
            element.SetAttributeValue("key", key);
            foreach (string[] values1 in values)
            {
                XElement ele1 = new XElement(HonooSettingsManager.Namespace + "value");
                foreach (string value in values1)
                {
                    ele1.Add(new XElement(HonooSettingsManager.Namespace + "value", value));
                }
                element.Add(ele1);
            }
            return element;
        }

        private static XElement GetElement(string key, string[][][] values)
        {
            XElement ele1 = new XElement(HonooSettingsManager.Namespace + "property");
            ele1.SetAttributeValue("key", key);
            foreach (string[][] values1 in values)
            {
                XElement ele2 = new XElement(HonooSettingsManager.Namespace + "value");
                foreach (string[] values2 in values1)
                {
                    XElement ele3 = new XElement(HonooSettingsManager.Namespace + "value");
                    foreach (string value in values2)
                    {
                        ele3.Add(new XElement(HonooSettingsManager.Namespace + "value", value));
                    }
                    ele2.Add(ele3);
                }
                ele1.Add(ele2);
            }
            return ele1;
        }

        private static bool GetValue(XElement element, out string[] values)
        {
            List<string> values1 = new List<string>();
            foreach (var ele1 in element.Elements(HonooSettingsManager.Namespace + "value"))
            {
                if (ele1.HasElements)
                {
                    values = null;
                    return false;
                }
                values1.Add(ele1.Value);
            }
            values = values1.ToArray();
            return true;
        }

        private static bool GetValue(XElement element, out string[][] values)
        {
            List<string[]> values2 = new List<string[]>();
            foreach (var ele2 in element.Elements(HonooSettingsManager.Namespace + "value"))
            {
                List<string> values1 = new List<string>();
                foreach (var ele1 in ele2.Elements(HonooSettingsManager.Namespace + "value"))
                {
                    if (ele1.HasElements)
                    {
                        values = null;
                        return false;
                    }
                    values1.Add(ele1.Value);
                }
                values2.Add(values1.ToArray());
            }
            values = values2.ToArray();
            return true;
        }

        private static bool GetValue(XElement element, out string[][][] values)
        {
            List<string[][]> values3 = new List<string[][]>();
            foreach (var ele3 in element.Elements(HonooSettingsManager.Namespace + "value"))
            {
                List<string[]> values2 = new List<string[]>();
                foreach (var ele2 in ele3.Elements(HonooSettingsManager.Namespace + "value"))
                {
                    List<string> values1 = new List<string>();
                    foreach (var ele1 in ele2.Elements(HonooSettingsManager.Namespace + "value"))
                    {
                        if (ele1.HasElements)
                        {
                            values = null;
                            return false;
                        }
                        values1.Add(ele1.Value);
                    }
                    values2.Add(values1.ToArray());
                }
                values3.Add(values2.ToArray());
            }
            values = values3.ToArray();
            return true;
        }
    }
}