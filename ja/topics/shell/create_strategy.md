# ストラテジーの作成

[Shell](../shell.md) に含まれるストラテジーは `Shell.Avalonia/Shell.Core/Strategies/` にあります。独自のストラテジーも同じ場所に追加し、3 つの手順を踏みます。ストラテジークラス、カタログへの登録、そして必要に応じて独自のパネルです。

## ストラテジークラス

ストラテジーは、StockSharp の `Strategy` を継承した通常のクラスです。Shell 固有のものは何もありません。

```cs
public class MyStrategy : Strategy
{
	private readonly StrategyParam<int> _length;

	public MyStrategy()
	{
		Name = "マイストラテジー";

		_length = Param(nameof(Length), 20)
			.SetGreaterThanZero()
			// 標準のパラメーターと同じグループに入るように、グループはリソースから取得します。
			.SetDisplay("長さ", "移動平均の長さ", LocalizedStrings.Parameters)
			.SetOptimize(10, 50, 5);
	}

	public int Length
	{
		get => _length.Value;
		set => _length.Value = value;
	}

	protected override void OnStarted2(DateTime time)
	{
		base.OnStarted2(time);

		// データのサブスクリプションと取引ロジック
	}
}
```

エントリポイントは `OnStarted` ではなく `OnStarted2(DateTime)` です。これが呼び出される時点で、ストラテジーはすでに銘柄、ポートフォリオ、接続を受け取っているため、ここで直接データをサブスクライブできます。

`SetOptimize(...)` を指定して `Param(...)` で宣言したパラメーターは、[最適化](user_interface/optimization.md) タブに自動的に表示されます。総当たりするパラメーターの一覧とその範囲は、別途指定するのではなく、ストラテジー自体から読み取られます。

## ストラテジーカタログ

ストラテジーを選択ウィンドウに表示するには、2 か所に追加する必要があります。

1 つ目は、型の識別子からインスタンスを作成する `BuiltInStrategies` のファクトリです。

```cs
public static Strategy TryCreate(string templateTypeId)
{
	...
	if (templateTypeId.Equals(MyTypeId, StringComparison.OrdinalIgnoreCase))
		return new MyStrategy();
	...
}
```

2 つ目は `LegacyStrategyTemplates` のテンプレートの説明です。一覧に表示されるストラテジー名と、型と既定値を持つパラメーターのセットは、ここから取得されます。

```cs
new StrategyTemplateDefinition(MyTypeId, "マイストラテジー", StrategyTemplateKind.Generic, true,
[
	.. CommonParameters(),
	new("Length", "長さ", StrategyParameterKind.Integer, "20", true, 1),
])
```

`CommonParameters()` は、すべてのストラテジーにある銘柄、ポートフォリオ、数量を返します。それ以外は、そのストラテジー独自のパラメーターです。説明のフィールドは、キー、ラベル、型、既定値、必須かどうか、最小値と最大値の順に並びます。

`StrategyTemplateKind` が区別するのは、シェルに専用のパネルがある種類（`Sma`、`SmaIndex`、`StairsTrend`、`StairsCountertrend`、`Designer`）だけです。専用のパネルを持たないストラテジーは `Generic` です。

ユーザーが **ストラテジー** パネルで指定した値は、`StrategyMaterializer` でインスタンスに適用されます。新しいパラメーターの解析もそこに追加します。

ストラテジーの設定はシェルによって保存・復元されるため、保存用のコードを別途書く必要はありません。

## 独自のパネル

標準のパラメーターパネルで十分な場合は、何もする必要はありません。Shell が自動的に表示します。独自のパネルが必要になるのは、ストラテジーにパラメーター以外にも表示するものがある場合です。その設定方法と監視方法は、[ストラテジーパネルの作成](create_strategy_panel.md) セクションで説明しています。

## 推奨コンテンツ

[ストラテジーパネルの作成](create_strategy_panel.md)

[Designer からストラテジーを実行](run_strategies_from_designer.md)
