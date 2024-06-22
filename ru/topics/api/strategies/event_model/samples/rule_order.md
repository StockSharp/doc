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

- Подписывается на сделки
- Создает два набора правил для обработки событий регистрации заявок

```cs
// Метод OnStarted
protected override void OnStarted(DateTimeOffset time)
{
    var sub = this.SubscribeTrades(Security);

    sub.WhenTickTradeReceived(this).Do(() =>
    {
        var order = this.BuyAtMarket(1);
        var ruleReg = order.WhenRegistered(this);
        var ruleRegFailed = order.WhenRegisterFailed(this);

        ruleReg
            .Do(() => this.AddInfoLog("Order №1 Registered"))
            .Once()
            .Apply(this)
            .Exclusive(ruleRegFailed);

        ruleRegFailed
            .Do(() => this.AddInfoLog("Order №1 RegisterFailed"))
            .Once()
            .Apply(this)
            .Exclusive(ruleReg);

        RegisterOrder(order);
    }).Once().Apply(this);

    sub.WhenTickTradeReceived(this).Do(() =>
    {
        var order = this.BuyAtMarket(10000000);
        var ruleReg = order.WhenRegistered(this);
        var ruleRegFailed = order.WhenRegisterFailed(this);

        ruleReg
            .Do(() => this.AddInfoLog("Order №2 Registered"))
            .Once()
            .Apply(this)
            .Exclusive(ruleRegFailed);

        ruleRegFailed
            .Do(() => this.AddInfoLog("Order №2 RegisterFailed"))
            .Once()
            .Apply(this)
            .Exclusive(ruleReg);

        RegisterOrder(order);
    }).Once().Apply(this);

    base.OnStarted(time);
}
```

## Логика работы

### Первый набор правил

- При получении тика создает рыночную заявку на покупку 1 единицы
- Устанавливает правила для обработки успешной регистрации и ошибки регистрации
- Правила являются взаимоисключающими и срабатывают только один раз

### Второй набор правил

- При получении следующего тика создает рыночную заявку на покупку 10,000,000 единиц
- Аналогично устанавливает правила для обработки успешной регистрации и ошибки регистрации
- Правила также взаимоисключающие и срабатывают только один раз

## Особенности

- Демонстрирует создание правил для обработки событий регистрации заявок
- Использует механизм взаимоисключающих правил (`Exclusive`)
- Показывает пример логирования информации о событиях заявок
- Иллюстрирует использование `Once()` для ограничения срабатывания правила
- Создает заявки с разным объемом для демонстрации различных сценариев (успешная регистрация и ошибка регистрации)