# Стаканы агрегированных котировок

В платформе [Plaza II](../plaza.md) стаканы котировок определены трех типов, которые отличаются друг от друга глубиной. 

- FORTS\_AGGR50\_REPL – стакан глубиной 50 котировок (см. [PlazaStreamRegistry.Aggregation50](xref:StockSharp.Plaza.PlazaStreamRegistry.Aggregation50)) 
- FORTS\_AGGR20\_REPL – стакан глубиной 20 котировок (см. [PlazaStreamRegistry.Aggregation20](xref:StockSharp.Plaza.PlazaStreamRegistry.Aggregation20)) 
- FORTS\_AGGR5\_REPL – стакан глубиной 5 котировок (см. [PlazaStreamRegistry.Aggregation5](xref:StockSharp.Plaza.PlazaStreamRegistry.Aggregation5)) 

По умолчанию в [PlazaMessageAdapter](xref:StockSharp.Plaza.PlazaMessageAdapter) используются стаканы глубиной в 5 котировок. Чтобы изменить глубину стакана, необходимо в свойство [PlazaMessageAdapter.DefaultDepthTable](xref:StockSharp.Plaza.PlazaMessageAdapter.DefaultDepthTable) присвоить соответствующее значение: 

```cs
adapter.DefaultDepthTable = adapter.TableRegistry.Aggregation20;
```

## Следующие шаги

[Получение произвольных таблиц](customtables.md)
