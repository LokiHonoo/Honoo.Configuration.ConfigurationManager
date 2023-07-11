using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 配置属性集合。
    /// </summary>
    public sealed class AppSettingsPropertySet : DictionaryPropertySet
    {
        internal AppSettingsPropertySet(XElement superior) : base(superior)
        {
        }
    }
}