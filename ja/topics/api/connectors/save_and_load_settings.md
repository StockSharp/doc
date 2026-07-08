# 設定の保存と読み込み

[Connector.Save](xref:StockSharp.Algo.Connector.Save(Ecng.Serialization.SettingsStorage)) メソッドと [Connector.Load](xref:StockSharp.Algo.Connector.Load(Ecng.Serialization.SettingsStorage)) メソッドは、それぞれ [Connector](xref:StockSharp.Algo.Connector) の設定を保存および読み込むために使用します。 

外部ファイルから設定を保存および読み込むには、[S#](../../api.md) に実装されているシリアル化と逆シリアル化をそれぞれ使用できます。 

```cs
...
private readonly Connector _connector = new Connector();
private const string _connectorFile = "ConnectorFile.json";
...
public void Load()
{
	if (File.Exists(_connectorFile))
	{
		//既存の構成ファイルからコネクター設定をダウンロードする
		_connector.Load(new JsonSerializer<SettingsStorage>().Deserialize(_connectorFile));
	}
}
...
public void Save()
{
	//コネクター設定を構成ファイルに保存する
	new JsonSerializer<SettingsStorage>().Serialize(_connector.Save(), _connectorFile);
}
...
		
```

## 推奨コンテンツ

[独自コネクターの作成](creating_own_connector.md)
