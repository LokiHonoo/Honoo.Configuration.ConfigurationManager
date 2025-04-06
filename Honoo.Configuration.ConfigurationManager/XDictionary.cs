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
        public XDictionary() : base(XPropertyType.XDictionary, new XElement(XConfigManager.Namespace + "dictionary"), null, false)
        {
            _properties = new XDictionaryPropertySet(base.Content);
        }

        internal XDictionary(XElement content, XComment comment, bool isProtected) : base(XPropertyType.XDictionary, content, comment, isProtected)
        {
            if (!isProtected)
            {
                _properties = new XDictionaryPropertySet(content);
            }
        }

        #endregion Construction

        /// <summary>
        /// 解密此配置属性。
        /// </summary>
        /// <param name="protectionAlgorithm">指定一个非对称加密算法，算法必须拥有私钥。</param>
        /// <exception cref="Exception"></exception>
        protected override XElement DecryptInternal(RSA protectionAlgorithm)
        {
            XElement content = base.DecryptInternal(protectionAlgorithm);
            _properties = new XDictionaryPropertySet(content);
            return content;
        }

        /// <summary>
        /// 加密此配置属性。
        /// </summary>
        /// <param name="protectionAlgorithm">指定一个非对称加密算法，算法可以是公钥或私钥。</param>
        /// <exception cref="Exception"></exception>
        protected override XElement EncryptInternal(RSA protectionAlgorithm)
        {
            XElement content = base.EncryptInternal(protectionAlgorithm);
            _properties = null;
            return content;
        }
    }
}