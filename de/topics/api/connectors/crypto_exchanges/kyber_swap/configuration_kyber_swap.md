# Connector-Konfiguration: KyberSwap

Konfigurieren Sie vor der Verbindung mit KyberSwap die folgenden Adaptereigenschaften. Die Liste wurde anhand von [KyberSwapMessageAdapter](xref:StockSharp.KyberSwap.KyberSwapMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `ClientId` (`string`)
- `Chain` (`KyberSwapChains`)
- `WalletAddress` (`string`)
- `PrivateKey` (`SecureString`)
- `ApiEndpoint` (`string`)
- `RpcEndpoint` (`string`)

## Erweiterte Einstellungen

Diese Eigenschaften steuern Endpunkte, Filter, Grenzwerte und weitere anbieterspezifische Optionen.

- `Markets` (`string`)
- `ProbeVolume` (`decimal`)
- `SlippageTolerance` (`decimal`)
- `PollingInterval` (`TimeSpan`)
- `ReceiptTimeout` (`TimeSpan`)
- `TransactionLifetime` (`TimeSpan`)
- `IsAutoApprove` (`bool`)

## Siehe auch

[Grafische Konfiguration](graphical_configuration_kyber_swap.md)

[Adapterinitialisierung](adapter_initialization_kyber_swap.md)
