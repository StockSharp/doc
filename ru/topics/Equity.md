# Кривая эквити

[S\#](StockSharpAbout.md) предусмотрена возможность построения кривых доходности (эквити) с целью дальнейшего анализа работы торгового робота. Данные для кривой получаются с помощью класса [PnLManager](../api/StockSharp.Algo.PnL.PnLManager.html). Данный класс производит расчёт текущего значения, оповещая робота об изменениях через событие [Strategy.PnLChanged](../api/StockSharp.Algo.Strategies.Strategy.PnLChanged.html). 

Для расчета параметров кривой (максимальная просадка, коэффициент Шарпа и т.д.) используется [StatisticManager](../api/StockSharp.Algo.Statistics.StatisticManager.html). Эти параметры хранятся в свойстве [StatisticManager.Parameters](../api/StockSharp.Algo.Statistics.StatisticManager.Parameters.html). Каждый параметр реализует интерфейсы [IPnLStatisticParameter](../api/StockSharp.Algo.Statistics.IPnLStatisticParameter.html), [ITradeStatisticParameter](../api/StockSharp.Algo.Statistics.ITradeStatisticParameter.html), [IOrderStatisticParameter](../api/StockSharp.Algo.Statistics.IOrderStatisticParameter.html) или [IPositionStatisticParameter](../api/StockSharp.Algo.Statistics.IPositionStatisticParameter.html). Если требуется особый параметр расчета, то необходимо реализовать один из этих интерфейсов и добавить параметр в [StatisticManager.Parameters](../api/StockSharp.Algo.Statistics.StatisticManager.Parameters.html). 

В разделе [тестирования на истории](StrategyTestingHistory.md) показано использование [Strategy.PnLChanged](../api/StockSharp.Algo.Strategies.Strategy.PnLChanged.html) применительно к [торговым стратегиям](Strategy.md): 

```cs
_strategy.PnLChanged += () =>
{
	var pnl = new EquityData
	{
		Time = strategy.CurrentTime,
		Value = strategy.PnL - strategy.Commission ?? 0
	};
	pnlCurve.Add(pnl);
};      
      
```

Менеджер статистики получается через свойство [Strategy.StatisticManager](../api/StockSharp.Algo.Strategies.Strategy.StatisticManager.html). 

При появлении новых данных, кривая будет отрисовываться на специальном графике [EquityCurveChart](../api/StockSharp.Xaml.Charting.EquityCurveChart.html). Чтобы начать рисовать кривую на данном графике, необходимо вызвать метод [Overload:StockSharp.Xaml.Charting.EquityCurveChart.CreateCurve](../api/Overload:StockSharp.Xaml.Charting.EquityCurveChart.CreateCurve.html). Полученная коллекция заполняется данными, которые передаются во время обработки события [Strategy.PnLChanged](../api/StockSharp.Algo.Strategies.Strategy.PnLChanged.html). 

График [EquityCurveChart](../api/StockSharp.Xaml.Charting.EquityCurveChart.html) позволяет рисовать одновременно несколько кривых, чтобы иметь возможность их сравнивать по доходности друг с другом. Пример такого подхода показан в разделе [тестирования на истории (оптимизация)](StrategyTestingOptimization.md). 

В разделе [тестирования на истории](StrategyTestingHistory.md) показано использование визуальной панели [StatisticParameterGrid](../api/StockSharp.Xaml.StatisticParameterGrid.html), которая позволяет отображать параметры [StatisticParameterGrid.Parameters](../api/StockSharp.Xaml.StatisticParameterGrid.Parameters.html). 
