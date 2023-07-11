using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 配置属性集合。
    /// </summary>
    public class NameValuePropertySet : IEnumerable<KeyValuePair<string, string[]>>
    {
        #region Class

        /// <summary>
        /// 代表此配置属性集合的键的集合。
        /// </summary>
        public sealed class KeyCollection : IEnumerable<string>
        {
            #region Properties

            private readonly Dictionary<string, IList<string>> _properties;

            /// <summary>
            /// 获取配置属性集合的键的元素数。
            /// </summary>
            public int Count => _properties.Count;

            #endregion Properties

            internal KeyCollection(Dictionary<string, IList<string>> properties)
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
        public sealed class ValueCollection : IEnumerable<string[]>
        {
            #region Properties

            private readonly Dictionary<string, IList<string>> _properties;

            /// <summary>
            /// 获取配置属性集合的值的元素数。
            /// </summary>
            public int Count => _properties.Count;

            #endregion Properties

            internal ValueCollection(Dictionary<string, IList<string>> properties)
            {
                _properties = properties;
            }

            /// <summary>
            /// 从指定数组索引开始将值元素复制到到指定数组。
            /// </summary>
            /// <param name="array">要复制到的目标数组。</param>
            /// <param name="arrayIndex">目标数组中从零开始的索引，从此处开始复制。</param>
            public void CopyTo(string[][] array, int arrayIndex)
            {
                _properties.Values.CopyTo(array, arrayIndex);
            }

            /// <summary>
            /// 返回循环访问集合的枚举数。
            /// </summary>
            /// <returns></returns>
            public IEnumerator<string[]> GetEnumerator()
            {
                List<string[]> result = new List<string[]>();
                foreach (var item in _properties.Values)
                {
                    result.Add(item.ToArray());
                }
                return result.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return _properties.Values.GetEnumerator();
            }
        }

        #endregion Class

        #region Properties

        private readonly Dictionary<string, IList<XComment>> _comments = new Dictionary<string, IList<XComment>>();
        private readonly Dictionary<string, IList<XElement>> _contents = new Dictionary<string, IList<XElement>>();
        private readonly KeyCollection _keyExhibits;
        private readonly Dictionary<string, IList<string>> _properties = new Dictionary<string, IList<string>>();
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

        #endregion Properties

        #region Construction

        internal NameValuePropertySet(XElement superior)
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
                                if (_properties.TryGetValue(key, out IList<string> values))
                                {
                                    _contents.TryGetValue(key, out IList<XElement> contents);
                                    _comments.TryGetValue(key, out IList<XComment> comments);
                                    values.Add(value);
                                    contents.Add(content);
                                    comments.Add(comment);
                                }
                                else
                                {
                                    _properties.Add(key, new List<string>() { value });
                                    _contents.Add(key, new List<XElement>() { content });
                                    _comments.Add(key, new List<XComment>() { comment });
                                }
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
        /// <param name="values">配置属性的值。</param>
        /// <exception cref="Exception"/>
        public void AddOrUpdate(string key, IList<string> values)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException($"The invalid argument - {nameof(key)}.");
            }
            if (values == null || values.Count == 0)
            {
                Remove(key);
            }
            else
            {
                if (_properties.TryGetValue(key, out _))
                {
                    Remove(key);
                }
                List<XElement> contents = new List<XElement>();
                List<XComment> comments = new List<XComment>();
                foreach (var value in values)
                {
                    XElement content = new XElement("add");
                    content.SetAttributeValue("key", key);
                    content.SetAttributeValue("value", value);
                    contents.Add(content);
                    comments.Add(null);
                }
                _properties.Add(key, values);
                _contents.Add(key, contents);
                _comments.Add(key, comments);
                _superior.Add(contents);
            }
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <exception cref="Exception"/>
        public void AddOrUpdate(string key, IList<bool> values)
        {
            if (values == null || values.Count == 0)
            {
                Remove(key);
            }
            else
            {
                List<string> stringValues = new List<string>();
                foreach (var value in values)
                {
                    stringValues.Add(value.ToString());
                }
                AddOrUpdate(key, stringValues);
            }
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <exception cref="Exception"/>
        public void AddOrUpdate(string key, IList<sbyte> values)
        {
            if (values == null || values.Count == 0)
            {
                Remove(key);
            }
            else
            {
                List<string> stringValues = new List<string>();
                foreach (var value in values)
                {
                    stringValues.Add(value.ToString());
                }
                AddOrUpdate(key, stringValues);
            }
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <exception cref="Exception"/>
        public void AddOrUpdate(string key, IList<byte> values)
        {
            if (values == null || values.Count == 0)
            {
                Remove(key);
            }
            else
            {
                List<string> stringValues = new List<string>();
                foreach (var value in values)
                {
                    stringValues.Add(value.ToString());
                }
                AddOrUpdate(key, stringValues);
            }
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <exception cref="Exception"/>
        public void AddOrUpdate(string key, IList<short> values)
        {
            if (values == null || values.Count == 0)
            {
                Remove(key);
            }
            else
            {
                List<string> stringValues = new List<string>();
                foreach (var value in values)
                {
                    stringValues.Add(value.ToString());
                }
                AddOrUpdate(key, stringValues);
            }
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <exception cref="Exception"/>
        public void AddOrUpdate(string key, IList<ushort> values)
        {
            if (values == null || values.Count == 0)
            {
                Remove(key);
            }
            else
            {
                List<string> stringValues = new List<string>();
                foreach (var value in values)
                {
                    stringValues.Add(value.ToString());
                }
                AddOrUpdate(key, stringValues);
            }
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <exception cref="Exception"/>
        public void AddOrUpdate(string key, IList<int> values)
        {
            if (values == null || values.Count == 0)
            {
                Remove(key);
            }
            else
            {
                List<string> stringValues = new List<string>();
                foreach (var value in values)
                {
                    stringValues.Add(value.ToString());
                }
                AddOrUpdate(key, stringValues);
            }
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <exception cref="Exception"/>
        public void AddOrUpdate(string key, IList<uint> values)
        {
            if (values == null || values.Count == 0)
            {
                Remove(key);
            }
            else
            {
                List<string> stringValues = new List<string>();
                foreach (var value in values)
                {
                    stringValues.Add(value.ToString());
                }
                AddOrUpdate(key, stringValues);
            }
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <exception cref="Exception"/>
        public void AddOrUpdate(string key, IList<long> values)
        {
            if (values == null || values.Count == 0)
            {
                Remove(key);
            }
            else
            {
                List<string> stringValues = new List<string>();
                foreach (var value in values)
                {
                    stringValues.Add(value.ToString());
                }
                AddOrUpdate(key, stringValues);
            }
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <exception cref="Exception"/>
        public void AddOrUpdate(string key, IList<ulong> values)
        {
            if (values == null || values.Count == 0)
            {
                Remove(key);
            }
            else
            {
                List<string> stringValues = new List<string>();
                foreach (var value in values)
                {
                    stringValues.Add(value.ToString());
                }
                AddOrUpdate(key, stringValues);
            }
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <exception cref="Exception"/>
        public void AddOrUpdate(string key, IList<float> values)
        {
            if (values == null || values.Count == 0)
            {
                Remove(key);
            }
            else
            {
                List<string> stringValues = new List<string>();
                foreach (var value in values)
                {
                    stringValues.Add(value.ToString());
                }
                AddOrUpdate(key, stringValues);
            }
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <exception cref="Exception"/>
        public void AddOrUpdate(string key, IList<double> values)
        {
            if (values == null || values.Count == 0)
            {
                Remove(key);
            }
            else
            {
                List<string> stringValues = new List<string>();
                foreach (var value in values)
                {
                    stringValues.Add(value.ToString());
                }
                AddOrUpdate(key, stringValues);
            }
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <exception cref="Exception"/>
        public void AddOrUpdate(string key, IList<decimal> values)
        {
            if (values == null || values.Count == 0)
            {
                Remove(key);
            }
            else
            {
                List<string> stringValues = new List<string>();
                foreach (var value in values)
                {
                    stringValues.Add(value.ToString());
                }
                AddOrUpdate(key, stringValues);
            }
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <exception cref="Exception"/>
        public void AddOrUpdate(string key, IList<char> values)
        {
            if (values == null || values.Count == 0)
            {
                Remove(key);
            }
            else
            {
                List<string> stringValues = new List<string>();
                foreach (var value in values)
                {
                    stringValues.Add(value.ToString());
                }
                AddOrUpdate(key, stringValues);
            }
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <exception cref="Exception"/>
        public void AddOrUpdate(string key, IList<byte[]> values)
        {
            if (values == null || values.Count == 0)
            {
                Remove(key);
            }
            else
            {
                List<string> stringValues = new List<string>();
                foreach (var value in values)
                {
                    stringValues.Add(BitConverter.ToString(value).Replace("-", string.Empty));
                }
                AddOrUpdate(key, stringValues);
            }
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <exception cref="Exception"/>
        public void AddOrUpdate<TEnum>(string key, IList<TEnum> values) where TEnum : Enum
        {
            if (values == null || values.Count == 0)
            {
                Remove(key);
            }
            else
            {
                List<string> stringValues = new List<string>();
                foreach (var value in values)
                {
                    stringValues.Add(value.ToString());
                }
                AddOrUpdate(key, stringValues);
            }
        }

        #endregion AddOrUpdate

        #region GetValue

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValues">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public string[] GetValues(string key, IList<string> defaultValues)
        {
            return _properties.TryGetValue(key, out IList<string> values) ? values.ToArray() : defaultValues.ToArray();
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <paramref name="defaultValues"/>。如果找到了指定键但无法转换为指定的类型，则仍返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValues">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool[] GetValues(string key, IList<bool> defaultValues)
        {
            if (_properties.TryGetValue(key, out IList<string> stringValues))
            {
                List<bool> values = new List<bool>();
                foreach (var stringValue in stringValues)
                {
                    if (bool.TryParse(stringValue, out bool value))
                    {
                        values.Add(value);
                    }
                    else
                    {
                        return defaultValues.ToArray();
                    }
                }
                if (values.Count > 0)
                {
                    return values.ToArray();
                }
            }
            return defaultValues.ToArray();
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <paramref name="defaultValues"/>。如果找到了指定键但无法转换为指定的类型，则仍返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValues">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public sbyte[] GetValues(string key, IList<sbyte> defaultValues)
        {
            if (_properties.TryGetValue(key, out IList<string> stringValues))
            {
                List<sbyte> values = new List<sbyte>();
                foreach (var stringValue in stringValues)
                {
                    if (sbyte.TryParse(stringValue, out sbyte value))
                    {
                        values.Add(value);
                    }
                    else
                    {
                        return defaultValues.ToArray();
                    }
                }
                if (values.Count > 0)
                {
                    return values.ToArray();
                }
            }
            return defaultValues.ToArray();
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <paramref name="defaultValues"/>。如果找到了指定键但无法转换为指定的类型，则仍返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValues">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public byte[] GetValues(string key, IList<byte> defaultValues)
        {
            if (_properties.TryGetValue(key, out IList<string> stringValues))
            {
                List<byte> values = new List<byte>();
                foreach (var stringValue in stringValues)
                {
                    if (byte.TryParse(stringValue, out byte value))
                    {
                        values.Add(value);
                    }
                    else
                    {
                        return defaultValues.ToArray();
                    }
                }
                if (values.Count > 0)
                {
                    return values.ToArray();
                }
            }
            return defaultValues.ToArray();
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <paramref name="defaultValues"/>。如果找到了指定键但无法转换为指定的类型，则仍返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValues">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public short[] GetValues(string key, IList<short> defaultValues)
        {
            if (_properties.TryGetValue(key, out IList<string> stringValues))
            {
                List<short> values = new List<short>();
                foreach (var stringValue in stringValues)
                {
                    if (short.TryParse(stringValue, out short value))
                    {
                        values.Add(value);
                    }
                    else
                    {
                        return defaultValues.ToArray();
                    }
                }
                if (values.Count > 0)
                {
                    return values.ToArray();
                }
            }
            return defaultValues.ToArray();
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <paramref name="defaultValues"/>。如果找到了指定键但无法转换为指定的类型，则仍返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValues">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public ushort[] GetValues(string key, IList<ushort> defaultValues)
        {
            if (_properties.TryGetValue(key, out IList<string> stringValues))
            {
                List<ushort> values = new List<ushort>();
                foreach (var stringValue in stringValues)
                {
                    if (ushort.TryParse(stringValue, out ushort value))
                    {
                        values.Add(value);
                    }
                    else
                    {
                        return defaultValues.ToArray();
                    }
                }
                if (values.Count > 0)
                {
                    return values.ToArray();
                }
            }
            return defaultValues.ToArray();
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <paramref name="defaultValues"/>。如果找到了指定键但无法转换为指定的类型，则仍返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValues">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public int[] GetValues(string key, IList<int> defaultValues)
        {
            if (_properties.TryGetValue(key, out IList<string> stringValues))
            {
                List<int> values = new List<int>();
                foreach (var stringValue in stringValues)
                {
                    if (int.TryParse(stringValue, out int value))
                    {
                        values.Add(value);
                    }
                    else
                    {
                        return defaultValues.ToArray();
                    }
                }
                if (values.Count > 0)
                {
                    return values.ToArray();
                }
            }
            return defaultValues.ToArray();
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <paramref name="defaultValues"/>。如果找到了指定键但无法转换为指定的类型，则仍返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValues">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public uint[] GetValues(string key, IList<uint> defaultValues)
        {
            if (_properties.TryGetValue(key, out IList<string> stringValues))
            {
                List<uint> values = new List<uint>();
                foreach (var stringValue in stringValues)
                {
                    if (uint.TryParse(stringValue, out uint value))
                    {
                        values.Add(value);
                    }
                    else
                    {
                        return defaultValues.ToArray();
                    }
                }
                if (values.Count > 0)
                {
                    return values.ToArray();
                }
            }
            return defaultValues.ToArray();
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <paramref name="defaultValues"/>。如果找到了指定键但无法转换为指定的类型，则仍返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValues">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public long[] GetValues(string key, IList<long> defaultValues)
        {
            if (_properties.TryGetValue(key, out IList<string> stringValues))
            {
                List<long> values = new List<long>();
                foreach (var stringValue in stringValues)
                {
                    if (long.TryParse(stringValue, out long value))
                    {
                        values.Add(value);
                    }
                    else
                    {
                        return defaultValues.ToArray();
                    }
                }
                if (values.Count > 0)
                {
                    return values.ToArray();
                }
            }
            return defaultValues.ToArray();
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <paramref name="defaultValues"/>。如果找到了指定键但无法转换为指定的类型，则仍返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValues">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public ulong[] GetValues(string key, IList<ulong> defaultValues)
        {
            if (_properties.TryGetValue(key, out IList<string> stringValues))
            {
                List<ulong> values = new List<ulong>();
                foreach (var stringValue in stringValues)
                {
                    if (ulong.TryParse(stringValue, out ulong value))
                    {
                        values.Add(value);
                    }
                    else
                    {
                        return defaultValues.ToArray();
                    }
                }
                if (values.Count > 0)
                {
                    return values.ToArray();
                }
            }
            return defaultValues.ToArray();
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <paramref name="defaultValues"/>。如果找到了指定键但无法转换为指定的类型，则仍返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValues">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public float[] GetValues(string key, IList<float> defaultValues)
        {
            if (_properties.TryGetValue(key, out IList<string> stringValues))
            {
                List<float> values = new List<float>();
                foreach (var stringValue in stringValues)
                {
                    if (float.TryParse(stringValue, out float value))
                    {
                        values.Add(value);
                    }
                    else
                    {
                        return defaultValues.ToArray();
                    }
                }
                if (values.Count > 0)
                {
                    return values.ToArray();
                }
            }
            return defaultValues.ToArray();
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <paramref name="defaultValues"/>。如果找到了指定键但无法转换为指定的类型，则仍返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValues">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public double[] GetValues(string key, IList<double> defaultValues)
        {
            if (_properties.TryGetValue(key, out IList<string> stringValues))
            {
                List<double> values = new List<double>();
                foreach (var stringValue in stringValues)
                {
                    if (double.TryParse(stringValue, out double value))
                    {
                        values.Add(value);
                    }
                    else
                    {
                        return defaultValues.ToArray();
                    }
                }
                if (values.Count > 0)
                {
                    return values.ToArray();
                }
            }
            return defaultValues.ToArray();
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <paramref name="defaultValues"/>。如果找到了指定键但无法转换为指定的类型，则仍返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValues">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public decimal[] GetValues(string key, IList<decimal> defaultValues)
        {
            if (_properties.TryGetValue(key, out IList<string> stringValues))
            {
                List<decimal> values = new List<decimal>();
                foreach (var stringValue in stringValues)
                {
                    if (decimal.TryParse(stringValue, out decimal value))
                    {
                        values.Add(value);
                    }
                    else
                    {
                        return defaultValues.ToArray();
                    }
                }
                if (values.Count > 0)
                {
                    return values.ToArray();
                }
            }
            return defaultValues.ToArray();
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <paramref name="defaultValues"/>。如果找到了指定键但无法转换为指定的类型，则仍返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValues">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public char[] GetValues(string key, IList<char> defaultValues)
        {
            if (_properties.TryGetValue(key, out IList<string> stringValues))
            {
                List<char> values = new List<char>();
                foreach (var stringValue in stringValues)
                {
                    if (char.TryParse(stringValue, out char value))
                    {
                        values.Add(value);
                    }
                    else
                    {
                        return defaultValues.ToArray();
                    }
                }
                if (values.Count > 0)
                {
                    return values.ToArray();
                }
            }
            return defaultValues.ToArray();
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <paramref name="defaultValues"/>。如果找到了指定键但无法转换为指定的类型，则仍返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValues">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public byte[][] GetValues(string key, IList<byte[]> defaultValues)
        {
            if (_properties.TryGetValue(key, out IList<string> stringValues))
            {
                List<byte[]> values = new List<byte[]>();
                foreach (var stringValue in stringValues)
                {
                    if (XValueHelper.TryParse(stringValue, out byte[] value))
                    {
                        values.Add(value);
                    }
                    else
                    {
                        return defaultValues.ToArray();
                    }
                }
                if (values.Count > 0)
                {
                    return values.ToArray();
                }
            }
            return defaultValues.ToArray();
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <paramref name="defaultValues"/>。如果找到了指定键但无法转换为指定的类型，则仍返回 <paramref name="defaultValues"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValues">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public TEnum[] GetValues<TEnum>(string key, IList<TEnum> defaultValues) where TEnum : struct
        {
            if (_properties.TryGetValue(key, out IList<string> stringValues))
            {
                if (typeof(TEnum).BaseType.FullName == "System.Enum")
                {
                    List<TEnum> values = new List<TEnum>();
                    foreach (var stringValue in stringValues)
                    {
                        if (Enum.TryParse(stringValue, false, out TEnum value))
                        {
                            values.Add(value);
                        }
                        else
                        {
                            return defaultValues.ToArray();
                        }
                    }
                    if (values.Count > 0)
                    {
                        return values.ToArray();
                    }
                }
            }
            return defaultValues.ToArray();
        }

        #endregion GetValue

        #region TryGetValue

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
            if (_properties.TryGetValue(key, out IList<string> stringValues))
            {
                if (typeof(TEnum).BaseType.FullName == "System.Enum")
                {
                    List<TEnum> result = new List<TEnum>();
                    foreach (var stringValue in stringValues)
                    {
                        if (Enum.TryParse(stringValue, false, out TEnum value))
                        {
                            result.Add(value);
                        }
                        else
                        {
                            values = default;
                            return false;
                        }
                    }
                    if (result.Count > 0)
                    {
                        values = result.ToArray();
                        return true;
                    }
                }
            }
            values = default;
            return false;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValues(string key, out IList<string> values)
        {
            if (_properties.TryGetValue(key, out IList<string> stringValues))
            {
                values = stringValues.ToArray();
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
        public bool TryGetValues(string key, out bool[] values)
        {
            if (_properties.TryGetValue(key, out IList<string> stringValues))
            {
                List<bool> result = new List<bool>();
                foreach (var stringValue in stringValues)
                {
                    if (bool.TryParse(stringValue, out bool value))
                    {
                        result.Add(value);
                    }
                    else
                    {
                        values = default;
                        return false;
                    }
                }
                if (result.Count > 0)
                {
                    values = result.ToArray();
                    return true;
                }
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
        public bool TryGetValues(string key, out sbyte[] values)
        {
            if (_properties.TryGetValue(key, out IList<string> stringValues))
            {
                List<sbyte> result = new List<sbyte>();
                foreach (var stringValue in stringValues)
                {
                    if (sbyte.TryParse(stringValue, out sbyte value))
                    {
                        result.Add(value);
                    }
                    else
                    {
                        values = default;
                        return false;
                    }
                }
                if (result.Count > 0)
                {
                    values = result.ToArray();
                    return true;
                }
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
        public bool TryGetValues(string key, out byte[] values)
        {
            if (_properties.TryGetValue(key, out IList<string> stringValues))
            {
                List<byte> result = new List<byte>();
                foreach (var stringValue in stringValues)
                {
                    if (byte.TryParse(stringValue, out byte value))
                    {
                        result.Add(value);
                    }
                    else
                    {
                        values = default;
                        return false;
                    }
                }
                if (result.Count > 0)
                {
                    values = result.ToArray();
                    return true;
                }
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
        public bool TryGetValues(string key, out short[] values)
        {
            if (_properties.TryGetValue(key, out IList<string> stringValues))
            {
                List<short> result = new List<short>();
                foreach (var stringValue in stringValues)
                {
                    if (short.TryParse(stringValue, out short value))
                    {
                        result.Add(value);
                    }
                    else
                    {
                        values = default;
                        return false;
                    }
                }
                if (result.Count > 0)
                {
                    values = result.ToArray();
                    return true;
                }
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
        public bool TryGetValues(string key, out ushort[] values)
        {
            if (_properties.TryGetValue(key, out IList<string> stringValues))
            {
                List<ushort> result = new List<ushort>();
                foreach (var stringValue in stringValues)
                {
                    if (ushort.TryParse(stringValue, out ushort value))
                    {
                        result.Add(value);
                    }
                    else
                    {
                        values = default;
                        return false;
                    }
                }
                if (result.Count > 0)
                {
                    values = result.ToArray();
                    return true;
                }
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
        public bool TryGetValues(string key, out int[] values)
        {
            if (_properties.TryGetValue(key, out IList<string> stringValues))
            {
                List<int> result = new List<int>();
                foreach (var stringValue in stringValues)
                {
                    if (int.TryParse(stringValue, out int value))
                    {
                        result.Add(value);
                    }
                    else
                    {
                        values = default;
                        return false;
                    }
                }
                if (result.Count > 0)
                {
                    values = result.ToArray();
                    return true;
                }
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
        public bool TryGetValues(string key, out uint[] values)
        {
            if (_properties.TryGetValue(key, out IList<string> stringValues))
            {
                List<uint> result = new List<uint>();
                foreach (var stringValue in stringValues)
                {
                    if (uint.TryParse(stringValue, out uint value))
                    {
                        result.Add(value);
                    }
                    else
                    {
                        values = default;
                        return false;
                    }
                }
                if (result.Count > 0)
                {
                    values = result.ToArray();
                    return true;
                }
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
        public bool TryGetValues(string key, out long[] values)
        {
            if (_properties.TryGetValue(key, out IList<string> stringValues))
            {
                List<long> result = new List<long>();
                foreach (var stringValue in stringValues)
                {
                    if (long.TryParse(stringValue, out long value))
                    {
                        result.Add(value);
                    }
                    else
                    {
                        values = default;
                        return false;
                    }
                }
                if (result.Count > 0)
                {
                    values = result.ToArray();
                    return true;
                }
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
        public bool TryGetValues(string key, out ulong[] values)
        {
            if (_properties.TryGetValue(key, out IList<string> stringValues))
            {
                List<ulong> result = new List<ulong>();
                foreach (var stringValue in stringValues)
                {
                    if (ulong.TryParse(stringValue, out ulong value))
                    {
                        result.Add(value);
                    }
                    else
                    {
                        values = default;
                        return false;
                    }
                }
                if (result.Count > 0)
                {
                    values = result.ToArray();
                    return true;
                }
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
        public bool TryGetValues(string key, out float[] values)
        {
            if (_properties.TryGetValue(key, out IList<string> stringValues))
            {
                List<float> result = new List<float>();
                foreach (var stringValue in stringValues)
                {
                    if (float.TryParse(stringValue, out float value))
                    {
                        result.Add(value);
                    }
                    else
                    {
                        values = default;
                        return false;
                    }
                }
                if (result.Count > 0)
                {
                    values = result.ToArray();
                    return true;
                }
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
        public bool TryGetValues(string key, out double[] values)
        {
            if (_properties.TryGetValue(key, out IList<string> stringValues))
            {
                List<double> result = new List<double>();
                foreach (var stringValue in stringValues)
                {
                    if (double.TryParse(stringValue, out double value))
                    {
                        result.Add(value);
                    }
                    else
                    {
                        values = default;
                        return false;
                    }
                }
                if (result.Count > 0)
                {
                    values = result.ToArray();
                    return true;
                }
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
        public bool TryGetValues(string key, out decimal[] values)
        {
            if (_properties.TryGetValue(key, out IList<string> stringValues))
            {
                List<decimal> result = new List<decimal>();
                foreach (var stringValue in stringValues)
                {
                    if (decimal.TryParse(stringValue, out decimal value))
                    {
                        result.Add(value);
                    }
                    else
                    {
                        values = default;
                        return false;
                    }
                }
                if (result.Count > 0)
                {
                    values = result.ToArray();
                    return true;
                }
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
        public bool TryGetValues(string key, out char[] values)
        {
            if (_properties.TryGetValue(key, out IList<string> stringValues))
            {
                List<char> result = new List<char>();
                foreach (var stringValue in stringValues)
                {
                    if (char.TryParse(stringValue, out char value))
                    {
                        result.Add(value);
                    }
                    else
                    {
                        values = default;
                        return false;
                    }
                }
                if (result.Count > 0)
                {
                    values = result.ToArray();
                    return true;
                }
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
        public bool TryGetValues(string key, out byte[][] values)
        {
            if (_properties.TryGetValue(key, out IList<string> stringValues))
            {
                List<byte[]> result = new List<byte[]>();
                foreach (var stringValue in stringValues)
                {
                    if (XValueHelper.TryParse(stringValue, out byte[] value))
                    {
                        result.Add(value);
                    }
                    else
                    {
                        values = default;
                        return false;
                    }
                }
                if (result.Count > 0)
                {
                    values = result.ToArray();
                    return true;
                }
            }
            values = default;
            return false;
        }

        #endregion TryGetValue

        #region Comment

        /// <summary>
        /// 删除一个与指定键关联的配置属性的注释。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="index">和键相关的配置属性值列表的索引。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool RemoveComment(string key, int index)
        {
            if (_comments.TryGetValue(key, out IList<XComment> comments))
            {
                if (index < comments.Count)
                {
                    if (comments[index] != null)
                    {
                        comments[index].Remove();
                        comments[index] = null;
                    }
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的注释。
        /// <br/>如果没有找到指定键，返回 <see langword="false"/>。如果找到了指定键但没有注释节点，则仍返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="index">和键相关的配置属性值列表的索引。</param>
        /// <param name="comment">配置属性的注释。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetComment(string key, int index, out string comment)
        {
            if (_comments.TryGetValue(key, out IList<XComment> comments))
            {
                if (index < comments.Count)
                {
                    if (comments[index] != null)
                    {
                        comment = comments[index].Value;
                        return true;
                    }
                }
            }
            comment = null;
            return false;
        }

        /// <summary>
        /// 添加或更新或删除一个与指定键关联的配置属性的注释。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="index">和键相关的配置属性值列表的索引。</param>
        /// <param name="comment">配置属性的注释。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TrySetComment(string key, int index, string comment)
        {
            if (comment == null)
            {
                RemoveComment(key, index);
                return true;
            }
            else
            {
                if (_comments.TryGetValue(key, out IList<XComment> comments))
                {
                    if (index < comments.Count)
                    {
                        if (comments[index] == null)
                        {
                            XComment comment_ = new XComment(comment);
                            _contents[key][index].AddBeforeSelf(comment_);
                            comments[index] = comment_;
                        }
                        else
                        {
                            comments[index].Value = comment;
                        }
                        return true;
                    }
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
        public IEnumerator<KeyValuePair<string, string[]>> GetEnumerator()
        {
            List<KeyValuePair<string, string[]>> result = new List<KeyValuePair<string, string[]>>();
            foreach (var item in _properties)
            {
                result.Add(new KeyValuePair<string, string[]>(item.Key, item.Value.ToArray()));
            }
            return result.GetEnumerator();
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