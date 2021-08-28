# Тейк\-профит и стоп\-лосс

С помощью подхода [дочерних стратегий](StrategyChilds.md) и использования [BasketStrategy](xref:StockSharp.Algo.Strategies.BasketStrategy) в [S\#](StockSharpAbout.md) реализован механизм автоматической защиты позиции через стратегии [TakeProfitStrategy](xref:StockSharp.Algo.Strategies.Protective.TakeProfitStrategy) и [StopLossStrategy](xref:StockSharp.Algo.Strategies.Protective.StopLossStrategy). Данные стратегии имеют ряд преимуществ перед обычными стоп заявками: 

1. Защитные стратегии не выставляют заявки до тех пор, пока не наступит условие. Размер депозита под заявки при этом не блокируются брокером.
2. Стратегии платформо\-независимые, а значит будут работать через любого брокера вне зависимости от их технологии. Стоп\-заявки же, условия которых задаются через [Order.Condition](xref:StockSharp.BusinessEntities.Order.Condition), эмулируются платформами [Quik](Quik.md), [SmartCOM](Smart.md) и т.д. самостоятельно, и их логика привязана к брокеру.
3. Защитные стратегии могут работать при прямом подключении к биржам.
4. Автоматическое отслеживание частичного или полного закрытия защищаемой позиции (с последующим снятием стопов). А также переворот позиции. Например, когда была длинная позиция, и она была перевернута в короткую. Стопы в этом случае нужно так же "перевернуть".

### Предварительные условия

[Дочерние стратегии](StrategyChilds.md)

[Событийная модель](StrategyAction.md)

### Тейк\-профит и Стоп\-лосс

1. В качестве примера разобрана регистрация заявки на покупку "по рынку" и последующая защита длинной позиции. Для этого создается правило, реагирующее на появление у заявки сделок (подробнее, в разделе [Событийная модель](StrategyAction.md)): 

   ```cs
   public class MyStrategy : Strategy
   {
   	public void OpenPosition()
   	{
   		// создаем заявку для открытия длинной позиции
   		var longPos = this.BuyAtMarket();
   		
   		// регистрируем правило, отслеживающее появление новых сделок по заявке
   		longPos
   			.WhenNewTrade()
   			.Do(OnNewOrderTrade)
   			.Apply(this);
   		
   		// отправляем заявку на регистрацию
   		RegisterOrder(longPos);
   	}
   }
   					
   ```
2. Чтобы защитить позицию необходимо использовать стратегии [TakeProfitStrategy](xref:StockSharp.Algo.Strategies.Protective.TakeProfitStrategy) или [StopLossStrategy](xref:StockSharp.Algo.Strategies.Protective.StopLossStrategy). Если требуется одновременная защита с двух сторон, то рекомендуется использовать [TakeProfitStopLossStrategy](xref:StockSharp.Algo.Strategies.Protective.TakeProfitStopLossStrategy). Данная стратегия автоматически изменяет объем одной из стратегий при частичной активации (например, при касании стоп\-лосс уровня закрылась лишь часть позиции, а затем рынок снова вернулся в безубыточную зону): 

   ```cs
   private void OnNewOrderTrade(MyTrade trade)
   {
       // для сделки добавляем защитную пару стратегии
       // выставляет тейк-профит в 40 пунктов
       var takeProfit = new TakeProfitStrategy(trade, 40);
       // выставляет стоп-лосс в 20 пунктов
       var stopLoss = new StopLossStrategy(trade, 20);
       var protectiveStrategies = new TakeProfitStopLossStrategy(takeProfit, stopLoss);
       ChildStrategies.AddRange(protectiveStrategies);
   }
   ```

### Автоматическое закрытие и переворот позиции

Стратегии [TakeProfitStrategy](xref:StockSharp.Algo.Strategies.Protective.TakeProfitStrategy) и [StopLossStrategy](xref:StockSharp.Algo.Strategies.Protective.StopLossStrategy) не отслеживают частичное закрытие позиции или ее переворот (например, позиция была закрыта руками в терминале и была открыта в противоположную сторону). Для того, чтобы автоматически отслеживать в роботе подобные ситуации, необходимо использовать [AutoProtectiveStrategy](xref:StockSharp.Algo.Strategies.Protective.AutoProtectiveStrategy). Данная стратегия через поступающие в нее сделки ([AutoProtectiveStrategy.ProcessNewMyTrade](xref:StockSharp.Algo.Strategies.Protective.AutoProtectiveStrategy.ProcessNewMyTrade)) решает, что нужно сделать: защитить их (если идет открытие позиции или ее увеличение) или остановить защитные стратегии (если идет закрытие позиции или ее уменьшение). Также стратегия автоматически переворачивает защитные стратегии в случае переворота позиции (из длинной в короткую или из короткой в длинную). 

### Следующие шаги

[Отчеты](StrategyReports.md)
