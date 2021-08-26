# Дельта\-хеджирование

Если требуется защитить позиции по опционным стратегиям (например, как [Котирование по волатильности](OptionsQuoting.md)) можно воспользоваться стратегией хеджирования по дельте [DeltaHedgeStrategy](../api/StockSharp.Algo.Strategies.Derivatives.DeltaHedgeStrategy.html). 

### Дельта хеджирование

Дельта хеджирование

1. В качестве демонстрации работы [DeltaHedgeStrategy](../api/StockSharp.Algo.Strategies.Derivatives.DeltaHedgeStrategy.html) изменен пример SampleOptionQuoting (подробнее, [Котирование по волатильности](OptionsQuoting.md)). 
2. Сама стратегия [VolatilityQuotingStrategy](../api/StockSharp.Algo.Strategies.Derivatives.VolatilityQuotingStrategy.html) не запускается, а вместо этого она передается в качестве дочерней, для стратегии [DeltaHedgeStrategy](../api/StockSharp.Algo.Strategies.Derivatives.DeltaHedgeStrategy.html)

   ```cs
   \/\/ Создаем Дельта\-хедж стратегию
   var hedge \= new DeltaHedgeStrategy
   {
   	Security \= option.GetUnderlyingAsset(Connector),
   	Portfolio \= Portfolio.SelectedPortfolio,
   	Connector \= Connector,
   };
   \/\/ создаем котирование на покупку 20\-ти контрактов
   var quoting \= new VolatilityQuotingStrategy(Sides.Buy, 20,
   		new Range\<decimal\>(ImpliedVolatilityMin.Value ?? 0, ImpliedVolatilityMax.Value ?? 100))
   {
   	\/\/ указываем, что котирование работает с объемом в 1 контракт
   	Volume \= 1,
   	Security \= option,
   	Portfolio \= Portfolio.SelectedPortfolio,
   	Connector \= Connector,
   };
   \/\/ Передаем котирование в Дельта\-хедж стратегию
   hedge.ChildStrategies.Add(quoting);
   \/\/ Запускаем Дельта\-хедж стратегию
   hedge.Start();
   ```

   [DeltaHedgeStrategy](../api/StockSharp.Algo.Strategies.Derivatives.DeltaHedgeStrategy.html) принимает в качестве [дочерних стратегий](StrategyChilds.md) стратегии, работающие отдельно по своему страйку. Таким образом [DeltaHedgeStrategy](../api/StockSharp.Algo.Strategies.Derivatives.DeltaHedgeStrategy.html) контролирует суммарную позицию по всем дочерним опционным стратегиям. 
3. Завершение работы дельта хеджирования: 

   ```none
   hedge.Stop();
   ```

## См. также
