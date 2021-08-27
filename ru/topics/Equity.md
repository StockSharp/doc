# Кривая эквити

[S\#](StockSharpAbout.md) предусмотрена возможность построения кривых доходности (эквити) с целью дальнейшего анализа работы торгового робота. Данные для кривой получаются с помощью класса [PnLManager](xref:StockSharp.Algo.PnL.PnLManager). Данный класс производит расчёт текущего значения, оповещая робота об изменениях через событие [Strategy.PnLChanged](xref:StockSharp.Algo.Strategies.Strategy.PnLChanged). 

Для расчета параметров кривой (максимальная просадка, коэффициент Шарпа и т.д.) используется [StatisticManager](xref:StockSharp.Algo.Statistics.StatisticManager). Эти параметры хранятся в свойстве [StatisticManager.Parameters](xref:StockSharp.Algo.Statistics.StatisticManager.Parameters). Каждый параметр реализует интерфейсы [IPnLStatisticParameter](xref:StockSharp.Algo.Statistics.IPnLStatisticParameter), [ITradeStatisticParameter](xref:StockSharp.Algo.Statistics.ITradeStatisticParameter), [IOrderStatisticParameter](xref:StockSharp.Algo.Statistics.IOrderStatisticParameter) или [IPositionStatisticParameter](xref:StockSharp.Algo.Statistics.IPositionStatisticParameter). Если требуется особый параметр расчета, то необходимо реализовать один из этих интерфейсов и добавить параметр в [StatisticManager.Parameters](xref:StockSharp.Algo.Statistics.StatisticManager.Parameters). 

В разделе [тестирования на истории](StrategyTestingHistory.md) показано использование [Strategy.PnLChanged](xref:StockSharp.Algo.Strategies.Strategy.PnLChanged) применительно к [торговым стратегиям](Strategy.md): 

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

Менеджер статистики получается через свойство [Strategy.StatisticManager](xref:StockSharp.Algo.Strategies.Strategy.StatisticManager). 

При появлении новых данных, кривая будет отрисовываться на специальном графике [EquityCurveChart](xref:StockSharp.Xaml.Charting.EquityCurveChart). Чтобы начать рисовать кривую на данном графике, необходимо вызвать метод [Overload:StockSharp.Xaml.Charting.EquityCurveChart.CreateCurve](xref:Overload:StockSharp.Xaml.Charting.EquityCurveChart.CreateCurve). Полученная коллекция заполняется данными, которые передаются во время обработки события [Strategy.PnLChanged](xref:StockSharp.Algo.Strategies.Strategy.PnLChanged). 

График [EquityCurveChart](xref:StockSharp.Xaml.Charting.EquityCurveChart) позволяет рисовать одновременно несколько кривых, чтобы иметь возможность их сравнивать по доходности друг с другом. Пример такого подхода показан в разделе [тестирования на истории (оптимизация)](StrategyTestingOptimization.md). 

В разделе [тестирования на истории](StrategyTestingHistory.md) показано использование визуальной панели [StatisticParameterGrid](xref:StockSharp.Xaml.StatisticParameterGrid), которая позволяет отображать параметры [StatisticParameterGrid.Parameters](xref:StockSharp.Xaml.StatisticParameterGrid.Parameters). 
