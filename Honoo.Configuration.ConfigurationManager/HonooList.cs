using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 列表类型的配置属性。
    /// </summary>
    public class HonooList : HonooProperty
    {
        #region Properties

        private readonly HonooListPropertySet _properties;

        /// <summary>
        /// 获取配置属性集合。
        /// </summary>
        public HonooListPropertySet Properties => _properties;

        #endregion Properties

        #region Construction

        /// <summary>
        /// 初始化 HonooList 类的新实例。
        /// </summary>
        public HonooList() : base(HonooPropertyKind.HonooList, new XElement(HonooSettingsManager.Namespace + "list"), null)
        {
            _properties = new HonooListPropertySet(base.Content);
        }

        internal HonooList(XElement content, XComment comment) : base(HonooPropertyKind.HonooList, content, comment)
        {
            _properties = new HonooListPropertySet(content);
        }

        #endregion Construction
    }
}