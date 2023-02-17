using System;
using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 配置容器。
    /// </summary>
    public sealed class TextSection : ConfigurationSection
    {
        #region Construction

        internal TextSection(XElement content) : base(content)
        {
        }

        #endregion Construction

        /// <summary>
        /// 设置配置容器属性。
        /// </summary>
        /// <param name="name">配置容器的属性名称。</param>
        /// <param name="value">配置容器的属性值。</param>
        /// <returns></returns>
        ///
        public void SetAttribute(string name, string value)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"The invalid attribute name - {nameof(name)}.");
            }
            _content.SetAttributeValue(name, value);
        }

        /// <summary>
        /// 设置配置容器的串联内容。
        /// </summary>
        /// <param name="value">配置容器的串联内容。</param>
        /// <returns></returns>
        ///
        public void SetValue(string value)
        {
            _content.SetValue(value ?? string.Empty);
        }
    }
}