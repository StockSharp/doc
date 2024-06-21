# Поиск инструментов

При создании собственного адаптера для работы с биржей необходимо реализовать метод поиска инструментов. Этот метод вызывается при отправке сообщения [SecurityLookupMessage](xref:StockSharp.Messages.SecurityLookupMessage) и возвращает информацию об инструментах через сообщения [SecurityMessage](xref:StockSharp.Messages.SecurityMessage).

## Реализация метода SecurityLookupAsync

Метод **SecurityLookupAsync** обычно выполняет следующие действия:

1. Получает список поддерживаемых типов инструментов из входящего сообщения.
2. Запрашивает список инструментов у биржи через API.
3. Для каждого полученного инструмента создает сообщение [SecurityMessage](xref:StockSharp.Messages.SecurityMessage), заполняя его данными об инструменте.
4. Проверяет, соответствует ли инструмент критериям поиска.
5. Отправляет созданное сообщение [SecurityMessage](xref:StockSharp.Messages.SecurityMessage) через метод **SendOutMessage**.
6. После обработки всех инструментов отправляет сообщение о завершении поиска.

Ниже приведен пример реализации метода SecurityLookupAsync на основе адаптера для биржи Coinbase. При создании собственного адаптера необходимо адаптировать этот код под API используемой биржи.

```cs
public override async ValueTask SecurityLookupAsync(SecurityLookupMessage lookupMsg, CancellationToken cancellationToken)
{
    // Получаем список типов инструментов, которые нужно найти
    var secTypes = lookupMsg.GetSecurityTypes();
    
    // Определяем максимальное количество инструментов для поиска
    var left = lookupMsg.Count ?? long.MaxValue;

    // Перебираем типы инструментов, поддерживаемые биржей
    foreach (var type in new[] { "SPOT", "FUTURE" })
    {
        // Запрашиваем список инструментов у биржи
        var products = await _restClient.GetProducts(type, cancellationToken);

        foreach (var product in products)
        {
            // Создаем идентификатор инструмента
            var secId = product.ProductId.ToStockSharp();

            // Создаем сообщение с информацией об инструменте
            var secMsg = new SecurityMessage
            {
                SecurityType = product.ProductType.ToSecurityType(),
                SecurityId = secId,
                Name = product.DisplayName,
                PriceStep = product.QuoteIncrement?.ToDecimal(),
                VolumeStep = product.BaseIncrement?.ToDecimal(),
                MinVolume = product.BaseMinSize?.ToDecimal(),
                MaxVolume = product.BaseMaxSize?.ToDecimal(),
                ExpiryDate = product.FutureProductDetails?.ContractExpiry,
                Multiplier = product.FutureProductDetails?.ContractSize?.ToDecimal(),

                // обязательно заполняем идентификатор подписки,
                // чтобы внешний код смог понять, к какой подписке были получены данные
                OriginalTransactionId = lookupMsg.TransactionId,
            }
            .TryFillUnderlyingId(product.BaseCurrencyId.ToUpperInvariant());

            // Проверяем, соответствует ли инструмент критериям поиска
            if (!secMsg.IsMatch(lookupMsg, secTypes))
                continue;

            // Отправляем сообщение с информацией об инструменте
            SendOutMessage(secMsg);

            // Уменьшаем счетчик оставшихся инструментов
            if (--left <= 0)
                break;
        }

        if (left <= 0)
            break;
    }

    // Отправляем сообщение о завершении поиска
    SendSubscriptionResult(lookupMsg);
}
```

Этот метод позволяет получить информацию о доступных на бирже инструментах, включая их основные характеристики, такие как тип инструмента, минимальный объем, шаг цены и т.д.