# Honoo.Configuration.ConfigurationManager

<!-- @import "[TOC]" {cmd="toc" depthFrom=1 depthTo=6 orderedList=false} -->

<!-- code_chunk_output -->

- [Honoo.Configuration.ConfigurationManager](#honooconfigurationconfigurationmanager)
  - [INTRODUCTION](#introduction)
  - [Project](#project)
    - [Github](#github)
    - [NuGet](#nuget)
  - [CHANGELOG](#changelog)
    - [1.4.0](#140)
    - [1.3.4](#134)
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

### Github

<https://github.com/LokiHonoo/Honoo.Configuration.ConfigurationManager>

### NuGet

<https://www.nuget.org/packages/Honoo.Configuration.ConfigurationManager/>

## CHANGELOG

### 1.4.0

**Refactored* 完全重构。现在能以文本方式处理混乱的 &lt;remove /&gt; &lt;clear /&gt; 标签。

### 1.3.4

**Fixed* 修复了 AssemblyBinding 节点缺少命名空间的问题。

**Changed* 移除了 IConfigSection 接口方式，更改为 ConfigSection 基类。

### 1.3.2

**Features* 提供了一个额外的加密方式加密整个配置文件。这和 ASP.NET 的默认加密方式无关，生成的加密配置文件仅可使用此项目工具读写。

**Features* 新增 &lt;assemblyBinding /&gt; 节点支持。

**Features* 读取 &lt;remove /&gt; &lt;clear /&gt; 标签时采用标签的默认行为。标签产生的删除行为会体现在配置文件中。例如 &lt;clear /&gt; 标签之前的 所有标签都会被移除。

### 1.3.1

**Changed* SectionGroup 和 Section 的注释（comment）设置从父级方法更改为自身方法。

**Changed* 移除了 ConfigSection 基类，更改为 IConfigSection 接口方式。

**Features* AppSettings、DictionarySection 读取同名键值不再抛出异常，使用最后读取的值。

**Features* NameValueSection 支持数组模式。

**Features* 属性值支持枚举类型。

**Features* 新增 GetValue(string, string) 方法，在取值时设置没有找到指定键时的默认值。

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
        // 赋值并设置注释。
        //
        manager.AppSettings.Properties.AddOrUpdate("prop1", "It's will remove this.").SetComment("It's will remove this.");
        manager.AppSettings.Properties.AddOrUpdate("prop2", "This is \"appSettings\" prop2 value.").SetComment("This is \"appSettings\" prop2 comment.");
        manager.AppSettings.Properties.AddOrUpdate("prop3", 1234567);
        manager.AppSettings.Properties.AddOrUpdate("prop4", LoaderOptimization.SingleDomain);
        //
        // 移除属性的方法。移除属性时相关注释一并移除。
        //
        manager.AppSettings.Properties.Remove("prop1");
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
        AddProperty value2 = manager.AppSettings.Properties.GetValue("prop2");
        if (value2.TryGetComment(out string comment2))
        {
            Console.WriteLine(comment2);
        }
        Console.WriteLine(value2.Value);
        //
        int value3 = manager.AppSettings.Properties.GetValue("prop3", 55555);
        Console.WriteLine(value3);
        //
        if (manager.AppSettings.Properties.TryGetValue("prop4", out LoaderOptimization value4))
        {
            Console.WriteLine(value4);
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
        // 赋值并设置注释。如果不设置引擎参数，读取时不能访问连接实例。
        //
        manager.ConnectionStrings.Properties.AddOrUpdate("prop1", conn1).SetComment("This is \"connectionStrings\" prop1 comment.");
        manager.ConnectionStrings.Properties.AddOrUpdate("prop2", conn1.ConnectionString, conn2.GetType().Namespace);
        manager.ConnectionStrings.Properties.AddOrUpdate("prop3", conn2.ConnectionString, typeof(MySqlConnection).AssemblyQualifiedName);
        manager.ConnectionStrings.Properties.AddOrUpdate("prop4", conn2).SetComment("It's will remove this.");
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
        if (value1.TryGetComment(out string comment1))
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
            DbConnection connection = value2.CreateInstance();
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
        group.SetComment("This is \"ConfigSectionGroup\" comment.");
        //
        // 配置容器。
        //
        SingleTagSection section1 = (SingleTagSection)manager.ConfigSections.Sections.GetOrAdd("section1", ConfigSectionKind.SingleTagSection);
        NameValueSection section2 = (NameValueSection)manager.ConfigSections.Sections.GetOrAdd("section2", ConfigSectionKind.NameValueSection);
        DictionarySection section3 = (DictionarySection)group.Sections.GetOrAdd("section3", ConfigSectionKind.DictionarySection);
        //
        // SingleTagSection 使用唯一键值。不支持属性值注释。
        //
        section1.Properties.AddOrUpdate("prop1", 0.6789d);
        section1.SetComment("This is \"SingleTagSection\" comment.");
        //
        // NameValueSection 允许同名键值。
        //
        section2.Properties.Add("prop1", 155.66d).SetComment("This is \"NameValueSection\" prop1 comment.");
        section2.Properties.Add("prop1", 7.9992d).SetComment("This is \"NameValueSection\" prop1 comment.");
        section2.SetComment("This is \"NameValueSection\" comment.");
        //
        // DictionarySection 使用唯一键值。
        //
        section3.Properties.AddOrUpdate("prop1", "DictionarySection prop.").SetComment("This is \"DictionarySection\" prop1 comment.");
        section3.SetComment("This is \"DictionarySection\" comment.");

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
        // 取出属性和注释。
        //
        Console.WriteLine(section1.Properties.GetValue("prop1", 0d));
        //
        AddProperty[] value2 = section2.Properties.GetValue("prop1");
        foreach (AddProperty val in value2)
        {
            Console.WriteLine(val.Value);
        }
        Console.WriteLine(section3.Properties.GetValue("prop1", string.Empty));
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
