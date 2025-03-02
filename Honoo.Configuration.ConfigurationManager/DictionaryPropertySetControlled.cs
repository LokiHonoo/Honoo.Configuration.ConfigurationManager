using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 应用 file 属性以及 &lt;remove /&gt;、&lt;clear /&gt; 标签后的只读配置属性集合。
    /// </summary>
    public sealed class DictionaryPropertySetControlled : IEnumerable<KeyValuePair<string, AddProperty>>
    {
        #region Members

        private readonly Dictionary<string, AddProperty> _properties = new Dictionary<string, AddProperty>();

        /// <summary>
        /// 获取配置属性集合中包含的元素数。
        /// </summary>
        public int Count => _properties.Count;

        /// <summary>
        /// 获取配置属性集合的键的集合。
        /// </summary>
        public Dictionary<string, AddProperty>.KeyCollection Keys => _properties.Keys;

        /// <summary>
        /// 获取配置属性集合的值的集合。
        /// </summary>
        public Dictionary<string, AddProperty>.ValueCollection Values => _properties.Values;

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public AddProperty this[string key] => GetValue(key);

        #endregion Members

        #region Construction

        internal DictionaryPropertySetControlled(XElement container)
        {
            LoadProperties(container);
        }

        private void LoadProperties(XElement container)
        {
            if (container.Attribute("file") is XAttribute attribute)
            {
                string file = attribute.Value;
                if (!string.IsNullOrEmpty(file) && File.Exists(file))
                {
                    XElement extra = XElement.Load(file);
                    if (extra.Name.LocalName == "appSettings")
                    {
                        LoadProperties(extra);
                    }
                }
            }
            //
            if (container.HasElements)
            {
                IEnumerator<XNode> enumerator = container.Nodes().GetEnumerator();
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
                                var key = content.Attribute("key").Value;
                                AddProperty value = new AddProperty(content, comment);
                                _properties.Remove(key);
                                _properties.Add(key, value);
                            }
                            else if (content.Name == "remove")
                            {
                                var key = content.Attribute("key").Value;
                                _properties.Remove(key);
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
        /// <br/>如果没有找到指定键，返回 <see langword="false"/>。如果找到了指定键但指定的类型不符，则仍返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="property">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string key, out AddProperty property)
        {
            return _properties.TryGetValue(key, out property);
        }

        #endregion TryGetValue

        #region GetValue

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，则抛出 <see cref="Exception"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public AddProperty GetValue(string key)
        {
            return _properties[key];
        }

        #endregion GetValue

        #region GetValueOrDefault

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，返回 <paramref name="defaultProperty"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultProperty">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public AddProperty GetValue(string key, AddProperty defaultProperty)
        {
            return TryGetValue(key, out AddProperty value) ? value : defaultProperty;
        }

        #endregion GetValueOrDefault

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
        public IEnumerator<KeyValuePair<string, AddProperty>> GetEnumerator()
        {
            return _properties.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _properties.GetEnumerator();
        }
    }
}