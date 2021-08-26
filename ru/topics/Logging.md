# Логирование

Для мониторинга работы торговых роботов, написанных на [S\#](StockSharpAbout.md), можно использовать специальный класс [LogManager](../api/StockSharp.Logging.LogManager.html). Данный класс принимает сообщения [LogMessage](../api/StockSharp.Logging.LogMessage.html) от источников [LogManager.Sources](../api/StockSharp.Logging.LogManager.Sources.html) через событие [ILogSource.Log](../api/StockSharp.Logging.ILogSource.Log.html) и передает их в слушатели [LogManager.Listeners](../api/StockSharp.Logging.LogManager.Listeners.html). Таким образом код робота сможет передавать отладочную информацию (например, об ошибках, случившихся в процессе работы, или дополнительную информацию о математических расчетах), а [LogManager](../api/StockSharp.Logging.LogManager.html) будет решать, как отобразить для оператора данную информацию. 

Стандартно в [S\#](StockSharpAbout.md) входят следующие реализации [ILogListener](../api/StockSharp.Logging.ILogListener.html), выбор которых влияет на то, куда будут переданы поступившие от стратегии сообщения: 

1. [FileLogListener](../api/StockSharp.Logging.FileLogListener.html)

    \- записывает сообщения в текстовый файл. Рекомендуется использовать для уже сделанного робота, и использовать логи в форс\-мажорных ситуациях. 
2. [SoundLogListener](../api/StockSharp.Xaml.SoundLogListener.html)

    \- проигрывает звуковое сообщение когда приходит новое сообщение. Рекомендуется использовать, если за роботом нет постоянного внимания. 
3. [ConsoleLogListener](../api/StockSharp.Logging.ConsoleLogListener.html)

    \- записывает сообщения в консольное окно (если робот не имеет такое окно, то оно будет автоматически создано). Рекомендуется использовать для отладки и тестирования робота. 
4. [DebugLogListener](../api/StockSharp.Logging.DebugLogListener.html)

    \- записывает сообщения в отладочное окно. Такое окно можно просматривать через специальные программы, такие как 

   [DebugView](https://technet.microsoft.com/en-us/sysinternals/bb896647.aspx)

   . Рекомендуется использовать для отладки и тестирования робота. 
5. [EmailLogListener](../api/StockSharp.Logging.EmailLogListener.html)

    \- отсылает сообщения на указанный email адрес. Рекомендуется использовать, если робот расположен на непостоянно контролируемом компьютере (на сервере у хостера). 
6. [GuiLogListener](../api/StockSharp.Xaml.GuiLogListener.html)

    \- отображает сообщения через специальное окно 

   [LogWindow](../api/StockSharp.Xaml.LogWindow.html)

   . Умеет работать в двух режимах: когда все сообщения пишутся в одно окно, и когда для каждого 

   [ILogSource](../api/StockSharp.Logging.ILogSource.html)

    создается отдельное окно. Рекомендуется использовать, если робот обладает графическим интерфейсом. 

[LogListener](../api/StockSharp.Logging.LogListener.html) можно настроить на фильтрацию сообщений через свойство [Filters](../api/StockSharp.Logging.LogListener.Filters.html). Например, через фильтры можно задать, какой тип сообщений стоит обрабатывать. Это особенно полезно в случаях использования [SoundLogListener](../api/StockSharp.Xaml.SoundLogListener.html) или [EmailLogListener](../api/StockSharp.Logging.EmailLogListener.html), чтобы, к примеру, посылать e\-mail только в критических ситуациях (ошибка алгоритма торговли), а не на каждое отладочное сообщение. 

### Следующие шаги

[Логирование Strategy](LoggingStrategy.md)

[Логирование IConnector](LoggingITrader.md)

[Другие источники логов](AppLogging.md)

[Визуальный мониторинг](LoggingMonitorWindow.md)

[Создание ILogListener](LoggingCustomListener.md)

## См. также
