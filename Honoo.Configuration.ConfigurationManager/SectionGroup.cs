using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 配置组。
    /// </summary>
    public sealed class SectionGroup
    {
        private readonly XElement _content;
        private readonly SectionGroupSet _groups;
        private readonly SectionSet _sections;

        /// <summary>
        /// 获取配置组集合。
        /// </summary>
        public SectionGroupSet Groups => _groups;

        /// <summary>
        /// 获取配置容器集合。
        /// </summary>
        public SectionSet Sections => _sections;

        #region Construction

        internal SectionGroup(XElement declaration, XElement content)
        {
            _content = content;
            _groups = new SectionGroupSet(declaration, content);
            _sections = new SectionSet(declaration, content);
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