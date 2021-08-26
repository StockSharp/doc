# Произвольные таблицы

Помимо стандартного набора таблиц шлюз [PlazaMessageAdapter](../api/StockSharp.Plaza.PlazaMessageAdapter.html) поддерживает дополнительные таблицы. Таблицы, по которым возможно получение данных, описаны в пространстве имен [Metadata](../api/StockSharp.Plaza.Metadata.html). Для получения информации по таблице необходимо добавить поток данных, соответствующий данной таблице, в [Tables](../api/StockSharp.Plaza.PlazaMessageAdapter.Tables.html). Все потоки данных описаны в классе [PlazaStreamRegistry](../api/StockSharp.Plaza.PlazaStreamRegistry.html). 

```cs
// Добавление таблицы об обязательствах маркетмейкера по фьючерсам
Connector Connector = new Connector();				
...				
var address = "<Address>".To<IPAddress>();
var messageAdapter = new PlazaMessageAdapter(Connector.TransactionIdGenerator)
{
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
// добавляем новую таблицу к стандартным
messageAdapter.Tables = messageAdapter.Tables.Concat(new[] { messageAdapter.TableRegistry.MarketMakingFuture.Id });
```

После добавления таблицы вся информация по ней будет приходить в обработчики событий [Inserted](../api/StockSharp.Plaza.Metadata.PlazaTable.Inserted.html) и [End](../api/StockSharp.Plaza.Metadata.PlazaTable.End.html).

```cs
//Подписка на событие добавления информации по таблице
messageAdapter.StreamManager.Inserted += OnStreamInserted;
void OnStreamInserted(PlazaStream stream, PlazaTable table, PlazaRecord record)
{
	// обрабатываем только нужную таблицу
	if (table != messageAdapter.TableRegistry.MarketMakingFuture)
		return;
		
    //Метадата для колонок таблицы обязательств ММ по фьючерсам
    var metadata = messageAdapter.TableRegistry.ColumnRegistry.MarketMakingFutureParams;
    //Получение текущего процента выполнения обязательств
    var percentTime = record.Get<decimal>(metadata.PercentTime);
    Console.WriteLine("Текущий процент выполнения обязательств: {0}".Put(percentTime));
}
```

### Следующие шаги

[Подключение нескольких роботов к одному роутеру](PlazaSingleRouter.md)

## См. также
