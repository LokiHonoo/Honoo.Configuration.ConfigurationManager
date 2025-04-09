using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 配置容器。
    /// </summary>
    public sealed class TextSection : ConfigSection
    {
        #region Members

        private XElement _content;
        private string _name;
        private ConfigSectionSet _set;

        #endregion Members

        #region Construction

        internal TextSection(XElement declaration, XElement content, XComment comment, ConfigSectionSet set)
            : base(ConfigSectionType.TextSection, declaration, content, comment)
        {
            _name = declaration.Attribute("name").Value;
            _set = set;
            _content = content;
        }

        #endregion Construction

        /// <summary>
        /// 获取与指定名称关联的配置容器的属性的值。
        /// </summary>
        /// <param name="name">配置容器的属性的名称。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public string GetAttribute(string name)
        {
            if (base.Content.Attribute(name) is XAttribute attribute)
            {
                return attribute.Value;
            }
            return null;
        }

        /// <summary>
        /// 获取配置容器的内联缩进 XML 文本。
        /// </summary>
        /// <returns></returns>
        public string GetValue()
        {
            StringBuilder result = new StringBuilder();
            foreach (XNode node in base.Content.Nodes())
            {
                result.Append(node.ToString());
                if (node.NextNode != null)
                {
                    result.AppendLine();
                }
            }
            return result.ToString();
        }

        /// <summary>
        /// 设置配置容器属性的值、添加或删除配置容器属性。
        /// </summary>
        /// <param name="name">配置容器的属性的名称。</param>
        /// <param name="value">配置容器的属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public void SetAttribute(string name, string value)
        {
            base.Content.SetAttributeValue(name, value);
        }

        /// <summary>
        /// 设置配置容器的串联内容。如果 <paramref name="value"/> 为 <see langword="null"/>，则删除配置容器。
        /// </summary>
        /// <param name="value">配置容器的串联内容。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public TextSection SetValue(string value)
        {
            return SetValue(value, false);
        }

        /// <summary>
        /// 设置配置容器的串联内容。如果 <paramref name="emptyRemove"/> 是 <see langword="true"/>，则 <paramref name="value"/> 为 <see langword="null"/> 或 <see cref="string.IsNullOrWhiteSpace(string)"/> 时删除注释配置容器。
        /// <br />如果 <paramref name="emptyRemove"/> 是 <see langword="false"/>，则仅在 <paramref name="value"/> 为 <see langword="null"/> 时删除配置容器。
        /// </summary>
        /// <param name="value">配置容器的串联内容。</param>
        /// <param name="emptyRemove">判断设置的内容是否是有效内容。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public TextSection SetValue(string value, bool emptyRemove)
        {
            if (emptyRemove ? value == null || string.IsNullOrWhiteSpace(value) : value == null)
            {
                _set.Remove(_name);
                _name = null;
                _set = null;
                _content = null;
                return null;
            }
            base.Content.RemoveNodes();
            string tmp = $"<encirclement>{value}</encirclement>";
            using (StringReader sReader = new StringReader(tmp))
            {
                using (XmlReader reader = XmlReader.Create(sReader, ConfigurationManager.ReaderSettings))
                {
                    XElement element = XElement.Load(reader);
                    foreach (XNode node in element.Nodes())
                    {
                        _content.Add(node);
                    }
                }
            }
            return this;
        }

        /// <summary>
        /// 获取与指定名称关联的配置容器的属性的值。如果没有找到指定属性，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="name">配置容器的属性的名称。</param>
        /// <param name="value">配置容器的属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetAttribute(string name, out string value)
        {
            if (base.Content.Attribute(name) is XAttribute attribute)
            {
                value = attribute.Value;
                return true;
            }
            value = null;
            return false;
        }
    }
}