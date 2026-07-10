# ストラテジーでの取引操作

StockSharp では、[Strategy](xref:StockSharp.Algo.Strategies.Strategy) クラスが注文を扱うためのさまざまなメソッドを提供しており、取引ストラテジーを便利に実装できます。

## 注文発注メソッド

StockSharp のストラテジーで注文を発注する方法はいくつかあります。

### 1. 高レベルメソッドの使用

最も簡単な方法は、1 回の呼び出しで注文を作成して登録する組み込みメソッドを使用することです。

```cs
// 成行価格で買う
BuyMarket(volume);

// 成行価格で売る
SellMarket(volume);

// 指値価格で買う
BuyLimit(price, volume);

// 指値価格で売る
SellLimit(price, volume);

// 現在のポジションを成行価格でクローズする
ClosePosition();
```

これらのメソッドは、最大限の簡潔さとコードの読みやすさを提供します。これらは自動的に次を行います。
- 指定されたパラメーターで注文オブジェクトを作成する
- 必要なフィールド（銘柄、ポートフォリオなど）を設定する
- 注文を取引システムに登録する

### 2. CreateOrder + RegisterOrder の使用

より柔軟な方法は、注文の作成と登録を分離することです。

```cs
// 注文オブジェクトを作成
var order = CreateOrder(Sides.Buy, price, volume);

// 追加の注文設定
order.Comment = "My special order";
order.TimeInForce = TimeInForce.MatchOrCancel;

// 注文を登録
RegisterOrder(order);
```

[CreateOrder](xref:StockSharp.Algo.Strategies.Strategy.CreateOrder(StockSharp.Messages.Sides,System.Decimal,System.Nullable{System.Decimal})) メソッドは、登録前にさらにカスタマイズできる初期化済みの注文オブジェクトを作成します。

### 3. 注文の直接作成と登録

最大限に制御したい場合は、注文オブジェクトを直接作成して登録できます。

```cs
// 注文オブジェクトを直接作成
var order = new Order
{
	Security = Security,
	Portfolio = Portfolio,
	Side = Sides.Buy,
	Type = OrderTypes.Limit,
	Price = price,
	Volume = volume,
	Comment = "Custom order"
};

// 注文を登録
RegisterOrder(order);
```

注文の扱いの詳細については、[注文](../orders_management.md) セクションを参照してください。

## 注文イベントの処理

注文を登録した後は、そのステータスを追跡することが重要です。ストラテジーでは次のことができます。

### 1. イベントハンドラーの使用

```cs
// 注文受信イベントを購読
OrderReceived += OnOrderReceived;

// 注文登録失敗イベントを購読
OrderRegisterFailed += OnOrderRegisterFailed;

private void OnOrderReceived(Order order)
{
	if (order.State == OrderStates.Done)
	{
		// 注文が約定したため、対応するロジックを実行
	}
}

private void OnOrderRegisterFailed(OrderFail fail)
{
	// 注文登録エラーを処理
	LogError($"注文登録エラー: {fail.Error}");
}
```

### 2. 注文にルールを使用する

より強力な方法は、注文に対して[ルール](event_model.md)を使用することです。

```cs
// 注文を作成
var order = BuyLimit(price, volume);

// 注文が約定したときに発火するルールを作成
order
	.WhenMatched(this)
	.Do(() => {
		// 注文約定後のアクション
		LogInfo($"Order {order.TransactionId} executed");
		
		// たとえば、ストップ注文を発注
		var stopOrder = SellLimit(price * 0.95, volume);
	})
	.Apply(this);

// 登録エラーを処理するルール
order
	.WhenRegisterFailed(this)
	.Do(fail => {
		LogError($"注文登録エラー: {fail.Error}");
		// 必要に応じて別のパラメーターで再試行
	})
	.Apply(this);
```

注文でルールを使用する詳しい例は、[注文ルールの例](event_model/samples/rule_order.md) セクションにあります。

## ポジション管理

ストラテジーはポジション管理のためのメソッドも提供します。

