# アダプタールーティング

StockSharp は、複数の取引所およびブローカーへの同時接続をサポートします。ルーティングシステム (バスケットルーティング) は、どのメッセージをどのアダプターへ送るかを管理し、複数接続での透過的な動作を保証します。

## 全体アーキテクチャ

複数のアダプターを使用する場合、コネクタはすべての接続を組み合わせたバスケットを自動的に作成します。ルーターは、マーケットデータサブスクリプション、トランザクション、ポートフォリオリクエストなど、それぞれの具体的なメッセージをどのアダプターへ送るべきかを判断します。

## AdapterRouter

[IAdapterRouter](xref:StockSharp.Algo.IAdapterRouter) インターフェイスは、アダプター間でメッセージをルーティングするためのロジックを定義します。

### 主なメソッド

| メソッド | 説明 |
|--------|-------------|
| `GetAdapters` | 指定されたメッセージの処理に適したアダプターの一覧を返します |
| `GetSubscriptionAdaptersAsync` | マーケットデータサブスクリプション用のアダプターを非同期に判断します |
| `GetPortfolioAdapter` | 特定のポートフォリオに紐付けられたアダプターを返します |
| `TryGetOrderAdapter` | 注文が登録されたアダプターを検索します |
| `SetSecurityAdapter` | 銘柄を特定のアダプターに紐付けます |
| `SetPortfolioAdapter` | ポートフォリオを特定のアダプターに紐付けます |

### ルーティング優先順位

システムは、次の優先順位で対象アダプターを判断します:

1. **明示的な指定** -- `message.Adapter` プロパティ経由でメッセージにアダプターが指定されている場合、そのアダプターが使用されます。
2. **銘柄の紐付け** -- `SetSecurityAdapter` 経由で設定されたマッピング。
3. **データ型の紐付け** -- 特定のメッセージ型に登録されたアダプター。
4. **サポート型によるフィルタリング** -- 指定されたメッセージ型をサポートするアダプターが選択されます。

### ルーティングの設定

```cs
var router = connector.Adapter.InnerAdapters;

// ティックデータ受信用に銘柄をアダプターへ紐付け
router.SetSecurityAdapter(
    secId,
    DataType.Ticks,
    binanceAdapter
);

// トランザクション用にポートフォリオをアダプターへ紐付け
router.SetPortfolioAdapter(
    "MyPortfolio",
    interactiveBrokersAdapter
);
```

## 接続の管理

### 接続状態

バスケット内の各アダプターは、標準の接続状態を経由します:

- **Disconnected** -- 切断済み
- **Connecting** -- 接続中
- **Connected** -- 接続済み
- **Disconnecting** -- 切断中

バスケットは、すべてのネストされたアダプターの状態を集約します。

### 集約パラメーター

`ConnectDisconnectEventOnFirstAdapter` プロパティは、バスケットがいつ接続済みとみなされるかを決定します:

- `true` -- **最初の**アダプターが接続した時点で接続イベントが発生します (既定)。すべての接続を待たずに作業を開始できます。
- `false` -- **すべての**アダプターが接続した後にのみイベントが発生します。

```cs
// すべてのアダプターが接続するまで待機
connector.Adapter.InnerAdapters.ConnectDisconnectEventOnFirstAdapter = false;

connector.Connected += () =>
{
    Console.WriteLine("すべてのアダプターが接続されました");
};

connector.Connect();
```

## 親サブスクリプションと子サブスクリプション

複数のアダプターを扱う場合、1 つのサブスクリプションを複数の子サブスクリプションに分割し、それぞれを個別のアダプターへ送ることができます。システムは自動的に以下を行います:

- 適切な各アダプター用に子サブスクリプションを作成する
- 親サブスクリプションへ通知する前に応答を集約する
- 部分的なエラーを処理する (1 つのアダプターがサブスクライブに失敗しても、他は動作を継続します)

### 複数取引所の例

```cs
// アダプターを追加
connector.Adapter.InnerAdapters.Add(binanceAdapter);
connector.Adapter.InnerAdapters.Add(bybitAdapter);

connector.Connect();

// ティックをサブスクライブ -- 指定された銘柄をサポートする
// すべてのアダプターへ自動的にルーティングされます
var subscription = new Subscription(DataType.Ticks, security);
connector.Subscribe(subscription);
```

## 保留メッセージキュー

メッセージ送信時に接続済みのアダプターがない場合、メッセージは保留キュー (`IPendingMessageState`) に配置されます。アダプターが接続すると、蓄積されたすべてのメッセージが自動的に送信されます。

```cs
// 接続前に注文を登録 -- 接続が確立された後、
// 注文は自動的に送信されます
connector.RegisterOrder(order);
connector.Connect();
```

## 複数接続の設定

### プログラムによる設定

```cs
// アダプターを作成
var binance = new BinanceMessageAdapter(connector.TransactionIdGenerator)
{
    Key = "<API_KEY>",
    Secret = "<API_SECRET>".Secure(),
};

var ib = new InteractiveBrokersMessageAdapter(connector.TransactionIdGenerator)
{
    Address = InteractiveBrokersMessageAdapter.DefaultAddress,
};

// バスケットに追加
connector.Adapter.InnerAdapters.Add(binance);
connector.Adapter.InnerAdapters.Add(ib);

// ルーティングを設定
connector.Adapter.InnerAdapters.SetPortfolioAdapter("BinancePortfolio", binance);
connector.Adapter.InnerAdapters.SetPortfolioAdapter("IBPortfolio", ib);

connector.Connect();
```

### グラフィカル設定

視覚的に接続を設定するには、グラフィカル設定コンポーネントを使用します。詳細は [グラフィカル設定](connectors/graphical_configuration.md) セクションを参照してください。

## 注文追跡

ルーターは、各注文の登録に使用されたアダプターを自動的に追跡します。注文更新 (状態変更、約定) を受信すると、システムは同じアダプター経由でそれらをルーティングします:

```cs
// 注文はポートフォリオに紐付けられたアダプター経由で登録されます
var order = new Order
{
    Security = security,
    Portfolio = portfolio,
    Side = Sides.Buy,
    Price = price,
    Volume = volume,
};

connector.RegisterOrder(order);

// キャンセルも自動的に同じアダプター経由で行われます
connector.CancelOrder(order);
```

## 関連項目

- [コネクタ](connectors.md)
- [グラフィカル設定](connectors/graphical_configuration.md)
- [ポジション管理](positions.md)
