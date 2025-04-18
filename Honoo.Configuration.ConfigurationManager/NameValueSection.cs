﻿using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 配置容器。
    /// </summary>
    public sealed class NameValueSection : ConfigSection
    {
        private readonly NameValuePropertySet _properties;

        /// <summary>
        /// 获取配置属性集合。
        /// </summary>
        public NameValuePropertySet Properties => _properties;

        #region Construction

        internal NameValueSection(XElement declaration, XElement content, XComment comment)
            : base(ConfigSectionType.NameValueSection, declaration, content, comment)
        {
            _properties = new NameValuePropertySet(content);
        }

        #endregion Construction
    }
}