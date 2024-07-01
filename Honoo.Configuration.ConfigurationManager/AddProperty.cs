using System;
using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 配置属性。
    /// </summary>
    public sealed class AddProperty : ConfigurationProperty
    {
        private readonly string _key;
        private readonly string _value;

        /// <summary>
        /// 获取配置属性的键。
        /// </summary>
        public string Key => _key;

        /// <summary>
        /// 获取配置属性的值。
        /// </summary>
        public string Value => _value;

        #region Construction

        /// <summary>
        /// 创建 AddProperty 的新实例。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        public AddProperty(string key, string value) : base(ConfigurationPropertyKind.AddProperty, GetElement(key, value), null)
        {
            _key = key ?? throw new ArgumentNullException(nameof(key));
            _value = value;
        }

        internal AddProperty(XElement content, XComment comment) : base(ConfigurationPropertyKind.AddProperty, content, comment)
        {
            _key = content.Attribute("key").Value;
            _value = content.Attribute("value").Value;
        }

        #endregion Construction

        private static XElement GetElement(string key, string value)
        {
            XElement element = new XElement("add");
            element.SetAttributeValue("key", key);
            element.SetAttributeValue("value", value);
            return element;
        }
    }
}