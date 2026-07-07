# Guardar e carregar definições

Os métodos [Connector.Save](xref:StockSharp.Algo.Connector.Save(Ecng.Serialization.SettingsStorage)) e [Connector.Load](xref:StockSharp.Algo.Connector.Load(Ecng.Serialization.SettingsStorage)) são usados, respetivamente, para guardar e carregar as definições do [Connector](xref:StockSharp.Algo.Connector).

Para guardar e carregar definições a partir de um ficheiro externo, pode usar a serialização e desserialização, respetivamente, implementadas no [S#](../../api.md).

```cs
...
private readonly Connector _connector = new Connector();
private const string _connectorFile = "ConnectorFile.json";
...
public void Load()
{
	if (File.Exists(_connectorFile))
	{
		//Carregar as definições do conector a partir de um ficheiro de configuração existente
		_connector.Load(new JsonSerializer<SettingsStorage>().Deserialize(_connectorFile));
	}
}
...
public void Save()
{
	//Guardar as definições do conector no ficheiro de configuração
	new JsonSerializer<SettingsStorage>().Serialize(_connector.Save(), _connectorFile);
}
...
		
```

## Ver também

[Criar o próprio conector](creating_own_connector.md)
