using System.Security.Cryptography;
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
            else if (_content.Attribute("configProtectionProvider") != null)
            {
                throw new CryptographicException("Encryped configuration sections are not supported.");
            }
            _properties = new AppSettingsPropertySet(_content);
        }

        #endregion Construction

        /// <summary>
        /// 设置 "file" 属性的值、添加或删除 "file" 属性。
        /// </summary>
        /// <param name="value">"file" 属性的值。"file" 特性指定的配置文件必须有一个根节点为 &lt;appSettings&gt;，而不是 &lt;configuration&gt;。</param>
        /// <returns></returns>
        public void SetFileAttribute(string value)
        {
            _content.SetAttributeValue("file", value);
        }

        /// <summary>
        /// 方法已重写。返回节点的缩进 XML 文本。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _content.ToString();
        }

        /// <summary>
        /// 获取 "file" 属性的值。
        /// <br/>如果没有找到指定属性，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="value">"file" 属性的值。</param>
        /// <returns></returns>
        public bool TryGetFileAttribute(out string value)
        {
            if (_content.Attribute("file") is XAttribute attribute)
            {
                value = attribute.Value;
                return true;
            }
            value = null;
            return false;
        }
    }
}