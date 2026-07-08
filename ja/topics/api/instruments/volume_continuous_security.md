# 出来高ベースの連続先物 (VolumeContinuousSecurity)

## 概要

`VolumeContinuousSecurity` クラスは、契約間の移行（ロールオーバー）が取引出来高または建玉に基づいて発生する連続先物契約を表します。これは、事前に定義された有効期限に従って切り替えが行われる `ExpirationContinuousSecurity` とは異なります。

どちらのクラスも `ContinuousSecurity` を継承しており、`ContinuousSecurity` はさらに `BasketSecurity` を継承します。

## ExpirationContinuousSecurity との違い

| 特性 | ExpirationContinuousSecurity | VolumeContinuousSecurity |
|---|---|---|
| ロールオーバー条件 | 有効期限（固定） | 出来高または建玉のしきい値 |
| 構成 | `SecurityId -> DateTime` ディクショナリ | `SecurityId` リスト + `VolumeLevel` |
| 予測可能性 | スケジュールによる切り替え | 市場条件による切り替え |
| バスケットコード | `CE` | `CV` |

`ExpirationContinuousSecurity` では、各契約の移行日を手動で指定する必要があります。`VolumeContinuousSecurity` は、次の契約の取引出来高（または建玉）が指定されたしきい値を超えたときに、自動的に次の契約へ切り替えます。

## 主なプロパティ

```csharp
public class VolumeContinuousSecurity : ContinuousSecurity
{
    // ロールオーバー順に並んだ内部金融商品（契約）のリスト
    public SynchronizedList<SecurityId> InnerSecurities { get; }

    // ロールオーバー判定に出来高ではなく建玉を使用します
    public bool IsOpenInterest { get; set; }

    // 次の契約へ切り替える出来高しきい値
    public Unit VolumeLevel { get; set; }
}
```

`VolumeLevel` プロパティは `Unit` 型であり、絶対値とパーセンテージ値の両方を指定できます。

## 使用例

```csharp
using StockSharp.Algo;
using StockSharp.Messages;

// 出来高ベースの連続先物を作成します
var continuous = new VolumeContinuousSecurity
{
    Id = "ES-CONT@CME",
    Board = ExchangeBoard.Cme,
};

// ロールオーバー順に契約を追加します
continuous.InnerSecurities.AddRange(new[]
{
    "ES-3.26@CME".ToSecurityId(),
    "ES-6.26@CME".ToSecurityId(),
    "ES-9.26@CME".ToSecurityId(),
});

// 切り替え用の出来高しきい値を設定します
continuous.VolumeLevel = new Unit(10000);

// または建玉を使用します
continuous.IsOpenInterest = true;
continuous.VolumeLevel = new Unit(50000);
```

## 比較用の ExpirationContinuousSecurity の例

```csharp
using StockSharp.Algo;
using StockSharp.Messages;

// 有効期限ベースの連続先物
var expContinuous = new ExpirationContinuousSecurity
{
    Id = "ES-CONT-EXP@CME",
    Board = ExchangeBoard.Cme,
};

// 各契約の正確な移行日を指定します
expContinuous.ExpirationJumps.Add(
    "ES-3.26@CME".ToSecurityId(),
    new DateTime(2026, 3, 15)
);
expContinuous.ExpirationJumps.Add(
    "ES-6.26@CME".ToSecurityId(),
    new DateTime(2026, 6, 15)
);
```

## 使用する場面

`VolumeContinuousSecurity` は、次のような状況に適しています。

- 正確なロールオーバー日が事前に分からない
- 流動性（取引出来高または建玉）に基づく切り替えが必要
- 市場条件に応答する、より適応的な移行が必要

`ExpirationContinuousSecurity` は、有効期限が事前に分かっており、決定論的なロールオーバーが必要な場合に適しています。
