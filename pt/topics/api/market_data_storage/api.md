# Trabalhar com a API

## Preparação

Para trabalhar com dados históricos nos exemplos, é utilizado um pacote NuGet com amostras de dados históricos. Pode ser instalado a partir da [NuGet Gallery](https://www.nuget.org/packages/StockSharp.Samples.HistoryData). Este pacote fornece um conjunto de dados que pode ser utilizado para demonstrar o trabalho com o armazenamento.

Todos os códigos estão disponíveis no [repositório StockSharp](https://github.com/StockSharp/StockSharp/tree/master/Samples/03_Storage).

## Criar um Registo de Armazenamento

Para trabalhar com armazenamento de dados de mercado no StockSharp, é utilizada a classe [StorageRegistry](xref:StockSharp.Algo.Storages.StorageRegistry). Ao criar um objeto desta classe, pode definir o caminho para o armazenamento predefinido através da propriedade [StorageRegistry.DefaultDrive](xref:StockSharp.Algo.Storages.StorageRegistry.DefaultDrive) ou especificar uma pasta concreta para trabalhar com dados históricos utilizando [LocalMarketDataDrive](xref:StockSharp.Algo.Storages.LocalMarketDataDrive).

```cs
// Criar StorageRegistry com caminho predefinido
var storageRegistry = new StorageRegistry();
```

```cs
// Criar StorageRegistry com o caminho para dados do pacote NuGet
var pathHistory = Paths.HistoryDataPath; // caminho para dados do pacote NuGet
var localDrive = new LocalMarketDataDrive(pathHistory);
var storageRegistry = new StorageRegistry()
{
	DefaultDrive = localDrive,
};
```

## Obter Dados

Através de [StorageRegistry](xref:StockSharp.Algo.Storages.StorageRegistry), pode aceder a vários tipos de dados de mercado para o intervalo de tempo pretendido. Os métodos utilizados para isto são:

- [StorageRegistry.GetTimeFrameCandleMessageStorage](xref:StockSharp.Algo.Storages.StorageHelper.GetTimeFrameCandleMessageStorage(StockSharp.Algo.Storages.IStorageRegistry,StockSharp.Messages.SecurityId,System.TimeSpan,StockSharp.Algo.Storages.IMarketDataDrive,StockSharp.Algo.Storages.StorageFormats)) para velas
- [StorageRegistry.GetTickMessageStorage](xref:StockSharp.Algo.Storages.StorageRegistry.GetTickMessageStorage(StockSharp.Messages.SecurityId,StockSharp.Algo.Storages.IMarketDataDrive,StockSharp.Algo.Storages.StorageFormats)) para ticks
- [StorageRegistry.GetQuoteMessageStorage](xref:StockSharp.Algo.Storages.StorageRegistry.GetQuoteMessageStorage(StockSharp.Messages.SecurityId,StockSharp.Algo.Storages.IMarketDataDrive,StockSharp.Algo.Storages.StorageFormats,System.Boolean)) para livros de ofertas

Cada um destes métodos devolve o armazenamento correspondente, a partir do qual os dados podem ser carregados utilizando o método `LoadAsync`, especificando as datas de início e fim.

```cs
// Obter velas
var securityId = "AAPL@NASDAQ".ToSecurityId();
var candleStorage = storageRegistry.GetTimeFrameCandleMessageStorage(securityId, TimeSpan.FromMinutes(1), StorageFormats.Binary);
var candles = candleStorage.LoadAsync(new DateTime(2020, 4, 1), new DateTime(2020, 4, 2));

await foreach (var candle in candles)
{
	Console.WriteLine(candle);
}
```

```cs
// Obter ticks
var tradeStorage = storageRegistry.GetTickMessageStorage(securityId, StorageFormats.Binary);
var trades = tradeStorage.LoadAsync(new DateTime(2020, 4, 1), new DateTime(2020, 4, 2));

await foreach (var trade in trades)
{
	Console.WriteLine(trade);
}
```

```cs
// Obter livros de ofertas
var marketDepthStorage = storageRegistry.GetQuoteMessageStorage(securityId, StorageFormats.Binary);
var marketDepths = marketDepthStorage.LoadAsync(new DateTime(2020, 4, 1), new DateTime(2020, 4, 2));

await foreach (var marketDepth in marketDepths)
{
	Console.WriteLine(marketDepth);
}
```

## Guardar Dados

Para guardar novos dados no armazenamento existente, utilize o método `SaveAsync` do armazenamento correspondente. Isto permite complementar dados históricos com novos valores.

```cs
// Guardar novas velas
var newCandles = new List<CandleMessage>
{
	// Novos objetos CandleMessage são criados aqui
};
await candleStorage.SaveAsync(newCandles);
```

```cs
// Guardar novos ticks
var newTrades = new List<ExecutionMessage>
{
	// Novos objetos ExecutionMessage para ticks são criados aqui
};
await tradeStorage.SaveAsync(newTrades);
```

```cs
// Guardar novos livros de ofertas
var newMarketDepths = new List<QuoteChangeMessage>
{
	// Novos objetos QuoteChangeMessage para livros de ofertas são criados aqui
};
await marketDepthStorage.SaveAsync(newMarketDepths);
```

## Eliminar Dados

Para eliminar dados de um período específico, utilize o método `DeleteAsync` do armazenamento correspondente. Tenha cuidado ao eliminar dados do pacote de exemplo.

```cs
// Eliminar velas para o período especificado
await candleStorage.DeleteAsync(new DateTime(2020, 4, 1), new DateTime(2020, 4, 2));
```

```cs
// Eliminar ticks para o período especificado
await tradeStorage.DeleteAsync(new DateTime(2020, 4, 1), new DateTime(2020, 4, 2));
```

```cs
// Eliminar livros de ofertas para o período especificado
await marketDepthStorage.DeleteAsync(new DateTime(2020, 4, 1), new DateTime(2020, 4, 2));
```

Estas operações permitem gerir eficazmente dados históricos, quer sejam carregados através do [Hydra](../../hydra.md), fornecidos no pacote NuGet ou criados durante o funcionamento da sua aplicação.
