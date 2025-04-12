# Honoo.Configuration.ConfigurationManager

<!-- @import "[TOC]" {cmd="toc" depthFrom=1 depthTo=6 orderedList=false} -->

<!-- code_chunk_output -->

- [Honoo.Configuration.ConfigurationManager](#honooconfigurationconfigurationmanager)
  - [INTRODUCTION](#introduction)
  - [GUIDE](#guide)
    - [NuGet](#nuget)
  - [USAGE](#usage)
    - [Namespace](#namespace)
    - [ConfigurationManager](#configurationmanager)
    - [Auto save](#auto-save)
    - [XConfigManager](#xconfigmanager)
  - [CHANGELOG](#changelog)
  - [LICENSE](#license)

<!-- /code_chunk_output -->

## INTRODUCTION

此项目是 System.Configuration.ConfigurationManager 的简单替代。

用于 .NET Framework 4.0+/.NET Standard 2.0+ 中读写默认配置文件或自定义配置文件。

提供对标准节点 appSettings、connectionStrings、configSections assemblyBinding/linkedConfiguration 节点的有限读写支持。

提供 XConfigManager 类读写一个精简的配置属性文件，支持字典/列表类型嵌套，支持节点加密。

## GUIDE

### NuGet

<https://www.nuget.org/packages/Honoo.Configuration.ConfigurationManager/>

## USAGE

### Namespace

```c#

using Honoo.Configuration;

```

### ConfigurationManager

```c#

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

```

### Auto save

在事件中实现自动保存。

```c#

internal static void AutoSaveDemo()
{
    using (ConfigurationManager manager = new ConfigurationManager())
    {
        manager.Changed += Manager_Changed;
        manager.Disposing += Manager_Disposing;
    }
}

private static void Manager_Changed(ConfigurationManager manager)
{
    manager.Save(filePath);
}

private static void Manager_Disposing(ConfigurationManager manager)
{
    manager.Save(filePath);
}

```

### XConfigManager

提供 XConfigManager 类读写一个精简的配置属性文件，支持加密，支持字典/列表类型无限嵌套。

```c#

internal static class TestXX
{
    private static bool _isFirst = true;
    private static RSA _rsa = RSA.Create();

    internal static void Create()
    {
        if (_isFirst)
        {
            File.Delete("config.x.xml");
            _isFirst = false;
        }
        _rsa.FromXmlString(keyStored);
        //
        // 使用自定义配置文件。
        //
        using (XConfigManager manager = new XConfigManager("config.x.xml", true))
        {
            #region Default

            //
            // 添加或更新属性，并设置注释。
            //
            manager.Default.Properties.AddOrUpdate("prop1", new XString("prop1")).Attributes.AddOrUpdateString("attr1", "attr1");
            XDictionary dict1 = manager.Default.Properties.AddOrUpdate("dictionary1", new XDictionary());
            dict1.Comment.SetValue("This is dictionary1 comment.");
            dict1.Properties.AddOrUpdateString("dict-prop1", "dict-prop1").Comment.SetValue("This is dictionary1 prop1 comment.");
            dict1.Properties.AddOrUpdate("dict-prop2", new XString("dict-prop2"));
            XList list1 = dict1.Properties.AddOrUpdate("dict-prop3-list1", new XList());
            list1.Properties.Add(new XString("dict-prop3-list1-prop1"));
            manager.Default.Properties.AddOrUpdate("prop5", new XString("Update this.")).Comment.SetValue("Will be update prop5.");
            manager.Default.Properties.AddOrUpdate("prop6", new XString("Remove this.")).Comment.SetValue("Will be remove prop6.");
            //
            // 更新。
            //
            manager.Default.Properties.AddOrUpdateString("prop5", "Update this successful.").Comment.SetValue("Update prop5 successful.");
            //
            // 移除属性的方法。移除属性时相关注释一并移除。
            //
            manager.Default.Properties.Remove("prop6");
            //
            // 添加同名属性异常。
            //
            try
            {
                manager.Default.Properties.Add("prop1", new XString("Test unique.")); // Allways error.
            }
            catch (Exception)
            {
                manager.Default.Properties.AddOrUpdateString("prop99", "Add prop1 exception OK.");
            }

            #endregion Default

            #region Sections

            //
            // 添加或更新容器组，并设置注释。
            //
            manager.Sections.AddOrUpdate("section1");
            manager.Sections.AddOrUpdate("section2").Comment.SetValue("This is section2 comment."); ;
            manager.Sections.AddOrUpdate("section3");
            XSection section1 = manager.Sections.GetOrAdd("section1");
            section1.Comment.SetValue("This is section1 comment.");
            //
            // 添加或更新属性，并设置注释。
            //
            section1.Properties.AddOrUpdate("section1-dict1", new XDictionary()).Comment.SetValue("This is section1-dict1 comment.");
            section1.Properties.AddOrUpdate("section1-list1", new XList()).Comment.SetValue("This is section1-list1 comment.");
            section1.Properties.AddOrUpdate("section1-string1", new XString("section1-string1")).Comment.SetValue("This is section1-string1 comment.");
            //
            // 加密。
            //
            section1.Encrypt(_rsa);

            #endregion Sections

            //
            // 保存到指定的文件。
            //
            manager.Save("config.x.xml");
        }
    }

    internal static void Load()
    {
        using (XConfigManager manager = new XConfigManager("config.x.xml"))
        {
            Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
            Console.WriteLine(manager.ToString());
            Console.WriteLine();
            Console.WriteLine();
            manager.Sections.GetValue("section1").Decrypt(_rsa);
            Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
            Console.WriteLine(manager.ToString());
            manager.Save("config.x.xml");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
            Console.WriteLine(File.ReadAllText("config.x.xml"));
        }
    }
}

```

## CHANGELOG

[CHANGELOG](CHANGELOG.md)

## LICENSE

[MIT](LICENSE) license.
