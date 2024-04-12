# Networking

To organize interaction on network, the components bundle [FixTrader](../connectors/common/fix_protocol.md) and [FixServer](../connectors/common/fix_server.md) can be used. [FixTrader](../connectors/common/fix_protocol.md) ensures translation of fix\-messages into the [S\#](../../api.md) messages. [FixServer](../connectors/common/fix_server.md) allows performing the opposite operation. It ensures compatibility of these types messages. 

Therefore, if it is required to translate messages somewhere or organize network interaction using commands, this approach can be used.
