using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 映射到标准格式的 "connectionStrings" 节点。
    /// </summary>
    public sealed class ConnectionStrings
    {
        private readonly XElement _content;
        private readonly ConnectionStringsPropertySet _properties;

        /// <summary>
        /// 包含的连接属性集合。
        /// </summary>
        public ConnectionStringsPropertySet Properties => _properties;

        #region Construction

        internal ConnectionStrings(XElement root)
        {
            _content = root.Element("connectionStrings");
            if (_content == null)
            {
                _content = new XElement("connectionStrings");
                root.Add(_content);
            }
            _properties = new ConnectionStringsPropertySet(_content);
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