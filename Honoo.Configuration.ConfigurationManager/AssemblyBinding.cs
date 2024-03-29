﻿using System.Security.Cryptography;
using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 映射到标准格式的 "assemblyBinding" 节点。
    /// </summary>
    public sealed class AssemblyBinding
    {
        private readonly XElement _content;
        private readonly AssemblyBindingPropertySet _properties;

        /// <summary>
        /// 获取配置属性集合。
        /// </summary>
        public AssemblyBindingPropertySet Properties => _properties;

        #region Construction

        internal AssemblyBinding(XElement root)
        {
            _content = root.Element("assemblyBinding");
            if (_content == null)
            {
                _content = new XElement("assemblyBinding");
                root.AddFirst(_content);
            }
            else if (_content.Attribute("configProtectionProvider") != null)
            {
                throw new CryptographicException("Encryped configuration sections are not supported.");
            }
            _properties = new AssemblyBindingPropertySet(_content);
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