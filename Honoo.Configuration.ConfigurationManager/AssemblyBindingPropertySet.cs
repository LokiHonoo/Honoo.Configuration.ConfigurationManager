using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 配置文件链接属性集合。
    /// </summary>
    public sealed class AssemblyBindingPropertySet : IEnumerable<LinkedConfigurationProperty>
    {
        #region Properties

        private readonly XElement _container;
        private readonly List<LinkedConfigurationProperty> _properties = new List<LinkedConfigurationProperty>();

        /// <summary>
        /// 获取配置属性集合中包含的元素数。
        /// </summary>
        public int Count => _properties.Count;

        #endregion Properties

        #region Construction

        internal AssemblyBindingPropertySet(XElement container)
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
                            if (content.Name == ((XNamespace)"urn:schemas-microsoft-com:asm.v1") + "linkedConfiguration")
                            {
                                LinkedConfigurationProperty property = new LinkedConfigurationProperty(content, comment);
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
        /// 添加一个配置文件链接属性。
        /// </summary>
        /// <param name="property">配置文件链接属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public LinkedConfigurationProperty Add(LinkedConfigurationProperty property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }
            _properties.Add(property);
            if (property.Comment != null)
            {
                _container.Add(property.Comment);
            }
            _container.Add(property.Content);
            return property;
        }

        /// <summary>
        /// 添加一个配置文件链接属性。
        /// </summary>
        /// <param name="href">要包含的配置文件的 URL。 href 属性支持的唯一格式是 file://。 支持本地文件和 UNC 文件。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public LinkedConfigurationProperty Add(string href)
        {
            return Add(new LinkedConfigurationProperty(href));
        }

        #endregion Add

        #region TryGetValue

        /// <summary>
        /// 获取指定索引处的配置属性的值。
        /// <br/>如果没有找到指定索引，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="index">配置属性的索引。</param>
        /// <param name="property">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(int index, out LinkedConfigurationProperty property)
        {
            if (index < _properties.Count)
            {
                property = _properties[index];
                return true;
            }
            property = null;
            return false;
        }

        /// <summary>
        /// 获取指定索引处的配置属性的值。
        /// <br/>如果没有找到指定索引，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="index">配置属性的索引。</param>
        /// <param name="href">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(int index, out string href)
        {
            if (TryGetValue(index, out LinkedConfigurationProperty value))
            {
                href = value.Href;
                return true;
            }
            href = default;
            return false;
        }

        #endregion TryGetValue

        #region GetValue

        /// <summary>
        /// 获取与指定索引处的配置属性的值。
        /// </summary>
        /// <param name="index">配置属性的索引。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public LinkedConfigurationProperty GetValue(int index)
        {
            return TryGetValue(index, out LinkedConfigurationProperty value) ? value : null;
        }

        /// <summary>
        /// 获取与指定索引处的配置属性的值。
        /// <br/>如果没有找到指定索引，返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="index">配置属性的索引。</param>
        /// <param name="defaultValue">没有找到指定索引时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public string GetValue(int index, string defaultValue)
        {
            return TryGetValue(index, out LinkedConfigurationProperty value) ? value.Href : defaultValue;
        }

        #endregion GetValue

        /// <summary>
        /// 从配置属性集合中移除所有配置属性。
        /// </summary>
        public void Clear()
        {
            _container.RemoveNodes();
            _properties.Clear();
        }

        /// <summary>
        /// 返回循环访问集合的枚举数。
        /// </summary>
        /// <returns></returns>
        public IEnumerator<LinkedConfigurationProperty> GetEnumerator()
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
        public bool Remove(LinkedConfigurationProperty property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }
            bool removed = _properties.Remove(property);
            property.Comment?.Remove();
            property.Content.Remove();
            return removed;
        }

        /// <summary>
        /// 从配置属性集合中移除指定索引处的配置属性。和配置属性相关的注释一并移除。
        /// <br/>如果该元素成功移除，返回 <see langword="true"/>。如果没有找到指定元素，则返回 <see langword="false"/>。
        /// </summary>
        /// <param name="index">配置属性的索引。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool Remove(int index)
        {
            if (index < _properties.Count)
            {
                LinkedConfigurationProperty property = _properties[index];
                property.Comment?.Remove();
                property.Content.Remove();
                _properties.RemoveAt(index);
                return true;
            }
            return false;
        }
    }
}