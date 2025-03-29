# Стакан

![GUI MarketDepthControl](../../../../images/gui_marketdepthcontrol.png)

[MarketDepthControl](xref:StockSharp.Xaml.MarketDepthControl) - графический компонент для отображения стакана. Компонент позволяет отображать котировки и собственные заявки. 

**Основные свойства и методы**

- [MarketDepthControl.MaxDepth](xref:StockSharp.Xaml.MarketDepthControl.MaxDepth) - глубина стакана.
- [MarketDepthControl.IsBidsOnTop](xref:StockSharp.Xaml.MarketDepthControl.IsBidsOnTop) - отображать покупки сверху.
- [MarketDepthControl.UpdateFormat](xref:StockSharp.Xaml.MarketDepthControl.UpdateFormat(StockSharp.BusinessEntities.Security))**(**[StockSharp.BusinessEntities.Security](xref:StockSharp.BusinessEntities.Security) security **)** - обновить формат отображения цен и объёмов при помощи инструмента.
- [MarketDepthControl.ProcessOrder](xref:StockSharp.Xaml.MarketDepthControl.ProcessOrder(StockSharp.BusinessEntities.Order,System.Decimal,System.Decimal,StockSharp.Messages.OrderStates))**(**[StockSharp.BusinessEntities.Order](xref:StockSharp.BusinessEntities.Order) order, [System.Decimal](xref:System.Decimal) price, [System.Decimal](xref:System.Decimal) balance, [StockSharp.Messages.OrderStates](xref:StockSharp.Messages.OrderStates) state **)** - обработать заявку.
- [MarketDepthControl.UpdateDepth](xref:StockSharp.Xaml.MarketDepthControl.UpdateDepth(StockSharp.Messages.IOrderBookMessage,StockSharp.BusinessEntities.Security))**(**[StockSharp.Messages.IOrderBookMessage](xref:StockSharp.Messages.IOrderBookMessage) message, [StockSharp.BusinessEntities.Security](xref:StockSharp.BusinessEntities.Security) security **)** - обновить стакан при помощи сообщения.

Ниже показаны фрагменты кода с его использованием:

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
        
        // Настраиваем форматирование стакана
        DepthCtrl.UpdateFormat(security);
        
        // Подписываемся на событие получения стакана
        _connector.OrderBookReceived += OnMarketDepthReceived;
        
        // Создаем подписку на стакан для выбранного инструмента
        _depthSubscription = new Subscription(DataType.MarketDepth, security);
        
        // Запускаем подписку
        _connector.Subscribe(_depthSubscription);
    }
    
    // Обработчик события получения стакана
    private void OnMarketDepthReceived(Subscription subscription, IOrderBookMessage depth)
    {
        // Проверяем, относится ли стакан к нашей подписке
        if (subscription != _depthSubscription)
            return;
            
        // Обновляем стакан в потоке пользовательского интерфейса
        this.GuiAsync(() => DepthCtrl.UpdateDepth(depth, _security));
    }
    
    // Метод для отписки при закрытии окна
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

### Отображение собственных заявок в стакане

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
        
        // Настраиваем форматирование стакана
        DepthCtrl.UpdateFormat(security);
        
        // Подписываемся на события получения стакана и заявок
        _connector.OrderBookReceived += OnMarketDepthReceived;
        _connector.OrderReceived += OnOrderReceived;
        
        // Создаем подписку на стакан
        var depthSubscription = new Subscription(DataType.MarketDepth, security);
        _connector.Subscribe(depthSubscription);
        
        // Если необходимо, создаем подписку на заявки
        var ordersSubscription = new Subscription(DataType.Transactions, null);
        _connector.Subscribe(ordersSubscription);
    }
    
    // Обработчик события получения стакана
    private void OnMarketDepthReceived(Subscription subscription, IOrderBookMessage depth)
    {
        if (depth.SecurityId != _security.ToSecurityId())
            return;
            
        // Обновляем стакан в потоке пользовательского интерфейса
        this.GuiAsync(() => DepthCtrl.UpdateDepth(depth, _security));
    }
    
    // Обработчик события получения заявки
    private void OnOrderReceived(Subscription subscription, Order order)
    {
        if (order.Security != _security)
            return;
            
        // Отображаем заявку в стакане
        this.GuiAsync(() => DepthCtrl.ProcessOrder(
            order, 
            order.Price, 
            order.Balance, 
            order.State));
    }
}
```

### Получение лучших цен из стакана

```cs
// Метод для получения лучших цен из стакана
public (decimal? BestBid, decimal? BestAsk) GetBestPrices(IOrderBookMessage depth)
{
    if (depth == null)
        return (null, null);
        
    var bestBid = depth.GetBestBid()?.Price;
    var bestAsk = depth.GetBestAsk()?.Price;
    
    return (bestBid, bestAsk);
}

// Использование метода для отображения спреда
private void OnMarketDepthReceived(Subscription subscription, IOrderBookMessage depth)
{
    if (depth.SecurityId != _security.ToSecurityId())
        return;
        
    // Получаем лучшие цены
    var (bestBid, bestAsk) = GetBestPrices(depth);
    
    // Вычисляем и отображаем спред
    if (bestBid.HasValue && bestAsk.HasValue)
    {
        var spread = bestAsk.Value - bestBid.Value;
        var spreadPercent = bestBid.Value > 0 ? spread / bestBid.Value * 100 : 0;
        
        this.GuiAsync(() => 
        {
            SpreadLabel.Content = $"Спред: {spread:F2} ({spreadPercent:F2}%)";
        });
    }
    
    // Обновляем стакан
    this.GuiAsync(() => DepthCtrl.UpdateDepth(depth, _security));
}
```