# WebSocket клиент

При разработке коннектора для ряда бирж важным компонентом является WebSocket клиент, который обеспечивает получение данных в реальном времени. В StockSharp для этой цели часто создается при разработке коннектора класс `SocketClient`, построенный на основе `WebSocketClient`.

## Особенности WebSocketClient

`WebSocketClient` - это системный класс, разработанный для автоматического переподключения в случае разрыва соединения по WebSocket. Его исходный код доступен в [репозитории Ecng](https://github.com/StockSharp/Ecng/blob/master/Net.SocketIO/WebSocketClient.cs).

## Структура SocketClient

`SocketClient` обычно включает следующие ключевые элементы:

1. **Конструктор**
  - Инициализирует базовый `WebSocketClient`
  - Устанавливает обработчики событий

2. **Методы подключения и отключения**
  - `Connect` / `ConnectAsync`
  - `Disconnect`

3. **Методы подписки на различные типы данных**
  - Например, `SubscribeTrades`, `SubscribeOrderBook`

4. **Методы отписки от данных**
  - Соответствующие методы отписки для каждого типа подписки

5. **Обработчики событий**
  - Для обработки различных типов входящих сообщений

6. **Вспомогательные методы**
  - Для формирования сообщений подписки/отписки
  - Для обработки полученных данных

```cs
class SocketClient : BaseLogReceiver
{
	private readonly WebSocketClient _client;
	private readonly Authenticator _authenticator;

	// События для различных типов данных
	public event Action<Heartbeat> HeartbeatReceived;
	public event Action<Ticker> TickerReceived;
	public event Action<Trade> TradeReceived;
	public event Action<string, string, IEnumerable<OrderBookChange>> OrderBookReceived;
	public event Action<Order> OrderReceived;
	public event Action<Exception> Error;
	public event Action Connected;
	public event Action<bool> Disconnected;

	public SocketClient(Authenticator authenticator, int reconnectAttempts)
	{
		_authenticator = authenticator;
		_client = new WebSocketClient(/* параметры */);
		_client.ReconnectAttempts = reconnectAttempts;
	}

	public ValueTask Connect(CancellationToken cancellationToken)
	{
		// Логика подключения
	}

	public void Disconnect()
	{
		// Логика отключения
	}

	public ValueTask SubscribeTicker(string symbol, CancellationToken cancellationToken)
	{
		// Логика подписки на тикеры
	}

	public ValueTask UnSubscribeTicker(string symbol, CancellationToken cancellationToken)
	{
		// Логика отписки от тикеров
	}

	// Аналогичные методы для других типов подписок (сделки, стакан и т.д.)

	private void OnProcess(dynamic obj)
	{
		// Обработка входящих сообщений
	}

	// Вспомогательные методы
}
```

## Рекомендации по реализации

- Адаптируйте структуру `SocketClient` под конкретную биржу, учитывая особенности её API.
- Используйте асинхронные методы для эффективной работы с WebSocket.
- Реализуйте обработку различных типов сообщений от биржи.
- Обеспечьте корректную обработку ошибок и переподключение при разрыве соединения.

Помните, что конкретная реализация может отличаться в зависимости от требований и особенностей API конкретной биржи.