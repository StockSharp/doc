# SBE configuration

Configure the client connection with the parameters issued by the SBE server administrator:

- `Address` - TCP endpoint of the SBE server.
- `SenderCompId` - client identifier used during logon.
- `TargetCompId` - target server identifier.
- `Password` - authentication credential.
- `IsSupportNativeCandles` - enables server-provided candles. When disabled, candles are built from market data on the client.

The server uses [SbeServerSettings](xref:StockSharp.Server.Sbe.SbeServerSettings):

- `IsEnabled` - enables the SBE endpoint.
- `Address` - listening endpoint. The default is `127.0.0.1:5002`.
- `HeartBeat` - session heartbeat interval. The default is 60 seconds.
- `TargetCompId` - server identifier. The default is `StockSharp`.

Use client and server builds with the same SBE schema version.
