# FIX のグラフィカル設定

すべての [S#](../../../../api.md) 製品では、接続のグラフィカル設定は [接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md) で行います。

![API GUI Settings FIX](../../../../../images/api_gui_settings_fix.png)

- **アドレス** - アドレス。
- **Dialect** - FIX プロトコルのダイアレクト。
- **Sender** - 送信者識別子。
- **Target** - ターゲット識別子。
- **ログイン** - ログイン。
- **パスワード** - パスワード。
- **Portfolios** - 起動時にすべてのポートフォリオを要求します。
- **Instruments** - 接続時にすべての銘柄を要求します。
- **Encoding** - データ転送に使用されるエンコーディング。
- **Sequence reset** - 識別子カウンターをリセットするかどうか。
- **Date format** - 日付形式。
- **Date and time format** - 日時形式。
- **Time format** - 時刻形式。
- **Receive timeout** - データ受信タイムアウト。
- **Send timeout** - データ送信タイムアウト。
- **Unknown transactions** - サードパーティによって生成された不明な約定を処理します。
- **Protocol** - 接続確立用の SSL プロトコル。
- **証明書** - SSL 証明書。
- **パスワード** - SSL 証明書のパスワード。
- **Revocation check** - 証明書失効チェック。
- **Check remote** - リモート証明書を確認します。
- **Server name** - SSL 接続を使用するサーバー名。
- **再接続設定** - 取引システムとの接続を追跡するメカニズムの設定（[再接続設定](../../reconnection_settings.md)）。
- **ハートビート間隔** - 接続がまだ有効であることをサーバーへ通知する間隔。既定値は 1 分です。
- **Unified board code** - 統一銘柄用のボードコード。

## 関連項目

[コネクター](../../../connectors.md)

[グラフィカル設定](../../graphical_configuration.md)

[独自コネクターの作成](../../creating_own_connector.md)

[設定の保存と読み込み](../../save_and_load_settings.md)
