# Логирование

Для мониторинга работы торговых роботов, написанных на [S\#](../api.md), можно использовать специальный класс [LogManager](xref:Ecng.Logging.LogManager). Данный класс принимает сообщения [LogMessage](xref:Ecng.Logging.LogMessage) от источников [LogManager.Sources](xref:Ecng.Logging.LogManager.Sources) через событие [ILogSource.Log](xref:Ecng.Logging.ILogSource.Log) и передает их в слушатели [LogManager.Listeners](xref:Ecng.Logging.LogManager.Listeners). Таким образом код робота сможет передавать отладочную информацию (например, об ошибках, случившихся в процессе работы, или дополнительную информацию о математических расчетах), а [LogManager](xref:Ecng.Logging.LogManager) будет решать, как отобразить для оператора данную информацию. 

Стандартно в [S\#](../api.md) входят следующие реализации [ILogListener](xref:Ecng.Logging.ILogListener), выбор которых влияет на то, куда будут переданы поступившие от стратегии сообщения: 

1. [FileLogListener](xref:Ecng.Logging.FileLogListener) \- записывает сообщения в текстовый файл. Рекомендуется использовать для уже сделанного робота, и использовать логи в форс\-мажорных ситуациях. 
2. [ConsoleLogListener](xref:Ecng.Logging.ConsoleLogListener) \- записывает сообщения в консольное окно (если робот не имеет такое окно, то оно будет автоматически создано). Рекомендуется использовать для отладки и тестирования робота. 
3. [DebugLogListener](xref:Ecng.Logging.DebugLogListener) \- записывает сообщения в отладочное окно. Такое окно можно просматривать через специальные программы, такие как [DebugView](https://technet.microsoft.com/en-us/sysinternals/bb896647.aspx). Рекомендуется использовать для отладки и тестирования робота. 
4. [EmailLogListener](xref:Ecng.Logging.EmailLogListener) \- отсылает сообщения на указанный email адрес. Рекомендуется использовать, если робот расположен на непостоянно контролируемом компьютере (на сервере у хостера). 
5. [GuiLogListener](xref:StockSharp.Xaml.GuiLogListener) \- отображает сообщения через специальное окно [LogControl](xref:StockSharp.Xaml.LogControl). Умеет работать в двух режимах: когда все сообщения пишутся в одно окно, и когда для каждого [ILogSource](xref:Ecng.Logging.ILogSource) создается отдельное окно. Рекомендуется использовать, если робот обладает графическим интерфейсом. 

[LogListener](xref:Ecng.Logging.LogListener) можно настроить на фильтрацию сообщений через свойство [LogListener.Filters](xref:Ecng.Logging.LogListener.Filters). Например, через фильтры можно задать, какой тип сообщений стоит обрабатывать. Это особенно полезно в случаях использования [EmailLogListener](xref:Ecng.Logging.EmailLogListener), чтобы, к примеру, посылать e\-mail только в критических ситуациях (ошибка алгоритма торговли), а не на каждое отладочное сообщение. 

## Следующие шаги

[Логирование Strategy](logging/strategy_logging.md)

[Логирование IConnector](logging/iconnector_logging.md)

[Другие источники логов](logging/other_logs_sources.md)

[Визуальный мониторинг](logging/visual_monitoring.md)

[Создание ILogListener](logging/custom_iloglistener.md)
