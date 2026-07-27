# Configuração do conector: Nuvama

Configure as propriedades a seguir antes de se conectar ao Nuvama. A lista foi verificada com [NuvamaMessageAdapter](xref:StockSharp.Nuvama.NuvamaMessageAdapter).

## Configurações básicas

O editor de conexões mostra primeiro estas configurações.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `RequestId` (`SecureString`)
- `AppIdKey` (`SecureString`)
- `PublicIpAddress` (`string`)

## Configurações avançadas

Estas propriedades controlam pontos de conexão, filtros, limites e outras opções específicas do provedor.

- `VendorToken` (`SecureString`)
- `Token` (`SecureString`)
- `AccountId` (`string`)
- `UserId` (`string`)
- `AccountType` (`string`)
- `EmployeeOrDependent` (`string`)
- `PortfolioName` (`string`)
- `DefaultProduct` (`NuvamaProducts`)
- `PollingInterval` (`TimeSpan`)
- `ReconnectAttempts` (`int`)
- `RestAddress` (`Uri`)
- `InstrumentAddress` (`Uri`)
- `IpAddressService` (`Uri`)
- `StreamHost` (`string`)
- `StreamPort` (`int`)

## Veja também

[Configuração gráfica](graphical_configuration_nuvama.md)

[Inicialização do adaptador](adapter_initialization_nuvama.md)
