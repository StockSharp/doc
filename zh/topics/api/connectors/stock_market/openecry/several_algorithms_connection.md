# 若干算法连接

根据特定的用户/应用程序，OEC 服务器可能不支持多个应用程序的同时连接。在这种情况下，其他连接可能会被中断。为了规避这些限制，此 [OpenECryTrader](xref:StockSharp.OpenECry.OpenECryTrader) 实现通过单一连接到 OEC 服务器支持多个应用程序的同时操作 – [OECRemoting](https://gainfutures.com/gainfuturesapi)。

支持以下 [OpenECryRemoting](xref:StockSharp.OpenECry.OpenECryRemoting) 模式：

- [无](xref:StockSharp.OpenECry.OpenECryRemoting.None) - [OpenECryRemoting](xref:StockSharp.OpenECry.OpenECryRemoting) 已断开连接。该应用程序会创建自己的 OEC 服务器连接。该应用程序不能作为其他应用程序的 [主](xref:StockSharp.OpenECry.OpenECryRemoting.Primary) 服务器。
- [Primary](xref:StockSharp.OpenECry.OpenECryRemoting.Primary) – 应用程序创建它自己的连接到 OEC 服务器。
- [Secondary](xref:StockSharp.OpenECry.OpenECryRemoting.Secondary) - 它会在初始化时搜索以 [Primary](xref:StockSharp.OpenECry.OpenECryRemoting.Primary) 模式运行的本地应用程序。如果找到这样的应用程序，它将使用它们与 OEC 服务器的连接。否则，应用程序将进入 [None](xref:StockSharp.OpenECry.OpenECryRemoting.None) 模式。

要显式设置 [OECRemoting](https://gainfutures.com/gainfuturesapi) 模式，您应在创建 [OpenECryTrader](xref:StockSharp.OpenECry.OpenECryTrader) 对象后立即指定所需模式。例如，要设置 [Secondary](xref:StockSharp.OpenECry.OpenECryRemoting.Secondary) 模式：

```cs
Trader.RemotingRequired = OECRemoting.Secondary;
		
```

默认情况下，[OpenECryTrader](xref:StockSharp.OpenECry.OpenECryTrader) 适配器在 [OpenECryRemoting.None](xref:StockSharp.OpenECry.OpenECryRemoting.None) 模式下运行。
