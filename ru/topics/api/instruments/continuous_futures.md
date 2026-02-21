# Непрерывный фьючерс

[ExpirationContinuousSecurity](xref:StockSharp.Algo.ExpirationContinuousSecurity) \- непрерывный инструмент (как правило, фьючерс), содержащий в себе инструменты, подверженные экспирации (окончание срока действия).

Например, два фьючерса индекса RTS \- **RIM5** и **RIU5**. При наступлении экспирации **RIM5** происходит автоматический переход на следующий инструмент \- **RIU5**.

[ExpirationContinuousSecurity](xref:StockSharp.Algo.ExpirationContinuousSecurity) можно торговать точно так же, как и [Security](xref:StockSharp.BusinessEntities.Security). До наступления экспирации для **RIM5** торговля будет вестись этим инструментом. После наступления экспирации, торговля будет вестись **RIU5**, и т.д.

## Создание ExpirationContinuousSecurity

1. Объявить составные инструменты, которые будут входить в [ExpirationContinuousSecurity](xref:StockSharp.Algo.ExpirationContinuousSecurity), а также сам [ExpirationContinuousSecurity](xref:StockSharp.Algo.ExpirationContinuousSecurity):

   ```cs
   private Security _rim5;
   private Security _riu5;
   private ExpirationContinuousSecurity _ri;
   							
   ```
2. Создать [ExpirationContinuousSecurity](xref:StockSharp.Algo.ExpirationContinuousSecurity):

   ```cs
   _ri = new ExpirationContinuousSecurity { Board = ExchangeBoard.Forts, Id = "RI" };
   							
   ```
3. Добавить в него составные инструменты, указав для каждого добавляемого инструмента дату и время экспирации:

   ```cs
   _ri.ExpirationJumps.Add(_rim5.ToSecurityId(), new DateTime(2015, 6, 15, 18, 45, 00));
   _ri.ExpirationJumps.Add(_riu5.ToSecurityId(), new DateTime(2015, 9, 15, 18, 45, 00));

   ```

## VolumeContinuousSecurity

Помимо [ExpirationContinuousSecurity](xref:StockSharp.Algo.ExpirationContinuousSecurity), в StockSharp также доступен класс [VolumeContinuousSecurity](xref:StockSharp.Algo.VolumeContinuousSecurity). Этот тип непрерывного инструмента осуществляет переключение между контрактами на основе объёма торгов, а не даты экспирации. Переход на следующий контракт происходит, когда объём торгов нового контракта превышает объём текущего.
