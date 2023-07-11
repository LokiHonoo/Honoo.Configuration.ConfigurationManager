using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 配置容器。
    /// </summary>
    public sealed class NameValueSection : IConfigSection
    {
        private readonly XElement _content;
        private readonly ConfigSectionKind _kind;
        private readonly NameValuePropertySet _properties;
        private XComment _comment = null;

        /// <inheritdoc/>
        public ConfigSectionKind Kind => _kind;

        /// <summary>
        /// 获取配置属性集合。
        /// </summary>
        public NameValuePropertySet Properties => _properties;

        #region Construction

        internal NameValueSection(XElement content, XComment comment)
        {
            _kind = ConfigSectionKind.NameValueSection;
            _content = content;
            _comment = comment;
            _properties = new NameValuePropertySet(content);
        }

        #endregion Construction

        /// <inheritdoc/>
        public void RemoveComment()
        {
            if (_comment != null)
            {
                _comment.Remove();
                _comment = null;
            }
        }

        /// <inheritdoc/>
        public void SetComment(string comment)
        {
            if (comment == null)
            {
                RemoveComment();
            }
            else
            {
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
        }

        /// <summary>
        /// 方法已重写。返回节点的缩进 XML 文本。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _content.ToString();
        }

        /// <inheritdoc/>
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
    }
}