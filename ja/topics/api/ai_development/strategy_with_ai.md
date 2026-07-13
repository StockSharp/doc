# AI で戦略を書く

AI ツールを使用して StockSharp のトレーディング戦略を作成するためのステップバイステップガイドです。

## 準備

### 1. AI ツールをインストールする

利用可能なツールから 1 つを選択します:
- **Claude Code** — `npm install -g @anthropic-ai/claude-code` (Node.js が必要)
- **Cursor** — [cursor.com](https://cursor.com) からダウンロード
- **GitHub Copilot** — IDE 用プラグインをインストール

### 2. プロジェクトを作成する

```bash
dotnet new console -n MyStrategy --framework net10.0
cd MyStrategy
dotnet add package StockSharp.Algo
dotnet add package StockSharp.Algo.Strategies
dotnet add package StockSharp.Algo.Indicators
dotnet add package StockSharp.Algo.Testing
dotnet add package StockSharp.Binance
```

### 3. コンテキストを準備する

プロジェクトルートに `CLAUDE.md` (または `.cursorrules`) ファイルを作成します:

```markdown
# プロジェクトルール

- フレームワーク: StockSharp 5.x, .NET 10
- 戦略は Strategy クラスを継承する
- ローソク足は Connector.Subscribe(subscription) 経由でサブスクライブする
- 注文は RegisterOrder(order) 経由で登録する
- ログ出力: this.AddInfoLog(), this.AddWarningLog(), this.AddErrorLog()
- インジケーター: new で作成し、indicator.Process(candle) を呼び出す
- connector.Error と戦略エラーを必ず処理する
```

## ステップバイステップの例: SMA 戦略

### ステップ 1: AI にタスクを説明する

プロンプト例:

```
StockSharp を使用して、次の条件を満たすトレーディング戦略を作成してください:
- Strategy を継承する
- 2 本の単純移動平均 (SMA) を使用する: 短期 (期間 10) と長期 (期間 30)
- 短期 SMA が長期 SMA を上抜けたら — 買い
- 短期 SMA が長期 SMA を下抜けたら — 売り
- ポジションサイズ: 1 lot
- 5 分足ローソク足を使用する
- OnStarted() でローソク足をサブスクライブする
- サブスクリプションルール経由でローソク足を処理する
```

### ステップ 2: 生成されたコードをレビューする

AI は次のようなコードを生成します:

```csharp
using StockSharp.Algo;
using StockSharp.Algo.Indicators;
using StockSharp.Algo.Strategies;
using StockSharp.BusinessEntities;
using StockSharp.Messages;

public class SmaCrossStrategy : Strategy
{
    private readonly StrategyParam<int> _fastPeriod;
    private readonly StrategyParam<int> _slowPeriod;
    private readonly StrategyParam<DataType> _candleType;

    private SimpleMovingAverage _fastSma;
    private SimpleMovingAverage _slowSma;

    private bool _prevFastAbove;
    private bool _isFirstCandle = true;

    public SmaCrossStrategy()
    {
        _fastPeriod = Param(nameof(FastPeriod), 10);
        _slowPeriod = Param(nameof(SlowPeriod), 30);
        _candleType = Param(nameof(CandleType), DataType.TimeFrame(TimeSpan.FromMinutes(5)));
    }

    public int FastPeriod
    {
        get => _fastPeriod.Value;
        set => _fastPeriod.Value = value;
    }

    public int SlowPeriod
    {
        get => _slowPeriod.Value;
        set => _slowPeriod.Value = value;
    }

    public DataType CandleType
    {
        get => _candleType.Value;
        set => _candleType.Value = value;
    }

    protected override void OnStarted(DateTimeOffset time)
    {
        base.OnStarted(time);

        _fastSma = new SimpleMovingAverage { Length = FastPeriod };
        _slowSma = new SimpleMovingAverage { Length = SlowPeriod };
        _isFirstCandle = true;

        var subscription = new Subscription(CandleType, Security);

        this
            .WhenCandlesFinished(subscription)
            .Do(ProcessCandle)
            .Apply(this);

        Subscribe(subscription);
    }

    private void ProcessCandle(ICandleMessage candle)
    {
        var fastValue = _fastSma.Process(candle);
        var slowValue = _slowSma.Process(candle);

        if (!_fastSma.IsFormed || !_slowSma.IsFormed)
            return;

        var fastAbove = fastValue.GetValue<decimal>() > slowValue.GetValue<decimal>();

        if (!_isFirstCandle)
        {
            if (fastAbove && !_prevFastAbove)
            {
                // 高速 SMA が低速 SMA を上抜け — 買い
                if (Position <= 0)
                    RegisterOrder(this.BuyAtMarket(Volume));
            }
            else if (!fastAbove && _prevFastAbove)
            {
                // 高速 SMA が低速 SMA を下抜け — 売り
                if (Position >= 0)
                    RegisterOrder(this.SellAtMarket(Volume));
            }
        }

        _prevFastAbove = fastAbove;
        _isFirstCandle = false;
    }
}
```

### ステップ 3: 確認すべきこと

このチェックリストを確認してください:

- **継承**: クラスが `Strategy` から継承している ✓
- **パラメーター**: 最適化のために `StrategyParam<T>` を使用している ✓
- **ローソク足のサブスクリプション**: `Subscribe(new Subscription(...))` 経由 ✓
- **ローソク足処理**: `WhenCandlesFinished` ルール経由 ✓
- **IsFormed チェック**: インジケーターの準備完了を確認している ✓
- **注文**: `BuyAtMarket` / `SellAtMarket` とともに `RegisterOrder()` 経由 ✓
- **ポジション**: 注文を出す前に `Position` を確認している ✓

### ステップ 4: AI にバックテストの追加を依頼する

```
過去データを使用して、この戦略のバックテストコードを追加してください。
HistoryEmulationConnector を使用し、ローカルストレージからデータを読み込み、
サマリー統計 (PnL、取引数、最大ドローダウン) を出力してください。
```

## プロンプト例

### ボリンジャーバンド戦略

```
ボリンジャーバンドを使用して取引する StockSharp 戦略を作成してください:
- 価格が下側バンドに触れたら買い
- 価格が上側バンドに触れたら売り
- 期間 20、乗数 2.0
- ストップロス: エントリー価格から 1%
- テイクプロフィット: エントリー価格から 2%
- すべてのパラメーターに StrategyParam を使用する
```

### 裁定取引戦略

```
StockSharp でペア裁定取引戦略を作成してください:
- 2 つの銘柄 (パラメーターで指定)
- 価格間のスプレッドを計算する
- スプレッドが 2 標準偏差乖離したらエントリーする
- スプレッドが平均へ戻ったら手仕舞う
- 出来高の中立化 (金額ベースで同等のポジション)
```

### 板スキャルピング

```
StockSharp でスキャルピング戦略を作成してください:
- Subscribe 経由で板情報 (MarketDepth) をサブスクライブする
- bid/ask の不均衡を分析する
- 強い不均衡 (> 3:1) でエントリーする
- テイクプロフィット (5 ティック) で素早く手仕舞う
- ストップロス: 3 ティック
- 同時に最大 1 ポジション
```

## AI がよくする間違い

### 1. 古いイベント

**誤り** (古い API):
```csharp
connector.NewSecurities += securities => { ... };
connector.CandleSeriesProcessing += (series, candle) => { ... };
```

**正しい例** (現在の API):
```csharp
// サブスクリプションを使用
var subscription = new Subscription(DataType.TimeFrame(TimeSpan.FromMinutes(5)), security);
connector.Subscribe(subscription);
```

### 2. ヘルパーを使わずに注文を作成する

**誤り**:
```csharp
var order = new Order
{
    Security = Security,
    Portfolio = Portfolio,
    Side = Sides.Buy,
    Type = OrderTypes.Market,
    Volume = 1,
};
```

**正しい例** (戦略ヘルパーを使用):
```csharp
RegisterOrder(this.BuyAtMarket(Volume));
// または
RegisterOrder(this.SellAtLimit(price, Volume));
```

### 3. IsFormed チェックの欠落

**誤り**:
```csharp
var value = _sma.Process(candle);
// すぐに value を使用 — まだ準備できていない可能性があります
```

**正しい例**:
```csharp
var value = _sma.Process(candle);
if (!_sma.IsFormed)
    return;
```

## ヒント

1. **AI にドキュメントを与える** — [doc.stocksharp.com](https://doc.stocksharp.com) を示すか、`Samples/` からコード例をコピーします
2. **CLAUDE.md を使用する** — プロジェクトルールファイルにより、間違いの数を大幅に減らせます
3. **シンプルに始める** — まず基本的な戦略を作成し、その後フィルターとリスク管理を追加します
4. **履歴でテストする** — ライブ取引の前に必ずバックテストを実行します
5. **リポジトリをクローンする** — AI が StockSharp ソースにアクセスできれば、API をより正確に使用できます
