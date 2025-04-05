using System;
using System.Security.Cryptography;
using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 字典类型的配置属性。
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1711:标识符应采用正确的后缀", Justification = "<挂起>")]
    public class XDictionary : XProperty
    {
        #region Members

        private XDictionaryPropertySet _properties;

        /// <summary>
        /// 获取配置属性集合。
        /// </summary>
        public XDictionaryPropertySet Properties => _properties;

        #endregion Members

        #region Construction

        /// <summary>
        /// 初始化 XDictionary 类的新实例。
        /// </summary>
        public XDictionary() : base(XPropertyKind.XDictionary, new XElement(XConfigManager.Namespace + "dictionary"), null)
        {
            _properties = new XDictionaryPropertySet(base.Content);
        }

        internal XDictionary(XElement content, XComment comment) : base(XPropertyKind.XDictionary, content, comment)
        {
            _properties = new XDictionaryPropertySet(content);
        }

        internal XDictionary(XElement content, XComment comment, bool isProtected) : base(XPropertyKind.XDictionary, content, comment, isProtected)
        {
            if (!isProtected)
            {
                _properties = new XDictionaryPropertySet(content);
            }
        }

        #endregion Construction

        /// <summary>
        /// 加密或解密此配置属性。
        /// </summary>
        /// <param name="encrypt">加密或解密此配置属性。</param>
        /// <param name="protectionAlgorithm">指定一个非对称加密算法。</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        protected override XElement Reset(bool encrypt, RSA protectionAlgorithm)
        {
            XElement content = base.Reset(encrypt, protectionAlgorithm);
            _properties = encrypt ? null : new XDictionaryPropertySet(content);
            return content;
        }
    }
}