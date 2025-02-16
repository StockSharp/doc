# Визуальный мониторинг

Для упрощения мониторинга работы можно использовать специальную компоненту [Monitor](xref:StockSharp.Xaml.Monitor). См. также [Визуальные компоненты логирования](../graphical_user_interface/logging.md). 

![GUI LogControl](../../../images/gui_logcontrol.png)

Данное окно позволяет выводить сообщения от всех [ILogSource](xref:Ecng.Logging.ILogSource): 

- стратегий ([Strategy](xref:StockSharp.Algo.Strategies.Strategy));
- подключений ([IConnector](xref:StockSharp.BusinessEntities.IConnector));
- собственных реализаций [ILogSource](xref:Ecng.Logging.ILogSource) (например, главное окно в роботе).

В виде дерева показывается вложенность источников. Каждая родительская вершина содержит сообщения всех вложенных и так далее, до самого нижнего уровня. Для подключений это также полезно в случае использования [множественных подключений](../connectors.md). Аналогично, такую же вложенность можно организовать и для собственного робота, реализовав свойство [ILogSource.Parent](xref:Ecng.Logging.ILogSource.Parent). 

## Использование Monitor

1. Вначале необходимо создать окно и добавить компоненту.
2. Далее, созданное окно необходимо через [GuiLogListener](xref:StockSharp.Xaml.GuiLogListener) добавить в свой [LogManager](xref:Ecng.Logging.LogManager):

   ```cs
   _logManager.Listeners.Add(new GuiLogListener(monitor));
   ```
3. После этого все источники [LogManager.Sources](xref:Ecng.Logging.LogManager.Sources) (стратегии, подключения и т.д.), будут посылать сообщения в [Monitor](xref:StockSharp.Xaml.Monitor).

## См. также

[Визуальные компоненты логирования](../graphical_user_interface/logging.md)
