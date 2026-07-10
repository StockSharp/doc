# cTrader のグラフィカル設定

すべての StockSharp 製品では、グラフィカルな接続設定は [接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md) 画面フォームで行います。

![API GUI 設定 cTrader](../../../../../images/api_gui_settings_ctrader.png)

- **デモ** - デモ取引への接続。

OAuth 認証:

cTrader は OAuth 方式の認証のみを提供しています。

OAuth 認証プロセス:

1. "Check" ボタンをクリックすると、ウィンドウが開きます。

   ![OAuth Start](../../../../../images/oauth_start.png)

2. "Start" をクリックすると、ユーザーはログインのために cTrader Web サイトへリダイレクトされます。cTrader Web サイトでは、StockSharp アプリケーションに取引操作へのアクセスを許可する必要があります。

   ![cTrader ログイン](../../../../../images/api_gui_settings_ctrader_2.png)

3. その後、StockSharp Web サイトへリダイレクトされ、プログラムは自動的にログインします。

## 関連項目

[コネクタ](../../../connectors.md)

[OAuth](../../oauth.md)

[グラフィカル設定](../../graphical_configuration.md)

[独自コネクタの作成](../../creating_own_connector.md)

[設定の保存と読み込み](../../save_and_load_settings.md)
