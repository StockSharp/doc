# Configuration E\*TRADE

要使用连接器，您必须指定**登录名**和**密码**。**登录名**和**密码**由经纪商提供。为了获取API访问权限，建议联系经纪商。

交互机制如图所示：

![ETrade](../../../../../images/etrade.png)

[E*TRADE](../e_trade.md) 使用OAuth 1.0a授权协议，这需要在[E*TRADE](https://etrade.com/)网站上通过浏览器登录并输入密码。完整的授权过程顺序如下面的图所示：

![etrade授权](../../../../../images/etrade_autoriazation.png)

完整的授权程序应每天只执行一次（[E*TRADE](../e_trade.md) 服务器会在美国东部时间午夜重置之前发出的 AccessToken）。如果在当前美东时间当天已经执行过完整授权程序，[ETradeMessageAdapter](xref:StockSharp.ETrade.ETradeMessageAdapter) 会自动下载存储在 [E*TRADE](../e_trade.md) 算法子目录中的 AccessToken。
