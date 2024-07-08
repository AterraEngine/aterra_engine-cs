﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.ConfigFiles.PluginConfig;
using AterraCore.Contracts.Boot.FlexiPlug;
using System.Reflection;
using Xml.Elements;

namespace AterraCore.Contracts.FlexiPlug;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IPluginLoader {
    public LinkedList<IPreLoadedPluginDto> Plugins { get; }
    public HashSet<string> Checksums { get; }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IPluginLoader IterateOverValid(Action<IPluginLoader, IPreLoadedPluginDto> action);
    public IPluginLoader IterateOverValidWithZipImporter(params Action<IPluginLoader, IPreLoadedPluginDto, IPluginZipImporter<PluginConfigXml>>[] actions);
}
