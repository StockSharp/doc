# Настройки коннектора OpenECry

Механизм взаимодействия показан на данном рисунке:

![OECTrader](../../../../../images/oectrader.png)

Как видно из рисунка, [OpenECryMessageAdapter](xref:StockSharp.OpenECry.OpenECryMessageAdapter) взаимодействует с сервером OEC посредством [GainFutures API](https://gainfutures.com/gainfuturesapi). Для использования [GainFutures API](https://gainfutures.com/gainfuturesapi) не требуется наличие работающего терминала OEC Trader.

При работе с коннектором требуется указать **Логин** и **Пароль** для подключения к торговой площадке. **Логин** и **Пароль** предоставляются брокером. Для получения API доступа рекомендуется обратиться к брокеру.
