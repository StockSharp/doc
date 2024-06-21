# IQFeed

**DTN IQFeed** \- Real\-time market data provider for stock quotes, Forex, news, futures contracts, etc..

Before you start writing trading robots for the current trading platform, it is recommended to read the links in the [Connectors](../../connectors.md). 

## Configuration IQFeed

The interaction mechanism is shown in this figure: 

![IQFeed](../../../../images/iqfeed.jpg)

To work with the **IQFeed** connector, you need to install the **IQ Feed Client** router on the computer, which can be installed on both the local computer and the remote one. Data exchange between the client application and the **IQ Feed Client**, as well as between the **IQ Feed Client** and the servers, is performed via the TCP\/IP protocol. 

To download the **IQ Feed Client** from the [IQFeed](https://www.iqfeed.net/stocksharp/) site, you must first authorize using the password and login received from **iQFeed**.

After installing **IQ Feed Client**, it is recommended to restart the computer.

After installing **IQ Feed Client, IQLink Launcher** must be started.

![iQFeedIQLinkLauncher](../../../../images/iqfeediqlinklauncher.png)

In the **IQLink Launcher** window that opens, click **Start IQLink**.

![iQFeedIQConnectLogin](../../../../images/iqfeediqconnectlogin.png)

In the opened **IQ Connect Login** window it is necessary to enter the **Login** and **Password** (or PIN) received from the **iQFeed** service, in this case the Login and Password are not the Login and Password from the **iQFeed** site. After filling in Login and Password, you have to click **Connect** to connect.

To receive data, the client application uses four connections through different ports: 

1. Level1 (port 5009) is used to get real\-time data on instruments (ticks, opening and closing prices, volatility, etc.) and news.
2. Level2 (port 9200) is used to get extended quotes for instruments, for each ECN you can get the best pair of quotes.
3. Lookup (port 9100) is used to search for instruments, retrieve historical data, get advanced information on news.
4. Admin (port 9300) is used to get general information about connection and changing settings.

The port numbers that are used by default for connection to the **IQ Feed Client** are specified in brackets. For client connections, the port numbers can be changed in the registry, for example, for Level1 on the following path: \[HKEY\_CURRENT\_USER\\SOFTWARE\\DTN\\IQFEED\\Startup\\Level1Port\]. Port numbers for connecting to IQ servers can not be changed. 

> [!CAUTION]
> Connector support only market\-data feed, transaction are not supported. 

## Recommended content

[Connectors](../../connectors.md)

[Graphical configuration](../graphical_configuration.md)

[Save and load settings](../save_and_load_settings.md)

[Creating own connector](../creating_own_connector.md)

[Orders management](../../orders_management.md)

[Create new order](../../orders_management/create_new_order.md)

[Create new stop order](../../orders_management/create_new_stop_order.md)

[Connection IQFeed](iqfeed/connection_iqfeed.md)

[Adapter initialization IQFeed](iqfeed/adapter_initialization_iqfeed.md)
