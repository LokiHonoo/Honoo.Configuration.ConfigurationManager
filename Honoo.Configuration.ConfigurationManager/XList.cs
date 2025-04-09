using System;
using System.Security.Cryptography;
using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 列表类型的配置属性。
    /// </summary>
    public class XList : XProperty
    {
        #region Members

        private XListPropertySet _properties;

        /// <summary>
        /// 获取配置属性集合。
        /// </summary>
        public XListPropertySet Properties => _properties;

        #endregion Members

        #region Construction

        /// <summary>
        /// 创建 XList 类的新实例。
        /// </summary>
        public XList() : base(XPropertyType.XList, new XElement(XConfigManager.Namespace + "list"), null, false)
        {
            _properties = new XListPropertySet(base.Content);
        }

        internal XList(XElement content, XComment comment, bool isProtected) : base(XPropertyType.XList, content, comment, isProtected)
        {
            if (!isProtected)
            {
                _properties = new XListPropertySet(content);
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
            _properties = new XListPropertySet(content);
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