# Строка транзакции

Регистрация, снятие и изменение заявок в [QuikTrader](xref:StockSharp.Quik.QuikTrader) осуществляется через отправку специальных транзакций в [Quik](Quik.md). Данные транзакции представляют собой особым образом оформленные строки. В некоторых случаях требуется модификация такой строки транзакции, прежде чем она будет отправлена в [Quik](Quik.md). Например, это требуется для регистрация заявок в [РТС Стандарт](http://rts.ru/ru/standard/), где необходимо удалить параметр *EXECUTION\_CONDITION*. Для этого существует специальное событие [QuikTrader.FormatTransaction](xref:StockSharp.Quik.QuikTrader.FormatTransaction). Данное событие передает [Transaction](xref:StockSharp.Quik.Transaction). Это вспомогательный класс, который упрощает работу со строкой транзакции. Для того, чтобы из данного класса посмотреть, как в итоге будет выглядеть [Quik](Quik.md)\-транзакция, нужно вызвать метод [Transaction.ToString](xref:StockSharp.Quik.Transaction.ToString). 

### Предварительные условия

[Создание первой стратегии](QuikFirstStrategy.md)

### Регистрация заявки в РТС Стандарт

Регистрация заявки в РТС Стандарт

1. Данный код написан в примере SampleConsole и закомментирован. Если необходима работа с [РТС Стандарт](http://rts.ru/ru/standard/), то его необходимо раскомментировать: 

   ```cs
   trader.FormatTransaction += transaction => transaction.RemoveInstruction(Transaction.ExecutionCondition);
   ```

### История транзакций

История транзакций

[QuikTrader](xref:StockSharp.Quik.QuikTrader) хранит в себе все транзакции, прошедшие через него в [Quik](Quik.md). Если в процессе тестирования необходимо получить данную информацию (например, для детального разбора ошибок), то можно использовать метод [GetTransaction](xref:StockSharp.Quik.QuikTrader.GetTransaction), возвращающий [Transaction](xref:StockSharp.Quik.Transaction) по номеру. 

## См. также
