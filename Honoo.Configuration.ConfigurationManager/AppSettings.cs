using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 映射到标准格式的 "appSettings" 节点。
    /// </summary>
    public sealed class AppSettings
    {
        private readonly XElement _content;
        private readonly AppSettingsPropertySet _properties;

        /// <summary>
        /// 获取配置属性集合。
        /// </summary>
        public AppSettingsPropertySet Properties => _properties;

        #region Construction

        internal AppSettings(XElement root)
        {
            _content = root.Element("appSettings");
            if (_content == null)
            {
                _content = new XElement("appSettings");
                root.Add(_content);
            }
            _properties = new AppSettingsPropertySet(_content);
        }

        #endregion Construction

        /// <summary>
        /// 方法已重写。返回节点的缩进 XML 文本。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _content.ToString();
        }
    }
}