using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 映射到标准格式的 &lt;configSections /&gt; 节点。
    /// </summary>
    public sealed class ConfigSections
    {
        private readonly XElement _declarationContainer;
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
            _declarationContainer = root.Element("configSections");
            if (_declarationContainer == null)
            {
                _declarationContainer = new XElement("configSections");
                XElement assemblyBinding = root.Element("assemblyBinding");
                if (assemblyBinding == null)
                {
                    root.AddFirst(_declarationContainer);
                }
                else
                {
                    assemblyBinding.AddAfterSelf(_declarationContainer);
                }
            }
            _groups = new ConfigSectionGroupSet(_declarationContainer, root);
            _sections = new ConfigSectionSet(_declarationContainer, root);
        }

        #endregion Construction

        /// <summary>
        /// 方法已重写。返回描述节点的缩进 XML 文本。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _declarationContainer.ToString();
        }
    }
}