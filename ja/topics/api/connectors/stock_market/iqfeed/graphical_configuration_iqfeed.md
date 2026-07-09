# IQFeed のグラフィカル設定

すべての [S#](../../../../api.md) 製品では、接続のグラフィカル設定は [接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md) で行います。

![API GUI Settings IQFeed](../../../../../images/api_gui_settings_iqfeed.png)

- **Level1 server** - Level1 のデータを取得するためのアドレス。
- **Level2 server** - Level2 のデータを取得するためのアドレス。
- **Lookup server** - 履歴データを取得するためのアドレス。
- **Admin server** - サービスデータを取得するためのアドレス。
- **Derivatives** - デリバティブデータを取得するためのアドレス。
- **Data for Level1** - 送信する必要がある Level1 のすべてのデータ型。
- **Data type** - データを受信する必要がある証券タイプ。
- **Load securities** - IQFeed Web サイトのアーカイブから証券の全セットを読み込むかどうか。
- **File with securities** - Web サイトからダウンロードした IQFeed の証券リストを含むファイルへのパス。パスが指定されている場合、Web サイトからの二次ダウンロードは行われず、ローカルコピーのみが解析されます。
- **Version** - バージョン。
- **ハートビート** - 接続が有効であることを追跡するためのサーバーチェック間隔。既定では 1 分です。
- **再接続設定** - 取引システム設定で接続を追跡するためのメカニズム。([再接続設定](../../reconnection_settings.md))

## 推奨コンテンツ

[コネクター](../../../connectors.md)

[グラフィカル設定](../../graphical_configuration.md)

[独自コネクターの作成](../../creating_own_connector.md)

[設定の保存と読み込み](../../save_and_load_settings.md)
