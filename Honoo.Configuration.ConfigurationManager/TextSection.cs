using System;
using System.Text;
using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 配置容器。
    /// </summary>
    public sealed class TextSection : ConfigSection
    {
        #region Construction

        internal TextSection(XElement declaration, XElement content, XComment comment) : base(ConfigSectionKind.TextSection, declaration, content, comment)
        {
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
        /// 设置配置容器的串联内容。
        /// </summary>
        /// <param name="value">配置容器的串联内容。</param>
        /// <returns></returns>
        ///
        public void SetValue(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                base.Content.SetValue(string.Empty);
            }
            else
            {
                base.Content.RemoveNodes();
                string tmp = $"<encirclement>{value}</encirclement>";
                XElement element = XElement.Parse(tmp);
                foreach (XNode node in element.Nodes())
                {
                    base.Content.Add(node);
                }
            }
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