```cs
// 現在のポジションを取得
decimal currentPosition = Position;

// 現在のポジションをクローズ
ClosePosition();

	// ストップロスとテイクプロフィットでポジションを保護
StartProtection(
	takeProfit: new Unit(50, UnitTypes.Absolute),   // テイクプロフィット
	stopLoss: new Unit(20, UnitTypes.Absolute),     // ストップロス
	isStopTrailing: true,                        // トレーリングストップ
	useMarketOrders: true                        // 成行注文を使用
);
```

## 取引前のストラテジー状態

取引操作を実行する前に、ストラテジーが正しい状態にあることを確認することが重要です。StockSharp は、ストラテジーの準備状態を確認するためのいくつかのプロパティとメソッドを提供します。

### IsFormed プロパティ

[IsFormed](xref:StockSharp.Algo.Strategies.Strategy.IsFormed) プロパティは、ストラテジーで使用されるすべてのインジケーターが形成済み（ウォームアップ済み）かどうかを示します。既定では、[Indicators](xref:StockSharp.Algo.Strategies.Strategy.Indicators) コレクションに追加されたすべてのインジケーターが [IIndicator.IsFormed](xref:StockSharp.Algo.Indicators.IIndicator.IsFormed) = `true` の状態であることを確認します。

ストラテジーでのインジケーターの扱いについて詳しくは、[ストラテジー内のインジケーター](indicators.md) セクションを参照してください。

### IsOnline プロパティ

[IsOnline](xref:StockSharp.Algo.Strategies.Strategy.IsOnline) プロパティは、ストラテジーがリアルタイムモードにあるかどうかを示します。これは、ストラテジーが開始され、すべての市場データサブスクリプションが [SubscriptionStates.Online](xref:StockSharp.Messages.SubscriptionStates.Online) 状態に遷移した場合にのみ `true` になります。

ストラテジーでの市場データサブスクリプションの詳細については、[ストラテジーでの市場データサブスクリプション](subscriptions.md) セクションを参照してください。

### TradingMode プロパティ

[TradingMode](xref:StockSharp.Algo.Strategies.Strategy.TradingMode) プロパティは、ストラテジーの取引モードを定義します。指定可能な値:

- [StrategyTradingModes.Full](xref:StockSharp.Algo.Strategies.StrategyTradingModes.Full) - すべての取引操作が許可されます（既定モード）
- [StrategyTradingModes.Disabled](xref:StockSharp.Algo.Strategies.StrategyTradingModes.Disabled) - 取引は完全に無効化されます
- [StrategyTradingModes.CancelOrdersOnly](xref:StockSharp.Algo.Strategies.StrategyTradingModes.CancelOrdersOnly) - 注文のキャンセルのみ許可されます
- [StrategyTradingModes.ReducePositionOnly](xref:StockSharp.Algo.Strategies.StrategyTradingModes.ReducePositionOnly) - ポジションを減少させる操作のみ許可されます

このプロパティは、ストラテジーパラメーターを通じて設定できます。

```cs
public SmaStrategy()
{
	_tradingMode = Param(nameof(TradingMode), StrategyTradingModes.Full)
					.SetDisplay("取引モード", "許可される取引操作", "基本設定");
}
```

### 状態確認のためのヘルパーメソッド

ストラテジーが取引可能な準備状態にあることを便利に確認するために、StockSharp はヘルパーメソッドを提供します。

- [IsFormedAndOnline()](xref:StockSharp.Algo.Strategies.Strategy.IsFormedAndOnline) - ストラテジーが `IsFormed = true` かつ `IsOnline = true` の状態であることを確認します

- [IsFormedAndOnlineAndAllowTrading(StrategyTradingModes)](xref:StockSharp.Algo.Strategies.Strategy.IsFormedAndOnlineAndAllowTrading(StockSharp.Algo.Strategies.StrategyTradingModes)) - ストラテジーが形成済みで、オンラインモードにあり、必要な取引権限を持っていることを確認します

`IsFormedAndOnlineAndAllowTrading` メソッドは、[StrategyTradingModes](xref:StockSharp.Algo.Strategies.StrategyTradingModes) 型の省略可能なパラメーター `required` を受け取ります。

```cs
public bool IsFormedAndOnlineAndAllowTrading(StrategyTradingModes required = StrategyTradingModes.Full)
```

このパラメーターにより、特定の操作に必要な取引権限の最小レベルを指定できます。

