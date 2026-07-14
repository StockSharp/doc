# Creación de su Propio Conector

El mecanismo de mensajería es una capa lógica interna de la arquitectura de [StockSharp](https://github.com/StockSharp/StockSharp), que proporciona la interacción entre varios elementos de la plataforma utilizando un protocolo estándar.

Hay dos clases principales:

- [Message](xref:StockSharp.Messages.Message) - un mensaje que transporta información.
- [AsyncMessageAdapter](xref:StockSharp.Messages.AsyncMessageAdapter) - un adaptador de mensajes (=convertidor).

Un **mensaje** actúa como un agente que transmite información. Los mensajes tienen su propio tipo [MessageTypes](xref:StockSharp.Messages.MessageTypes). Cada tipo de mensaje corresponde a una clase específica. A su vez, todas las clases de mensajes heredan de la clase abstracta [Message](xref:StockSharp.Messages.Message), que dota a los descendientes de propiedades como el tipo de mensaje [Message.Type](xref:StockSharp.Messages.Message.Type) y [Message.LocalTime](xref:StockSharp.Messages.Message.LocalTime) - la hora local de creación/recepción del mensaje.

Los mensajes pueden ser *entrantes* y *salientes*:

- Mensajes *entrantes* - mensajes que se envían a un sistema externo. Por lo general, son comandos generados por el programa, por ejemplo, el mensaje [ConnectMessage](xref:StockSharp.Messages.ConnectMessage) - un comando que solicita una conexión al servidor.
- Mensajes *salientes* - mensajes que provienen de un sistema externo. Son mensajes que transmiten información sobre datos de mercado, transacciones, carteras, eventos de conexión, etc. Por ejemplo, el mensaje [QuoteChangeMessage](xref:StockSharp.Messages.QuoteChangeMessage) transmite información sobre los cambios en el libro de órdenes.

El **adaptador de mensajes** desempeña el papel de intermediario entre el sistema de negociación y el programa. Para cada tipo de conector, existe una clase adaptadora separada que hereda de la clase abstracta [AsyncMessageAdapter](xref:StockSharp.Messages.AsyncMessageAdapter).

El adaptador realiza dos funciones principales:

1. Convierte los mensajes entrantes en comandos de un sistema de negociación específico.
2. Convierte la información recibida del sistema de negociación (conexión, datos de mercado, transacciones, etc.) en mensajes salientes.

A continuación se describe el proceso de creación de su propio adaptador para [Coinbase](https://github.com/StockSharp/StockSharp/tree/master/Connectors/Coinbase) (todos los conectores con código fuente están disponibles en el [repositorio de StockSharp](https://github.com/StockSharp/StockSharp/tree/master/Connectors) y se proporcionan como tutorial).

## Ejemplo de Creación de un Adaptador de Mensajes de Coinbase

### 1. Creación de la Clase del Adaptador

Primero, creamos la clase del adaptador de mensajes **CoinbaseMessageAdapter**, heredada de la clase abstracta [AsyncMessageAdapter](xref:StockSharp.Messages.AsyncMessageAdapter).

```cs
public partial class CoinbaseMessageAdapter : AsyncMessageAdapter
{
	private Authenticator _authenticator;
	private HttpClient _restClient;
	private SocketClient _socketClient;

	// Otros campos y propiedades del adaptador
}
```

### 2. Constructor del Adaptador

En el constructor del adaptador, debe realizar las siguientes acciones:

1. Pasar el generador de ID de transacción que se utilizará para crear los ID de mensaje.

2. Especificar los tipos de mensajes admitidos utilizando los métodos:
 - [AddMarketDataSupport](xref:StockSharp.Messages.Extensions.AddMarketDataSupport(StockSharp.Messages.MessageAdapter)) - soporte para mensajes de suscripción a datos de mercado.
 - [AddTransactionalSupport](xref:StockSharp.Messages.Extensions.AddTransactionalSupport(StockSharp.Messages.MessageAdapter)) - soporte para mensajes transaccionales.

3. Especificar los tipos específicos de datos de mercado admitidos por el adaptador utilizando el método [AddSupportedMarketDataType](xref:StockSharp.Messages.Extensions.AddSupportedMarketDataType(StockSharp.Messages.MessageAdapter,StockSharp.Messages.DataType)).

```cs
public CoinbaseMessageAdapter(IdGenerator transactionIdGenerator)
	: base(transactionIdGenerator)
{
	HeartbeatInterval = TimeSpan.FromSeconds(5);

	// Añadir soporte para datos de mercado y transacciones
	this.AddMarketDataSupport();
	this.AddTransactionalSupport();

	// Eliminar tipos de mensaje no admitidos
	this.RemoveSupportedMessage(MessageTypes.Portfolio);
	this.RemoveSupportedMessage(MessageTypes.OrderGroupCancel);

	// Añadir tipos de datos de mercado admitidos
	this.AddSupportedMarketDataType(DataType.Ticks);
	this.AddSupportedMarketDataType(DataType.MarketDepth);
	this.AddSupportedMarketDataType(DataType.Level1);
	this.AddSupportedMarketDataType(DataType.CandleTimeFrame);
}
```

### 3. Conexión y Desconexión del Adaptador

Para conectar el adaptador al sistema de negociación, se llama al método [AsyncMessageAdapter.ConnectAsync](xref:StockSharp.Messages.AsyncMessageAdapter.ConnectAsync(StockSharp.Messages.ConnectMessage,System.Threading.CancellationToken)). Se le pasa el mensaje entrante [ConnectMessage](xref:StockSharp.Messages.ConnectMessage). Si la conexión se realiza correctamente, el adaptador envía un mensaje saliente [ConnectMessage](xref:StockSharp.Messages.ConnectMessage).

```cs
public override async ValueTask ConnectAsync(ConnectMessage connectMsg, CancellationToken cancellationToken)
{
	// Comprobar la presencia de claves para el modo transaccional
	if (this.IsTransactional())
	{
		if (Key.IsEmpty())
			throw new InvalidOperationException(LocalizedStrings.KeyNotSpecified);

		if (Secret.IsEmpty())
			throw new InvalidOperationException(LocalizedStrings.SecretNotSpecified);
	}

	// Inicializar autenticador
	_authenticator = new(this.IsTransactional(), Key, Secret, Passphrase);

	// Comprobar que los clientes aún no se han creado
	if (_restClient != null)
		throw new InvalidOperationException(LocalizedStrings.NotDisconnectPrevTime);

	if (_socketClient != null)
		throw new InvalidOperationException(LocalizedStrings.NotDisconnectPrevTime);

	// Crear cliente REST
	_restClient = new(_authenticator) { Parent = this };

	// Crear y configurar cliente WebSocket
	_socketClient = new(_authenticator, ReConnectionSettings.ReAttemptCount) { Parent = this };
	SubscribePusherClient();

	// Conectar cliente WebSocket
	await _socketClient.Connect(cancellationToken);

	// Enviar mensaje de conexión correcta
	SendOutMessage(new ConnectMessage());
}
```

Para desconectar el adaptador del sistema de negociación, se llama al método [AsyncMessageAdapter.DisconnectAsync](xref:StockSharp.Messages.AsyncMessageAdapter.DisconnectAsync(StockSharp.Messages.DisconnectMessage,System.Threading.CancellationToken)). Si la desconexión se realiza correctamente, el adaptador envía un mensaje saliente [DisconnectMessage](xref:StockSharp.Messages.DisconnectMessage).

```cs
public override ValueTask DisconnectAsync(DisconnectMessage disconnectMsg, CancellationToken cancellationToken)
{
	// Comprobar que los clientes se han creado
	if (_restClient == null)
		throw new InvalidOperationException(LocalizedStrings.ConnectionNotOk);

	if (_socketClient == null)
		throw new InvalidOperationException(LocalizedStrings.ConnectionNotOk);

	// Liberar recursos del cliente REST
	_restClient.Dispose();
	_restClient = null;

	// Desconectar cliente WebSocket
	_socketClient.Disconnect();

	// Enviar mensaje de desconexión
	SendOutDisconnectMessage(true);
	return default;
}
```

Además, el adaptador proporciona el método [AsyncMessageAdapter.ResetAsync](xref:StockSharp.Messages.AsyncMessageAdapter.ResetAsync(StockSharp.Messages.ResetMessage,System.Threading.CancellationToken)) para restablecer el estado, que cierra la conexión y devuelve el adaptador a su estado inicial.

```cs
public override ValueTask ResetAsync(ResetMessage resetMsg, CancellationToken cancellationToken)
{
	// Liberar recursos del cliente REST
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

	// Desconectar y limpiar cliente WebSocket
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

	// Liberar recursos del autenticador
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

	// Limpiar datos adicionales
	_candlesTransIds.Clear();

	// Enviar mensaje de reinicio
	SendOutMessage(new ResetMessage());
	return default;
}
```

### Después de Finalizar el Desarrollo

Una vez que el conector está implementado, hay dos opciones para su uso:

1. Publicarlo en la [Tienda de StockSharp](https://stocksharp.com/es/store) como un producto de pago o gratuito. En este caso, los usuarios instalan el conector automáticamente a través del [Instalador](../../installer/setup.md).
2. Para uso personal, copie el archivo *.dll* del conector compilado a la carpeta de su aplicación (o de cualquier producto de StockSharp). Al iniciarse, la aplicación escanea el directorio actual en busca de adaptadores utilizando los siguientes criterios:

   - Solo se consideran los archivos con la extensión **.dll** cuyos nombres comienzan con `StockSharp.`.
   - Cada archivo restante se verifica para asegurarse de que es un ensamblado .NET válido.
   - El ensamblado se carga y se recopilan todos los tipos que implementan `IMessageAdapter`.
   - Cualquier error encontrado durante el escaneo o la carga se escribe en el registro y no detiene la búsqueda. Si la carga falla, abra la ventana de registro de la aplicación o el archivo de registro para ver los detalles del error.

Este documento describe los principios generales del funcionamiento del adaptador, su creación y la gestión de la conexión con el sistema de negociación. Los siguientes documentos estarán dedicados a la implementación de la funcionalidad del adaptador:

- [Búsqueda de Instrumentos](creating_own_connector/instrument_lookup.md)
- [Trabajo con Datos de Mercado](creating_own_connector/market_data.md)
- [Solicitud del Estado Actual de la Cartera y las Órdenes](creating_own_connector/portfolio_and_orders_state.md)
- [Trabajo con Operaciones de Negociación](creating_own_connector/trading_operations.md)
- [Almacenamiento de configuraciones](creating_own_connector/settings.md)
- [Condiciones de Órdenes Extendidas](creating_own_connector/order_extended.md)
</content>
