# オーダーブック

![GUI MarketDepthControl](../../../../images/gui_marketdepthcontrol.png)

[MarketDepthControl](xref:StockSharp.Xaml.MarketDepthControl) は、オーダーブックを表示するためのグラフィカルコンポーネントです。このコンポーネントでは、気配と自分の注文を表示できます。

**主なプロパティとメソッド**

- [MarketDepthControl.MaxDepth](xref:StockSharp.Xaml.MarketDepthControl.MaxDepth) - オーダーブックの深さ。
- [MarketDepthControl.IsBidsOnTop](xref:StockSharp.Xaml.MarketDepthControl.IsBidsOnTop) - 買気配を上部に表示します。
- [MarketDepthControl.UpdateFormat](xref:StockSharp.Xaml.MarketDepthControl.UpdateFormat(StockSharp.BusinessEntities.Security))**(**[StockSharp.BusinessEntities.Security](xref:StockSharp.BusinessEntities.Security) security **)** - 銘柄を使用して価格と数量の表示形式を更新します。
- [MarketDepthControl.ProcessOrder](xref:StockSharp.Xaml.MarketDepthControl.ProcessOrder(StockSharp.BusinessEntities.Order,System.Decimal,System.Decimal,StockSharp.Messages.OrderStates))**(**[StockSharp.BusinessEntities.Order](xref:StockSharp.BusinessEntities.Order) order, [System.Decimal](xref:System.Decimal) price, [System.Decimal](xref:System.Decimal) balance, [StockSharp.Messages.OrderStates](xref:StockSharp.Messages.OrderStates) state **)** - 注文を処理します。
- [MarketDepthControl.UpdateDepth](xref:StockSharp.Xaml.MarketDepthControl.UpdateDepth(StockSharp.Messages.IOrderBookMessage,StockSharp.BusinessEntities.Security))**(**[StockSharp.Messages.IOrderBookMessage](xref:StockSharp.Messages.IOrderBookMessage) message, [StockSharp.BusinessEntities.Security](xref:StockSharp.BusinessEntities.Security) security **)** - メッセージを使用してオーダーブックを更新します。

以下は、その使用方法を示すコード断片です。

```xaml
<Window x:Class="SampleBarChart.QuotesWindow"
	xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
	xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
	xmlns:xaml="http://schemas.stocksharp.com/xaml"
	Title="QuotesWindow" Height="600" Width="280">
	<xaml:MarketDepthControl x:Name="DepthCtrl" x:FieldModifier="public" />
</Window>
```

```cs
public class MarketDepthWindow
{
	private readonly Connector _connector;
	private readonly Security _security;
	private Subscription _depthSubscription;
	
	public MarketDepthWindow(Connector connector, Security security)
	{
		InitializeComponent();
		
		_connector = connector;
		_security = security;
		
		// オーダーブックの書式を設定します
		DepthCtrl.UpdateFormat(security);
		
		// オーダーブック受信イベントを購読します
		_connector.OrderBookReceived += OnMarketDepthReceived;
		
		// 選択した銘柄のオーダーブックへの購読を作成します
		_depthSubscription = new Subscription(DataType.MarketDepth, security);
		
		// 購読を開始します
		_connector.Subscribe(_depthSubscription);
	}
	
	// オーダーブック受信イベントのハンドラー
	private void OnMarketDepthReceived(Subscription subscription, IOrderBookMessage depth)
	{
		// オーダーブックが自分の購読に属しているか確認します
		if (subscription != _depthSubscription)
			return;
			
		// ユーザーインターフェイススレッドでオーダーブックを更新します
		this.GuiAsync(() => DepthCtrl.UpdateDepth(depth, _security));
	}
	
	// ウィンドウを閉じるときに購読を解除するメソッド
	public void Unsubscribe()
	{
		if (_depthSubscription != null)
		{
			_connector.OrderBookReceived -= OnMarketDepthReceived;
			_connector.UnSubscribe(_depthSubscription);
			_depthSubscription = null;
		}
	}
}
```

### オーダーブックで自分の注文を表示する

```cs
public class MarketDepthWithOrdersWindow
{
	private readonly Connector _connector;
	private readonly Security _security;
	
	public MarketDepthWithOrdersWindow(Connector connector, Security security)
	{
		InitializeComponent();
		
		_connector = connector;
		_security = security;
		
		// オーダーブックの書式を設定します
		DepthCtrl.UpdateFormat(security);
		
		// オーダーブックと注文の受信イベントを購読します
		_connector.OrderBookReceived += OnMarketDepthReceived;
		_connector.OrderReceived += OnOrderReceived;
		
		// オーダーブックへの購読を作成します
		var depthSubscription = new Subscription(DataType.MarketDepth, security);
		_connector.Subscribe(depthSubscription);
		
		// 必要に応じて注文への購読を作成します
		var ordersSubscription = new Subscription(DataType.Transactions, null);
		_connector.Subscribe(ordersSubscription);
	}
	
	// オーダーブック受信イベントのハンドラー
	private void OnMarketDepthReceived(Subscription subscription, IOrderBookMessage depth)
	{
		if (depth.SecurityId != _security.ToSecurityId())
			return;
			
		// ユーザーインターフェイススレッドでオーダーブックを更新します
		this.GuiAsync(() => DepthCtrl.UpdateDepth(depth, _security));
	}
	
	// 注文受信イベントのハンドラー
	private void OnOrderReceived(Subscription subscription, Order order)
	{
		if (order.Security != _security)
			return;
			
		// オーダーブックに注文を表示します
		this.GuiAsync(() => DepthCtrl.ProcessOrder(
			order, 
			order.Price, 
			order.Balance, 
			order.State));
	}
}
```

### オーダーブックから最良価格を取得する

```cs
// オーダーブックから最良価格を取得するメソッド
public (decimal? BestBid, decimal? BestAsk) GetBestPrices(IOrderBookMessage depth)
{
	if (depth == null)
		return (null, null);
		
	var bestBid = depth.GetBestBid()?.Price;
	var bestAsk = depth.GetBestAsk()?.Price;
	
	return (bestBid, bestAsk);
}

// スプレッドを表示するためにメソッドを使用します
private void OnMarketDepthReceived(Subscription subscription, IOrderBookMessage depth)
{
	if (depth.SecurityId != _security.ToSecurityId())
		return;
		
	// 最良価格を取得します
	var (bestBid, bestAsk) = GetBestPrices(depth);
	
	// スプレッドを計算して表示します
	if (bestBid.HasValue && bestAsk.HasValue)
	{
		var spread = bestAsk.Value - bestBid.Value;
		var spreadPercent = bestBid.Value > 0 ? spread / bestBid.Value * 100 : 0;
		
		this.GuiAsync(() => 
		{
			SpreadLabel.Content = $"Spread: {spread:F2} ({spreadPercent:F2}%)";
		});
	}
	
	// オーダーブックを更新します
	this.GuiAsync(() => DepthCtrl.UpdateDepth(depth, _security));
}
```
