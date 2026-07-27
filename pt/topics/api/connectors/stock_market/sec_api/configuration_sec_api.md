# Configuração do conector: SEC API

Configure as propriedades a seguir antes de se conectar ao SEC API. A lista foi verificada com [SecApiMessageAdapter](xref:StockSharp.SecApi.SecApiMessageAdapter).

## Configurações básicas

O editor de conexões mostra primeiro estas configurações.

- `Token` (`SecureString`)
- `Address` (`Uri`)

## Configurações avançadas

Estas propriedades controlam pontos de conexão, filtros, limites e outras opções específicas do provedor.

- `ActiveOnly` (`bool`)
- `DefaultExchange` (`string`)
- `FormTypes` (`string`)
- `ResultLimit` (`int`)

## Veja também

[Configuração gráfica](graphical_configuration_sec_api.md)

[Inicialização do adaptador](adapter_initialization_sec_api.md)
