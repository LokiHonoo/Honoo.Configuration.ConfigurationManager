namespace Honoo.Configuration
{
    /// <summary>
    /// 标签配置属性的类型。
    /// </summary>
    public enum ConfigPropertyType
    {
        /// <summary>
        /// 配置属性的标签是 <see langword="&lt;add /&gt;"/>，并具有 <see langword="key"/>，<see langword="value"/> 属性字段。
        /// </summary>
        AddProperty,

        /// <summary>
        /// 配置属性的标签是 <see langword="&lt;remove /&gt;"/>，并具有 <see langword="key"/> 属性字段。
        /// </summary>
        RemoveProperty,

        /// <summary>
        /// 配置属性的标签是 <see langword="&lt;clear /&gt;"/>。
        /// </summary>
        ClearProperty,
    }
}