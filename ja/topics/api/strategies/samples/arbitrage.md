# アービトラージ ストラテジー

## 概要

`ArbitrageStrategy` は、先物コントラクトとその原資産の間のアービトラージ ストラテジーです。銘柄間のスプレッドを追跡し、アービトラージ機会が発生したときにポジションを開きます。

## 主なコンポーネント

このストラテジーは [Strategy](xref:StockSharp.Algo.Strategies.Strategy) から継承し、設定にパラメーターを使用します。

```cs
public class ArbitrageStrategy : Strategy
{
	private enum ArbitrageState
	{
		Contango,        // 先物価格が原資産より高い
		Backwardation,   // 原資産価格が先物より高い
		None,            // ポジションなし
		OrderRegistration // 注文登録中
	}

	// ストラテジー パラメーター
	private readonly StrategyParam<Security> _futureSecurity;
	private readonly StrategyParam<Security> _stockSecurity;
	private readonly StrategyParam<Portfolio> _futurePortfolio;
	private readonly StrategyParam<Portfolio> _stockPortfolio;
	private readonly StrategyParam<decimal> _stockMultiplicator;
	private readonly StrategyParam<decimal> _futureVolume;
	private readonly StrategyParam<decimal> _stockVolume;
	private readonly StrategyParam<decimal> _profitToExit;
	private readonly StrategyParam<decimal> _spreadToGenerateSignal;
}
```

## ストラテジー パラメーター

このストラテジーでは、次のパラメーターをカスタマイズできます。

- **FutureSecurity** - 先物銘柄
- **StockSecurity** - 原資産銘柄
- **FuturePortfolio** - 先物取引用ポートフォリオ
- **StockPortfolio** - 原資産取引用ポートフォリオ
- **StockMultiplicator** - 原資産の乗数 (例: ロット サイズ)
- **FutureVolume** - 先物取引の数量
- **StockVolume** - 原資産取引の数量
- **ProfitToExit** - ポジション退出の利益しきい値
- **SpreadToGenerateSignal** - エントリー シグナル生成のスプレッドしきい値

## ストラテジーの初期化

