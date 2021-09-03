# Управление ревизиями

Ревизия \- уникальный номер строчки в таблице с данными.

Для того чтобы не скачивать уже полученные данные при переконнекте, необходимо задать нужные таблицы в [PlazaMessageAdapter.RevisionTables](xref:StockSharp.Plaza.PlazaMessageAdapter.RevisionTables):

```cs
public PlazaMessageAdapter Adapter;
...
		
Adapter.RevisionTables = new[]
{
	Adapter.TableRegistry.IndexLog,
	Adapter.TableRegistry.TradeFuture,
	Adapter.TableRegistry.TradeOption,
};
		
```

После переконнекта, данные начнут скачиваться с номера последней сохраненной ревизии. Интервал через который сохраняется номер последней ревизии доступен через [PlazaMessageAdapter.RevisionInterval](xref:StockSharp.Plaza.PlazaMessageAdapter.RevisionInterval). По умолчанию файл с номером последней ревизии по выбранной таблице сохраняется в папку *Revisions*, которая создается в папке из которой запускается торговый робот. Изменить путь сохранения можно через [PlazaMessageAdapter.RevisionPath](xref:StockSharp.Plaza.PlazaMessageAdapter.RevisionPath).

Для того чтобы перезакачать все данные, достаточно удалить файлы из папки *Revisions*.
