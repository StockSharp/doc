# Configuração do conector: STON.fi

Configure as propriedades a seguir antes de se conectar à STON.fi. A lista foi verificada com [StonFiMessageAdapter](xref:StockSharp.StonFi.StonFiMessageAdapter).

## Configurações básicas

O editor de conexões mostra primeiro estas configurações.

- `ApiEndpoint` (`string`)
- `TonCenterEndpoint` (`string`)
- `TonCenterApiKey` (`SecureString`)
- `WalletAddress` (`string`)
- `Mnemonic` (`SecureString`)

## Configurações avançadas

Estas propriedades controlam os dados da carteira, a seleção de reservas, os limites, as consultas periódicas, o histórico e o comportamento das transações.

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

## Veja também

[Configuração gráfica](graphical_configuration_stonfi.md)

[Inicialização do adaptador](adapter_initialization_stonfi.md)
