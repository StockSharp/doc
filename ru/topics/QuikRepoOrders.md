# РЕПО и РПС заявки

[S\#](StockSharpAbout.md) позволяет отправлять через [Quik](Quik.md) РЕПО и РПС заявки. 

### Пример создания РПС заявки:

Пример создания РПС заявки:

1. Создаем новую заявку, указывая необходимый тип заявки: 

   ```cs
   var order = new Order
   {
   	Portfolio = _portfolio,
   	Volume = 1,
   	Security = _lkoh,
   	Price = _price,
   	Type = OrderTypes.Rps
   };
   ```
2. После этого инициализируем поле RpsInfo, заполняя необходимые для заявки поля (часть из полей [NtmOrderInfo](xref:StockSharp.Messages.NtmOrderInfo) являются необязательными): 

   ```cs
   order.RpsInfo = new NtmOrderInfo
   {
   	Partner = _partner
   };
   ```
3. Последний шаг \- стандартная регистрация заявки: 

   ```cs
   _connector.RegisterOrder(order);
   ```

## См. также
