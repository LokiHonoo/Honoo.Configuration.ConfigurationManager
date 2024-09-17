using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 配置容器。
    /// </summary>
    public sealed class SingleTagSection : ConfigSection
    {
        private readonly SingleTagPropertySet _properties;

        /// <summary>
        /// 获取配置属性集合。
        /// </summary>
        public SingleTagPropertySet Properties => _properties;

        #region Construction

        internal SingleTagSection(XElement declaration, XElement content, XComment comment) : base(declaration, content, comment)
        {
            _properties = new SingleTagPropertySet(content);
        }

        #endregion Construction
    }
}