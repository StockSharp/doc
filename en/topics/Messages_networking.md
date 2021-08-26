# Networking

To organize interaction on network, the components bundle [FixTrader](Fix.md) and [FixServer](FixServer.md) can be used. [FixTrader](Fix.md) ensures translation of fix\-messages into the [S\#](StockSharpAbout.md) messages. [FixServer](FixServer.md) allows performing the opposite operation. It ensures compatibility of these types messages. 

Therefore, if it is required to translate messages somewhere or organize network interaction using commands, this approach can be used.
