# Создание собственного коннектора

Механизм сообщений является внутренним логическим слоем архитектуры [StockSharp](https://github.com/StockSharp/StockSharp), который обеспечивает взаимодействие различных элементов платформы по стандартному протоколу. 

Выделяются два основных класса:

- [Message](xref:StockSharp.Messages.Message) - сообщение, несущее в себе информацию.
- [MessageAdapter](xref:StockSharp.Messages.MessageAdapter) - адаптер (=преобразователь) сообщений.

**Сообщение** выполняет роль агента, передающего информацию. Сообщения имеют свой тип [MessageTypes](xref:StockSharp.Messages.MessageTypes). Каждому типу сообщения соответствует определенный класс. В свою очередь все классы сообщений наследуют от абстрактного класса [Message](xref:StockSharp.Messages.Message), который наделяет потомков такими свойствами, как тип сообщения [Message.Type](xref:StockSharp.Messages.Message.Type) и [Message.LocalTime](xref:StockSharp.Messages.Message.LocalTime) - локальное время создания/получения сообщения.

Сообщения могут быть *входящими* и *исходящими*:

- *Входящие* сообщения - сообщения, которые посылаются во внешнюю систему. Обычно это команды, которые генерирует программа, например, сообщение [ConnectMessage](xref:StockSharp.Messages.ConnectMessage) - команда, запрашивающая соединение с сервером.
- *Исходящие* сообщения - сообщения, поступающие из внешней системы. Это сообщения, передающие информацию о рыночных данных, транзакциях, портфелях, событиях соединения и т.п. Например, сообщение [QuoteChangeMessage](xref:StockSharp.Messages.QuoteChangeMessage) - передает информацию об изменении стакана.

**Адаптер сообщений** играет роль посредника между торговой системой и программой. Для каждого типа коннектора имеется свой класс адаптера, который наследуется от абстрактного класса [MessageAdapter](xref:StockSharp.Messages.MessageAdapter).

> [!NOTE]
> [MessageAdapter](xref:StockSharp.Messages.MessageAdapter) является базовым классом для создания пользовательских адаптеров. Он предоставляет асинхронные виртуальные методы (`ConnectAsync`, `DisconnectAsync`, `ResetAsync` и др.), которые необходимо переопределить в наследнике.

Адаптер выполняет две основные функции:

1. Преобразует входящие сообщения в команды конкретной торговой системы.
2. Преобразует информацию, поступающую от торговой системы (соединение, рыночные данные, транзакции и т.п.), в исходящие сообщения.

Ниже описан процесс создания собственного адаптера для [Coinbase](https://github.com/StockSharp/StockSharp/tree/master/Connectors/Coinbase) (все коннекторы с исходным кодом доступны в [репозитории StockSharp](https://github.com/StockSharp/StockSharp/tree/master/Connectors) и предоставляются как учебное пособие).

## Пример создания адаптера сообщений Coinbase

### 1. Создание класса адаптера

Вначале создаем класс адаптера сообщений **CoinbaseMessageAdapter**, унаследованный от абстрактного класса [MessageAdapter](xref:StockSharp.Messages.MessageAdapter).

```cs
public partial class CoinbaseMessageAdapter : MessageAdapter
{
	private Authenticator _authenticator;
	private HttpClient _restClient;
	private SocketClient _socketClient;

	// Другие поля и свойства адаптера
}
```

### 2. Конструктор адаптера

В конструкторе адаптера необходимо выполнить следующие действия:

1. Передать генератор идентификаторов транзакций, который будет использоваться для создания идентификаторов сообщений.

2. Указать поддерживаемые типы сообщений с помощью методов:
  - [AddMarketDataSupport](xref:StockSharp.Messages.Extensions.AddMarketDataSupport(StockSharp.Messages.MessageAdapter)) - поддержка сообщений для подписки на рыночные данные.
  - [AddTransactionalSupport](xref:StockSharp.Messages.Extensions.AddTransactionalSupport(StockSharp.Messages.MessageAdapter)) - поддержка транзакционных сообщений.

3. Указать конкретные типы рыночных данных, которые поддерживает адаптер, с помощью метода [AddSupportedMarketDataType](xref:StockSharp.Messages.Extensions.AddSupportedMarketDataType(StockSharp.Messages.MessageAdapter,StockSharp.Messages.DataType)).

```cs
public CoinbaseMessageAdapter(IdGenerator transactionIdGenerator)
	: base(transactionIdGenerator)
{
	HeartbeatInterval = TimeSpan.FromSeconds(5);

	// Добавление поддержки рыночных данных и транзакций
	this.AddMarketDataSupport();
	this.AddTransactionalSupport();

	// Удаление неподдерживаемых типов сообщений
	this.RemoveSupportedMessage(MessageTypes.Portfolio);
	this.RemoveSupportedMessage(MessageTypes.OrderGroupCancel);

	// Добавление поддерживаемых типов рыночных данных
	this.AddSupportedMarketDataType(DataType.Ticks);
	this.AddSupportedMarketDataType(DataType.MarketDepth);
	this.AddSupportedMarketDataType(DataType.Level1);
	this.AddSupportedMarketDataType(DataType.CandleTimeFrame);
}
```

### 3. Подключение и отключение адаптера

Для подключения адаптера к торговой системе вызывается метод [MessageAdapter.ConnectAsync](xref:StockSharp.Messages.MessageAdapter.ConnectAsync(StockSharp.Messages.ConnectMessage,System.Threading.CancellationToken)). В него передается входящее сообщение [ConnectMessage](xref:StockSharp.Messages.ConnectMessage), при успешном соединении адаптер отправляет исходящее сообщение [ConnectMessage](xref:StockSharp.Messages.ConnectMessage):

```cs
public override async ValueTask ConnectAsync(ConnectMessage connectMsg, CancellationToken cancellationToken)
{
	// Проверка наличия ключей для транзакционного режима
	if (this.IsTransactional())
	{
		if (Key.IsEmpty())
			throw new InvalidOperationException(LocalizedStrings.KeyNotSpecified);

		if (Secret.IsEmpty())
			throw new InvalidOperationException(LocalizedStrings.SecretNotSpecified);
	}

	// Инициализация аутентификатора
	_authenticator = new(this.IsTransactional(), Key, Secret, Passphrase);

	// Проверка, что клиенты еще не созданы
	if (_restClient != null)
		throw new InvalidOperationException(LocalizedStrings.NotDisconnectPrevTime);

	if (_socketClient != null)
		throw new InvalidOperationException(LocalizedStrings.NotDisconnectPrevTime);

	// Создание REST клиента
	_restClient = new(_authenticator) { Parent = this };

	// Создание и настройка WebSocket клиента
	_socketClient = new(_authenticator, ReConnectionSettings.ReAttemptCount) { Parent = this };
	SubscribePusherClient();

	// Подключение WebSocket клиента
	await _socketClient.Connect(cancellationToken);

	// Отправка сообщения об успешном подключении
	SendOutMessage(new ConnectMessage());
}
```

Для отключения адаптера от торговой системы вызывается метод [MessageAdapter.DisconnectAsync](xref:StockSharp.Messages.MessageAdapter.DisconnectAsync(StockSharp.Messages.DisconnectMessage,System.Threading.CancellationToken)). При успешном отключении адаптер отправляет исходящее сообщение [DisconnectMessage](xref:StockSharp.Messages.DisconnectMessage):

```cs
public override ValueTask DisconnectAsync(DisconnectMessage disconnectMsg, CancellationToken cancellationToken)
{
	// Проверка, что клиенты созданы
	if (_restClient == null)
		throw new InvalidOperationException(LocalizedStrings.ConnectionNotOk);

	if (_socketClient == null)
		throw new InvalidOperationException(LocalizedStrings.ConnectionNotOk);

	// Освобождение ресурсов REST клиента
	_restClient.Dispose();
	_restClient = null;

	// Отключение WebSocket клиента
	_socketClient.Disconnect();

	// Отправка сообщения об отключении
	SendOutDisconnectMessage(true);
	return default;
}
```

Дополнительно адаптер предоставляет метод [MessageAdapter.ResetAsync](xref:StockSharp.Messages.MessageAdapter.ResetAsync(StockSharp.Messages.ResetMessage,System.Threading.CancellationToken)) для сброса состояния, который закрывает соединение и возвращает адаптер в исходное состояние:

```cs
public override ValueTask ResetAsync(ResetMessage resetMsg, CancellationToken cancellationToken)
{
	// Освобождение ресурсов REST клиента
	if (_restClient != null)
	{
		try
		{
			_restClient.Dispose();
		}
		catch (Exception ex)
		{
			SendOutError(ex);
		}

		_restClient = null;
	}

	// Отключение и очистка WebSocket клиента
	if (_socketClient != null)
	{
		try
		{
			UnsubscribePusherClient();
			_socketClient.Disconnect();
		}
		catch (Exception ex)
		{
			SendOutError(ex);
		}

		_socketClient = null;
	}

	// Освобождение ресурсов аутентификатора
	if (_authenticator != null)
	{
		try
		{
			_authenticator.Dispose();
		}
		catch (Exception ex)
		{
			SendOutError(ex);
		}

		_authenticator = null;
	}

	// Очистка дополнительных данных
	_candlesTransIds.Clear();

	// Отправка сообщения о сбросе
	SendOutMessage(new ResetMessage());
	return default;
}
```

### После завершения разработки

После того как коннектор реализован, его можно использовать двумя способами:

1. Опубликовать его в [StockSharp Store](https://stocksharp.ru/store) — платно или бесплатно. В этом случае установка на компьютеры пользователей будет выполняться автоматически через [Инсталлер](../../installer/setup.md).
2. Для личного использования достаточно скопировать собранный *.dll* файл коннектора в папку с программой (своей или нашей). При запуске приложение ищет доступные коннекторы в текущем каталоге по следующим правилам:

   - учитываются только файлы **.dll**, чьи имена начинаются с `StockSharp.`;
   - каждый такой файл проверяется на корректность как .NET‑сборка;
   - сборка загружается, и добавляются все типы, реализующие `IMessageAdapter`;
   - возникающие при поиске или загрузке ошибки фиксируются в журнале, не прерывая обработку. Чтобы увидеть сообщение об ошибке, откройте окно журнала приложения или соответствующий лог‑файл.

Этот документ описывает общие принципы работы адаптера, его создание и управление соединением с торговой системой. Следующие документы будут посвящены реализации функционала адаптера:

- [Поиск инструментов](creating_own_connector/instrument_lookup.md)
- [Работа с рыночными данными](creating_own_connector/market_data.md)
- [Запрос текущего состояния портфеля и заявок](creating_own_connector/portfolio_and_orders_state.md)
- [Работа с торговыми операциями](creating_own_connector/trading_operations.md)
- [Хранение настроек](creating_own_connector/settings.md)
- [Расширенные условия заявок](creating_own_connector/order_extended.md)
