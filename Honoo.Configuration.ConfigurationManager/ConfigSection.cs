using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 配置容器的基类。
    /// </summary>
    public abstract class ConfigSection
    {
        private readonly XConfigComment _comment;
        private readonly XElement _content;
        private readonly XElement _declaration;
        private readonly ConfigSectionType _sectionType;

        /// <summary>
        /// 配置容器的注释。
        /// </summary>
        public XConfigComment Comment => _comment;

        /// <summary>
        /// 获取此配置容器的类型。
        /// </summary>
        public ConfigSectionType SectionType => _sectionType;

        internal XElement Content => _content;
        internal XElement Declaration => _declaration;

        #region Construction

        /// <summary>
        /// 创建 ConfigSection 的新实例。
        /// </summary>
        /// <param name="sectionType">配置容器的类型。</param>
        /// <param name="declaration">配置容器的描述节点。</param>
        /// <param name="content">配置容器的内容节点。</param>
        /// <param name="comment">配置容器的注释节点。</param>
        protected ConfigSection(ConfigSectionType sectionType, XElement declaration, XElement content, XComment comment)
        {
            _sectionType = sectionType;
            _declaration = declaration;
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