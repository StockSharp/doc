# Сохранение и загрузка настроек

Для сохранения и загрузки настроек [Connector](xref:StockSharp.Algo.Connector) используются переопределения методов [Connector.Save](xref:StockSharp.Algo.Connector.Save(Ecng.Serialization.SettingsStorage)) и [Connector.Load](xref:StockSharp.Algo.Connector.Load(Ecng.Serialization.SettingsStorage)) соответственно. 

Для сохранения и загрузки настроек из внешнего файла можно воспользоваться соответственно сериализацией и десериализацией, реализованной в [S\#](../../api.md). 

```cs
...
private readonly Connector _connector = new Connector();
private readonly IFileSystem _fileSystem = Paths.FileSystem;
private const string _connectorFile = "ConnectorFile.json";
...
public void Load()
{
	if (_fileSystem.FileExists(_connectorFile))
	{
		//Загрузка настроек коннектора из существующего конфигурационного файла
		_connector.Load(_connectorFile.Deserialize<SettingsStorage>(_fileSystem));
	}
}
...
public void Save()
{
	//Сохранение настроек коннектора в конфигурационный файл
	_connector.Save().Serialize(_fileSystem, _connectorFile);
}
...

```

> [!NOTE]
> Методы `Serialize` и `Deserialize` без параметра `IFileSystem` помечены как `[Obsolete]`. Используйте перегрузки с `IFileSystem` (например, `Paths.FileSystem`).

## См. также

[Создание собственного коннектора](creating_own_connector.md)
