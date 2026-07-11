# 注文ログ

![注文ログ のスクリーンショット](../../../../images/gui_orderlog.png)

[OrderLogGrid](xref:StockSharp.Xaml.OrderLogGrid) は、注文ログ（[OrderLogItem](xref:StockSharp.BusinessEntities.OrderLogItem)）を表示するためのグラフィカルコンポーネントです。

**主なプロパティとメソッド**

- [OrderLogGrid.LogItems](xref:StockSharp.Xaml.OrderLogGrid.LogItems) - 注文ログ項目のリスト。
- [OrderLogGrid.SelectedLogItem](xref:StockSharp.Xaml.OrderLogGrid.SelectedLogItem) - 選択された注文ログ項目。
- [OrderLogGrid.SelectedLogItems](xref:StockSharp.Xaml.OrderLogGrid.SelectedLogItems) - 選択された注文ログ項目群。

以下は、その使用方法を示すコード断片です。

```xaml
<Window x:Class="SampleITCH.OrdersLogWindow"
		xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
		xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
		xmlns:loc="clr-namespace:StockSharp.Localization;assembly=StockSharp.Localization"
		xmlns:xaml="http://schemas.stocksharp.com/xaml"
		Title="{x:Static loc:LocalizedStrings.OrderLog}" Height="750" Width="900">
	<xaml:OrderLogGrid x:Name="OrderLogGrid" x:FieldModifier="public" />
</Window>
```

```cs
public class OrderLogWindow
{
	private readonly Connector _connector;
	private readonly Security _security;
	private Subscription _orderLogSubscription;
	
	public OrderLogWindow(Connector connector, Security security)
	{
		InitializeComponent();
		
		_connector = connector;
		_security = security;
		
		// 注文ログ項目受信イベントを購読します
		_connector.OrderLogItemReceived += OnOrderLogItemReceived;
		
		// 注文ログへの購読を作成します
		_orderLogSubscription = new Subscription(DataType.OrderLog, security);
		
		// 購読を開始します
		_connector.Subscribe(_orderLogSubscription);
	}
	
	// 注文ログ項目受信イベントのハンドラー
	private void OnOrderLogItemReceived(Subscription subscription, OrderLogItem item)
	{
		// ログ項目が自分の購読に属しているか確認します
		if (subscription != _orderLogSubscription)
			return;
			
		// ユーザーインターフェイススレッドで項目を OrderLogGrid に追加します
		this.GuiAsync(() => OrderLogGrid.LogItems.Add(item));
	}
	
	// ウィンドウを閉じるときに購読を解除するメソッド
	public void Unsubscribe()
	{
		if (_orderLogSubscription != null)
		{
			_connector.OrderLogItemReceived -= OnOrderLogItemReceived;
			_connector.UnSubscribe(_orderLogSubscription);
			_orderLogSubscription = null;
		}
	}
}
```

### 注文ログのフィルタリング

```cs
// フィルタリング付きで注文ログへの購読を作成します
public void SubscribeOrderLog(Security security, DateTime from, DateTime to)
{
	// 注文ログへの購読を作成します
	var orderLogSubscription = new Subscription(DataType.OrderLog, security)
	{
		MarketData =
		{
			// 履歴データの期間を指定します
			From = from,
			To = to
		}
	};
	
	// 注文ログ項目受信イベントを購読します
	_connector.OrderLogItemReceived += OnFilteredOrderLogItemReceived;
	
	// 購読を開始します
	_connector.Subscribe(orderLogSubscription);
}

// フィルタリング付き注文ログ項目受信イベントのハンドラー
private void OnFilteredOrderLogItemReceived(Subscription subscription, OrderLogItem item)
{
	// 購読タイプを確認します
	if (subscription.DataType != DataType.OrderLog)
		return;
		
	// 価格でフィルタリングします（例）
	if (item.Price < _minPrice || item.Price > _maxPrice)
		return;
		
	// ユーザーインターフェイススレッドで項目を OrderLogGrid に追加します
	this.GuiAsync(() => 
	{
		OrderLogGrid.LogItems.Add(item);
		
		// 表示項目数を制限します
		while (OrderLogGrid.LogItems.Count > _maxItems)
			OrderLogGrid.LogItems.RemoveAt(0);
	});
}
```

### 注文ログの動態分析

```cs
// 注文ログの動態を分析するクラス
public class OrderLogAnalyzer
{
	private readonly Connector _connector;
	private readonly Security _security;
	private readonly OrderLogGrid _orderLogGrid;
	
	// 分析用カウンター
	private int _buyCount = 0;
	private int _sellCount = 0;
	private decimal _buyVolume = 0;
	private decimal _sellVolume = 0;
	
	public OrderLogAnalyzer(Connector connector, Security security, OrderLogGrid orderLogGrid)
	{
		_connector = connector;
		_security = security;
		_orderLogGrid = orderLogGrid;
		
		// 注文ログ項目受信イベントを購読します
		_connector.OrderLogItemReceived += OnOrderLogItemReceived;
		
		// 注文ログへの購読を作成します
		var subscription = new Subscription(DataType.OrderLog, security);
		
		// 購読を開始します
		_connector.Subscribe(subscription);
	}
	
	// 注文ログ項目受信イベントのハンドラー
	private void OnOrderLogItemReceived(Subscription subscription, OrderLogItem item)
	{
		if (item.SecurityId != _security.ToSecurityId())
			return;
			
		// 注文ログ項目を分析します
		if (item.Side == Sides.Buy)
		{
			_buyCount++;
			_buyVolume += item.Volume;
		}
		else if (item.Side == Sides.Sell)
		{
			_sellCount++;
			_sellVolume += item.Volume;
		}
		
		// 分析結果でインターフェイスを更新します
		this.GuiAsync(() => 
		{
			// 項目を OrderLogGrid に追加します
			_orderLogGrid.LogItems.Add(item);
			
			// 統計を更新します
			UpdateStatistics();
		});
	}
	
	// 統計を更新します
	private void UpdateStatistics()
	{
		BuyCountLabel.Content = $"買い: {_buyCount}";
		SellCountLabel.Content = $"売り: {_sellCount}";
		BuyVolumeLabel.Content = $"買い数量: {_buyVolume}";
		SellVolumeLabel.Content = $"売り数量: {_sellVolume}";
		
		// 不均衡を計算します
		var volumeImbalance = _buyVolume - _sellVolume;
		var imbalancePercent = (_buyVolume + _sellVolume) > 0 
			? volumeImbalance / (_buyVolume + _sellVolume) * 100 
			: 0;
			
		ImbalanceLabel.Content = $"不均衡: {volumeImbalance:F2} ({imbalancePercent:F2}%)";
	}
}
```
