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

- Подписывается на сделки
- Создает комбинированное правило для анализа цен сделок

```cs
// Метод OnStarted
protected override void OnStarted(DateTimeOffset time)
{
    var sub = this.SubscribeTrades(Security);

    sub.WhenTickTradeReceived(this).Do(() =>
    {
        new IMarketRule[] { Security.WhenLastTradePriceMore(this, 2), Security.WhenLastTradePriceLess(this, 2) }
            .Or() // or conditions (WhenLastTradePriceMore or WhenLastTradePriceLess)
            .Do(() =>
            {
                this.AddInfoLog($"The rule WhenLastTradePriceMore Or WhenLastTradePriceLess candle={Security.LastTick}");
            })
            .Apply(this);
    })
    .Once() // call this rule only once
    .Apply(this);

    base.OnStarted(time);
}
```

## Логика работы

- При получении первого тика создается комбинированное правило
- Правило срабатывает, когда цена последней сделки становится больше 2 или меньше 2
- При срабатывании правила в лог добавляется информация о последнем тике
- Внешнее правило срабатывает только один раз (`Once()`)

## Особенности

- Демонстрирует создание комбинированных правил с использованием `Or()`
- Использует `WhenLastTradePriceMore` и `WhenLastTradePriceLess` для анализа цен
- Показывает пример логирования информации о сделках
- Иллюстрирует использование `Once()` для ограничения срабатывания правила
- Правило создается только один раз при получении первого тика