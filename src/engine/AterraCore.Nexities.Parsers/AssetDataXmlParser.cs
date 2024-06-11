// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Nexities.Parsers.FileElements;
using Serilog;
using Xml;

namespace AterraCore.Nexities.Parsers;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class AssetDataXmlParser(ILogger logger, string nameSpace, string xsdPath) : XmlParser<AssetDataXml>(logger, nameSpace, xsdPath);
