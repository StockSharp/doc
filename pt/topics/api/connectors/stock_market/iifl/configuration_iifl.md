# Configuração do conector: IIFL

Configure as propriedades a seguir antes de se conectar à IIFL. A lista foi verificada com [IIFLMessageAdapter](xref:StockSharp.IIFL.IIFLMessageAdapter).

## Configurações básicas

O editor de conexões mostra primeiro estas configurações.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `ClientId` (`string`)

## Configurações avançadas

Estas propriedades controlam a autenticação e a sessão, os pontos de conexão, a transmissão e as consultas periódicas.

- `AuthorizationCode` (`string`)
- `SessionToken` (`SecureString`)
- `PortfolioName` (`string`)
- `RestEndpoint` (`string`)
- `BridgeHost` (`string`)
- `BridgePort` (`int`)
- `TokenValidationEndpoint` (`string`)
- `StreamingEnabled` (`bool`)
- `PollingInterval` (`TimeSpan`)

## Veja também

[Configuração gráfica](graphical_configuration_iifl.md)

[Inicialização do adaptador](adapter_initialization_iifl.md)
