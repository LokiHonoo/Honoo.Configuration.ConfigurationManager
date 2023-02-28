using System;
using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 配置容器。
    /// </summary>
    public sealed class TextSection : ConfigSection
    {
        #region Construction

        internal TextSection(XElement content) : base(content, ConfigSectionKind.TextSection)
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
            if (string.IsNullOrWhiteSpace(value))
            {
                _content.SetValue(string.Empty);
            }
            else
            {
                value = value.Trim(' ', '\r', '\n');
                if (value.StartsWith("<![CDATA[", StringComparison.InvariantCultureIgnoreCase)
                    && value.EndsWith("]]>", StringComparison.InvariantCultureIgnoreCase))
                {
                    XCData cData = new XCData(value.Substring(9, value.Length - 9 - 3));
                    _content.RemoveNodes();
                    _content.Add(cData);
                }
                else
                {
                    string tmp = $"<encirclement>{value}</encirclement>";
                    XElement element = XElement.Parse(tmp);
                    if (element.HasElements)
                    {
                        _content.RemoveNodes();
                        foreach (XElement sub in element.Elements())
                        {
                            _content.Add(sub);
                        }
                    }
                    else
                    {
                        _content.SetValue(value);
                    }
                }
            }
        }
    }
}