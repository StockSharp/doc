# Управление ревизиями

Ревизия \- уникальный номер строчки в таблице с данными.

[PlazaRevisionManager](../api/StockSharp.Plaza.PlazaRevisionManager.html) \- класс отслеживающий изменение ревизий по выбранным таблицам.

Для того чтобы не скачивать уже полученные данные при переконнекте, необходимо добавить нужные таблицы в [PlazaRevisionManager.Tables](../api/StockSharp.Plaza.PlazaRevisionManager.Tables.html):

```cs
public PlazaMessageAdapter Adapter;
...
		
var revisionManager = Adapter.StreamManager.RevisionManager;
revisionManager.Tables.Add(Adapter.TableRegistry.IndexLog);
revisionManager.Tables.Add(Adapter.TableRegistry.TradeFuture);
revisionManager.Tables.Add(Adapter.TableRegistry.TradeOption);
		
```

После переконнекта, данные начнут скачиваться с номера последней сохраненной ревизии. Интервал через который сохраняется номер последней ревизии доступен через [PlazaRevisionManager.Interval](../api/StockSharp.Plaza.PlazaRevisionManager.Interval.html). По умолчанию файл с номером последней ревизии по выбранной таблице сохраняется в папку *Revisions*, которая создается в папке из которой запускается торговый робот. Изменить путь сохранения можно через [PlazaMessageAdapter.RevisionPath](../api/StockSharp.Plaza.PlazaMessageAdapter.RevisionPath.html).

Для того чтобы перезакачать все данные, достаточно удалить файлы из папки *Revisions*.
