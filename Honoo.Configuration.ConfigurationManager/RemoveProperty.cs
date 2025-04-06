using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 配置属性。
    /// </summary>
    public sealed class RemoveProperty : ConfigProperty
    {
        #region Construction

        /// <summary>
        /// 创建 RemoveProperty 的新实例。
        /// </summary>
        public RemoveProperty() : base(ConfigPropertyType.RemoveProperty, new XElement("remove"), null)
        {
        }

        internal RemoveProperty(XElement content, XComment comment) : base(ConfigPropertyType.RemoveProperty, content, comment)
        {
        }

        #endregion Construction
    }
}