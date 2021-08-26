# Управление окном Quik

[QuikTerminal](../api/StockSharp.Quik.QuikTerminal.html) позволяет не только производить [Включение и выключение Quik](QuikProcess.md), но так же осуществлять некоторые действия с окном терминала. Для [QuikTrader](../api/StockSharp.Quik.QuikTrader.html)[QuikTerminal](../api/StockSharp.Quik.QuikTerminal.html) проделывает всю низкоуровневую работу c окном [Quik](Quik.md), скрывая реализацию межпрограммного взаимодействия: 

- [GetTableSettings](../api/StockSharp.Quik.QuikTerminal.GetTableSettings.html)

   проверяет правильность настроек таблиц (подробнее в разделе 

  [Verifier](QuikVerifier.md)

  ). 
- [OpenQuotes](../api/StockSharp.Quik.QuikTerminal.OpenQuotes.html)

   открывает и настраивает окно со стаканом для переданного инструмента. 
- [GetAccounts](../api/StockSharp.Quik.QuikTerminal.GetAccounts.html)

   возвращает список счетов. 
- [RegisterTrades](../api/StockSharp.Quik.QuikTerminal.RegisterTrades.html)

   и 

  [UnRegisterTrades](../api/StockSharp.Quik.QuikTerminal.UnRegisterTrades.html)

   настраивает фильтр в таблице Все Сделки (нужно включить режим 

  [EnableFiltering](../api/StockSharp.Quik.QuikTerminal.EnableFiltering.html)

   ). 

Если необходимо произвести какое\-то действие с окном [Quik](Quik.md), но его нет среди методов [QuikTerminal](../api/StockSharp.Quik.QuikTerminal.html), то можно воспользоваться свойством [QuikTerminal.MainWindow](../api/StockSharp.Quik.QuikTerminal.MainWindow.html). Оно возвращает системное описание окна, через которое можно посылать [Windows сообщения](https://msdn.microsoft.com/en-us/library/ms644927(VS.85).aspx). 

## См. также
