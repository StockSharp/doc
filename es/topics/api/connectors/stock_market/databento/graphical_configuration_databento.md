# Configuración gráfica: Databento

En todos los productos StockSharp, la conexión se configura en la [ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md).

- `Key` - credencial de autenticación.
- `Dataset` - parámetro de conexión. Valor predeterminado: `GLBX.MDP3`.
- `LiveAddress` - dirección del servicio.
- `HistoricalAddress` - dirección del servicio. Valor predeterminado: `https://hist.databento.com/v0/timeseries.get_range`.
- `Symbology` - modo u opción del conector. Valor predeterminado: `DatabentoSymbologyTypes.RawSymbol`.

## Véase también

[Conectores](../../../connectors.md)

[Configuración gráfica](../../graphical_configuration.md)

[Guardar y cargar la configuración](../../save_and_load_settings.md)

[Creación de un conector propio](../../creating_own_connector.md)
