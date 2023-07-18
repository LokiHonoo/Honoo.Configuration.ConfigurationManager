# Honoo.Configuration.ConfigurationManager

<!-- @import "[TOC]" {cmd="toc" depthFrom=1 depthTo=6 orderedList=false} -->

<!-- code_chunk_output -->

- [Honoo.Configuration.ConfigurationManager](#honooconfigurationconfigurationmanager)
  - [INTRODUCTION](#introduction)
  - [Project](#project)
    - [NuGet](#nuget)
  - [CHANGELOG](#changelog)
    - [1.3.2](#132)
    - [1.3.1](#131)
    - [1.2.5](#125)
    - [1.2.3](#123)
    - [1.2.0](#120)
  - [USAGE](#usage)
    - [Namespace](#namespace)
    - [appSettings](#appsettings)
    - [connectionStrings](#connectionstrings)
    - [sectionGroup/section](#sectiongroupsection)
    - [Auto save](#auto-save)
    - [Protection](#protection)
    - [UWP](#uwp)
  - [LICENSE](#license)

<!-- /code_chunk_output -->

## INTRODUCTION

此项目是 System.Configuration.ConfigurationManager 的简单替代。

用于 .NET Framework 4.0+/.NET Standard 2.0+ 中读写默认配置文件或自定义配置文件。

提供对标准节点 appSettings、connectionStrings、configSections 节点的有限读写支持。

提供了一个额外的加密方式加密整个配置文件。这和 ASP.NET 的默认加密方式无关，生成的加密配置文件仅可使用此项目工具读写。

## Project

### NuGet

<https://www.nuget.org/packages/Honoo.Configuration.ConfigurationManager/>

## CHANGELOG

### 1.3.2

**Features* 提供了一个额外的加密方式加密整个配置文件。这和 ASP.NET 的默认加密方式无关，生成的加密配置文件仅可使用此项目工具读写。

**Features* 新增 &lt;assemblyBinding /&gt; 节点支持。

**Features* 读取 &lt;remove /&gt; &lt;clear /&gt; 标签时采用标签的默认行为。标签产生的删除行为会体现在配置文件中。例如 &lt;clear /&gt; 标签之前的 所有标签都会被移除。

### 1.3.1

**Features* AppSettings、DictionarySection 读取同名键值不再抛出异常，使用最后读取的值。

**Features* NameValueSection 支持数组模式。

**Features* 属性值支持枚举类型。

**Features* 新增 GetValue(string, string) 方法，在取值时设置没有找到指定键时的默认值。

**Changed* SectionGroup 和 Section 的注释（comment）设置从父级移动到自身。

**Changed* 移除了 ConfigSection 基类，更改为 IConfigSection 接口方式。

### 1.2.5

**Features* TextSection 以 xml 方式处理。现在 TextSection 支持解析 CDATA 区段。

### 1.2.3

**Changed* 移除了访问 ConnectionStrings 直接创建和访问实例的代码。提供 CreateInstance() 方法主动创建实例。如果没有引用相关的数据库程序集，在主动创建实例前不会抛出异常。

### 1.2.0

**Changed* 移除了原有的自动保存的代码，增加了事件，可在 OnChanged，OnDisposing 事件中实现自动保存。

**Features* 现在支持读写注释（comment）节点。

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
    using (ConfigurationManager manager = new ConfigurationManager(filePath))
    {
        //
        // 直接赋值必须转换为 string 类型。
        //
        manager.AppSettings.Properties["prop1"] = "It's will remove this.";
        manager.AppSettings.Properties.AddOrUpdate("prop2", "This is \"appSettings\" prop2 value.");
        manager.AppSettings.Properties.AddOrUpdate("prop3", 1234567);
        manager.AppSettings.Properties.AddOrUpdate("prop4", LoaderOptimization.SingleDomain);
        //
        // 设置注释。
        //
        manager.AppSettings.Properties.TrySetComment("prop1", "It's will remove this.");
        manager.AppSettings.Properties.TrySetComment("prop2", "This is \"appSettings\" prop2 comment.");
        manager.AppSettings.Properties.TrySetComment("prop3", "This is \"appSettings\" prop3 comment.");
        manager.AppSettings.Properties.TrySetComment("prop4", "This is \"appSettings\" prop4 comment.");
        //
        // 移除属性的方法。选择其一。移除属性时相关注释一并移除。
        //
        manager.AppSettings.Properties.Remove("prop1");
        manager.AppSettings.Properties["prop1"] = null;
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
        // 取出属性。
        //
        string value1 = manager.AppSettings.Properties.GetValue("prop1", "This is default value when prop1 not fount.");
        Console.WriteLine(value1);
        string value2 = manager.AppSettings.Properties.GetValue("prop2", "This is default value when prop2 not fount.");
        Console.WriteLine(value2);
        double value3 = manager.AppSettings.Properties.GetValue("prop3", 0.77777777d);
        Console.WriteLine(value3);
        LoaderOptimization value4 = manager.AppSettings.Properties.GetValue("prop4", LoaderOptimization.MultiDomainHost);
        Console.WriteLine(value4);
        //
        // 取出注释。
        //
        if (manager.AppSettings.Properties.TryGetComment("prop1", out string comment))
        {
            Console.WriteLine(comment);
        }
        if (manager.AppSettings.Properties.TryGetComment("prop2", out comment))
        {
            Console.WriteLine(comment);
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
        SqlConnectionStringBuilder builder1 = new SqlConnectionStringBuilder()
        {
            DataSource = "127.0.0.1",
            InitialCatalog = "DemoCatalog",
            UserID = "sa",
            Password = "12345"
        };
        SqlConnection conn1 = new SqlConnection(builder1.ConnectionString);
        MySqlConnectionStringBuilder builder2 = new MySqlConnectionStringBuilder()
        {
            Server = "127.0.0.1",
            Database = "DemoDB",
            UserID = "root",
            Password = "12345"
        };
        MySqlConnection conn2 = new MySqlConnection(builder2.ConnectionString);
        //
        // 如果不设置引擎参数，读取时不能访问连接实例。
        //
        manager.ConnectionStrings.Properties["prop1"] = new ConnectionStringsValue(conn1);
        manager.ConnectionStrings.Properties["prop2"] = new ConnectionStringsValue(conn1.ConnectionString, null);
        manager.ConnectionStrings.Properties.AddOrUpdate("prop3", conn1);
        manager.ConnectionStrings.Properties.AddOrUpdate("prop4", conn2.ConnectionString, conn2.GetType().Namespace);
        manager.ConnectionStrings.Properties.AddOrUpdate("prop5", conn2.ConnectionString, typeof(MySqlConnection).AssemblyQualifiedName);
        //
        // 设置注释。
        //
        manager.ConnectionStrings.Properties.TrySetComment("prop1", "It's will remove this.");
        manager.ConnectionStrings.Properties.TrySetComment("prop2", "This is \"sql server connection\" comment without provider name.");
        manager.ConnectionStrings.Properties.TrySetComment("prop3", "This is \"sql server connection\" comment.");
        manager.ConnectionStrings.Properties.TrySetComment("prop4", "This is \"mysql connection\" comment with assembly namespace.");
        manager.ConnectionStrings.Properties.TrySetComment("prop5", "This is \"mysql connection\" comment with assembly qualified name.");
        //
        // 移除属性的方法。选择其一。移除属性时相关注释一并移除。
        //
        manager.ConnectionStrings.Properties.Remove("prop1");
        manager.ConnectionStrings.Properties["prop1"] = null;
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
        // 取出属性。
        //
        if (manager.ConnectionStrings.Properties.TryGetValue("prop2", out ConnectionStringsValue value))
        {
            Console.WriteLine(value.ConnectionString);
        }
        string connectionString = manager.ConnectionStrings.Properties["prop3"].ConnectionString;
        Console.WriteLine(connectionString);
        //
        // 访问实例。
        //
        DbConnection connection = manager.ConnectionStrings.Properties["prop4"].CreateInstance();
        Console.WriteLine(connection.ConnectionString);
        //
        MySqlConnection mysql = (MySqlConnection)manager.ConnectionStrings.Properties["prop5"].CreateInstance();
        Console.WriteLine(mysql.ConnectionString);
        //
        // 取出注释。
        //
        if (manager.AppSettings.Properties.TryGetComment("prop1", out string comment))
        {
            Console.WriteLine(comment);
        }
        if (manager.AppSettings.Properties.TryGetComment("prop2", out comment))
        {
            Console.WriteLine(comment);
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
        group.SetComment("This is \"ConfigSectionGroup\" comment.");
        //
        // 配置容器。
        //
        SingleTagSection section1 = (SingleTagSection)manager.ConfigSections.Sections.GetOrAdd("section1", ConfigSectionKind.SingleTagSection);
        NameValueSection section2 = (NameValueSection)manager.ConfigSections.Sections.GetOrAdd("section2", ConfigSectionKind.NameValueSection);
        DictionarySection section3 = (DictionarySection)group.Sections.GetOrAdd("section3", ConfigSectionKind.DictionarySection);
        //
        // SingleTagSection 属性操作与 appSettings 节点相同。不支持属性值注释。
        //
        section1.Properties["prop1"] = 0.6789d.ToString();
        section1.SetComment("This is \"SingleTagSection\" comment.");
        //section1.SetComment("prop1", "This is \"SingleTagSection\" prop1  comment.");
        //
        // DictionarySection 属性操作和注释操作与 appSettings 节点相同。
        //
        section3.Properties.AddOrUpdate("prop1", new byte[] { 0x01, 0x02, 0x03, 0xAA, 0xBB, 0xCC, });
        section3.Properties.TrySetComment("prop1", "This is \"DictionarySection\" prop1 comment.");
        section3.SetComment("This is \"DictionarySection\" comment.");
        //
        // NameValueSection 属性以数组操作，注释需指定索引。
        //
        section2.Properties.AddOrUpdate("prop1", new double[] { 155.66d, 7.9992d });
        section2.Properties.TrySetComment("prop1", 0, "This is \"NameValueSection\" prop1 sub 0 comment.");
        section2.Properties.TrySetComment("prop1", 1, "This is \"NameValueSection\" prop1 sub 1 comment.");
        section2.SetComment("This is \"NameValueSection\" comment.");
        //
        // 以文本方式创建。
        //
        TextSection section4 = (TextSection)manager.ConfigSections.Sections.GetOrAdd("section4", ConfigSectionKind.TextSection);
        section4.SetAttribute("attr1", "attr1value");
        section4.SetValue("<!-- Comment --><arbitrarily>abc</arbitrarily><arbitrarily>def</arbitrarily>");
        section4.SetComment("This is \"TextSection\" comment.");

        TextSection section5 = (TextSection)manager.ConfigSections.Sections.GetOrAdd("section5", ConfigSectionKind.TextSection);
        section5.SetValue("<![CDATA[<arbitrarily>abc</arbitrarily><arbitrarily>def</arbitrarily>]]>");

        TextSection section6 = (TextSection)manager.ConfigSections.Sections.GetOrAdd("section6", ConfigSectionKind.TextSection);
        section6.SetValue("abcdefg");
        //
        //
        //
        NameValueSection section7 = (NameValueSection)manager.ConfigSections.Sections.GetOrAdd("section7", ConfigSectionKind.NameValueSection);
        section7.SetComment("It's will remove this.");
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
        SingleTagSection section1 = (SingleTagSection)manager.ConfigSections.Sections.GetOrAdd("section1", ConfigSectionKind.SingleTagSection);
        NameValueSection section2 = (NameValueSection)manager.ConfigSections.Sections.GetOrAdd("section2", ConfigSectionKind.NameValueSection);
        DictionarySection section3 = (DictionarySection)group.Sections.GetOrAdd("section3", ConfigSectionKind.DictionarySection);
        TextSection section4 = (TextSection)manager.ConfigSections.Sections.GetOrAdd("section4", ConfigSectionKind.TextSection);
        TextSection section5 = (TextSection)manager.ConfigSections.Sections.GetOrAdd("section5", ConfigSectionKind.TextSection);
        TextSection section6 = (TextSection)manager.ConfigSections.Sections.GetOrAdd("section6", ConfigSectionKind.TextSection);
        //
        // 取出属性。
        //
        Console.WriteLine(section1.Properties.GetValue("prop1", string.Empty));
        Console.WriteLine(section3.Properties.GetValue("prop1", string.Empty));
        double[] value2 = section2.Properties.GetValues("prop1", new double[] { 11, 22, 33 });
        foreach (double v in value2)
        {
            Console.WriteLine(v);
        }
        //
        // 取出注释。
        //
        if (section1.TryGetComment(out string comment))
        {
            Console.WriteLine(comment);
        }
        if (section2.Properties.TryGetComment("prop1", 0, out comment))
        {
            Console.WriteLine(comment);
        }
        if (section3.Properties.TryGetComment("prop1", out comment))
        {
            Console.WriteLine(comment);
        }
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
        manager.OnChanged += Manager_OnChanged;
        manager.OnDisposing += Manager_OnDisposing;
    }
}

private static void Manager_OnChanged(ConfigurationManager manager)
{
    manager.Save(filePath);
}

private static void Manager_OnDisposing(ConfigurationManager manager)
{
    manager.Save(filePath);
}

```

### Protection

提供了一个额外的加密方式加密整个配置文件。这和 ASP.NET 的默认加密方式无关，生成的加密配置文件仅可使用此项目工具读写。

```c#

internal static void Create(string filePath)
{
    RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
    rsa.FromXmlString(keyStored);
    //
    // 读取加密配置文件。加密配置文件和 .NET 程序的默认配置文件不兼容。
    //
    using (ConfigurationManager manager = new ConfigurationManager(filePath, rsa))
    {
        //
        // 加密方式保存到指定的文件。
        //
        manager.Save(filePath, rsa);
    }
}

```

### UWP

```c#

public static async void Test()
{
    using (var read = await storageFile.OpenStreamForReadAsync())
    {
        using (ConfigurationManager manager = new ConfigurationManager(read))
        {
            using (var write = await storageFile.OpenStreamForWriteAsync())
            {
                manager.Save(write);
            }
        }
    }
}

```

## LICENSE

This project based on [MIT](LICENSE) license.
