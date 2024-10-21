using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 配置属性的基类。<see langword="&lt;dictionary /&gt;"/>、<see langword="&lt;list /&gt;"/>、<see langword="&lt;string /&gt;"/> 从此类中继承。
    /// </summary>
    public abstract class HonooProperty
    {
        private readonly HonooComment _comment;
        private readonly XElement _content;
        private readonly HonooPropertyKind _kind;

        /// <summary>
        /// 配置属性的注释。
        /// </summary>
        public HonooComment Comment => _comment;

        /// <summary>
        /// 获取配置属性的类型。
        /// </summary>
        public HonooPropertyKind Kind => _kind;

        internal XElement Content => _content;

        #region Construction

        /// <summary>
        /// 创建 HonooProperty 的新实例。
        /// </summary>
        /// <param name="kind">配置属性的类型。</param>
        /// <param name="content">配置属性的节点元素。</param>
        /// <param name="comment">注释元素。</param>
        protected HonooProperty(HonooPropertyKind kind, XElement content, XComment comment)
        {
            _kind = kind;
            _content = content;
            _comment = new HonooComment(comment, content);
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