# Save and load settings

[Connector.Save](xref:StockSharp.Algo.Connector.Save(Ecng.Serialization.SettingsStorage)) and [Connector.Load](xref:StockSharp.Algo.Connector.Load(Ecng.Serialization.SettingsStorage)) methods, respectively, are used to save and load [Connector](xref:StockSharp.Algo.Connector) settings. 

To save and load settings from an external file, you can use the serialization and deserialization, respectively, implemented in [S\#](../../api.md). 

```cs
...
private readonly Connector _connector = new Connector();
private const string _connectorFile = "ConnectorFile.json";
...
public void Load()
{
	if (File.Exists(_connectorFile))
	{
		//Download connector settings from an existing configuration file
		_connector.Load(new JsonSerializer<SettingsStorage>().Deserialize(_connectorFile));
	}
}
...
public void Save()
{
	//Save the connector settings to the configuration file
	new JsonSerializer<SettingsStorage>().Serialize(_connector.Save(), _connectorFile);
}
...
		
```

## Recommended content

[Creating own connector](creating_own_connector.md)
