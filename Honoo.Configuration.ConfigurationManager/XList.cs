using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 列表类型的配置属性。
    /// </summary>
    public class XList : XProperty
    {
        #region Properties

        private readonly XListPropertySet _properties;

        /// <summary>
        /// 获取配置属性集合。
        /// </summary>
        public XListPropertySet Properties => _properties;

        #endregion Properties

        #region Construction

        /// <summary>
        /// 初始化 XList 类的新实例。
        /// </summary>
        public XList() : base(XPropertyKind.XList, new XElement(XSettingsManager.Namespace + "list"), null)
        {
            _properties = new XListPropertySet(base.Content);
        }

        internal XList(XElement content, XComment comment) : base(XPropertyKind.XList, content, comment)
        {
            _properties = new XListPropertySet(content);
        }

        #endregion Construction
    }
}