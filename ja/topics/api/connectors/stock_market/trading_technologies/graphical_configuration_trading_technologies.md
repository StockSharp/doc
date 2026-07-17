# グラフィカル設定: 取引テクノロジー

すべての StockSharp 製品では、[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)で接続を設定します。

- `SdkPath` - ローカルファイルまたはディレクトリへのパスです。
- `AppSecretKey` - 認証用の資格情報です。
- `Environment` - コネクターの動作モードまたは選択項目です。 既定値: `TradingTechnologiesEnvironments.ProdSim`.
- `InitializationTimeout` - 時間間隔です。 既定値: `5000`.
- `MarketDepth` - コネクターの数値パラメーターです。 既定値: `20`.
- `IsBinaryProtocol` - コネクターの動作を制御する切り替え項目です。 既定値: `true`.
- `IsOptionsEnabled` - コネクターの動作を制御する切り替え項目です。 既定値: `true`.

## 関連項目

[コネクター](../../../connectors.md)

[グラフィカル設定](../../graphical_configuration.md)

[設定の保存と読み込み](../../save_and_load_settings.md)

[独自コネクターの作成](../../creating_own_connector.md)
