# Configuración gráfica: lemon.markets

En todos los productos StockSharp, la conexión se configura en la [ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md).

- `ApiKey` - credencial de autenticación.
- `IsDemo` - interruptor que controla el comportamiento del conector. Valor predeterminado: `true`.
- `AccountId` - identificador de cuenta o cliente.
- `SecuritiesAccountId` - identificador de cuenta o cliente.
- `DataPrivacyPrincipal` - parámetro de conexión.
- `DataPrivacyJustification` - parámetro de conexión. Valor predeterminado: `app_usage-stocksharp`.
- `PersonId` - identificador de cuenta o cliente.
- `DefaultFeeAmount` - parámetro numérico del conector.
- `IsAppropriatenessConsentAccepted` - interruptor que controla el comportamiento del conector.
- `PollingInterval` - intervalo de tiempo. Valor predeterminado: `TimeSpan.FromSeconds(10)`.

## Véase también

[Conectores](../../../connectors.md)

[Configuración gráfica](../../graphical_configuration.md)

[Guardar y cargar la configuración](../../save_and_load_settings.md)

[Creación de un conector propio](../../creating_own_connector.md)
