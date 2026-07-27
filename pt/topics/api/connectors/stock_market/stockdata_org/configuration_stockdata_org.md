# Configuração do conector: StockData.org

Configure as propriedades a seguir antes de se conectar ao StockData.org. A lista foi verificada com [StockDataOrgMessageAdapter](xref:StockSharp.StockDataOrg.StockDataOrgMessageAdapter).

## Configurações básicas

O editor de conexões mostra primeiro estas configurações.

- `Token` (`SecureString`)
- `Address` (`Uri`)

## Configurações avançadas

Estas propriedades controlam pontos de conexão, filtros, limites e outras opções específicas do provedor.

- `ExtendedHours` (`bool`)
- `AdjustedIntraday` (`bool`)
- `NewsLanguage` (`string`)
- `NewsPageSize` (`int`)
- `MaxRequests` (`int`)
- `QuoteTimeZoneId` (`string`)

## Veja também

[Configuração gráfica](graphical_configuration_stockdata_org.md)

[Inicialização do adaptador](adapter_initialization_stockdata_org.md)
