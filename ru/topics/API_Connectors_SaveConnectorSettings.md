# Сохранение и загрузка настроек

Для сохранения и загрузки настроек [Connector](../api/StockSharp.Algo.Connector.html) используются переопределения методов [Save](../api/StockSharp.Algo.Connector.Save.html) и [Load](../api/StockSharp.Algo.Connector.Load.html) соответственно. 

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