1. **StrategyTradingModes.Full**（既定値） - ストラテジーが完全な取引モード（`TradingMode = StrategyTradingModes.Full`）にある場合にのみ `true` を返します。ポジションを増加させる可能性がある操作に使用します。

2. **StrategyTradingModes.ReducePositionOnly** - ストラテジーが完全な取引モード、またはポジション削減のみのモードにある場合に `true` を返します。ポジションのクローズまたは一部クローズ操作に使用します。

3. **StrategyTradingModes.CancelOrdersOnly** - 任意のアクティブな取引モード（`Disabled` を除く）で `true` を返します。注文キャンセル操作に使用します。

これにより、現在の取引モードに応じて、さまざまな取引操作を選択的に許可または禁止できます。

```cs
// ポジションを増加させる新規注文を発注するには、完全な取引モードが必要
if (IsFormedAndOnlineAndAllowTrading(StrategyTradingModes.Full))
{
	// 任意の注文を発注できる
	RegisterOrder(CreateOrder(Sides.Buy, price, volume));
}
// ポジションをクローズするには、ポジション削減モードで十分
else if (IsFormedAndOnlineAndAllowTrading(StrategyTradingModes.ReducePositionOnly) && Position != 0)
{
	// ポジションのクローズのみ可能
	ClosePosition();
}
// アクティブ注文をキャンセルするには、注文キャンセルモードで十分
else if (IsFormedAndOnlineAndAllowTrading(StrategyTradingModes.CancelOrdersOnly))
{
	// 注文のキャンセルのみ可能
	CancelActiveOrders();
}
```

したがって、このメソッドにより、取引機能に対する安全なアクセス制御メカニズムを実装できます。より重要な操作（新規ポジションのオープンなど）にはより高い権限レベルを要求し、重要度の低い操作（注文キャンセル）は制限付き取引モードでも実行できます。

取引操作を行う前に、これらのメソッドを使用することをお勧めします。

```cs
private void ProcessCandle(ICandleMessage candle)
{
	// ストラテジーが形成済みでオンラインモードにあり、
	// 取引が許可されているか確認
	if (!IsFormedAndOnlineAndAllowTrading())
		return;
	
	// 取引ロジック
	// ...
}
```

## 取引操作の例

以下は、ストラテジーで注文を発注するさまざまな方法と、その約定を処理する方法を示す例です。

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);
	
	// ローソク足を購読
	var subscription = new Subscription(
		DataType.TimeFrame(TimeSpan.FromMinutes(5)),
		Security);
	
	// ローソク足を処理するルールを作成
	Connector
		.WhenCandlesFinished(subscription)
		.Do(ProcessCandle)
		.Apply(this);
	
	Connector.Subscribe(subscription);
}

private void ProcessCandle(ICandleMessage candle)
{
	// ストラテジーが取引可能な準備状態か確認
	if (!this.IsFormedAndOnlineAndAllowTrading())
		return;
	
	// 終値に基づく取引ロジックの例
	if (candle.ClosePrice > _previousClose * 1.01)
	{
		// オプション 1: 高レベルメソッドを使用
		var order = BuyLimit(candle.ClosePrice, Volume);
		
		// 注文約定を処理するルールを作成
		order
			.WhenMatched(this)
			.Do(() => {
				// 注文が約定したら、ストップロスとテイクプロフィットを設定
				StartProtection(
					takeProfit: new Unit(50, UnitTypes.Absolute),
					stopLoss: new Unit(20, UnitTypes.Absolute)
				);
			})
			.Apply(this);
	}
	else if (candle.ClosePrice < _previousClose * 0.99)
	{
		// オプション 2: 作成と登録を分離
		var order = CreateOrder(Sides.Sell, candle.ClosePrice, Volume);
		RegisterOrder(order);
		
		// イベント経由で処理する別の方法
		OrderReceived += (o) => {
			if (o == order && o.State == OrderStates.Done)
			{
				// 約定後のアクション
			}
		};
	}
	
	_previousClose = candle.ClosePrice;
}
```

## 関連項目

- [注文](../orders_management.md)
- [注文ルール](event_model/samples/rule_order.md)
- [イベントモデル](event_model.md)
- [ポジション保護](take_profit_and_stop_loss.md)
