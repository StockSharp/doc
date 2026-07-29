# Configuração do conector: m.Stock

Configure as propriedades a seguir antes de se conectar à m.Stock. A lista foi verificada com [MStockMessageAdapter](xref:StockSharp.MStock.MStockMessageAdapter).

## Configurações básicas

O editor de conexões mostra primeiro estas configurações.

- `Key` (`SecureString`)
- `ClientCode` (`string`)

## Configurações avançadas

Estas propriedades controlam a autenticação e a sessão, os pontos de conexão, a transmissão e as consultas periódicas.

- `Password` (`SecureString`)
- `Otp` (`SecureString`)
- `UseTotp` (`bool`)
- `RefreshToken` (`SecureString`)
- `AccessToken` (`SecureString`)
- `RestEndpoint` (`string`)
- `StreamingEndpoint` (`string`)
- `StreamingEnabled` (`bool`)
- `PollingInterval` (`TimeSpan`)

## Veja também

[Configuração gráfica](graphical_configuration_mstock.md)

[Inicialização do adaptador](adapter_initialization_mstock.md)
