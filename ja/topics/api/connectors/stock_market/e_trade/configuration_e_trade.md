# E\*TRADE の設定

コネクターを使用するには、**ログイン** と **パスワード** を指定する必要があります。**ログイン** と **パスワード** はブローカーから提供されます。API アクセスを取得するには、ブローカーへ問い合わせることを推奨します。

連携メカニズムは次の図に示されています。 

![ETrade](../../../../../images/etrade.png)

[E\*TRADE](../e_trade.md) は OAuth 1.0a 認証プロトコルを使用します。このプロトコルでは、ブラウザー上で [E\*TRADE](https://etrade.com/) サイトにログインとパスワードを入力する必要があります。完全な認証手順のシーケンスは、次の図に示されています。

![etrade authorization](../../../../../images/etrade_autoriazation.png)

完全な認証手順は 1 日に 1 回だけ実行する必要があります（[E\*TRADE](../e_trade.md) サーバーは、以前に発行された AccessTokens を EST の午前 0 時にリセットします）。EST で当日に完全な認証手順がすでに実行されている場合、[ETradeMessageAdapter](xref:StockSharp.ETrade.ETradeMessageAdapter) は [E\*TRADE](../e_trade.md) アルゴリズムのサブディレクトリに保存されている AccessToken を自動的にダウンロードします。

