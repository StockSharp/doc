# Guardar y cargar la configuración

Los métodos [Connector.Save](xref:StockSharp.Algo.Connector.Save(Ecng.Serialization.SettingsStorage)) y [Connector.Load](xref:StockSharp.Algo.Connector.Load(Ecng.Serialization.SettingsStorage)) se usan, respectivamente, para guardar y cargar la configuración de [Connector](xref:StockSharp.Algo.Connector). 

Para guardar y cargar la configuración desde un archivo externo, puede usar la serialización y deserialización, respectivamente, implementadas en [S#](../../api.md). 

```cs
...
private readonly Connector _connector = new Connector();
private const string _connectorFile = "ConnectorFile.json";
...
public void Load()
{
	if (File.Exists(_connectorFile))
	{
		// Cargar ajustes del conector desde un archivo de configuración existente
		_connector.Load(new JsonSerializer<SettingsStorage>().Deserialize(_connectorFile));
	}
}
...
public void Save()
{
	// Guardar ajustes del conector en el archivo de configuración
	new JsonSerializer<SettingsStorage>().Serialize(_connector.Save(), _connectorFile);
}
...
		
```

## Contenido recomendado

[Creación de un conector propio](creating_own_connector.md)
