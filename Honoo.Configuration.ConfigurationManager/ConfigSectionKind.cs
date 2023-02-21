using System.ComponentModel;

namespace Honoo.Configuration
{
    /// <summary>
    /// 配置容器的类型。
    /// </summary>
    public enum ConfigSectionKind
    {
        /// <summary>
        /// Honoo.Configuration.TextSectionHandler 类型。
        /// </summary>
        [Description("Honoo.Configuration.ConfigurationSectionHandler")]
        TextSection = 0,

        /// <summary>
        /// System.Configuration.SingleTagSectionHandler 类型。
        /// </summary>
        [Description("System.Configuration.SingleTagSectionHandler")]
        SingleTagSection = 1,

        /// <summary>
        /// System.Configuration.NameValueSectionHandler 类型。
        /// </summary>
        [Description("System.Configuration.NameValueSectionHandler")]
        NameValueSection = 2,

        /// <summary>
        /// System.Configuration.DictionarySectionHandler 类型。
        /// </summary>
        [Description("System.Configuration.DictionarySectionHandler")]
        DictionarySection = 3,
    }
}