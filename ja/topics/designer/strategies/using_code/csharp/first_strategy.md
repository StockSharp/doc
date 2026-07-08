# C# ストラテジーの例

ソースコードからストラテジーを作成する方法を、SMA ストラテジーの例で示します。これは、[キューブからアルゴリズムを作成する](../../using_visual_designer/first_strategy.md) セクションでキューブから組み立てた SMA ストラテジーの例と同様です。

このセクションでは、C# 言語の構文 (またはストラテジー作成の基底として使用される [Strategy](../../../../api/strategies.md)) については説明せず、**Designer** でコードを扱う際の特有の機能について述べます。

> [!TIP]
> **Designer** で作成されたストラテジーは、[API](../../../../api.md) で作成されたストラテジーと互換性があります。これは共通の基底クラス [Strategy](../../../../api/strategies.md) を使用しているためです。これにより、**Designer** の外部でこのようなストラテジーを実行することは、[スキーム](../../../live_execution/running_strategies_outside_of_designer.md)を実行するよりも大幅に簡単になります。

1. ストラテジーパラメーターは、特別な方法を使用して作成します。

```cs
_candleTypeParam = this.Param(nameof(CandleType), DataType.TimeFrame(TimeSpan.FromMinutes(1)));
_long = this.Param(nameof(Long), 80);
_short = this.Param(nameof(Short), 20);


private readonly StrategyParam<DataType> _candleTypeParam;

public DataType CandleType
{
	get => _candleTypeParam.Value;
	set => _candleTypeParam.Value = value;
}

private readonly StrategyParam<int> _long;

public int Long
{
	get => _long.Value;
	set => _long.Value = value;
}

private readonly StrategyParam<int> _short;

public int Short
{
	get => _short.Value;
	set => _short.Value = value;
}
```

[StrategyParam](xref:StockSharp.Algo.Strategies.StrategyParam`1) クラスを使用すると、設定の保存と復元の方法が自動的に使用されます。

2. インジケーターを作成してマーケットデータをサブスクライブする場合、サブスクリプションからの受信データでインジケーター値を更新できるように、それらをバインドする必要があります。

```cs
// ---------- インジケーターを作成します -----------

var longSma = new SMA { Length = Long };
var shortSma = new SMA { Length = Short };

// ----------------------------------------

// --- ローソク足セットとインジケーターをバインドします ----

var subscription = SubscribeCandles(CandleType);

subscription
	// インジケーターをローソク足にバインドします。
	.Bind(longSma, shortSma, OnProcess)
	// 処理を開始します。
	.Start();
```

3. チャートを扱う場合、[Designer の外部](../../../live_execution/running_strategies_outside_of_designer.md)でストラテジーを実行すると、チャートオブジェクトが存在しない可能性があることを考慮する必要があります。

```cs
var area = CreateChartArea();

// GUI がない場合 (Runner または独自のコンソールアプリでストラテジーがホストされている場合)、area は null になることがあります。
if (area != null)
{
	DrawCandles(area, subscription);

	DrawIndicator(area, shortSma, System.Drawing.Color.Coral);
	DrawIndicator(area, longSma);

	DrawOwnTrades(area);
}
```

4. ストラテジーロジックで必要な場合は、[StartProtection](xref:StockSharp.Algo.Strategies.Strategy.StartProtection(StockSharp.Messages.Unit,StockSharp.Messages.Unit,System.Boolean,System.Nullable{System.TimeSpan},System.Nullable{System.TimeSpan},System.Boolean)) による開始ポジション保護を行います。

```cs
// 利益確定および/またはストップロスによる保護を開始します。
StartProtection(TakeValue, StopValue);
```

5. ストラテジーロジック自体は、OnProcess メソッドに実装されています。このメソッドはステップ 1 で作成したサブスクリプションによって呼び出されます。

```cs
private void OnProcess(ICandleMessage candle, decimal longValue, decimal shortValue)
{
	LogInfo(LocalizedStrings.SmaNewCandleLog, candle.OpenTime, candle.OpenPrice, candle.HighPrice, candle.LowPrice, candle.ClosePrice, candle.TotalVolume, candle.SecurityId);

	// 未確定のローソク足のみをサブスクライブしている場合
	if (candle.State != CandleStates.Finished)
		return;

	// short と long の新しい値を計算します。
	var isShortLessThenLong = shortValue < longValue;

	if (_isShortLessThenLong == null)
	{
		_isShortLessThenLong = isShortLessThenLong;
	}
	else if (_isShortLessThenLong != isShortLessThenLong)
	{
		// クロスが発生しました。

		// short が long より小さい場合は売り、それ以外は買いです。
		var direction = isShortLessThenLong ? Sides.Sell : Sides.Buy;

		// ポジションを開く、または反転するためのサイズを計算します。
		var volume = Position == 0 ? Volume : Position.Abs().Min(Volume) * 2;

		var priceStep = GetSecurity().PriceStep ?? 1;

		// 終値 + オフセットとして注文価格を計算します。
		var price = candle.ClosePrice + (direction == Sides.Buy ? priceStep : -priceStep);

		if (direction == Sides.Buy)
			BuyLimit(price, volume);
		else
			SellLimit(price, volume);

		// short と long の現在値を保存します。
		_isShortLessThenLong = isShortLessThenLong;
	}
}
```
