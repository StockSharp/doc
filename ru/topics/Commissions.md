# Комиссия

Для учета комиссий в торговом роботе используется менеджер расчета комиссии [CommissionManager](../api/StockSharp.Algo.Commissions.CommissionManager.html).

Тарифный план создается с помощью добавления соответствующих правил [CommissionRule](../api/StockSharp.Algo.Commissions.CommissionRule.html), на основе которых в дальнейшем и будет вестись расчет комиссий.

### Создание CommissionManager

Создание CommissionManager

1. Создать [CommissionManager](../api/StockSharp.Algo.Commissions.CommissionManager.html):

   ```cs
   private CommissionManager \_commissionManager \= new CommissionManager();
   						
   ```
2. Далее, необходимо создать правило:

   ```cs
    CommissionRule commissionRule \=  new CommissionPerTradeRule {  Value \= new Unit(1m) };
   						
   ```
3. И добавить его в [CommissionManager](../api/StockSharp.Algo.Commissions.CommissionManager.html):

   ```cs
   \_commissionManager.Rules.Add(commissionRule);;
   						
   ```

Подсчет комиссии можно вести как по сделкам, так и по заявкам. Для подсчета комиссии по сделке вызывается метод [Process](../api/StockSharp.Algo.Commissions.CommissionManager.Process.html), в который в качестве параметра передается [Message](../api/StockSharp.Messages.Message.html) \- сообщение, содержащее информацию о заявке или собственной сделке.

Суммарное значение комиссии можно узнать через [Commission](../api/StockSharp.Algo.Commissions.CommissionManager.Commission.html).
