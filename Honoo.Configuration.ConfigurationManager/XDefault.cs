using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 映射到 &lt;default /&gt; 配置容器节点。
    /// </summary>
    public sealed class XDefault : XDictionary
    {
        #region Construction

        internal XDefault(XElement content, XComment comment, bool isProtected) : base(content, comment, isProtected)
        {
        }

        #endregion Construction
    }
}