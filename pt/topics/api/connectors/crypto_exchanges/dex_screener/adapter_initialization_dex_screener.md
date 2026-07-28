# Inicialização do adaptador: DEX Screener

O código a seguir inicializa [DexScreenerMessageAdapter](xref:StockSharp.DexScreener.DexScreenerMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new DexScreenerMessageAdapter(connector.TransactionIdGenerator);
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Informe as credenciais e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_dex_screener.md).

## Veja também

[Configuração do conector](configuration_dex_screener.md)

[janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
