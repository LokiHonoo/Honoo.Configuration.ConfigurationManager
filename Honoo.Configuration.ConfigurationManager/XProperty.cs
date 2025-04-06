using System;
using System.Security.Cryptography;
using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 标签配置属性的基类。<see langword="&lt;dictionary /&gt;"/>、<see langword="&lt;list /&gt;"/>、<see langword="&lt;string /&gt;"/> 从此类中继承。
    /// </summary>
    public abstract class XProperty
    {
        private readonly XPropertyType _propertyType;
        private XConfigAttributeSet _attributes;
        private XConfigComment _comment;
        private XElement _content;
        private bool _isProtected;

        /// <summary>
        /// 获取配置属性集合。
        /// </summary>
        public XConfigAttributeSet Attributes => _attributes;

        /// <summary>
        /// 获取配置属性的注释。
        /// </summary>
        public XConfigComment Comment => _comment;

        /// <summary>
        /// 获取一个值，指示此配置容器是否是已加密保护。
        /// </summary>
        public bool IsProtected => _isProtected;

        /// <summary>
        /// 获取配置属性的类型。
        /// </summary>
        public XPropertyType PropertyType => _propertyType;

        internal XElement Content => _content;

        #region Construction

        /// <summary>
        /// 创建 XProperty 的新实例。
        /// </summary>
        /// <param name="propertyTyp">配置属性的类型。</param>
        /// <param name="content">配置属性的节点元素。</param>
        /// <param name="comment">注释元素。</param>
        /// <param name="isProtected">指示此配置属性是否是已加密保护。</param>
        protected XProperty(XPropertyType propertyTyp, XElement content, XComment comment, bool isProtected)
        {
            _propertyType = propertyTyp;
            _content = content;
            _comment = new XConfigComment(comment, content);
            _isProtected = isProtected;
            if (!isProtected)
            {
                _attributes = new XConfigAttributeSet(content);
            }
        }

        #endregion Construction

        /// <summary>
        /// 解密此配置属性。
        /// </summary>
        /// <param name="protectionAlgorithm">指定一个非对称加密算法，算法必须拥有私钥。</param>
        /// <exception cref="Exception"></exception>
        public void Decrypt(RSA protectionAlgorithm)
        {
            DecryptInternal(protectionAlgorithm);
        }

        /// <summary>
        /// 加密此配置属性。
        /// </summary>
        /// <param name="protectionAlgorithm">指定一个非对称加密算法，算法可以是公钥或私钥。</param>
        /// <exception cref="Exception"></exception>
        public void Encrypt(RSA protectionAlgorithm)
        {
            EncryptInternal(protectionAlgorithm);
        }

        /// <summary>
        /// 方法已重写。返回节点的缩进 XML 文本。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _content.ToString();
        }

        /// <summary>
        /// 解密此配置属性。
        /// </summary>
        /// <param name="protectionAlgorithm">指定一个非对称加密算法，算法必须拥有私钥。</param>
        /// <exception cref="Exception"></exception>
        protected virtual XElement DecryptInternal(RSA protectionAlgorithm)
        {
            if (protectionAlgorithm is null)
            {
                throw new ArgumentNullException(nameof(protectionAlgorithm));
            }
            if (!_isProtected)
            {
                throw new CryptographicException($"This property is not encrypted.");
            }
            XElement content = ProtectionHelper.Decrypt(_content, protectionAlgorithm);
            _content.AddBeforeSelf(content);
            _content.Remove();
            _content = content;
            _comment = new XConfigComment(_comment.Comment, content);
            _attributes = new XConfigAttributeSet(content);
            _isProtected = false;
            return content;
        }

        /// <summary>
        /// 加密此配置属性。
        /// </summary>
        /// <param name="protectionAlgorithm">指定一个非对称加密算法，算法可以是公钥或私钥。</param>
        /// <exception cref="Exception"></exception>
        protected virtual XElement EncryptInternal(RSA protectionAlgorithm)
        {
            if (protectionAlgorithm is null)
            {
                throw new ArgumentNullException(nameof(protectionAlgorithm));
            }
            if (_isProtected)
            {
                throw new CryptographicException($"This property is encrypted yet.");
            }
            XElement content = ProtectionHelper.Encrypt(_content, protectionAlgorithm);
            _content.AddBeforeSelf(content);
            _content.Remove();
            _content = content;
            _comment = new XConfigComment(_comment.Comment, content);
            _attributes = null;
            _isProtected = true;
            return content;
        }
    }
}