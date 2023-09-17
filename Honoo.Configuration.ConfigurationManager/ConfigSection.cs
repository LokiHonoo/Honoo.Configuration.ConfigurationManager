using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 配置容器的基类。
    /// </summary>
    public abstract class ConfigSection
    {
        /// <summary>
        /// 获取此配置容器的内容节点。
        /// </summary>
        protected readonly XElement _content;

        /// <summary>
        /// 获取此配置容器的类型。
        /// </summary>
        protected readonly ConfigSectionKind _kind;

        /// <summary>
        /// 获取此配置容器的注释节点。
        /// </summary>
        protected XComment _comment = null;

        /// <summary>
        /// 获取此配置容器的类型。
        /// </summary>
        public ConfigSectionKind Kind => _kind;

        #region Construction

        /// <summary>
        /// 创建 ConfigSection 的新实例。
        /// </summary>
        /// <param name="kind">配置容器的类型。</param>
        /// <param name="content">配置容器的内容节点。</param>
        /// <param name="comment">配置容器的注释节点。</param>
        protected ConfigSection(ConfigSectionKind kind, XElement content, XComment comment)
        {
            _kind = kind;
            _comment = comment;
            _content = content;
        }

        #endregion Construction

        /// <summary>
        /// 删除注释。
        /// <br/>如果注释成功删除，返回 <see langword="true"/>。如果没有找到注释节点，则返回 <see langword="false"/>。
        /// </summary>
        /// <returns></returns>
        public bool RemoveComment()
        {
            if (_comment != null)
            {
                _comment.Remove();
                _comment = null;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 添加或更新或删除注释。
        /// </summary>
        /// <param name="comment">配置容器的注释。</param>
        public void SetComment(string comment)
        {
            if (comment == null)
            {
                RemoveComment();
            }
            else
            {
                if (_comment == null)
                {
                    _comment = new XComment(comment);
                    _content.AddBeforeSelf(_comment);
                }
                else
                {
                    _comment.Value = comment;
                }
            }
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
        /// 获取注释。
        /// <br/>如果没有找到注释，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="comment">配置容器的注释。</param>
        /// <returns></returns>
        public bool TryGetComment(out string comment)
        {
            if (_comment != null)
            {
                comment = _comment.Value;
                return true;
            }
            comment = null;
            return false;
        }
    }
}