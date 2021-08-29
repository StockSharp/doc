# Работа с несколькими Quik\-ами

[S\#](StockSharpAbout.md) позволяет работать с несколькими [Quik](Quik.md)\-ами одновременно из одного робота. К примеру, это необходимо, когда идет торговля на разных площадках через разных брокеров. 

### Пример работы с несколькими Quik\-ами

1. В папку с программой необходимо поместить два файла *TRANS2QUIK.dll* (пример работает с 2\-мя [Quik](Quik.md)\-ами). Например, один будет иметь оригинальное название, а второй будет переименован *TRANS2QUIK\_2.dll*. Это обязательное требование работы с несколькими [Quik](Quik.md)\-ами из одной программы: один [Quik](Quik.md) \- одна dll. 
2. После этого, идет создание нескольких [QuikTrader](xref:StockSharp.Quik.QuikTrader). Через свойство [QuikTrader.DdeServer](xref:StockSharp.Quik.QuikTrader.DdeServer) передается уникальное имя для [DDE](https://en.wikipedia.org/wiki/Dynamic_Data_Exchange) сервера, а через [QuikTrader.DllName](xref:StockSharp.Quik.QuikTrader.DllName) путь к *TRANS2QUIK.dll*: 

   ```cs
   var quikTrader1 = new QuikTrader { Path = path1, DdeServer = "quik1" };
   var quikTrader2 = new QuikTrader { Path = path2, DdeServer = "quik2", DllName = @"TRANS2QUIK_2.dll" };
   				
   // если вторая dll находится в другой папке, то можно создать шлюз, указав путь к dll
   // var quikTrader2 = new QuikTrader { Path = path2, DdeServer = "quik2", DllName = @"Folder1\TRANS2QUIK_2.dll" };
   ```
3. Когда [QuikTrader](xref:StockSharp.Quik.QuikTrader)\-ы созданы, идет обычная работа с каждом из них: 

   ```cs
   // подписываемся на событие ошибок обработки данных и разрыва соединения
   //
   quikTrader1.Error += OnError;
   quikTrader2.Error += OnError;
   quikTrader1.ConnectionError += OnError;
   quikTrader2.ConnectionError += OnError;
   var portfoliosWait = new ManualResetEvent(false);
   Action<Portfolio> newPortfolio = portfolio =>
   {
   	if (_portfolio1 == null && portfolio.Name == account1)
   		_portfolio1 = portfolio;
   	if (_portfolio2 == null && portfolio.Name == account2)
   		_portfolio2 = portfolio;
   	// если оба инструмента появились
   	if (_portfolio1 != null && _portfolio2 != null)
   		portfoliosWait.Set();
   };
   // подписываемся на события новых портфелей
   quikTrader1.NewPortfolio += newPortfolio;
   quikTrader2.NewPortfolio += newPortfolio;
   var securitiesWait = new ManualResetEvent(false);
   // подписываемся на события новых инструментов
   quikTrader1.NewSecurity += security =>
   {
   	if (_lkoh == null && security.Code == "LKOH")
   		_lkoh = security;
   	// если оба инструмента появились
   	if (_lkoh != null && _ri != null)
   		securitiesWait.Set();
   };
   quikTrader2.NewSecurity += security =>
   {
   	if (_ri == null && security.Code == "RIZ7")
   		_ri = security;
   	// если оба инструмента появились
   	if (_lkoh != null && _ri != null)
   		securitiesWait.Set();
   };
   // запускаем экспорты в Quik-ах, когда получим событие об успешном соединении
   //
   quikTrader1.Connected += () => quikTrader1.StartExport(new[] {quikTrader1.SecuritiesTable});
   quikTrader2.Connected += () => quikTrader2.StartExport(new[] {quikTrader2.SecuritiesTable});
   // производим подключение каждого из QuikTrader-а
   //
   quikTrader1.Connect();
   quikTrader2.Connect();
   Console.WriteLine("Дожидаемся появления инструментов и портфелей...");
   portfoliosWait.WaitOne();
   securitiesWait.WaitOne();
   Console.WriteLine("Информация появилась. Производим регистрацию заявок...");
   if (_lkoh.BestBid == null || _riz0.BestBid == null)
   	throw new Exception("Нет лучшего бида для котировки.");
   quikTrader1.RegisterOrder(new Order
   {
   	Portfolio = _portfolio1,
   	Volume = 1,
   	Security = _lkoh,
   	Price = _lkoh.BestBid.Price
   });
   Console.WriteLine("Заявка на LKOH зарегистрирована");
   quikTrader2.RegisterOrder(new Order
   {
   	Portfolio = _portfolio2,
   	Volume = 1,
   	Security = _riz0,
   	Price = _riz0.BestBid.Price
   });
   Console.WriteLine("Заявка на RIZ0 зарегистрирована");
   ```

   > [!CAUTION]
   > Номера счетов, которые в примере записаны в переменные **account1** и **account2**, это **не логины** в [Quik](Quik.md), а коды клиентов. Об особенности портфелей в Quik читайте в соответствующем [разделе](QuikPortfolio.md). 
4. Для более удобной работы с несколькими [QuikTrader](xref:StockSharp.Quik.QuikTrader)\-ами [S\#](StockSharpAbout.md) предоставляет [Коннекторы](API_Connectors.md). 
5. Исходные коды примера лежат в папке *SampleFewQuiks*. 

### Следующие шаги

[Включение и выключение Quik](QuikProcess.md)
