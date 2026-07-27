# Configuração do conector: comdirect

Configure as propriedades a seguir antes de se conectar ao comdirect. A lista foi verificada com [ComdirectMessageAdapter](xref:StockSharp.Comdirect.ComdirectMessageAdapter).

## Configurações básicas

O editor de conexões mostra primeiro estas configurações.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `Login` (`string`)
- `Password` (`SecureString`)
- `TanType` (`ComdirectTanTypes`)
- `PollingInterval` (`TimeSpan`)

## Configurações avançadas

Estas propriedades controlam pontos de conexão, filtros, limites e outras opções específicas do provedor.

- `DefaultCurrency` (`string`)
- `Address` (`Uri`)

## Veja também

[Configuração gráfica](graphical_configuration_comdirect.md)

[Inicialização do adaptador](adapter_initialization_comdirect.md)
