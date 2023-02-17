﻿using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 配置容器。
    /// </summary>
    public sealed class NameValueSection : IConfigSection
    {
        private readonly XElement _content;
        private readonly NameValueSectionPropertySet _properties;

        /// <summary>
        /// 获取配置属性集合。
        /// </summary>
        public NameValueSectionPropertySet Properties => _properties;

        #region Construction

        internal NameValueSection(XElement content)
        {
            _properties = new NameValueSectionPropertySet(content);
            _content = content;
        }

        #endregion Construction

        /// <summary>
        /// 确定指定的对象是否等于当前对象。
        /// </summary>
        /// <param name="obj">要与当前对象进行比较的对象。</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return obj is NameValueSection other && _content.Equals(other._content);
        }

        /// <summary>
        /// 作为默认哈希函数。
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return _content.GetHashCode();
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