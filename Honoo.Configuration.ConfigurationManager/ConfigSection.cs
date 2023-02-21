using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 表示任何配置容器从中继承的基本类型。
    /// </summary>
    public abstract class ConfigSection
    {
        /// <summary>
        ///
        /// </summary>
        protected readonly XElement _content;

        private readonly ConfigSectionKind _kind;

        /// <summary>
        /// 获取此配置容器的类型。
        /// </summary>
        public ConfigSectionKind Kind => _kind;

        #region Construction

        /// <summary>
        /// 创建 ConfigurationSection 的新实例。
        /// </summary>
        /// <param name="content"></param>
        /// <param name="kind"></param>
        protected ConfigSection(XElement content, ConfigSectionKind kind)
        {
            _content = content;
            _kind = kind;
        }

        #endregion Construction

        /// <summary>
        /// 返回节点的缩进 XML 文本。
        /// </summary>
        /// <returns></returns>
        public string GetXmlString()
        {
            return _content.ToString();
        }

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