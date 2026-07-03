# Conexión de varios algoritmos

Según el usuario\/aplicación concreta, el servidor OEC puede no admitir la conexión simultánea de varias aplicaciones. En este caso, otras conexiones pueden interrumpirse. Para evitar estas limitaciones, esta implementación de [OpenECryTrader](xref:StockSharp.OpenECry.OpenECryTrader) admite la operación simultánea de varias aplicaciones mediante una única conexión al servidor OEC – [OECRemoting](https://gainfutures.com/gainfuturesapi).

Se admiten los siguientes modos de [OpenECryRemoting](xref:StockSharp.OpenECry.OpenECryRemoting):

- [None](xref:StockSharp.OpenECry.OpenECryRemoting.None) - [OpenECryRemoting](xref:StockSharp.OpenECry.OpenECryRemoting) desconectado. La aplicación crea su propia conexión al servidor OEC. La aplicación no puede actuar como [Primary](xref:StockSharp.OpenECry.OpenECryRemoting.Primary) para otras aplicaciones.
- [Primary](xref:StockSharp.OpenECry.OpenECryRemoting.Primary) – la aplicación crea su propia conexión al servidor OEC.
- [Secondary](xref:StockSharp.OpenECry.OpenECryRemoting.Secondary) - busca aplicaciones locales que se ejecuten en modo [Primary](xref:StockSharp.OpenECry.OpenECryRemoting.Primary) en el momento de la inicialización. Si se encuentran tales aplicaciones, usa su conexión al servidor OEC. De lo contrario, la aplicación entra en modo [None](xref:StockSharp.OpenECry.OpenECryRemoting.None).

Para establecer explícitamente el modo [OECRemoting](https://gainfutures.com/gainfuturesapi), debe especificar el modo deseado inmediatamente después de crear el objeto [OpenECryTrader](xref:StockSharp.OpenECry.OpenECryTrader). Por ejemplo, para establecer el modo [Secondary](xref:StockSharp.OpenECry.OpenECryRemoting.Secondary):

```cs
Trader.RemotingRequired = OECRemoting.Secondary;
		
```

Por defecto, el adaptador [OpenECryTrader](xref:StockSharp.OpenECry.OpenECryTrader) opera en el modo [OpenECryRemoting.None](xref:StockSharp.OpenECry.OpenECryRemoting.None).
