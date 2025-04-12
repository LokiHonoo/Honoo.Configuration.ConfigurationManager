using Honoo.Configuration;
using System;
using System.Data.SqlClient;
using System.IO;

namespace Test
{
    internal static class TestC
    {
        private static bool _isFirst = true;

        internal static void Create()
        {
            if (_isFirst)
            {
                File.Delete("config.exrea.xml");
                File.Delete("config.main.xml");
                _isFirst = false;
            }
            //
            // 测试连接。
            //
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder()
            {
                DataSource = "127.0.0.1",
                InitialCatalog = "DemoCatalog",
                UserID = "sa",
                Password = "12345"
            };
            SqlConnection connection = new SqlConnection(builder.ConnectionString);
            //
            // 附加配置文件。
            //
            using (AppSettingsManager manager = new AppSettingsManager("config.exrea.xml", true))
            {
                manager.Properties.AddOrUpdate("exrea-prop1", new AddProperty("exrea-prop1")).Comment.SetValue("This is exrea file prop1 comment.");
                manager.Properties.AddOrUpdate("exrea-prop2", new AddProperty("exrea-prop2")).Comment.SetValue("This is exrea file prop2 comment.");
                manager.Save("config.exrea.xml");
            }
            //
            // 使用 .NET 程序的默认配置文件或自定义配置文件。
            //
            using (ConfigurationManager manager = new ConfigurationManager("config.main.xml", true))
            {
                #region AppSettings

                manager.AppSettings.SetFileAttribute("config.exrea.xml");
                //
                // 添加或更新属性，并设置注释。
                //
                manager.AppSettings.Properties.AddOrUpdate("appSettings-prop1", new AddProperty("appSettings-prop1")).Comment.SetValue("This is \"appSettings\" prop1 comment.");
                manager.AppSettings.Properties.AddOrUpdateString("appSettings-prop2", "123456789").Comment.SetValue("This is \"appSettings\" prop2 comment.");
                manager.AppSettings.Properties.AddOrUpdateString("appSettings-prop3", "F058C");
                manager.AppSettings.Properties.AddOrUpdate("appSettings-prop4", new AddProperty(LoaderOptimization.SingleDomain.ToString()));
                manager.AppSettings.Properties.AddOrUpdate("appSettings-prop5", new AddProperty("Update this.")).Comment.SetValue("Will be update prop5.");
                manager.AppSettings.Properties.AddOrUpdate("appSettings-prop6", new AddProperty("Remove this.")).Comment.SetValue("Will be remove prop6.");
                //
                // 更新。
                //
                manager.AppSettings.Properties.AddOrUpdateString("appSettings-prop5", "Update this successful.").Comment.SetValue("Update prop5 successful.");
                //
                // 移除属性的方法。移除属性时相关注释一并移除。
                //
                manager.AppSettings.Properties.Remove("appSettings-prop6");
                //
                // 添加同名属性异常。
                //
                try
                {
                    manager.AppSettings.Properties.Add("appSettings-prop1", new AddProperty("Test unique.")); // Allways error.
                }
                catch (Exception)
                {
                    manager.AppSettings.Properties.AddOrUpdateString("appSettings-prop99", "Add prop1 exception OK.");
                }

                #endregion AppSettings

                #region ConnectionStrings

                //
                // 添加或更新属性，并设置注释。
                //
                manager.ConnectionStrings.Properties.AddOrUpdate("connectionStrings-prop1", new ConnectionStringProperty(connection)).Comment.SetValue("This is \"connectionStrings\" prop1 comment.");
                manager.ConnectionStrings.Properties.AddOrUpdate("connectionStrings-prop2", new ConnectionStringProperty(connection.ConnectionString, connection.GetType().Namespace));
                manager.ConnectionStrings.Properties.AddOrUpdate("connectionStrings-prop3", connection.ConnectionString, connection.GetType().Namespace).Comment.SetValue("Will be remove prop3.");
                //
                // 移除属性的方法。移除属性时相关注释一并移除。
                //
                manager.ConnectionStrings.Properties.Remove("connectionStrings-prop3");
                //
                // 添加同名属性异常。
                //
                try
                {
                    manager.ConnectionStrings.Properties.Add("connectionStrings-prop1", connection.ConnectionString, connection.GetType().Namespace); // Allways error.
                }
                catch (Exception)
                {
                    manager.ConnectionStrings.Properties.AddOrUpdate("connectionStrings-prop99", "Add prop1 exception OK.", string.Empty);
                }

                #endregion ConnectionStrings

                #region ConfigSections

                //
                // 添加或更新容器组，并设置注释。
                //
                ConfigSectionGroup group1 = manager.ConfigSections.Groups.AddOrUpdate("group1");
                group1.Comment.SetValue("This is \"configSections\" group1 comment.");
                group1.Sections.AddOrUpdate<DictionarySection>("group1-section1").Comment.SetValue("This is group1 section1 comment.");
                //
                // 添加或更新容器，并设置注释。
                //
                DictionarySection dictSection = manager.ConfigSections.Sections.AddOrUpdate<DictionarySection>("dict-section1");
                dictSection.Comment.SetValue("This is main section1 comment.");
                SingleTagSection stSection = manager.ConfigSections.Sections.AddOrUpdate<SingleTagSection>("st-section2");
                stSection.Comment.SetValue("This is main section2 comment.");
                NameValueSection nvtSection = manager.ConfigSections.Sections.AddOrUpdate<NameValueSection>("nv-section3");
                nvtSection.Comment.SetValue("This is main section3 comment.");
                TextSection textSection = manager.ConfigSections.Sections.AddOrUpdate<TextSection>("section4");
                textSection.Comment.SetValue("This is main section4 comment.");
                //
                // 添加或更新属性，并设置注释。
                //
                dictSection.Properties.AddOrUpdate("dict-prop1", new AddProperty("dict-prop1")).Comment.SetValue("This is \"dict-section\" prop1 comment.");
                stSection.Properties.AddOrUpdate("st-prop1-name", new SingleTagProperty("st-prop1"));
                stSection.Properties.AddOrUpdate("st-prop2-name", new SingleTagProperty("st-prop2"));
                try
                {
                    stSection.Properties.Add("st-prop2-name", new SingleTagProperty("st-prop2")); // Allways error.
                }
                catch (Exception)
                {
                    stSection.Properties.AddOrUpdateString("st-prop99-name", "Add st-prop2 exception OK.");
                }
                nvtSection.Properties.Add("nv-prop1", new AddProperty("123456789")).Comment.SetValue("This is \"nv-section\" prop1 comment.");
                nvtSection.Properties.Add("nv-prop1", new AddProperty("987654321"));
                textSection.SetValue("<arbitrarily>abc</arbitrarily><arbitrarily><![CDATA[sssAAA]]></arbitrarily>");

                #endregion ConfigSections

                //
                // 保存到指定的文件。
                //
                manager.Save("config.main.xml");
            }
        }

        internal static void Load()
        {
            using (ConfigurationManager manager = new ConfigurationManager("config.main.xml"))
            {
                Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
                Console.WriteLine(manager.ToString());
                manager.Save("config.main.xml");
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
                Console.WriteLine(File.ReadAllText("config.main.xml"));
            }
        }
    }
}