# Formatos de Armazenamento

O StockSharp suporta dois formatos de armazenamento de dados de mercado definidos pela enumeração [StorageFormats](xref:StockSharp.Algo.Storages.StorageFormats): **Binary** e **CSV**. Cada formato tem as suas próprias vantagens e é adequado para diferentes cenários de utilização.

## Formato Binário

O formato binário é o principal formato de armazenamento de dados de alto desempenho no StockSharp. A implementação encontra-se na classe `BinaryMarketDataSerializer`, em `Algo/Storages/Binary/`.

### Funcionalidades

- **Compactação** -- os dados são serializados em forma binária com compressão, garantindo tamanhos de ficheiro mínimos.
- **Desempenho** -- a leitura e a escrita são significativamente mais rápidas em comparação com formatos de texto.
- **Metadados** -- a classe `BinaryMetaInfo` armazena informação auxiliar: primeiros e últimos preços, valores fracionários e carimbos de data/hora. Isto permite obter rapidamente informação geral sobre os dados sem ler o ficheiro completo.
- **Arquitetura preparada para compressão** -- o formato foi concebido de raiz para compressão eficiente em fluxo contínuo.

Os ficheiros binários têm a extensão `.bin`.

### Tipos de Dados Suportados

O formato binário suporta a serialização de todos os principais tipos de dados de mercado: velas, ticks (negócios), livros de ofertas (Level2), dados Level1, ordens e negócios próprios. Cada tipo tem um serializador especializado otimizado para a estrutura dos dados específicos.

## Formato CSV

O formato de texto CSV (Comma-Separated Values) é implementado na classe `CsvMarketDataSerializer`, localizada em `Algo/Storages/Csv/`.

### Funcionalidades

- **Legibilidade** -- os ficheiros podem ser abertos em qualquer editor de texto ou no Excel para análise visual dos dados.
- **Editabilidade** -- os dados podem ser corrigidos manualmente quando necessário.
- **Metadados** -- a classe `CsvMetaInfo` disponibiliza armazenamento de metadados com suporte de codificação. Contém a propriedade `IncrementalOnly` (suporte de dados incrementais) e `LastId` (identificador do último registo).
- **Maior tamanho de ficheiro** -- a representação em texto ocupa significativamente mais espaço em disco.
- **Processamento mais lento** -- a análise de dados em texto requer recursos computacionais adicionais.

Os ficheiros CSV têm a extensão `.csv`.

## Quando Usar Cada Formato

| Cenário | Formato Recomendado |
|----------|-------------------|
| Utilização em produção | Binary |
| Grandes volumes de dados | Binary |
| Crítico para desempenho | Binary |
| Depuração e diagnósticos | CSV |
| Análise visual de dados | CSV |
| Integração com ferramentas externas | CSV |
| Correção manual de dados | CSV |

## Organização dos Ficheiros em Disco

Os dados em disco são organizados de acordo com a seguinte estrutura de caminhos:

```
{root_folder}/{first_letter}/{instrument_identifier}/{yyyy_MM_dd}/{file_name}.{extension}
```

Em que a extensão é `.bin` para o formato binário ou `.csv` para o formato de texto. Esta organização hierárquica garante uma pesquisa rápida dos dados por instrumento e data.

Por exemplo, velas de 5 minutos para o instrumento AAPL@NASDAQ em 1 de abril de 2024, no formato binário, ficariam localizados num caminho como:

```
Storage/S/AAPL@NASDAQ/2024_04_01/candles_5m.bin
```

## Conversão Entre Formatos

O StockSharp permite carregar dados num formato e guardá-los noutro. Isto pode ser útil, por exemplo, para exportar dados binários para CSV para análise em ferramentas externas:

```cs
// Carregar do armazenamento binário
var binaryStorage = storageRegistry.GetTimeFrameCandleMessageStorage(
    securityId, TimeSpan.FromMinutes(5), StorageFormats.Binary);
var candles = await binaryStorage.LoadAsync(from, to).ToArrayAsync();

// Salvar no armazenamento CSV
var csvStorage = storageRegistry.GetTimeFrameCandleMessageStorage(
    securityId, TimeSpan.FromMinutes(5), StorageFormats.Csv);
await csvStorage.SaveAsync(candles);
```

## Exemplo de Código

O formato é selecionado ao criar um armazenamento através de [StorageRegistry](xref:StockSharp.Algo.Storages.StorageRegistry):

```cs
var storageRegistry = new StorageRegistry();

// Criar armazenamento de velas em formato binário
var binaryStorage = storageRegistry.GetTimeFrameCandleMessageStorage(
    securityId,
    TimeSpan.FromMinutes(5),
    StorageFormats.Binary);

// Criar armazenamento de velas em formato CSV
var csvStorage = storageRegistry.GetTimeFrameCandleMessageStorage(
    securityId,
    TimeSpan.FromMinutes(5),
    StorageFormats.Csv);

// Carregar dados
var from = new DateTime(2024, 1, 1);
var to = new DateTime(2024, 1, 31);
var candles = await binaryStorage.LoadAsync(from, to).ToArrayAsync();
```

O formato de armazenamento também pode ser especificado ao trabalhar com dados de ticks e livros de ofertas:

```cs
// Ticks em formato binário
var tickStorage = storageRegistry.GetTickMessageStorage(
    securityId, StorageFormats.Binary);

// Livros de ofertas em formato CSV
var depthStorage = storageRegistry.GetQuoteMessageStorage(
    securityId, StorageFormats.Csv);
```

## Ver Também

- [Trabalhar com a API](api.md)
- [Unidades de Armazenamento](drives.md)
