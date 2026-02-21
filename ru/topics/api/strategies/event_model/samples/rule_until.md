# Правило До

## Обзор

`SimpleRulesUntilStrategy` - это стратегия, демонстрирующая использование правила с условием завершения (`Until`) в StockSharp. Она подписывается на сделки и стакан, а затем устанавливает правило, которое выполняется до достижения определенного условия.

## Основные компоненты

```cs
// Основные компоненты
public class SimpleRulesUntilStrategy : Strategy
{
}
```

## Метод OnStarted

Вызывается при запуске стратегии:

- Создает подписки на тики и стакан
- Создает правило, которое выполняется при получении данных стакана до достижения определенного условия

```cs
// Метод OnStarted
protected override void OnStarted2(DateTime time)
{
	var tickSub = new Subscription(DataType.Ticks, Security);
	var mdSub = new Subscription(DataType.MarketDepth, Security);

	var i = 0;
	mdSub.WhenOrderBookReceived(this).Do(depth =>
	{
		i++;
		LogInfo($"The rule WhenOrderBookReceived BestBid={depth.GetBestBid()}, BestAsk={depth.GetBestAsk()}");
		LogInfo($"The rule WhenOrderBookReceived i={i}");
	})
	.Until(() => i >= 10)
	.Apply(this);

	// Sending requests for subscribe to market data.
	Subscribe(tickSub);
	Subscribe(mdSub);

	base.OnStarted2(time);
}
```

## Логика работы

- При запуске стратегия создает подписки на тики и стакан
- Создается правило, которое срабатывает при каждом получении данных стакана
- При срабатывании правила:
  - Увеличивается счетчик `i`
  - В лог добавляется информация о лучших ценах bid и ask
  - В лог добавляется текущее значение счетчика `i`
- Правило выполняется до тех пор, пока значение счетчика `i` не достигнет или не превысит 10
- После выполнения условия правило автоматически прекращает действовать

## Особенности

- Демонстрирует использование метода `Until()` для ограничения выполнения правила
- Использует подписку на сделки и стакан
- Показывает пример логирования информации о стакане и состоянии счетчика через метод `LogInfo`
- Иллюстрирует, как можно ограничить количество выполнений правила на основе определенного условия