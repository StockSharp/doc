# Configuración del conector: Nuvama

Configure las siguientes propiedades antes de conectarse a Nuvama. La lista se ha verificado con [NuvamaMessageAdapter](xref:StockSharp.Nuvama.NuvamaMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `RequestId` (`SecureString`)
- `AppIdKey` (`SecureString`)
- `PublicIpAddress` (`string`)

## Configuración avanzada

Estas propiedades controlan los puntos de conexión, filtros, límites y otras opciones específicas del proveedor.

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

## Véase también

[Configuración gráfica](graphical_configuration_nuvama.md)

[Inicialización del adaptador](adapter_initialization_nuvama.md)
