# Interactive Brokers

**Interactive Brokers** - 株式、オプション、先物、EFP、先物オプション、外国為替、債券、ファンドなどの金融資産を取引するための取引プラットフォームです。

この取引プラットフォーム向けの取引ロボットを作成する前に、[コネクター](../../connectors.md) のリンクを読んでください。

## TWS Interactive Brokers の設定

1. 他のプログラム ([S#](../../../api.md) 上の取引アルゴリズムなど) からの接続を許可する必要があります。これを行うには、設定メニュー "File -\> Global configuration..." を開きます。新しいウィンドウで "Configuration -\> API -\> Settings" を選択します。

   ![ib 設定](../../../../images/ib_settings.png)
2. "Enable ActiveX and Socket Clients" モードを有効にします。
3. アルゴリズムを実行するコンピューターのアドレス (ローカルアドレス: 127.0.0.1) も追加します。これにより、アルゴリズムを起動するたびにターミナル接続の許可を確認する必要がなくなります。

## 関連項目

[コネクター](../../connectors.md)

[グラフィカル設定](../graphical_configuration.md)

[設定の保存と読み込み](../save_and_load_settings.md)

[独自コネクターの作成](../creating_own_connector.md)

[注文管理](../../orders_management.md)

[新規注文の作成](../../orders_management/create_new_order.md)

[新規ストップ注文の作成](../../orders_management/create_new_stop_order.md)

[Interactive Brokers アダプターの初期化](interactive_brokers/adapter_initialization_interactive_brokers.md)
