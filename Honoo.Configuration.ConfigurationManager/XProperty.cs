using System;
using System.Security.Cryptography;
using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 配置属性的基类。<see langword="&lt;dictionary /&gt;"/>、<see langword="&lt;list /&gt;"/>、<see langword="&lt;string /&gt;"/> 从此类中继承。
    /// </summary>
    public abstract class XProperty
    {
        private readonly XPropertyKind _kind;
        private XConfigAttributeSet _attributes;
        private XConfigComment _comment;
        private XElement _content;

        /// <summary>
        /// 获取配置属性集合。
        /// </summary>
        public XConfigAttributeSet Attributes => _attributes;

        /// <summary>
        /// 获取配置属性的注释。
        /// </summary>
        public XConfigComment Comment => _comment;

        /// <summary>
        /// 获取配置属性的类型。
        /// </summary>
        public XPropertyKind Kind => _kind;

        internal XElement Content => _content;

        #region Construction

        /// <summary>
        /// 创建 XProperty 的新实例。
        /// </summary>
        /// <param name="kind">配置属性的类型。</param>
        /// <param name="content">配置属性的节点元素。</param>
        /// <param name="comment">注释元素。</param>
        protected XProperty(XPropertyKind kind, XElement content, XComment comment)
        {
            _kind = kind;
            _content = content;
            _comment = new XConfigComment(comment, content);
            _attributes = new XConfigAttributeSet(content);
        }

        /// <summary>
        /// 创建 XProperty 的新实例。
        /// </summary>
        /// <param name="kind">配置属性的类型。</param>
        /// <param name="content">配置属性的节点元素。</param>
        /// <param name="comment">注释元素。</param>
        /// <param name="isProtected">指示此配置属性是否是已加密保护。</param>
        protected XProperty(XPropertyKind kind, XElement content, XComment comment, bool isProtected)
        {
            _kind = kind;
            _content = content;
            _comment = new XConfigComment(comment, content);
            if (!isProtected)
            {
                _attributes = new XConfigAttributeSet(content);
            }
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

        /// <summary>
        /// 加密或解密此配置属性。
        /// </summary>
        /// <param name="encrypt">加密或解密此配置属性。</param>
        /// <param name="protectionAlgorithm">指定一个非对称加密算法。</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        protected virtual XElement Reset(bool encrypt, RSA protectionAlgorithm)
        {
            if (protectionAlgorithm is null)
            {
                throw new ArgumentNullException(nameof(protectionAlgorithm));
            }
            XElement content = encrypt ? ProtectionHelper2.Encrypt(_content, protectionAlgorithm) : ProtectionHelper2.Decrypt(_content, protectionAlgorithm);
            _content.AddBeforeSelf(content);
            _content.Remove();
            _content = content;
            _comment = new XConfigComment(_comment.Comment, content);
            _attributes = new XConfigAttributeSet(content);
            return content;
        }
    }
}