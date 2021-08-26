# Several algorithms connection

Depending on the particular user\/application the OEC server may not support simultaneous connection of multiple applications. In this case, other connections can be interrupted. For circumvention of these limitations this [OpenECryTrader](../api/StockSharp.OpenECry.OpenECryTrader.html) implementation supports simultaneous operation of multiple applications through a single connection to the OEC server – [OECRemoting](https://gainfutures.com/gainfuturesapi).

The following modes of [OpenECryRemoting](../api/StockSharp.OpenECry.OpenECryRemoting.html) supported:

- [None](../api/StockSharp.OpenECry.OpenECryRemoting.None.html) \- [OpenECryRemoting](../api/StockSharp.OpenECry.OpenECryRemoting.html) disconnected. The application creates its own connection to the OEC server. The application can not serve as the [Primary](../api/StockSharp.OpenECry.OpenECryRemoting.Primary.html) for other applications.
- [Primary](../api/StockSharp.OpenECry.OpenECryRemoting.Primary.html) – the application creates its own connection to the OEC server.
- [Secondary](../api/StockSharp.OpenECry.OpenECryRemoting.Secondary.html) \- it searches for local applications running in the [Primary](../api/StockSharp.OpenECry.OpenECryRemoting.Primary.html) mode at the time of initialization. If such applications are found it uses their connection to the OEC server. Otherwise, the application enters [None](../api/StockSharp.OpenECry.OpenECryRemoting.None.html) mode.

To explicitly set the [OECRemoting](https://gainfutures.com/gainfuturesapi) mode you should specify the desired mode immediately after the [OpenECryTrader](../api/StockSharp.OpenECry.OpenECryTrader.html) object creation. For example, to set the [Secondary](../api/StockSharp.OpenECry.OpenECryRemoting.Secondary.html) mode:

```cs
Trader.RemotingRequired \= OECRemoting.Secondary;
		
```

By default, the [OpenECryTrader](../api/StockSharp.OpenECry.OpenECryTrader.html) adapter operates in the [OpenECryRemoting.None](../api/StockSharp.OpenECry.OpenECryRemoting.None.html) mode.
