using System;
using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 配置文件链接属性。
    /// </summary>
    public sealed class LinkedConfigurationProperty
    {
        private readonly XConfigComment _comment;
        private readonly XElement _content;
        private readonly string _href;

        /// <summary>
        /// 配置文件链接属性的注释。
        /// </summary>
        public XConfigComment Comment => _comment;

        /// <summary>
        /// 获取配置文件的 URL。 href 属性支持的唯一格式是 file://。 支持本地文件和 UNC 文件。
        /// </summary>
        public string Href => _href;

        internal XElement Content => _content;

        #region Construction

        /// <summary>
        /// 创建 LinkedConfigurationProperty 的新实例。
        /// </summary>
        /// <param name="href">要包含的配置文件的 URL。 href 属性支持的唯一格式是 file://。 支持本地文件和 UNC 文件。</param>
        public LinkedConfigurationProperty(string href)
        {
            _href = href ?? throw new ArgumentNullException(nameof(href));
            _content = GetElement(href);
            _comment = new XConfigComment(null, _content);
        }

        internal LinkedConfigurationProperty(XElement content, XComment comment)
        {
            _href = content.Attribute("href").Value;
            _content = content;
            _comment = new XConfigComment(comment, content);
        }

        #endregion Construction

        /// <summary>
        /// 方法已重写。返回节点的缩进 XML 文本。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _content.ToString();
        }

        private static XElement GetElement(string href)
        {
            XElement content = new XElement(ConfigurationManager.AssemblyBindingNamespace + "linkedConfiguration");
            content.SetAttributeValue("href", href);
            return content;
        }
    }
}