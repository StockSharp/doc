# Логирование

Для мониторинга работы торговых роботов, написанных на [S\#](StockSharpAbout.md), можно использовать специальный класс [LogManager](xref:StockSharp.Logging.LogManager). Данный класс принимает сообщения [LogMessage](xref:StockSharp.Logging.LogMessage) от источников [LogManager.Sources](xref:StockSharp.Logging.LogManager.Sources) через событие [ILogSource.Log](xref:StockSharp.Logging.ILogSource.Log) и передает их в слушатели [LogManager.Listeners](xref:StockSharp.Logging.LogManager.Listeners). Таким образом код робота сможет передавать отладочную информацию (например, об ошибках, случившихся в процессе работы, или дополнительную информацию о математических расчетах), а [LogManager](xref:StockSharp.Logging.LogManager) будет решать, как отобразить для оператора данную информацию. 

Стандартно в [S\#](StockSharpAbout.md) входят следующие реализации [ILogListener](xref:StockSharp.Logging.ILogListener), выбор которых влияет на то, куда будут переданы поступившие от стратегии сообщения: 

1. [FileLogListener](xref:StockSharp.Logging.FileLogListener) \- записывает сообщения в текстовый файл. Рекомендуется использовать для уже сделанного робота, и использовать логи в форс\-мажорных ситуациях. 
2. [ConsoleLogListener](xref:StockSharp.Logging.ConsoleLogListener) \- записывает сообщения в консольное окно (если робот не имеет такое окно, то оно будет автоматически создано). Рекомендуется использовать для отладки и тестирования робота. 
3. [DebugLogListener](xref:StockSharp.Logging.DebugLogListener) \- записывает сообщения в отладочное окно. Такое окно можно просматривать через специальные программы, такие как [DebugView](https://technet.microsoft.com/en-us/sysinternals/bb896647.aspx). Рекомендуется использовать для отладки и тестирования робота. 
4. [EmailLogListener](xref:StockSharp.Logging.EmailLogListener) \- отсылает сообщения на указанный email адрес. Рекомендуется использовать, если робот расположен на непостоянно контролируемом компьютере (на сервере у хостера). 
5. [GuiLogListener](xref:StockSharp.Xaml.GuiLogListener) \- отображает сообщения через специальное окно [LogControl](xref:StockSharp.Xaml.LogControl). Умеет работать в двух режимах: когда все сообщения пишутся в одно окно, и когда для каждого [ILogSource](xref:StockSharp.Logging.ILogSource) создается отдельное окно. Рекомендуется использовать, если робот обладает графическим интерфейсом. 

[LogListener](xref:StockSharp.Logging.LogListener) можно настроить на фильтрацию сообщений через свойство [LogListener.Filters](xref:StockSharp.Logging.LogListener.Filters). Например, через фильтры можно задать, какой тип сообщений стоит обрабатывать. Это особенно полезно в случаях использования [EmailLogListener](xref:StockSharp.Logging.EmailLogListener), чтобы, к примеру, посылать e\-mail только в критических ситуациях (ошибка алгоритма торговли), а не на каждое отладочное сообщение. 

## Следующие шаги

[Логирование Strategy](LoggingStrategy.md)

[Логирование IConnector](LoggingITrader.md)

[Другие источники логов](AppLogging.md)

[Визуальный мониторинг](LoggingMonitorWindow.md)

[Создание ILogListener](LoggingCustomListener.md)
