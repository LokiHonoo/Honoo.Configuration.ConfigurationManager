using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 配置容器。
    /// </summary>
    public sealed class DictionarySection : ConfigSection
    {
        private readonly DictionaryPropertySet _properties;

        /// <summary>
        /// 获取配置属性集合。
        /// </summary>
        public DictionaryPropertySet Properties => _properties;

        #region Construction

        internal DictionarySection(XElement content, XComment comment) : base(ConfigSectionKind.SingleTagSection, content, comment)
        {
            _properties = new DictionaryPropertySet(content);
        }

        #endregion Construction
    }
}