using System.Security.Cryptography;
using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 映射到标准格式的 "connectionStrings" 节点。
    /// </summary>
    public sealed class ConnectionStrings
    {
        private readonly XElement _container;
        private readonly ConnectionStringsPropertySet _properties;

        /// <summary>
        /// 获取连接属性集合。
        /// </summary>
        public ConnectionStringsPropertySet Properties => _properties;

        #region Construction

        internal ConnectionStrings(XElement root)
        {
            _container = root.Element("connectionStrings");
            if (_container == null)
            {
                _container = new XElement("connectionStrings");
                root.Add(_container);
            }
            else if (_container.Attribute("configProtectionProvider") != null)
            {
                throw new CryptographicException("Encryped configuration sections are not supported.");
            }
            _properties = new ConnectionStringsPropertySet(_container);
        }

        #endregion Construction

        /// <summary>
        /// 方法已重写。返回节点的缩进 XML 文本。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _container.ToString();
        }
    }
}