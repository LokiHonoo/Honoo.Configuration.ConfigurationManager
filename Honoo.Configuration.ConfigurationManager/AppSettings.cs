using System.Security.Cryptography;
using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 映射到标准格式的 &lt;appSettings /&gt; 节点。
    /// </summary>
    public sealed class AppSettings
    {
        private readonly XElement _content;
        private readonly DictionaryPropertySet _properties;

        /// <summary>
        /// 获取配置属性集合。
        /// </summary>
        public DictionaryPropertySet Properties => _properties;

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
            _properties = new DictionaryPropertySet(_content);
        }

        #endregion Construction

        #region File

        /// <summary>
        /// 获取 "file" 属性的值。
        /// </summary>
        /// <returns></returns>
        public string GetFileAttribute()
        {
            return TryGetFileAttribute(out string file) ? file : null;
        }

        /// <summary>
        /// 设置 "file" 属性的值、添加或删除 "file" 属性。
        /// </summary>
        /// <param name="value">"file" 属性的值。"file" 特性指向一个根节点为 &lt;appSettings&gt; 的配置文件。</param>
        /// <returns></returns>
        public void SetFileAttribute(string value)
        {
            _content.SetAttributeValue("file", value);
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

        #endregion File

        /// <summary>
        /// 获取应用 file 属性以及 &lt;remove /&gt;、&lt;clear /&gt; 标签后的只读配置属性集合。
        /// </summary>
        /// <returns></returns>
        public DictionaryPropertySetControlled GetPropertySetControlled()
        {
            return new DictionaryPropertySetControlled(_content);
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