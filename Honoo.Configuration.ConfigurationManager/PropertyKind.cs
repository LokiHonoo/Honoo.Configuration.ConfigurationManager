namespace Honoo.Configuration
{
    /// <summary>
    /// 配置属性的类型。
    /// </summary>
    public enum PropertyKind
    {
        /// <summary>
        /// 配置属性的标签是 <see langword="&lt;add /&gt;"/>。
        /// </summary>
        Add = 0,

        /// <summary>
        /// 配置属性的标签是 <see langword="&lt;remove /&gt;"/>。
        /// </summary>
        Remove = 1,

        /// <summary>
        /// 配置属性的标签是 <see langword="&lt;clear /&gt;"/>。
        /// </summary>
        Clear = 2,
    }
}