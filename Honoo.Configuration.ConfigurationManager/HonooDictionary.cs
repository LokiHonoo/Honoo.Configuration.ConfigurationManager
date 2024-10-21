using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 字典类型的配置属性。
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1711:标识符应采用正确的后缀", Justification = "<挂起>")]
    public class HonooDictionary : HonooProperty
    {
        #region Properties

        private readonly HonooDictionaryPropertySet _properties;

        /// <summary>
        /// 获取配置属性集合。
        /// </summary>
        public HonooDictionaryPropertySet Properties => _properties;

        #endregion Properties

        #region Construction

        /// <summary>
        /// 初始化 HonooDictionary 类的新实例。
        /// </summary>
        public HonooDictionary() : base(HonooPropertyKind.HonooDictionary, new XElement(HonooSettingsManager.Namespace + "dictionary"), null)
        {
            _properties = new HonooDictionaryPropertySet(base.Content);
        }

        internal HonooDictionary(XElement content, XComment comment) : base(HonooPropertyKind.HonooDictionary, content, comment)
        {
            _properties = new HonooDictionaryPropertySet(content);
        }

        #endregion Construction
    }
}