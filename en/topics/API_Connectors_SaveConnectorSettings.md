# Save and load settings

[Save](xref:StockSharp.Algo.Connector.Save) and [Load](xref:StockSharp.Algo.Connector.Load) methods, respectively, are used to save and load [Connector](xref:StockSharp.Algo.Connector) settings. 

To save and load settings from an external file, you can use the serialization and deserialization, respectively, implemented in [S\#](StockSharpAbout.md). 

```cs
...
private readonly Connector _connector = new Connector();
private const string _connectorFile = "ConnectorFile";
...
public void Load()
{
	if (File.Exists(_connectorFile))
	{
		//Download connector settings from an existing configuration file
		_connector.Load(new XmlSerializer<SettingsStorage>().Deserialize(_connectorFile));
	}
}
...
public void Save()
{
	//Save the connector settings to the configuration file
	new XmlSerializer<SettingsStorage>().Serialize(_connector.Save(), _connectorFile);
}
...
		
```

## Recommended content

[Creating own connector](ConnectorCreating.md)
