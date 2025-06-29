# Configuration E\*TRADE

To work with a connector, you have to specify the **Login** and **Password**. **Login** and **Password** are provided by the broker. To get the API access, it is recommended to contact the broker.

The interaction mechanism is shown in this figure: 

![ETrade](../../../../../images/etrade.png)

[E\*TRADE](../e_trade.md) uses the OAuth 1.0a authorization protocol, which requires a login and password over the browser on the [E\*TRADE](https://etrade.com/) site. The full authorization procedure sequence is shown in the following figure:

![etrade authorization](../../../../../images/etrade_autoriazation.png)

A full authorization procedure should be performed only once a day (the [E\*TRADE](../e_trade.md) server resets AccessTokens issued earlier at midnight by EST). If the full authorization procedure is already carried out on current day by EST, [ETradeMessageAdapter](xref:StockSharp.ETrade.ETradeMessageAdapter) automatically downloads AccessToken, stored in a subdirectory of [E\*TRADE](../e_trade.md) algorithm.
