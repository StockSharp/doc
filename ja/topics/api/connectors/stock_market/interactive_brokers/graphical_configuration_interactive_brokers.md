# Interactive Brokers のグラフィカル設定

すべての [S#](../../../../api.md) 製品では、接続のグラフィカル設定は [接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md) で行います。

![API GUI 設定 Interactive Brokers](../../../../../images/api_gui_settings_interactivebrokers.png)

- **アドレス** - TWS アドレス。
- **Identifier** - 一意の ID。複数のクライアントが 1 つのターミナルまたはゲートウェイに接続している場合に使用されます。
- **Real-time** - リアルタイムデータを使用するか、ブローカーサーバー上の「フリーズ」データを使用するかを指定します。
- **ログレベル** - サーバーメッセージのログレベル。
- **市場データフィールド** - 購読した Level1 メッセージで受信される市場データフィールド。
- **Protocol** - 接続を確立するための SSL プロトコル
- **証明書** - SSL 証明書。
- **パスワード** - SSL 証明書のパスワード。
- **証明書の失効確認** - 証明書の失効を確認します。
- **リモート証明書の検証** - リモート証明書を検証します。
- **ホスト名** - SSL 接続を共有するサーバーの名前。
- **MaxVersion** - MaxVersion
- **ハートビート** - 接続が有効であることを追跡するためのサーバーチェック間隔。既定では 1 分です。
- **再接続設定** - 取引システム設定で接続を追跡するためのメカニズム。([再接続設定](../../reconnection_settings.md))

## 推奨コンテンツ

[コネクター](../../../connectors.md)

[グラフィカル設定](../../graphical_configuration.md)

[独自コネクターの作成](../../creating_own_connector.md)

[設定の保存と読み込み](../../save_and_load_settings.md)
