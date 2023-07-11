# Honoo.Configuration.ConfigurationManager

<!-- @import "[TOC]" {cmd="toc" depthFrom=1 depthTo=6 orderedList=false} -->

<!-- code_chunk_output -->

- [Honoo.Configuration.ConfigurationManager](#honooconfigurationconfigurationmanager)
  - [INTRODUCTION](#introduction)
  - [CHANGELOG](#changelog)
    - [1.3.0](#130)
    - [1.2.5](#125)
    - [1.2.3](#123)
    - [1.2.2](#122)
    - [1.2.1](#121)
    - [1.2.0](#120)
  - [USAGE](#usage)
    - [NuGet](#nuget)
    - [Namespace](#namespace)
    - [appSettings](#appsettings)
    - [connectionStrings](#connectionstrings)
    - [sectionGroup/section](#sectiongroupsection)
    - [Auto save](#auto-save)
    - [UWP](#uwp)
  - [LICENSE](#license)

<!-- /code_chunk_output -->

## INTRODUCTION

此项目是 System.Configuration.ConfigurationManager 的简单替代。

用于 .NET Framework 4.0+/.NET Standard 2.0+ 中读写默认配置文件或自定义配置文件。

提供对标准节点 appSettings、connectionStrings、configSections 节点的有限读写支持。

## CHANGELOG

### 1.3.0

**Changed* 移除了 ConfigSection 基类，更改为 IConfigSection 接口方式。

**Features* 新增 GetValue(string, string) 方法，在取值时设置没有找到指定键时的默认值。

### 1.2.5

**Features* TextSection 以 xml 方式处理。现在 TextSection 支持解析 CDATA 区段。

### 1.2.3

**Changed* 移除了访问 ConnectionStrings 直接创建和访问实例的代码。提供 CreateInstance() 方法主动调用。

### 1.2.2

**Fixed* 实例释放后仍可访问缓存后的节点的问题。

**Changed* ConfigSectionType 更名为 ConfigSectionKind。

### 1.2.1

**Fixed* 创建 TextSection 时遗漏了 type 属性。

**Fixed* 读取 DictionarySection 时遗漏了 type 属性。

### 1.2.0

**Changed* 移除了原有的自动保存的代码，现在在事件中实现自动保存。

**Features* 现在支持读写注释（comment）节点。

## USAGE

### NuGet

<https://www.nuget.org/packages/Honoo.Configuration.ConfigurationManager/>

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
        // 直接赋值等同于 AddOrUpdate 方法。
        //
        manager.AppSettings.Properties["prop1"] = Common.Random.NextDouble().ToString();
        manager.AppSettings.Properties.AddOrUpdate("prop2", Common.Random.NextDouble().ToString());
        manager.AppSettings.Properties.AddOrUpdate("prop3", Common.Random.NextDouble().ToString());
        //
        // 设置注释。
        //
        manager.AppSettings.Properties.TrySetComment("prop1", string.Empty);
        manager.AppSettings.Properties.TrySetComment("prop2", "This is \"appSettings\" prop2 comment");
        manager.AppSettings.Properties.TrySetComment("prop3", null);
        //
        // 移除属性的方法。选择其一。移除属性时相关注释一并移除。
        //
        manager.AppSettings.Properties.Remove("prop1");
        manager.AppSettings.Properties["prop1"] = null;
        manager.AppSettings.Properties.AddOrUpdate("prop1", null);
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
        if (manager.AppSettings.Properties.TryGetValue("prop2", out string value))
        {
            Console.WriteLine(value);
        }
        value = manager.AppSettings.Properties["prop3"];
        Console.WriteLine(value);
        value = manager.AppSettings.Properties.GetValue("prop1", "prop1 not fount default value.");
        Console.WriteLine(value);
        //
        // 取出注释。
        //
        if (manager.AppSettings.Properties.TryGetComment("prop2", out string comment))
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
        // 直接赋值等同于 AddOrUpdate 方法。不设置引擎参数，读取时不能访问连接实例。
        //
        manager.ConnectionStrings.Properties["prop1"] = new ConnectionStringsValue(conn1.ConnectionString, null);
        manager.ConnectionStrings.Properties["prop2"] = new ConnectionStringsValue(conn1);
        manager.ConnectionStrings.Properties.AddOrUpdate("prop3", conn1);
        manager.ConnectionStrings.Properties.AddOrUpdate("prop4", conn2.ConnectionString, conn2.GetType().Namespace);
        manager.ConnectionStrings.Properties.AddOrUpdate("prop5", conn2.ConnectionString, typeof(MySqlConnection).AssemblyQualifiedName);
        //
        // 设置注释。
        //
        manager.ConnectionStrings.Properties.TrySetComment("prop1", string.Empty);
        manager.ConnectionStrings.Properties.TrySetComment("prop2", "This is \"sql server connection\" comment");
        manager.ConnectionStrings.Properties.TrySetComment("prop3", null);
        manager.ConnectionStrings.Properties.TrySetComment("prop4", "This is \"mysql connection\" comment ");
        manager.ConnectionStrings.Properties.TrySetComment("prop5", "This is \"mysql connection\" comment used assembly qualified name");
        //
        // 移除属性的方法。选择其一。移除属性时相关注释一并移除。
        //
        manager.ConnectionStrings.Properties.Remove("prop1");
        manager.ConnectionStrings.Properties["prop1"] = null;
        manager.ConnectionStrings.Properties.AddOrUpdate("prop1", (DbConnection)null);
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

        MySqlConnection mysql = (MySqlConnection)manager.ConnectionStrings.Properties["prop5"].CreateInstance();
        Console.WriteLine(mysql.ConnectionString);
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
        // 直接赋值等同于 AddOrUpdate 方法。
        //
        SingleTagSection section1 = (SingleTagSection)manager.ConfigSections.Sections.GetOrAdd("section1", ConfigSectionKind.SingleTagSection);
        section1.Properties.AddOrUpdate("prop1", Common.Random.NextDouble().ToString());
        section1.Properties["prop2"] = Common.Random.NextDouble().ToString();
        NameValueSection section2 = (NameValueSection)manager.ConfigSections.Sections.GetOrAdd("section2", ConfigSectionKind.NameValueSection);
        section2.Properties.AddOrUpdate("prop1", Common.Random.NextDouble().ToString());
        section2.Properties["prop2"] = Common.Random.NextDouble().ToString();
        section2.Properties.TrySetComment("prop1", "This is a name value section child");
        section2.Properties.TrySetComment("prop2", "This is a name value section child");
        //
        // 配置组和注释。
        //
        ConfigSectionGroup group = manager.ConfigSections.Groups.GetOrAdd("sectionGroup1");
        manager.ConfigSections.Groups.TrySetComment("sectionGroup1", "This is a section group");
        //
        // 配置容器和注释。
        //
        DictionarySection section3 = (DictionarySection)group.Sections.GetOrAdd("section3", ConfigSectionKind.DictionarySection);
        group.Sections.TrySetComment("section3", "This is a dictionary section");
        //
        section3.Properties.AddOrUpdate("prop1", true);
        section3.Properties.AddOrUpdate("prop2", sbyte.MaxValue);
        section3.Properties.AddOrUpdate("prop3", byte.MaxValue);
        section3.Properties.AddOrUpdate("prop4", short.MaxValue);
        section3.Properties.AddOrUpdate("prop5", ushort.MaxValue);
        section3.Properties.AddOrUpdate("prop6", int.MaxValue);
        section3.Properties.AddOrUpdate("prop7", uint.MaxValue);
        section3.Properties["prop8"] = long.MaxValue;
        section3.Properties["prop9"] = ulong.MaxValue;
        section3.Properties["prop10"] = float.MaxValue / 2;
        section3.Properties["prop11"] = double.MaxValue / 2;
        section3.Properties["prop12"] = decimal.MaxValue;
        section3.Properties["prop13"] = (char)Common.Random.Next(65, 91);
        section3.Properties["prop14"] = new byte[] { 0x01, 0x02, 0x03, 0x0A, 0x0B, 0x0C };
        section3.Properties["prop15"] = "支持 15 种可序列化类型";
        section3.Properties.TrySetComment("prop15", "This is a dictionary section child");
        //
        // 以文本方式创建。
        //
        TextSection section4 = (TextSection)manager.ConfigSections.Sections.GetOrAdd("section4", ConfigSectionKind.TextSection);
        section4.SetAttribute("attr1", "attr1value");
        section4.SetValue("<!-- Comment --><arbitrarily>abc</arbitrarily><arbitrarily>def</arbitrarily>");
        manager.ConfigSections.Sections.TrySetComment("section4", "This is a text section");

        TextSection section5 = (TextSection)manager.ConfigSections.Sections.GetOrAdd("section5", ConfigSectionKind.TextSection);
        section5.SetValue("<![CDATA[<arbitrarily>abc</arbitrarily><arbitrarily>def</arbitrarily>]]>");

        TextSection section6 = (TextSection)manager.ConfigSections.Sections.GetOrAdd("section6", ConfigSectionKind.TextSection);
        section6.SetValue("abcdefg");
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
        if (manager.ConfigSections.Sections.TryGetValue("section1", out SingleTagSection section1))
        {
            foreach (KeyValuePair<string, string> prop in section1.Properties)
            {
                Console.WriteLine(prop.Value);
            }
        }
        if (manager.ConfigSections.Sections.TryGetValue("section2", out NameValueSection section2))
        {
            foreach (KeyValuePair<string, string> prop in section2.Properties)
            {
                Console.WriteLine(prop.Value);
            }
        }
        if (manager.ConfigSections.Groups.TryGetValue("sectionGroup1", out ConfigSectionGroup group))
        {
            if (group.Sections.TryGetValue("section3", out DictionarySection section3))
            {
                // 根据 type 参数返回强类型值。如果没有 type 参数，以 string 类型处理。
                foreach (KeyValuePair<string, object> prop in section3.Properties)
                {
                    Console.WriteLine($"{prop.Value.GetType().Name,-10}{prop.Value}");
                }
                Console.WriteLine(section3.Properties.GetValue("prop99", 12345678L));
            }
        }
        if (manager.ConfigSections.Sections.TryGetValue("section4", out TextSection section4))
        {
            Console.WriteLine(section4.GetValue());
        }
        if (manager.ConfigSections.Sections.TryGetValue("section5", out TextSection section5))
        {
            Console.WriteLine(section5.GetValue());
        }
        if (manager.ConfigSections.Sections.TryGetValue("section6", out TextSection section6))
        {
            Console.WriteLine(section6.GetValue());
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

### UWP

必须使用流方式。

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

MIT 协议。
