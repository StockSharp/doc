# Коннекторы

Для работы с биржами и источниками данных в [S\#](../api.md) рекомендуется работать через базовый класс [Connector](xref:StockSharp.Algo.Connector).

Рассмотрим работу с [Connector](xref:StockSharp.Algo.Connector). Исходные коды примера находятся в проекте Samples\/01\_Basic\/01\_ConnectAndDownloadInstruments.

![multiconnection main](../../images/multiconnection_main.png)

Создаём экземпляр класса [Connector](xref:StockSharp.Algo.Connector):

```cs
...
public Connector Connector;
...
public MainWindow()
{
	InitializeComponent();
	Connector = new Connector();
	InitConnector();
}
		
```

Для конфигурирования [Connector](xref:StockSharp.Algo.Connector) у **API** есть специальный графический интерфейс, в котором можно настроить сразу несколько подключений одновременно. Как им воспользоваться описано в пункте [Графическое конфигурирование](connectors/graphical_configuration.md). 

```cs
...
private readonly IFileSystem _fileSystem = Paths.FileSystem;
private const string _connectorFile = "ConnectorFile.json";
...
private void Setting_Click(object sender, RoutedEventArgs e)
{
	if (Connector.Configure(this))
	{
		Connector.Save().Serialize(_fileSystem, _connectorFile);
	}
}

```

![API GUI ConnectorWindow](../../images/api_gui_connectorwindow.png)

Аналогично можно добавлять подключения напрямую из кода (без графических окон), воспользовавшись методом расширением [TraderHelper.AddAdapter\<TAdapter\>](xref:StockSharp.Algo.TraderHelper.AddAdapter``1(StockSharp.Algo.Connector,System.Action{``0}))**(**[StockSharp.Algo.Connector](xref:StockSharp.Algo.Connector) connector, [System.Action\<TAdapter\>](xref:System.Action`1) init **)**:

```cs
...
// Добавляем адаптер для подключения к Binance
connector.AddAdapter<BinanceMessageAdapter>(a => 
{
	a.Key = "<Your API Key>";
	a.Secret = "<Your Secret Key>";
});

// Добавляем RSS для новостей
connector.AddAdapter<RssMessageAdapter>(a =>
{
	a.Address = "https://news-source.com/feed";
});
	  				
```

В один объект [Connector](xref:StockSharp.Algo.Connector) можно добавлять неограниченное количество подключений. Поэтому одновременно из программы можно подключаться сразу к нескольким биржам и брокерам.

В методе *InitConnector* устанавливаем требуемые обработчики событий [IConnector](xref:StockSharp.BusinessEntities.IConnector):

```cs
private void InitConnector()
{
	// Подписка на событие успешного подключения
	Connector.Connected += () =>
	{
		this.GuiAsync(() => ChangeConnectStatus(true));
	};
	
	// Подписка на событие ошибки подключения
	Connector.ConnectionError += error => this.GuiAsync(() =>
	{
		ChangeConnectStatus(false);
		MessageBox.Show(this, error.ToString(), LocalizedStrings.ErrorConnection);
	});
	
	// Подписка на событие отключения
	Connector.Disconnected += () => this.GuiAsync(() => ChangeConnectStatus(false));
	
	// Подписка на событие ошибки
	Connector.Error += error =>
		this.GuiAsync(() => MessageBox.Show(this, error.ToString(), LocalizedStrings.Str2955));
	
	// Подписка на событие ошибки подписки на рыночные данные
	Connector.SubscriptionFailed += (subscription, error, isSubscribe) =>
		this.GuiAsync(() => MessageBox.Show(this, error.ToString(),
			LocalizedStrings.Str2956Params.Put(subscription.DataType, subscription.SecurityId)));
	
	// Подписки на получение данных
	
	// Инструменты
	Connector.SecurityReceived += (sub, security) => _securitiesWindow.SecurityPicker.Securities.Add(security);
	
	// Тиковые сделки
	Connector.TickTradeReceived += (sub, trade) => _tradesWindow.TradeGrid.Trades.TryAdd(trade);
	
	// Заявки
	Connector.OrderReceived += (sub, order) => _ordersWindow.OrderGrid.Orders.TryAdd(order);
	
	// Собственные сделки
	Connector.OwnTradeReceived += (sub, trade) => _myTradesWindow.TradeGrid.Trades.TryAdd(trade);
	
	// Позиции
	Connector.PositionReceived += (sub, position) => _portfoliosWindow.PortfolioGrid.Positions.TryAdd(position);

	// Ошибки регистрации заявок
	Connector.OrderRegisterFailReceived += (sub, fail) => _ordersWindow.OrderGrid.AddRegistrationFail(fail);
	
	// Ошибки снятия заявок
	Connector.OrderCancelFailReceived += (sub, fail) => OrderFailed(fail);
	
	// Установка поставщика рыночных данных
	_securitiesWindow.SecurityPicker.MarketDataProvider = Connector;
	
	try
	{
		if (File.Exists(_connectorFile))
		{
			var ctx = new ContinueOnExceptionContext();
			ctx.Error += ex => ex.LogError();
			using (new Scope<ContinueOnExceptionContext>(ctx))
				Connector.Load(_connectorFile.Deserialize<SettingsStorage>(_fileSystem));
		}
	}
	catch
	{
	}
	
	ConfigManager.RegisterService<IExchangeInfoProvider>(new InMemoryExchangeInfoProvider());
	
	// Регистрация провайдера адаптеров для графического конфигурирования
	ConfigManager.RegisterService<IMessageAdapterProvider>(
		new InMemoryMessageAdapterProvider(Connector.Adapter.InnerAdapters));
}
```

Как сохранять и загружать настройки [Connector](xref:StockSharp.Algo.Connector) в файл можно ознакомиться в пункте [Сохранение и загрузка настроек](connectors/save_and_load_settings.md).

О создании собственного [Connector](xref:StockSharp.Algo.Connector) можно ознакомиться в пункте [Создание собственного коннектора](connectors/creating_own_connector.md).

Выставление заявок описано в пунктах [Заявки](orders_management.md), [Создать новую заявку](orders_management/create_new_order.md), [Создать новую стоп заявку](orders_management/create_new_stop_order.md).

## Дополнительные возможности

### IFileSystem и Paths.FileSystem

Для файловых операций (сериализация/десериализация настроек) используйте `IFileSystem`. Экземпляр по умолчанию доступен через `Paths.FileSystem`:

```cs
private readonly IFileSystem _fileSystem = Paths.FileSystem;
```

Методы `Serialize` и `Deserialize` без параметра `IFileSystem` помечены как `[Obsolete]`.

### Асинхронные методы работы с заявками

Для работы с заявками доступны асинхронные методы:

- `RegisterOrderAsync` - асинхронная регистрация заявки.
- `CancelOrderAsync` - асинхронная отмена заявки.
- `EditOrderAsync` - асинхронное изменение заявки.

### SubscriptionsOnConnect

Свойство `SubscriptionsOnConnect` позволяет управлять подписками, которые автоматически выполняются при подключении. По умолчанию включает подписки на инструменты, портфели и заявки.

### События адаптеров

Для отслеживания событий по конкретным адаптерам доступны события:

- `ConnectedEx` - подключение конкретного адаптера.
- `DisconnectedEx` - отключение конкретного адаптера.
- `ConnectionErrorEx` - ошибка подключения конкретного адаптера.

### Жизненный цикл подписки

- `SubscriptionStarted` - подписка начата.
- `SubscriptionOnline` - подписка перешла в online-состояние (исторические данные получены, начата передача реальных данных).
- `SubscriptionFailed` - ошибка подписки (третий параметр `isSubscribe` указывает, произошла ли ошибка при подписке или отписке).
- `SubscriptionStopped` - подписка остановлена.

## См. также

[Графическое конфигурирование](connectors/graphical_configuration.md)