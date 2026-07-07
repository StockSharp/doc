# Configuração OpenECry

O mecanismo de interação é apresentado nesta figura: 

![OECTrader](../../../../../images/oectrader.png)

Como se pode ver na figura, o [OpenECryMessageAdapter](xref:StockSharp.OpenECry.OpenECryMessageAdapter) comunica com o servidor OEC através da [GainFutures API](https://gainfutures.com/gainfuturesapi). A utilização da [GainFutures API](https://gainfutures.com/gainfuturesapi) não requer um terminal OEC Trader em funcionamento.

Para trabalhar com um conector, é necessário especificar o **Login** e a **Password**. O **Login** e a **Password** são fornecidos pelo corretor. Para obter acesso à API, recomenda-se contactar o corretor.
