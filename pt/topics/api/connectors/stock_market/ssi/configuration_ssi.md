# Configuração do conector: SSI

Configure as propriedades a seguir antes de se conectar à SSI. A lista foi verificada com [SSIMessageAdapter](xref:StockSharp.SSI.SSIMessageAdapter).

## Configurações básicas

O editor de conexões mostra primeiro estas configurações.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `ClientId` (`string`)
- `Account` (`string`)

## Configurações avançadas

Estas propriedades controlam a autenticação e a sessão, os pontos de conexão, a transmissão e as consultas periódicas.

- `PrivateKey` (`SecureString`)
- `Otp` (`SecureString`)
- `RestEndpoint` (`string`)
- `StreamingEndpoint` (`string`)
- `PollingInterval` (`TimeSpan`)

## Veja também

[Configuração gráfica](graphical_configuration_ssi.md)

[Inicialização do adaptador](adapter_initialization_ssi.md)
