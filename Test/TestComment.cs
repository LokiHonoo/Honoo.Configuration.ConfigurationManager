using Honoo.Configuration;
using System;

namespace Test
{
    /// <summary>
    /// HoonooSettings。
    /// </summary>
    internal static class TestComment
    {
        internal static void Test()
        {
            using (XConfigManager manager = new XConfigManager())
            {
                manager.Default.Comment.SetValue("default comment");
                manager.Default.Properties.AddOrUpdate("prop1", new XString("prop1 value.")).Comment.SetValue("prop1 comment.");

                XDictionary section = manager.Sections.GetOrAdd("section1");
                section.Comment.SetValue("section1 comment.");
                section.Properties.AddOrUpdate("prop1", new XString("123456789")).Comment.SetValue("prop1 comment.");
                //
                //
                //
                Console.WriteLine(manager.Default.Comment.GetValue());
                Console.WriteLine(manager.Sections["section1"].Comment.GetValue());
                Console.WriteLine(manager.Sections["section1"].Properties["prop1"].Comment.GetValue());
            }

            using (AppSettingsManager manager = new AppSettingsManager())
            {
                manager.Properties.AddOrUpdate("prop1", "prop1 value.").Comment.SetValue("prop1 comment.");
                //
                //
                //
                Console.WriteLine(manager.Properties["prop1"].Comment.GetValue());
            }

            using (ConfigurationManager manager = new ConfigurationManager())
            {
                manager.AppSettings.Properties.AddOrUpdate("prop1", "prop1 value.").Comment.SetValue("prop1 comment.");
                manager.ConnectionStrings.Properties.AddOrUpdate("prop2", "prop2 value.", "").Comment.SetValue("prop2 comment.");
                manager.AssemblyBinding.Properties.Add("prop3").Comment.SetValue("prop3 comment.");
                var group = manager.ConfigSections.Groups.GetOrAdd("group1");
                group.Comment.SetValue("group1 comment.");
                var section1 = group.Sections.GetOrAdd<SingleTagSection>("section1");
                section1.Comment.SetValue("section1 comment.");
                section1.Properties.AddOrUpdate("prop3", "prop3");
                var section2 = group.Sections.GetOrAdd<NameValueSection>("section2");
                section2.Comment.SetValue("section2 comment.");
                section2.Properties.Add("prop4", "prop4").Comment.SetValue("prop4 comment.");
                var section3 = group.Sections.GetOrAdd<DictionarySection>("section3");
                section3.Comment.SetValue("section3 comment.");
                section3.Properties.Add("prop5", "prop5").Comment.SetValue("prop5 comment.");
                //
                //
                //
                Console.WriteLine(manager.AppSettings.Properties["prop1"].Comment.GetValue());
                Console.WriteLine(manager.ConnectionStrings.Properties["prop2"].Comment.GetValue());
                Console.WriteLine(manager.AssemblyBinding.Properties[0].Comment.GetValue());
                group = manager.ConfigSections.Groups["group1"];
                Console.WriteLine(group.Comment.GetValue());
                Console.WriteLine(group.Sections["section1"].Comment.GetValue());
                section2 = (NameValueSection)group.Sections["section2"];
                Console.WriteLine(section2.Comment.GetValue());
                Console.WriteLine(section2.Properties["prop4"][0].Comment.GetValue());
                section3 = (DictionarySection)group.Sections["section3"];
                Console.WriteLine(section3.Comment.GetValue());
                Console.WriteLine(section3.Properties["prop5"].Comment.GetValue());
            }
        }
    }
}