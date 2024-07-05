using System;
using System.Xml;
using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 配置容器。
    /// </summary>
    public sealed class HonooSection
    {
        private readonly XElement _content;
        private readonly HonooPropertySet _properties;
        private XComment _comment;

        /// <summary>
        /// 获取配置属性集合。
        /// </summary>
        public HonooPropertySet Properties => _properties;

        internal XComment Comment => _comment;
        internal XElement Content => _content;

        #region Construction

        internal HonooSection(XElement root)
        {
            XName name = HonooSettingsManager.Namespace + "default";
            _content = root.Element(name);
            if (_content == null)
            {
                _content = new XElement(name);
                root.AddFirst(_content);
            }
            else
            {
                XNode pre = _content.PreviousNode;
                if (pre != null && pre.NodeType == XmlNodeType.Comment)
                {
                    _comment = (XComment)pre;
                }
            }
            _properties = new HonooPropertySet(_content);
        }

        internal HonooSection(XElement content, XComment comment)
        {
            _content = content;
            _comment = comment;
            _properties = new HonooPropertySet(_content);
        }

        #endregion Construction

        #region Comment

        /// <summary>
        /// 获取注释。如果没有找到注释，返回 <see langword="null"/>。
        /// </summary>
        /// <returns></returns>
        public string GetComment()
        {
            return TryGetComment(out string comment) ? comment : null;
        }

        /// <summary>
        /// 删除注释。如果注释成功删除，返回 <see langword="true"/>。如果没有找到注释节点，则返回 <see langword="false"/>。
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
            if (comment == null)
            {
                if (_comment != null)
                {
                    _comment.Remove();
                    _comment = null;
                }
            }
            else if (_comment == null)
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
        /// 获取注释。如果没有找到注释，返回 <see langword="false"/>。
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
    }
}