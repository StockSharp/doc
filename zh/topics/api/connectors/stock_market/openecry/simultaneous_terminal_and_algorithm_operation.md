# 终端和算法的同时操作

如果需要，您可以将 OEC Trader 终端配置为以 [Primary](xref:StockSharp.OpenECry.OpenECryRemoting.Primary) 模式运行，然后运行算法 [S#](../../../../api.md)，并让算法也处于 [Primary](xref:StockSharp.OpenECry.OpenECryRemoting.Primary) 模式。在这种情况下，终端和算法将使用相同的 OEC 服务器连接。要进行配置，您应该在 OEC Trader 终端中勾选菜单项 **文件 → 允许远程控制**：

![终端和算法的同时操作 截图](../../../../../images/oectradersettings.png)
