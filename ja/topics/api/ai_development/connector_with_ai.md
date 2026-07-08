# AI でコネクタを書く

AI ツールを使用して StockSharp 向けの取引所コネクタを作成するためのステップバイステップガイドです。

## 準備

### 1. 取引所 API を調査する

開始する前に、以下を準備します:
- 取引所の REST/WebSocket API ドキュメント
- テスト用 API キー (sandbox/testnet)
- サポートされるデータ型の一覧 (キャンドル、板情報、ティック、約定)
- サポートされる注文タイプの一覧 (limit、market、stop)

### 2. プロジェクトを作成する

```bash
dotnet new classlib -n StockSharp.MyExchange --framework net10.0
cd StockSharp.MyExchange
dotnet add package StockSharp.Messages
dotnet add package StockSharp.Algo
```

### 3. AI 向けのコンテキストを準備する

`CLAUDE.md` ファイルを作成します:

```markdown
# プロジェクトルール — 取引所コネクタ

- フレームワーク: StockSharp 5.x, .NET 10
- コネクタは MessageAdapter として実装する
- async/await のために AsyncMessageAdapter を継承する
- すべての HTTP リクエストは CancellationToken 付きの HttpClient 経由
- WebSocket サブスクリプションはネイティブクライアントまたは ClientWebSocket 経由
- 型マッピング: 取引所の型 → StockSharp Messages
- エラー処理: 接続エラーには SendOutError() を使用
- すべての文字列はローカライズリソースに置く (少なくとも const にする)
```

## コネクタのアーキテクチャ

StockSharp のコネクタは、次の処理を行う `MessageAdapter` です:
1. コアから受信メッセージ (リクエスト) を受け取る
2. それらを処理する (取引所 API を呼び出す)
3. 応答メッセージ (結果) を返送する

```
StockSharp Core → [Message] → MessageAdapter → [HTTP/WS] → Exchange
Exchange → [HTTP/WS] → MessageAdapter → [Message] → StockSharp Core
```

## ステップバイステップの例

### ステップ 1: 基本的なアダプター構造

プロンプト:

```
StockSharp を使用して、MyExchange という暗号資産取引所コネクタ向けの
基本的な MessageAdapter を作成してください:
- AsyncMessageAdapter を継承する
- 接続/切断 (ConnectMessage, DisconnectMessage) を実装する
- 設定を追加する: ApiKey, Secret, demo mode
- REST API には HttpClient を使用する
- ベース API URL: https://api.myexchange.com/v1
```

### ステップ 2: 銘柄検索

プロンプト:

```
アダプターに SecurityLookupMessage の処理を追加してください:
- GET /api/v1/symbols リクエストは銘柄の JSON リストを返す
- 各銘柄には次が含まれる: symbol, baseAsset, quoteAsset,
  minQty, maxQty, tickSize, status
- マッピング: symbol → SecurityId, baseAsset/quoteAsset → name,
  tickSize → SecurityMessage.PriceStep
- 各銘柄について SecurityMessage を送信する
- 最後に SubscriptionFinishedMessage を送信する
```

### ステップ 3: マーケットデータ

プロンプト:

```
アダプターにマーケットデータサブスクリプションを追加してください:

1. キャンドル (MarketDataTypes.CandleTimeFrame):
   - REST: GET /api/v1/klines?symbol={}&interval={}&limit=1000
   - WebSocket: kline_{symbol}_{interval} チャンネルをサブスクライブする
   - 間隔マッピング: 1m, 5m, 15m, 1h, 4h, 1d

2. 板情報 (MarketDataTypes.MarketDepth):
   - WebSocket: depth_{symbol} チャンネルをサブスクライブする
   - bids/asks を QuoteChangeMessage に解析する

3. ティック (MarketDataTypes.Trades):
   - WebSocket: trades_{symbol} チャンネルをサブスクライブする
   - ExecutionTypes.Tick を持つ ExecutionMessage に解析する
```

### ステップ 4: 取引操作

プロンプト:

```
アダプターに取引操作のサポートを追加してください:

1. 注文登録 (OrderRegisterMessage):
   - パラメーター symbol, side, type, quantity, price を指定して POST /api/v1/order
   - ExecutionTypes.Transaction を持つ ExecutionMessage を返す

2. 注文キャンセル (OrderCancelMessage):
   - DELETE /api/v1/order/{orderId}
   - ステータス OrderStates.Done の ExecutionMessage を返す

3. ポートフォリオ取得 (PortfolioLookupMessage):
   - GET /api/v1/account
   - 残高を PositionChangeMessage に解析する

4. 注文更新用 WebSocket:
   - チャンネル orders_{listenKey}
   - 注文ステータス更新を解析する
```

