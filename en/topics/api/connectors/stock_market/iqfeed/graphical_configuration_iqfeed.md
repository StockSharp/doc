# Graphical configuration IQFeed

For all [S\#](../../../../api.md) products, graphical configuration of the connection is performed on the [Connection settings window](../../../graphical_user_interface/connection_settings_window.md):

![API GUI Settings IQFeed](../../../../../images/api_gui_settings_iqfeed.png)

- **Levell server** \- Address for obtaining data on Levell.
- **Level2 server** \- Address for obtaining data on Level2.
- **Lookup server** \- Address for obtaining history data.
- **Admin server** \- Address for obtaining service data.
- **Derivatives** \- Address for obtaining derivative data.
- **Data for Levell** \- All types of data for Levell, which have to be translated.
- **Data type** \- Securities types, for which data must be received.
- **Load securities** \- Should the whole set of securities be loaded from IQFeed website archive.
- **File with securities** \- Path to file with IQFeed list of securities, downloaded from the website. If path is specified, then secondary download from website does not occur, and only the local copy gets parsed.
- **Version** \- Version.
- **Heart beat** \- Server check interval for track the connection alive. By default equal to 1 minute.
- **Reconnection settings** \- Mechanism for tracking connections with the trading system settings. ([Reconnection settings](../../reconnection_settings.md))

## Recommended content

[Connectors](../../../connectors.md)

[Graphical configuration](../../graphical_configuration.md)

[Creating own connector](../../creating_own_connector.md)

[Save and load settings](../../save_and_load_settings.md)
