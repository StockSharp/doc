# Configuração do conector: B3 UP2DATA

Configure as propriedades a seguir antes de se conectar ao B3 UP2DATA. A lista foi verificada com [B3Up2DataMessageAdapter](xref:StockSharp.B3Up2Data.B3Up2DataMessageAdapter).

## Configurações básicas

O editor de conexões mostra primeiro estas configurações.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `CertificatePath` (`string`)
- `CertificatePassword` (`SecureString`)
- `Address` (`Uri`)

## Configurações avançadas

Estas propriedades controlam pontos de conexão, filtros, limites e outras opções específicas do provedor.

- `SasUri` (`SecureString`)
- `ChannelName` (`string`)
- `FileFormat` (`B3Up2DataFileFormats`)
- `BlobPrefix` (`string`)
- `LookbackDays` (`int`)
- `PageSize` (`int`)
- `MaxPages` (`int`)
- `MaxRawFiles` (`int`)

## Veja também

[Configuração gráfica](graphical_configuration_b3_up2data.md)

[Inicialização do adaptador](adapter_initialization_b3_up2data.md)
