namespace Honoo.Configuration
{
    /// <summary>
    /// 本地化消息。
    /// </summary>
    public static class Localization
    {
        /// <summary>
        /// 表示指定了无效的键时引发的错误。
        /// </summary>
        public static string EX_0X0001_InvalidKey { get; set; } = "The invalid key.";

        /// <summary>
        /// 表示指定了无效的类型或类型枚举时引发的错误。
        /// </summary>
        public static string EX_0X0002_InvalidType { get; set; } = "The invalid type.";

        /// <summary>
        /// 表示在一个列表中，遇到重复键时引发的错误。
        /// </summary>
        public static string EX_0X0003_DuplicateKey { get; set; } = "The specified key already exists.";
    }
}