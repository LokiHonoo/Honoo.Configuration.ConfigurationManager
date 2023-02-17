﻿using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 配置容器。
    /// </summary>
    public sealed class DictionarySection : ConfigurationSection
    {
        private readonly DictionarySectionPropertySet _properties;

        /// <summary>
        /// 获取配置属性集合。
        /// </summary>
        public DictionarySectionPropertySet Properties => _properties;

        #region Construction

        internal DictionarySection(XElement content) : base(content)
        {
            _properties = new DictionarySectionPropertySet(content);
        }

        #endregion Construction
    }
}