### ステップ 5: コードレビュー

各ステップを生成した後、AI に次のように依頼します:

```
生成されたアダプターが StockSharp API に準拠しているかレビューしてください:
1. すべてのメッセージ型が処理されていますか?
2. CancellationToken は正しく使用されていますか?
3. HTTP エラー処理は実装されていますか?
4. 完了後に SubscriptionFinishedMessage は送信されていますか?
5. 切断時に WebSocket の再接続は動作しますか?
```

## 主要な実装詳細

### MessageAdapter — 処理すべき内容

| 受信メッセージ | アクション | 応答メッセージ |
|-----------------|--------|-----------------|
| `ConnectMessage` | API に接続 | `ConnectMessage` (応答) |
| `DisconnectMessage` | 切断 | `DisconnectMessage` (応答) |
| `SecurityLookupMessage` | 銘柄をリクエスト | `SecurityMessage` × N |
| `MarketDataMessage` (subscribe) | データをサブスクライブ | `SubscriptionResponseMessage` |
| `OrderRegisterMessage` | 注文を作成 | `ExecutionMessage` |
| `OrderCancelMessage` | 注文をキャンセル | `ExecutionMessage` |
| `PortfolioLookupMessage` | ポートフォリオをリクエスト | `PortfolioMessage`, `PositionChangeMessage` |

### 非同期パターン

```csharp
public class MyExchangeAdapter : AsyncMessageAdapter
{
    private HttpClient _httpClient;

    protected override ValueTask OnConnectAsync(ConnectMessage msg, CancellationToken token)
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("https://api.myexchange.com/v1/");
        _httpClient.DefaultRequestHeaders.Add("X-API-KEY", Key.To<string>());

        SendOutMessage(new ConnectMessage());
        return default;
    }

    protected override ValueTask OnSecurityLookupAsync(SecurityLookupMessage msg, CancellationToken token)
    {
        // ... request instruments
    }

    // ... other methods
}
```

### リクエスト署名

ほとんどの取引所では、プライベートリクエストに HMAC 署名が必要です:

```csharp
private string SignRequest(string payload)
{
    using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(Secret.To<string>()));
    var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
    return Convert.ToHexString(hash).ToLowerInvariant();
}
```

## コネクタレビュー用チェックリスト

### 接続
- [ ] 接続/切断が正しく動作する
- [ ] 認証エラーが処理される
- [ ] 切断時に再接続が動作する

### 銘柄
- [ ] 銘柄一覧が正常に読み込まれる
- [ ] 正しくマッピングされている: SecurityId、PriceStep、VolumeStep
- [ ] `SubscriptionFinishedMessage` が送信される

### マーケットデータ
- [ ] キャンドル: 履歴読み込み + 新規キャンドルのサブスクリプション
- [ ] 板情報: 正しい深度、更新
- [ ] ティック: 正しい時刻、出来高、方向

### 取引
- [ ] 指値注文: 作成、キャンセル
- [ ] 成行注文: 作成
- [ ] 注文ステータス更新
- [ ] ポートフォリオ残高更新

### 全般
- [ ] すべての `CancellationToken` が伝播される
- [ ] エラーが `SendOutError()` 経由でログに記録される
- [ ] リソースリークがない (WebSocket、HttpClient)
- [ ] エラーや警告なしでコンパイルされる

## プロンプト例

### 新しいデータ型を追加する

```
自分のコネクタに Level 1 データサポート (BestBid/BestAsk) を追加してください:
- WebSocket チャンネル: ticker_{symbol}
- bid, ask, last, volume を解析する
- 次のフィールドを持つ Level1ChangeMessage を送信する:
  Level1Fields.BestBidPrice, Level1Fields.BestAskPrice,
  Level1Fields.LastTradePrice, Level1Fields.Volume
```

### レート制限を処理する

```
自分のコネクタにレート制限処理を追加してください:
- API はヘッダー X-RateLimit-Remaining と X-RateLimit-Reset を返す
- 制限に達した場合: Reset まで待機し、警告をログに記録する
- 同時リクエスト数を制限するために SemaphoreSlim を使用する
```

## ヒント

1. **読み取り専用から開始する** — まず接続、銘柄、マーケットデータを実装します。検証後に取引操作を追加します
2. **sandbox を使用する** — 取引所のテスト環境でテストします
3. **既存コネクタを参照する** — 既存の StockSharp コネクタのコードを参照として AI に渡します
4. **すべてをログに記録する** — 詳細なログは、コネクタのデバッグ時に非常に役立ちます
5. **エッジケースを処理する** — 再接続、銘柄変更、非標準の注文タイプ
