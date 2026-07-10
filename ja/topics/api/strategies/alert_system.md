# アラートシステム

## 概要

StockSharp のストラテジーには組み込みのアラートシステムがあり、ポップアップウィンドウ、サウンドシグナル、ログエントリ、Telegram メッセージなど、さまざまな種類の通知を送信できます。アラートは、ポジションのエントリー、レベルの突破、エラー、その他の取引シグナルなど、重要なイベントをトレーダーに知らせるために役立ちます。

バックテスト中は、テストの妨げを避けるため、`Log` 型以外のアラートは自動的にスキップされます。

## アラートの種類

`AlertNotifications` 列挙体は、利用可能な種類を定義します。

| 種類 | 説明 |
|------|-------------|
| `Sound` | サウンドシグナル |
| `Popup` | ポップアップウィンドウ |
| `Log` | ログファイルエントリ |
| `Telegram` | Telegram メッセージ |

## メソッド

### Alert

指定した種類、キャプション、メッセージでアラートを送信する基本メソッドです。

```csharp
// キャプションとメッセージを指定
Alert(AlertNotifications type, string caption, string message);

// 自動キャプションを使用（ストラテジー名を使用）
Alert(AlertNotifications type, string message);
```

### AlertPopup

ポップアップ通知を送信します。キャプションはストラテジー名になります。

```csharp
AlertPopup(string message);
```

### AlertSound

サウンド通知を送信します。

```csharp
AlertSound(string message);
```

### AlertLog

ログに通知を送信します。この種類はバックテスト中も機能します。

```csharp
AlertLog(string message);
```

## アラートサービスの設定

アラートを機能させるには、ストラテジーの環境に `IAlertNotificationService` サービスが登録されている必要があります。これは拡張メソッドを介して行います。

```csharp
strategy.SetAlertService(alertService);
```

現在のサービスは次のように取得できます。

```csharp
var service = strategy.GetAlertService();
```

グラフィカルアプリケーション（Designer、terminal）では、通常このサービスは自動的に登録されます。

## 使用例

```csharp
public class AlertStrategy : Strategy
{
    private readonly StrategyParam<DataType> _candleType;
    private readonly StrategyParam<decimal> _priceLevel;

    public DataType CandleType
    {
        get => _candleType.Value;
        set => _candleType.Value = value;
    }

    public decimal PriceLevel
    {
        get => _priceLevel.Value;
        set => _priceLevel.Value = value;
    }

    public AlertStrategy()
    {
        _candleType = Param(nameof(CandleType), TimeSpan.FromMinutes(5).TimeFrame());
        _priceLevel = Param(nameof(PriceLevel), 100m);
    }

    protected override void OnStarted2(DateTime time)
    {
        base.OnStarted2(time);

        var subscription = SubscribeCandles(CandleType);

        subscription
            .Bind(ProcessCandle)
            .Start();

        // ストラテジー開始に関するアラート
        AlertLog("Strategy started, tracked level: " + PriceLevel);
    }

    private void ProcessCandle(ICandleMessage candle)
    {
        if (!IsFormedAndOnlineAndAllowTrading())
            return;

        // 価格が下から上へレベルを交差
        if (candle.OpenPrice < PriceLevel && candle.ClosePrice >= PriceLevel)
        {
            AlertPopup("価格がレベルを交差しました " + PriceLevel + " upward!");
            AlertSound("Level breakout!");
            BuyMarket();
        }

        // 価格が上から下へレベルを交差
        if (candle.OpenPrice > PriceLevel && candle.ClosePrice <= PriceLevel)
        {
            Alert(AlertNotifications.Telegram, "Trading signal",
                "価格がレベルを突破しました " + PriceLevel + " downward");
            SellMarket();
        }
    }
}
```

この例では、ストラテジーは状況に応じて異なるアラート種類を使用しています。`AlertPopup` と `AlertSound` はトレーダーの注意を即座に引くために使用し、`Telegram` 型を指定した `Alert` はリモート通知に使用します。
