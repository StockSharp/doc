# Bear Power

**Bear Power** は Alexander Elder の Elder-ray システムの一部であり、指数移動平均 (EMA) と比較して売り手がどれほど強いかを示します。日中の安値が平均価格をどれだけ下回っているかを測定し、弱気側が
コントロールを失う局面を明らかにします。

インジケーターにアクセスするには、[BearPower](xref:StockSharp.Algo.Indicators.BearPower) クラスを使用します。

## 説明

このインジケーターは、バーの安値と EMA 値の差として計算されます。

`Bear Power = Low - EMA`。

- 負の値は売り圧力を確認します。
- ゼロ方向またはゼロを上回る方向へ値が上昇する場合、弱気側の弱まりと強気反転の可能性を示します。
- 深い谷は、特にパニック的な売りの局面で、反発に先行することがよくあります。

## パラメーター

Bear Power は [ExponentialMovingAverage](xref:StockSharp.Algo.Indicators.ExponentialMovingAverage) の設定を継承します。

- **Length** - EMA 期間。
- **Alpha**（任意） - EMA がこの方法で設定されている場合の平滑化係数。

## 使用方法

- Bear Power が極端な安値の後に上向きへ転じ、EMA も上昇し始める場合、反転を探します。
- ゼロラインのクロスは、支配的なトレンドの変化を確認する場合があります。
- Bear Power を [Bull Power](bull_power.md) および価格 EMA と組み合わせて、完全な [Elder Ray](elder_ray.md) インジケーターを構築します。

![indicator_bear_power](../../../../images/indicator_bear_power.png)

## 関連項目

[Bull Power](bull_power.md)
[Elder Ray](elder_ray.md)
[指数移動平均](ema.md)
