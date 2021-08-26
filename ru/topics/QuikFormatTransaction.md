# Строка транзакции

Регистрация, снятие и изменение заявок в [QuikTrader](../api/StockSharp.Quik.QuikTrader.html) осуществляется через отправку специальных транзакций в [Quik](Quik.md). Данные транзакции представляют собой особым образом оформленные строки. В некоторых случаях требуется модификация такой строки транзакции, прежде чем она будет отправлена в [Quik](Quik.md). Например, это требуется для регистрация заявок в [РТС Стандарт](http://rts.ru/ru/standard/), где необходимо удалить параметр *EXECUTION\_CONDITION*. Для этого существует специальное событие [QuikTrader.FormatTransaction](../api/StockSharp.Quik.QuikTrader.FormatTransaction.html). Данное событие передает [Transaction](../api/StockSharp.Quik.Transaction.html). Это вспомогательный класс, который упрощает работу со строкой транзакции. Для того, чтобы из данного класса посмотреть, как в итоге будет выглядеть [Quik](Quik.md)\-транзакция, нужно вызвать метод [Transaction.ToString](../api/StockSharp.Quik.Transaction.ToString.html). 

### Предварительные условия

[Создание первой стратегии](QuikFirstStrategy.md)

### Регистрация заявки в РТС Стандарт

Регистрация заявки в РТС Стандарт

1. Данный код написан в примере SampleConsole и закомментирован. Если необходима работа с [РТС Стандарт](http://rts.ru/ru/standard/), то его необходимо раскомментировать: 

   ```cs
   trader.FormatTransaction +\= transaction \=\> transaction.RemoveInstruction(Transaction.ExecutionCondition);
   ```

### История транзакций

История транзакций

[QuikTrader](../api/StockSharp.Quik.QuikTrader.html) хранит в себе все транзакции, прошедшие через него в [Quik](Quik.md). Если в процессе тестирования необходимо получить данную информацию (например, для детального разбора ошибок), то можно использовать метод [GetTransaction](../api/StockSharp.Quik.QuikTrader.GetTransaction.html), возвращающий [Transaction](../api/StockSharp.Quik.Transaction.html) по номеру. 

## См. также
