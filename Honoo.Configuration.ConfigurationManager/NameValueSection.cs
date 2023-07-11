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
        private readonly NameValueSectionPropertySet _properties;

        /// <inheritdoc/>
        public ConfigSectionKind Kind => _kind;

        /// <summary>
        /// 获取配置属性集合。
        /// </summary>
        public NameValueSectionPropertySet Properties => _properties;

        #region Construction

        internal NameValueSection(XElement content)
        {
            _kind = ConfigSectionKind.NameValueSection;
            _content = content;
            _properties = new NameValueSectionPropertySet(content);
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
    }
}