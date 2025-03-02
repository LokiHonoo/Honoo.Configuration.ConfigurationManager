using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 配置属性的基类。<see langword="&lt;dictionary /&gt;"/>、<see langword="&lt;list /&gt;"/>、<see langword="&lt;string /&gt;"/> 从此类中继承。
    /// </summary>
    public abstract class XProperty
    {
        private readonly XConfigAttributeSet _attributes;
        private readonly XConfigComment _comment;
        private readonly XElement _content;
        private readonly XPropertyKind _kind;

        /// <summary>
        /// 获取配置属性集合。
        /// </summary>
        public XConfigAttributeSet Attributes => _attributes;

        /// <summary>
        /// 获取配置属性的注释。
        /// </summary>
        public XConfigComment Comment => _comment;

        /// <summary>
        /// 获取配置属性的类型。
        /// </summary>
        public XPropertyKind Kind => _kind;

        internal XElement Content => _content;

        #region Construction

        /// <summary>
        /// 创建 XProperty 的新实例。
        /// </summary>
        /// <param name="kind">配置属性的类型。</param>
        /// <param name="content">配置属性的节点元素。</param>
        /// <param name="comment">注释元素。</param>
        protected XProperty(XPropertyKind kind, XElement content, XComment comment)
        {
            _kind = kind;
            _content = content;
            _attributes = new XConfigAttributeSet(content);
            _comment = new XConfigComment(comment, content);
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