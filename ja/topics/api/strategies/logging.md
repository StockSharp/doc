# ストラテジーでのログ記録

StockSharp では、[Strategy](xref:StockSharp.Algo.Strategies.Strategy) クラスが [BaseLogReceiver](xref:Ecng.Logging.BaseLogReceiver) を継承しているため、取引ストラテジーの動作中に発生するすべてのアクションやイベントをログ記録する組み込みツールを使用できます。

## ログレベル

StockSharp は次のログレベルをサポートしています (重要度が低いものから高いものの順)。

1. Verbose - トレース用の最も詳細なログレベル
2. Debug - デバッグ用メッセージ
3. Info - 通常の情報メッセージ
4. Warning - 潜在的な問題に関する警告
5. Error - エラーメッセージ

## ストラテジー内のログ記録メソッド

ストラテジーは、ログへメッセージを書き込むために次のメソッドを提供します。

### LogVerbose

[LogVerbose](xref:Ecng.Logging.BaseLogReceiver.LogVerbose(System.String,System.Object[])) メソッドは、トレース用の詳細メッセージを記録するために設計されています。

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);
	
	LogVerbose("Strategy started with parameters: Long SMA={0}, Short SMA={1}", LongSmaLength, ShortSmaLength);
	
	// ...
}
```

### LogDebug

[LogDebug](xref:Ecng.Logging.BaseLogReceiver.LogDebug(System.String,System.Object[])) メソッドは、デバッグメッセージに使用されます。

```cs
private void ProcessCandle(ICandleMessage candle)
{
	LogDebug("Processing candle: {0}, Open={1}, Close={2}, High={3}, Low={4}, Volume={5}", 
		candle.OpenTime, candle.OpenPrice, candle.ClosePrice, candle.HighPrice, candle.LowPrice, candle.TotalVolume);
	
	// ...
}
```

### LogInfo

[LogInfo](xref:Ecng.Logging.BaseLogReceiver.LogInfo(System.String,System.Object[])) メソッドは、通常の情報メッセージに使用されます。

```cs
private void CalculateSignal(decimal shortSma, decimal longSma)
{
	bool isShortGreaterThanLong = shortSma > longSma;
	
	LogInfo("Signal: {0}, Short SMA={1}, Long SMA={2}", 
		isShortGreaterThanLong ? "Buy" : "Sell", shortSma, longSma);
	
	// ...
}
```

### LogWarning

[LogWarning](xref:Ecng.Logging.BaseLogReceiver.LogWarning(System.String,System.Object[])) メソッドは、警告を記録するために使用されます。

```cs
public void RegisterOrder(Order order)
{
	if (order.Volume <= 0)
	{
		LogWarning("Attempt to register an order with invalid volume: {0}", order.Volume);
		return;
	}
	
	// ...
}
```

### LogError

[LogError](xref:Ecng.Logging.BaseLogReceiver.LogError(System.String,System.Object[])) メソッドは、エラーメッセージを記録するために使用されます。

```cs
try
{
	// 何らかの処理
}
catch (Exception ex)
{
	LogError("Error while performing operation: {0}", ex.Message);
	Stop();
}
```

例外を直接受け取る [LogError](xref:Ecng.Logging.BaseLogReceiver.LogError(System.Exception)) オーバーロードもあります。

```cs
try
{
	// 何らかの処理
}
catch (Exception ex)
{
	LogError(ex);
	Stop();
}
```

## ログレベルの設定

[Strategy](xref:StockSharp.Algo.Strategies.Strategy) クラスには、どのメッセージをログへ書き込むかを決定する [LogLevel](xref:Ecng.Logging.ILogSource.LogLevel) プロパティがあります。

```cs
// ストラテジーのログレベルを設定
strategy.LogLevel = LogLevels.Info;
```

選択したログレベルでは、そのレベル以上のメッセージのみが記録されます。たとえば `LogLevels.Info` が設定されている場合、Verbose と Debug のメッセージは無視されます。

## LogLevel パラメーター

ストラテジーコンストラクターでログレベルを便利に設定するために、パラメーターを追加できます。

```cs
public class SmaStrategy : Strategy
{
	private readonly StrategyParam<LogLevels> _logLevel;
	
	public SmaStrategy()
	{
		_logLevel = Param(nameof(LogLevel), LogLevels.Info)
					.SetDisplay("Logging Level", "Level of log message detail", "Logging Settings");
	}
	
	public override LogLevels LogLevel
	{
		get => _logLevel.Value;
		set => _logLevel.Value = value;
	}
	
	// ...
}
```

## 実際のストラテジーでの使用例

### ストラテジー開始と停止のログ記録

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);
	
	LogInfo("Strategy {0} started at {1}. Instrument: {2}, Portfolio: {3}", 
		Name, time, Security?.Code, Portfolio?.Name);
	
	// ...
}

protected override void OnStopped()
{
	LogInfo("Strategy {0} stopped. Position: {1}, P&L: {2}", 
		Name, Position, PnL);
	
	base.OnStopped();
}
```

### 約定のログ記録

```cs
protected override void OnNewMyTrade(MyTrade trade)
{
	LogInfo("{0} {1} {2} at price {3}. Volume: {4}",
		trade.Order.Direction == Sides.Buy ? "Bought" : "Sold",
		trade.Order.Security.Code,
		trade.Order.Type,
		trade.Trade.Price,
		trade.Trade.Volume);

	base.OnNewMyTrade(trade);
}
```

### 注文登録エラーのログ記録

```cs
protected override void OnOrderRegisterFailed(OrderFail fail, bool calcRisk)
{
	LogError("Order registration error {0}: {1}", 
		fail.Order.TransactionId, fail.Error.Message);
	
	base.OnOrderRegisterFailed(fail, calcRisk);
}
```

## ログリスナーの接続

ストラテジーからメッセージを受信するには、[LogManager](xref:Ecng.Logging.LogManager) を通じてリスナーを接続します。

```cs
var logManager = new LogManager();

// ファイルへ書き込み
var fileListener = new FileLogListener("{0}_{1:00}_{2:00}.txt".Put(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day));
logManager.Listeners.Add(fileListener);

// メール送信
var emailListener = new EmailLogListener("from@stocksharp.com", "to@stocksharp.com");
emailListener.Filters.Add(msg => msg.Level == LogLevels.Error);
logManager.Listeners.Add(emailListener);

// ストラテジーをログソースとして追加
logManager.Sources.Add(strategy);
```

## ログの表示

ストラテジーログへ書き込まれたメッセージは、次の場所で表示できます。

1. [Designer](../../designer.md) プログラムの "Logs" パネル
2. [FileLogListener](xref:Ecng.Logging.FileLogListener) が設定されている場合はログファイル
3. [LogControl](xref:StockSharp.Xaml.LogControl) を通じたユーザーインターフェイス（[GuiLogListener](xref:StockSharp.Xaml.GuiLogListener) が使用されている場合）

## 関連項目

[ログ記録](../logging.md)
[ログパネル](../graphical_user_interface/logging/log_panel.md)
