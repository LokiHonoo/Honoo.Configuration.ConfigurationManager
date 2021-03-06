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

        #region Constructor

        internal AppSettings(XElement root, ISavable savable)
        {
            _content = root.Element("appSettings");
            if (_content is null)
            {
                _content = new XElement("appSettings");
                root.Add(_content);
            }
            _properties = new AppSettingsPropertySet(_content, savable);
        }

        #endregion Constructor

        /// <summary>
        /// 确定指定的对象是否等于当前对象。
        /// </summary>
        /// <param name="obj">要与当前对象进行比较的对象。</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return obj is AppSettings other && _content.Equals(other._content);
        }

        /// <summary>
        /// 作为默认哈希函数。
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return _content.GetHashCode();
        }

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