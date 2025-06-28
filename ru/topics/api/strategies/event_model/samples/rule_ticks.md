# Правила на тиковые сделки

## Обзор

`SimpleTradeRulesStrategy` - это стратегия, демонстрирующая использование комбинированных правил для анализа цен сделок в StockSharp. Она подписывается на сделки и создает правило, которое срабатывает при определенных условиях цены.

## Основные компоненты

```cs
// Основные компоненты
public class SimpleTradeRulesStrategy : Strategy
{
}
```

## Метод OnStarted

Вызывается при запуске стратегии:

- Создает подписку на тики
- Создает комбинированное правило для анализа цен сделок

```cs
// Метод OnStarted
protected override void OnStarted(DateTimeOffset time)
{
	var sub = new Subscription(DataType.Ticks, Security);

	sub.WhenTickTradeReceived(this).Do(t =>
	{
		sub
			.WhenLastTradePriceMore(this, t.Price + 2)
			.Or(sub.WhenLastTradePriceLess(this, t.Price - 2))
			.Do(t =>
			{
				LogInfo($"The rule WhenLastTradePriceMore Or WhenLastTradePriceLess tick={t}");
			})
			.Apply(this);
	})
	.Once() // call this rule only once
	.Apply(this);

	// Sending request for subscribe to market data.
	Subscribe(sub);

	base.OnStarted(time);
}
```

## Логика работы

- При получении первого тика создается комбинированное правило
- Базируется на цене полученного тика: создает правило, которое срабатывает при изменении цены на +/- 2
- Правило срабатывает, когда цена последней сделки становится больше текущей + 2 или меньше текущей - 2
- При срабатывании правила в лог добавляется информация о тике
- Внешнее правило срабатывает только один раз (`Once()`)

## Особенности

- Демонстрирует создание комбинированных правил с использованием `Or()`
- Использует `WhenLastTradePriceMore` и `WhenLastTradePriceLess` для анализа цен
- Показывает пример логирования информации о сделках через метод `LogInfo`
- Иллюстрирует использование `Once()` для ограничения срабатывания правила
- Передает параметр тика в обработчик событий (в отличие от примера в документации)