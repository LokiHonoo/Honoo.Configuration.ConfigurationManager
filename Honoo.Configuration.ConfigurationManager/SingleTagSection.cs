using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 配置容器。
    /// </summary>
    public sealed class SingleTagSection : IConfigSection
    {
        private readonly XElement _content;
        private readonly ConfigSectionKind _kind;
        private readonly SingleTagSectionPropertySet _properties;

        /// <inheritdoc/>
        public ConfigSectionKind Kind => _kind;

        /// <summary>
        /// 获取配置属性集合。
        /// </summary>
        public SingleTagSectionPropertySet Properties => _properties;

        #region Construction

        internal SingleTagSection(XElement content)
        {
            _kind = ConfigSectionKind.SingleTagSection;
            _content = content;
            _properties = new SingleTagSectionPropertySet(content);
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