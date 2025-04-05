# Honoo.Configuration.ConfigurationManager

<!-- @import "[TOC]" {cmd="toc" depthFrom=1 depthTo=6 orderedList=false} -->

<!-- code_chunk_output -->

- [Honoo.Configuration.ConfigurationManager](#honooconfigurationconfigurationmanager)
  - [INTRODUCTION](#introduction)
  - [GUIDE](#guide)
    - [NuGet](#nuget)
  - [USAGE](#usage)
    - [Namespace](#namespace)
    - [appSettings](#appsettings)
    - [connectionStrings](#connectionstrings)
    - [sectionGroup/section](#sectiongroupsection)
    - [Auto save](#auto-save)
    - [Protection](#protection)
    - [XConfigManager](#xconfigmanager)
  - [CHANGELOG](#changelog)
  - [LICENSE](#license)

<!-- /code_chunk_output -->

## INTRODUCTION

此项目是 System.Configuration.ConfigurationManager 的简单替代。

用于 .NET Framework 4.0+/.NET Standard 2.0+ 中读写默认配置文件或自定义配置文件。

提供对标准节点 appSettings、connectionStrings、configSections assemblyBinding/linkedConfiguration 节点的有限读写支持。

提供 XConfigManager 类读写一个精简的配置属性文件，支持加密，支持字典/列表类型嵌套。

## GUIDE

### NuGet

<https://www.nuget.org/packages/Honoo.Configuration.ConfigurationManager/>

## USAGE

### Namespace

```c#

using Honoo.Configuration;

```

### appSettings

```c#

internal static void Create(string filePath)
{
    //
    // 使用 .NET 程序的默认配置文件或自定义配置文件。
    //
    using (ConfigurationManager manager = File.Exists(filePath) ? new ConfigurationManager(filePath) : new ConfigurationManager())
    {
        manager.AppSettings.SetFileAttribute("config.exrea1.xml");
        //
        // 赋值并设置注释。
        //
        manager.AppSettings.Properties.AddOrUpdate("prop1", new AddProperty("This is \"appSettings\" prop1 value.")).Comment.SetValue("This is \"appSettings\" prop1 comment.");
        manager.AppSettings.Properties.AddOrUpdate("prop6", new AddProperty("Update this."));
        //manager.AppSettings.Properties.Add(new ClearProperty());
        manager.AppSettings.Properties.AddOrUpdate("prop2", new AddProperty("123456789"));
        manager.AppSettings.Properties.AddOrUpdate("prop3", new AddProperty("F058C"));
        manager.AppSettings.Properties.AddOrUpdate("prop4", new AddProperty(LoaderOptimization.SingleDomain.ToString()));
        manager.AppSettings.Properties.AddOrUpdate("prop5", new AddProperty("Remove this."));
        //manager.AppSettings.Properties.Add("prop4", new RemoveProperty());
        //manager.AppSettings.Properties.Add("prop1", new AddProperty("Test unique."));
        //
        // 移除属性的方法。移除属性时相关注释一并移除。
        //
        manager.AppSettings.Properties.Remove("prop5");
        //
        // 更新。
        //
        manager.AppSettings.Properties.AddOrUpdate("prop6", new AddProperty("Update this successful."));
        //
        // 保存到指定的文件。
        //
        manager.Save(filePath);
    }
}

internal static void Load(string filePath)
{
    //
    // 使用 .NET 程序的默认配置文件或自定义配置文件。
    //
    using (ConfigurationManager manager = new ConfigurationManager(filePath))
    {
        //
        // 取出属性和注释。
        //
        AddProperty value1 = manager.AppSettings.Properties.GetValue("prop1");
        if (value1.Comment.TryGetValue(out string comment1))
        {
            Console.WriteLine(comment1);
        }
        Console.WriteLine(value1.Value);
        //
        int value2 = manager.AppSettings.Properties.GetValue("prop2", 55555);
        Console.WriteLine(value2);
        //
        AddProperty value3 = manager.AppSettings.Properties["prop3"];
        Console.WriteLine(value3.GetValue(Binaries.Empty));
        //
        if (manager.AppSettings.Properties.TryGetValue("prop4", out LoaderOptimization value4))
        {
            Console.WriteLine(value4);
        }
        // 取出应用控制标签后的属性。
        foreach (AddProperty property in manager.AppSettings.GetControlledProperties())
        {
            Console.WriteLine(property.Value);
        }
    }
}

```

### connectionStrings

```c#

internal static void Create(string filePath)
{
    //
    // 使用 .NET 程序的默认配置文件或自定义配置文件。
    //
    using (ConfigurationManager manager = new ConfigurationManager(filePath))
    {
        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder()
        {
            DataSource = "127.0.0.1",
            InitialCatalog = "DemoCatalog",
            UserID = "sa",
            Password = "12345"
        };
        SqlConnection connection = new SqlConnection(builder.ConnectionString);
        //
        // 赋值并设置注释。
        //
        manager.ConnectionStrings.Properties.AddOrUpdate("prop1", connection).Comment.SetValue("This is \"connectionStrings\" prop1 comment.");
        manager.ConnectionStrings.Properties.AddOrUpdate("prop2", connection.ConnectionString, connection.GetType().Namespace);
        manager.ConnectionStrings.Properties.AddOrUpdate("prop3", connection.ConnectionString, string.Empty);
        manager.ConnectionStrings.Properties.AddOrUpdate("prop4", connection).Comment.SetValue("It's will remove this.");;
        //manager.ConnectionStrings.Properties.Add("prop1", connection);
        //
        // 移除属性的方法。
        //
        manager.ConnectionStrings.Properties.Remove("prop4");
        //
        // 保存到指定的文件。
        //
        manager.Save(filePath);
    }
}

internal static void Load(string filePath)
{
    //
    // 使用 .NET 程序的默认配置文件或自定义配置文件。
    //
    using (ConfigurationManager manager = new ConfigurationManager(filePath))
    {
        //
        // 取出属性和注释。
        //
        ConnectionStringProperty value1 = manager.ConnectionStrings.Properties.GetValue("prop1");
        if (value1.Comment.TryGetValue(out string comment1))
        {
            Console.WriteLine(comment1);
        }
        Console.WriteLine(value1.ConnectionString);
        //
        if (manager.ConnectionStrings.Properties.TryGetValue("prop2", out ConnectionStringProperty value2))
        {
            Console.WriteLine(value2.ConnectionString);
            //
            // 访问实例。
            //
            SqlConnection connection = value2.CreateInstance<SqlConnection>();
            Console.WriteLine(connection.ConnectionString);
        }
    }
}

```

### sectionGroup/section

```c#

internal static void Create(string filePath)
{
    //
    // 使用 .NET 程序的默认配置文件或自定义配置文件。
    //
    using (ConfigurationManager manager = new ConfigurationManager(filePath))
    {
        //
        // 支持三种标准类型的创建。
        // System.Configuration.DictionarySectionHandler
        // System.Configuration.NameValueSectionHandler
        // System.Configuration.SingleTagSectionHandler
        //
        // 配置组。
        //
        ConfigSectionGroup group = manager.ConfigSections.Groups.GetOrAdd("sectionGroup1");
        group.Comment.SetValue("This is \"ConfigSectionGroup\" comment.");
        //
        // 配置容器。
        //
        SingleTagSection section1 = manager.ConfigSections.Sections.GetOrAdd<SingleTagSection>("section1");
        NameValueSection section2 = manager.ConfigSections.Sections.GetOrAdd<NameValueSection>("section2");
        DictionarySection section3 = group.Sections.GetOrAdd<DictionarySection>("section3");
        //
        // SingleTagSection 使用唯一键值。不支持属性值注释。
        //
        section1.Properties.AddOrUpdate("prop1", new SingleTagProperty("0.6789"));
        section1.Comment.SetValue("This is \"SingleTagSection\" comment.");
        section1.Properties.Remove("prop3");
        section1.Properties.Add("prop3", new SingleTagProperty("Update this."));
        section1.Properties.AddOrUpdate("prop2", new SingleTagProperty("abc"));
        //section1.Properties.Add("prop1", new SingleTagProperty("Test unique."));
        //
        // 更新。
        //
        section1.Properties.AddOrUpdate("prop3", new SingleTagProperty("Update this successful."));
        //
        // NameValueSection 允许同名键值。
        //
        section2.Properties.Clear();
        section2.Properties.Add("prop1", new AddProperty("155.66")).Comment.SetValue("This is \"NameValueSection\" prop1 comment.");
        section2.Properties.Add("prop1", new AddProperty("7.9992")).Comment.SetValue("This is \"NameValueSection\" prop1 comment.");
        section2.Comment.SetValue("This is \"NameValueSection\" comment.");
        //
        // DictionarySection 使用唯一键值。
        //
        section3.Properties.AddOrUpdate("prop1", new AddProperty("DictionarySection prop.")).Comment.SetValue("This is \"DictionarySection\" prop1 comment.");
        section3.Comment.SetValue("This is \"DictionarySection\" comment.");

        //
        // 以文本方式创建。
        //
        TextSection section4 = manager.ConfigSections.Sections.GetOrAdd<TextSection>("section4");
        section4.SetAttribute("attr1", "attr1value");
        section4.SetValue("<!-- Comment --><arbitrarily>abc</arbitrarily><arbitrarily>def</arbitrarily>");
        section4.Comment.SetValue("This is \"TextSection\" comment.");

        TextSection section5 = manager.ConfigSections.Sections.GetOrAdd<TextSection>("section5");
        section5.SetValue("<![CDATA[<arbitrarily>abc</arbitrarily><arbitrarily>def</arbitrarily>]]>");

        TextSection section6 = manager.ConfigSections.Sections.GetOrAdd<TextSection>("section6");
        section6.SetValue("abcdefg");
        //
        //
        //
        NameValueSection section7 = manager.ConfigSections.Sections.GetOrAdd<NameValueSection>("section7");
        section7.Comment.SetValue("It's will remove this.");
        manager.ConfigSections.Sections.Remove("section7");
        //
        // 保存到指定的文件。
        //
        manager.Save(filePath);
    }
}

internal static void Load(string filePath)
{
    //
    // 使用 .NET 程序的默认配置文件或自定义配置文件。
    //
    using (ConfigurationManager manager = new ConfigurationManager(filePath))
    {
        //
        // 取出容器。
        //
        ConfigSectionGroup group = manager.ConfigSections.Groups.GetOrAdd("sectionGroup1");
        SingleTagSection section1 = manager.ConfigSections.Sections.GetOrAdd<SingleTagSection>("section1");
        NameValueSection section2 = manager.ConfigSections.Sections.GetOrAdd<NameValueSection>("section2");
        DictionarySection section3 = group.Sections.GetOrAdd<DictionarySection>("section3");
        TextSection section4 = manager.ConfigSections.Sections.GetOrAdd<TextSection>("section4");
        TextSection section5 = manager.ConfigSections.Sections.GetOrAdd<TextSection>("section5");
        TextSection section6 = manager.ConfigSections.Sections.GetOrAdd<TextSection>("section6");
        //
        // 取出属性和注释。
        //
        Console.WriteLine(section1.Properties.GetValue("prop1"));
        //
        AddProperty[] value2 = section2.Properties.GetValue("prop1");
        foreach (AddProperty val in value2)
        {
            Console.WriteLine(val.Value);
        }
        Console.WriteLine(section3.Properties.GetValue("prop1", new AddProperty(string.Empty)));
        //
        // 以文本方式取出节点内容。
        //
        Console.WriteLine(section4.GetValue());
        Console.WriteLine(section5.GetValue());
        Console.WriteLine(section6.GetValue());
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

### Protection

提供了一个额外的加密方式加密整个配置文件。这和 ASP.NET 的默认加密方式无关，生成的加密配置文件仅可使用此项目工具读写。

```c#

internal static void Create()
{
    RSA rsa = RSA.Create();
    rsa.FromXmlString(keyStored);
    //
    // 读取加密配置文件。加密方式和 ASP.NET 加密不兼容。
    //
    using (XConfigManager manager = new XConfigManager(filePath))
    {
        //
        // 配置容器加解密。
        //
        manager.Default.Encrypt(rsa);
        manager.Default.Decrypt(rsa);
        XSection section1 = manager.Sections.GetOrAdd("section1");
        section1.Encrypt(rsa);
        section1.Decrypt(rsa);
    }
}

```

### XConfigManager

提供 XConfigManager 类读写一个精简的配置属性文件，支持加密，支持字典/列表类型无限嵌套。

```c#

internal static void Create(string filePath)
{
    //
    // 使用自定义配置文件。
    //
    using (XConfigManager manager = File.Exists(filePath) ? new XConfigManager(filePath) : new XConfigManager())
    {
        //
        // 赋值并设置注释。
        //
        manager.Default.Properties.AddOrUpdate("prop1", new XString(StringComparison.Ordinal.ToString())).Comment.SetValue("This is \"XCconfig\" prop1 comment.");
        manager.Default.Properties.AddOrUpdate("prop7", new XString("Update this."));
        var prop2 = manager.Default.Properties.AddOrUpdate("prop2", new XDictionary());
        prop2.Properties.AddOrUpdate("prop4", new XString("Sub this dictionary prop."));
        var prop3 = manager.Default.Properties.AddOrUpdate("prop3", new XList());
        prop3.Properties.Add(new XString("Sub this list prop.")).Comment.SetValue("This is \"XCconfig\" list prop comment.");
        XDictionary prop5 = prop3.Properties.Add(new XDictionary());
        prop5.Properties.Add("prop5", new XString("F024AC4"));
        //
        // 附加属性。
        //
        prop5.Attributes.AddOrUpdate("attr1", new XConfigAttribute("add attr"));
        prop55.Attributes.AddOrUpdate("attr1", new XConfigAttribute("add attr"));
        //
        manager.Default.Properties.AddOrUpdate("prop6", new XString("Remove this."));
        // manager.Default.Properties.Add("prop1", new XString("Test unique."));
        //
        // 移除属性的方法。移除属性时相关注释一并移除。
        //
        manager.Default.Properties.Remove("prop6");
        //
        // 更新。
        //
        manager.Default.Properties.AddOrUpdate("prop7", new XString("Update this successful."));
        //
        // 附加配置容器。
        //
        XDictionary section = manager.Sections.GetOrAdd("section1");
        section.Comment.SetValue("This is \"XCconfig\" section1");
        section.Properties.AddOrUpdate("prop1", new XString("123456789"));
        //
        // 保存到指定的文件。
        //
        manager.Save(filePath);
    }
}

```

```c#

internal static void Load(string filePath)
{
    //
    // 使用自定义配置文件。
    //
    using (XConfigManager manager = new XConfigManager(filePath))
    {
        //
        // 取出属性和注释。
        //
        XString value1 = manager.Default.Properties.GetValue<XString>("prop1");
        if (value1.Comment.TryGetValue(out string comment1))
        {
            Console.WriteLine(comment1);
        }
        Console.WriteLine(value1.GetEnumValue<StringComparison>());
        //
        XDictionary value2 = manager.Default.Properties.GetValue<XDictionary>("prop2");
        value2.Properties.TryGetValue("prop4", out XString val2);
        Console.WriteLine(val2.GetStringValue());
        //
        XList value3 = manager.Default.Properties.GetValue<XList>("prop3");
        Console.WriteLine(value3.Properties.GetValue<XString>(0).GetStringValue());
        //
        XString value5 = ((XDictionary)value3.Properties[1]).Properties.GetValue<XString>("prop5");
        byte[] val5 = value5.GetBytesValue();
        Console.WriteLine(BitConverter.ToString(val5));
        Console.WriteLine(value5.Attributes.GetValue("attr1").GetStringValue());
        //
        XDictionary section = manager.Sections.GetValue("section1");
        Console.WriteLine(section.Comment.GetValue());
        Console.WriteLine(section.Properties.GetValue<XString>("prop1").GetInt64Value());
    }
}

```

## CHANGELOG

[CHANGELOG](CHANGELOG.md)

## LICENSE

[MIT](LICENSE) license.
