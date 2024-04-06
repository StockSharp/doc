# Нормализация цен закрытия

Скрипт "Нормализация цен закрытия" предназначен для приведения цен закрытия финансовых инструментов к общему виду, что позволяет провести сопоставление и анализ различных активов на одной шкале. Это особенно полезно при сравнении инструментов с разной стоимостью и волатильностью.

![hydra_analitics_normalize](../images/hydra_analitics_normalize.png)

## Описание работы скрипта

Скрипт выполняет адаптацию данных о ценах закрытия путем их масштабирования или преобразования в соответствии с выбранным методом нормализации. Результатом является набор стандартизированных значений, которые могут быть использованы для количественного сравнения и мультиинструментального анализа.

## Применение нормализации

- **Унификация шкалы цен**: Нормализация помогает привести данные различных инструментов к единой шкале, упрощая визуальное сравнение и аналитическую оценку.
- **Анализ корреляций**: Стандартизированные данные позволяют выявлять корреляционные связи между активами и формировать диверсифицированные портфели.
- **Создание индексов и моделей**: Нормализованные цены используются для создания композитных индексов, моделей ценообразования и других квантовых исследований.

## Методика нормализации

Нормализация может включать следующие методы:

1. **Масштабирование**: Приведение цен закрытия к определенному диапазону значений, например от 0 до 1.
2. **Z-оценка**: Преобразование цен закрытия с использованием Z-оценки, которая показывает, на сколько стандартных отклонений значение отклоняется от среднего.
3. **Логарифмирование**: Применение логарифмического преобразования для сглаживания разброса данных и снижения влияния экстремальных значений.

## Реализация скрипта

Процесс нормализации обычно включает следующие шаги:

1. **Выбор нормализации**: Определение метода нормализации в зависимости от целей анализа и характеристик данных.
2. **Обработка данных**: Применение выбранного метода нормализации к ценам закрытия для каждого инструмента.
3. **Анализ результатов**: Использование нормализованных данных для последующего анализа и сравнения инструментов.

Скрипт "Нормализация цен закрытия" является важным инструментом для подготовки данных к торговому и квантовому анализу, позволяя трейдерам и аналитикам более точно сравнивать и оценивать финансовые активы в рамках различных стратегий и исследований.

## Код скрипта

```cs
namespace StockSharp.Algo.Analytics
{
	/// <summary>
	/// The analytic script, normalize securities close prices and shows on same chart.
	/// </summary>
	public class NormalizePriceScript : IAnalyticsScript
	{
		Task IAnalyticsScript.Run(ILogReceiver logs, IAnalyticsPanel panel, SecurityId[] securities, DateTime from, DateTime to, IStorageRegistry storage, IMarketDataDrive drive, StorageFormats format, TimeSpan timeFrame, CancellationToken cancellationToken)
		{
			if (securities.Length == 0)
			{
				logs.AddWarningLog("No instruments.");
				return Task.CompletedTask;
			}

			var chart = panel.CreateChart<DateTimeOffset, decimal>();

			foreach (var security in securities)
			{
				// stop calculation if user cancel script execution
				if (cancellationToken.IsCancellationRequested)
					break;

				var series = new Dictionary<DateTimeOffset, decimal>();

				// get candle storage
				var candleStorage = storage.GetTimeFrameCandleMessageStorage(security, timeFrame, drive, format);

				decimal? firstClose = null;

				foreach (var candle in candleStorage.Load(from, to))
				{
					firstClose ??= candle.ClosePrice;

					// normalize close prices by dividing on first close
					series[candle.OpenTime] = candle.ClosePrice / firstClose.Value;
				}

				// draw series on chart
				chart.Append(security.ToStringId(), series.Keys, series.Values);
			}

			return Task.CompletedTask;
		}
	}
}
```