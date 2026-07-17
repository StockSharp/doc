# グラフィカル設定: OpenMarkets

すべての StockSharp 製品では、[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)で接続を設定します。

- `ClientId` - 口座またはクライアントの識別子です。
- `ClientSecret` - 認証用の資格情報です。
- `AccountCode` - 口座またはクライアントの識別子です。
- `IsTest` - コネクターの動作を制御する切り替え項目です。
- `DataSource` - 接続パラメーターです。 既定値: `OpenMarketsExtensions.DefaultDataSource`.
- `DefaultExchange` - 接続パラメーターです。 既定値: `OpenMarketsExtensions.DefaultExchange`.
- `DefaultDestination` - 接続パラメーターです。 既定値: `OpenMarketsExtensions.DefaultExchange`.
- `OrderGiver` - 接続パラメーターです。
- `OrderTaker` - 接続パラメーターです。
- `DefaultPriceMultiplier` - コネクターの数値パラメーターです。 既定値: `0.01m`.
- `DepthPollingInterval` - 時間間隔です。 既定値: `TimeSpan.FromSeconds(2)`.

## 関連項目

[コネクター](../../../connectors.md)

[グラフィカル設定](../../graphical_configuration.md)

[設定の保存と読み込み](../../save_and_load_settings.md)

[独自コネクターの作成](../../creating_own_connector.md)
