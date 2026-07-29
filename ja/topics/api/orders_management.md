# 注文管理

[S#](../api.md) は、ライフサイクルのすべての段階で取引注文を効率的に管理するための幅広い機能を提供します。このセクションでは、取引アプリケーションで注文を扱う際の主要な側面について説明します。

## 主な機能

- **注文の作成** - さまざまな種類の取引注文（成行、指値、ストップ注文など）の作成
- **状態の追跡** - 注文の現在状態に関する最新情報の受信
- **注文管理** - 既存注文のキャンセル、変更、置換
- **イベント処理** - 注文の登録、約定、キャンセルに関するイベントへの応答
- **一括操作** - 注文グループの効率的な処理

## 注文のライフサイクル

S# の各注文は、特定のライフサイクル段階を経ます。

1. **作成** - 必要なパラメーターを持つ [Order](xref:StockSharp.BusinessEntities.Order) オブジェクトの作成
2. **登録** - 注文を取引システムへ送信
3. **約定** - 注文の部分約定または完全約定、約定の成立
4. **完了** - 注文の完全約定、キャンセル、または拒否

API は各段階における注文の状態について詳細な情報を提供します。これにより、正確な執行制御を備えた複雑な取引アルゴリズムを構築できます。

## 取引戦略との統合

注文管理メカニズムは、取引戦略を開発するためのコンポーネント [Strategy](xref:StockSharp.Algo.Strategies.Strategy) と密接に統合されており、次のことが可能です。

- 注文管理ロジックを戦略内にカプセル化する
- 注文登録および約定のイベントを自動的に追跡して処理する
- 実取引とテストの両方で、注文管理に統一されたアプローチを使用する

## 関連項目

[新しい注文の作成](orders_management/create_new_order.md)

[新しいストップ注文の作成](orders_management/create_new_stop_order.md)

[注文状態](orders_management/orders_states.md)

[注文のキャンセル](orders_management/order_cancel.md)

[注文の一括キャンセル](orders_management/orders_mass_cancel.md)

[注文の置換](orders_management/orders_replacement.md)

[トランザクション番号](orders_management/transaction_number.md)
