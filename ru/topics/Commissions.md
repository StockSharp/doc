# Комиссия

Для учета комиссий в торговом роботе используется менеджер расчета комиссии [CommissionManager](xref:StockSharp.Algo.Commissions.CommissionManager).

Тарифный план создается с помощью добавления соответствующих правил [CommissionRule](xref:StockSharp.Algo.Commissions.CommissionRule), на основе которых в дальнейшем и будет вестись расчет комиссий.

## Создание CommissionManager

1. Создать [CommissionManager](xref:StockSharp.Algo.Commissions.CommissionManager):

   ```cs
   private CommissionManager _commissionManager = new CommissionManager();
   						
   ```
2. Далее, необходимо создать правило:

   ```cs
    CommissionRule commissionRule =  new CommissionPerTradeRule {  Value = new Unit(1m) };
   						
   ```
3. И добавить его в [CommissionManager](xref:StockSharp.Algo.Commissions.CommissionManager):

   ```cs
   _commissionManager.Rules.Add(commissionRule);;
   						
   ```

Подсчет комиссии можно вести как по сделкам, так и по заявкам. Для подсчета комиссии по сделке вызывается метод [Process](xref:StockSharp.Algo.Commissions.CommissionManager.Process), в который в качестве параметра передается [Message](xref:StockSharp.Messages.Message) \- сообщение, содержащее информацию о заявке или собственной сделке.

Суммарное значение комиссии можно узнать через [Commission](xref:StockSharp.Algo.Commissions.CommissionManager.Commission).
