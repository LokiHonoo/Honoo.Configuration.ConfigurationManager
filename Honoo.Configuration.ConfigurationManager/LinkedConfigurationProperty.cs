using System;
using System.Runtime.InteropServices;
using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 配置文件链接属性。
    /// </summary>
    public sealed class LinkedConfigurationProperty
    {
        private readonly XElement _content;
        private readonly string _href;
        private XComment _comment;
        private static readonly XNamespace _namespace = "urn:schemas-microsoft-com:asm.v1";

        /// <summary>
        /// 获取配置文件的 URL。 href 属性支持的唯一格式是 file://。 支持本地文件和 UNC 文件。
        /// </summary>
        public string Href => _href;

        internal XComment Comment => _comment;
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
            _comment = null;
        }

        internal LinkedConfigurationProperty(XElement content, XComment comment)
        {
            _href = content.Attribute("href").Value;
            _content = content;
            _comment = comment;
        }

        #endregion Construction

        #region Comment

        /// <summary>
        /// 获取注释。
        /// </summary>
        /// <returns></returns>
        public string GetComment()
        {
            return TryGetComment(out string comment) ? comment : null;
        }

        /// <summary>
        /// 删除注释。
        /// <br/>如果注释成功删除，返回 <see langword="true"/>。如果没有找到注释节点，则返回 <see langword="false"/>。
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
            if (string.IsNullOrEmpty(comment))
            {
                throw new ArgumentException($"The invalid argument - {nameof(comment)}.");
            }
            if (_comment == null)
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
        /// 获取注释。
        /// <br/>如果没有找到注释，返回 <see langword="false"/>。
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

        private static XElement GetElement(string href)
        {
            XElement content = new XElement(_namespace + "linkedConfiguration");
            content.SetAttributeValue("href", href);
            return content;
        }
    }
}