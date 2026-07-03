# Configuración OpenECry

El mecanismo de interacción se muestra en esta figura: 

![OECTrader](../../../../../images/oectrader.png)

Como se ve en la figura, [OpenECryMessageAdapter](xref:StockSharp.OpenECry.OpenECryMessageAdapter) se comunica con el servidor OEC mediante [GainFutures API](https://gainfutures.com/gainfuturesapi). El uso de [GainFutures API](https://gainfutures.com/gainfuturesapi) no requiere un terminal OEC Trader en funcionamiento.

Para trabajar con un conector, debe especificar **Login** y **Password**. **Login** y **Password** los proporciona el bróker. Para obtener acceso API, se recomienda contactar con el bróker.
