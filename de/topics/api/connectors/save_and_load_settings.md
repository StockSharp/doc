# Einstellungen speichern und laden

Die Methoden [Connector.Save](xref:StockSharp.Algo.Connector.Save(Ecng.Serialization.SettingsStorage)) und [Connector.Load](xref:StockSharp.Algo.Connector.Load(Ecng.Serialization.SettingsStorage)) werden verwendet, um Einstellungen des [Connector](xref:StockSharp.Algo.Connector) zu speichern bzw. zu laden.

Um Einstellungen aus einer externen Datei zu speichern und zu laden, koennen Sie die in [S#](../../api.md) implementierte Serialisierung bzw. Deserialisierung verwenden.

```cs
...
private readonly Connector _connector = new Connector();
private const string _connectorFile = "ConnectorFile.json";
...
public void Load()
{
	if (File.Exists(_connectorFile))
	{
		// Connector-Einstellungen aus einer vorhandenen Konfigurationsdatei laden
		_connector.Load(new JsonSerializer<SettingsStorage>().Deserialize(_connectorFile));
	}
}
...
public void Save()
{
	// Connector-Einstellungen in der Konfigurationsdatei speichern
	new JsonSerializer<SettingsStorage>().Serialize(_connector.Save(), _connectorFile);
}
...
		
```

## Empfohlene Inhalte

[Eigenen Connector erstellen](creating_own_connector.md)
