# Gráfico 3D

El script `Chart3DScript` demuestra la creación de un gráfico 3D para visualizar la distribución de volúmenes de trading por hora para distintos instrumentos financieros. Este método de visualización permite representar claramente la dinámica de negociación e identificar picos de actividad del mercado.

![Gráfico 3D](../../../../images/hydra_analytics_chart3d.png)

## Descripción del funcionamiento del script

El script analiza datos de velas para el período especificado, los agrupa por hora y calcula el volumen total negociado para cada hora. Los resultados se presentan en un gráfico 3D, donde los ejes representan:

- **Eje X**: instrumentos financieros.
- **Eje Y**: horas de la sesión de trading (de 0 a 23).
- **Eje Z**: volúmenes de trading.

## Utilidad de usar un gráfico 3D

### Análisis de actividad del mercado

El gráfico 3D permite evaluar cuándo se produce la mayor actividad para varios instrumentos simultáneamente. Esto puede ser útil para identificar ventanas óptimas de trading o estudiar el impacto de eventos globales en el mercado.

### Comparación de instrumentos

Gracias a la visualización de volúmenes de trading por hora en un espacio tridimensional, los traders pueden comparar instrumentos entre sí por nivel de actividad y horarios de negociación preferidos. Esto puede ayudar a seleccionar los instrumentos más líquidos en determinadas horas o a encontrar instrumentos con patrones de actividad similares para diversificar carteras.

### Optimización de estrategias

El análisis de la distribución de volúmenes de trading puede servir como base para optimizar estrategias de trading, permitiendo adaptarlas a marcos temporales con la mayor actividad del mercado. Esto es especialmente relevante para trading algorítmico y de alta frecuencia.

## Implementación del script

El script realiza las siguientes acciones:

1. Comprueba la presencia de instrumentos financieros para análisis.
2. Forma etiquetas para los ejes X (instrumentos) e Y (horas).
3. Carga y agrupa datos de velas.
4. Calcula los volúmenes totales de trading por hora y rellena los datos del eje Z.
5. Dibuja el gráfico 3D usando el método `panel.Draw3D`.

## Código del script en C#

```cs
namespace StockSharp.Algo.Analytics
{
	/// <summary>
	/// Script analítico que calcula la distribución del mayor volumen por horas
	/// y la muestra en un gráfico 3D.
	/// </summary>
	public class Chart3DScript : IAnalyticsScript
	{
		Task IAnalyticsScript.Run(ILogReceiver logs, IAnalyticsPanel panel, SecurityId[] securities, DateTime from, DateTime to, IStorageRegistry storage, IMarketDataDrive drive, StorageFormats format, DataType dataType, CancellationToken cancellationToken)
		{
			if (securities.Length == 0)
			{
				logs.LogWarning("No hay instrumentos.");
				return Task.CompletedTask;
			}

			var x = new List<string>();
			var y = new List<string>();

			// rellenar etiquetas Y
			for (var h = 0; h < 24; h++)
				y.Add(h.ToString());

			var z = new double[securities.Length, y.Count];

			for (var i = 0; i < securities.Length; i++)
			{
				// detener el cálculo si el usuario cancela la ejecución del script
				if (cancellationToken.IsCancellationRequested)
					break;

				var security = securities[i];

				// rellenar etiquetas X
				x.Add(security.ToStringId());

				// obtener almacenamiento de velas
				var candleStorage = storage.GetCandleMessageStorage(security, dataType, drive, format);

				// obtener fechas disponibles para el período especificado
				var dates = candleStorage.GetDates(from, to).ToArray();

				if (dates.Length == 0)
				{
					logs.LogWarning("No hay datos.");
					return Task.CompletedTask;
				}

				// agrupar velas por hora de apertura (solo parte de hora) truncando a 1 hora
				var byHours = candleStorage.Load(from, to)
					.GroupBy(c => c.OpenTime.TimeOfDay.Truncate(TimeSpan.FromHours(1)))
					.ToDictionary(g => g.Key.Hours, g => g.Sum(c => c.TotalVolume));

				// rellenar valores Z
				foreach (var pair in byHours)
					z[i, pair.Key] = (double)pair.Value;
			}

			panel.Draw3D(x, y, z, "Instrumentos", "Horas", "Volumen");

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
from numpy_extensions import nx

# Script analítico que calcula la distribución del mayor volumen por horas y la muestra en un gráfico 3D.
class chart3d_script(IAnalyticsScript):
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
		# Comprobar si no hay instrumentos
		if not securities:
			logs.LogWarning("No hay instrumentos.")
			return Task.CompletedTask

		x = []  # Etiquetas X para instrumentos
		y = []  # Etiquetas Y para horas

		# Rellenar etiquetas Y con horas de 0 a 23
		for h in range(24):
			y.append(str(h))

		# Crear una matriz 2D para valores Z con dimensiones: (número de instrumentos) x (número de horas)
		z = [[0.0 for _ in range(len(y))] for _ in range(len(securities))]

		if data_type is None:
			logs.LogWarning(f"Tipo de datos no admitido {data_type}.")
			return Task.CompletedTask

		message_type = data_type.MessageType

		for i, security in enumerate(securities):
			# Detener el cálculo si el usuario cancela la ejecución del script
			if cancellation_token.IsCancellationRequested:
				break

			# Rellenar etiquetas X con identificadores de instrumentos
			x.append(to_string_id(security))

			# Obtener almacenamiento de velas para el instrumento actual
			candle_storage = get_candle_storage(storage, security, data_type, drive, format)

			# Obtener fechas disponibles para el período especificado
			dates = get_dates(candle_storage, from_date, to_date)

			if len(dates) == 0:
				logs.LogWarning("No hay datos.")
				return Task.CompletedTask

			# Agrupar velas por hora de apertura (truncada a la hora más cercana) y sumar volúmenes
			candles = load_range(candle_storage, message_type, from_date, to_date)
			by_hours = {}
			for candle in candles:
				hour = int(candle.OpenTime.TimeOfDay.TotalHours)
				by_hours[hour] = by_hours.get(hour, 0) + candle.TotalVolume

			# Rellenar valores Z para el instrumento actual
			for hour, volume in by_hours.items():
				if hour < len(y):
					z[i][hour] = float(volume)

		# Dibujar el gráfico 3D usando panel
		panel.Draw3D(x, y, nx.to2darray(z), "Instrumentos", "Horas", "Volumen")

		return Task.CompletedTask

```
