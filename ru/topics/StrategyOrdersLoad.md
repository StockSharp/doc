# Загрузка заявок и сделок

При старте стратегии может возникнуть необходимость загрузки ранее совершённых заявок и сделок (например, когда робот был перезагружен в течении торговой сессии или сделки и заявки переносятся через ночь). Для этого нужно: 

1. Найти те заявки, которые необходимо загрузить в стратегию, и вернуть их из метода (например, загрузить идентификаторы заявок, если стратегия записывает каждый раз при регистрации через метод [Strategy.ProcessNewOrders](xref:StockSharp.Algo.Strategies.Strategy.ProcessNewOrders) из файла). 
2. Объединить полученный результат с базовым методом [Strategy.ProcessNewOrders](xref:StockSharp.Algo.Strategies.Strategy.ProcessNewOrders). 
3. После того, как заявки будут загружены в стратегию, загрузятся и все совершенные по ним сделки. Это будет сделано автоматически. 

Следующий пример показывает загрузку всех сделок в стратегию: 

## Загрузка в стратегию ранее совершенных заявок и сделок

1. Для этого, чтобы [Strategy](xref:StockSharp.Algo.Strategies.Strategy) загрузила свое предыдущее состояние, необходимо переопределить [Strategy.ProcessNewOrders](xref:StockSharp.Algo.Strategies.Strategy.ProcessNewOrders). На вход данному методу из [Strategy.OnStarted](xref:StockSharp.Algo.Strategies.Strategy.OnStarted) поступят все [IConnector.Orders](xref:StockSharp.BusinessEntities.IConnector.Orders) и [IConnector.StopOrders](xref:StockSharp.BusinessEntities.IConnector.StopOrders), и их необходимо отфильтровать:

   ```cs
   private bool _isOrdersLoaded;
   private bool _isStopOrdersLoaded;
   		  	
   protected override IEnumerable<Order> ProcessNewOrders(IEnumerable<Order> newOrders, bool isStopOrders)
   {
   	// если заявки уже были ранее загружены
   	if ((!isStopOrders && _isOrdersLoaded) || (isStopOrders && _isStopOrdersLoaded))
   		return base.ProcessNewOrders(newOrders, isStopOrders);
   	return Filter(newOrders);
   }
   ```
2. Чтобы реализовать фильтрацию заявок, необходимо определить критерий отсеивания. Например, если в процессе работы стратегии сохранять все регистрируемые заявки в файл, то можно сделать фильтр по номеру транзакции [Order.TransactionId](xref:StockSharp.BusinessEntities.Order.TransactionId). Если такой номер присутствует в файле, значит заявка была зарегистрирована через данную стратегию: 

   ```cs
   private IEnumerable<Order> Filter(IEnumerable<Order> orders)
   {
   	// считываем номера транзакций из файла
   	var transactions = File.ReadAllLines("orders_{0}.txt".Put(Name)).Select(l => l.To<long>()).ToArray();
   	
   	// находим наши заявки по считанным номерам
   	return orders.Where(o => transactions.Contains(o.TransactionId));
   }
   ```
3. Запись номеров транзакций заявок, регистрируемых через стратегию, можно осуществить, переопределив метод [Strategy.RegisterOrder](xref:StockSharp.Algo.Strategies.Strategy.RegisterOrder): 

   ```cs
   protected override void RegisterOrder(Order order)
   {
   	// отравляем заявку дальше на регистрацию
   	base.RegisterOrder(order);
   	
   	// добавляем новый номер транзакции
   	File.AppendAllLines("orders_{0}.txt".Put(Name), new[]{ order.TransactionId.ToString() });
   }
   ```
4. После того, как заявки будут загружены в стратегию, загрузятся и все совершенные по ним сделки. Это будет сделано автоматически. 
