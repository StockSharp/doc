# Save and load settings

[Save](../api/StockSharp.Algo.Connector.Save.html) and [Load](../api/StockSharp.Algo.Connector.Load.html) methods, respectively, are used to save and load [Connector](../api/StockSharp.Algo.Connector.html) settings. 

To save and load settings from an external file, you can use the serialization and deserialization, respectively, implemented in [S\#](StockSharpAbout.md). 

```cs
...
private readonly Connector \_connector \= new Connector();
private const string \_connectorFile \= "ConnectorFile";
...
public void Load()
{
	if (File.Exists(\_connectorFile))
	{
		\/\/Download connector settings from an existing configuration file
		\_connector.Load(new XmlSerializer\<SettingsStorage\>().Deserialize(\_connectorFile));
	}
}
...
public void Save()
{
	\/\/Save the connector settings to the configuration file
	new XmlSerializer\<SettingsStorage\>().Serialize(\_connector.Save(), \_connectorFile);
}
...
		
```

## Recommended content

[Creating own connector](ConnectorCreating.md)
