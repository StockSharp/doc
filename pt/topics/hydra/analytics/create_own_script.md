# Criar um Script

O **Analytics** permite criar os seus próprios scripts. Como exemplo, vamos analisar o **ChartDrawScript**, que demonstra as capacidades de desenho de gráficos:

```cs
namespace StockSharp.Algo.Analytics
{
	/// <summary>
	/// O script analítico, mostra as possibilidades de desenho de gráficos.
	/// </summary>
	public class ChartDrawScript : IAnalyticsScript
	{
		Task IAnalyticsScript.Run(ILogReceiver logs, IAnalyticsPanel panel, SecurityId[] securities, DateTime from, DateTime to, IStorageRegistry storage, IMarketDataDrive drive, StorageFormats format, DataType dataType, CancellationToken cancellationToken)
		{
			if (securities.Length == 0)
			{
				logs.LogWarning("No instruments.");
				return Task.CompletedTask;
			}

			var lineChart = panel.CreateChart<DateTimeOffset, decimal>();
			var histogramChart = panel.CreateChart<DateTimeOffset, decimal>();

			foreach (var security in securities)
			{
				// parar o cálculo se o utilizador cancelar a execução do script
				if (cancellationToken.IsCancellationRequested)
					break;

				var candlesSeries = new Dictionary<DateTimeOffset, decimal>();
				var volsSeries = new Dictionary<DateTimeOffset, decimal>();

				// obter o armazenamento de candles
				var candleStorage = storage.GetCandleMessageStorage(security, dataType, drive, format);

				foreach (var candle in candleStorage.Load(from, to))
				{
					// preencher séries
					candlesSeries[candle.OpenTime] = candle.ClosePrice;
					volsSeries[candle.OpenTime] = candle.TotalVolume;
				}

				// desenhar séries no gráfico como linha e histograma
				lineChart.Append($"{security} (close)", candlesSeries.Keys, candlesSeries.Values, DrawStyles.DashedLine);
				histogramChart.Append($"{security} (vol)", volsSeries.Keys, volsSeries.Values, DrawStyles.Histogram);
			}

			return Task.CompletedTask;
		}
	}
}

```

## Visão Geral

Este script foi concebido para desenhar gráficos com base nos dados de preço e volume de instrumentos financeiros ao longo de um período específico. Implementa a interface [IAnalyticsScript](xref:StockSharp.Algo.Analytics.IAnalyticsScript), que define um contrato para qualquer script analítico que possa ser executado no programa **Hydra**.

## Interface `IAnalyticsScript`

A interface [IAnalyticsScript](xref:StockSharp.Algo.Analytics.IAnalyticsScript) garante que qualquer script analítico que a implemente terá o método [Run](xref:StockSharp.Algo.Analytics.IAnalyticsScript.Run(Ecng.Logging.ILogReceiver,StockSharp.Algo.Analytics.IAnalyticsPanel,StockSharp.Messages.SecurityId[],System.DateTime,System.DateTime,StockSharp.Algo.Storages.IStorageRegistry,StockSharp.Algo.Storages.IMarketDataDrive,StockSharp.Algo.Storages.StorageFormats,StockSharp.Messages.DataType,System.Threading.CancellationToken)), necessário para executar as operações analíticas do script.

### Método `Run`

O método [Run](xref:StockSharp.Algo.Analytics.IAnalyticsScript.Run(Ecng.Logging.ILogReceiver,StockSharp.Algo.Analytics.IAnalyticsPanel,StockSharp.Messages.SecurityId[],System.DateTime,System.DateTime,StockSharp.Algo.Storages.IStorageRegistry,StockSharp.Algo.Storages.IMarketDataDrive,StockSharp.Algo.Storages.StorageFormats,StockSharp.Messages.DataType,System.Threading.CancellationToken)) é o ponto de entrada de um script analítico, onde são efetuados o processamento real dos dados e as operações analíticas.

#### Parâmetros:

- `logs`: Recebe uma instância de [ILogReceiver](xref:Ecng.Logging.ILogReceiver) para registo no script.
- `panel`: Fornece [IAnalyticsPanel](xref:StockSharp.Algo.Analytics.IAnalyticsPanel), que é um elemento da interface de utilizador para desenhar gráficos e apresentar resultados.
- `securities`: Uma matriz de [SecurityId](xref:StockSharp.Messages.SecurityId) que identifica os instrumentos financeiros para análise.
- `from`: A data de início do intervalo de dados para análise.
- `to`: A data de fim do intervalo de dados para análise.
- `storage`: Uma instância de [IStorageRegistry](xref:StockSharp.Algo.Storages.IStorageRegistry) que permite aceder ao armazenamento de dados de mercado.
- `drive`: Representa [IMarketDataDrive](xref:StockSharp.Algo.Storages.IMarketDataDrive) para especificar a localização do armazenamento de dados de mercado.
- `format`: Um valor [StorageFormats](xref:StockSharp.Algo.Storages.StorageFormats) que indica o formato dos dados de mercado.
- `dataType`: [DataType](xref:StockSharp.Messages.DataType) que descreve o tipo de dados de mercado solicitado e os seus parâmetros (por exemplo, o período temporal da candle).
- `cancellationToken`: [CancellationToken](xref:System.Threading.CancellationToken) que monitoriza pedidos de cancelamento.

#### Devolve:

- [Task](xref:System.Threading.Tasks.Task), que representa a operação assíncrona do script analítico.

## Detalhes de Implementação

A classe `ChartDrawScript` processa especificamente dados de mercado para cada security fornecida. Cria dois tipos de gráficos: um gráfico de linhas para os preços de fecho e um histograma para os dados de volume.

### Principais Etapas de Processamento:

1. Verificar a presença de instrumentos a processar. Se nenhum estiver disponível, registar um aviso e concluir a tarefa.
2. Criar um gráfico de linhas e um histograma usando o método [IAnalyticsPanel.CreateChart](xref:StockSharp.Algo.Analytics.IAnalyticsPanel.CreateChart``2).
3. Iterar por cada security e verificar pedidos de cancelamento.
4. Obter o armazenamento de candles usando o método `storage.GetCandleMessageStorage`.
5. Carregar dados de candles dentro do intervalo de datas especificado.
6. Preencher dicionários com dados de séries temporais de abertura, preços de fecho correspondentes e volumes totais.
7. Desenhar os dados das séries nos gráficos usando os métodos `lineChart.Append` e `histogramChart.Append`.

O script utiliza estilos, como [DrawStyles.DashedLine](xref:Ecng.Drawing.DrawStyles.DashedLine) para o gráfico de linhas e [DrawStyles.Histogram](xref:Ecng.Drawing.DrawStyles.Histogram) para o histograma, para distinguir visualmente diferentes apresentações de dados.

Ao implementar [IAnalyticsScript](xref:StockSharp.Algo.Analytics.IAnalyticsScript), a classe `ChartDrawScript` permite integrar uma abordagem para executar scripts analíticos personalizáveis, tornando-a uma ferramenta versátil para traders e analistas que utilizam a plataforma StockSharp.

## Resultado da Execução

![Criar um Script](../../../images/hydra_analytics_chart.png)
