using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 配置组。
    /// </summary>
    public sealed class ConfigSectionGroup
    {
        private readonly ConfigComment _comment;
        private readonly XElement _content;
        private readonly XElement _declaration;
        private readonly ConfigSectionGroupSet _groups;
        private readonly ConfigSectionSet _sections;

        /// <summary>
        /// 配置组的注释。
        /// </summary>
        public ConfigComment Comment => _comment;

        /// <summary>
        /// 获取配置组集合。
        /// </summary>
        public ConfigSectionGroupSet Groups => _groups;

        /// <summary>
        /// 获取配置容器集合。
        /// </summary>
        public ConfigSectionSet Sections => _sections;

        internal XElement Content => _content;
        internal XElement Declaration => _declaration;

        #region Construction

        internal ConfigSectionGroup(XElement declaration, XElement content, XComment comment)
        {
            _declaration = declaration;
            _content = content;
            _comment = new ConfigComment(comment, content);
            _groups = new ConfigSectionGroupSet(declaration, content);
            _sections = new ConfigSectionSet(declaration, content);
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