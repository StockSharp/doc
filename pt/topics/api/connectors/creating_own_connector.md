# Criando Seu Próprio Conector

O mecanismo de mensagens é uma camada lógica interna da arquitetura [StockSharp](https://github.com/StockSharp/StockSharp), que fornece a interação entre vários elementos da plataforma usando um protocolo padrão.

Existem duas classes principais:

- [Message](xref:StockSharp.Messages.Message) - uma mensagem que carrega informações.
- [AsyncMessageAdapter](xref:StockSharp.Messages.AsyncMessageAdapter) - um adaptador de mensagens (=conversor).

Uma **mensagem** atua como um agente que transmite informações. As mensagens têm seu próprio tipo [MessageTypes](xref:StockSharp.Messages.MessageTypes). Cada tipo de mensagem corresponde a uma classe específica. Por sua vez, todas as classes de mensagens herdam da classe abstrata [Message](xref:StockSharp.Messages.Message), que dota os descendentes de propriedades como o tipo de mensagem [Message.Type](xref:StockSharp.Messages.Message.Type) e [Message.LocalTime](xref:StockSharp.Messages.Message.LocalTime) - a hora local de criação/recebimento da mensagem.

As mensagens podem ser *de entrada* e *de saída*:

- Mensagens *de entrada* - mensagens que são enviadas para um sistema externo. Geralmente, são comandos gerados pelo programa, por exemplo, a mensagem [ConnectMessage](xref:StockSharp.Messages.ConnectMessage) - um comando que solicita uma conexão com o servidor.
- Mensagens *de saída* - mensagens provenientes de um sistema externo. São mensagens que transmitem informações sobre dados de mercado, transações, portfólios, eventos de conexão, etc. Por exemplo, a mensagem [QuoteChangeMessage](xref:StockSharp.Messages.QuoteChangeMessage) transmite informações sobre mudanças no livro de ofertas.

O **adaptador de mensagens** desempenha o papel de intermediário entre o sistema de negociação e o programa. Para cada tipo de conector, existe uma classe adaptadora separada que herda da classe abstrata [AsyncMessageAdapter](xref:StockSharp.Messages.AsyncMessageAdapter).

O adaptador executa duas funções principais:

1. Converte mensagens de entrada em comandos de um sistema de negociação específico.
2. Converte informações recebidas do sistema de negociação (conexão, dados de mercado, transações, etc.) em mensagens de saída.

Abaixo está uma descrição do processo de criação de seu próprio adaptador para o [Coinbase](https://github.com/StockSharp/StockSharp/tree/master/Connectors/Coinbase) (todos os conectores com código-fonte estão disponíveis no [repositório StockSharp](https://github.com/StockSharp/StockSharp/tree/master/Connectors) e são fornecidos como um tutorial).

## Exemplo de Criação de um Adaptador de Mensagens Coinbase

### 1. Criando uma Classe de Adaptador

Primeiro, criamos a classe adaptadora de mensagens **CoinbaseMessageAdapter**, herdada da classe abstrata [AsyncMessageAdapter](xref:StockSharp.Messages.AsyncMessageAdapter).

```cs
public partial class CoinbaseMessageAdapter : AsyncMessageAdapter
{
	private Authenticator _authenticator;
	private HttpClient _restClient;
	private SocketClient _socketClient;

	// Other adapter fields and properties
}
```

### 2. Construtor do Adaptador

No construtor do adaptador, você precisa executar as seguintes ações:

1. Passar o gerador de ID de transação que será usado para criar IDs de mensagem.

2. Especificar os tipos de mensagem suportados usando os métodos:
 - [AddMarketDataSupport](xref:StockSharp.Messages.Extensions.AddMarketDataSupport(StockSharp.Messages.MessageAdapter)) - suporte para mensagens de assinatura de dados de mercado.
 - [AddTransactionalSupport](xref:StockSharp.Messages.Extensions.AddTransactionalSupport(StockSharp.Messages.MessageAdapter)) - suporte para mensagens transacionais.

3. Especificar os tipos específicos de dados de mercado suportados pelo adaptador usando o método [AddSupportedMarketDataType](xref:StockSharp.Messages.Extensions.AddSupportedMarketDataType(StockSharp.Messages.MessageAdapter,StockSharp.Messages.DataType)).

```cs
public CoinbaseMessageAdapter(IdGenerator transactionIdGenerator)
	: base(transactionIdGenerator)
{
	HeartbeatInterval = TimeSpan.FromSeconds(5);

	// Add support for market data and transactions
	this.AddMarketDataSupport();
	this.AddTransactionalSupport();

	// Remove unsupported message types
	this.RemoveSupportedMessage(MessageTypes.Portfolio);
	this.RemoveSupportedMessage(MessageTypes.OrderGroupCancel);

	// Add supported market data types
	this.AddSupportedMarketDataType(DataType.Ticks);
	this.AddSupportedMarketDataType(DataType.MarketDepth);
	this.AddSupportedMarketDataType(DataType.Level1);
	this.AddSupportedMarketDataType(DataType.CandleTimeFrame);
}
```

### 3. Conectando e Desconectando o Adaptador

Para conectar o adaptador ao sistema de negociação, o método [AsyncMessageAdapter.ConnectAsync](xref:StockSharp.Messages.AsyncMessageAdapter.ConnectAsync(StockSharp.Messages.ConnectMessage,System.Threading.CancellationToken)) é chamado. A ele é passada a mensagem de entrada [ConnectMessage](xref:StockSharp.Messages.ConnectMessage). Se a conexão for bem-sucedida, o adaptador envia uma mensagem de saída [ConnectMessage](xref:StockSharp.Messages.ConnectMessage).

```cs
public override async ValueTask ConnectAsync(ConnectMessage connectMsg, CancellationToken cancellationToken)
{
	// Check the presence of keys for transactional mode
	if (this.IsTransactional())
	{
		if (Key.IsEmpty())
			throw new InvalidOperationException(LocalizedStrings.KeyNotSpecified);

		if (Secret.IsEmpty())
			throw new InvalidOperationException(LocalizedStrings.SecretNotSpecified);
	}

	// Initialize the authenticator
	_authenticator = new(this.IsTransactional(), Key, Secret, Passphrase);

	// Check that clients are not yet created
	if (_restClient != null)
		throw new InvalidOperationException(LocalizedStrings.NotDisconnectPrevTime);

	if (_socketClient != null)
		throw new InvalidOperationException(LocalizedStrings.NotDisconnectPrevTime);

	// Create REST client
	_restClient = new(_authenticator) { Parent = this };

	// Create and configure WebSocket client
	_socketClient = new(_authenticator, ReConnectionSettings.ReAttemptCount) { Parent = this };
	SubscribePusherClient();

	// Connect WebSocket client
	await _socketClient.Connect(cancellationToken);

	// Send successful connection message
	SendOutMessage(new ConnectMessage());
}
```

Para desconectar o adaptador do sistema de negociação, o método [AsyncMessageAdapter.DisconnectAsync](xref:StockSharp.Messages.AsyncMessageAdapter.DisconnectAsync(StockSharp.Messages.DisconnectMessage,System.Threading.CancellationToken)) é chamado. Se a desconexão for bem-sucedida, o adaptador envia uma mensagem de saída [DisconnectMessage](xref:StockSharp.Messages.DisconnectMessage).

```cs
public override ValueTask DisconnectAsync(DisconnectMessage disconnectMsg, CancellationToken cancellationToken)
{
	// Check that clients are created
	if (_restClient == null)
		throw new InvalidOperationException(LocalizedStrings.ConnectionNotOk);

	if (_socketClient == null)
		throw new InvalidOperationException(LocalizedStrings.ConnectionNotOk);

	// Free REST client resources
	_restClient.Dispose();
	_restClient = null;

	// Disconnect WebSocket client
	_socketClient.Disconnect();

	// Send disconnection message
	SendOutDisconnectMessage(true);
	return default;
}
```

Além disso, o adaptador fornece o método [AsyncMessageAdapter.ResetAsync](xref:StockSharp.Messages.AsyncMessageAdapter.ResetAsync(StockSharp.Messages.ResetMessage,System.Threading.CancellationToken)) para redefinir o estado, que fecha a conexão e retorna o adaptador ao seu estado inicial.

```cs
public override ValueTask ResetAsync(ResetMessage resetMsg, CancellationToken cancellationToken)
{
	// Free REST client resources
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

	// Disconnect and clear WebSocket client
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

	// Free authenticator resources
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

	// Clear additional data
	_candlesTransIds.Clear();

	// Send reset message
	SendOutMessage(new ResetMessage());
	return default;
}
```

### Após a Conclusão do Desenvolvimento

Assim que o conector estiver implementado, existem duas opções para usá-lo:

1. Publicá-lo na [StockSharp Store](https://stocksharp.com/store) como um produto pago ou gratuito. Nesse caso, os usuários instalam o conector automaticamente através do [Installer](../../installer/setup.md).
2. Para uso pessoal, copie o arquivo *.dll* do conector compilado para a pasta de sua aplicação (ou qualquer produto StockSharp). Na inicialização, a aplicação verifica o diretório atual em busca de adaptadores usando os seguintes critérios:

   - Somente arquivos com a extensão **.dll** cujos nomes comecem com `StockSharp.` são considerados.
   - Cada arquivo restante é verificado para garantir que seja um assembly .NET válido.
   - O assembly é carregado e todos os tipos que implementam `IMessageAdapter` são coletados.
   - Quaisquer erros encontrados durante a verificação ou o carregamento são gravados no log e não interrompem a busca. Se o carregamento falhar, abra a janela de log da aplicação ou o arquivo de log para ver os detalhes do erro.

Este documento descreve os princípios gerais de funcionamento do adaptador, sua criação e o gerenciamento da conexão com o sistema de negociação. Os documentos a seguir serão dedicados à implementação da funcionalidade do adaptador:

- [Busca de Instrumentos](creating_own_connector/instrument_lookup.md)
- [Trabalhando com Dados de Mercado](creating_own_connector/market_data.md)
- [Solicitando o Estado Atual do Portfólio e das Ordens](creating_own_connector/portfolio_and_orders_state.md)
- [Trabalhando com Operações de Negociação](creating_own_connector/trading_operations.md)
- [Armazenando Configurações](creating_own_connector/settings.md)
- [Condições de Ordem Estendidas](creating_own_connector/order_extended.md)
