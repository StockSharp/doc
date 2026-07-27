# Configuração do conector: Open DART

Configure as propriedades a seguir antes de se conectar ao Open DART. A lista foi verificada com [OpenDartMessageAdapter](xref:StockSharp.OpenDart.OpenDartMessageAdapter).

## Configurações básicas

O editor de conexões mostra primeiro estas configurações.

- `Token` (`SecureString`)

## Configurações avançadas

Estas propriedades controlam pontos de conexão, filtros, limites e outras opções específicas do provedor.

- `Address` (`Uri`)
- `DisclosureAddress` (`Uri`)
- `DisclosureType` (`OpenDartDisclosureTypes`)
- `CorporationClass` (`OpenDartCorporationClasses`)
- `FinalReportsOnly` (`bool`)
- `BusinessYear` (`int?`)
- `ReportType` (`OpenDartReportTypes`)
- `FinancialSearchYears` (`int`)
- `MaxPages` (`int`)

## Veja também

[Configuração gráfica](graphical_configuration_open_dart.md)

[Inicialização do adaptador](adapter_initialization_open_dart.md)
