using System;

namespace Honoo.Configuration
{
    /// <summary>
    /// 提供配置容器公共接口。
    /// </summary>
    public interface IConfigSection
    {
        /// <summary>
        /// 获取此配置容器的类型。
        /// </summary>
        ConfigSectionKind Kind { get; }

        /// <summary>
        /// 删除注释。
        /// </summary>
        /// <exception cref="Exception"/>
        void RemoveComment();

        /// <summary>
        /// 添加或更新或删除注释。
        /// </summary>
        /// <param name="comment">配置容器的注释。</param>
        /// <exception cref="Exception"/>
        void SetComment(string comment);

        /// <summary>
        /// 方法已重写。返回节点的缩进 XML 文本。
        /// </summary>
        /// <returns></returns>
        string ToString();

        /// <summary>
        /// 获取注释。
        /// <br/>如果没有找到指定键，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="comment">配置容器的注释。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        bool TryGetComment(out string comment);
    }
}