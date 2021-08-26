# Дочерние стратегии

С помощью [Strategy](../api/StockSharp.Algo.Strategies.Strategy.html) можно так же создавать дочерние стратегии. Например, когда требуется реализовать процесс торговли, который оперирует стандартными (или уже ранее реализованными) алгоритмами, выстраивая их в единую цепочку: 

![strategychilds](../images/strategy_childs.png)

### Предварительные условия

[Создание стратегии](StrategyCreate.md)

### Работа с дочерними стратегиями

Работа с дочерними стратегиями

Чтобы добавить дочернюю стратегию необходимо использовать свойство [Strategy.ChildStrategies](../api/StockSharp.Algo.Strategies.Strategy.ChildStrategies.html). Значения [Strategy.Connector](../api/StockSharp.Algo.Strategies.Strategy.Connector.html), [Strategy.Portfolio](../api/StockSharp.Algo.Strategies.Strategy.Portfolio.html) и [Strategy.Security](../api/StockSharp.Algo.Strategies.Strategy.Security.html) для дочерних стратегий можно не устанавливать, и они автоматически заполнятся при добавлении в родительскую стратегию. 

При добавлении новой дочерней стратегии она автоматически получает состояние из родительской. Например, если родительская стратегия находится в работающем состоянии ([ProcessStates.Started](../api/StockSharp.Algo.ProcessStates.Started.html)), то дочерняя автоматически будет установлена в работающее состояние (и, наоборот, в случае с [ProcessStates.Stopped](../api/StockSharp.Algo.ProcessStates.Stopped.html)). Поэтому вызывать метод [Strategy.Start](../api/StockSharp.Algo.Strategies.Strategy.Start.html) для дочерней стратегии не нужно. Он автоматически будет вызван, при запуске родительской стратегии. Таким образом, вызывать метод [Strategy.Start](../api/StockSharp.Algo.Strategies.Strategy.Start.html) можно только для тех стратегий, которые являются корневыми. 

Родительская и все ее дочерние стратегии исполняются параллельно. Это значит, что, если была выбрана [итерационная модель](StrategyCreate.md), то методы [TimeFrameStrategy.OnProcess](../api/StockSharp.Algo.Strategies.TimeFrameStrategy.OnProcess.html) для дочерних и родительских стратегий выполняются параллельно. Аналогично и с правилами [Strategy.Rules](../api/StockSharp.Algo.Strategies.Strategy.Rules.html), если была выбрана [событийная модель](StrategyAction.md). 

> [!TIP]
> Метод [Strategy.Stop](../api/StockSharp.Algo.Strategies.Strategy.Stop.html) для дочерней стратегии, в отличие от [Strategy.Start](../api/StockSharp.Algo.Strategies.Strategy.Start.html), можно вызывать в любое время. Например, когда алгоритму требуется принудительно остановить работу дочерней стратегии при том, что сам алгоритм дочерней стратегии еще не выполнился до конца (например, прервать [Котирование](StrategyQuoting.md)). 

По умолчанию, дочерние стратегии не связаны друг с другом, и исполняются независимо. Когда требуется установить зависимость между стратегиями, необходимо использовать класс [BasketStrategy](../api/StockSharp.Algo.Strategies.BasketStrategy.html). Данный класс позволяет задать условия завершения стратегий в зависимости друг от друга через признаки [BasketStrategyFinishModes](../api/StockSharp.Algo.Strategies.BasketStrategyFinishModes.html). Например, через значение [First](../api/StockSharp.Algo.Strategies.BasketStrategyFinishModes.First.html) задается условие, при котором все дочерние стратегии будут остановлены, когда исполнится хотя бы одна из них. Пример использования [BasketStrategy](../api/StockSharp.Algo.Strategies.BasketStrategy.html) показан в разделе [Тейк\-профит и стоп\-лосс](StrategyProtective.md). 

### Следующие шаги

[Котирование](StrategyQuoting.md)

[Тейк\-профит и стоп\-лосс](StrategyProtective.md)

## См. также
