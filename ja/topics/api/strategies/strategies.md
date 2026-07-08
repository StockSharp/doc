# 戦略

## 概要

`Strategy` クラスは、StockSharp で取引戦略を作成するための基本クラスです。市場データの購読、注文とポジションの管理、統計の計算、レポート生成のための一式のツールを提供します。

`Strategy` クラスの主な機能:

- ローソク足、板情報、ティック、その他の市場データへの購読
- 注文の発注、変更、キャンセル
- 目標ポジション管理
- PnL、手数料、統計の計算
- リスク管理
- タイマーおよびルールシステム
- アラート
- レポート生成

> [!WARNING]
> 子戦略機能 (`ChildStrategies`) は廃止済みと宣言され、現在はサポートされていません。`ChildStrategies` プロパティには `[Obsolete("Child strategies no longer supported.")]` 属性が付与されています。コードで子戦略を使用している場合は、リファクタリングして、各戦略を独立したインスタンスとして実行することを推奨します。

## ドキュメントセクション

- [目標ポジション管理](target_position_management.md) -- `SetTargetPosition` による宣言的なポジションサイズ管理
- [取引モード](trading_modes.md) -- `StrategyTradingModes` による取引アクティビティの制限
- [アラートシステム](alert_system.md) -- 通知の送信 (ポップアップ、サウンド、ログ、Telegram)
- [タイマーシステム](timer_system.md) -- 定期的なアクション実行
- [リスク管理](risk_management.md) -- リスク管理ルール
- [高レベルサブスクリプション](high_level_subscriptions.md) -- 簡略化された市場データ購読
- [戦略レポート](reporting.md) -- 取引結果レポートの生成
- [高度な機能](advanced_features.md) -- 注文コメント、スケジュール、リスクフリーレート、インジケーターソース

## 最小構成の戦略

```csharp
public class MyStrategy : Strategy
{
    private readonly StrategyParam<DataType> _candleType;

    public DataType CandleType
    {
        get => _candleType.Value;
        set => _candleType.Value = value;
    }

    public MyStrategy()
    {
        _candleType = Param(nameof(CandleType), TimeSpan.FromMinutes(5).TimeFrame());
    }

    protected override void OnStarted2(DateTime time)
    {
        base.OnStarted2(time);

        var subscription = SubscribeCandles(CandleType);

        subscription
            .Bind(ProcessCandle)
            .Start();
    }

    private void ProcessCandle(ICandleMessage candle)
    {
        if (!IsFormedAndOnlineAndAllowTrading())
            return;

        // 取引ロジック
    }
}
```

## 戦略のライフサイクル

1. **作成** -- コンストラクター、`Param<T>` によるパラメーター宣言。
2. **設定** -- `Security`、`Portfolio`、`Connector`、およびパラメーターの設定。
3. **開始** -- `Start()` の呼び出し、`ProcessStates.Started` 状態への遷移、`OnStarted2(DateTime)` の呼び出し。
4. **実行中** -- 市場データの処理、注文の発注。
5. **停止** -- `Stop()` の呼び出し、`ProcessStates.Stopping` から `ProcessStates.Stopped` への遷移、`OnStopped()` の呼び出し。

