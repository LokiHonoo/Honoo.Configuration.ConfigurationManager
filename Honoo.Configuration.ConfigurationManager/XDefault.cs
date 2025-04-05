using System;
using System.Security.Cryptography;
using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 映射到 &lt;default /&gt; 配置容器节点。
    /// </summary>
    public class XDefault : XDictionary
    {
        #region Members

        private bool _isProtected;

        /// <summary>
        /// 获取一个值，指示此配置容器是否是已加密保护。
        /// </summary>
        public bool IsProtected => _isProtected;

        #endregion Members

        #region Construction

        internal XDefault(XElement content, XComment comment) : base(content, comment, QueryProtected(content, out bool isProtected))
        {
            _isProtected = isProtected;
        }

        #endregion Construction

        /// <summary>
        /// 解密此配置容器。
        /// </summary>
        /// <param name="protectionAlgorithm">指定一个非对称加密算法，算法必须拥有私钥。</param>
        /// <exception cref="Exception"></exception>
        public void Decrypt(RSA protectionAlgorithm)
        {
            if (protectionAlgorithm is null)
            {
                throw new ArgumentNullException(nameof(protectionAlgorithm));
            }
            if (!_isProtected)
            {
                throw new CryptographicException($"\"Default\" is not encrypted.");
            }
            base.Reset(false, protectionAlgorithm);
            _isProtected = false;
        }

        /// <summary>
        /// 加密此配置容器。
        /// </summary>
        /// <param name="protectionAlgorithm">指定一个非对称加密算法，算法可以是公钥或私钥。</param>
        /// <exception cref="Exception"></exception>
        public void Encrypt(RSA protectionAlgorithm)
        {
            if (protectionAlgorithm is null)
            {
                throw new ArgumentNullException(nameof(protectionAlgorithm));
            }
            if (_isProtected)
            {
                throw new CryptographicException($"\"Default\" is encrypted yet.");
            }
            base.Reset(true, protectionAlgorithm);
            _isProtected = true;
        }

        private static bool QueryProtected(XElement content, out bool isProtected)
        {
            if (content.Attribute("protected") is XAttribute attribute)
            {
                if (bool.TryParse(attribute.Value, out isProtected))
                {
                    return isProtected;
                }
                else
                {
                    throw new CryptographicException($"Attribute \"protected\" is not a boolean value.");
                }
            }
            isProtected = false;
            return isProtected;
        }
    }
}