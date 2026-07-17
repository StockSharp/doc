# コネクターの設定: lemon.markets

プロバイダーから認証情報を取得し、接続パラメーターを指定します。

- `ApiKey` - 認証用の資格情報です。
- `IsDemo` - コネクターの動作を制御する切り替え項目です。 既定値: `true`.
- `AccountId` - 口座またはクライアントの識別子です。
- `SecuritiesAccountId` - 口座またはクライアントの識別子です。
- `DataPrivacyPrincipal` - 接続パラメーターです。
- `DataPrivacyJustification` - 接続パラメーターです。 既定値: `app_usage-stocksharp`.
- `PersonId` - 口座またはクライアントの識別子です。
- `DefaultFeeAmount` - コネクターの数値パラメーターです。
- `IsAppropriatenessConsentAccepted` - コネクターの動作を制御する切り替え項目です。
- `PollingInterval` - 時間間隔です。 既定値: `TimeSpan.FromSeconds(10)`.
