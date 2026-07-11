# Volumen intradía

El script "Volumen intradía" es una herramienta para analizar la distribución del volumen de negociación de valores por horas dentro de una sola sesión de trading. Diseñado para usarse dentro de la plataforma StockSharp, está orientado a traders y analistas cuantitativos que buscan estudiar en profundidad el comportamiento del mercado y optimizar estrategias de trading.

![hydra_analytics_intraday_volume](../../../../images/hydra_analytics_intraday_volume.png)

## Descripción funcional

El script recopila datos de operaciones para un período seleccionado y los presenta en formato de gráfico, permitiendo visualizar los cambios del volumen de negociación por hora. Esto permite evaluar en qué horas del día se observa una actividad de trading mayor o menor.

## Importancia práctica

- **Para trading**: comprender las horas pico y valle ayuda a identificar los períodos más activos del mercado, lo que influye en decisiones sobre cuándo entrar o salir de posiciones.
- **Para análisis cuantitativo**: los analistas cuantitativos pueden usar datos de volumen intradía para crear modelos matemáticos y algoritmos que predicen el comportamiento del mercado según indicadores de volumen.

## Distribución horaria

La distribución del volumen de trading por hora arroja luz sobre la dinámica del mercado, destacando los intervalos temporales con la actividad principal. Esto puede indicar cambios en tendencias, niveles de soporte y resistencia, así como posibles momentos de aumento o escasez de liquidez.

## Aplicación de datos

El script "Volumen intradía" puede integrarse en un sistema más amplio de análisis de mercado, proporcionando datos que pueden usarse para:

- **Adaptación de estrategias**: ajustar parámetros de algoritmos de trading según niveles de actividad del mercado.
- **Evaluación de riesgos**: calcular la probabilidad de movimientos significativos de precio según la hora del día.

El uso del script "Volumen intradía" dentro de la plataforma de trading StockSharp permite a traders y analistas basar sus decisiones en datos específicos sobre la actividad del mercado y adaptar sus estrategias para ajustarse de forma óptima a las condiciones actuales de trading.

## Código del script en C#

```cs
namespace StockSharp.Algo.Analytics
{
	/// <summary>
	/// Script analítico que calcula la distribución del mayor volumen por horas.
	/// </summary>
	public class TimeVolumeScript : IAnalyticsScript
	{
		Task IAnalyticsScript.Run(ILogReceiver logs, IAnalyticsPanel panel, SecurityId[] securities, DateTime from, DateTime to, IStorageRegistry storage, IMarketDataDrive drive, StorageFormats format, DataType dataType, CancellationToken cancellationToken)
		{
			if (securities.Length == 0)
			{
				logs.LogWarning("No instruments.");
				return Task.CompletedTask;
			}

			// el script puede procesar solo 1 instrumento
			var security = securities.First();

			// obtener almacenamiento de velas
			var candleStorage = storage.GetCandleMessageStorage(security, dataType, drive, format);

			// obtener fechas disponibles para el período especificado
			var dates = candleStorage.GetDates(from, to).ToArray();

			if (dates.Length == 0)
			{
				logs.LogWarning("no data");
				return Task.CompletedTask;
			}

			// agrupar velas por hora de apertura (solo parte de hora) truncando a 1 hora
			var rows = candleStorage.Load(from, to)
				.GroupBy(c => c.OpenTime.TimeOfDay.Truncate(TimeSpan.FromHours(1)))
				.ToDictionary(g => g.Key, g => g.Sum(c => c.TotalVolume));

			// colocar nuestros cálculos en la tabla
			var grid = panel.CreateGrid("Time", "Volume");

			foreach (var row in rows)
				grid.SetRow(row.Key, row.Value);

			// ordenar por columna Volume (descendente)
			grid.SetSort("Volume", false);

			return Task.CompletedTask;
		}
	}
}

```

## Código del script en Python

```python
import clr

# Añadir referencias .NET
clr.AddReference("StockSharp.Algo.Analytics")
clr.AddReference("StockSharp.Messages")
clr.AddReference("Ecng.Drawing")

from Ecng.Drawing import DrawStyles
from System import TimeSpan
from System.Threading.Tasks import Task
from StockSharp.Algo.Analytics import IAnalyticsScript
from storage_extensions import *
from candle_extensions import *
from chart_extensions import *
from indicator_extensions import *

# Script analítico que calcula la distribución del mayor volumen por horas.
class time_volume_script(IAnalyticsScript):
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
			logs.LogWarning("No instruments.")
			return Task.CompletedTask

		# El script puede procesar solo 1 instrumento
		security = securities[0]

		if data_type is None:
			logs.LogWarning(f"Tipo de datos no admitido {data_type}.")
			return Task.CompletedTask

		message_type = data_type.MessageType

		# Obtener almacenamiento de velas
		candle_storage = get_candle_storage(storage, security, data_type, drive, format)

		# Obtener fechas disponibles para el período especificado
		dates = get_dates(candle_storage, from_date, to_date)

		if len(dates) == 0:
			logs.LogWarning("no data")
			return Task.CompletedTask

		# Agrupar velas por hora de apertura (truncado horario) y sumar sus volúmenes
		candles = load_range(candle_storage, message_type, from_date, to_date)
		rows = {}
		for candle in candles:
			time_of_day = candle.OpenTime.TimeOfDay
			truncated = TimeSpan.FromHours(int(time_of_day.TotalHours))
			rows[truncated] = rows.get(truncated, 0) + candle.TotalVolume

		# Colocar nuestros cálculos en la tabla
		grid = panel.CreateGrid("Time", "Volume")

		for key, value in rows.items():
			grid.SetRow(key, value)

		# Ordenar por la columna Volume en orden descendente
		grid.SetSort("Volume", False)

		return Task.CompletedTask

```
