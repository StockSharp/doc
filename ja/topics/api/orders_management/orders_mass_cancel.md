# 注文の一括キャンセル

複数の注文をキャンセルするには、渡されたパラメーター マスクに従ってアクティブな注文をキャンセルする [Connector.CancelOrders](xref:StockSharp.Algo.Connector.CancelOrders(System.Nullable{System.Boolean},StockSharp.BusinessEntities.Portfolio,System.Nullable{StockSharp.Messages.Sides},StockSharp.BusinessEntities.ExchangeBoard,StockSharp.BusinessEntities.Security,System.Nullable{StockSharp.Messages.SecurityTypes},System.Nullable{System.Int64}))**(**[System.Nullable\<System.Boolean\>](xref:System.Nullable`1) isStopOrder, [StockSharp.BusinessEntities.Portfolio](xref:StockSharp.BusinessEntities.Portfolio) portfolio, [System.Nullable\<StockSharp.Messages.Sides\>](xref:System.Nullable`1) direction, [StockSharp.BusinessEntities.ExchangeBoard](xref:StockSharp.BusinessEntities.ExchangeBoard) board, [StockSharp.BusinessEntities.Security](xref:StockSharp.BusinessEntities.Security) security, [System.Nullable\<StockSharp.Messages.SecurityTypes\>](xref:System.Nullable`1) securityType, [System.Nullable\<System.Int64\>](xref:System.Nullable`1) transactionId **)** メソッドを使用します。

## 注文の一括キャンセルの例

指定したポートフォリオと銘柄のすべての通常注文 ([OrderTypes.Limit](xref:StockSharp.Messages.OrderTypes.Limit)) をキャンセルするには:

```cs
_connector.CancelOrders(false, MainWindow.Instance.Portfolio, null, null, security);
```

指定した銘柄のすべての注文をキャンセルするには:

```cs
_connector.CancelOrders(null, null, null, null, security);
```

すべてのロングのストップ注文をキャンセルするには:

```cs
_connector.CancelOrders(true, null, Sides.Buy, null, null);
```

