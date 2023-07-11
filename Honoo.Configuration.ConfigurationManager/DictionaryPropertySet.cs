using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 配置属性集合。
    /// </summary>
    public class DictionaryPropertySet : IEnumerable<KeyValuePair<string, string>>
    {
        #region Class

        /// <summary>
        /// 代表此配置属性集合的键的集合。
        /// </summary>
        public sealed class KeyCollection : IEnumerable<string>
        {
            #region Properties

            private readonly Dictionary<string, string> _properties;

            /// <summary>
            /// 获取配置属性集合的键的元素数。
            /// </summary>
            public int Count => _properties.Count;

            #endregion Properties

            internal KeyCollection(Dictionary<string, string> properties)
            {
                _properties = properties;
            }

            /// <summary>
            /// 从指定数组索引开始将键元素复制到到指定数组。
            /// </summary>
            /// <param name="array">要复制到的目标数组。</param>
            /// <param name="arrayIndex">目标数组中从零开始的索引，从此处开始复制。</param>
            public void CopyTo(string[] array, int arrayIndex)
            {
                _properties.Keys.CopyTo(array, arrayIndex);
            }

            /// <summary>
            /// 返回循环访问集合的枚举数。
            /// </summary>
            /// <returns></returns>
            public IEnumerator<string> GetEnumerator()
            {
                return _properties.Keys.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return _properties.Keys.GetEnumerator();
            }
        }

        /// <summary>
        /// 代表此配置属性集合的值的集合。
        /// </summary>
        public sealed class ValueCollection : IEnumerable<string>
        {
            #region Properties

            private readonly Dictionary<string, string> _properties;

            /// <summary>
            /// 获取配置属性集合的值的元素数。
            /// </summary>
            public int Count => _properties.Count;

            #endregion Properties

            internal ValueCollection(Dictionary<string, string> properties)
            {
                _properties = properties;
            }

            /// <summary>
            /// 从指定数组索引开始将值元素复制到到指定数组。
            /// </summary>
            /// <param name="array">要复制到的目标数组。</param>
            /// <param name="arrayIndex">目标数组中从零开始的索引，从此处开始复制。</param>
            public void CopyTo(string[] array, int arrayIndex)
            {
                _properties.Values.CopyTo(array, arrayIndex);
            }

            /// <summary>
            /// 返回循环访问集合的枚举数。
            /// </summary>
            /// <returns></returns>
            public IEnumerator<string> GetEnumerator()
            {
                return _properties.Values.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return _properties.Values.GetEnumerator();
            }
        }

        #endregion Class

        #region Properties

        private readonly Dictionary<string, XComment> _comments = new Dictionary<string, XComment>();
        private readonly Dictionary<string, XElement> _contents = new Dictionary<string, XElement>();
        private readonly KeyCollection _keyExhibits;
        private readonly Dictionary<string, string> _properties = new Dictionary<string, string>();
        private readonly XElement _superior;
        private readonly ValueCollection _valueExhibits;

        /// <summary>
        /// 获取配置属性集合中包含的元素数。
        /// </summary>
        public int Count => _properties.Count;

        /// <summary>
        /// 获取配置属性集合的键的集合。
        /// </summary>
        public KeyCollection Keys => _keyExhibits;

        /// <summary>
        /// 获取配置属性集合的值的集合。
        /// </summary>
        public ValueCollection Values => _valueExhibits;

        /// <summary>
        /// 获取或设置具有指定键的配置属性的值。直接赋值等同于 AddOrUpdate 方法。
        /// <br/>取值时如果没有找到指定键，返回 <see langword="null"/>。使用 <see cref="GetValue(string, string)"/> 设置没有找到指定键时的默认值。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public string this[string key]
        {
            get => _properties.TryGetValue(key, out string value) ? value : null;
            set { AddOrUpdate(key, value); }
        }

        #endregion Properties

        #region Construction

        internal DictionaryPropertySet(XElement superior)
        {
            _superior = superior;
            if (superior.HasElements)
            {
                IEnumerator<XNode> enumerator = superior.Nodes().GetEnumerator();
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
                                string key = content.Attribute("key").Value;
                                string value = content.Attribute("value").Value;
                                if (_properties.TryGetValue(key, out _))
                                {
                                    Remove(key);
                                }
                                _properties.Add(key, value);
                                _contents.Add(key, content);
                                _comments.Add(key, comment);
                            }
                        }
                        comment = null;
                    }
                }
            }
            _keyExhibits = new KeyCollection(_properties);
            _valueExhibits = new ValueCollection(_properties);
        }

        #endregion Construction

        #region AddOrUpdate

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <exception cref="Exception"/>
        public void AddOrUpdate(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException($"The invalid argument - {nameof(key)}.");
            }
            if (value == null)
            {
                Remove(key);
            }
            else
            {
                if (_properties.TryGetValue(key, out _))
                {
                    _contents[key].SetAttributeValue("value", value);
                    _properties[key] = value;
                }
                else
                {
                    XElement content = new XElement("add");
                    content.SetAttributeValue("key", key);
                    content.SetAttributeValue("value", value);
                    _properties.Add(key, value);
                    _contents.Add(key, content);
                    _comments.Add(key, null);
                    _superior.Add(content);
                }
            }
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <exception cref="Exception"/>
        public void AddOrUpdate(string key, bool value)
        {
            AddOrUpdate(key, value.ToString());
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <exception cref="Exception"/>
        public void AddOrUpdate(string key, sbyte value)
        {
            AddOrUpdate(key, value.ToString());
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <exception cref="Exception"/>
        public void AddOrUpdate(string key, byte value)
        {
            AddOrUpdate(key, value.ToString());
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <exception cref="Exception"/>
        public void AddOrUpdate(string key, short value)
        {
            AddOrUpdate(key, value.ToString());
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <exception cref="Exception"/>
        public void AddOrUpdate(string key, ushort value)
        {
            AddOrUpdate(key, value.ToString());
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <exception cref="Exception"/>
        public void AddOrUpdate(string key, int value)
        {
            AddOrUpdate(key, value.ToString());
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <exception cref="Exception"/>
        public void AddOrUpdate(string key, uint value)
        {
            AddOrUpdate(key, value.ToString());
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <exception cref="Exception"/>
        public void AddOrUpdate(string key, long value)
        {
            AddOrUpdate(key, value.ToString());
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <exception cref="Exception"/>
        public void AddOrUpdate(string key, ulong value)
        {
            AddOrUpdate(key, value.ToString());
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <exception cref="Exception"/>
        public void AddOrUpdate(string key, float value)
        {
            AddOrUpdate(key, value.ToString());
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <exception cref="Exception"/>
        public void AddOrUpdate(string key, double value)
        {
            AddOrUpdate(key, value.ToString());
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <exception cref="Exception"/>
        public void AddOrUpdate(string key, decimal value)
        {
            AddOrUpdate(key, value.ToString());
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <exception cref="Exception"/>
        public void AddOrUpdate(string key, char value)
        {
            AddOrUpdate(key, value.ToString());
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <exception cref="Exception"/>
        public void AddOrUpdate(string key, byte[] value)
        {
            if (value == null)
            {
                Remove(key);
            }
            else
            {
                AddOrUpdate(key, BitConverter.ToString(value).Replace("-", string.Empty));
            }
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <exception cref="Exception"/>
        public void AddOrUpdate<TEnum>(string key, TEnum value) where TEnum : Enum
        {
            AddOrUpdate(key, value.ToString());
        }

        #endregion AddOrUpdate

        #region GetValue

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValue">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public string GetValue(string key, string defaultValue)
        {
            return _properties.TryGetValue(key, out string value) ? value : defaultValue;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <paramref name="defaultValue"/>。如果找到了指定键但无法转换为指定的类型，则仍返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValue">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool GetValue(string key, bool defaultValue)
        {
            if (_properties.TryGetValue(key, out string stringValue))
            {
                if (bool.TryParse(stringValue, out bool value))
                {
                    return value;
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <paramref name="defaultValue"/>。如果找到了指定键但无法转换为指定的类型，则仍返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValue">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public sbyte GetValue(string key, sbyte defaultValue)
        {
            if (_properties.TryGetValue(key, out string stringValue))
            {
                if (sbyte.TryParse(stringValue, out sbyte value))
                {
                    return value;
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <paramref name="defaultValue"/>。如果找到了指定键但无法转换为指定的类型，则仍返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValue">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public byte GetValue(string key, byte defaultValue)
        {
            if (_properties.TryGetValue(key, out string stringValue))
            {
                if (byte.TryParse(stringValue, out byte value))
                {
                    return value;
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <paramref name="defaultValue"/>。如果找到了指定键但无法转换为指定的类型，则仍返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValue">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public short GetValue(string key, short defaultValue)
        {
            if (_properties.TryGetValue(key, out string stringValue))
            {
                if (short.TryParse(stringValue, out short value))
                {
                    return value;
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <paramref name="defaultValue"/>。如果找到了指定键但无法转换为指定的类型，则仍返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValue">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public ushort GetValue(string key, ushort defaultValue)
        {
            if (_properties.TryGetValue(key, out string stringValue))
            {
                if (ushort.TryParse(stringValue, out ushort value))
                {
                    return value;
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <paramref name="defaultValue"/>。如果找到了指定键但无法转换为指定的类型，则仍返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValue">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public int GetValue(string key, int defaultValue)
        {
            if (_properties.TryGetValue(key, out string stringValue))
            {
                if (int.TryParse(stringValue, out int value))
                {
                    return value;
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <paramref name="defaultValue"/>。如果找到了指定键但无法转换为指定的类型，则仍返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValue">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public uint GetValue(string key, uint defaultValue)
        {
            if (_properties.TryGetValue(key, out string stringValue))
            {
                if (uint.TryParse(stringValue, out uint value))
                {
                    return value;
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <paramref name="defaultValue"/>。如果找到了指定键但无法转换为指定的类型，则仍返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValue">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public long GetValue(string key, long defaultValue)
        {
            if (_properties.TryGetValue(key, out string stringValue))
            {
                if (long.TryParse(stringValue, out long value))
                {
                    return value;
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <paramref name="defaultValue"/>。如果找到了指定键但无法转换为指定的类型，则仍返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValue">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public ulong GetValue(string key, ulong defaultValue)
        {
            if (_properties.TryGetValue(key, out string stringValue))
            {
                if (ulong.TryParse(stringValue, out ulong value))
                {
                    return value;
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <paramref name="defaultValue"/>。如果找到了指定键但无法转换为指定的类型，则仍返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValue">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public float GetValue(string key, float defaultValue)
        {
            if (_properties.TryGetValue(key, out string stringValue))
            {
                if (float.TryParse(stringValue, out float value))
                {
                    return value;
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <paramref name="defaultValue"/>。如果找到了指定键但无法转换为指定的类型，则仍返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValue">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public double GetValue(string key, double defaultValue)
        {
            if (_properties.TryGetValue(key, out string stringValue))
            {
                if (double.TryParse(stringValue, out double value))
                {
                    return value;
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <paramref name="defaultValue"/>。如果找到了指定键但无法转换为指定的类型，则仍返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValue">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public decimal GetValue(string key, decimal defaultValue)
        {
            if (_properties.TryGetValue(key, out string stringValue))
            {
                if (decimal.TryParse(stringValue, out decimal value))
                {
                    return value;
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <paramref name="defaultValue"/>。如果找到了指定键但无法转换为指定的类型，则仍返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValue">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public char GetValue(string key, char defaultValue)
        {
            if (_properties.TryGetValue(key, out string stringValue))
            {
                if (char.TryParse(stringValue, out char value))
                {
                    return value;
                }
            }
            return defaultValue;
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
            if (_properties.TryGetValue(key, out string stringValue))
            {
                if (XValueHelper.TryParse(stringValue, out byte[] value))
                {
                    return value;
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <paramref name="defaultValue"/>。如果找到了指定键但无法转换为指定的类型，则仍返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValue">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public TEnum GetValue<TEnum>(string key, TEnum defaultValue) where TEnum : struct
        {
            if (_properties.TryGetValue(key, out string stringValue))
            {
                if (typeof(TEnum).BaseType.FullName == "System.Enum")
                {
                    if (Enum.TryParse(stringValue, false, out TEnum value))
                    {
                        return value;
                    }
                }
            }
            return defaultValue;
        }

        #endregion GetValue

        #region TryGetValue

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string key, out string value)
        {
            return _properties.TryGetValue(key, out value);
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <see langword="false"/>。如果找到了指定键但无法转换指定的类型，则仍返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string key, out bool value)
        {
            if (_properties.TryGetValue(key, out string stringValue))
            {
                return bool.TryParse(stringValue, out value);
            }
            value = default;
            return false;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <see langword="false"/>。如果找到了指定键但无法转换指定的类型，则仍返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string key, out sbyte value)
        {
            if (_properties.TryGetValue(key, out string stringValue))
            {
                return sbyte.TryParse(stringValue, out value);
            }
            value = default;
            return false;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <see langword="false"/>。如果找到了指定键但无法转换指定的类型，则仍返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string key, out byte value)
        {
            if (_properties.TryGetValue(key, out string stringValue))
            {
                return byte.TryParse(stringValue, out value);
            }
            value = default;
            return false;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <see langword="false"/>。如果找到了指定键但无法转换指定的类型，则仍返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string key, out short value)
        {
            if (_properties.TryGetValue(key, out string stringValue))
            {
                return short.TryParse(stringValue, out value);
            }
            value = default;
            return false;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <see langword="false"/>。如果找到了指定键但无法转换指定的类型，则仍返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string key, out ushort value)
        {
            if (_properties.TryGetValue(key, out string stringValue))
            {
                return ushort.TryParse(stringValue, out value);
            }
            value = default;
            return false;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <see langword="false"/>。如果找到了指定键但无法转换指定的类型，则仍返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string key, out int value)
        {
            if (_properties.TryGetValue(key, out string stringValue))
            {
                return int.TryParse(stringValue, out value);
            }
            value = default;
            return false;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <see langword="false"/>。如果找到了指定键但无法转换指定的类型，则仍返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string key, out uint value)
        {
            if (_properties.TryGetValue(key, out string stringValue))
            {
                return uint.TryParse(stringValue, out value);
            }
            value = default;
            return false;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <see langword="false"/>。如果找到了指定键但无法转换指定的类型，则仍返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string key, out long value)
        {
            if (_properties.TryGetValue(key, out string stringValue))
            {
                return long.TryParse(stringValue, out value);
            }
            value = default;
            return false;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <see langword="false"/>。如果找到了指定键但无法转换指定的类型，则仍返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string key, out ulong value)
        {
            if (_properties.TryGetValue(key, out string stringValue))
            {
                return ulong.TryParse(stringValue, out value);
            }
            value = default;
            return false;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <see langword="false"/>。如果找到了指定键但无法转换指定的类型，则仍返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string key, out float value)
        {
            if (_properties.TryGetValue(key, out string stringValue))
            {
                return float.TryParse(stringValue, out value);
            }
            value = default;
            return false;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <see langword="false"/>。如果找到了指定键但无法转换指定的类型，则仍返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string key, out double value)
        {
            if (_properties.TryGetValue(key, out string stringValue))
            {
                return double.TryParse(stringValue, out value);
            }
            value = default;
            return false;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <see langword="false"/>。如果找到了指定键但无法转换指定的类型，则仍返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string key, out decimal value)
        {
            if (_properties.TryGetValue(key, out string stringValue))
            {
                return decimal.TryParse(stringValue, out value);
            }
            value = default;
            return false;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <see langword="false"/>。如果找到了指定键但无法转换指定的类型，则仍返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string key, out char value)
        {
            if (_properties.TryGetValue(key, out string stringValue))
            {
                return char.TryParse(stringValue, out value);
            }
            value = default;
            return false;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <see langword="false"/>。如果找到了指定键但无法转换指定的类型，则仍返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string key, out byte[] value)
        {
            if (_properties.TryGetValue(key, out string stringValue))
            {
                return XValueHelper.TryParse(stringValue, out value);
            }
            value = default;
            return false;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <see langword="false"/>。如果找到了指定键但无法转换指定的类型，则仍返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue<TEnum>(string key, out TEnum value) where TEnum : struct
        {
            if (_properties.TryGetValue(key, out string stringValue))
            {
                if (typeof(TEnum).BaseType.FullName == "System.Enum")
                {
                    return Enum.TryParse(stringValue, false, out value);
                }
            }
            value = default;
            return false;
        }

        #endregion TryGetValue

        #region Comment

        /// <summary>
        /// 删除一个与指定键关联的配置属性的注释。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool RemoveComment(string key)
        {
            if (_comments.TryGetValue(key, out XComment comment_))
            {
                if (comment_ != null)
                {
                    comment_.Remove();
                    _comments[key] = null;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的注释。
        /// <br/>如果没有找到指定键，返回 <see langword="false"/>。如果找到了指定键但没有注释节点，则仍返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="comment">配置属性的注释。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetComment(string key, out string comment)
        {
            if (_comments.TryGetValue(key, out XComment comment_))
            {
                if (comment_ != null)
                {
                    comment = comment_.Value;
                    return true;
                }
            }
            comment = null;
            return false;
        }

        /// <summary>
        /// 添加或更新或删除一个与指定键关联的配置属性的注释。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="comment">配置属性的注释。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TrySetComment(string key, string comment)
        {
            if (comment == null)
            {
                RemoveComment(key);
                return true;
            }
            else
            {
                if (_comments.TryGetValue(key, out XComment comment_))
                {
                    if (comment_ == null)
                    {
                        comment_ = new XComment(comment);
                        _contents[key].AddBeforeSelf(comment_);
                        _comments[key] = comment_;
                    }
                    else
                    {
                        comment_.Value = comment;
                    }
                    return true;
                }
            }
            return false;
        }

        #endregion Comment

        /// <summary>
        /// 从配置属性集合中移除所有配置属性。
        /// </summary>
        public void Clear()
        {
            _properties.Clear();
            _contents.Clear();
            _comments.Clear();
            _superior.RemoveNodes();
        }

        /// <summary>
        /// 确定配置属性集合是否包含带有指定键的配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool ContainsKey(string key)
        {
            return _properties.ContainsKey(key);
        }

        /// <summary>
        /// 返回循环访问集合的枚举数。
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return _properties.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _properties.GetEnumerator();
        }

        /// <summary>
        /// 从配置属性集合中移除带有指定键的配置属性。和指定键关联的配置属性的注释一并移除。
        /// <br/>如果该元素成功移除，返回 <see langword="true"/>。如果没有找到指定键，则返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool Remove(string key)
        {
            if (_properties.Remove(key))
            {
                _contents[key].Remove();
                _contents.Remove(key);
                _comments[key]?.Remove();
                _comments.Remove(key);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}