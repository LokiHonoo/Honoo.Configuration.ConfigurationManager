using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 配置属性。
    /// </summary>
    public sealed class ClearProperty : ConfigurationProperty
    {
        #region Construction

        /// <summary>
        /// 创建 ClearProperty 的新实例。
        /// </summary>
        public ClearProperty() : base(ConfigurationPropertyKind.ClearProperty, new XElement("clear"), null)
        {
        }

        internal ClearProperty(XElement content, XComment comment) : base(ConfigurationPropertyKind.ClearProperty, content, comment)
        {
        }

        #endregion Construction
    }
}