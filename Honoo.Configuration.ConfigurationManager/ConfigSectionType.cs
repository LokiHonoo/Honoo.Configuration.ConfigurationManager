using System.ComponentModel;

namespace Honoo.Configuration
{
    /// <summary>
    /// 配置容器的类型。
    /// </summary>
    public enum ConfigSectionType
    {
        /// <summary>
        /// Honoo.Configuration.CustumSectionHandler 类型。
        /// </summary>
        [Description("Honoo.Configuration.CustumSectionHandler")]
        CustumSection = 0,

        /// <summary>
        /// System.Configuration.DictionarySectionHandler 类型。
        /// </summary>
        [Description("System.Configuration.DictionarySectionHandler")]
        DictionarySection = 1,

        /// <summary>
        /// System.Configuration.NameValueSectionHandler 类型。
        /// </summary>
        [Description("System.Configuration.NameValueSectionHandler")]
        NameValueSection = 2,

        /// <summary>
        /// System.Configuration.SingleTagSectionHandler 类型。
        /// </summary>
        [Description("System.Configuration.SingleTagSectionHandler")]
        SingleTagSection = 3,
    }
}