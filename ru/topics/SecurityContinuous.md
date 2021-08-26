# Непрерывный фьючерс

[ContinuousSecurity](../api/StockSharp.Algo.ContinuousSecurity.html) \- непрерывный инструмент (как правило, фьючерс), содержащий в себе инструменты, подверженные экспирации (окончание обращения действия).

Например, два фьючерса индекса RTS \- **RIM5** и **RIU5**. При наступлении экспирации **RIM5** происходит автоматический переход на следующий инструмент \- **RIU5**.

[ContinuousSecurity](../api/StockSharp.Algo.ContinuousSecurity.html) можно торговать точно так же, как и [Security](../api/StockSharp.BusinessEntities.Security.html). До наступления экспирации для **RIM5** торговля будет вестись этим инструментом. После наступления экспирации, торговля будет вестись **RIU5**, и т.д..

### Создание ContinuousSecurity

Создание ContinuousSecurity

1. Объявить составные инструменты, которые будут входить в [ContinuousSecurity](../api/StockSharp.Algo.ContinuousSecurity.html), а также сам [ContinuousSecurity](../api/StockSharp.Algo.ContinuousSecurity.html):

   ```cs
   private Security \_rim5;
   private Security \_riu5;
   private ContinuousSecurity \_ri;
   							
   ```
2. Создать [ContinuousSecurity](../api/StockSharp.Algo.ContinuousSecurity.html):

   ```cs
   \_ri \= new ContinuousSecurity { Board \= ExchangeBoard.Forts, Id \= "RI" };
   							
   ```
3. Добавить в него составные инструменты, указав для каждого добавляемого инструмента дату и время экспирации:

   ```cs
   \_ri.ExpirationJumps.Add(\_rim5.ToSecurityId(), new DateTime(2015, 6, 15, 18, 45, 00));
   \_ri.ExpirationJumps.Add(\_riu5.ToSecurityId(), new DateTime(2015, 9, 15, 18, 45, 00));
   							
   ```
