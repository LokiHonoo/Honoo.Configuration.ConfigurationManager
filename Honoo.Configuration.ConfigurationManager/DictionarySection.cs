using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 配置容器。
    /// </summary>
    public sealed class DictionarySection : ConfigSection
    {
        private readonly DictionaryPropertySet _properties;

        /// <summary>
        /// 获取配置属性集合。
        /// </summary>
        public DictionaryPropertySet Properties => _properties;

        #region Construction

        internal DictionarySection(XElement declaration, XElement content, XComment comment)
            : base(ConfigSectionKind.DictionarySection, declaration, content, comment)
        {
            _properties = new DictionaryPropertySet(content);
        }

        #endregion Construction

        /// <summary>
        /// 获取应用 file 属性以及 &lt;remove /&gt;、&lt;clear /&gt; 标签后的只读配置属性集合。
        /// </summary>
        /// <returns></returns>
        public DictionaryPropertySetControlled GetPropertySetControlled()
        {
            return new DictionaryPropertySetControlled(_content);
        }
    }
}