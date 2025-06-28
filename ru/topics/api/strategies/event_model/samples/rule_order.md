# Правило на заявки

## Обзор

`SimpleOrderRulesStrategy` - это стратегия, демонстрирующая использование правил для обработки событий, связанных с заявками в StockSharp. Она подписывается на сделки и создает правила для обработки регистрации заявок.

## Основные компоненты

```cs
// Основные компоненты
public class SimpleOrderRulesStrategy : Strategy
{
}
```

## Метод OnStarted

Вызывается при запуске стратегии:

- Создает подписку на тики
- Создает два набора правил для обработки событий регистрации заявок

```cs
// Метод OnStarted
protected override void OnStarted(DateTimeOffset time)
{
	var sub = new Subscription(DataType.Ticks, Security);

	sub.WhenTickTradeReceived(this).Do(() =>
	{
		var order = CreateOrder(Sides.Buy, default, 1);

		var ruleReg = order.WhenRegistered(this);
		var ruleRegFailed = order.WhenRegisterFailed(this);

		ruleReg
			.Do(() => LogInfo("Order №1 Registered"))
			.Once()
			.Apply(this)
			.Exclusive(ruleRegFailed);

		ruleRegFailed
			.Do(() => LogInfo("Order №1 RegisterFailed"))
			.Once()
			.Apply(this)
			.Exclusive(ruleReg);

		RegisterOrder(order);
	}).Once().Apply(this);

	sub.WhenTickTradeReceived(this).Do(() =>
	{
		var order = CreateOrder(Sides.Buy, default, 10000000);

		var ruleReg = order.WhenRegistered(this);
		var ruleRegFailed = order.WhenRegisterFailed(this);

		ruleReg
			.Do(() => LogInfo("Order №2 Registered"))
			.Once()
			.Apply(this)
			.Exclusive(ruleRegFailed);

		ruleRegFailed
			.Do(() => LogInfo("Order №2 RegisterFailed"))
			.Once()
			.Apply(this)
			.Exclusive(ruleReg);

		RegisterOrder(order);
	}).Once().Apply(this);

	// Sending request for subscribe to market data.
	Subscribe(sub);

	base.OnStarted(time);
}
```

## Логика работы

### Первый набор правил

- При получении тика создает заявку на покупку 1 единицы
- Заявка создается через метод `CreateOrder` с указанием направления, цены (default = рыночная) и объема
- Устанавливает правила для обработки успешной регистрации и ошибки регистрации
- Правила являются взаимоисключающими и срабатывают только один раз

### Второй набор правил

- При получении следующего тика создает заявку на покупку 10,000,000 единиц
- Аналогично устанавливает правила для обработки успешной регистрации и ошибки регистрации
- Правила также взаимоисключающие и срабатывают только один раз

## Особенности

- Демонстрирует создание правил для обработки событий регистрации заявок
- Использует механизм взаимоисключающих правил (`Exclusive`)
- Показывает пример логирования информации о событиях заявок через метод `LogInfo`
- Иллюстрирует использование `Once()` для ограничения срабатывания правила
- Создает заявки с разным объемом для демонстрации различных сценариев (успешная регистрация и ошибка регистрации)