# Livro de ofertas

![GUI MarketDepthControl](../../../../images/gui_marketdepthcontrol.png)

[MarketDepthControl](xref:StockSharp.Xaml.MarketDepthControl) - um componente gráfico para apresentar o livro de ofertas. O componente permite apresentar cotações e ordens próprias.

**Propriedades e métodos principais**

- [MarketDepthControl.MaxDepth](xref:StockSharp.Xaml.MarketDepthControl.MaxDepth) - profundidade do livro de ofertas.
- [MarketDepthControl.IsBidsOnTop](xref:StockSharp.Xaml.MarketDepthControl.IsBidsOnTop) - apresentar bids no topo.
- [MarketDepthControl.UpdateFormat](xref:StockSharp.Xaml.MarketDepthControl.UpdateFormat(StockSharp.BusinessEntities.Security))**(**[StockSharp.BusinessEntities.Security](xref:StockSharp.BusinessEntities.Security) security **)** - atualizar o formato de apresentação de preço e volume usando o instrumento.
- [MarketDepthControl.ProcessOrder](xref:StockSharp.Xaml.MarketDepthControl.ProcessOrder(StockSharp.BusinessEntities.Order,System.Decimal,System.Decimal,StockSharp.Messages.OrderStates))**(**[StockSharp.BusinessEntities.Order](xref:StockSharp.BusinessEntities.Order) order, [System.Decimal](xref:System.Decimal) price, [System.Decimal](xref:System.Decimal) balance, [StockSharp.Messages.OrderStates](xref:StockSharp.Messages.OrderStates) state **)** - processar uma ordem.
- [MarketDepthControl.UpdateDepth](xref:StockSharp.Xaml.MarketDepthControl.UpdateDepth(StockSharp.Messages.IOrderBookMessage,StockSharp.BusinessEntities.Security))**(**[StockSharp.Messages.IOrderBookMessage](xref:StockSharp.Messages.IOrderBookMessage) message, [StockSharp.BusinessEntities.Security](xref:StockSharp.BusinessEntities.Security) security **)** - atualizar o livro de ofertas usando uma mensagem.

Abaixo encontram-se fragmentos de código que demonstram a sua utilização:

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
		
		// Configurar formatação do livro de ofertas
		DepthCtrl.UpdateFormat(security);
		
		// Assinar evento de recebimento do livro de ofertas
		_connector.OrderBookReceived += OnMarketDepthReceived;
		
		// Criar assinatura do livro de ofertas para o instrumento selecionado
		_depthSubscription = new Subscription(DataType.MarketDepth, security);
		
		// Iniciar assinatura
		_connector.Subscribe(_depthSubscription);
	}
	
	// Manipulador do evento de recebimento do livro de ofertas
	private void OnMarketDepthReceived(Subscription subscription, IOrderBookMessage depth)
	{
		// Verificar se o livro de ofertas pertence à nossa assinatura
		if (subscription != _depthSubscription)
			return;
			
		// Atualizar livro de ofertas na thread da interface
		this.GuiAsync(() => DepthCtrl.UpdateDepth(depth, _security));
	}
	
	// Método para cancelar a assinatura quando a janela é fechada
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

### Apresentar ordens próprias no livro de ofertas

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
		
		// Configurar formatação do livro de ofertas
		DepthCtrl.UpdateFormat(security);
		
		// Assinar eventos de recebimento do livro de ofertas e ordens
		_connector.OrderBookReceived += OnMarketDepthReceived;
		_connector.OrderReceived += OnOrderReceived;
		
		// Criar assinatura do livro de ofertas
		var depthSubscription = new Subscription(DataType.MarketDepth, security);
		_connector.Subscribe(depthSubscription);
		
		// If necessary, create a subscription to orders
		var ordersSubscription = new Subscription(DataType.Transactions, null);
		_connector.Subscribe(ordersSubscription);
	}
	
	// Manipulador do evento de recebimento do livro de ofertas
	private void OnMarketDepthReceived(Subscription subscription, IOrderBookMessage depth)
	{
		if (depth.SecurityId != _security.ToSecurityId())
			return;
			
		// Atualizar livro de ofertas na thread da interface
		this.GuiAsync(() => DepthCtrl.UpdateDepth(depth, _security));
	}
	
	// Manipulador do evento de recebimento de ordens
	private void OnOrderReceived(Subscription subscription, Order order)
	{
		if (order.Security != _security)
			return;
			
		// Exibir ordem no livro de ofertas
		this.GuiAsync(() => DepthCtrl.ProcessOrder(
			order, 
			order.Price, 
			order.Balance, 
			order.State));
	}
}
```

### Obter os melhores preços do livro de ofertas

```cs
// Método para obter melhores preços do livro de ofertas
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
		
	// Obter melhores preços
	var (bestBid, bestAsk) = GetBestPrices(depth);
	
	// Calcular e exibir spread
	if (bestBid.HasValue && bestAsk.HasValue)
	{
		var spread = bestAsk.Value - bestBid.Value;
		var spreadPercent = bestBid.Value > 0 ? spread / bestBid.Value * 100 : 0;
		
		this.GuiAsync(() => 
		{
			SpreadLabel.Content = $"Spread: {spread:F2} ({spreadPercent:F2}%)";
		});
	}
	
	// Atualizar livro de ofertas
	this.GuiAsync(() => DepthCtrl.UpdateDepth(depth, _security));
}
```
