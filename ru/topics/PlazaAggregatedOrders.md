# Стаканы агрегированных котировок

В платформе [Plaza II](Plaza.md) стаканы котировок определены трех типов, которые отличаются друг от друга глубиной. 

Для фьючерсов: 

- FORTS\_FUTAGGR50\_REPL – стакан глубиной 50 котировок (см. [PlazaStreamRegistry.Aggregation50Future](xref:StockSharp.Plaza.PlazaStreamRegistry.Aggregation50Future)) 
- FORTS\_FUTAGGR20\_REPL – стакан глубиной 20 котировок (см. [PlazaStreamRegistry.Aggregation20Future](xref:StockSharp.Plaza.PlazaStreamRegistry.Aggregation20Future)) 
- FORTS\_FUTAGGR5\_REPL – стакан глубиной 5 котировок (см. [PlazaStreamRegistry.Aggregation5Future](xref:StockSharp.Plaza.PlazaStreamRegistry.Aggregation5Future)) 

Для опиционов: 

- FORTS\_OPTAGGR50\_REPL – стакан глубиной 50 котировок (см. [PlazaStreamRegistry.Aggregation50Option](xref:StockSharp.Plaza.PlazaStreamRegistry.Aggregation50Option)) 
- FORTS\_OPTAGGR20\_REPL – стакан глубиной 20 котировок (см. [PlazaStreamRegistry.Aggregation20Option](xref:StockSharp.Plaza.PlazaStreamRegistry.Aggregation20Option)) 
- FORTS\_OPTAGGR5\_REPL – стакан глубиной 5 котировок (см. [PlazaStreamRegistry.Aggregation5Option](xref:StockSharp.Plaza.PlazaStreamRegistry.Aggregation5Option)) 

[DefaultFutureDepthTable](xref:StockSharp.Plaza.PlazaMessageAdapter.DefaultFutureDepthTable) По умолчанию в шлюзе [PlazaMessageAdapter](xref:StockSharp.Plaza.PlazaMessageAdapter) используются стаканы глубиной в 5 котировок. Чтобы изменить глубину стакана, необходимо в шлюзе [PlazaMessageAdapter](xref:StockSharp.Plaza.PlazaMessageAdapter) свойству [DefaultFutureDepthTable](xref:StockSharp.Plaza.PlazaMessageAdapter.DefaultFutureDepthTable) (для фьючерсов) или [DefaultOptionDepthTable](xref:StockSharp.Plaza.PlazaMessageAdapter.DefaultOptionDepthTable) (для опционов) присвоить новое значение: 

```cs
adapter.DefaultFutureDepthTable = adapter.TableRegistry.Aggregation20Future;
```

## Следующие шаги

[Получение произвольных таблиц](PlazaCustomTables.md)
