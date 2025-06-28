# Правило одной свечи

## Обзор

`SimpleCandleRulesStrategy` - это стратегия, демонстрирующая использование правил для свечей в StockSharp. Она отслеживает объем свечей и логирует информацию при достижении определенных условий.

## Основные компоненты

```cs
// Основные компоненты
public class SimpleCandleRulesStrategy : Strategy
{
}
```

## Метод OnStarted

Вызывается при запуске стратегии:

- Инициализирует подписку на 5-минутные свечи
- Устанавливает правила для обработки свечей

```cs
// Метод OnStarted
protected override void OnStarted(DateTimeOffset time)
{
	var subscription = new Subscription(TimeSpan.FromMinutes(5).TimeFrame(), Security)
	{
		// ready-to-use candles much faster than compression on fly mode
		// turn off compression to boost optimizer (!!! make sure you have candles)

		//MarketData =
		//{
		//    BuildMode = MarketDataBuildModes.Build,
		//    BuildFrom = DataType.Ticks,
		//}
	};
	Subscribe(subscription);

	var i = 0;
	var diff = "10%".ToUnit();

	this.WhenCandlesStarted(subscription)
		.Do((candle) =>
		{
			i++;

			this
				.WhenTotalVolumeMore(candle, diff)
				.Do((candle1) =>
				{
					LogInfo($"The rule WhenCandlesStarted and WhenTotalVolumeMore candle={candle1}");
					LogInfo($"The rule WhenCandlesStarted and WhenTotalVolumeMore i={i}");
				})
				.Once().Apply(this);

		}).Apply(this);

	base.OnStarted(time);
}
```

## Логика работы

- Стратегия подписывается на 5-минутные свечи
- При начале формирования каждой свечи устанавливается правило
- Правило срабатывает, когда общий объем свечи превышает 10% (используется процентное значение)
- При срабатывании правила в лог добавляется информация о свече и счетчике
- После первого срабатывания правило перестает действовать благодаря методу `Once()`

## Особенности

- Демонстрирует использование правил `WhenCandlesStarted` и `WhenTotalVolumeMore`
- Использует механизм подписки на свечи
- Показывает пример создания процентного значения через `"10%".ToUnit()`
- Показывает пример логирования информации в стратегии через метод `LogInfo`
- Содержит закомментированный код для настройки построения свечей из тиков