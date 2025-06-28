# Тиковые сделки

![GUI TradeGrid](../../../../images/gui_tradegrid.png)

[TradeGrid](xref:StockSharp.Xaml.TradeGrid) - таблица сделок. 

**Основные свойства**

- [TradeGrid.Trades](xref:StockSharp.Xaml.TradeGrid.Trades) - список сделок.
- [TradeGrid.SelectedTrade](xref:StockSharp.Xaml.TradeGrid.SelectedTrade) - выбранная сделка.
- [TradeGrid.SelectedTrades](xref:StockSharp.Xaml.TradeGrid.SelectedTrades) - выбранные сделки.

Ниже показаны фрагменты кода с его использованием:

```xaml
<Window x:Class="Sample.TradesWindow"
	xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
	xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
	xmlns:loc="clr-namespace:StockSharp.Localization;assembly=StockSharp.Localization"
	xmlns:xaml="http://schemas.stocksharp.com/xaml"
	Title="{x:Static loc:LocalizedStrings.Str985}" Height="284" Width="544">
	<xaml:TradeGrid x:Name="TradeGrid" x:FieldModifier="public" />
</Window>
```

```cs
public class TradesWindow
{
	private readonly Connector _connector;
	private readonly Security _security;
	private Subscription _tickSubscription;
	
	public TradesWindow(Connector connector, Security security)
	{
		InitializeComponent();
		
		_connector = connector;
		_security = security;
		
		// Подписываемся на событие получения тиковых сделок
		_connector.TickTradeReceived += OnTickReceived;
		
		// Создаем подписку на тиковые сделки
		_tickSubscription = new Subscription(DataType.Ticks, security);
		
		// Запускаем подписку
		_connector.Subscribe(_tickSubscription);
	}
	
	// Обработчик события получения тиковой сделки
	private void OnTickReceived(Subscription subscription, ITickTradeMessage tick)
	{
		// Проверяем, относится ли сделка к нашей подписке
		if (subscription != _tickSubscription)
			return;
			
		// Добавляем сделку в TradeGrid в потоке пользовательского интерфейса
		this.GuiAsync(() => TradeGrid.Trades.Add(tick));
	}
	
	// Метод для отписки при закрытии окна
	public void Unsubscribe()
	{
		if (_tickSubscription != null)
		{
			_connector.TickTradeReceived -= OnTickReceived;
			_connector.UnSubscribe(_tickSubscription);
			_tickSubscription = null;
		}
	}
}
```

### Отображение собственных сделок

```cs
public class MyTradesWindow
{
	private readonly Connector _connector;
	
	public MyTradesWindow(Connector connector)
	{
		InitializeComponent();
		
		_connector = connector;
		
		// Подписываемся на событие получения собственных сделок
		_connector.OwnTradeReceived += OnOwnTradeReceived;
		
		// Создаем подписку на транзакционные данные
		var myTradesSubscription = new Subscription(DataType.Transactions, null);
		
		// Запускаем подписку
		_connector.Subscribe(myTradesSubscription);
	}
	
	// Обработчик события получения собственной сделки
	private void OnOwnTradeReceived(Subscription subscription, MyTrade myTrade)
	{
		// Добавляем собственную сделку в TradeGrid в потоке пользовательского интерфейса
		this.GuiAsync(() => TradeGrid.Trades.Add(myTrade));
	}
}
```

### Получение исторических тиковых сделок

```cs
// Метод для получения исторических тиковых сделок
public void LoadHistoricalTicks(Security security, DateTime from, DateTime to)
{
	// Очищаем текущие сделки
	TradeGrid.Trades.Clear();
	
	// Создаем подписку на исторические тиковые сделки
	var historySubscription = new Subscription(DataType.Ticks, security)
	{
		MarketData =
		{
			// Указываем временной период для получения исторических данных
			From = from,
			To = to
		}
	};
	
	// Подписываемся на событие получения тиковых сделок
	_connector.TickTradeReceived += OnHistoricalTickReceived;
	
	// Запускаем подписку
	_connector.Subscribe(historySubscription);
}

// Обработчик события получения исторической тиковой сделки
private void OnHistoricalTickReceived(Subscription subscription, ITickTradeMessage tick)
{
	// Добавляем тик в TradeGrid в потоке пользовательского интерфейса
	this.GuiAsync(() => 
	{
		TradeGrid.Trades.Add(tick);
		
		// Обновляем статистику
		UpdateTradeStatistics();
	});
}

// Метод обновления статистики по сделкам
private void UpdateTradeStatistics()
{
	int totalTrades = TradeGrid.Trades.Count;
	decimal totalVolume = TradeGrid.Trades.Sum(t => t.Volume);
	decimal averagePrice = TradeGrid.Trades.Any() 
		? TradeGrid.Trades.Average(t => t.Price)
		: 0;
	
	// Обновляем элементы интерфейса статистики
	TotalTradesLabel.Content = $"Всего сделок: {totalTrades}";
	TotalVolumeLabel.Content = $"Общий объем: {totalVolume}";
	AveragePriceLabel.Content = $"Средняя цена: {averagePrice:F2}";
}
```

### Фильтрация сделок по объему

```cs
// Метод для фильтрации сделок по минимальному объему
public void FilterTicksByVolume(decimal minVolume)
{
	// Сохраняем значение фильтра
	_minVolumeFilter = minVolume;
	
	// Обновляем обработчик события получения тиковых сделок
	_connector.TickTradeReceived -= OnTickReceived;
	_connector.TickTradeReceived += OnFilteredTickReceived;
}

// Обработчик события получения тиковой сделки с фильтрацией по объему
private void OnFilteredTickReceived(Subscription subscription, ITickTradeMessage tick)
{
	// Проверяем, относится ли сделка к выбранному инструменту
	if (tick.SecurityId != _security.ToSecurityId())
		return;
		
	// Применяем фильтр по объему
	if (tick.Volume < _minVolumeFilter)
		return;
		
	// Добавляем сделку в TradeGrid в потоке пользовательского интерфейса
	this.GuiAsync(() => TradeGrid.Trades.Add(tick));
	
	// Если сделка крупная, можно выделить ее или подать уведомление
	if (tick.Volume >= _largeVolumeThreshold)
	{
		NotifyLargeVolumeTrade(tick);
	}
}

// Метод уведомления о крупной сделке
private void NotifyLargeVolumeTrade(ITickTradeMessage tick)
{
	// Выводим информацию о крупной сделке
	Console.WriteLine($"Крупная сделка: {tick.SecurityId}, {tick.ServerTime}, Цена: {tick.Price}, Объем: {tick.Volume}");
	
	// Можно добавить звуковое или визуальное уведомление
	this.GuiAsync(() => 
	{
		// Пример визуального выделения в списке
		var tradeItem = TradeGrid.Trades.LastOrDefault();
		if (tradeItem != null)
		{
			TradeGrid.SelectedTrade = tradeItem;
			HighlightTrade(tradeItem);
		}
	});
}
```