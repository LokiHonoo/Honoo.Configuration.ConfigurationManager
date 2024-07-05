using System.Security.Cryptography;
using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 映射到标准格式的 &lt;assemblyBinding /&gt; 节点。这是配置级的程序集绑定策略节点。
    /// </summary>
    public sealed class AssemblyBinding
    {
        private readonly XElement _content;
        private readonly AssemblyBindingPropertySet _properties;

        /// <summary>
        /// 获取配置属性集合。
        /// </summary>
        public AssemblyBindingPropertySet Properties => _properties;

        #region Construction

        internal AssemblyBinding(XElement root)
        {
            XNamespace ns = "urn:schemas-microsoft-com:asm.v1";
            XName name = ns + "assemblyBinding";
            _content = root.Element(name);
            if (_content == null)
            {
                _content = new XElement(name);
                root.AddFirst(_content);
            }
            else if (_content.Attribute("configProtectionProvider") != null)
            {
                throw new CryptographicException("Encryped configuration sections are not supported.");
            }
            _properties = new AssemblyBindingPropertySet(_content);
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