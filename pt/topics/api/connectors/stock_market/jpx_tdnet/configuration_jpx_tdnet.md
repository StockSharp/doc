# Configuração do conector: JPX TDnet

Configure as propriedades a seguir antes de se conectar ao JPX TDnet. A lista foi verificada com [JpxTdnetMessageAdapter](xref:StockSharp.JpxTdnet.JpxTdnetMessageAdapter).

## Configurações básicas

O editor de conexões mostra primeiro estas configurações.

- `Token` (`SecureString`)

## Configurações avançadas

Estas propriedades controlam pontos de conexão, filtros, limites e outras opções específicas do provedor.

- `Address` (`Uri`)
- `ViewerAddress` (`Uri`)
- `IndexMode` (`JpxTdnetIndexModes`)
- `DefaultLookupDays` (`int`)
- `MaxDays` (`int`)
- `SecurityLookupDays` (`int`)
- `RequestInterval` (`TimeSpan`)
- `MaxDocumentSizeMb` (`int`)

## Veja também

[Configuração gráfica](graphical_configuration_jpx_tdnet.md)

[Inicialização do adaptador](adapter_initialization_jpx_tdnet.md)
