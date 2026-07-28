# Configuración del conector: STON.fi

Configure las siguientes propiedades antes de conectarse a STON.fi. La lista se ha verificado con [StonFiMessageAdapter](xref:StockSharp.StonFi.StonFiMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `ApiEndpoint` (`string`)
- `TonCenterEndpoint` (`string`)
- `TonCenterApiKey` (`SecureString`)
- `WalletAddress` (`string`)
- `Mnemonic` (`SecureString`)

## Configuración avanzada

Estas propiedades controlan los datos del monedero, la selección de fondos, los límites, las consultas periódicas, el historial y el comportamiento de las transacciones.

- `WalletSubwalletId` (`uint`)
- `WalletRevision` (`int`)
- `Pools` (`string`)
- `PoolLimit` (`int`)
- `ProbeVolume` (`decimal`)
- `SlippageTolerance` (`decimal`)
- `PollingInterval` (`TimeSpan`)
- `HistoryBlockLimit` (`int`)
- `PrivatePollingInterval` (`TimeSpan`)
- `TransactionTimeout` (`TimeSpan`)

## Véase también

[Configuración gráfica](graphical_configuration_stonfi.md)

[Inicialización del adaptador](adapter_initialization_stonfi.md)
