# Libro de órdenes

![GUI MarketDepthControl](../../../../images/gui_marketdepthcontrol.png)

[MarketDepthControl](xref:StockSharp.Xaml.MarketDepthControl) - componente gráfico para mostrar el libro de órdenes. El componente permite mostrar cotizaciones y órdenes propias.

**Propiedades y métodos principales**

- [MarketDepthControl.MaxDepth](xref:StockSharp.Xaml.MarketDepthControl.MaxDepth) - profundidad del libro de órdenes.
- [MarketDepthControl.IsBidsOnTop](xref:StockSharp.Xaml.MarketDepthControl.IsBidsOnTop) - mostrar bids arriba.
- [MarketDepthControl.UpdateFormat](xref:StockSharp.Xaml.MarketDepthControl.UpdateFormat(StockSharp.BusinessEntities.Security))**(**instrumento [StockSharp.BusinessEntities.Security](xref:StockSharp.BusinessEntities.Security) **)** - actualizar el formato de visualización de precio y volumen usando el instrumento.
- [MarketDepthControl.ProcessOrder](xref:StockSharp.Xaml.MarketDepthControl.ProcessOrder(StockSharp.BusinessEntities.Order,System.Decimal,System.Decimal,StockSharp.Messages.OrderStates))**(**orden [StockSharp.BusinessEntities.Order](xref:StockSharp.BusinessEntities.Order), precio [System.Decimal](xref:System.Decimal), saldo [System.Decimal](xref:System.Decimal), estado [StockSharp.Messages.OrderStates](xref:StockSharp.Messages.OrderStates) **)** - procesar una orden.
- [MarketDepthControl.UpdateDepth](xref:StockSharp.Xaml.MarketDepthControl.UpdateDepth(StockSharp.Messages.IOrderBookMessage,StockSharp.BusinessEntities.Security))**(**mensaje [StockSharp.Messages.IOrderBookMessage](xref:StockSharp.Messages.IOrderBookMessage), instrumento [StockSharp.BusinessEntities.Security](xref:StockSharp.BusinessEntities.Security) **)** - actualizar el libro de órdenes usando un mensaje.

A continuación se muestran fragmentos de código que demuestran su uso:

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
		
		// Configurar el formato del libro de órdenes
		DepthCtrl.UpdateFormat(security);
		
		// Suscribirse al evento de recepción del libro de órdenes
		_connector.OrderBookReceived += OnMarketDepthReceived;
		
		// Crear una suscripción al libro de órdenes para el instrumento seleccionado
		_depthSubscription = new Subscription(DataType.MarketDepth, security);
		
		// Iniciar suscripción
		_connector.Subscribe(_depthSubscription);
	}
	
	// Manejador del evento de recepción del libro de órdenes
	private void OnMarketDepthReceived(Subscription subscription, IOrderBookMessage depth)
	{
		// Comprobar si el libro de órdenes pertenece a nuestra suscripción
		if (subscription != _depthSubscription)
			return;
			
		// Actualizar el libro de órdenes en el hilo de la interfaz de usuario
		this.GuiAsync(() => DepthCtrl.UpdateDepth(depth, _security));
	}
	
	// Método para cancelar la suscripción al cerrar la ventana
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

### Mostrar órdenes propias en el libro de órdenes

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
		
		// Configurar el formato del libro de órdenes
		DepthCtrl.UpdateFormat(security);
		
		// Suscribirse a eventos de recepción del libro de órdenes y de órdenes
		_connector.OrderBookReceived += OnMarketDepthReceived;
		_connector.OrderReceived += OnOrderReceived;
		
		// Crear una suscripción al libro de órdenes
		var depthSubscription = new Subscription(DataType.MarketDepth, security);
		_connector.Subscribe(depthSubscription);
		
		// Si es necesario, crear una suscripción a órdenes
		var ordersSubscription = new Subscription(DataType.Transactions, null);
		_connector.Subscribe(ordersSubscription);
	}
	
	// Manejador del evento de recepción del libro de órdenes
	private void OnMarketDepthReceived(Subscription subscription, IOrderBookMessage depth)
	{
		if (depth.SecurityId != _security.ToSecurityId())
			return;
			
		// Actualizar el libro de órdenes en el hilo de la interfaz de usuario
		this.GuiAsync(() => DepthCtrl.UpdateDepth(depth, _security));
	}
	
	// Manejador del evento de recepción de órdenes
	private void OnOrderReceived(Subscription subscription, Order order)
	{
		if (order.Security != _security)
			return;
			
		// Mostrar la orden en el libro de órdenes
		this.GuiAsync(() => DepthCtrl.ProcessOrder(
			order, 
			order.Price, 
			order.Balance, 
			order.State));
	}
}
```

### Obtener los mejores precios del libro de órdenes

```cs
// Método para obtener los mejores precios del libro de órdenes
public (decimal? BestBid, decimal? BestAsk) GetBestPrices(IOrderBookMessage depth)
{
	if (depth == null)
		return (null, null);
		
	var bestBid = depth.GetBestBid()?.Price;
	var bestAsk = depth.GetBestAsk()?.Price;
	
	return (bestBid, bestAsk);
}

// Usar el método para mostrar el spread
private void OnMarketDepthReceived(Subscription subscription, IOrderBookMessage depth)
{
	if (depth.SecurityId != _security.ToSecurityId())
		return;
		
	// Obtener mejores precios
	var (bestBid, bestAsk) = GetBestPrices(depth);
	
	// Calcular y mostrar spread
	if (bestBid.HasValue && bestAsk.HasValue)
	{
		var spread = bestAsk.Value - bestBid.Value;
		var spreadPercent = bestBid.Value > 0 ? spread / bestBid.Value * 100 : 0;
		
		this.GuiAsync(() => 
		{
			SpreadLabel.Content = $"Diferencial: {spread:F2} ({spreadPercent:F2}%)";
		});
	}
	
	// Actualizar libro de órdenes
	this.GuiAsync(() => DepthCtrl.UpdateDepth(depth, _security));
}
```

