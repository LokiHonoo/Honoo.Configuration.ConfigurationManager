namespace Honoo.Configuration
{
    /// <summary>
    /// 指定要转换为 <see cref="byte"/>[] 类型的字符串的源格式。
    /// </summary>
    public enum XStringFormat
    {
        /// <summary>
        /// 源格式是二进制字符串。
        /// </summary>
        Binary,

        /// <summary>
        /// 源格式是十六进制字符串。
        /// </summary>
        Hex,

        /// <summary>
        /// 源格式是 Base64 字符串。
        /// </summary>
        Base64
    }
}