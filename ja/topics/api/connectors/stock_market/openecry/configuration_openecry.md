# OpenECry の設定

相互作用の仕組みを次の図に示します。

![OpenECry の設定 のスクリーンショット](../../../../../images/oectrader.png)

図から分かるように、[OpenECryMessageAdapter](xref:StockSharp.OpenECry.OpenECryMessageAdapter) は [GainFutures API](https://gainfutures.com/gainfuturesapi) を通じて OEC サーバーと通信します。[GainFutures API](https://gainfutures.com/gainfuturesapi) の使用に、稼働中の OEC Trader ターミナルは必要ありません。

コネクタを使用するには、**ログイン** と **パスワード** を指定する必要があります。**ログイン** と **パスワード** はブローカーから提供されます。API アクセスを取得するには、ブローカーに問い合わせることを推奨します。
