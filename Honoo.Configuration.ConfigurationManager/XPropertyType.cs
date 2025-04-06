namespace Honoo.Configuration
{
    /// <summary>
    /// 标签配置属性的类型。
    /// </summary>
    public enum XPropertyType
    {
        /// <summary>
        /// 字典配置属性类型。标签是 <see langword="&lt;dictionary /&gt;"/>，内容为字典配置属性集合。
        /// </summary>
        XDictionary = 0,

        /// <summary>
        /// 列表配置属性类型。标签是 <see langword="&lt;list /&gt;"/>，内容为列表配置属性集合。
        /// </summary>
        XList,

        /// <summary>
        /// 串行数据配置属性类型。标签是 <see langword="&lt;string /&gt;"/>，内容为 <see cref="string"/>。
        /// </summary>
        XString,
    }
}