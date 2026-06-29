# 订单簿

![GUI MarketDepthControl](../../../../images/gui_marketdepthcontrol.png)

[MarketDepthControl](xref:StockSharp.Xaml.MarketDepthControl) - 一个用于显示订单簿的图形组件。该组件允许显示报价和自己的订单。

**主要属性和方法**

- [MarketDepthControl.MaxDepth](xref:StockSharp.Xaml.MarketDepthControl.MaxDepth) - 订单簿深度。
- [MarketDepthControl.IsBidsOnTop](xref:StockSharp.Xaml.MarketDepthControl.IsBidsOnTop) - 将出价显示在顶部。
- [MarketDepthControl.UpdateFormat](xref:StockSharp.Xaml.MarketDepthControl.UpdateFormat(StockSharp.BusinessEntities.Security))**(**[StockSharp.BusinessEntities.Security](xref:StockSharp.BusinessEntities.Security) 安全性 **)** - 使用该工具更新价格和成交量显示的格式。
- [MarketDepthControl.ProcessOrder](xref:StockSharp.Xaml.MarketDepthControl.ProcessOrder(StockSharp.BusinessEntities.Order,System.Decimal,System.Decimal,StockSharp.Messages.OrderStates))**(**[StockSharp.BusinessEntities.Order](xref:StockSharp.BusinessEntities.Order) 订单, [System.Decimal](xref:System.Decimal) 价格, [System.Decimal](xref:System.Decimal) 余额, [StockSharp.Messages.OrderStates](xref:StockSharp.Messages.OrderStates) 状态 **)** - 处理订单。
- [MarketDepthControl.UpdateDepth](xref:StockSharp.Xaml.MarketDepthControl.UpdateDepth(StockSharp.Messages.IOrderBookMessage,StockSharp.BusinessEntities.Security))**(**[StockSharp.Messages.IOrderBookMessage](xref:StockSharp.Messages.IOrderBookMessage) 消息, [StockSharp.BusinessEntities.Security](xref:StockSharp.BusinessEntities.Security) 安全 **)** - 使用消息更新订单簿。

以下是展示其用法的代码片段：

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
		
		// Configure order book formatting
		DepthCtrl.UpdateFormat(security);
		
		// Subscribe to order book reception event
		_connector.OrderBookReceived += OnMarketDepthReceived;
		
		// Create a subscription to order book for the selected instrument
		_depthSubscription = new Subscription(DataType.MarketDepth, security);
		
		// Start subscription
		_connector.Subscribe(_depthSubscription);
	}
	
	// Handler for order book reception event
	private void OnMarketDepthReceived(Subscription subscription, IOrderBookMessage depth)
	{
		// Check if the order book belongs to our subscription
		if (subscription != _depthSubscription)
			return;
			
		// Update the order book in the user interface thread
		this.GuiAsync(() => DepthCtrl.UpdateDepth(depth, _security));
	}
	
	// Method for unsubscribing when the window is closed
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

### 在订单簿中显示自己的订单

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
		
		// Configure order book formatting
		DepthCtrl.UpdateFormat(security);
		
		// Subscribe to order book and order reception events
		_connector.OrderBookReceived += OnMarketDepthReceived;
		_connector.OrderReceived += OnOrderReceived;
		
		// Create a subscription to order book
		var depthSubscription = new Subscription(DataType.MarketDepth, security);
		_connector.Subscribe(depthSubscription);
		
		// If necessary, create a subscription to orders
		var ordersSubscription = new Subscription(DataType.Transactions, null);
		_connector.Subscribe(ordersSubscription);
	}
	
	// Handler for order book reception event
	private void OnMarketDepthReceived(Subscription subscription, IOrderBookMessage depth)
	{
		if (depth.SecurityId != _security.ToSecurityId())
			return;
			
		// Update the order book in the user interface thread
		this.GuiAsync(() => DepthCtrl.UpdateDepth(depth, _security));
	}
	
	// Handler for order reception event
	private void OnOrderReceived(Subscription subscription, Order order)
	{
		if (order.Security != _security)
			return;
			
		// Display the order in the order book
		this.GuiAsync(() => DepthCtrl.ProcessOrder(
			order, 
			order.Price, 
			order.Balance, 
			order.State));
	}
}
```

### 从订单簿获取最佳价格

```cs
// Method to get best prices from the order book
public (decimal? BestBid, decimal? BestAsk) GetBestPrices(IOrderBookMessage depth)
{
	if (depth == null)
		return (null, null);
		
	var bestBid = depth.GetBestBid()?.Price;
	var bestAsk = depth.GetBestAsk()?.Price;
	
	return (bestBid, bestAsk);
}

// Using the method to display spread
private void OnMarketDepthReceived(Subscription subscription, IOrderBookMessage depth)
{
	if (depth.SecurityId != _security.ToSecurityId())
		return;
		
	// Get best prices
	var (bestBid, bestAsk) = GetBestPrices(depth);
	
	// Calculate and display spread
	if (bestBid.HasValue && bestAsk.HasValue)
	{
		var spread = bestAsk.Value - bestBid.Value;
		var spreadPercent = bestBid.Value > 0 ? spread / bestBid.Value * 100 : 0;
		
		this.GuiAsync(() => 
		{
			SpreadLabel.Content = $"Spread: {spread:F2} ({spreadPercent:F2}%)";
		});
	}
	
	// Update order book
	this.GuiAsync(() => DepthCtrl.UpdateDepth(depth, _security));
}
```