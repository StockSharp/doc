# Indicadores

El script "Indicador" está destinado a demostrar el trabajo con indicadores de análisis técnico dentro de la plataforma StockSharp. Permite cargar datos históricos, aplicarles distintos indicadores y mostrar los resultados en un gráfico. Este enfoque ayuda a analizar tendencias del mercado y tomar decisiones de negociación informadas.

![Indicadores](../../../../images/hydra_analytics_indicator.png)

## Capacidades funcionales

El script proporciona la siguiente funcionalidad:

- **Carga de datos históricos**: seleccionar instrumentos financieros de interés y cargar sus datos históricos para un período especificado.
- **Aplicación de indicadores**: aplicar uno o varios indicadores de análisis técnico a los datos cargados.
- **Visualización**: mostrar datos y resultados de análisis usando indicadores en un gráfico, proporcionando una vista clara de la dinámica del mercado.

## Ejemplos de indicadores

El script puede trabajar con una amplia gama de indicadores, incluidos entre otros:

- **Medias móviles (MA)**: representan el precio medio durante un cierto período y ayudan a identificar tendencias.
- **Índice de fuerza relativa (RSI)**: evalúa la magnitud y velocidad de los cambios de precio, ayudando a identificar condiciones de sobrecompra o sobreventa.
- **Bandas de Bollinger (BB)**: muestran el rango y la volatilidad de precios, basándose en medias móviles y desviaciones estándar.

## Aplicación en negociación y análisis

El uso de indicadores de análisis técnico mediante este script permite:

- **Identificar tendencias**: detectar la dirección de movimiento del mercado para planificar estrategias de entrada y salida.
- **Identificar puntos de reversión**: determinar momentos en los que la tendencia del mercado puede cambiar de dirección.
- **Analizar volatilidad**: evaluar el nivel de inestabilidad de precios para adaptar estrategias a las condiciones del mercado.

## Implementación en el script

Para trabajar con el script, deben realizarse los siguientes pasos:

1. **Seleccionar instrumento y período**: determinar valores y marco temporal para el análisis.
2. **Aplicar indicadores**: elegir y configurar parámetros para los indicadores que se aplicarán a los datos.
3. **Mostrar resultados**: visualizar datos históricos e indicadores en un gráfico para su análisis.

El script "Indicador" proporciona una herramienta potente para el análisis profundo de mercados financieros, permitiendo a operadores y analistas usar estos indicadores para desarrollar estrategias de negociación eficaces.

## Código del script en C#

```cs
namespace StockSharp.Algo.Analytics
{
	/// <summary>
	/// Script analítico que usa el indicador ROC.
	/// </summary>
	public class IndicatorScript : IAnalyticsScript
	{
		Task IAnalyticsScript.Run(ILogReceiver logs, IAnalyticsPanel panel, SecurityId[] securities, DateTime from, DateTime to, IStorageRegistry storage, IMarketDataDrive drive, StorageFormats format, DataType dataType, CancellationToken cancellationToken)
		{
			if (securities.Length == 0)
			{
				logs.LogWarning("No hay instrumentos.");
				return Task.CompletedTask;
			}

			// crear 2 paneles para velas y series de indicador
			var candleChart = panel.CreateChart<DateTimeOffset, decimal>();
			var indicatorChart = panel.CreateChart<DateTimeOffset, decimal>();

			foreach (var security in securities)
			{
				// detener el cálculo si el usuario cancela la ejecución del script
				if (cancellationToken.IsCancellationRequested)
					break;

				var candlesSeries = new Dictionary<DateTimeOffset, decimal>();
				var indicatorSeries = new Dictionary<DateTimeOffset, decimal>();

				// crear ROC
				var roc = new RateOfChange();

				// obtener almacenamiento de velas
				var candleStorage = storage.GetCandleMessageStorage(security, dataType, drive, format);

				foreach (var candle in candleStorage.Load(from, to))
				{
					// rellenar series
					candlesSeries[candle.OpenTime] = candle.ClosePrice;
					indicatorSeries[candle.OpenTime] = roc.Process(candle).ToDecimal();
				}

				// dibujar series en el gráfico
				candleChart.Append($"{security} (cierre)", candlesSeries.Keys, candlesSeries.Values);
				indicatorChart.Append($"{security} (ROC)", indicatorSeries.Keys, indicatorSeries.Values);
			}

			return Task.CompletedTask;
		}
	}
}

```

## Código del script en Python

```python
import clr

# Añadir referencias .NET
clr.AddReference("StockSharp.Messages")
clr.AddReference("StockSharp.Algo.Analytics")
clr.AddReference("Ecng.Drawing")

from Ecng.Drawing import DrawStyles
from System.Threading.Tasks import Task
from StockSharp.Algo.Analytics import IAnalyticsScript
from StockSharp.Algo.Indicators import ROC
from storage_extensions import *
from candle_extensions import *
from chart_extensions import *
from indicator_extensions import *

# Script analítico que usa el indicador ROC.
class indicator_script(IAnalyticsScript):
	def Run(self, logs, panel, securities, from_date, to_date, storage, drive, format, data_type, cancellation_token):
		if not securities:
			logs.LogWarning("No hay instrumentos.")
			return Task.CompletedTask

		# crear 2 paneles para velas y series de indicador
		candle_chart = create_chart(panel, datetime, float)
		indicator_chart = create_chart(panel, datetime, float)

		if data_type is None:
			logs.LogWarning(f"Tipo de datos no admitido {data_type}.")
			return Task.CompletedTask

		message_type = data_type.MessageType

		for security in securities:
			# detener el cálculo si el usuario cancela la ejecución del script
			if cancellation_token.IsCancellationRequested:
				break

			candles_series = {}
			indicator_series = {}

			# crear ROC
			roc = ROC()

			# obtener almacenamiento de velas
			candle_storage = get_candle_storage(storage, security, data_type, drive, format)

			for candle in load_range(candle_storage, message_type, from_date, to_date):
				# rellenar series
				candles_series[candle.OpenTime] = candle.ClosePrice
				indicator_series[candle.OpenTime] = to_decimal(process_candle(roc, candle))

			# dibujar series en el gráfico
			candle_chart.Append(
				f"{security} (cierre)",
				list(candles_series.keys()),
				list(candles_series.values())
			)
			indicator_chart.Append(
				f"{security} (ROC)",
				list(indicator_series.keys()),
				list(indicator_series.values())
			)

		return Task.CompletedTask

```
