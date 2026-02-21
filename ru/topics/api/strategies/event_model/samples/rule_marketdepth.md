# Правила на стаканы и сделки

## Обзор

`SimpleRulesStrategy` - это стратегия, демонстрирующая различные способы создания и применения правил в StockSharp. Она подписывается на сделки и стакан, а затем устанавливает различные правила для обработки получаемых данных.

## Основные компоненты

```cs
// Основные компоненты
public class SimpleRulesStrategy : Strategy
{
}
```

## Метод OnStarted

Вызывается при запуске стратегии:

- Создает подписки на сделки и стакан
- Демонстрирует различные способы создания и применения правил

```cs
// Метод OnStarted
protected override void OnStarted2(DateTime time)
{
	var tickSub = new Subscription(DataType.Ticks, Security);
	var mdSub = new Subscription(DataType.MarketDepth, Security);

	//-----------------------Create a rule. Method №1-----------------------------------
	mdSub.WhenOrderBookReceived(this).Do((depth) =>
	{
		LogInfo($"The rule WhenOrderBookReceived №1 BestBid={depth.GetBestBid()}, BestAsk={depth.GetBestAsk()}");
	}).Once().Apply(this);

	//-----------------------Create a rule. Method №2-----------------------------------
	var whenMarketDepthChanged = mdSub.WhenOrderBookReceived(this);

	whenMarketDepthChanged.Do((depth) =>
	{
		LogInfo($"The rule WhenOrderBookReceived №2 BestBid={depth.GetBestBid()}, BestAsk={depth.GetBestAsk()}");
	}).Once().Apply(this);

	//----------------------Rule inside rule-----------------------------------
	mdSub.WhenOrderBookReceived(this).Do((depth) =>
	{
		LogInfo($"The rule WhenOrderBookReceived №3 BestBid={depth.GetBestBid()}, BestAsk={depth.GetBestAsk()}");

		//----------------------not a Once rule-----------------------------------
		mdSub.WhenOrderBookReceived(this).Do((depth1) =>
		{
			LogInfo($"The rule WhenOrderBookReceived №4 BestBid={depth1.GetBestBid()}, BestAsk={depth1.GetBestAsk()}");
		}).Apply(this);
	}).Once().Apply(this);

	// Sending requests for subscribe to market data.
	Subscribe(tickSub);
	Subscribe(mdSub);

	base.OnStarted2(time);
}
```

## Логика работы

### Метод №1: Создание правила

- Создает правило, которое срабатывает при получении стакана
- Логирует лучшие бид и аск
- Правило срабатывает только один раз (`Once()`)

### Метод №2: Создание правила

- Демонстрирует альтернативный способ создания правила
- Функционально идентично методу №1

### Правило внутри правила

- Создает правило, которое срабатывает при получении стакана
- Внутри этого правила создается еще одно правило
- Внешнее правило срабатывает один раз, внутреннее - каждый раз при получении стакана

## Особенности

- Демонстрирует различные способы создания и применения правил в StockSharp
- Использует подписку на сделки и стакан
- Показывает пример логирования информации в стратегии через метод `LogInfo`
- Иллюстрирует использование `Once()` для ограничения срабатывания правила