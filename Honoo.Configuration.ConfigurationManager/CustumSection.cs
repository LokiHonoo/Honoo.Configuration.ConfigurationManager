using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 配置容器。
    /// </summary>
    public sealed class CustumSection : IConfigSection
    {
        private readonly XElement _content;
        private readonly ISavable _savable;

        /// <summary>
        /// 获取或设置配置容器的串联内容。
        /// </summary>
        public string Value
        {
            get
            {
                return GetValue();
            }
            set
            {
                SetValue(value);
            }
        }

        #region Construction

        internal CustumSection(XElement content, ISavable savable)
        {
            _content = content;
            _savable = savable;
        }

        #endregion Construction

        /// <summary>
        /// 确定指定的对象是否等于当前对象。
        /// </summary>
        /// <param name="obj">要与当前对象进行比较的对象。</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return obj is CustumSection other && _content.Equals(other._content);
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
        /// 获取配置容器的串联内容。
        /// </summary>
        /// <returns></returns>
        public string GetValue()
        {
            return _content.Value;
        }

        /// <summary>
        /// 设置配置容器的串联内容。
        /// </summary>
        /// <param name="value">配置容器的串联内容。</param>
        /// <returns></returns>
        ///
        public void SetValue(string value)
        {
            if (value is null || value.Length == 0)
            {
                if (_content.Value.Length != 0)
                {
                    _content.SetValue(string.Empty);
                    if (_savable.AutoSave)
                    {
                        _savable.Save();
                    }
                }
            }
            else
            {
                if (!_content.Value.Equals(value))
                {
                    _content.SetValue(value);
                    if (_savable.AutoSave)
                    {
                        _savable.Save();
                    }
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
    }
}