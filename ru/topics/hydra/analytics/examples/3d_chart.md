# График 3D

Скрипт `Chart3DScript` демонстрирует создание 3D графика для визуализации распределения объемов торгов по часам для различных финансовых инструментов. Этот метод визуализации позволяет наглядно представить динамику торговли и выявить пики активности на рынке.

![hydra_analytics_chart3d](../../../../images/hydra_analytics_chart3d.png)

## Описание работы скрипта

Скрипт анализирует данные по свечам за указанный период, группирует их по часам и рассчитывает суммарный объем торгов для каждого часа. Результаты представляются в виде 3D графика, где оси представляют собой:

- **Ось X**: Финансовые инструменты.
- **Ось Y**: Часы торговой сессии (от 0 до 23).
- **Ось Z**: Объем торгов.

## Полезность использования 3D графика

### Анализ рыночной активности

3D график позволяет сразу для нескольких инструментов оценить, в какие часы наблюдается наибольшая активность. Это может быть полезно для выявления оптимальных временных окон для торговли или исследования влияния глобальных событий на рынке.

### Сравнение инструментов

Благодаря визуализации объемов торгов по часам в трехмерном пространстве, трейдеры могут сравнивать инструменты между собой по уровню активности и предпочтительным временам сделок. Это может помочь в выборе наиболее ликвидных инструментов в определенные часы или в поиске инструментов с похожими паттернами активности для диверсификации портфеля.

### Оптимизация стратегий

Анализ распределения объемов торгов может служить основой для оптимизации торговых стратегий, позволяя адаптировать их под временные рамки с наибольшей рыночной активностью. Это особенно актуально для алгоритмического и частотного трейдинга.

## Реализация в скрипте

Скрипт выполняет следующие действия:

1. Проверка наличия финансовых инструментов для анализа.
2. Формирование меток для осей X (инструменты) и Y (часы).
3. Загрузка и группирование данных по свечам.
4. Расчет суммарного объема торгов по часам и заполнение данных для оси Z.
5. Отрисовка 3D графика с помощью метода `panel.Draw3D`.

## Код скрипта на C#

```cs
namespace StockSharp.Algo.Analytics
{
	/// <summary>
	/// Аналитический скрипт рассчитывает распределение максимального объёма по часам
	/// и показывает его на 3D-графике.
	/// </summary>
	public class Chart3DScript : IAnalyticsScript
	{
		Task IAnalyticsScript.Run(ILogReceiver logs, IAnalyticsPanel panel, SecurityId[] securities, DateTime from, DateTime to, IStorageRegistry storage, IMarketDataDrive drive, StorageFormats format, DataType dataType, CancellationToken cancellationToken)
		{
			if (securities.Length == 0)
			{
				logs.LogWarning("No instruments.");
				return Task.CompletedTask;
			}

			var x = new List<string>();
			var y = new List<string>();

			// заполнить подписи Y
			for (var h = 0; h < 24; h++)
				y.Add(h.ToString());

			var z = new double[securities.Length, y.Count];

			for (var i = 0; i < securities.Length; i++)
			{
				// остановить расчёт, если пользователь отменил выполнение скрипта
				if (cancellationToken.IsCancellationRequested)
					break;

				var security = securities[i];

				// заполнить подписи X
				x.Add(security.ToStringId());

				// получение хранилища свечей
				var candleStorage = storage.GetCandleMessageStorage(security, dataType, drive, format);

				// получить доступные даты за указанный период
				var dates = candleStorage.GetDates(from, to).ToArray();

				if (dates.Length == 0)
				{
					logs.LogWarning("no data");
					return Task.CompletedTask;
				}

				// grouping candles by opening time (time part only) with 1 hour truncating
				var byHours = candleStorage.Load(from, to)
					.GroupBy(c => c.OpenTime.TimeOfDay.Truncate(TimeSpan.FromHours(1)))
					.ToDictionary(g => g.Key.Hours, g => g.Sum(c => c.TotalVolume));

				// заполнить значения Z
				foreach (var pair in byHours)
					z[i, pair.Key] = (double)pair.Value;
			}

			panel.Draw3D(x, y, z, "Instruments", "Hours", "Volume");

			return Task.CompletedTask;
		}
	}
}

```

## Код скрипта на Python

```python
import clr

# Добавить ссылки .NET
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

# Аналитический скрипт рассчитывает распределение максимального объёма по часам и показывает его на 3D-графике.
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
		# Проверить, что инструменты отсутствуют
		if not securities:
			logs.LogWarning("No instruments.")
			return Task.CompletedTask

		x = []  # X labels for instruments
		y = []  # Y labels for hours

		# Заполнить подписи Y часами от 0 до 23
		for h in range(24):
			y.append(str(h))

		# Create a 2D array for Z values with dimensions: (number of securities) x (number of hours)
		z = [[0.0 for _ in range(len(y))] for _ in range(len(securities))]

		if data_type is None:
			logs.LogWarning(f"Unsupported data type {data_type}.")
			return Task.CompletedTask

		message_type = data_type.MessageType

		for i, security in enumerate(securities):
			# Остановить расчёт, если пользователь отменил выполнение скрипта
			if cancellation_token.IsCancellationRequested:
				break

			# Заполнить подписи X идентификаторами инструментов
			x.append(to_string_id(security))

			# Получить хранилище свечей для текущего инструмента
			candle_storage = get_candle_storage(storage, security, data_type, drive, format)

			# Получить доступные даты за указанный период
			dates = get_dates(candle_storage, from_date, to_date)

			if len(dates) == 0:
				logs.LogWarning("no data")
				return Task.CompletedTask

			# Grouping candles by opening time (truncated to the nearest hour) and summing volumes
			candles = load_range(candle_storage, message_type, from_date, to_date)
			by_hours = {}
			for candle in candles:
				hour = int(candle.OpenTime.TimeOfDay.TotalHours)
				by_hours[hour] = by_hours.get(hour, 0) + candle.TotalVolume

			# Заполнить значения Z для текущего инструмента
			for hour, volume in by_hours.items():
				if hour < len(y):
					z[i][hour] = float(volume)

		# Нарисовать 3D-график с использованием панели
		panel.Draw3D(x, y, nx.to2darray(z), "Instruments", "Hours", "Volume")

		return Task.CompletedTask

```