# Лог заявок

![GUI orderlog](../../../../images/gui_orderlog.png)

[OrderLogGrid](xref:StockSharp.Xaml.OrderLogGrid) - графический компонент для отображения лога заявок ([OrderLogItem](xref:StockSharp.BusinessEntities.OrderLogItem)). 

**Основные свойства и методы**

- [OrderLogGrid.LogItems](xref:StockSharp.Xaml.OrderLogGrid.LogItems) - список элементов лога заявок.
- [OrderLogGrid.SelectedLogItem](xref:StockSharp.Xaml.OrderLogGrid.SelectedLogItem) - выбранный элемент лога заявок.
- [OrderLogGrid.SelectedLogItems](xref:StockSharp.Xaml.OrderLogGrid.SelectedLogItems) - выбранные элементы лога заявок.

Ниже показаны фрагменты кода с его использованием:

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
        
        // Подписываемся на событие получения элементов лога заявок
        _connector.OrderLogItemReceived += OnOrderLogItemReceived;
        
        // Создаем подписку на лог заявок
        _orderLogSubscription = new Subscription(DataType.OrderLog, security);
        
        // Запускаем подписку
        _connector.Subscribe(_orderLogSubscription);
    }
    
    // Обработчик события получения элемента лога заявок
    private void OnOrderLogItemReceived(Subscription subscription, OrderLogItem item)
    {
        // Проверяем, относится ли элемент лога к нашей подписке
        if (subscription != _orderLogSubscription)
            return;
            
        // Добавляем элемент в OrderLogGrid в потоке пользовательского интерфейса
        this.GuiAsync(() => OrderLogGrid.LogItems.Add(item));
    }
    
    // Метод для отписки при закрытии окна
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

### Фильтрация лога заявок

```cs
// Создание подписки на лог заявок с фильтрацией
public void SubscribeOrderLog(Security security, DateTime from, DateTime to)
{
    // Создаем подписку на лог заявок
    var orderLogSubscription = new Subscription(DataType.OrderLog, security)
    {
        MarketData =
        {
            // Указываем временной период для получения исторических данных
            From = from,
            To = to
        }
    };
    
    // Подписываемся на событие получения элементов лога заявок
    _connector.OrderLogItemReceived += OnFilteredOrderLogItemReceived;
    
    // Запускаем подписку
    _connector.Subscribe(orderLogSubscription);
}

// Обработчик события получения элемента лога заявок с фильтрацией
private void OnFilteredOrderLogItemReceived(Subscription subscription, OrderLogItem item)
{
    // Проверяем тип подписки
    if (subscription.DataType != DataType.OrderLog)
        return;
        
    // Фильтруем по цене (пример)
    if (item.Price < _minPrice || item.Price > _maxPrice)
        return;
        
    // Добавляем элемент в OrderLogGrid в потоке пользовательского интерфейса
    this.GuiAsync(() => 
    {
        OrderLogGrid.LogItems.Add(item);
        
        // Ограничиваем количество отображаемых элементов
        while (OrderLogGrid.LogItems.Count > _maxItems)
            OrderLogGrid.LogItems.RemoveAt(0);
    });
}
```

### Анализ динамики лога заявок

```cs
// Класс для анализа динамики лога заявок
public class OrderLogAnalyzer
{
    private readonly Connector _connector;
    private readonly Security _security;
    private readonly OrderLogGrid _orderLogGrid;
    
    // Счетчики для анализа
    private int _buyCount = 0;
    private int _sellCount = 0;
    private decimal _buyVolume = 0;
    private decimal _sellVolume = 0;
    
    public OrderLogAnalyzer(Connector connector, Security security, OrderLogGrid orderLogGrid)
    {
        _connector = connector;
        _security = security;
        _orderLogGrid = orderLogGrid;
        
        // Подписываемся на событие получения элементов лога заявок
        _connector.OrderLogItemReceived += OnOrderLogItemReceived;
        
        // Создаем подписку на лог заявок
        var subscription = new Subscription(DataType.OrderLog, security);
        
        // Запускаем подписку
        _connector.Subscribe(subscription);
    }
    
    // Обработчик события получения элемента лога заявок
    private void OnOrderLogItemReceived(Subscription subscription, OrderLogItem item)
    {
        if (item.SecurityId != _security.ToSecurityId())
            return;
            
        // Анализируем элемент лога заявок
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
        
        // Обновляем интерфейс с результатами анализа
        this.GuiAsync(() => 
        {
            // Добавляем элемент в OrderLogGrid
            _orderLogGrid.LogItems.Add(item);
            
            // Обновляем статистику
            UpdateStatistics();
        });
    }
    
    // Обновление статистики
    private void UpdateStatistics()
    {
        BuyCountLabel.Content = $"Покупки: {_buyCount}";
        SellCountLabel.Content = $"Продажи: {_sellCount}";
        BuyVolumeLabel.Content = $"Объем покупок: {_buyVolume}";
        SellVolumeLabel.Content = $"Объем продаж: {_sellVolume}";
        
        // Вычисляем дисбаланс
        var volumeImbalance = _buyVolume - _sellVolume;
        var imbalancePercent = (_buyVolume + _sellVolume) > 0 
            ? volumeImbalance / (_buyVolume + _sellVolume) * 100 
            : 0;
            
        ImbalanceLabel.Content = $"Дисбаланс: {volumeImbalance:F2} ({imbalancePercent:F2}%)";
    }
}
```