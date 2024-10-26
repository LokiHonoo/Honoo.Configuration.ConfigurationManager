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
    - [XConfigManager](#XConfigManager)
    - [UWP](#uwp)
  - [CHANGELOG](#changelog)
    - [1.5.4](#154)
    - [1.4.19-final](#1419-final)
    - [1.4.18](#1418)
    - [1.4.17](#1417)
    - [1.4.16](#1416)
    - [1.4.11](#1411)
    - [1.4.10](#1410)
    - [1.3.4](#134)
    - [1.3.2](#132)
    - [1.3.1](#131)
    - [1.2.5](#125)
    - [1.2.3](#123)
    - [1.2.0](#120)
  - [LICENSE](#license)

<!-- /code_chunk_output -->

## INTRODUCTION

此项目是 System.Configuration.ConfigurationManager 的简单替代。

用于 .NET Framework 4.0+/.NET Standard 2.0+ 中读写默认配置文件或自定义配置文件。

提供对标准节点 appSettings、connectionStrings、configSections assemblyBinding/linkedConfiguration 节点的有限读写支持。

提供了一个额外的加密方式加密整个配置文件。这和 ASP.NET 的默认加密方式无关，生成的加密配置文件仅可使用此项目工具读写。

提供 XConfigManager 类读写一个精简的配置属性文件，支持加密，支持字典/列表类型无限嵌套。

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
        manager.AppSettings.Properties.AddOrUpdate("prop1", "This is \"appSettings\" prop1 value.").Comment.SetValue("This is \"appSettings\" prop1 comment.");
        manager.AppSettings.Properties.AddOrUpdate("prop6", "Update this.");
        manager.AppSettings.Properties.AddOrUpdate("prop2", 123456789);
        manager.AppSettings.Properties.AddOrUpdate("prop3", new Binaries(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }));
        manager.AppSettings.Properties.AddOrUpdate("prop4", LoaderOptimization.SingleDomain);
        manager.AppSettings.Properties.AddOrUpdate("prop5", "Remove this.");
        //manager.AppSettings.Properties.Add("prop1", "Test unique.");
        //
        // 移除属性的方法。移除属性时相关注释一并移除。
        //
        manager.AppSettings.Properties.Remove("prop5");
        //
        // 更新。
        //
        manager.AppSettings.Properties.AddOrUpdate("prop6", "Update this successful.");
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
        section1.Properties.AddOrUpdate("prop1", 0.6789d);
        section1.Comment.SetValue("This is \"SingleTagSection\" comment.");
        section1.Properties.Remove("prop3");
        section1.Properties.Add("prop3", "Update this.");
        section1.Properties.AddOrUpdate("prop2", "abc");
        //section1.Properties.Add("prop1", "Test unique.");
        //
        // 更新。
        //
        section1.Properties.AddOrUpdate("prop3", "Update this successful.");
        //
        // NameValueSection 允许同名键值。
        //
        section2.Properties.Clear();
        section2.Properties.Add("prop1", 155.66d).Comment.SetValue("This is \"NameValueSection\" prop1 comment.");
        section2.Properties.Add("prop1", 7.9992d).Comment.SetValue("This is \"NameValueSection\" prop1 comment.");
        section2.Comment.SetValue("This is \"NameValueSection\" comment.");
        //
        // DictionarySection 使用唯一键值。
        //
        section3.Properties.AddOrUpdate("prop1", "DictionarySection prop.").Comment.SetValue("This is \"DictionarySection\" prop1 comment.");
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
        manager.Default.Properties.AddOrUpdate("prop1", StringComparison.Ordinal.ToString()).Comment.SetValue("This is \"hoonoo-settings\" prop1 comment.");
        manager.Default.Properties.AddOrUpdate("prop7", "Update this.");
        var prop2 = manager.Default.Properties.AddOrUpdate("prop2", new XDictionary());
        prop2.Properties.AddOrUpdate("prop4", "Sub this dictionary prop.");
        var prop3 = manager.Default.Properties.AddOrUpdate("prop3", new XList());
        prop3.Properties.Add(new XString("Sub this list prop.")).Comment.SetValue("This is \"hoonoo-settings\" list prop comment."); ;
        XDictionary prop5 = prop3.Properties.Add(new XDictionary());
        prop5.Properties.Add("prop5", new XString("F024AC4"));
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
        section.Comment.SetValue("This is \"hoonoo-settings\" section1");
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
        Console.WriteLine(value3.Properties.GetValue(0).GetStringValue());
        //
        XString value5 = ((XDictionary)value3.Properties[1]).Properties.GetValue<XString>("prop5");
        byte[] val5 = value5.GetBytesValue();
        Console.WriteLine(BitConverter.ToString(val5));
        //
        XDictionary section = manager.Sections.GetValue("section1");
        Console.WriteLine(section.Comment.GetValue());
        Console.WriteLine(section.Properties.GetValue<XString>("prop1").GetInt64Value());
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

## CHANGELOG

### 1.5.4

**Features* 提供针对字符串的方法的重载。
**Features* 重写 XConfigManager。取消数组支持，更改为类型嵌套。支持 Dictionary、List 类型无限嵌套。
**Fix* 修复 BUG。

### 1.4.19-final

**Features* 添加方法。

### 1.4.18

**Fix* 修复 BUG。

### 1.4.17

**Features* 注释操作提取到单独的 XConfigComment 类中。

### 1.4.16

**Features* 新增 Binaries 类型用于封装 byte[]。避免过多的 [] 符号造成的视觉混乱。

### 1.4.11

**Features* XConfigManager 增加 2 维数组和 3 维数组支持。

### 1.4.10

**Refactored* 完全重构。现在能以标签方式处理混乱的 &lt;remove /&gt; &lt;clear /&gt; 标签。所有属性封装为类型并将注释（comment）的设置移动到属性封装中。

**Features* 提供 AppSettings.GetControlledProperties() 用于获取应用 file 属性以及 &lt;remove /&gt;、&lt;clear /&gt; 标签后的只读配置属性集合。

**Features* 提供 DictionarySection.GetControlledProperties() 用于获取应用 file 属性以及 &lt;remove /&gt;、&lt;clear /&gt; 标签后的只读配置属性集合。

**Features* 提供 NameValueSection.GetControlledProperties() 用于获取应用 &lt;remove /&gt;、&lt;clear /&gt; 标签后的只读配置属性集合。

**Features* 提供 AppSettingsManager.GetControlledProperties() 用于获取应用 file 属性以及 &lt;remove /&gt;、&lt;clear /&gt; 标签后的只读配置属性集合。

**Features* 提供 AppSettingsManager 用于读写 appSettings 节点的 file 属性指定的附加配置文件。

**Features* 提供一个额外的精简的配置属性文件，以字典类型保存，支持分组，支持加密，支持单一属性值和数组属性值。使用 XConfigManager 类读写此文件。

### 1.3.4

**Fixed* 修复了 AssemblyBinding 节点缺少命名空间的问题。

**Fixed* 修复了 Section 标签类型错误的问题。

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

## LICENSE

[MIT](LICENSE) license.
