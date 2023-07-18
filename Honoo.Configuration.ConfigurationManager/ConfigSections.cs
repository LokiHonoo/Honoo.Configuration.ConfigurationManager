using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 映射到标准格式的 "configSections" 节点。
    /// </summary>
    public sealed class ConfigSections
    {
        private readonly XElement _content;
        private readonly ConfigSectionGroupSet _groups;
        private readonly ConfigSectionSet _sections;

        /// <summary>
        /// 包含的配置组集合。
        /// </summary>
        public ConfigSectionGroupSet Groups => _groups;

        /// <summary>
        /// 包含的配置容器集合。
        /// </summary>
        public ConfigSectionSet Sections => _sections;

        #region Construction

        internal ConfigSections(XElement root)
        {
            _content = root.Element("configSections");
            if (_content == null)
            {
                _content = new XElement("configSections");
                XElement assemblyBinding = root.Element("assemblyBinding");
                if (assemblyBinding == null)
                {
                    root.AddFirst(_content);
                }
                else
                {
                    assemblyBinding.AddAfterSelf(_content);
                }
            }
            _groups = new ConfigSectionGroupSet(_content, root);
            _sections = new ConfigSectionSet(_content, root);
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