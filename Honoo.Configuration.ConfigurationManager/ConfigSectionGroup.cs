using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 配置组。
    /// </summary>
    public sealed class ConfigSectionGroup
    {
        private readonly XElement _content;
        private readonly ConfigSectionGroupSet _groups;
        private readonly ConfigSectionSet _sections;

        /// <summary>
        /// 获取配置组集合。
        /// </summary>
        public ConfigSectionGroupSet Groups => _groups;

        /// <summary>
        /// 获取配置容器集合。
        /// </summary>
        public ConfigSectionSet Sections => _sections;

        #region Construction

        internal ConfigSectionGroup(XElement declaration, XElement content)
        {
            _content = content;
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