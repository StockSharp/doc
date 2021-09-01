# Управление окном Quik

[QuikTerminal](xref:StockSharp.Quik.QuikTerminal) позволяет не только производить [Включение и выключение Quik](QuikProcess.md), но так же осуществлять некоторые действия с окном терминала. Для [QuikTrader](xref:StockSharp.Quik.QuikTrader)[QuikTerminal](xref:StockSharp.Quik.QuikTerminal) проделывает всю низкоуровневую работу c окном [Quik](Quik.md), скрывая реализацию межпрограммного взаимодействия: 

- [GetTableSettings](xref:StockSharp.Quik.QuikTerminal.GetTableSettings(StockSharp.Quik.DdeTable[])) проверяет правильность настроек таблиц (подробнее в разделе [Verifier](QuikVerifier.md)). 
- [OpenQuotes](xref:StockSharp.Quik.QuikTerminal.OpenQuotes(StockSharp.Messages.SecurityId)) открывает и настраивает окно со стаканом для переданного инструмента. 
- [GetAccounts](xref:StockSharp.Quik.QuikTerminal.GetAccounts) возвращает список счетов. 
- [RegisterTrades](xref:StockSharp.Quik.QuikTerminal.RegisterTrades(System.String)) и [UnRegisterTrades](xref:StockSharp.Quik.QuikTerminal.UnRegisterTrades(System.String)) настраивает фильтр в таблице Все Сделки (нужно включить режим [EnableFiltering](xref:StockSharp.Quik.QuikTerminal.EnableFiltering) ). 

Если необходимо произвести какое\-то действие с окном [Quik](Quik.md), но его нет среди методов [QuikTerminal](xref:StockSharp.Quik.QuikTerminal), то можно воспользоваться свойством [QuikTerminal.MainWindow](xref:StockSharp.Quik.QuikTerminal.MainWindow). Оно возвращает системное описание окна, через которое можно посылать [Windows сообщения](https://msdn.microsoft.com/en-us/library/ms644927(VS.85).aspx). 
