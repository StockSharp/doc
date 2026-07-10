# Pesquisa de Instrumentos

A maioria dos conectores para bolsas de valores dos EUA (por exemplo, [Interactive Brokers](../connectors/stock_market/interactive_brokers.md), [PolygonIO](../connectors/stock_market/polygonio.md) e outros) não transfere todos os instrumentos disponíveis para o cliente depois de estabelecer uma ligação através do método [IConnector.Connect](xref:StockSharp.BusinessEntities.IConnector.Connect). Isto deve-se ao grande número de instrumentos negociados nas bolsas americanas e é feito para reduzir a carga nos servidores dos corretores e nas fontes de dados.

## Noções Básicas da Pesquisa de Instrumentos

Para pesquisar instrumentos no S#, é utilizado um mecanismo de subscrição, semelhante à recepção de dados de mercado. Esta abordagem permite utilizar código uniforme para todos os tipos de dados, incluindo instrumentos.

### Criar uma Subscrição para Pesquisa de Instrumentos

Para pesquisar instrumentos, tem de criar uma instância da classe [Subscription](xref:StockSharp.BusinessEntities.Subscription) com base na mensagem [SecurityLookupMessage](xref:StockSharp.Messages.SecurityLookupMessage), que contém parâmetros de filtragem:

```csharp
// Criar um objecto de filtro para pesquisa
var lookupMessage = new SecurityLookupMessage
{
	// Definir critérios de pesquisa
	SecurityId = new SecurityId
	{
		// Pesquisar por código do instrumento (pode utilizar uma máscara como "AAPL*")
		SecurityCode = "AAPL",
		// Opcionalmente, pode especificar o código da bolsa
		BoardCode = "NASDAQ"
	},
	// Pode especificar o tipo de instrumento
	SecurityType = SecurityTypes.Stock,
	// Definir ID da transacção
	TransactionId = Connector.TransactionIdGenerator.GetNextId()
};

// Criar uma subscrição para pesquisa de instrumentos
var subscription = new Subscription(lookupMessage);
```

### Parâmetros de Filtragem Possíveis

A mensagem [SecurityLookupMessage](xref:StockSharp.Messages.SecurityLookupMessage) permite definir os seguintes critérios de pesquisa:

- **SecurityId** — identificador do instrumento, contendo:
  - **SecurityCode** — código ou máscara do código do instrumento (por exemplo, "AAPL" ou "MS*")
  - **BoardCode** — código da bolsa (por exemplo, [ExchangeBoard.Nasdaq](xref:StockSharp.BusinessEntities.ExchangeBoard.Nasdaq))
- **SecurityType** — tipo de instrumento ([SecurityTypes.Stock](xref:StockSharp.Messages.SecurityTypes.Stock), [SecurityTypes.Future](xref:StockSharp.Messages.SecurityTypes.Future), etc.)
- **SecurityTypes** — matriz de tipos de instrumentos para pesquisa avançada
- **Currency** — moeda de negociação do instrumento
- **ExpiryDate** — data de vencimento (para derivados)
- **Strike** — preço de exercício (para opções)
- **OptionType** — tipo de opção (para opções)
- **Name** — nome do instrumento ou parte dele
- **Class** — classe do instrumento

### Processar Resultados da Pesquisa

Depois de criar a subscrição, tem de subscrever eventos para receber instrumentos e enviar o pedido:

```csharp
// Manipulador do evento de recepção de instrumento
private void OnSecurityReceived(Subscription subscription, Security security)
{
	if (subscription.SubscriptionMessage is not SecurityLookupMessage)
		return;
		
	Console.WriteLine($"Instrumento encontrado: {security.Id} - {security.Name}, Tipo: {security.Type}");
	
	// Aqui pode adicionar o instrumento a uma colecção ou executar outras acções
	Securities.Add(security);
}

// Manipulador do evento de conclusão da pesquisa
private void OnSubscriptionFinished(Subscription subscription)
{
	if (subscription.SubscriptionMessage is not SecurityLookupMessage)
		return;
		
	Console.WriteLine($"Pesquisa concluída. Instrumentos encontrados: {Securities.Count}");
}

// Manipulador de erros da subscrição
private void OnSubscriptionFailed(Subscription subscription, Exception error, bool isSubscribe)
{
	if (subscription.SubscriptionMessage is not SecurityLookupMessage)
		return;
		
	Console.WriteLine($"Erro na pesquisa de instrumentos: {error.Message}");
}

// Subscrever eventos
Connector.SecurityReceived += OnSecurityReceived;
Connector.SubscriptionFinished += OnSubscriptionFinished;
Connector.SubscriptionFailed += OnSubscriptionFailed;

// Enviar o pedido de pesquisa de instrumentos
Connector.Subscribe(subscription);
```

### Exemplo Completo de Pesquisa de Instrumentos

Abaixo está um exemplo completo de um método para pesquisar instrumentos:

