# Scripts de Análise

O [S#](../api.md) implementa um subsistema de scripts de análise que permite realizar análises arbitrárias de dados de mercado com visualização dos resultados. As classes estão localizadas no namespace `StockSharp.Algo.Analytics`.

## IAnalyticsScript — Interface Principal

A interface [IAnalyticsScript](xref:StockSharp.Algo.Analytics.IAnalyticsScript) define um único método:

```cs
Task Run(
    ILogReceiver logs,
    IAnalyticsPanel panel,
    SecurityId[] securities,
    DateTime from,
    DateTime to,
    IStorageRegistry storage,
    IMarketDataDrive drive,
    StorageFormats format,
    DataType dataType,
    CancellationToken cancellationToken);
```

Parâmetros:

- **logs** — receptor de logs para gerar mensagens de diagnóstico.
- **panel** — painel para exibir os resultados da análise.
- **securities** — array de instrumentos a analisar.
- **from** / **to** — intervalo de tempo.
- **storage** — registro de armazenamento de dados de mercado.
- **drive** — fonte de dados.
- **format** — formato de armazenamento de dados.
- **dataType** — tipo de dado a analisar.
- **cancellationToken** — token de cancelamento.

## IAnalyticsPanel — Painel de Resultados

A interface [IAnalyticsPanel](xref:StockSharp.Algo.Analytics.IAnalyticsPanel) fornece métodos para criar várias visualizações:

- **CreateGrid(params string[] columns)** — cria uma tabela [IAnalyticsGrid](xref:StockSharp.Algo.Analytics.IAnalyticsGrid) com as colunas especificadas.
- **CreateChart\<X, Y\>()** — cria um gráfico bidimensional [IAnalyticsChart](xref:StockSharp.Algo.Analytics.IAnalyticsChart`2).
- **CreateChart\<X, Y, Z\>()** — cria um gráfico tridimensional [IAnalyticsChart](xref:StockSharp.Algo.Analytics.IAnalyticsChart`3).
- **DrawHeatmap(string[] xTitles, string[] yTitles, double[,] data)** — desenha um mapa de calor.
- **Draw3D(string[] xTitles, string[] yTitles, data, xTitle, yTitle, zTitle)** — desenha uma visualização 3D.

## IAnalyticsChart — Gráficos

A interface [IAnalyticsChart](xref:StockSharp.Algo.Analytics.IAnalyticsChart`2) fornece um método para adicionar séries de dados:

```cs
void Append(string title, IEnumerable<X> xValues, IEnumerable<Y> yValues,
    DrawStyles style, Color? color = null);
```

Estilos de desenho disponíveis ([DrawStyles](xref:StockSharp.Algo.Analytics.DrawStyles)):

- **Line** — gráfico de linha.
- **DashedLine** — linha tracejada.
- **Histogram** — histograma.
- **Bubble** — gráfico de bolhas.

## IAnalyticsGrid — Tabelas

A interface [IAnalyticsGrid](xref:StockSharp.Algo.Analytics.IAnalyticsGrid) permite exibir dados tabulares:

- **SetSort(string column, bool ascending)** — define a ordenação por coluna.
- **SetRow(params object[] values)** — adiciona uma linha de dados.

## Scripts Integrados

O pacote `StockSharp.Algo.Analytics.CSharp` inclui scripts prontos:

- **IndicatorScript** — calcula e visualiza indicadores em um gráfico.
- **ChartDrawScript** — demonstra a construção de vários tipos de gráficos.
- **PriceVolumeScript** — analisa a distribuição de volume entre níveis de preço.

## Exemplo: Script de Análise Personalizado

Abaixo está um exemplo de script que carrega candles para uma lista de instrumentos e exibe os preços de fechamento em um gráfico de linha:

```cs
public class MyAnalyticsScript : IAnalyticsScript
{
    public async Task Run(ILogReceiver logs, IAnalyticsPanel panel,
        SecurityId[] securities, DateTime from, DateTime to,
        IStorageRegistry storage, IMarketDataDrive drive,
        StorageFormats format, DataType dataType,
        CancellationToken cancellationToken)
    {
        // create a two-dimensional chart
        var chart = panel.CreateChart<DateTime, decimal>();

        foreach (var secId in securities)
        {
            // get candle storage
            var candleStorage = storage.GetCandleMessageStorage(
                secId, dataType, drive, format);

            // load data for the period
            var candles = await candleStorage
                .LoadAsync(from, to)
                .WithCancellation(cancellationToken)
                .ToArrayAsync(cancellationToken);

            if (candles.Length == 0)
            {
                logs.AddWarningLog($"No data for {secId}");
                continue;
            }

            // add series to the chart
            chart.Append(secId.ToString(),
                candles.Select(c => c.OpenTime.UtcDateTime),
                candles.Select(c => c.ClosePrice),
                DrawStyles.Line);

            logs.AddInfoLog($"{secId}: loaded {candles.Length} candles");
        }
    }
}
```

## Exemplo: Tabela de Volume

```cs
public class VolumeTableScript : IAnalyticsScript
{
    public async Task Run(ILogReceiver logs, IAnalyticsPanel panel,
        SecurityId[] securities, DateTime from, DateTime to,
        IStorageRegistry storage, IMarketDataDrive drive,
        StorageFormats format, DataType dataType,
        CancellationToken cancellationToken)
    {
        var grid = panel.CreateGrid("Instrument", "Total Candles",
            "Total Volume", "Average Volume");
        grid.SetSort("Total Volume", false);

        foreach (var secId in securities)
        {
            var candleStorage = storage.GetCandleMessageStorage(
                secId, dataType, drive, format);

            var candles = await candleStorage
                .LoadAsync(from, to)
                .WithCancellation(cancellationToken)
                .ToArrayAsync(cancellationToken);

            if (candles.Length == 0)
                continue;

            var totalVolume = candles.Sum(c => c.TotalVolume);
            var avgVolume = totalVolume / candles.Length;

            grid.SetRow(secId.ToString(), candles.Length,
                totalVolume, avgVolume);
        }
    }
}
```

## Veja Também

[Indicadores](indicators.md)

[Armazenamento de Dados](market_data_storage.md)
