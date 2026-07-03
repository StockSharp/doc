# Normalización del precio de cierre

El script "Closing Price Normalization" está diseñado para estandarizar los precios de cierre de instrumentos financieros, permitiendo comparar y analizar distintos activos en una escala unificada. Esto es especialmente útil al comparar instrumentos con distintos costes y volatilidad.

![hydra_analytics_normalize](../../../../images/hydra_analytics_normalize.png)

## Descripción del funcionamiento del script

El script adapta los datos de precios de cierre escalándolos o transformándolos según el método de normalización seleccionado. El resultado es un conjunto de valores estandarizados que pueden usarse para comparación cuantitativa y análisis multiinstrumento.

## Aplicación de la normalización

- **Unificación de escala de precios**: la normalización ayuda a llevar datos de varios instrumentos a una sola escala, simplificando la comparación visual y la evaluación analítica.
- **Análisis de correlación**: los datos estandarizados permiten identificar relaciones de correlación entre activos y formar carteras diversificadas.
- **Creación de índices y modelos**: los precios normalizados se usan para crear índices compuestos, modelos de valoración y otras investigaciones cuantitativas.

## Metodología de normalización

La normalización puede incluir los siguientes métodos:

1. **Escalado**: ajustar precios de cierre a un cierto rango de valores, por ejemplo de 0 a 1.
2. **Z-score**: transformar precios de cierre usando Z-score, que indica cuántas desviaciones estándar se aleja un valor de la media.
3. **Logaritmización**: aplicar transformación logarítmica para suavizar la dispersión de datos y reducir el impacto de valores extremos.

## Implementación del script

El proceso de normalización normalmente incluye los siguientes pasos:

1. **Selección de normalización**: determinar el método de normalización según los objetivos del análisis y las características de los datos.
2. **Procesamiento de datos**: aplicar el método elegido a los precios de cierre de cada instrumento.
3. **Análisis de resultados**: usar datos normalizados para análisis posterior y comparación de instrumentos.

El script "Closing Price Normalization" es una herramienta crucial para preparar datos para trading y análisis cuantitativo, permitiendo a traders y analistas comparar y evaluar con mayor precisión activos financieros dentro de distintas estrategias e investigaciones.

## Código del script en C#

```cs
namespace StockSharp.Algo.Analytics
{
	/// <summary>
	/// Script analítico que normaliza precios de cierre de instrumentos y los muestra en el mismo gráfico.
	/// </summary>
	public class NormalizePriceScript : IAnalyticsScript
	{
		Task IAnalyticsScript.Run(ILogReceiver logs, IAnalyticsPanel panel, SecurityId[] securities, DateTime from, DateTime to, IStorageRegistry storage, IMarketDataDrive drive, StorageFormats format, DataType dataType, CancellationToken cancellationToken)
		{
			if (securities.Length == 0)
			{
				logs.LogWarning("No instruments.");
				return Task.CompletedTask;
			}

			var chart = panel.CreateChart<DateTimeOffset, decimal>();

			foreach (var security in securities)
			{
				// detener el cálculo si el usuario cancela la ejecución del script
				if (cancellationToken.IsCancellationRequested)
					break;

				var series = new Dictionary<DateTimeOffset, decimal>();

				// obtener almacenamiento de velas
				var candleStorage = storage.GetCandleMessageStorage(security, dataType, drive, format);

				decimal? firstClose = null;

				foreach (var candle in candleStorage.Load(from, to))
				{
					firstClose ??= candle.ClosePrice;

					// normalizar precios de cierre dividiendo por el primer cierre
					series[candle.OpenTime] = candle.ClosePrice / firstClose.Value;
				}

				// dibujar series en el gráfico
				chart.Append(security.ToStringId(), series.Keys, series.Values);
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
from storage_extensions import *
from candle_extensions import *
from chart_extensions import *
from indicator_extensions import *

# Script analítico que normaliza precios de cierre de instrumentos y los muestra en el mismo gráfico.
class normalize_price_script(IAnalyticsScript):
	def Run(self, logs, panel, securities, from_date, to_date, storage, drive, format, data_type, cancellation_token):
		if not securities:
			logs.LogWarning("No instruments.")
			return Task.CompletedTask

		chart = create_chart(panel, datetime, float)

		if data_type is None:
			logs.LogWarning(f"Unsupported data type {data_type}.")
			return Task.CompletedTask

		message_type = data_type.MessageType

		for security in securities:
			# detener el cálculo si el usuario cancela la ejecución del script
			if cancellation_token.IsCancellationRequested:
				break

			series = {}

			# obtener almacenamiento de velas
			candle_storage = get_candle_storage(storage, security, data_type, drive, format)

			first_close = None

			for candle in load_range(candle_storage, message_type, from_date, to_date):
				if first_close is None:
					first_close = candle.ClosePrice

				# normalizar precios de cierre dividiendo por el primer cierre
				series[candle.OpenTime] = candle.ClosePrice / first_close

			# dibujar series en el gráfico
			chart.Append(to_string_id(security), list(series.keys()), list(series.values()))

		return Task.CompletedTask

```
