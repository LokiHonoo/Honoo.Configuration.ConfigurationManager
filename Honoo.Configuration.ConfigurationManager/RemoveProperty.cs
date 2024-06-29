using System;
using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 配置属性。
    /// </summary>
    public sealed class RemoveProperty : ConfigurationProperty
    {
        private readonly string _key;

        /// <summary>
        /// 获取配置属性的键。
        /// </summary>
        public string Key => _key;

        #region Construction

        /// <summary>
        /// 创建 RemoveProperty 的新实例。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        public RemoveProperty(string key) : base(ConfigurationPropertyKind.RemoveProperty, GetElement(key), null)
        {
            _key = key ?? throw new ArgumentNullException(nameof(key));
        }

        internal RemoveProperty(XElement content, XComment comment) : base(ConfigurationPropertyKind.RemoveProperty, content, comment)
        {
            _key = content.Attribute("key").Value;
        }

        #endregion Construction

        private static XElement GetElement(string key)
        {
            XElement element = new XElement("remove");
            element.SetAttributeValue("key", key);
            return element;
        }
    }
}