```csharp
public void FindSecurities(string searchCode, SecurityTypes? securityType = null)
{
	// Criar um objecto para pesquisa de instrumentos
	var lookupMessage = new SecurityLookupMessage
	{
		SecurityId = new SecurityId
		{
			SecurityCode = searchCode,
			// Se precisar de pesquisar numa bolsa específica
			// BoardCode = ExchangeBoard.Nyse.Code,
		},
		SecurityType = securityType,
		TransactionId = Connector.TransactionIdGenerator.GetNextId()
	};
	
	// Criar uma subscrição
	var subscription = new Subscription(lookupMessage);
	
	// Limpar a colecção para resultados da pesquisa
	_searchResults.Clear();
	
	// Colecção temporária para acumular resultados
	var foundSecurities = new List<Security>();
	
	// Subscrição para receber instrumentos
	void OnSecurityReceived(Subscription sub, Security security)
	{
		if (sub != subscription)
			return;
			
		// Adicionar o instrumento encontrado à colecção
		foundSecurities.Add(security);
		Console.WriteLine($"Encontrado: {security.Id}, {security.Name}");
	}
	
	// Subscrição para conclusão da pesquisa
	void OnSubscriptionFinished(Subscription sub)
	{
		if (sub != subscription)
			return;
			
		// Copiar resultados para a colecção principal
		_searchResults.AddRange(foundSecurities);
		
		Console.WriteLine($"Pesquisa concluída. Instrumentos encontrados: {foundSecurities.Count}");
		
		// Anular subscrição dos eventos
		Connector.SecurityReceived -= OnSecurityReceived;
		Connector.SubscriptionFinished -= OnSubscriptionFinished;
		Connector.SubscriptionFailed -= OnSubscriptionFailed;
	}
	
	// Tratamento de erros da subscrição
	void OnSubscriptionFailed(Subscription sub, Exception error, bool isSubscribe)
	{
		if (sub != subscription)
			return;
			
		Console.WriteLine($"Erro na pesquisa de instrumentos: {error.Message}");
		
		// Anular subscrição dos eventos
		Connector.SecurityReceived -= OnSecurityReceived;
		Connector.SubscriptionFinished -= OnSubscriptionFinished;
		Connector.SubscriptionFailed -= OnSubscriptionFailed;
	}
	
	// Subscrever eventos
	Connector.SecurityReceived += OnSecurityReceived;
	Connector.SubscriptionFinished += OnSubscriptionFinished;
	Connector.SubscriptionFailed += OnSubscriptionFailed;
	
	// Enviar o pedido de pesquisa
	Connector.Subscribe(subscription);
}
```

### Exemplo de Utilização numa Aplicação WPF

Numa aplicação gráfica, a pesquisa de instrumentos é frequentemente chamada a partir de um manipulador de clique de botão:

```csharp
private void FindButton_Click(object sender, RoutedEventArgs e)
{
	// Obter critérios de pesquisa do campo de texto
	var searchText = SearchTextBox.Text;
	
	if (string.IsNullOrWhiteSpace(searchText))
	{
		MessageBox.Show("Enter a search criterion");
		return;
	}
	
	// Criar e enviar uma subscrição de pesquisa
	var lookupMessage = new SecurityLookupMessage
	{
		SecurityId = new SecurityId { SecurityCode = searchText },
		// Se for seleccionado um tipo na interface
		SecurityType = SecurityTypeComboBox.SelectedItem as SecurityTypes?
	};
	
	var subscription = new Subscription(lookupMessage);
	
	// Aqui pode mostrar um indicador de carregamento
	LoadingIndicator.Visibility = Visibility.Visible;
	
	// Enviar o pedido
	Connector.Subscribe(subscription);
}
```

## Utilizar SecurityLookupWindow

O StockSharp também fornece uma caixa de diálogo pronta a usar para pesquisa de instrumentos — [SecurityLookupWindow](xref:StockSharp.Xaml.SecurityLookupWindow):

```csharp
private void ShowSecurityLookupWindow_Click(object sender, RoutedEventArgs e)
{
	var lookupWindow = new SecurityLookupWindow
	{
		// Especificar a capacidade de pesquisar todos os instrumentos
		// (se o conector suportar esta função)
		ShowAllOption = Connector.Adapter.IsSupportSecuritiesLookupAll(),
		
		// Definir critérios de pesquisa iniciais
		CriteriaMessage = new SecurityLookupMessage
		{
			SecurityId = new SecurityId { SecurityCode = "AAPL" },
			SecurityType = SecurityTypes.Stock
		}
	};
	
	// Mostrar a janela como uma caixa de diálogo modal
	if (lookupWindow.ShowModal(this))
	{
		// Se o utilizador confirmou a selecção, enviar o pedido
		Connector.Subscribe(new Subscription(lookupWindow.CriteriaMessage));
	}
}
```

## Conclusão

O mecanismo de subscrição no StockSharp fornece uma forma unificada de obter dados, incluindo pesquisa de instrumentos. Isto permite utilizar a mesma abordagem para trabalhar com diferentes conectores e tipos de dados, o que simplifica significativamente o desenvolvimento de aplicações de negociação.
