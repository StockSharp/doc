# График 3D

Скрипт `Chart3DScript` демонстрирует создание 3D графика для визуализации распределения объемов торгов по часам для различных финансовых инструментов. Этот метод визуализации позволяет наглядно представить динамику торговли и выявить пики активности на рынке.

![hydra_analitics_chart3d](../images/hydra_analitics_chart3d.png)

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

## Код скрипта

```cs
namespace StockSharp.Algo.Analytics
{
	/// <summary>
	/// The analytic script, calculating distribution of the biggest volume by hours
	/// and shows its in 3D chart.
	/// </summary>
	public class Chart3DScript : IAnalyticsScript
	{
		Task IAnalyticsScript.Run(ILogReceiver logs, IAnalyticsPanel panel, SecurityId[] securities, DateTime from, DateTime to, IStorageRegistry storage, IMarketDataDrive drive, StorageFormats format, TimeSpan timeFrame, CancellationToken cancellationToken)
		{
			if (securities.Length == 0)
			{
				logs.AddWarningLog("No instruments.");
				return Task.CompletedTask;
			}

			var x = new List<string>();
			var y = new List<string>();

			// fill Y labels
			for (var h = 0; h < 24; h++)
				y.Add(h.ToString());

			var z = new double[securities.Length, y.Count];

			for (var i = 0; i < securities.Length; i++)
			{
				// stop calculation if user cancel script execution
				if (cancellationToken.IsCancellationRequested)
					break;

				var security = securities[i];

				// fill X labels
				x.Add(security.ToStringId());

				// get candle storage
				var candleStorage = storage.GetTimeFrameCandleMessageStorage(security, timeFrame, drive, format);

				// get available dates for the specified period
				var dates = candleStorage.GetDates(from, to).ToArray();

				if (dates.Length == 0)
				{
					logs.AddWarningLog("no data");
					return Task.CompletedTask;
				}

				// grouping candles by opening time (time part only) with 1 hour truncating
				var byHours = candleStorage.Load(from, to)
					.GroupBy(c => c.OpenTime.TimeOfDay.Truncate(TimeSpan.FromHours(1)))
					.ToDictionary(g => g.Key.Hours, g => g.Sum(c => c.TotalVolume));

				// fill Z values
				foreach (var pair in byHours)
					z[i, pair.Key] = (double)pair.Value;
			}

			panel.Draw3D(x, y, z, "Instruments", "Hours", "Volume");

			return Task.CompletedTask;
		}
	}
}
```