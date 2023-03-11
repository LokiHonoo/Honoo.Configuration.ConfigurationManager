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
        /// 方法已重写。返回节点的缩进 XML 文本。
        /// </summary>
        /// <returns></returns>
        string ToString();
    }
}