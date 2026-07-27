# Configuração do conector: TASE Data Hub

Configure as propriedades a seguir antes de se conectar ao TASE Data Hub. A lista foi verificada com [TaseDataHubMessageAdapter](xref:StockSharp.TaseDataHub.TaseDataHubMessageAdapter).

## Configurações básicas

O editor de conexões mostra primeiro estas configurações.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)

## Configurações avançadas

Estas propriedades controlam pontos de conexão, filtros, limites e outras opções específicas do provedor.

- `Address` (`Uri`)
- `Scope` (`string`)
- `SecurityLookupDays` (`int`)
- `ReferenceCacheTimeout` (`TimeSpan`)

## Veja também

[Configuração gráfica](graphical_configuration_tase_data_hub.md)

[Inicialização do adaptador](adapter_initialization_tase_data_hub.md)
