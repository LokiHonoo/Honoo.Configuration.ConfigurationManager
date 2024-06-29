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
    public class DictionaryPropertySet : IEnumerable<ConfigurationProperty>
    {
        #region Properties

        private readonly XElement _container;
        private readonly HashSet<string> _keys = new HashSet<string>();
        private readonly List<ConfigurationProperty> _properties = new List<ConfigurationProperty>();

        /// <summary>
        /// 获取配置属性集合中包含的元素数。
        /// </summary>
        public int Count => _properties.Count;

        #endregion Properties

        #region Construction

        internal DictionaryPropertySet(XElement container)
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
                                if (_keys.Contains(property.Key))
                                {
                                    for (int i = _properties.Count - 1; i >= 0; i--)
                                    {
                                        if (_properties[i] is AddProperty prop)
                                        {
                                            if (prop.Key == property.Key)
                                            {
                                                _keys.Remove(property.Key);
                                                _properties.RemoveAt(i);
                                            }
                                        }
                                    }
                                }
                                _keys.Add(property.Key);
                                _properties.Add(property);
                            }
                            else if (content.Name == "remove")
                            {
                                RemoveProperty property = new RemoveProperty(content, comment);
                                _properties.Add(property);
                            }
                            else if (content.Name == "clear")
                            {
                                ClearProperty property = new ClearProperty(content, comment);
                                _properties.Add(property);
                            }
                        }
                        comment = null;
                    }
                }
            }
        }

        #endregion Construction

        #region Add

        /// <summary>
        /// 添加一个配置属性。
        /// </summary>
        /// <param name="property">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public ConfigurationProperty Add(ConfigurationProperty property)
        {
            if (property == null)
            {
                throw new ArgumentException($"The invalid argument - {nameof(property)}.");
            }
            switch (property.Kind)
            {
                case ConfigurationPropertyKind.AddProperty: return Add((AddProperty)property);
                case ConfigurationPropertyKind.RemoveProperty: return Add((RemoveProperty)property);
                case ConfigurationPropertyKind.ClearProperty: return Add((ClearProperty)property);
                default: throw new ArgumentException($"The invalid property kind - {nameof(property)}.");
            }
        }

        /// <summary>
        /// 添加一个配置属性。
        /// </summary>
        /// <param name="property">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public AddProperty Add(AddProperty property)
        {
            if (string.IsNullOrWhiteSpace(property.Key))
            {
                throw new ArgumentException($"The invalid argument - {nameof(property)}.");
            }
            if (property.Comment != null)
            {
                _container.Add(property.Comment);
            }
            _container.Add(property.Content);
            _keys.Add(property.Key);
            _properties.Add(property);
            return property;
        }

        /// <summary>
        /// 添加一个配置属性。
        /// </summary>
        /// <param name="property">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public RemoveProperty Add(RemoveProperty property)
        {
            if (string.IsNullOrWhiteSpace(property.Key))
            {
                throw new ArgumentException($"The invalid argument - {nameof(property)}.");
            }
            if (property.Comment != null)
            {
                _container.Add(property.Comment);
            }
            _container.Add(property.Content);
            _properties.Add(property);
            return property;
        }

        /// <summary>
        /// 添加一个配置属性。
        /// </summary>
        /// <param name="property">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public ClearProperty Add(ClearProperty property)
        {
            if (property.Comment != null)
            {
                _container.Add(property.Comment);
            }
            _container.Add(property.Content);
            _properties.Add(property);
            return property;
        }

        /// <summary>
        /// 添加一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public AddProperty Add(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException($"The invalid argument - {nameof(key)}.");
            }
            return Add(new AddProperty(key, value));
        }

        /// <summary>
        /// 添加一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public AddProperty Add(string key, bool value)
        {
            return Add(key, value.ToString());
        }

        /// <summary>
        /// 添加一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public AddProperty Add(string key, sbyte value)
        {
            return Add(key, value.ToString());
        }

        /// <summary>
        /// 添加一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public AddProperty Add(string key, byte value)
        {
            return Add(key, value.ToString());
        }

        /// <summary>
        /// 添加一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public AddProperty Add(string key, short value)
        {
            return Add(key, value.ToString());
        }

        /// <summary>
        /// 添加一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public AddProperty Add(string key, ushort value)
        {
            return Add(key, value.ToString());
        }

        /// <summary>
        /// 添加一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public AddProperty Add(string key, int value)
        {
            return Add(key, value.ToString());
        }

        /// <summary>
        /// 添加一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public AddProperty Add(string key, uint value)
        {
            return Add(key, value.ToString());
        }

        /// <summary>
        /// 添加一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public AddProperty Add(string key, long value)
        {
            return Add(key, value.ToString());
        }

        /// <summary>
        /// 添加一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public AddProperty Add(string key, ulong value)
        {
            return Add(key, value.ToString());
        }

        /// <summary>
        /// 添加一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public AddProperty Add(string key, float value)
        {
            return Add(key, value.ToString());
        }

        /// <summary>
        /// 添加一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public AddProperty Add(string key, double value)
        {
            return Add(key, value.ToString());
        }

        /// <summary>
        /// 添加一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public AddProperty Add(string key, decimal value)
        {
            return Add(key, value.ToString());
        }

        /// <summary>
        /// 添加一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public AddProperty Add(string key, char value)
        {
            return Add(key, value.ToString());
        }

        /// <summary>
        /// 添加一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public AddProperty Add(string key, byte[] value)
        {
            return Add(key, BitConverter.ToString(value).Replace("-", string.Empty));
        }

        /// <summary>
        /// 添加一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public AddProperty Add<TEnum>(string key, TEnum value) where TEnum : Enum
        {
            return Add(key, value.ToString());
        }

        #endregion Add

        #region AddOrUpdate

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="property">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public AddProperty AddOrUpdate(AddProperty property)
        {
            if (string.IsNullOrWhiteSpace(property.Key))
            {
                throw new ArgumentException($"The invalid argument - {nameof(property)}.");
            }
            Remove(property.Key);
            return Add(property);
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public AddProperty AddOrUpdate(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException($"The invalid argument - {nameof(key)}.");
            }
            return AddOrUpdate(new AddProperty(key, value));
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public AddProperty AddOrUpdate(string key, bool value)
        {
            return AddOrUpdate(key, value.ToString());
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public AddProperty AddOrUpdate(string key, sbyte value)
        {
            return AddOrUpdate(key, value.ToString());
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public AddProperty AddOrUpdate(string key, byte value)
        {
            return AddOrUpdate(key, value.ToString());
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public AddProperty AddOrUpdate(string key, short value)
        {
            return AddOrUpdate(key, value.ToString());
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public AddProperty AddOrUpdate(string key, ushort value)
        {
            return AddOrUpdate(key, value.ToString());
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public AddProperty AddOrUpdate(string key, int value)
        {
            return AddOrUpdate(key, value.ToString());
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public AddProperty AddOrUpdate(string key, uint value)
        {
            return AddOrUpdate(key, value.ToString());
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public AddProperty AddOrUpdate(string key, long value)
        {
            return AddOrUpdate(key, value.ToString());
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public AddProperty AddOrUpdate(string key, ulong value)
        {
            return AddOrUpdate(key, value.ToString());
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public AddProperty AddOrUpdate(string key, float value)
        {
            return AddOrUpdate(key, value.ToString());
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public AddProperty AddOrUpdate(string key, double value)
        {
            return AddOrUpdate(key, value.ToString());
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public AddProperty AddOrUpdate(string key, decimal value)
        {
            return AddOrUpdate(key, value.ToString());
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public AddProperty AddOrUpdate(string key, char value)
        {
            return AddOrUpdate(key, value.ToString());
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public AddProperty AddOrUpdate(string key, byte[] value)
        {
            return AddOrUpdate(key, BitConverter.ToString(value).Replace("-", string.Empty));
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public AddProperty AddOrUpdate<TEnum>(string key, TEnum value) where TEnum : Enum
        {
            return AddOrUpdate(key, value.ToString());
        }

        #endregion AddOrUpdate

        #region TryGetValue

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="property">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string key, out AddProperty property)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException($"The invalid argument - {nameof(key)}.");
            }
            for (int i = _properties.Count - 1; i >= 0; i--)
            {
                if (_properties[i] is AddProperty prop)
                {
                    if (prop.Key == key)
                    {
                        property = prop;
                        return true;
                    }
                }
            }
            property = null;
            return false;
        }

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
            if (TryGetValue(key, out AddProperty val))
            {
                value = val.Value;
                return true;
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
        public bool TryGetValue(string key, out bool value)
        {
            if (TryGetValue(key, out AddProperty val))
            {
                return bool.TryParse(val.Value, out value);
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
            if (TryGetValue(key, out AddProperty val))
            {
                return sbyte.TryParse(val.Value, out value);
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
            if (TryGetValue(key, out AddProperty val))
            {
                return byte.TryParse(val.Value, out value);
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
            if (TryGetValue(key, out AddProperty val))
            {
                return short.TryParse(val.Value, out value);
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
            if (TryGetValue(key, out AddProperty val))
            {
                return ushort.TryParse(val.Value, out value);
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
            if (TryGetValue(key, out AddProperty val))
            {
                return int.TryParse(val.Value, out value);
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
            if (TryGetValue(key, out AddProperty val))
            {
                return uint.TryParse(val.Value, out value);
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
            if (TryGetValue(key, out AddProperty val))
            {
                return long.TryParse(val.Value, out value);
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
            if (TryGetValue(key, out AddProperty val))
            {
                return ulong.TryParse(val.Value, out value);
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
            if (TryGetValue(key, out AddProperty val))
            {
                return float.TryParse(val.Value, out value);
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
            if (TryGetValue(key, out AddProperty val))
            {
                return double.TryParse(val.Value, out value);
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
            if (TryGetValue(key, out AddProperty val))
            {
                return decimal.TryParse(val.Value, out value);
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
            if (TryGetValue(key, out AddProperty val))
            {
                return char.TryParse(val.Value, out value);
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
            if (TryGetValue(key, out AddProperty val))
            {
                return XValueHelper.TryParse(val.Value, out value);
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
            if (typeof(TEnum).BaseType.FullName == "System.Enum")
            {
                if (TryGetValue(key, out AddProperty val))
                {
                    return Enum.TryParse(val.Value, false, out value);
                }
            }
            value = default;
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
        public AddProperty GetValue(string key)
        {
            return TryGetValue(key, out AddProperty value) ? value : null;
        }

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
            return TryGetValue(key, out string value) ? value : defaultValue;
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
            return TryGetValue(key, out bool value) ? value : defaultValue;
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
            return TryGetValue(key, out sbyte value) ? value : defaultValue;
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
            return TryGetValue(key, out byte value) ? value : defaultValue;
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
            return TryGetValue(key, out short value) ? value : defaultValue;
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
            return TryGetValue(key, out ushort value) ? value : defaultValue;
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
            return TryGetValue(key, out int value) ? value : defaultValue;
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
            return TryGetValue(key, out uint value) ? value : defaultValue;
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
            return TryGetValue(key, out long value) ? value : defaultValue;
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
            return TryGetValue(key, out ulong value) ? value : defaultValue;
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
            return TryGetValue(key, out float value) ? value : defaultValue;
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
            return TryGetValue(key, out double value) ? value : defaultValue;
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
            return TryGetValue(key, out decimal value) ? value : defaultValue;
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
            return TryGetValue(key, out char value) ? value : defaultValue;
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
            return TryGetValue(key, out byte[] value) ? value : defaultValue;
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
            return TryGetValue(key, out TEnum value) ? value : defaultValue;
        }

        #endregion GetValue

        /// <summary>
        /// 从配置属性集合中移除所有配置属性。
        /// </summary>
        public void Clear()
        {
            _container.RemoveNodes();
            _keys.Clear();
            _properties.Clear();
        }

        /// <summary>
        /// 确定配置属性集合是否包含带有指定键的配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool Contains(string key)
        {
            return _keys.Contains(key);
        }

        /// <summary>
        /// 返回循环访问集合的枚举数。
        /// </summary>
        /// <returns></returns>
        public IEnumerator<ConfigurationProperty> GetEnumerator()
        {
            return _properties.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _properties.GetEnumerator();
        }

        /// <summary>
        /// 从配置属性集合中移除配置属性。配置属性的注释一并移除。
        /// <br/>如果该元素成功移除，返回 <see langword="true"/>。如果没有找到指定元素，则返回 <see langword="false"/>。
        /// </summary>
        /// <param name="property">配置属性。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool Remove(ConfigurationProperty property)
        {
            bool removed = _properties.Remove(property);
            property.Comment?.Remove();
            property.Content.Remove();
            if (property is AddProperty prop)
            {
                _keys.Remove(prop.Key);
            }
            return removed;
        }

        /// <summary>
        /// 从配置属性集合中移除带有指定键的配置属性。和指定键关联的配置属性的注释一并移除。
        /// <br/>如果该元素成功移除，返回 <see langword="true"/>。如果没有找到指定元素，则返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool Remove(string key)
        {
            for (int i = _properties.Count - 1; i >= 0; i--)
            {
                if (_properties[i] is AddProperty property)
                {
                    if (property.Key == key)
                    {
                        property.Comment?.Remove();
                        property.Content.Remove();
                        _keys.Remove(key);
                        _properties.RemoveAt(i);
                        return true;
                    }
                }
            }
            return false;
        }
    }
}