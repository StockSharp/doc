# CTP コネクタの設定

CTP 取引・市場データ API に必要な認証情報、サーバーアドレス、口座識別子を取得し、次のコネクタ設定を指定します。

- **ユーザー名** - プロパティ `Login`。
- **パスワード** - プロパティ `Password`。
- **ブローカー ID** - プロパティ `BrokerId`。
- **投資家 ID** - プロパティ `InvestorId`。
- **市場データアドレス** - プロパティ `MarketDataAddress`。
- **取引サーバーアドレス** - プロパティ `TraderAddress`。
- **アプリケーション ID** - プロパティ `AppId`。
- **認証コード** - プロパティ `AuthCode`。
- **製品情報** - プロパティ `ProductInfo`。既定値は `StockSharp` です。
- **復旧モード** - プロパティ `ResumeType`。既定値は `Quick` です。
- **本番モード** - プロパティ `ProductionMode`。既定値は `true` です。
- **データディレクトリ** - プロパティ `DataPath`。
- **照会間隔** - プロパティ `QueryInterval`。既定値は `1 s` です。
- **接続タイムアウト** - プロパティ `ConnectionTimeout`。既定値は `30 s` です。
