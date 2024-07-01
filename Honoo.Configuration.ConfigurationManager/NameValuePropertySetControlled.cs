using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 应用 &lt;remove /&gt;、&lt;clear /&gt; 标签后的只读配置属性集合。
    /// </summary>
    public sealed class NameValuePropertySetControlled : IEnumerable<AddProperty>
    {
        #region Properties

        private readonly XElement _container;
        private readonly List<AddProperty> _properties = new List<AddProperty>();

        /// <summary>
        /// 获取配置属性集合中包含的元素数。
        /// </summary>
        public int Count => _properties.Count;

        #endregion Properties

        #region Construction

        internal NameValuePropertySetControlled(XElement container)
        {
            _container = container;
            if (_container.HasElements)
            {
                IEnumerator<XNode> enumerator = _container.Nodes().GetEnumerator();
                XComment comment = null;
                while (enumerator.MoveNext())
                {
                    if (enumerator.Current.NodeType == XmlNodeType.Comment)
                    {
                        comment = (XComment)enumerator.Current;
                    }
                    else
                    {
                        if (enumerator.Current.NodeType == XmlNodeType.Element)
                        {
                            XElement content = (XElement)enumerator.Current;
                            if (content.Name == "add")
                            {
                                AddProperty property = new AddProperty(content, comment);
                                _properties.Add(property);
                            }
                            else if (content.Name == "remove")
                            {
                                for (int i = _properties.Count - 1; i >= 0; i--)
                                {
                                    if (_properties[i].Key == content.Attribute("key").Value)
                                    {
                                        _properties.RemoveAt(i);
                                    }
                                }
                            }
                            else if (content.Name == "clear")
                            {
                                _properties.Clear();
                            }
                        }
                        comment = null;
                    }
                }
            }
        }

        #endregion Construction

        #region TryGetValue

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="properties">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string key, out AddProperty[] properties)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException($"The invalid argument - {nameof(key)}.");
            }
            List<AddProperty> result = new List<AddProperty>();
            for (int i = 0; i < _properties.Count; i++)
            {
                if (_properties[i] is AddProperty property)
                {
                    if (property.Key == key)
                    {
                        result.Add(property);
                    }
                }
            }
            properties = result.ToArray();
            return properties.Length > 0;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string key, out string[] values)
        {
            if (TryGetValue(key, out AddProperty[] val))
            {
                List<string> result = new List<string>();
                foreach (AddProperty property in val)
                {
                    result.Add(property.Value);
                }
                values = result.ToArray();
                return true;
            }
            values = default;
            return false;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <see langword="false"/>。如果找到了指定键但无法转换指定的类型，则仍返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string key, out bool[] values)
        {
            if (TryGetValue(key, out AddProperty[] value))
            {
                List<bool> result = new List<bool>();
                foreach (AddProperty property in value)
                {
                    if (bool.TryParse(property.Value, out bool val))
                    {
                        result.Add(val);
                    }
                }
                values = result.ToArray();
                return true;
            }
            values = default;
            return false;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <see langword="false"/>。如果找到了指定键但无法转换指定的类型，则仍返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string key, out sbyte[] values)
        {
            if (TryGetValue(key, out AddProperty[] value))
            {
                List<sbyte> result = new List<sbyte>();
                foreach (AddProperty property in value)
                {
                    if (sbyte.TryParse(property.Value, out sbyte val))
                    {
                        result.Add(val);
                    }
                }
                values = result.ToArray();
                return true;
            }
            values = default;
            return false;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <see langword="false"/>。如果找到了指定键但无法转换指定的类型，则仍返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string key, out byte[] values)
        {
            if (TryGetValue(key, out AddProperty[] value))
            {
                List<byte> result = new List<byte>();
                foreach (AddProperty property in value)
                {
                    if (byte.TryParse(property.Value, out byte val))
                    {
                        result.Add(val);
                    }
                }
                values = result.ToArray();
                return true;
            }
            values = default;
            return false;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <see langword="false"/>。如果找到了指定键但无法转换指定的类型，则仍返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string key, out short[] values)
        {
            if (TryGetValue(key, out AddProperty[] value))
            {
                List<short> result = new List<short>();
                foreach (AddProperty property in value)
                {
                    if (short.TryParse(property.Value, out short val))
                    {
                        result.Add(val);
                    }
                }
                values = result.ToArray();
                return true;
            }
            values = default;
            return false;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <see langword="false"/>。如果找到了指定键但无法转换指定的类型，则仍返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string key, out ushort[] values)
        {
            if (TryGetValue(key, out AddProperty[] value))
            {
                List<ushort> result = new List<ushort>();
                foreach (AddProperty property in value)
                {
                    if (ushort.TryParse(property.Value, out ushort val))
                    {
                        result.Add(val);
                    }
                }
                values = result.ToArray();
                return true;
            }
            values = default;
            return false;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <see langword="false"/>。如果找到了指定键但无法转换指定的类型，则仍返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string key, out int[] values)
        {
            if (TryGetValue(key, out AddProperty[] value))
            {
                List<int> result = new List<int>();
                foreach (AddProperty property in value)
                {
                    if (int.TryParse(property.Value, out int val))
                    {
                        result.Add(val);
                    }
                }
                values = result.ToArray();
                return true;
            }
            values = default;
            return false;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <see langword="false"/>。如果找到了指定键但无法转换指定的类型，则仍返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string key, out uint[] values)
        {
            if (TryGetValue(key, out AddProperty[] value))
            {
                List<uint> result = new List<uint>();
                foreach (AddProperty property in value)
                {
                    if (uint.TryParse(property.Value, out uint val))
                    {
                        result.Add(val);
                    }
                }
                values = result.ToArray();
                return true;
            }
            values = default;
            return false;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <see langword="false"/>。如果找到了指定键但无法转换指定的类型，则仍返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string key, out long[] values)
        {
            if (TryGetValue(key, out AddProperty[] value))
            {
                List<long> result = new List<long>();
                foreach (AddProperty property in value)
                {
                    if (long.TryParse(property.Value, out long val))
                    {
                        result.Add(val);
                    }
                }
                values = result.ToArray();
                return true;
            }
            values = default;
            return false;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <see langword="false"/>。如果找到了指定键但无法转换指定的类型，则仍返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string key, out ulong[] values)
        {
            if (TryGetValue(key, out AddProperty[] value))
            {
                List<ulong> result = new List<ulong>();
                foreach (AddProperty property in value)
                {
                    if (ulong.TryParse(property.Value, out ulong val))
                    {
                        result.Add(val);
                    }
                }
                values = result.ToArray();
                return true;
            }
            values = default;
            return false;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <see langword="false"/>。如果找到了指定键但无法转换指定的类型，则仍返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string key, out float[] values)
        {
            if (TryGetValue(key, out AddProperty[] value))
            {
                List<float> result = new List<float>();
                foreach (AddProperty property in value)
                {
                    if (float.TryParse(property.Value, out float val))
                    {
                        result.Add(val);
                    }
                }
                values = result.ToArray();
                return true;
            }
            values = default;
            return false;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <see langword="false"/>。如果找到了指定键但无法转换指定的类型，则仍返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string key, out double[] values)
        {
            if (TryGetValue(key, out AddProperty[] value))
            {
                List<double> result = new List<double>();
                foreach (AddProperty property in value)
                {
                    if (double.TryParse(property.Value, out double val))
                    {
                        result.Add(val);
                    }
                }
                values = result.ToArray();
                return true;
            }
            values = default;
            return false;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <see langword="false"/>。如果找到了指定键但无法转换指定的类型，则仍返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string key, out decimal[] values)
        {
            if (TryGetValue(key, out AddProperty[] value))
            {
                List<decimal> result = new List<decimal>();
                foreach (AddProperty property in value)
                {
                    if (decimal.TryParse(property.Value, out decimal val))
                    {
                        result.Add(val);
                    }
                }
                values = result.ToArray();
                return true;
            }
            values = default;
            return false;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <see langword="false"/>。如果找到了指定键但无法转换指定的类型，则仍返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string key, out char[] values)
        {
            if (TryGetValue(key, out AddProperty[] value))
            {
                List<char> result = new List<char>();
                foreach (AddProperty property in value)
                {
                    if (char.TryParse(property.Value, out char val))
                    {
                        result.Add(val);
                    }
                }
                values = result.ToArray();
                return true;
            }
            values = default;
            return false;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <see langword="false"/>。如果找到了指定键但无法转换指定的类型，则仍返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string key, out byte[][] values)
        {
            if (TryGetValue(key, out AddProperty[] value))
            {
                List<byte[]> result = new List<byte[]>();
                foreach (AddProperty property in value)
                {
                    if (XValueHelper.TryParse(property.Value, out byte[] val))
                    {
                        result.Add(val);
                    }
                }
                values = result.ToArray();
                return true;
            }
            values = default;
            return false;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <see langword="false"/>。如果找到了指定键但无法转换指定的类型，则仍返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue<TEnum>(string key, out TEnum[] values) where TEnum : struct
        {
            if (typeof(TEnum).BaseType.FullName == "System.Enum")
            {
                if (TryGetValue(key, out AddProperty[] value))
                {
                    List<TEnum> result = new List<TEnum>();
                    foreach (AddProperty property in value)
                    {
                        if (Enum.TryParse(property.Value, false, out TEnum val))
                        {
                            result.Add(val);
                        }
                    }
                    values = result.ToArray();
                    return true;
                }
            }
            values = default;
            return false;
        }

        #endregion TryGetValue

        #region GetValue

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public AddProperty[] GetValue(string key)
        {
            return TryGetValue(key, out AddProperty[] properties) ? properties : null;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValue">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public string[] GetValue(string key, string[] defaultValue)
        {
            return TryGetValue(key, out string[] values) ? values : defaultValue;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <paramref name="defaultValue"/>。如果找到了指定键但无法转换为指定的类型，则仍返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValue">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool[] GetValue(string key, bool[] defaultValue)
        {
            return TryGetValue(key, out bool[] values) ? values : defaultValue;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <paramref name="defaultValue"/>。如果找到了指定键但无法转换为指定的类型，则仍返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValue">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public sbyte[] GetValue(string key, sbyte[] defaultValue)
        {
            return TryGetValue(key, out sbyte[] values) ? values : defaultValue;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <paramref name="defaultValue"/>。如果找到了指定键但无法转换为指定的类型，则仍返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValue">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public byte[] GetValue(string key, byte[] defaultValue)
        {
            return TryGetValue(key, out byte[] values) ? values : defaultValue;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <paramref name="defaultValue"/>。如果找到了指定键但无法转换为指定的类型，则仍返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValue">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public short[] GetValue(string key, short[] defaultValue)
        {
            return TryGetValue(key, out short[] values) ? values : defaultValue;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <paramref name="defaultValue"/>。如果找到了指定键但无法转换为指定的类型，则仍返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValue">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public ushort[] GetValue(string key, ushort[] defaultValue)
        {
            return TryGetValue(key, out ushort[] values) ? values : defaultValue;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <paramref name="defaultValue"/>。如果找到了指定键但无法转换为指定的类型，则仍返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValue">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public int[] GetValue(string key, int[] defaultValue)
        {
            return TryGetValue(key, out int[] values) ? values : defaultValue;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <paramref name="defaultValue"/>。如果找到了指定键但无法转换为指定的类型，则仍返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValue">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public uint[] GetValue(string key, uint[] defaultValue)
        {
            return TryGetValue(key, out uint[] values) ? values : defaultValue;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <paramref name="defaultValue"/>。如果找到了指定键但无法转换为指定的类型，则仍返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValue">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public long[] GetValue(string key, long[] defaultValue)
        {
            return TryGetValue(key, out long[] values) ? values : defaultValue;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <paramref name="defaultValue"/>。如果找到了指定键但无法转换为指定的类型，则仍返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValue">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public ulong[] GetValue(string key, ulong[] defaultValue)
        {
            return TryGetValue(key, out ulong[] values) ? values : defaultValue;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <paramref name="defaultValue"/>。如果找到了指定键但无法转换为指定的类型，则仍返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValue">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public float[] GetValue(string key, float[] defaultValue)
        {
            return TryGetValue(key, out float[] values) ? values : defaultValue;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <paramref name="defaultValue"/>。如果找到了指定键但无法转换为指定的类型，则仍返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValue">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public double[] GetValue(string key, double[] defaultValue)
        {
            return TryGetValue(key, out double[] values) ? values : defaultValue;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <paramref name="defaultValue"/>。如果找到了指定键但无法转换为指定的类型，则仍返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValue">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public decimal[] GetValue(string key, decimal[] defaultValue)
        {
            return TryGetValue(key, out decimal[] values) ? values : defaultValue;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <paramref name="defaultValue"/>。如果找到了指定键但无法转换为指定的类型，则仍返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValue">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public char[] GetValue(string key, char[] defaultValue)
        {
            return TryGetValue(key, out char[] values) ? values : defaultValue;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <paramref name="defaultValue"/>。如果找到了指定键但无法转换为指定的类型，则仍返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValue">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public byte[][] GetValue(string key, byte[][] defaultValue)
        {
            return TryGetValue(key, out byte[][] values) ? values : defaultValue;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <paramref name="defaultValue"/>。如果找到了指定键但无法转换为指定的类型，则仍返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValue">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public TEnum[] GetValue<TEnum>(string key, TEnum[] defaultValue) where TEnum : struct
        {
            return TryGetValue(key, out TEnum[] values) ? values : defaultValue;
        }

        #endregion GetValue

        /// <summary>
        /// 确定配置属性集合是否包含带有指定键的配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool Contains(string key)
        {
            for (int i = 0; i < _properties.Count; i++)
            {
                if (_properties[i] is AddProperty property)
                {
                    if (property.Key == key)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 返回循环访问集合的枚举数。
        /// </summary>
        /// <returns></returns>
        public IEnumerator<AddProperty> GetEnumerator()
        {
            return _properties.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _properties.GetEnumerator();
        }
    }
}