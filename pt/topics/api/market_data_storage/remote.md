# Trabalhar com Armazenamento Remoto

## Introdução

Além do armazenamento local, a API disponibiliza a capacidade de trabalhar com armazenamento remoto de dados de mercado. Isto é especialmente útil ao utilizar o Hydra em [modo servidor](../../hydra/server_mode/settings.md) ou ao ligar a um [servidor Hydra](../../hydra_server.md).

## Ligar ao Armazenamento Remoto

Para trabalhar com armazenamento remoto, use a classe [RemoteMarketDataDrive](xref:StockSharp.Algo.Storages.RemoteMarketDataDrive).

```cs
// Criar RemoteMarketDataDrive
var remoteDrive = new RemoteMarketDataDrive(RemoteMarketDataDrive.DefaultAddress, new FixMessageAdapter(new IncrementalIdGenerator()))
{
	Credentials = { Email = "hydra_user", Password = "hydra_user".To<SecureString>() }
};

// Este código cria uma instância de RemoteMarketDataDrive para conectar ao armazenamento remoto.
// Ele usa o endereço padrão e FixMessageAdapter para comunicação.
// As credenciais são definidas para autenticação.
```

## Carregar Informação de Instrumentos

Antes de carregar dados de mercado, é necessário obter informação sobre os instrumentos disponíveis.

```cs
// Carregar informações de instrumentos
var exchangeInfoProvider = new InMemoryExchangeInfoProvider();
remoteDrive.LookupSecurities(Extensions.LookupAllCriteriaMessage, registry.Securities,
	s => securityStorage.Save(s.ToSecurity(exchangeInfoProvider), false), () => false,
	(c, t) => Console.WriteLine($"Transferido [{c}]/[{t}]"));

var securities = securityStorage.LookupAll();

// Este código carrega informações sobre todos os instrumentos disponíveis do armazenamento remoto.
// Os instrumentos carregados são salvos no armazenamento local e exibidos no console.
```

## Carregar Dados de Mercado

Depois de obter a informação dos instrumentos, pode avançar para o carregamento dos dados de mercado.

```cs
// Carregar dados de mercado
foreach (var dataType in remoteDrive.GetAvailableDataTypes(secId, format))
{
	var localStorage = storageRegistry.GetStorage(secId, dataType.MessageType, dataType.Arg, localDrive, format);
	var remoteStorage = remoteDrive.GetStorageDrive(secId, dataType, format);

// ... (código de carregamento de dados)
}

// Este loop percorre todos os tipos de dados disponíveis para o instrumento especificado.
// Para cada tipo de dados, é criado um armazenamento local e o armazenamento remoto é acedido.
```

## Guardar Dados Localmente

Os dados carregados podem ser guardados localmente para utilização posterior.

```cs
// Salvar dados localmente
foreach (var dateTime in dates)
{
	using (var stream = remoteStorage.LoadStream(dateTime))
	{
		if (stream == Stream.Null)
			continue;

		localStorage.Drive.SaveStream(dateTime, stream);
	}

// ... (código de saída de dados)
}

// Este código carrega dados de cada data do armazenamento remoto e os salva no armazenamento local.
```

## Usar Dados para Testes

Os dados carregados e guardados localmente podem ser usados para testar estratégias de negociação com o [HistoryEmulationConnector](xref:StockSharp.Algo.Testing.HistoryEmulationConnector).

```cs
// Usar HistoryEmulationConnector
var connector = new HistoryEmulationConnector(secProvider, new[] { pf }, new StorageRegistry { DefaultDrive = remoteDrive });
```

## Obter Intervalos de Datas Disponíveis

```cs
// Trabalhar com vários tipos de dados
foreach (var dataType in remoteDrive.GetAvailableDataTypes(secId, format))
{
// ... (código de processamento de dados)

	// Tratamento de erros e logging
	Console.WriteLine($"Remote {dataType}: {remoteStorage.Dates.FirstOrDefault()}-{remoteStorage.Dates.LastOrDefault()}");
	Console.WriteLine($"{dataType}={dateTime}");
}
```

## Conclusão

A funcionalidade de armazenamento remoto de dados de mercado da API fornece capacidades flexíveis para obter e usar dados históricos. Isto permite testar estratégias de negociação e analisar o mercado de forma eficiente usando conjuntos de dados extensos disponíveis através do [servidor Hydra](../../hydra_server.md).
