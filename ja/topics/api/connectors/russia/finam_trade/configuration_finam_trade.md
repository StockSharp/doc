# コネクタ設定: Finam Trade API

Finam に接続する前に、次のプロパティを設定します。この一覧は [FinamTradeMessageAdapter](xref:StockSharp.FinamTrade.FinamTradeMessageAdapter) の実装で確認されています。

## 基本設定

接続エディターでは、これらの設定が最初に表示されます。

- `Token` (`SecureString`) — 必須の Finam Trade API シークレットです。アダプターが短時間有効なセッショントークンへ交換します。
- `AccountId` (`string`) — 任意の取引口座識別子です。空の場合、アダプターはトークンで利用できる最初の口座を使用します。

## 詳細設定

- `AppId` (`string`) — セッション作成時に送信するアプリケーション識別子です。既定値は `StockSharp` です。
- `PollingInterval` (`TimeSpan`) — 口座と注文のスナップショットを取得する間隔です。既定値は 30 秒で、1 秒未満の値は使用できません。
- `LookupLimit` (`int`) — 制限なしの検索で返す銘柄の最大数です。既定値は `10000` で、正の値が必要です。
- `RestAddress` (`string`) — REST API のベースアドレスです。既定値は `https://api.finam.ru/` です。
- `WebSocketAddress` (`string`) — WebSocket API のアドレスです。既定値は `wss://api.finam.ru/ws` です。

Finam または互換ゲートウェイから別の接続先を指定されていない限り、入力済みのアドレスは変更しないでください。

## 関連項目

[グラフィカル設定](graphical_configuration_finam_trade.md)

[アダプターの初期化](adapter_initialization_finam_trade.md)
