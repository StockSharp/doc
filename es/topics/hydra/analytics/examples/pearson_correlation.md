# Correlación de Pearson

La correlación de Pearson es un método estadístico usado para medir el grado de relación lineal entre dos variables cuantitativas. En el análisis financiero, este método se usa ampliamente para estudiar relaciones entre distintos activos, como acciones o pares de divisas.

![Correlación de Pearson](../../../../images/hydra_analytics_pearson_correlation.png)

## Descripción del método

El coeficiente de correlación de Pearson varía de -1 a 1, donde:

- **1** indica una correlación positiva perfecta,
- **0** sugiere ausencia de relación lineal,
- **-1** significa una correlación negativa perfecta.

El valor del coeficiente muestra qué tan estrechamente están relacionadas linealmente dos variables.

## Aplicación práctica

- **Gestión de carteras**: evaluar correlaciones entre activos ayuda a construir carteras diversificadas para minimizar riesgo.
- **Estrategias de cobertura**: identificar activos con alta correlación negativa puede usarse para desarrollar estrategias de cobertura.
- **Análisis de tendencias de mercado**: estudiar correlaciones entre distintos mercados e instrumentos aporta información sobre interconexiones económicas globales.

## Cálculo de la correlación de Pearson

El coeficiente de correlación de Pearson se calcula con la siguiente fórmula:

\[ r = \frac{n(\sum xy) - (\sum x)(\sum y)}{\sqrt{[n\sum x^2 - (\sum x)^2][n\sum y^2 - (\sum y)^2]}} \]

donde:
- \(n\) es el número de observaciones,
- \(x\) e \(y\) son los valores de las variables,
- \(\sum\) denota la suma.

## Implementación del script

Un script para calcular la correlación de Pearson debe incluir los siguientes pasos:

1. **Recopilación de datos**: cargar series temporales de las dos variables analizadas.
2. **Procesamiento preliminar**: alinear series temporales por fechas y eliminar valores faltantes.
3. **Cálculo**: aplicar la fórmula de correlación de Pearson a los datos procesados.
4. **Análisis de resultados**: interpretar el coeficiente de correlación resultante para la toma de decisiones.

Calcular la correlación de Pearson proporciona información crucial para el análisis de mercado y la optimización de estrategias de inversión, permitiendo evaluar el grado de influencia mutua entre activos financieros.

## Código del script en C#

```cs
namespace StockSharp.Algo.Analytics
{
	using MathNet.Numerics.Statistics;

	/// <summary>
	/// Script analítico que calcula la correlación de Pearson para los instrumentos especificados.
	/// </summary>
	public class PearsonCorrelationScript : IAnalyticsScript
	{
		Task IAnalyticsScript.Run(ILogReceiver logs, IAnalyticsPanel panel, SecurityId[] securities, DateTime from, DateTime to, IStorageRegistry storage, IMarketDataDrive drive, StorageFormats format, DataType dataType, CancellationToken cancellationToken)
		{
			if (securities.Length == 0)
			{
				logs.LogWarning("No instruments.");
				return Task.CompletedTask;
			}

			var closes = new List<double[]>();

			foreach (var security in securities)
			{
				// detener el cálculo si el usuario cancela la ejecución del script
				if (cancellationToken.IsCancellationRequested)
					break;

				// obtener almacenamiento de velas
				var candleStorage = storage.GetCandleMessageStorage(security, dataType, drive, format);

				// obtener precios de cierre
				var prices = candleStorage.Load(from, to).Select(c => (double)c.ClosePrice).ToArray();

				if (prices.Length == 0)
				{
					logs.LogWarning("No hay datos para {0}", security);
					return Task.CompletedTask;
				}

				closes.Add(prices);
			}

			// todos los arreglos deben tener la misma longitud, por eso se recortan los más largos
			var min = closes.Select(arr => arr.Length).Min();

			for (var i = 0; i < closes.Count; i++)
			{
				var arr = closes[i];

				if (arr.Length > min)
					closes[i] = arr.Take(min).ToArray();
			}

			// calcular correlación
			var matrix = Correlation.PearsonMatrix(closes);

			// mostrar resultado en heatmap
			var ids = securities.Select(s => s.ToStringId());
			panel.DrawHeatmap(ids, ids, matrix.ToArray());

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
from numpy_extensions import nx

clr.AddReference("NumpyDotNet")
from NumpyDotNet import np

# Script analítico que calcula la correlación de Pearson para los instrumentos especificados.
class pearson_correlation_script(IAnalyticsScript):
	def Run(
		self,
		logs,
		panel,
		securities,
		from_date,
		to_date,
		storage,
		drive,
		format,
		data_type,
		cancellation_token
	):
		if not securities:
			logs.LogWarning("No instruments.")
			return Task.CompletedTask

		closes = []

		if data_type is None:
			logs.LogWarning(f"Tipo de datos no admitido {data_type}.")
			return Task.CompletedTask

		message_type = data_type.MessageType

		for security in securities:
			# detener el cálculo si el usuario cancela la ejecución del script
			if cancellation_token.IsCancellationRequested:
				break

			# obtener almacenamiento de velas
			candle_storage = get_candle_storage(storage, security, data_type, drive, format)

			# obtener precios de cierre
			prices = [float(c.ClosePrice) for c in load_range(candle_storage, message_type, from_date, to_date)]

			if len(prices) == 0:
				logs.LogWarning("No hay datos para {0}", security)
				return Task.CompletedTask

			closes.append(prices)

		# todos los arreglos deben tener la misma longitud, por eso se recortan los más largos
		min_length = min(len(arr) for arr in closes)
		closes = [arr[:min_length] for arr in closes]

		# convertir lista o arreglo en arreglo 2D
		array2d = nx.to2darray(closes)

		# calcular correlación usando NumSharp
		np_array = np.array(array2d)
		matrix = np.corrcoef(np_array)

		# mostrar resultado en heatmap
		ids = [to_string_id(s) for s in securities]
		panel.DrawHeatmap(ids, ids, nx.tosystemarray(matrix))

		return Task.CompletedTask

```
