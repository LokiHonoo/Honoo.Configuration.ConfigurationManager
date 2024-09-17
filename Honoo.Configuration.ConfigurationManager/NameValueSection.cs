using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 配置容器。
    /// </summary>
    public sealed class NameValueSection : ConfigSection
    {
        private readonly NameValuePropertySet _properties;

        /// <summary>
        /// 获取配置属性集合。
        /// </summary>
        public NameValuePropertySet Properties => _properties;

        #region Construction

        internal NameValueSection(XElement declaration, XElement content, XComment comment) : base(declaration, content, comment)
        {
            _properties = new NameValuePropertySet(content);
        }

        #endregion Construction

        /// <summary>
        /// 获取应用 &lt;remove /&gt;、&lt;clear /&gt; 标签后的只读配置属性集合。当配置文件修改时应重新获取。
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1024:在适用处使用属性", Justification = "<挂起>")]
        public NameValuePropertySetControlled GetControlledProperties()
        {
            return new NameValuePropertySetControlled(base.Content);
        }
    }
}