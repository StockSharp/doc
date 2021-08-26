# Визуальный мониторинг

Для упрощения мониторинга работы можно использовать специальное окно [MonitorWindow](../api/StockSharp.Xaml.MonitorWindow.html). См. также [Визуальные компоненты логирования](GuiLogging.md). 

![GUI LogControl](../images/GUI_LogControl.png)

Данное окно позволяет выводить сообщения от всех [ILogSource](../api/StockSharp.Logging.ILogSource.html): 

- стратегий (

  [Strategy](../api/StockSharp.Algo.Strategies.Strategy.html)

  );
- подключений (

  [IConnector](../api/StockSharp.BusinessEntities.IConnector.html)

  );
- собственных реализаций 

  [ILogSource](../api/StockSharp.Logging.ILogSource.html)

   (например, главное окно в роботе).

В виде дерева показывается вложенность источников. Каждая родительская вершина содержит сообщения всех вложенных и так далее, до самого нижнего уровня. Для стратегий такая иерархия позволяет увидеть [дочерние стратегии](StrategyChilds.md). Для подключений это также полезно в случае использования [множественных подключений](API_Connectors.md). Аналогично, такую же вложенность можно организовать и для собственного робота, реализовав свойство [ILogSource.Parent](../api/StockSharp.Logging.ILogSource.Parent.html). 

### Использование MonitorWindow

Использование MonitorWindow

1. Вначале необходимо создать окно:

   ```cs
   var monitor = new MonitorWindow();
   monitor.Show();
   ```
2. Далее, созданное окно необходимо через [GuiLogListener](../api/StockSharp.Xaml.GuiLogListener.html) добавить в свой [LogManager](../api/StockSharp.Logging.LogManager.html):

   ```cs
   _logManager.Listeners.Add(new GuiLogListener(monitor));
   ```
3. После этого все источники [LogManager.Sources](../api/StockSharp.Logging.LogManager.Sources.html) (стратегии, подключения и т.д.), будут посылать сообщения в [MonitorWindow](../api/StockSharp.Xaml.MonitorWindow.html).

## См. также

[Визуальные компоненты логирования](GuiLogging.md)
