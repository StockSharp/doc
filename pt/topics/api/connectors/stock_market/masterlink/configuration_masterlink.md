# Configuração do conector: MasterLink

Configure as propriedades a seguir antes de se conectar ao MasterLink. A lista foi verificada com [MasterLinkMessageAdapter](xref:StockSharp.MasterLink.MasterLinkMessageAdapter).

## Configurações básicas

O editor de conexões mostra primeiro estas configurações.

- `Login` (`string`)
- `Password` (`SecureString`)
- `CertificatePath` (`string`)
- `CertificatePassword` (`SecureString`)
- `NodePath` (`string`)
- `GatewayDirectory` (`string`)

## Configurações avançadas

Estas propriedades controlam pontos de conexão, filtros, limites e outras opções específicas do provedor.

- `Account` (`string`)
- `RegisterApiAuth` (`bool`)
- `MarketDataMode` (`MasterLinkMarketDataModes`)
- `AdjustedCandles` (`bool`)
- `AccountPollingInterval` (`TimeSpan`)
- `MaxLookupResults` (`int`)

## Veja também

[Configuração gráfica](graphical_configuration_masterlink.md)

[Inicialização do adaptador](adapter_initialization_masterlink.md)