[OnStarted2](xref:StockSharp.Algo.Strategies.Strategy.OnStarted2(System.DateTime)) メソッドでは、パラメーターが検証され、オーダーブックと自己約定へのサブスクリプションが作成されます。

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	if (FutureSecurity == null)
		throw new InvalidOperationException("先物銘柄が指定されていません。");

	if (StockSecurity == null)
		throw new InvalidOperationException("株式銘柄が指定されていません。");

	if (FuturePortfolio == null)
		throw new InvalidOperationException("先物ポートフォリオが指定されていません。");

	if (StockPortfolio == null)
		throw new InvalidOperationException("株式ポートフォリオが指定されていません。");

	_futId = FutureSecurity.ToSecurityId();
	_stockId = StockSecurity.ToSecurityId();

	// 両方の銘柄のオーダーブック更新を購読する
	var futureDepthSubscription = new Subscription(DataType.MarketDepth, FutureSecurity);
	var stockDepthSubscription = new Subscription(DataType.MarketDepth, StockSecurity);

	futureDepthSubscription.WhenOrderBookReceived(this).Do(ProcessMarketDepth).Apply(this);
	stockDepthSubscription.WhenOrderBookReceived(this).Do(ProcessMarketDepth).Apply(this);

	// 執行価格を追跡するために自己約定を購読する
	this
		.WhenOwnTradeReceived()
		.Do(OnOwnTradeReceived)
		.Apply(this);

	// マーケット データ購読リクエストを送信する
	Subscribe(futureDepthSubscription);
	Subscribe(stockDepthSubscription);
}
```

## マーケット データの処理

`ProcessMarketDepth` メソッドはオーダーブックが更新されたときに呼び出され、主要ロジックを実装します。

```cs
private void ProcessMarketDepth(IOrderBookMessage depth)
{
	// 各銘柄の直近オーダーブックを更新する
	if (depth.SecurityId == _futId)
		_lastFut = depth;
	else if (depth.SecurityId == _stockId)
		_lastSt = depth;

	// 両方の銘柄のデータを待つ
	if (_lastFut is null || _lastSt is null)
		return;

	// 指定数量に対する出来高加重平均価格を計算する
	_futBid = GetAveragePrice(_lastFut, Sides.Sell, FutureVolume);
	_futAck = GetAveragePrice(_lastFut, Sides.Buy, FutureVolume);
	_stBid = GetAveragePrice(_lastSt, Sides.Sell, StockVolume) * StockMultiplicator;
	_stAsk = GetAveragePrice(_lastSt, Sides.Buy, StockVolume) * StockMultiplicator;

	// 価格を検証する
	if (_futBid == 0 || _futAck == 0 || _stBid == 0 || _stAsk == 0)
		return;

	// スプレッドを計算する
	var contangoSpread = _futBid - _stAsk;        // 先物価格 > 原資産価格
	var backwardationSpread = _stBid - _futAck;   // 原資産価格 > 先物価格

	decimal spread;
	ArbitrageState arbitrageSignal;

	// 最良のアービトラージ機会を判定する
	if (backwardationSpread > contangoSpread)
	{
		arbitrageSignal = ArbitrageState.Backwardation;
		spread = backwardationSpread;
	}
	else
	{
		arbitrageSignal = ArbitrageState.Contango;
		spread = contangoSpread;
	}

	// 現在の状態とスプレッドをログに記録する
	LogInfo($"Current state {_currentState}, enter spread = {_enterSpread}");
	LogInfo($"{ArbitrageState.Backwardation} spread = {backwardationSpread}");
	LogInfo($"{ArbitrageState.Contango}        spread = {contangoSpread}");
	LogInfo($"スプレッドでエントリー:{SpreadToGenerateSignal}。利益で決済:{ProfitToExit}");

	// 現在の市場状況に基づいて利益を再計算する
	if (_currentState != ArbitrageState.None && _currentState != ArbitrageState.OrderRegistration)
	{
		CalculateProfit();
		LogInfo($"Profit: {_profit}");
	}

	// 現在の状態と市場状況に基づいてシグナルを処理する
	ProcessSignals(arbitrageSignal, spread);
}
```

## 取引ロジック

シグナル処理とエントリー/退出判断は、`ProcessSignals` メソッドに実装されています。

```cs
private void ProcessSignals(ArbitrageState arbitrageSignal, decimal spread)
{
	// オープン ポジションがなく、スプレッドがしきい値を超えたときに新しいポジションへ入る
	if (_currentState == ArbitrageState.None && spread > SpreadToGenerateSignal)
	{
		_currentState = ArbitrageState.OrderRegistration;

		if (arbitrageSignal == ArbitrageState.Backwardation)
		{
			ExecuteBackwardation();
		}
		else
		{
			ExecuteContango();
		}
	}
	// 利益しきい値に到達したとき Backwardation ポジションから退出する
	else if (_currentState == ArbitrageState.Backwardation && _profit >= ProfitToExit)
	{
		_currentState = ArbitrageState.OrderRegistration;
		CloseBackwardationPosition();
	}
	// 利益しきい値に到達したとき Contango ポジションから退出する
	else if (_currentState == ArbitrageState.Contango && _profit >= ProfitToExit)
	{
		_currentState = ArbitrageState.OrderRegistration;
		CloseContangoPosition();
	}
}
```

## 利益計算

`CalculateProfit` メソッドは、エントリー価格と現在価格に基づいて現在の利益を計算します。

```cs
private void CalculateProfit()
{
	switch (_currentState)
	{
		case ArbitrageState.Backwardation:
			// 先物を買い、原資産を売る - 先物価格が上昇し、原資産価格が下落すると利益
			_profit = (_stockExitPrice * StockMultiplicator - _stAsk) + (_futBid - _futureBuyPrice);
			break;

		case ArbitrageState.Contango:
			// 先物を売り、原資産を買う - 先物価格が下落し、原資産価格が上昇すると利益
			_profit = (_futureExitPrice - _futAck) + (_stBid - _stockBuyPrice * StockMultiplicator);
			break;

		default:
			_profit = 0;
			break;
	}
}
```

## 注文生成

アービトラージ ストラテジーを実行するために、注文を生成するメソッドが使用されます。

```cs
private (Order buy, Order sell) GenerateOrdersBackwardation()
{
	var futureBuy = CreateOrder(Sides.Buy, FutureVolume);
	futureBuy.Portfolio = FuturePortfolio;
	futureBuy.Security = FutureSecurity;
	futureBuy.Type = OrderTypes.Market;

	var stockSell = CreateOrder(Sides.Sell, StockVolume);
	stockSell.Portfolio = StockPortfolio;
	stockSell.Security = StockSecurity;
	stockSell.Type = OrderTypes.Market;

	return (futureBuy, stockSell);
}

private (Order sell, Order buy) GenerateOrdersContango()
{
	var futureSell = CreateOrder(Sides.Sell, FutureVolume);
	futureSell.Portfolio = FuturePortfolio;
	futureSell.Security = FutureSecurity;
	futureSell.Type = OrderTypes.Market;

	var stockBuy = CreateOrder(Sides.Buy, StockVolume);
	stockBuy.Portfolio = StockPortfolio;
	stockBuy.Security = StockSecurity;
	stockBuy.Type = OrderTypes.Market;

	return (futureSell, stockBuy);
}
```

## 特徴

- このストラテジーは 2 つの異なる銘柄と 2 つのポートフォリオでの動作をサポートします
- 迅速な執行のために成行注文が使用されます
- 注文執行の追跡にはルール (IMarketRule) が使用されます
- より正確な価格のために、数量に基づく出来高加重平均価格が計算されます
- アービトラージ ロジックは、順方向 (contango) と逆方向 (backwardation) の両方のスプレッドを考慮します
- 目標しきい値に到達したときの自動利益計算と退出をサポートします
