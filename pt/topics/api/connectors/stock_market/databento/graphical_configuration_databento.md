# Configuração gráfica: Databento

Em todos os produtos StockSharp, a conexão é configurada na [janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md).

- `Key` - credencial de autenticação.
- `Dataset` - parâmetro de conexão. Valor predefinido: `GLBX.MDP3`.
- `LiveAddress` - endereço do serviço.
- `HistoricalAddress` - endereço do serviço. Valor predefinido: `https://hist.databento.com/v0/timeseries.get_range`.
- `Symbology` - modo ou opção do conector. Valor predefinido: `DatabentoSymbologyTypes.RawSymbol`.

## Consulte também

[Conectores](../../../connectors.md)

[Configuração gráfica](../../graphical_configuration.md)

[Guardar e carregar configurações](../../save_and_load_settings.md)

[Criar um conector próprio](../../creating_own_connector.md)
