# Creación de un script

**Analítica** permite crear sus propios scripts. Como ejemplo, revisemos **ChartDrawScript**, que demuestra las capacidades de dibujo de gráficos:

```cs
namespace StockSharp.Algo.Analytics
{
	/// <summary>
	/// Script analítico que muestra las posibilidades de dibujo de gráficos.
	/// </summary>
	public class ChartDrawScript : IAnalyticsScript
	{
		Task IAnalyticsScript.Run(ILogReceiver logs, IAnalyticsPanel panel, SecurityId[] securities, DateTime from, DateTime to, IStorageRegistry storage, IMarketDataDrive drive, StorageFormats format, DataType dataType, CancellationToken cancellationToken)
		{
			if (securities.Length == 0)
			{
				logs.LogWarning("No hay instrumentos.");
				return Task.CompletedTask;
			}

			var lineChart = panel.CreateChart<DateTimeOffset, decimal>();
			var histogramChart = panel.CreateChart<DateTimeOffset, decimal>();

			foreach (var security in securities)
			{
				// detener el cálculo si el usuario cancela la ejecución del script
				if (cancellationToken.IsCancellationRequested)
					break;

				var candlesSeries = new Dictionary<DateTimeOffset, decimal>();
				var volsSeries = new Dictionary<DateTimeOffset, decimal>();

				// obtener almacenamiento de velas
				var candleStorage = storage.GetCandleMessageStorage(security, dataType, drive, format);

				foreach (var candle in candleStorage.Load(from, to))
				{
					// rellenar series
					candlesSeries[candle.OpenTime] = candle.ClosePrice;
					volsSeries[candle.OpenTime] = candle.TotalVolume;
				}

				// dibujar series en el gráfico como línea e histograma
				lineChart.Append($"{security} (cierre)", candlesSeries.Keys, candlesSeries.Values, DrawStyles.DashedLine);
				histogramChart.Append($"{security} (volumen)", volsSeries.Keys, volsSeries.Values, DrawStyles.Histogram);
			}

			return Task.CompletedTask;
		}
	}
}

```

## Resumen

Este script está diseñado para dibujar gráficos basados en datos de precio y volumen de instrumentos financieros durante un período de tiempo específico. Implementa la interfaz [IAnalyticsScript](xref:StockSharp.Algo.Analytics.IAnalyticsScript), que define un contrato para cualquier script analítico que pueda ejecutarse en el programa **Hydra**.

## Interfaz `IAnalyticsScript`

La interfaz [IAnalyticsScript](xref:StockSharp.Algo.Analytics.IAnalyticsScript) garantiza que cualquier script analítico que la implemente tendrá el método [Run](xref:StockSharp.Algo.Analytics.IAnalyticsScript.Run(Ecng.Logging.ILogReceiver,StockSharp.Algo.Analytics.IAnalyticsPanel,StockSharp.Messages.SecurityId[],System.DateTime,System.DateTime,StockSharp.Algo.Storages.IStorageRegistry,StockSharp.Algo.Storages.IMarketDataDrive,StockSharp.Algo.Storages.StorageFormats,StockSharp.Messages.DataType,System.Threading.CancellationToken)), necesario para realizar las operaciones analíticas del script.

### Método `Run`

El método [Run](xref:StockSharp.Algo.Analytics.IAnalyticsScript.Run(Ecng.Logging.ILogReceiver,StockSharp.Algo.Analytics.IAnalyticsPanel,StockSharp.Messages.SecurityId[],System.DateTime,System.DateTime,StockSharp.Algo.Storages.IStorageRegistry,StockSharp.Algo.Storages.IMarketDataDrive,StockSharp.Algo.Storages.StorageFormats,StockSharp.Messages.DataType,System.Threading.CancellationToken)) es el punto de entrada de un script analítico, donde se realizan el procesamiento real de datos y las operaciones analíticas.

#### Parámetros:

- `logs`: recibe una instancia de [ILogReceiver](xref:Ecng.Logging.ILogReceiver) para registrar mensajes dentro del script.
- `panel`: proporciona [IAnalyticsPanel](xref:StockSharp.Algo.Analytics.IAnalyticsPanel), que es un elemento de interfaz de usuario para dibujar gráficos y mostrar resultados.
- `securities`: matriz de [SecurityId](xref:StockSharp.Messages.SecurityId) que identifica los instrumentos financieros para el análisis.
- `from`: fecha de inicio del rango de datos para análisis.
- `to`: fecha de fin del rango de datos para análisis.
- `storage`: instancia de [IStorageRegistry](xref:StockSharp.Algo.Storages.IStorageRegistry) que permite acceder al almacenamiento de datos de mercado.
- `drive`: representa [IMarketDataDrive](xref:StockSharp.Algo.Storages.IMarketDataDrive) para especificar la ubicación del almacenamiento de datos de mercado.
- `format`: valor [StorageFormats](xref:StockSharp.Algo.Storages.StorageFormats) que indica el formato de datos de mercado.
- `dataType`: [DataType](xref:StockSharp.Messages.DataType) que describe el tipo de datos de mercado solicitado y sus parámetros (por ejemplo, el marco temporal de las velas).
- `cancellationToken`: [CancellationToken](xref:System.Threading.CancellationToken) que monitorea solicitudes de cancelación.

#### Devuelve:

- [Task](xref:System.Threading.Tasks.Task), que representa la operación asíncrona del script analítico.

## Detalles de implementación

La clase `ChartDrawScript` procesa específicamente datos de mercado para cada instrumento proporcionado. Crea dos tipos de gráficos: un gráfico de líneas para precios de cierre y un histograma para datos de volumen.

### Etapas principales de procesamiento:

1. Comprobar la presencia de instrumentos para procesar. Si no hay ninguno disponible, registrar una advertencia y completar la tarea.
2. Crear un gráfico de líneas y un histograma usando el método [IAnalyticsPanel.CreateChart](xref:StockSharp.Algo.Analytics.IAnalyticsPanel.CreateChart``2).
3. Iterar por cada instrumento y comprobar solicitudes de cancelación.
4. Obtener el almacenamiento de velas usando el método `storage.GetCandleMessageStorage`.
5. Cargar datos de velas dentro del rango de fechas especificado.
6. Rellenar diccionarios con datos de series de tiempo de apertura, precios de cierre correspondientes y volúmenes totales.
7. Dibujar datos de series en gráficos usando los métodos `lineChart.Append` e `histogramChart.Append`.

El script utiliza estilos como [DrawStyles.DashedLine](xref:Ecng.Drawing.DrawStyles.DashedLine) para el gráfico de líneas y [DrawStyles.Histogram](xref:Ecng.Drawing.DrawStyles.Histogram) para el histograma, para distinguir visualmente distintas presentaciones de datos.

Al implementar [IAnalyticsScript](xref:StockSharp.Algo.Analytics.IAnalyticsScript), la clase `ChartDrawScript` permite integrar un enfoque para ejecutar scripts analíticos personalizables, convirtiéndola en una herramienta versátil para traders y analistas que usan la plataforma StockSharp.

## Resultado de ejecución

![Creación de un script](../../../images/hydra_analytics_chart.png)
