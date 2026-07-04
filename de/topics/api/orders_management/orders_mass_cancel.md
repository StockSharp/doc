# Massenstornierung von Orders

Um mehrere Orders zu stornieren, verwenden Sie die Methode [Connector.CancelOrders](xref:StockSharp.Algo.Connector.CancelOrders(System.Nullable{System.Boolean},StockSharp.BusinessEntities.Portfolio,System.Nullable{StockSharp.Messages.Sides},StockSharp.BusinessEntities.ExchangeBoard,StockSharp.BusinessEntities.Security,System.Nullable{StockSharp.Messages.SecurityTypes},System.Nullable{System.Int64}))**(**[System.Nullable\<System.Boolean\>](xref:System.Nullable`1) isStopOrder, [StockSharp.BusinessEntities.Portfolio](xref:StockSharp.BusinessEntities.Portfolio) portfolio, [System.Nullable\<StockSharp.Messages.Sides\>](xref:System.Nullable`1) direction, [StockSharp.BusinessEntities.ExchangeBoard](xref:StockSharp.BusinessEntities.ExchangeBoard) board, [StockSharp.BusinessEntities.Security](xref:StockSharp.BusinessEntities.Security) security, [System.Nullable\<StockSharp.Messages.SecurityTypes\>](xref:System.Nullable`1) securityType, [System.Nullable\<System.Int64\>](xref:System.Nullable`1) transactionId **)**. Sie storniert aktive Orders gemäß der übergebenen Parametermaske.

## Beispiele zur Massenstornierung von Orders

Um alle gewöhnlichen Orders ([OrderTypes.Limit](xref:StockSharp.Messages.OrderTypes.Limit)) für das angegebene Portfolio und Instrument zu stornieren:

```cs
_connector.CancelOrders(false, MainWindow.Instance.Portfolio, null, null, security);
```

Um alle Orders für das angegebene Instrument zu stornieren:

```cs
_connector.CancelOrders(null, null, null, null, security);
```

Um alle Long-Stop-Orders zu stornieren:

```cs
_connector.CancelOrders(true, null, Sides.Buy, null, null);
```

