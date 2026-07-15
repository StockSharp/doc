# Importação de dados

[S#](../api.md) implementa um subsistema de importação de dados de mercado a partir de ficheiros CSV. As classes principais estão localizadas no namespace `StockSharp.Algo.Import`.

## CsvParser — Analisador base

A classe [CsvParser](xref:StockSharp.Algo.Import.CsvParser) executa a análise de ficheiros CSV e converte linhas em mensagens [S#](../api.md).

- **Construtor**: `(DataType dataType, IEnumerable<FieldMapping> fields)`
- **Separador de colunas** — separador de colunas (predefinição `","`).
- **Separador de linhas** — separador de linhas (CRLF por predefinição).
- **Linhas de cabeçalho ignoradas** — número de linhas a ignorar desde o início do ficheiro (predefinição `0`).
- **Ignorar instrumentos sem identificador** — ignorar linhas com instrumentos não reconhecidos (predefinição `true`).
- **Parse(Stream)** — método de análise, devolve `IAsyncEnumerable<Message>`.

```cs
var fields = FieldMappingRegistry.CreateFields(DataType.Ticks);
var parser = new CsvParser(DataType.Ticks, fields)
{
    ColumnSeparator = ";",
    SkipFromHeader = 1
};

await using var stream = File.OpenRead("trades.csv");

await foreach (var msg in parser.Parse(stream))
{
    // processar cada mensagem
}
```

## CsvImporter — Importação com gravação no armazenamento

A classe [CsvImporter](xref:StockSharp.Algo.Import.CsvImporter) expande [CsvParser](xref:StockSharp.Algo.Import.CsvParser), adicionando a capacidade de guardar automaticamente dados num armazenamento de dados de mercado.

- **Construtor**: `(DataType dataType, IEnumerable<FieldMapping> fields, ISecurityStorage securityStorage, IExchangeInfoProvider exchangeInfoProvider, Func<SecurityId, IMarketDataStorage> getStorage)`
- **Import(Stream, Action\<int\> progress, CancellationToken)** — executa a importação e devolve `ValueTask<(int count, DateTime? lastTime)>`.
- **Atualizar instrumentos duplicados** — se deve atualizar instrumentos duplicados (predefinição `false`).
- **Instrumento atualizado** — evento gerado quando um instrumento é atualizado.

```cs
var fields = FieldMappingRegistry.CreateFields(DataType.Ticks);

var importer = new CsvImporter(
    DataType.Ticks,
    fields,
    securityStorage,
    exchangeInfoProvider,
    secId => storageRegistry.GetStorage(secId, DataType.Ticks))
{
    ColumnSeparator = ";",
    SkipFromHeader = 1
};

await using var stream = File.OpenRead("trades.csv");
var (count, lastTime) = await importer.Import(
    stream,
    p => Console.WriteLine($"Progresso: {p}%"),
    token);

Console.WriteLine($"{count} registos importados, último: {lastTime}");
```

## FieldMapping — Descrições de campos

A classe [FieldMapping](xref:StockSharp.Algo.Import.FieldMapping) descreve o mapeamento entre colunas do ficheiro CSV e propriedades das mensagens.

Propriedades principais:

- **Nome** — nome do campo na mensagem.
- **Nome de apresentação** — nome apresentado.
- **Tipo** — tipo de valor.
- **Ordem** — índice da coluna no ficheiro (a começar em 0).
- **Obrigatório** — se o campo é obrigatório.
- **Formato** — formato de análise (por exemplo, formato de data).
- **Valor predefinido** — valor predefinido.
- **Zero como nulo** — se deve interpretar valores zero como `null`.

Para transformações de valores personalizadas, use [FieldMappingValue](xref:StockSharp.Algo.Import.FieldMappingValue). Por exemplo, pode definir um mapeamento de valores de texto para enumerações:

```cs
var sideField = fields.First(f => f.Name == "Side");
sideField.Order = 3;
sideField.Values.Add(new FieldMappingValue
{
    ValueFrom = "B",
    ValueTo = Sides.Buy
});
sideField.Values.Add(new FieldMappingValue
{
    ValueFrom = "S",
    ValueTo = Sides.Sell
});
```

## FieldMappingRegistry — Registo de campos padrão

A classe estática [FieldMappingRegistry](xref:StockSharp.Algo.Import.FieldMappingRegistry) fornece um método para criar um conjunto padrão de campos:

- **CreateFields(DataType)** — devolve uma lista de [FieldMapping](xref:StockSharp.Algo.Import.FieldMapping) para o tipo de dados especificado.

Tipos de dados suportados: ticks, velas, livros de ofertas, Level1, registo de ordens, transações, instrumentos, notícias e posições.

## ImportSettings — Definições de importação

A classe [ImportSettings](xref:StockSharp.Algo.Import.ImportSettings) combina todos os parâmetros de importação num único objeto de configuração:

- **Tipo de dados** — o tipo de dados a importar.
- **Nome do ficheiro** — caminho do ficheiro.
- **Diretório** — diretório para pesquisa de ficheiros.
- **Máscara do ficheiro** — máscara de pesquisa de ficheiros (por exemplo, `*.csv`).
- **Separador de colunas** — separador de colunas.
- **Linhas de cabeçalho ignoradas** — número de linhas a ignorar.
- **Campos selecionados** — campos selecionados para importação.
- **Atualizar instrumentos duplicados** — se deve atualizar instrumentos duplicados.

Métodos auxiliares:

- **GetFiles(IFileSystem)** — obter uma lista de ficheiros que correspondem à máscara.
- **FillParser(CsvParser)** — preencher as definições do analisador.
- **FillImporter(CsvImporter)** — preencher as definições do importador.

```cs
var settings = new ImportSettings
{
    DataType = DataType.Ticks,
    FileName = "trades.csv",
    ColumnSeparator = ";",
    SkipFromHeader = 1
};

var fields = FieldMappingRegistry.CreateFields(settings.DataType);
// configurar ordem das colunas
fields[0].Order = 0; // SecurityId
fields[1].Order = 1; // Date
fields[2].Order = 2; // Price
fields[3].Order = 3; // Volume

var importer = new CsvImporter(
    settings.DataType,
    fields,
    securityStorage,
    exchangeInfoProvider,
    secId => storageRegistry.GetStorage(secId, settings.DataType));

settings.FillImporter(importer);

await using var stream = File.OpenRead(settings.FileName);
var (count, lastTime) = await importer.Import(stream, p => { }, token);
```

## Ver também

[Exportação de dados](export.md)

[Armazenamento de dados](market_data_storage.md)
