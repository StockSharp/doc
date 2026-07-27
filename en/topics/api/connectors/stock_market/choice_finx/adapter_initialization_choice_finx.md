# Adapter initialization: Choice FinX

The following code initializes [ChoiceFinXMessageAdapter](xref:StockSharp.ChoiceFinX.ChoiceFinXMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new ChoiceFinXMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Set the credentials and any other required properties described on the [Connector configuration](configuration_choice_finx.md) page.

## See also

[Connector configuration](configuration_choice_finx.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
