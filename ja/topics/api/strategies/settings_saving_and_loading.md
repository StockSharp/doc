# 設定の保存と読み込み

StockSharp では、戦略設定の保存と読み込みの仕組みは [Strategy.Save](xref:StockSharp.Algo.Strategies.Strategy.Save(Ecng.Serialization.SettingsStorage)) および [Strategy.Load](xref:StockSharp.Algo.Strategies.Strategy.Load(Ecng.Serialization.SettingsStorage)) メソッドを通じて実装されています。

## 自動パラメーター処理

ほとんどの場合、`Save` および `Load` メソッドをオーバーライドする**必要はありません**。基底 [Strategy](xref:StockSharp.Algo.Strategies.Strategy) クラスは、[StrategyParam\<T\>](xref:StockSharp.Algo.Strategies.StrategyParam`1) を使用して作成された戦略パラメーターを自動的に保存および読み込みするためです。

推奨される方法は、[戦略パラメーター](parameters.md) セクションで詳しく説明されている戦略パラメーターメカニズムを使用することです。この方法では、すべてのパラメーターが自動的に保存および読み込みされます。

```cs
public class SmaStrategy : Strategy
{
	private readonly StrategyParam<int> _longSmaLength;

	public int LongSmaLength
	{
		get => _longSmaLength.Value;
		set => _longSmaLength.Value = value;
	}

	public SmaStrategy()
	{
		_longSmaLength = Param(nameof(LongSmaLength), 80)
							.SetDisplay("Long SMA length", string.Empty, "Base settings");
	}
}
```

## 特別なケースでのオーバーライド

`Save` および `Load` メソッドのオーバーライドが必要になるのは、標準的な戦略パラメーターのセットに含まれないデータを保存または読み込みする必要がある特別なケースに限られます。たとえば、内部状態、非標準のデータ構造、キャッシュ値を保存する場合です。

これらのメソッドをオーバーライドする場合、**基底クラスのメソッドを呼び出す必要があります**。

```cs
public override void Save(SettingsStorage settings)
{
	// まず基底メソッドを呼び出して標準パラメーターを保存
	base.Save(settings);
	
	// 次に固有の保存ロジックを追加
	settings.SetValue("CustomState", _customState);
}
	
public override void Load(SettingsStorage settings)
{
	// まず基底メソッドを呼び出して標準パラメーターを読み込み
	base.Load(settings);
	
	// 次に固有の読み込みロジックを追加
	if (settings.Contains("CustomState"))
		_customState = settings.GetValue<string>("CustomState");
}
```

## ファイルからの保存と読み込み

設定をファイルに保存したり、ファイルから読み込んだりするには、StockSharp に実装されているシリアル化および逆シリアル化を使用できます。

```cs
// 設定をファイルに保存
var settingsStorage = new SettingsStorage();
strategy.Save(settingsStorage);
new JsonSerializer<SettingsStorage>().Serialize(settingsStorage, "strategy.json");

// 設定をファイルから読み込み
var newStrategy = new SmaStrategy();
if (File.Exists("strategy.json"))
{
	var loadedSettings = new JsonSerializer<SettingsStorage>().Deserialize("strategy.json");
	newStrategy.Load(loadedSettings);
}
```

## 推奨事項

1. 可能な場合は、すべての設定可能な戦略パラメーターに [StrategyParam\<T\>](xref:StockSharp.Algo.Strategies.StrategyParam`1) を使用します。
2. 非標準データを保存/読み込みする必要がある場合にのみ、`Save` および `Load` メソッドをオーバーライドします。
3. オーバーライド時は、必ず基底メソッド `base.Save()` および `base.Load()` を呼び出します。
4. 設定をファイルに保存したり、ファイルから読み込んだりするには、StockSharp の標準シリアル化ツールを使用します。

## 関連項目

[戦略パラメーター](parameters.md)
