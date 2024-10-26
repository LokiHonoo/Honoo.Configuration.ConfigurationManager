using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 字典类型的配置属性。
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1711:标识符应采用正确的后缀", Justification = "<挂起>")]
    public class XDictionary : XProperty
    {
        #region Properties

        private readonly XDictionaryPropertySet _properties;

        /// <summary>
        /// 获取配置属性集合。
        /// </summary>
        public XDictionaryPropertySet Properties => _properties;

        #endregion Properties

        #region Construction

        /// <summary>
        /// 初始化 XDictionary 类的新实例。
        /// </summary>
        public XDictionary() : base(XPropertyKind.XDictionary, new XElement(XSettingsManager.Namespace + "dictionary"), null)
        {
            _properties = new XDictionaryPropertySet(base.Content);
        }

        internal XDictionary(XElement content, XComment comment) : base(XPropertyKind.XDictionary, content, comment)
        {
            _properties = new XDictionaryPropertySet(content);
        }

        #endregion Construction
    }
}