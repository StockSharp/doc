# Cliente WebSocket

Ao desenvolver um conector para diversas exchanges, um componente importante é o cliente WebSocket, que fornece a obtenção de dados em tempo real. No StockSharp, para esse propósito, uma classe `SocketClient` é frequentemente criada ao desenvolver um conector, construída com base no `WebSocketClient`.

## Recursos do WebSocketClient

`WebSocketClient` é uma classe de sistema desenvolvida para a reconexão automática em caso de perda da conexão WebSocket. Seu código-fonte está disponível no [repositório Ecng](https://github.com/StockSharp/Ecng/blob/master/Net.SocketIO/WebSocketClient.cs).

## Estrutura do SocketClient

O `SocketClient` geralmente inclui os seguintes elementos-chave:

1. **Construtor**
  - Inicializa o `WebSocketClient` base
  - Configura os manipuladores de eventos

2. **Métodos de conexão e desconexão**
  - `Connect` / `ConnectAsync`
  - `Disconnect`

3. **Métodos de assinatura para diferentes tipos de dados**
  - Por exemplo, `SubscribeTrades`, `SubscribeOrderBook`

4. **Métodos de cancelamento de assinatura de dados**
  - Métodos correspondentes de cancelamento de assinatura para cada tipo de assinatura

5. **Manipuladores de eventos**
  - Para tratar diferentes tipos de mensagens recebidas

6. **Métodos auxiliares**
  - Para formar mensagens de assinatura/cancelamento de assinatura
  - Para processar os dados recebidos

```cs
class SocketClient : BaseLogReceiver
{
	private readonly WebSocketClient _client;
	private readonly Authenticator _authenticator;

	// Eventos para diferentes tipos de dados
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
		_client = new WebSocketClient(/* parameters */);
		_client.ReconnectAttempts = reconnectAttempts;
	}

	public ValueTask Connect(CancellationToken cancellationToken)
	{
		// Lógica de conexão
	}

	public void Disconnect()
	{
		// Lógica de desconexão
	}

	public ValueTask SubscribeTicker(string symbol, CancellationToken cancellationToken)
	{
		// Lógica de assinatura de ticker
	}

	public ValueTask UnSubscribeTicker(string symbol, CancellationToken cancellationToken)
	{
		// Lógica de cancelamento de assinatura de ticker
	}

	// Métodos semelhantes para outros tipos de subscrições (negócios, livro de ofertas, etc.)

	private void OnProcess(dynamic obj)
	{
		// Processar mensagens recebidas
	}

	// Métodos auxiliares
}
```

## Recomendações de Implementação

- Adapte a estrutura do `SocketClient` à exchange específica, levando em conta as particularidades de sua API.
- Use métodos assíncronos para um trabalho eficiente com o WebSocket.
- Implemente o tratamento de diferentes tipos de mensagens da exchange.
- Garanta o tratamento adequado de erros e a reconexão em caso de perda de conexão.

Lembre-se de que a implementação específica pode variar dependendo dos requisitos e das especificidades da API de uma exchange em particular.
