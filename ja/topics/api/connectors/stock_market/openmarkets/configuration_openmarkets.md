# コネクターの設定: OpenMarkets

プロバイダーから認証情報を取得し、接続パラメーターを指定します。

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
