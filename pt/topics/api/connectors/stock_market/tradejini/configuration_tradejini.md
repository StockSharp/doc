# Configuração do conector: Tradejini

Configure as propriedades a seguir antes de se conectar ao Tradejini. A lista foi verificada com [TradejiniMessageAdapter](xref:StockSharp.Tradejini.TradejiniMessageAdapter).

## Configurações básicas

O editor de conexões mostra primeiro estas configurações.

- `ApiKey` (`SecureString`)
- `Password` (`SecureString`)
- `TwoFactorCode` (`SecureString`)
- `TwoFactorType` (`TradejiniTwoFactorTypes`)
- `Token` (`SecureString`)

## Configurações avançadas

Estas propriedades controlam pontos de conexão, filtros, limites e outras opções específicas do provedor.

- `PortfolioName` (`string`)
- `DefaultProduct` (`TradejiniProducts`)
- `Address` (`Uri`)
- `PollingInterval` (`TimeSpan`)

## Veja também

[Configuração gráfica](graphical_configuration_tradejini.md)

[Inicialização do adaptador](adapter_initialization_tradejini.md)
