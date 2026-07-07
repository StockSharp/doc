# Ligação de vários algoritmos

Dependendo do utilizador/aplicação específico, o servidor OEC pode não suportar a ligação simultânea de várias aplicações. Neste caso, outras ligações podem ser interrompidas. Para contornar estas limitações, esta implementação de [OpenECryTrader](xref:StockSharp.OpenECry.OpenECryTrader) suporta a operação simultânea de várias aplicações através de uma única ligação ao servidor OEC – [OECRemoting](https://gainfutures.com/gainfuturesapi).

São suportados os seguintes modos de [OpenECryRemoting](xref:StockSharp.OpenECry.OpenECryRemoting):

- [None](xref:StockSharp.OpenECry.OpenECryRemoting.None) - [OpenECryRemoting](xref:StockSharp.OpenECry.OpenECryRemoting) desligado. A aplicação cria a sua própria ligação ao servidor OEC. A aplicação não pode servir como [Primary](xref:StockSharp.OpenECry.OpenECryRemoting.Primary) para outras aplicações.
- [Primary](xref:StockSharp.OpenECry.OpenECryRemoting.Primary) – a aplicação cria a sua própria ligação ao servidor OEC.
- [Secondary](xref:StockSharp.OpenECry.OpenECryRemoting.Secondary) - procura aplicações locais em execução no modo [Primary](xref:StockSharp.OpenECry.OpenECryRemoting.Primary) no momento da inicialização. Se forem encontradas aplicações desse tipo, utiliza a ligação delas ao servidor OEC. Caso contrário, a aplicação entra no modo [None](xref:StockSharp.OpenECry.OpenECryRemoting.None).

Para definir explicitamente o modo [OECRemoting](https://gainfutures.com/gainfuturesapi), deve especificar o modo pretendido imediatamente após a criação do objeto [OpenECryTrader](xref:StockSharp.OpenECry.OpenECryTrader). Por exemplo, para definir o modo [Secondary](xref:StockSharp.OpenECry.OpenECryRemoting.Secondary):

```cs
Trader.RemotingRequired = OECRemoting.Secondary;
		
```

Por predefinição, o adaptador [OpenECryTrader](xref:StockSharp.OpenECry.OpenECryTrader) opera no modo [OpenECryRemoting.None](xref:StockSharp.OpenECry.OpenECryRemoting.None).
