# Стаканы агрегированных котировок

В платформе [Plaza II](Plaza.md) стаканы котировок определены трех типов, которые отличаются друг от друга глубиной. 

Для фьючерсов: 

- FORTS\_FUTAGGR50\_REPL – стакан глубиной 50 котировок (см. 

  [PlazaStreamRegistry.Aggregation50Future](../api/StockSharp.Plaza.PlazaStreamRegistry.Aggregation50Future.html)

  ) 
- FORTS\_FUTAGGR20\_REPL – стакан глубиной 20 котировок (см. 

  [PlazaStreamRegistry.Aggregation20Future](../api/StockSharp.Plaza.PlazaStreamRegistry.Aggregation20Future.html)

  ) 
- FORTS\_FUTAGGR5\_REPL – стакан глубиной 5 котировок (см. 

  [PlazaStreamRegistry.Aggregation5Future](../api/StockSharp.Plaza.PlazaStreamRegistry.Aggregation5Future.html)

  ) 

Для опиционов: 

- FORTS\_OPTAGGR50\_REPL – стакан глубиной 50 котировок (см. 

  [PlazaStreamRegistry.Aggregation50Option](../api/StockSharp.Plaza.PlazaStreamRegistry.Aggregation50Option.html)

  ) 
- FORTS\_OPTAGGR20\_REPL – стакан глубиной 20 котировок (см. 

  [PlazaStreamRegistry.Aggregation20Option](../api/StockSharp.Plaza.PlazaStreamRegistry.Aggregation20Option.html)

  ) 
- FORTS\_OPTAGGR5\_REPL – стакан глубиной 5 котировок (см. 

  [PlazaStreamRegistry.Aggregation5Option](../api/StockSharp.Plaza.PlazaStreamRegistry.Aggregation5Option.html)

  ) 

[DefaultFutureDepthTable](../api/StockSharp.Plaza.PlazaMessageAdapter.DefaultFutureDepthTable.html) По умолчанию в шлюзе [PlazaMessageAdapter](../api/StockSharp.Plaza.PlazaMessageAdapter.html) используются стаканы глубиной в 5 котировок. Чтобы изменить глубину стакана, необходимо в шлюзе [PlazaMessageAdapter](../api/StockSharp.Plaza.PlazaMessageAdapter.html) свойству [DefaultFutureDepthTable](../api/StockSharp.Plaza.PlazaMessageAdapter.DefaultFutureDepthTable.html) (для фьючерсов) или [DefaultOptionDepthTable](../api/StockSharp.Plaza.PlazaMessageAdapter.DefaultOptionDepthTable.html) (для опционов) присвоить новое значение: 

```cs
adapter.DefaultFutureDepthTable \= adapter.TableRegistry.Aggregation20Future;
```

### Следующие шаги

[Получение произвольных таблиц](PlazaCustomTables.md)

## См. также
