# Сохранение и загрузка настроек

Для сохранения и загрузки настроек [Connector](xref:StockSharp.Algo.Connector) используются переопределения методов [Connector.Save](xref:StockSharp.Algo.Connector.Save(Ecng.Serialization.SettingsStorage))**(**[Ecng.Serialization.SettingsStorage](xref:Ecng.Serialization.SettingsStorage) storage**)** и [Connector.Load](xref:StockSharp.Algo.Connector.Load(Ecng.Serialization.SettingsStorage))**(**[Ecng.Serialization.SettingsStorage](xref:Ecng.Serialization.SettingsStorage) storage**)** соответственно. 

Для сохранения и загрузки настроек из внешнего файла можно воспользоваться соответственно сериализацией и десериализацией, реализованной в [S\#](StockSharpAbout.md). 

```cs
...
private readonly Connector _connector = new Connector();
private const string _connectorFile = "ConnectorFile";
...
public void Load()
{
	if (File.Exists(_connectorFile))
	{
		//Загрузка настроек коннектора из существующего конфигурационного файла
		_connector.Load(new XmlSerializer<SettingsStorage>().Deserialize(_connectorFile));
	}
}
...
public void Save()
{
	//Сохранение настроек коннектора в конфигурационный файл
	new XmlSerializer<SettingsStorage>().Serialize(_connector.Save(), _connectorFile);
}
...
		
```

## См. также

[Создание собственного коннектора](ConnectorCreating.md)
