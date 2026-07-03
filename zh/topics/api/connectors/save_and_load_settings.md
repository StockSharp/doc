# 保存和加载设置

[Connector.Save](xref:StockSharp.Algo.Connector.Save(Ecng.Serialization.SettingsStorage)) 和 [Connector.Load](xref:StockSharp.Algo.Connector.Load(Ecng.Serialization.SettingsStorage)) 方法分别用于保存和加载 [Connector](xref:StockSharp.Algo.Connector) 设置。

要从外部文件保存和加载设置，可以分别使用在 [S#](../../api.md) 中实现的序列化和反序列化。

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

## 推荐内容

[创建自己的连接器](creating_own_connector.md)
