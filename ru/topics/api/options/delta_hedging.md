# Дельта\-хеджирование

Если требуется защитить позиции по опционным стратегиям (например, как [Котирование по волатильности](volatility_trading.md)) можно воспользоваться стратегией хеджирования по дельте `DeltaHedgeStrategy`.

> [!NOTE]
> Класс `DeltaHedgeStrategy` более не является библиотечным классом StockSharp. Он перенесён в пример *Samples/06\_Strategies/09\_LiveOptionsQuoting/Strategies/DeltaHedgeStrategy.cs*. Конструктор принимает параметр [BasketBlackScholes](xref:StockSharp.Algo.Derivatives.BasketBlackScholes) \- портфельную модель для расчёта греков.

## Дельта хеджирование

1. В качестве демонстрации работы `DeltaHedgeStrategy` изменен пример *Samples/06\_Strategies/09\_LiveOptionsQuoting* (подробнее, [Котирование по волатильности](volatility_trading.md)).
2. Сама стратегия [VolatilityQuotingStrategy](xref:StockSharp.Algo.Strategies.Quoting.VolatilityQuotingStrategy) не запускается, а вместо этого она передается в качестве дочерней для стратегии `DeltaHedgeStrategy`

   ```cs
   // Создаем Дельта-хедж стратегию (передаем модель BasketBlackScholes)
   var hedge = new DeltaHedgeStrategy(PosChart.Model)
   {
   	Security = option.GetUnderlyingAsset(Connector),
   	Portfolio = Portfolio.SelectedPortfolio,
   	Connector = Connector,
   };
   // создаем котирование на покупку 20-ти контрактов
   var quoting = new VolatilityQuotingStrategy
   {
   	QuotingSide = Sides.Buy,
   	QuotingVolume = 20,
   	IVRange = new Range<decimal>((decimal?)ImpliedVolatilityMin.EditValue ?? 0, (decimal?)ImpliedVolatilityMax.EditValue ?? 100),
   	// указываем, что котирование работает с объемом в 1 контракт
   	Volume = 1,
   	Security = option,
   	Portfolio = Portfolio.SelectedPortfolio,
   	Connector = Connector,
   };
   // Передаем котирование в Дельта-хедж стратегию
   hedge.ChildStrategies.Add(quoting);
   // Запускаем Дельта-хедж стратегию
   hedge.Start();
   ```

   `DeltaHedgeStrategy` принимает в качестве дочерних стратегий стратегии, работающие отдельно по своему страйку. Таким образом `DeltaHedgeStrategy` контролирует суммарную позицию по всем дочерним опционным стратегиям.

3. Завершение работы дельта хеджирования: 

   ```none
   hedge.Stop();
   ```
