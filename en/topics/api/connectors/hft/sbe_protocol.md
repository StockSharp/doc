# SBE protocol

**SBE (Simple Binary Encoding)** support provides a compact binary transport for low-latency market data and trading messages.

StockSharp includes [SbeRecordSerializer](xref:StockSharp.Server.Sbe.SbeRecordSerializer) for encoding and decoding records, [SbeServer](xref:StockSharp.Server.Sbe.SbeServer) for accepting client connections, and [CryBroSBEMessageAdapter](xref:StockSharp.CryBro.SBE.CryBroSBEMessageAdapter) for client connections.

The implementation supports instrument lookup, Level1, order books, ticks, optional native candles, portfolio and position data, and order operations. Client and server schema identifiers and versions must match.

## See also

[SBE configuration](sbe_protocol/configuration_sbe.md)

[SBE adapter initialization](sbe_protocol/adapter_initialization_sbe.md)

[FIX protocol](../common/fix_protocol.md)

[FAST protocol](../common/fast_protocol.md)
