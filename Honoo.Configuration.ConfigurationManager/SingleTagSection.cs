using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 配置容器。
    /// </summary>
    public sealed class SingleTagSection : ConfigurationSection
    {
        private readonly SingleTagSectionPropertySet _properties;

        /// <summary>
        /// 获取配置属性集合。
        /// </summary>
        public SingleTagSectionPropertySet Properties => _properties;

        #region Construction

        internal SingleTagSection(XElement content) : base(content)
        {
            _properties = new SingleTagSectionPropertySet(content);
        }

        #endregion Construction
    }
}