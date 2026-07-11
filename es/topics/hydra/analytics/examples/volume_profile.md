# Perfil de volumen

El script "Perfil de volumen" sirve como herramienta para analizar la distribución del volumen de trading por niveles de precio durante un período seleccionado. Permite a traders y analistas cuantitativos visualizar y examinar dónde se concentró la principal actividad de trading en términos de niveles de precio.

![Perfil de volumen](../../../../images/hydra_analytics_volume_profile.png)

## Descripción de funcionalidad

El script agrega datos de transacciones para formar un perfil que muestra los volúmenes ejecutados en distintos niveles de precio. Esta información puede representarse en un gráfico, ilustrando la densidad de operaciones en distintos puntos de precio.

## Importancia práctica

El análisis del perfil de volumen ayuda a identificar zonas clave de demanda y oferta, y puede usarse para:

- Identificar niveles de soporte y resistencia donde el instrumento encuentra interés significativo de los participantes del mercado.
- Evaluar la fuerza de la tendencia actual o su posible debilitamiento, basándose en el cambio de distribución de volumen.
- Planificar puntos de entrada y salida del mercado, considerando niveles con máxima liquidez acumulada.

## Aplicación en trading y análisis cuantitativo

- **Trading**: el perfil de volumen puede usarse para desarrollar estrategias basadas en análisis de volumen, proporcionando una vista clara de dónde se producen las principales operaciones de trading.
- **Análisis cuantitativo**: los datos sobre distribución de volumen pueden servir como entrada para modelos cuantitativos que predicen la probabilidad de movimientos de precio basándose en el volumen acumulado en un nivel.

## Implementación del script

El script "Perfil de volumen" realiza los siguientes pasos:

1. **Recopilación de datos**: el script agrega datos de transacciones para el período especificado.
2. **Formación del perfil**: con base en los datos recopilados, el script forma un perfil de volumen que refleja la actividad de trading en cada nivel de precio.
3. **Visualización**: los resultados del script se visualizan como gráfico o histograma, donde cada barra corresponde a un nivel de precio específico y su volumen de trading.

Usar el script "Perfil de volumen" dentro de la plataforma StockSharp permite realizar un análisis de mercado integral, construir hipótesis de trading fundamentadas y mejorar la calidad de las decisiones de trading tomadas.

## Código del script en C#

```cs
namespace StockSharp.Algo.Analytics
{
	/// <summary>
	/// Script analítico que calcula la distribución del volumen por niveles de precio.
	/// </summary>
	public class PriceVolumeScript : IAnalyticsScript
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

			// agrupar velas por precio medio
			var rows = candleStorage.Load(from, to)
				.GroupBy(c => c.LowPrice + c.GetLength() / 2)
				.ToDictionary(g => g.Key, g => g.Sum(c => c.TotalVolume));

			// dibujar en el gráfico
			panel.CreateChart<decimal, decimal>()
				.Append(security.ToStringId(), rows.Keys, rows.Values, DrawStyles.Histogram);

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

# Script analítico que calcula la distribución del volumen por niveles de precio.
class price_volume_script(IAnalyticsScript):
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

		# Agrupar velas por precio medio y sumar sus volúmenes
		candles = load_range(candle_storage, message_type, from_date, to_date)
		rows_dict = {}
		for candle in candles:
			# Calcular precio medio de la vela
			key = candle.LowPrice + get_length(candle) / 2
			# Sumar volúmenes para el mismo nivel de precio
			rows_dict[key] = rows_dict.get(key, 0) + candle.TotalVolume

		# Dibujar en el gráfico
		chart = create_chart(panel, float, float)
		chart.Append(to_string_id(security), list(rows_dict.keys()), list(rows_dict.values()), DrawStyles.Histogram)

		return Task.CompletedTask

```
