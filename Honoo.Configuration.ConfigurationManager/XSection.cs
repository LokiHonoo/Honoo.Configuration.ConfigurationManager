using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 配置容器。
    /// </summary>
    public class XSection : XDictionary
    {
        #region Members

        private readonly string _name;

        /// <summary>
        /// 获取此配置容器的名称。
        /// </summary>
        public string Name => _name;

        #endregion Members

        #region Construction

        internal XSection(XElement content, XComment comment, bool isProtected) : base(content, comment, isProtected)
        {
            _name = content.Attribute("name").Value;
        }

        #endregion Construction
    }
}