# Honoo.Configuration.ConfigurationManager

<!-- @import "[TOC]" {cmd="toc" depthFrom=1 depthTo=6 orderedList=false} -->

<!-- code_chunk_output -->

- [Honoo.Configuration.ConfigurationManager](#honooconfigurationconfigurationmanager)
  - [INTRODUCTION](#introduction)
  - [GUIDE](#guide)
    - [GitHub](#github)
  - [LICENSE](#license)

<!-- /code_chunk_output -->

## INTRODUCTION

The project is a replacement for "System.Configuration.ConfigurationManager".
Used to read/write default profiles or custom profiles in the .NET Framework 4.0+/.NET Standard 2.0+.

Read/write support for appSettings, connectionStrings, configSections, assemblyBinding/linkedConfiguration nodes.

Provides an method to encrypt the configuration file (not a ASP.NET encryption). 

Waring: The encryption method is different from ASP.NET, and the generated encryption file can only using by this project tool.

Provides another "XSettingsManager" class to read/write a concise configuration file, encryption supported, dictionary/list nesting supported.

## GUIDE

### GitHub

<https://github.com/LokiHonoo/Honoo.Configuration.ConfigurationManager>

## LICENSE

[MIT](LICENSE) license.
