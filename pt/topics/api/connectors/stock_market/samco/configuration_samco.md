# Configuração do conector: Samco

Configure as propriedades a seguir antes de se conectar à Samco. A lista foi verificada com [SamcoMessageAdapter](xref:StockSharp.Samco.SamcoMessageAdapter).

## Configurações básicas

O editor de conexões mostra primeiro estas configurações.

- `Key` (`SecureString`)

## Configurações avançadas

Estas propriedades controlam a autenticação e a sessão, os pontos de conexão, a transmissão e as consultas periódicas.

- `Secret` (`SecureString`)
- `SessionToken` (`SecureString`)
- `RestEndpoint` (`string`)
- `InstrumentEndpoint` (`string`)
- `StreamingEndpoint` (`string`)
- `StreamingEnabled` (`bool`)
- `PollingInterval` (`TimeSpan`)

## Veja também

[Configuração gráfica](graphical_configuration_samco.md)

[Inicialização do adaptador](adapter_initialization_samco.md)
