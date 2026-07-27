# Inicialización del adaptador: Choice FinX

El siguiente código inicializa [ChoiceFinXMessageAdapter](xref:StockSharp.ChoiceFinX.ChoiceFinXMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new ChoiceFinXMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Indique las credenciales y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_choice_finx.md).

## Véase también

[Configuración del conector](configuration_choice_finx.md)

[ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
