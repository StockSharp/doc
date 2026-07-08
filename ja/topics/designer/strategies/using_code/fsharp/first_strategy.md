# F# ストラテジーの例

ソースコードからのストラテジー作成は、[キューブからアルゴリズムを作成](../../using_visual_designer/first_strategy.md)セクションでキューブから組み立てた SMA ストラテジーの例と同様の、SMA ストラテジーの例を使って示します。

このセクションでは、F# 言語の構文（またはストラテジー作成の基底として使用される [Strategy](../../../../api/strategies.md)）については説明せず、**Designer** でコードを扱う際の固有の機能に触れます。

> [!TIP]
> **Designer** で作成されたストラテジーは、[API](../../../../api.md) で作成されたストラテジーと互換性があります。これは共通の基底クラス [Strategy](../../../../api/strategies.md) を使用しているためです。これにより、**Designer** 外でこのようなストラテジーを実行することは、[スキーム](../../../live_execution/running_strategies_outside_of_designer.md)を実行する場合よりも大幅に簡単になります。

1. ストラテジーパラメーターは、特別な方法で作成します。

```fsharp
// --- ストラテジーパラメーター: CandleType, Long, Short, TakeValue, StopValue ---

// ローソク足タイプのパラメーター
let candleTypeParam =
	this.Param<DataType>(nameof(this.CandleType), DataType.TimeFrame(TimeSpan.FromMinutes 1.0))
		.SetDisplay("Candle type", "Candle type for strategy calculation.", "General")

// 長期 SMA のパラメーター
let longParam =
	this.Param<int>(nameof(this.Long), 80)

// 短期 SMA のパラメーター
let shortParam =
	this.Param<int>(nameof(this.Short), 30)

// テイクプロフィットのパラメーター
let takeValueParam =
	this.Param<Unit>(nameof(this.TakeValue), Unit(0m, UnitTypes.Absolute))

// ストップロスのパラメーター
let stopValueParam =
	this.Param<Unit>(nameof(this.StopValue), Unit(2m, UnitTypes.Percent))

// SMA クロスを追跡するフラグ
let mutable isShortLessThenLong : bool option = None

// --------------------- パブリックプロパティ ---------------------

/// <summary>ストラテジーで使用するローソク足タイプ。</summary>
member this.CandleType
	with get () = candleTypeParam.Value
	and set value = candleTypeParam.Value <- value

/// <summary>長期 SMA インジケーターの期間。</summary>
member this.Long
	with get () = longParam.Value
	and set value = longParam.Value <- value

/// <summary>短期 SMA インジケーターの期間。</summary>
member this.Short
	with get () = shortParam.Value
	and set value = shortParam.Value <- value

/// <summary>テイクプロフィット値。</summary>
member this.TakeValue
	with get () = takeValueParam.Value
	and set value = takeValueParam.Value <- value

/// <summary>ストップロス値。</summary>
member this.StopValue
	with get () = stopValueParam.Value
	and set value = stopValueParam.Value <- value
```

[StrategyParam](xref:StockSharp.Algo.Strategies.StrategyParam`1) クラスを使用すると、設定の保存と復元の仕組みが自動的に使用されます。

2. インジケーターを作成し、市場データを購読する場合は、購読から入ってくるデータでインジケーター値を更新できるように、それらをバインドする必要があります。

```fsharp
// ---------- インジケーターを作成 ----------
let longSma = SMA()
longSma.Length <- this.Long

let shortSma = SMA()
shortSma.Length <- this.Short
// ---------------------------------------

// ------ ローソク足フローを購読し、インジケーターをバインド ------
let subscription = this.SubscribeCandles(this.CandleType)

// インジケーターを購読にバインドし、処理関数を割り当てる
subscription
	.Bind(longSma, shortSma, fun candle longV shortV -> this.OnProcess(candle, longV, shortV))
	.Start() |> ignore
```

3. チャートを扱う場合、ストラテジーを [Designer 外で](../../../live_execution/running_strategies_outside_of_designer.md)実行すると、チャートオブジェクトが存在しない可能性があることを考慮することが重要です。

```fsharp
// ------------- チャートを設定 -------------
let area = this.CreateChartArea()

// GUI がない場合（例: Runner またはコンソールアプリ）、area は null になる可能性がある
if not (isNull area) then
	// ローソク足を描画
	this.DrawCandles(area, subscription) |> ignore

	// インジケーターを描画
	this.DrawIndicator(area, shortSma, System.Drawing.Color.Coral) |> ignore
	this.DrawIndicator(area, longSma) |> ignore

	// 自分の取引を描画
	this.DrawOwnTrades(area) |> ignore
```

4. ストラテジーロジックで必要な場合は、[StartProtection](xref:StockSharp.Algo.Strategies.Strategy.StartProtection(StockSharp.Messages.Unit,StockSharp.Messages.Unit,System.Boolean,System.Nullable{System.TimeSpan},System.Nullable{System.TimeSpan},System.Boolean)) によってポジション保護を開始します。

```fsharp
this.StartProtection(this.TakeValue, this.StopValue)
```

5. ストラテジーロジック本体は、OnProcess メソッドに実装します。このメソッドは、ステップ 1 で作成した購読によって呼び出されます。

```fsharp
member private this.OnProcess
	(
		candle: ICandleMessage,
		longValue: decimal,
		shortValue: decimal
	) =
	// ローソク足情報をログに記録
	this.LogInfo(
		LocalizedStrings.SmaNewCandleLog,
		candle.OpenTime,
		candle.OpenPrice,
		candle.HighPrice,
		candle.LowPrice,
		candle.ClosePrice,
		candle.TotalVolume,
		candle.SecurityId
	)

	// ローソク足が確定していなければスキップ
	if candle.State <> CandleStates.Finished then
		()
	else
		// 短期 SMA が長期 SMA より小さいかを判定
		let shortLess = shortValue < longValue

		match isShortLessThenLong with
		| None ->
			// 初回: 現在の関係だけを記憶
			isShortLessThenLong <- Some shortLess
		| Some prevValue when prevValue <> shortLess ->
			// クロスが発生
			// short < long の場合は Sell、それ以外は Buy
			let direction =
				if shortLess then
					Sides.Sell
				else
					Sides.Buy

			// 新規ポジションを開く、または反転するための数量を計算
			// ポジションがない場合は Volume を使用し、それ以外の場合は
			// 絶対ポジションサイズと Volume の小さい方を 2 倍する
			let vol =
				if this.Position = 0m then
					this.Volume
				else
					(abs this.Position |> min this.Volume) * 2m

			// 指値価格用の価格ステップを取得
			let priceStep =
				let step = this.GetSecurity().PriceStep
				if step.HasValue then step.Value else 1m

			// 現在の終値より少し高い/低い指値注文価格を設定
			let limitPrice =
				match direction with
				| Sides.Buy  -> candle.ClosePrice + priceStep
				| Sides.Sell -> candle.ClosePrice - priceStep
				| _          -> candle.ClosePrice // 発生しないはず

			// 指値注文を送信
			match direction with
			| Sides.Buy  -> this.BuyLimit(limitPrice, vol) |> ignore
			| Sides.Sell -> this.SellLimit(limitPrice, vol) |> ignore
			| _          -> ()

			// 追跡フラグを更新
			isShortLessThenLong <- Some shortLess
		| _ ->
			// クロスがない場合は何もしない
			()
```
