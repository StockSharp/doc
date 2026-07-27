# Configuração do conector: Definedge

Configure as propriedades a seguir antes de se conectar ao Definedge. A lista foi verificada com [DefinedgeMessageAdapter](xref:StockSharp.Definedge.DefinedgeMessageAdapter).

## Configurações básicas

O editor de conexões mostra primeiro estas configurações.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `Token` (`SecureString`)
- `WebSocketToken` (`SecureString`)
- `UserId` (`string`)
- `AccountId` (`string`)

## Configurações avançadas

Estas propriedades controlam pontos de conexão, filtros, limites e outras opções específicas do provedor.

- `OneTimePassword` (`SecureString`)
- `DefaultProduct` (`DefinedgeProducts`)
- `AlgoId` (`string`)
- `Address` (`Uri`)
- `LoginAddress` (`Uri`)
- `HistoryAddress` (`Uri`)
- `InstrumentMasterAddress` (`Uri`)
- `WebSocketAddress` (`Uri`)
- `PollingInterval` (`TimeSpan`)

## Veja também

[Configuração gráfica](graphical_configuration_definedge.md)

[Inicialização do adaptador](adapter_initialization_definedge.md)
