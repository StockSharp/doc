# Several algorithms connection

Depending on the particular user\/application the OEC server may not support simultaneous connection of multiple applications. In this case, other connections can be interrupted. For circumvention of these limitations this [OpenECryTrader](xref:StockSharp.OpenECry.OpenECryTrader) implementation supports simultaneous operation of multiple applications through a single connection to the OEC server – [OECRemoting](https://gainfutures.com/gainfuturesapi).

The following modes of [OpenECryRemoting](xref:StockSharp.OpenECry.OpenECryRemoting) supported:

- [None](xref:StockSharp.OpenECry.OpenECryRemoting.None) \- [OpenECryRemoting](xref:StockSharp.OpenECry.OpenECryRemoting) disconnected. The application creates its own connection to the OEC server. The application can not serve as the [Primary](xref:StockSharp.OpenECry.OpenECryRemoting.Primary) for other applications.
- [Primary](xref:StockSharp.OpenECry.OpenECryRemoting.Primary) – the application creates its own connection to the OEC server.
- [Secondary](xref:StockSharp.OpenECry.OpenECryRemoting.Secondary) \- it searches for local applications running in the [Primary](xref:StockSharp.OpenECry.OpenECryRemoting.Primary) mode at the time of initialization. If such applications are found it uses their connection to the OEC server. Otherwise, the application enters [None](xref:StockSharp.OpenECry.OpenECryRemoting.None) mode.

To explicitly set the [OECRemoting](https://gainfutures.com/gainfuturesapi) mode you should specify the desired mode immediately after the [OpenECryTrader](xref:StockSharp.OpenECry.OpenECryTrader) object creation. For example, to set the [Secondary](xref:StockSharp.OpenECry.OpenECryRemoting.Secondary) mode:

```cs
Trader.RemotingRequired = OECRemoting.Secondary;
		
```

By default, the [OpenECryTrader](xref:StockSharp.OpenECry.OpenECryTrader) adapter operates in the [OpenECryRemoting.None](xref:StockSharp.OpenECry.OpenECryRemoting.None) mode.
