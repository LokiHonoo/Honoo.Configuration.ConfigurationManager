using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 配置容器。
    /// </summary>
    public sealed class NameValueSection : ConfigurationSection
    {
        private readonly NameValueSectionPropertySet _properties;

        /// <summary>
        /// 获取配置属性集合。
        /// </summary>
        public NameValueSectionPropertySet Properties => _properties;

        #region Construction

        internal NameValueSection(XElement content) : base(content)
        {
            _properties = new NameValueSectionPropertySet(content);
        }

        #endregion Construction
    }
}