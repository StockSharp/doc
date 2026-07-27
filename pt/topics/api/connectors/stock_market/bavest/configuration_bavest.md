# Configuração do conector: Bavest

Configure as propriedades a seguir antes de se conectar ao Bavest. A lista foi verificada com [BavestMessageAdapter](xref:StockSharp.Bavest.BavestMessageAdapter).

## Configurações básicas

O editor de conexões mostra primeiro estas configurações.

- `Token` (`SecureString`)
- `Address` (`Uri`)

## Configurações avançadas

Estas propriedades controlam pontos de conexão, filtros, limites e outras opções específicas do provedor.

- `Currency` (`string`)
- `Exchange` (`string`)
- `ExchangeCode` (`string`)
- `FinancialFrequency` (`BavestFinancialFrequencies`)
- `TraceEtfMetrics` (`bool`)
- `ScreenerQuery` (`string`)
- `PageSize` (`int`)
- `MaxPages` (`int`)
- `NewsLimit` (`int`)
- `DatasetLimit` (`int`)

## Veja também

[Configuração gráfica](graphical_configuration_bavest.md)

[Inicialização do adaptador](adapter_initialization_bavest.md)
