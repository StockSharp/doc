# 連続先物

[ContinuousSecurityWindow](xref:StockSharp.Xaml.ContinuousSecurityWindow) は、*連続*（[ContinuousSecurity](xref:StockSharp.Algo.ContinuousSecurity)）銘柄を作成するためのビジュアルエディターです。[連続先物](../../instruments/continuous_futures.md) を参照してください。

![Hydra カスタム連続先物](../../../../images/hydragluingcscustom.png)

このコンポーネントには次のものが含まれます。

- 特殊な [SecurityIdTextBox](xref:StockSharp.Xaml.SecurityIdTextBox) テキストフィールド。Id - \[Code\]@\[Board\] の入力により *連続* 銘柄を生成します。
- [SecurityJumpsEditor](xref:StockSharp.Xaml.SecurityJumpsEditor) コンポーネントは、*連続* 銘柄を構成する銘柄を扱うための特殊な DataGrid です。これらの銘柄は [SecurityJump](xref:StockSharp.Xaml.SecurityJump) クラスでラップされます。このクラスには [SecurityJump.Security](xref:StockSharp.Xaml.SecurityJump.Security) と [SecurityJump.Date](xref:StockSharp.Xaml.SecurityJump.Date)（ロールフォワード）という 2 つのプロパティがあります。追加された銘柄は [SecurityJumpsEditor.Jumps](xref:StockSharp.Xaml.SecurityJumpsEditor.Jumps) リストに格納されます。このコンポーネントには、コンポーネント内の銘柄の正確性を確認するための [SecurityJumpsEditor.Validate](xref:StockSharp.Xaml.SecurityJumpsEditor.Validate) 関数があります。
- 銘柄を追加\/削除するためのボタン。
- **Auto** ボタンを使用すると、*連続* 銘柄を自動的に作成できます。
- **Ok** ボタンは、*連続* 銘柄の作成を完了します。

**主なプロパティ**

- [ContinuousSecurityWindow.Security](xref:StockSharp.Xaml.ContinuousSecurityWindow.Security) - 連続銘柄
- [ContinuousSecurityWindow.SecurityStorage](xref:StockSharp.Xaml.ContinuousSecurityWindow.SecurityStorage) - 銘柄に関する情報のプロバイダー。

以下は、その使用例のコードスニペットです。

```cs
private void CreateContinuousSecurity_OnClick(object sender, RoutedEventArgs e)
{
	_continuousSecurityWindow = new ContinuousSecurityWindow
	{
		SecurityStorage = _entityRegistry.Securities,
		Security = new ContinuousSecurity { Board = ExchangeBoard.Associated }
	};
	if (!_continuousSecurityWindow.ShowModal(this))
		return;
	_continuousSecurity = _continuousSecurityWindow.Security;
	ContinuousSecurity.Content = _continuousSecurity.Id;
	var first = _continuousSecurity.InnerSecurities.First();
	var gluingSecurity = new Security
	{
		Id = _continuousSecurity.Id,
		Code = _continuousSecurity.Code,
		Board = ExchangeBoard.Associated,
		Type = _continuousSecurity.Type,
		VolumeStep = first.VolumeStep,
		PriceStep = first.PriceStep,
		ExtensionInfo = new Dictionary<object, object> { { "GluingSecurity", true } }
	};
	if (_entityRegistry.Securities.ReadById(gluingSecurity.Id) == null)
	{
		_entityRegistry.Securities.Save(gluingSecurity);
	}
}
```

## 推奨コンテンツ

[連続先物](../../../hydra/instruments_and_boards/continuous_futures.md)
