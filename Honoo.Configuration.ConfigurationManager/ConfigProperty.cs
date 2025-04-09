using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 标签配置属性的基类。<see langword="&lt;add /&gt;"/>、<see langword="&lt;remove /&gt;"/>、<see langword="&lt;clear /&gt;"/> 从此类中继承。
    /// </summary>
    public abstract class ConfigProperty
    {
        private readonly XConfigComment _comment;
        private readonly XElement _content;
        private readonly ConfigPropertyType _propertyType;

        /// <summary>
        /// 配置属性的注释。
        /// </summary>
        public XConfigComment Comment => _comment;

        /// <summary>
        /// 获取配置属性的类型。
        /// </summary>
        public ConfigPropertyType PropertyType => _propertyType;

        internal XElement Content => _content;

        #region Construction

        /// <summary>
        /// 创建 ConfigProperty 的新实例。
        /// </summary>
        /// <param name="propertyType">配置属性的类型。</param>
        /// <param name="content">配置属性的内容节点。</param>
        /// <param name="comment">配置属性的注释节点。</param>
        protected ConfigProperty(ConfigPropertyType propertyType, XElement content, XComment comment)
        {
            _propertyType = propertyType;
            _content = content;
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