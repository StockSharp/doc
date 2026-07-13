# Cancelamento em massa de ordens

Para cancelar várias ordens, use o método [Connector.CancelOrders](xref:StockSharp.Algo.Connector.CancelOrders(System.Nullable{System.Boolean},StockSharp.BusinessEntities.Portfolio,System.Nullable{StockSharp.Messages.Sides},StockSharp.BusinessEntities.ExchangeBoard,StockSharp.BusinessEntities.Security,System.Nullable{StockSharp.Messages.SecurityTypes},System.Nullable{System.Int64}))**(**[System.Nullable\<System.Boolean\>](xref:System.Nullable`1) isStopOrder, [StockSharp.BusinessEntities.Portfolio](xref:StockSharp.BusinessEntities.Portfolio) portfolio, [System.Nullable\<StockSharp.Messages.Sides\>](xref:System.Nullable`1) direction, [StockSharp.BusinessEntities.ExchangeBoard](xref:StockSharp.BusinessEntities.ExchangeBoard) board, [StockSharp.BusinessEntities.Security](xref:StockSharp.BusinessEntities.Security) security, [System.Nullable\<StockSharp.Messages.SecurityTypes\>](xref:System.Nullable`1) securityType, [System.Nullable\<System.Int64\>](xref:System.Nullable`1) transactionId **)**, que cancela as ordens ativas de acordo com a máscara de parâmetros passada.

## Exemplos de cancelamento em massa de ordens

Para cancelar todas as ordens comuns ([OrderTypes.Limit](xref:StockSharp.Messages.OrderTypes.Limit)) para a carteira e o instrumento especificados:

```cs
_connector.CancelOrders(false, MainWindow.Instance.Portfolio, null, null, security);
```

Para cancelar todas as ordens para o instrumento especificado:

```cs
_connector.CancelOrders(null, null, null, null, security);
```

Para cancelar todas as ordens stop longas:

```cs
_connector.CancelOrders(true, null, Sides.Buy, null, null);
```
