# Graphical configuration of cTrader

For all StockSharp products, graphical connection setup is performed in the [Connection settings window](../../../graphical_user_interface/connection_settings_window.md) screen form:

![API GUI Settings cTrader](../../../../../images/api_gui_settings_ctrader.png)

- **Demo** - Connection to demo trading.

OAuth Authorization:

cTrader provides only OAuth method of authorization.

OAuth authorization process:

1. When you click the "Check" button, a window will open:

   ![OAuth Start](../../../../../images/oauth_start.png)

2. After clicking "Start", the user will be redirected to the cTrader website to log in. On the cTrader website, you need to allow the StockSharp application access to trading operations:

   ![cTrader Login](../../../../../images/api_gui_settings_ctrader_2.png)

3. After that, you will be redirected back to the StockSharp website, and the program will automatically log in.

## See also

[Connectors](../../../connectors.md)

[OAuth](../../oauth.md)

[Graphical configuration](../../graphical_configuration.md)

[Creating your own connector](../../creating_own_connector.md)

[Saving and loading settings](../../save_and_load_settings.md)