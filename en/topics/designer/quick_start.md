# Quick start

On first run [Designer](../designer.md) will prompt you to open the [Download market\-data](market_data_storage/download_market_data.md) window. Also, historical data can be downloaded by the [Hydra](../hydra.md) program (codenamed Hydra) designed for automatically downloading market data (instruments, candles, tick trades and order books, etc.) from various sources and storing them in a local storage. About downloading and storing historical data is described in detail in the [Market data storage](market_data_storage.md) section.

When you click the **Download securities** button, the [Download instruments](market_data_storage/download_instruments.md) window appears. To download the instrument, you should enter the instrument code, instrument type, select the data source and click **OK**. [Designer](../designer.md) will query the data source for available instruments. All found instruments will appear in the **All securities** panel. By default, it is specified as a data source in [Designer](../designer.md). You can also use trading terminals as a data source. How to configure the connection to terminals is described in the [Connections settings](connections_settings.md) section.

![Designer Quick start 01](../../images/designer_quick_start_01.png)

To obtain historical data on the instrument, you should select the necessary instrument from the **All securities** list, set the historical data period, select the type and Time Frame of the candles and press the **Start** button. All data will be stored in the [Market data storage](market_data_storage.md).

![Designer Quick start 02](../../images/designer_quick_start_02.png)

After obtaining the historical data, choose one of the demonstration strategies. Double\-click the [Schemas](user_interface/schemas.md) panel in the **Strategy** folder selects the **SMA** strategy example, after that the Sma tab appears in the workspace. After switching to the strategy, in the bar the **Emulation** tab will automatically open, which contains the main elements for strategy creation, debugging, strategy testing ([Strategies](strategies/using_visual_designer.md), [Getting started](backtesting/getting_started.md))

![Designer Quick start 03](../../images/designer_quick_start_03.png)

In the **Emulation** tab, you need to set a test period, and select [Market data storage](market_data_storage.md) in the **Market Data** field.

Clicking the ![Designer Quick start 04](../../images/designer_quick_start_04.png) icon in the **Security** field opens the **Select security** window. You should select the required instrument in this window.

![Designer Quick start 05](../../images/designer_quick_start_05.png)

After selecting any block in the **Designer** panel, the properties of this block appear in the **Properties** panel. In the **Properties** panel of the **Candles** block, you can set the type and Time Frame of [Candles](../api/candles.md).

After clicking the **Start** button, the trading emulation will start.

![Designer Quick start 06](../../images/designer_quick_start_06.png)
