# コネクタ設定: MetaApi

MetaApi トークンを作成し、取引口座をデプロイして、接続パラメーターを指定します。

- `Token` - MetaApi のアクセストークン。
- `AccountId` - デプロイ済み MetaApi 口座の識別子。
- `Region` - 口座のリージョン。API トークンでは自動的に解決されるため、口座スコープのトークンの場合のみ明示的に設定します。
- `Domain` - MetaApi のドメイン。既定値は `agiliumtrade.agiliumtrade.ai`。
- `SynchronizationTimeout` - サーバー側のターミナル同期を待機する時間。既定値は 2 分で、これより短い値は 10 秒に引き上げられます。

MetaApi がターミナル状態を同期済みと報告した時点で接続が確立されるため、タイムアウトは口座履歴の初期同期を賄える長さにする必要があります。

待機注文と保護のパラメーターは [MetaApiOrderCondition](xref:StockSharp.MetaApi.MetaApiOrderCondition) を通じて渡されます。発動価格、ストップロス価格とテイクプロフィット価格、ポジションとともに保存されるマジックナンバー、コメント、クライアント識別子です。

## 関連項目

[MetaApi 公式ドキュメント](https://metaapi.cloud/docs/client/)
