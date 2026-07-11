# SuperTrend

**SuperTrend インジケーター** は、平均真の値幅 (ATR) に基づくトレンドフォロー型インジケーターです。現在のトレンド方向と、反転の可能性があるポイントを識別するのに役立ちます。

このインジケーターを使用するには、[SuperTrend](xref:StockSharp.Algo.Indicators.SuperTrend) クラスを使用する必要があります。

## 説明

SuperTrend は、平均価格と ATR 値を使用して構築されます。インジケーターラインは、トレンドが変化すると、価格の上側から下側へ、またはその逆へ切り替わります。このように、SuperTrend は価格がインジケーターラインを交差するまで、現在のトレンドを視覚的に強調します。

## パラメーター

- **ATR 期間** - ATR 計算に使用される期間。
- **Multiplier** - ラインを平均価格からどれだけ離して配置するかを定義する係数。

## 計算

1. 選択した期間で ATR を計算します。
2. 2 つの境界を計算します。
   ```
   UpperBand = (High + Low) / 2 + Multiplier * ATR
   LowerBand = (High + Low) / 2 - Multiplier * ATR
   ```
3. SuperTrend は、現在のトレンドに応じて、最初はいずれかのバンドと等しくなります。
4. 終値が SuperTrend ラインを交差すると、トレンド方向が変化し、ラインは反対側へ移動します。

![IndicatorSuperTrend](../../../../images/indicator_supertrend.png)

## 関連項目

[ATR](atr.md)
[パラボリック SAR](parabolic_sar.md)
